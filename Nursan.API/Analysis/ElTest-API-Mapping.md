# Анализ на ElTest.cs - Мапинг към API

## Текущи зависимости и техните замени

### 1. **TorkService** използвания:

#### `TorkService.GitSytemeSayiElTestBack()` 
- **Локация:** Ред 423, 1039
- **Използване:** Проверка на системата за ElTest
- **API замяна:** `POST /api/tork/check-system-eltest`
- **Команда:** `CheckSystemElTestCommand`

#### `TorkService.GitSytemeSayiBac()`
- **Локация:** Ред 486
- **Използване:** Проверка на системата
- **API замяна:** `POST /api/tork/check-system`
- **Команда:** `CheckSystemCommand`

### 2. **EltestValidasyonlari** използвания:

#### `_elTest.GitSystemeYukle()`
- **Локация:** Ред 272
- **Използване:** Зареждане на данни в системата от файл
- **API замяна:** `POST /api/tork/process-eltest-barcode`
- **Команда:** `ProcessElTestBarcodeCommand`
- **Забележка:** Приема масив от баркодове `string[]`

#### `_elTest.GithataYukle()`
- **Локация:** Ред 336
- **Използване:** Зареждане на грешки в системата
- **API замяна:** Трябва да се създаде нов endpoint за обработка на грешки
- **Забележка:** Чете файл ред по ред и обработва грешки

#### `_elTest.GetPcId()`
- **Локация:** Ред 802
- **Използване:** Получаване на PC ID от базата
- **API замяна:** Трябва да се създаде endpoint за получаване на PC ID
- **Алтернатива:** Може да остане локално ако се използва само за логване

### 3. **Директни DB достъпи:**

#### `AddTicket()` - AmbarContext
- **Локация:** Ред 800-822
- **Използване:** Добавяне на тикет локално в базата
- **API замяна:** `SystemTicket.CreateTicket()` - вече използва API
- **Забележка:** Вече има API имплементация, може да се използва директно

### 4. **SystemTicket** (вече използва API):
- `CreateTicket()` - вече използва HTTP API
- `GetTicketsByRoleName()` - вече използва HTTP API
- **Не изисква промяна**

## Нов API Client нужда

Трябва да създадем API Client service, който да обвива HTTP заявките към API-то.

## Структура на ElTest-vApi

1. Замени `UnitOfWork` с `HttpClient` или API Client Service
2. Замени `TorkService` извикванията с API calls
3. Замени `EltestValidasyonlari` извикванията с API calls
4. Запази `SystemTicket` (вече използва API)
5. Замени директните DB достъпи с API calls
