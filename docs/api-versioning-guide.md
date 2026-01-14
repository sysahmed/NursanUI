# API Версиониране - Ръководство

## Обзор

API-то вече поддържа версиониране (v1, v2, ...) което позволява постепенна еволюция без breaking changes.

## Версиониране на Endpoints

### Формат на URL

Всички API endpoints следват формата:
```
/api/v{version}/{controller}/{action}
```

**Примери:**
- `GET /api/v1/barcode/scan` - Сканиране на баркод v1
- `POST /api/v1/station-management/reorder-sequence` - Преподреждане на станции v1
- `GET /api/v1/station/bootstrap` - Bootstrap конфигурация v1

### По подразбиране версия v1.0

Ако не е указана версия, по подразбиране се използва v1.0:
- `GET /api/barcode/scan` → `GET /api/v1/barcode/scan`

## Основни Endpoints

### 1. Barcode Operations (v1)

#### `POST /api/v1/barcode/scan`
Сканира баркод с проверка на последователност (Toplam).

**Request:**
```json
{
  "machineName": "STATION-01",
  "barcode": "R2X6-17K400-AAB_00001234",
  "vardiyaName": "Day",
  "sicil": "12345",
  "barcodeType": "First"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Баркод прочетен успешно",
  "donanimId": 1234,
  "donanimReferans": "R2X6-17K400-AAB_00001234",
  "currentStationId": 5,
  "nextStationId": 6,
  "previousStationId": null,
  "shouldPrintBarcode": true,
  "printData": {
    "barcode": "R2X6-17K400-AAB_00001234",
    "printerId": 1,
    "printerName": "Printer-01",
    "printerIp": "192.168.1.100",
    "printTemplate": "template.prn"
  }
}
```

**Грешки:**
- `400 Bad Request` - Предишна станция не е прочетена
- `400 Bad Request` - Баркод вече е прочетен
- `404 Not Found` - Станция не е намерена

### 2. Station Management (v1)

#### `POST /api/v1/station-management/reorder-sequence`
Променя последователността (Toplam) на станциите динамично.

**Сценарий:** Ако имаме станции с Toplam 2, 3, 4 и искаме да добавим нова станция на позиция 3:
- Станция 3 става 4
- Станция 4 става 5
- Новата станция става 3

**Request:**
```json
{
  "stationId": 10,
  "newToplam": 3
}
```

**Response:**
```json
{
  "success": true,
  "message": "Станция 'Station-10' успешно преместена от Toplam 5 към 3",
  "stationId": 10,
  "oldToplam": 5,
  "newToplam": 3,
  "affectedStationsCount": 2
}
```

#### `POST /api/v1/station-management/add-station`
Добавя нова станция в последователността без спиране на производството.

**Request:**
```json
{
  "stationId": 10,
  "insertAtToplam": 3
}
```

#### `POST /api/v1/station-management/deactivate/{stationId}`
Деактивира станция и автоматично премества следващите станции.

#### `GET /api/v1/station-management/sequence/{familyId}`
Връща текущата последователност на станциите за дадено Family.

**Response:**
```json
[
  {
    "stationId": 1,
    "stationName": "Station-01",
    "currentToplam": 1,
    "etap": "Konveyor",
    "isActive": true
  },
  {
    "stationId": 2,
    "stationName": "Station-02",
    "currentToplam": 2,
    "etap": "Gromet",
    "isActive": true
  }
]
```

### 3. Station Bootstrap (v1)

#### `GET /api/v1/station/bootstrap?machineName=STATION-01`
Връща конфигурация за станция при старт на клиента.

### 4. Tork Operations (v1)

#### `POST /api/v1/tork/process-barcode`
Обработва Tork баркодове.

#### `POST /api/v1/tork/process-eltest-barcode`
Обработва ElTest баркодове.

### 5. Harness Models (v1)

#### `GET /api/v1/harnessmodels`
Връща всички модели на кабелни комплекти.

#### `POST /api/v1/harnessmodels`
Създава нов модел.

### 6. Videos (v1)

#### `GET /api/v1/videos/{id}`
Връща видео по ID.

#### `GET /api/v1/videos/{id}/file`
Стримва видео файл.

## API Key Authentication

Всички endpoints (с изключение на `/api/auth/*`) изискват API Key в header:

```
X-API-Key: your-api-key-here
X-Device-Id: your-device-id (optional)
```

**Пример:**
```bash
curl -X POST https://api.example.com/api/v1/barcode/scan \
  -H "X-API-Key: your-api-key" \
  -H "Content-Type: application/json" \
  -d '{
    "machineName": "STATION-01",
    "barcode": "R2X6-17K400-AAB_00001234"
  }'
```

## Последователност на Станции (Toplam)

### Как работи

1. Всяка станция има `Toplam` (ред/последователност) в рамките на `Family`
2. При сканиране на баркод, системата проверява дали всички предишни станции (с по-малък Toplam) са прочетени
3. Ако предишна станция не е прочетена, се връща грешка: "Прочетете баркода първо в предишна станция"

### Пример

Ако имаме:
- Станция 1: Toplam = 1 (Konveyor)
- Станция 2: Toplam = 2 (Gromet)
- Станция 3: Toplam = 3 (ElTest)

При опит за сканиране на Станция 3:
- Проверява се дали баркодът е прочетен в Станция 2 (Toplam 2)
- Ако не е, се връща грешка
- Ако е, се позволява сканирането

### Еднократно прочитане

- Всеки баркод може да бъде прочетен само веднъж на конкретна станция
- Ако баркодът е прочетен на друга станция, се връща грешка за дубликат

## Динамично управление на станции

### Преподреждане без спиране

Сценарий: Искаме да добавим нова станция между станции 2 и 3.

**Преди:**
- Станция A: Toplam = 2
- Станция B: Toplam = 3
- Станция C: Toplam = 4

**Операция:** Добавяне на нова Станция D на позиция 3

**След:**
- Станция A: Toplam = 2
- Станция D: Toplam = 3 (нова)
- Станция B: Toplam = 4 (преместена)
- Станция C: Toplam = 5 (преместена)

### Безопасност

- Всички операции са атомарни (транзакции)
- При грешка, всички промени се отменят (rollback)
- Логване на всички операции за audit trail

## Версиониране в Swagger

Swagger UI автоматично показва всички версии на API-то:
- `https://api.example.com/swagger/index.html`

Можеш да избереш версия от dropdown менюто.

## Миграция към нова версия

Когато се създава нова версия (v2):

1. Създаваш нови controllers в папка `Controllers/V2/`
2. Старият код остава в v1 за обратна съвместимост
3. Клиентите постепенно мигрират към v2
4. След определен период, v1 може да се депрекеира (deprecate)

**Пример:**
```csharp
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BarcodeController : ControllerBase
{
    // Нова имплементация с подобрения
}
```

## Best Practices

1. **Винаги използвай версиониране** за нови endpoints
2. **Не променяй съществуващи endpoints** в текущата версия
3. **Документирай breaking changes** при нова версия
4. **Поддържай обратна съвместимост** минимум 1 версия назад
5. **Логвай всички операции** за debugging и audit

## Поддръжка

За въпроси и проблеми, свържи се с екипа за разработка.
