# План за миграция: Заместване на директните връзки с API

## Обзор

Това API (`Nursan.API`) трябва да замести всички директни връзки към базата данни от `Nursan.Validations` и да се използва от `Nursan.UI` вместо директните зависимости.

## Текущо състояние

### 1. Проблеми с директните връзки

#### A. `TarihHesapla.GetSystemDate()`
- **Местоположение:** `Nursan.Validations/ValidationCode/TarihHesapla.cs`
- **Проблем:** Използва `SqlConnection` директно с hardcoded connection string
- **Решение:** Създай API endpoint `/api/System/Date` който връща системна дата

#### B. `Nursan.UI` директни зависимости
- **Местоположение:** Множество файлове (`Gromet.cs`, `SicilOkuma.cs`, `DonanimTanitma.cs`, и др.)
- **Проблем:** Създават нови `DbContext` и `UnitOfWork` директно
- **Решение:** Създай API Client Service Layer който комуникира с `Nursan.API`

## План за миграция

### Фаза 1: Подобряване на API

#### 1.1 Добави липсващи endpoints

**SystemController.cs** (нов)
```csharp
[ApiController]
[Route("api/[controller]")]
public class SystemController : ControllerBase
{
    private readonly UretimOtomasyonContext _context;
    
    [HttpGet("date")]
    public async Task<ActionResult<DateTime>> GetSystemDate()
    {
        // Използва EF Core вместо SqlConnection
        var date = await _context.Database.SqlQueryRaw<DateTime>("SELECT GETDATE()")
            .FirstOrDefaultAsync();
        return Ok(date);
    }
}
```

**TarihController.cs** (нов)
```csharp
[ApiController]
[Route("api/[controller]")]
public class TarihController : ControllerBase
{
    private readonly UretimOtomasyonContext _context;
    
    [HttpGet("calculate")]
    public async Task<ActionResult<TarihHIMDto>> CalculateTarih()
    {
        // Логика от TarihHesapla.TarihHesab()
        // Връща TarihHIMDto
    }
}
```

#### 1.2 Рефакториране на Handlers

**Проблем:** Handlers създават нови инстанции на `TorkService` с нови `UnitOfWork`
**Решение:** Използвай dependency injection за `TorkService` или премести логиката в handlers

#### 1.3 Добави Station Bootstrap endpoint

**Вече има:** `StationBootstrapController.GetBootstrap()` ✅
- Това е добър пример за това как трябва да работи API-то

### Фаза 2: Създаване на API Client Layer

#### 2.1 API Client Service

**Nursan.UI/Services/ApiClientService.cs** (нов)
```csharp
public class ApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _apiKey;
    
    public ApiClientService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _baseUrl = XMLSeverIp.XmlWebApiAddress(); // от Baglanti.xml
        _apiKey = XMLSeverIp.XmlApiKey(); // от Baglanti.xml
        
        _httpClient.BaseAddress = new Uri(_baseUrl);
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
    }
    
    // System methods
    public async Task<DateTime> GetSystemDateAsync()
    {
        var response = await _httpClient.GetAsync("/api/System/date");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DateTime>();
    }
    
    // Tork methods
    public async Task<TorkResultDto> ProcessTorkBarcodeAsync(BarcodeRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/Tork/process-barcode", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TorkResultDto>();
    }
    
    // Station bootstrap
    public async Task<StationBootstrapDto> GetStationBootstrapAsync(string machineName)
    {
        var response = await _httpClient.GetAsync($"/api/station/bootstrap?machineName={machineName}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StationBootstrapDto>();
    }
}
```

#### 2.2 Dependency Injection в UI

**Program.cs** (рефакториране)
```csharp
// Регистрация на HttpClient и ApiClientService
services.AddHttpClient<ApiClientService>(client =>
{
    var baseUrl = XMLSeverIp.XmlWebApiAddress();
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("X-API-Key", XMLSeverIp.XmlApiKey());
});

services.AddScoped<ApiClientService>();

// ПРЕМАХНИ регистрацията на DbContext от UI
// services.AddDbContext<UretimOtomasyonContext>(); // ❌ ПРЕМАХНИ
```

### Фаза 3: Миграция на UI код

#### 3.1 Рефакториране на Gromet.cs

**Преди:**
```csharp
private readonly UnitOfWork _repo;
personal = new PersonalValidasyonu(new UnitOfWorPersonal(new PersonalContext()), _repo);
tork = new TorkService(repo, vardiya);
```

**След:**
```csharp
private readonly ApiClientService _apiClient;

public Gromet(ApiClientService apiClient)
{
    _apiClient = apiClient;
    // Зареждане на station bootstrap от API
    var bootstrap = await _apiClient.GetStationBootstrapAsync(Environment.MachineName);
}
```

#### 3.2 Рефакториране на TarihHesapla използвания

**Преди:**
```csharp
DateTime date = TarihHesaplama.GetSystemDate(); // Директно SQL
```

