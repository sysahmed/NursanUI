# Ръководство за модификация на ElTest към ElTest-vApi

## Ключови промени, които трябва да се направят:

### 1. **Промяна на using statements (в началото на файла)**

**Премахни:**
```csharp
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;
```

**Добави:**
```csharp
using Nursan.UI.Services;
```

### 2. **Промяна на полетата в класа**

**Премахни:**
```csharp
private TorkService TorkService;
private EltestValidasyonlari _elTest;
private UnitOfWork _repo;
```

**Добави:**
```csharp
private ElTestApiService _elTestApi;
```

### 3. **Промяна на конструктора**

**ПРЕДИ:**
```csharp
public ElTest(UnitOfWork repo)
{
    // ...
    _elTest = new EltestValidasyonlari(repo);
    _repo = repo;
    // ...
}
```

**СЛЕД:**
```csharp
public ElTestApi()  // Променено име
{
    // ...
    _elTestApi = new ElTestApiService();
    // ...
}
```

### 4. **Промяна в метода `Watcher1()` (ред ~272)**

**ПРЕДИ:**
```csharp
var result = _elTest.GitSystemeYukle($"{fileInfo.Name.ToUpper()}");
```

**СЛЕД:**
```csharp
string[] getParca = fileInfo.Name.ToUpper().Split('_');
string vardiyaName = getParca.Length > 2 ? getParca[2] : "";
var result = await _elTestApi.GitSystemeYukle(getParca, vardiyaName);
```

**ВАЖНО:** Методът трябва да стане `async Task`:
```csharp
private async Task Watcher1(string Path, string Format)
```

### 5. **Промяна в метода `GitSystemeDesktopAc()` (ред ~423)**

**ПРЕДИ:**
```csharp
TorkService = new TorkService(_repo, new UrVardiya() { Name = gelenDegerler.Name });
var idBak = TorkService.GitSytemeSayiElTestBack(new SyBarcodeInput() { BarcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}" });
```

**СЛЕД:**
```csharp
string barcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}";
var idBak = await _elTestApi.CheckSystemElTest(barcodeIcerik, gelenDegerler.Name);
```

**ВАЖНО:** Методът трябва да стане `async Task`:
```csharp
private async Task GitSystemeDesktopAc(string name)
```

### 6. **Промяна в метода `GitSystemeDesktopKapa()` (ред ~486)**

**ПРЕДИ:**
```csharp
TorkService = new TorkService(_repo, new UrVardiya() { Name = gelenDegerler.Name });
var idBak = TorkService.GitSytemeSayiBac(new SyBarcodeInput() { BarcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}" });
```

**СЛЕД:**
```csharp
string barcodeIcerik = $"{gelenDegerler.prefix}-{gelenDegerler.family}-{gelenDegerler.suffix}{gelenDegerler.IdDonanim}";
var idBak = await _elTestApi.CheckSystem(barcodeIcerik, gelenDegerler.Name);
```

**ВАЖНО:** Методът трябва да стане `async Task`:
```csharp
private async Task GitSystemeDesktopKapa(string name)
```

### 7. **Промяна в метода `Watcher3()` (ред ~336)**

**ПРЕДИ:**
```csharp
var result = _elTest.GithataYukle($"{Path}{fileInfo.Name.ToUpper()}");
```

**СЛЕД:**
```csharp
// TODO: Трябва да се създаде endpoint в API за обработка на грешки
// За момента може да се използва същия метод като Watcher1 или да се имплементира отделно
// var result = await _elTestApi.GithataYukle($"{Path}{fileInfo.Name.ToUpper()}");
```

**Забележка:** `GithataYukle` чете файл ред по ред и обработва грешки. Трябва да се създаде специален endpoint в API за това или да се адаптира логиката.

### 8. **Промяна в метода `AddTicket()` (ред ~800)**

**ПРЕДИ:**
```csharp
using (var context = new AmbarContext())
{
    var islemler = new Islemler { ... };
    context.Islemlers.Add(islemler);
    context.SaveChanges();
}
```

**СЛЕД:**
```csharp
// Вече използва SystemTicket.CreateTicket() който работи с API
// Този метод вече е имплементиран в формата и работи с API
// Не е нужна промяна, но може да се опрости
```

### 9. **Промяна в метода `ValidateBeforeTicketSubmission()` (ред ~1039)**

**ПРЕДИ:**
```csharp
TorkService = new TorkService(_repo, new UrVardiya() { Name = gelenDegerler.Name });
var idBak = TorkService.GitSytemeSayiElTestBack(new SyBarcodeInput() { BarcodeIcerik = currentBarcode });
```

**СЛЕД:**
```csharp
var idBak = await _elTestApi.CheckSystemElTest(currentBarcode, gelenDegerler.Name);
```

**ВАЖНО:** Методът трябва да стане `async Task<(bool, string)>`:
```csharp
private async Task<(bool IsValid, string Message)> ValidateBeforeTicketSubmission(TickedRolleNote ticket)
```

### 10. **Промяна на FileSystemWatcher event handlers**

Всички event handlers, които извикват модифицираните методи, трябва да станат `async`:

```csharp
private async void Watcher1_Created(object sender, FileSystemEventArgs e)
{
    // ...
    await Watcher1(veri.Path, veri.Filter);
}

private async void Watcher2_Created(object sender, FileSystemEventArgs e)
{
    // ...
    await GitSystemeDesktopKapa(fileInfo.Name.ToUpper());
}

private async void Watcher3_Created(object sender, FileSystemEventArgs e)
{
    // ...
    // await обработката
}
```

### 11. **Добавяне на Dispose() метод**

**Добави в края на класа:**
```csharp
protected override void Dispose(bool disposing)
{
    if (disposing)
    {
        _elTestApi?.Dispose();
    }
    base.Dispose(disposing);
}
```

## Важни забележки:

1. **Всички асинхронни извиквания** трябва да използват `await`
2. **Event handlers** могат да останат `async void` но трябва да извикват `await`
3. **ElTestApiService** трябва да бъде в `Nursan.UI/Services/` папката
4. **SystemTicket** вече използва API, така че не се нуждае от промяна
5. **File Watchers** остават същите - те работят само с файловата система

## Стъпки за имплементация:

1. Копирай `ElTest.cs` като `ElTest-vApi.cs`
2. Копирай `ElTest.Designer.cs` като `ElTest-vApi.Designer.cs` и промени `partial class ElTest` на `partial class ElTestApi`
3. Направи всички промени според горното ръководство
4. Тествай функционалността

## TODO endpoints в API (ако са нужни):

1. **Endpoint за обработка на грешки** (`GithataYukle`) - четене на файл и обработка
2. **Endpoint за получаване на PC ID** - опционално
