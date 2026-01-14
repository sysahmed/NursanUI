# API Key Setup Guide

## Как работи системата за API Key аутентикация

### 1. **Генериране на API Key**

#### Чрез API endpoint:
```http
POST https://your-api-url/api/auth/generate-api-key
```

Това ще:
- Генерира нов сигурен API Key (256-битов)
- Запише го в `Baglanti.xml` файла
- Върне ключа в отговора (запишете го на безопасно място!)

**Важно:** Ключът се показва само веднъж при генериране!

#### Програмен път:
```csharp
using Nursan.XMLTools;

// Генерира нов API Key
string apiKey = ApiKeyManager.GenerateApiKey();

// Записва в XML
ApiKeyManager.SaveApiKey(apiKey);

// Или едно всичко:
string apiKey = ApiKeyManager.GenerateAndSaveApiKey();
```

### 2. **Структура в XML файла**

API Key се записва в `Baglanti.xml` като:
```xml
<config>
    ...
    <apiKey Value="your-generated-api-key-here" />
    ...
</config>
```

### 3. **Използване в клиент (ElTestApiService)**

Клиентът автоматично:
- Чете API Key от XML при инициализация
- Добавя го като `X-API-Key` header във всички заявки
- Няма нужда от допълнителна настройка!

### 4. **CORS Политика**

#### Development:
- `AllowAll` - разрешава всички origins, методи и headers
- Има кеширане на preflight заявки (10 минути)

#### Production:
- `Production` - по-ограничена политика
- **ВАЖНО:** Трябва да се настрои конкретен origin:
```csharp
policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
```

### 5. **Сигурност**

#### API Key Middleware:
- Проверява `X-API-Key` header при всяка заявка
- Използва безопасно сравнение (защита срещу timing attacks)
- Пропуска само Swagger, health checks и endpoint за генериране на ключ

#### Изключени от аутентикация:
- `/swagger/*` - Swagger документация
- `/health` - Health check endpoints
- `/api/auth/generate-api-key` - Генериране на ключ
- `/` - Root endpoint

### 6. **Проверка на API Key**

```http
POST https://your-api-url/api/auth/validate-api-key
Headers:
  X-API-Key: your-api-key-here
```

### 7. **Конфигурация в appsettings.json**

```json
{
  "ApiSettings": {
    "ApiKey": "" // Ако е празно, се взима от XML
  }
}
```

## Стъпки за инсталация:

1. **Генерирай API Key:**
   ```http
   POST /api/auth/generate-api-key
   ```

2. **Запиши ключа:**
   - Копирай го от отговора
   - Запиши в `Baglanti.xml` файла на сървъра
   - Или използва автоматичното записване (вече е имплементирано)

3. **Копирай ключа в UI:**
   - Отвори `Baglanti.xml` в UI проекта
   - Добави `<apiKey Value="your-key" />` в `<config>` секцията

4. **Рестартирай API:**
   - API Key middleware ще зареди ключа от XML при стартиране

5. **Тествай:**
   - Всички заявки от `ElTestApiService` автоматично ще използват ключа

## Production съвети:

1. **Ограничи CORS origins:**
   ```csharp
   policy.WithOrigins("https://yourdomain.com")
   ```

2. **Не показвай API Key в logs:**
   - Винаги маскирай ключа при логиране

3. **Ротация на ключовете:**
   - Редовно сменяй API Key за по-добра сигурност
   - Генерирай нов чрез `/api/auth/generate-api-key`

4. **HTTPS само:**
   - Винаги използвай HTTPS в production

5. **Ограничи достъп до generate endpoint:**
   - В production може да се ограничи само за администратори