**След:**
```csharp
DateTime date = await _apiClient.GetSystemDateAsync(); // Чрез API
```

#### 3.3 Рефакториране на TorkService използвания

**Преди:**
```csharp
tork = new TorkService(new UnitOfWork(new UretimOtomasyonContext()), vardiya);
var result = tork.GetTorkDonanimBarcode(barcodes);
```

**След:**
```csharp
var request = new BarcodeRequestDto 
{ 
    Barcodes = barcodes.Select(b => new BarcodeInputDto { ... }),
    VardiyaName = vardiya.Name 
};
var result = await _apiClient.ProcessTorkBarcodeAsync(request);
```

### Фаза 4: Премахване на директните зависимости

#### 4.1 Рефакториране на TarihHesapla

**Вариант A: Премести в API**
- Премахни `TarihHesapla` от `Nursan.Validations`
- Използвай API endpoint вместо статичен метод

**Вариант B: Остави като helper (за обратна съвместимост)**
- Остави `TarihHesapla` но го направи да използва API вътрешно
- Постепенна миграция

#### 4.2 Премахни директни DbContext създавания

**Файлове за рефакториране:**
- `Gromet.cs` - Използва `ApiClientService` вместо `UnitOfWork`
- `SicilOkuma.cs` - Използва API за Personal операции
- `DonanimTanitma.cs` - Използва API за Donanim операции
- `BarcodeConfig.cs` - Използва API за Barcode операции
- `ScreenSaverForm.cs` - Използва API за TorkService
- И всички останали форми

## Приоритети за подобрение

### Висок приоритет ⚠️

1. **TarihHesapla.GetSystemDate()** - Критична точка за security
   - Hardcoded connection string с парола
   - Директно SQL connection
   - **Действие:** Създай `/api/System/date` endpoint веднага

2. **Директни DbContext в UI** - Архитектурен проблем
   - Множество места създават нови контексти
   - Няма connection pooling
   - **Действие:** Създай `ApiClientService` и започни миграция

### Среден приоритет 📋

3. **Handlers в API** - Оптимизация
   - Handlers създават нови `UnitOfWork` всяка заявка
   - Възможност за подобрение чрез DI
   - **Действие:** Рефакторирай handlers да използват DI

4. **Station Bootstrap** - Вече е готово ✅
   - Добър пример за API архитектура
   - **Действие:** Използвай като шаблон за други endpoints

### Нисък приоритет 📝

5. **Документация** - Дългосрочно
   - Swagger е наличен ✅
   - Добави примери за използване
   - **Действие:** Подобри Swagger документацията

## Препоръки за сигурност

### 1. API Key Management
- ✅ Вече има `ApiKeyMiddleware` 
- ✅ Поддържа и JWT и API Key
- **Подобрение:** Добави rate limiting

### 2. Connection Strings
- ❌ Hardcoded в `TarihHesapla`
- ✅ В `appsettings.json` за API
- **Подобрение:** Премахни hardcoded connection strings

### 3. Error Handling
- ✅ Има exception handlers
- **Подобрение:** Добави structured logging

## Метрики за успех

### Фаза 1 (1-2 седмици)
- [ ] Създаден `SystemController` с `/api/System/date`
- [ ] Създаден `TarihController` с `/api/Tarih/calculate`
- [ ] Всички handlers рефакторирани да използват DI

### Фаза 2 (2-3 седмици)
- [ ] Създаден `ApiClientService` в `Nursan.UI`
- [ ] Регистриран в DI контейнер
- [ ] Тестван с базови операции

### Фаза 3 (3-4 седмици)
- [ ] `Gromet.cs` мигриран към API
- [ ] `SicilOkuma.cs` мигриран към API
- [ ] Поне 50% от формите мигрирани

### Фаза 4 (4-6 седмици)
- [ ] `TarihHesapla` премахнат или рефакториран
- [ ] Всички директни `DbContext` премахнати от UI
- [ ] Пълна миграция към API

## Тестване

### Unit Tests
- Тествай API endpoints изолирано
- Mock `DbContext` в handlers

### Integration Tests
- Тествай API + Database заедно
- Тествай `ApiClientService` с тестов API server

### Manual Testing
- Тествай всяка форма след миграция
- Проверявай performance impact

## Рискове и мерки

### Риск 1: Breaking Changes
- **Мерка:** Поддържай обратна съвместимост временно
- Версия на API endpoints (`/api/v1/...`)

### Риск 2: Performance
- **Мерка:** Мониторирай response times
- Добави caching където е възможно

### Риск 3: Network Issues
- **Мерка:** Добави retry logic в `ApiClientService`
- Fallback механизми при недостъпност

## Заключение

API-то (`Nursan.API`) е добре структурирано и следва best practices. Основният проблем е, че `Nursan.UI` не го използва, а използва директно `Nursan.Validations` с директни връзки към базата.

**Следващи стъпки:**
1. Създай `SystemController` за системни операции
2. Създай `ApiClientService` в UI
3. Започни постепенна миграция на формите
4. Премахни директните зависимости