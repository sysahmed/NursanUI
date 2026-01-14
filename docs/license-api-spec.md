# Лицензен API – техническа спецификация

Документът описва изискванията към бекенд услугата, която проверява и издава лиценз за клиентското WinForms приложение (`Nursan.UI`). Цел: при стартиране клиентът да получи временни SQL креденшъли само ако машината е лицензирана.

## 1. Ендпойнт

- **Метод:** `POST`
- **URL:** `https://{domain}/api/license/check`
- **Срок за отговор:** ≤ 3 сек
- **Content-Type:** `application/json`
- **Автентикация:** HMAC подпис (описан по-долу)
- **TLS:** задължително (препоръка: certificate pinning)

## 2. Request payload

```json
{
  "machineId": "HEX_SHA256",
  "appVersion": "1.0.0",
  "timestampUtc": "2025-01-08T14:23:10.000Z",
  "signature": "BASE64_HMAC"
}
```

### 2.1 Полета

| Поле         | Тип       | Описание                                                                                      |
|--------------|-----------|------------------------------------------------------------------------------------------------|
| `machineId`  | string    | 64-символен HEX (SHA256 на machine name + домейн + ОС + MAC). Използва се като основен ключ. |
| `appVersion` | string    | Версията на клиента (напр. `Application.ProductVersion`).                                     |
| `timestampUtc` | ISO8601 | UTC време на заявката (за предотвратяване на replay атаки).                                     |
| `signature`  | string    | Base64 HMAC-SHA256 (`machineId|timestampUtc`, секретен ключ).                                  |

> Препоръка: секретният ключ се споделя между клиента и API-то. Сменя се периодично. На API страна – валидирай времевия прозорец (±2 мин).

## 3. Response payload

```json
{
  "isValid": true,
  "message": "OK",
  "expiryUtc": "2025-01-09T00:00:00.000Z",
  "licenseToken": "BASE64",
  "encryptedSqlUser": "BASE64_AES",
  "encryptedSqlPassword": "BASE64_AES"
}
```

### 3.1 Полета

| Поле                  | Тип       | Описание                                                                                                     |
|-----------------------|-----------|---------------------------------------------------------------------------------------------------------------|
| `isValid`             | bool      | `true` ако машината е лицензирана.                                                                           |
| `message`             | string    | Човешко четим текст (за логване/съобщения към потребител).                                                    |
| `expiryUtc`           | ISO8601   | Време, до което токенът е валиден (примерно +1 час). Ако `null`/изтекъл → клиентът отказва достъп.           |
| `licenseToken`        | string    | Опционален токен (може да носи информация за права/feature flags).                                           |
| `encryptedSqlUser`    | string    | SQL username, криптиран (AES-CBC, Base64). Ако няма база → `null`.                                           |
| `encryptedSqlPassword`| string    | SQL password, криптиран.                                                                                    |

### 3.2 Статуси и грешки

- `200 OK` – валиден отговор (дори и при `isValid:false`).
- `400 BadRequest` – некоректно тяло/подпис.
- `401 Unauthorized` – липсва подпис или машината няма лиценз.
- `429 TooManyRequests` – rate limit (препоръчително, според IP/machineId).
- `500 InternalServerError` – техническа грешка (логва се, клиентът ще опита отново след 60 сек).

## 4. Сървърна логика (псевдо-код)

```csharp
public async Task<IActionResult> CheckLicense(LicenseRequestDto dto)
{
    if (!ValidateSignature(dto))
        return Unauthorized(new { isValid = false, message = "Invalid signature" });

    var license = await LicensesRepository.FindByMachineId(dto.MachineId);
    if (license == null || license.IsRevoked)
        return Ok(new { isValid = false, message = "No license" });

    if (license.ExpiryUtc.HasValue && license.ExpiryUtc < DateTime.UtcNow)
        return Ok(new { isValid = false, message = "License expired" });

    // 1) Генерирай/върни SQL потребител
    var sqlCredentials = await SqlAccountService.EnsureTemporaryCredentialsAsync(dto.MachineId, license);

    var response = new LicenseResponse
    {
        IsValid = true,
        Message = "OK",
        ExpiryUtc = DateTime.UtcNow.AddHours(1),
        LicenseToken = GenerateJwtOrGuid(dto, license),
        EncryptedSqlUser = Encrypt(sqlCredentials.User),
        EncryptedSqlPassword = Encrypt(sqlCredentials.Password)
    };

    return Ok(response);
}
```

## 5. SQL част – временни креденшъли

### 5.1 Подход

1. **Пул** от SQL logins – `svc_license_{guid}` с ограничени права (само нужната база).
2. При валидна заявка API:
   - Избира свободен login или го създава (ако се ползва динамично създаване → `CREATE LOGIN` + `CREATE USER`).
   - Задава сложна парола, валидна за 1 час.
   - Пази асоциация `machineId ↔ sqlLogin ↔ expiry`.
   - Връща криптираните данни към клиента.
3. Ако срокът изтече – API може да обнови паролата или да освободи login-а.

### 5.2 Примерна таблица

```sql
CREATE TABLE Licenses
(
    MachineId      NVARCHAR(128) PRIMARY KEY,
    CustomerId     INT NOT NULL,
    ExpiryUtc      DATETIME2 NULL,
    SqlLogin       NVARCHAR(128) NULL,
    LastIssuedUtc  DATETIME2 NULL,
    IsRevoked      BIT NOT NULL CONSTRAINT DF_Licenses_IsRevoked DEFAULT (0)
);
```

### 5.3 Стратегия за пароли

- Дължина ≥ 32 символа.
- Съхраняват се само хеширани (или криптирани) в API.
- Валидност: 1 час (може да се обнови при жест на клиента).

## 6. Криптография (AES)

- **Алгоритъм:** AES-256-CBC, PKCS7.
- **Ключ:** Base64 (32 байта). Задължително различен от HMAC ключа.
- **IV:** 16 байта, фиксиран или изпратен заедно с ciphertext.
- **Изпълнение:** API → `Aes.Create()` → `CreateEncryptor(Key, IV)` → `Convert.ToBase64String`.

> В клиентския код ключът/IV са твърдо кодирани (обфусцирани). API-то трябва да използва същите стойности.

## 7. HMAC подпис

- `signature = Base64(HMAC_SHA256(machineId + "|" + timestampUtc, SECRET_KEY))`
- SECRET_KEY трябва да е 32+ символа, пазен само от API и клиента.
- API валидира, че `timestampUtc` е в допустим прозорец (примерно ±2 минути).

## 8. Жизнен цикъл на лицензите

1. **Издаване** – администратор регистрара machineId/клиент в базата.
2. **Проверка** – client → API `license/check`; ако е валиден – получава временни креденшъли.
3. **Подновяване** – клиентът изисква нов токен (след изтичане) – API може да обнови паролата.
4. **Отнемане** – флаг `IsRevoked = 1`; API връща `isValid:false` при следваща проверка.

## 9. Логване и мониторинг

- Логвай всички заявки (`machineId`, IP, timestamp, резултат).
- Rate limit по IP/machineId (напр. 30 req/min).
- Аларми при твърде много откази или многократни различни IP за един и същи machineId.

## 10. Допълнителни препоръки

- API-то да бъде зад reverse proxy (Cloudflare, Nginx) с Web Application Firewall.
- Поддръжка на `POST /api/license/heartbeat` (за периодично подновяване без пълно login).
- При `IsValid:false` клиентът показва съобщение и спира процеса.
- Използвайте `JwtSecurityToken` ако има нужда от подписан токен за офлайн валидиране.

## 11. QR активация (desktop → mobile)

- Настолната форма `Lisanslama` генерира QR код с JSON полезен товар:

```json
{
  "machineId": "HEX_SHA256",
  "serialKey": "VALUE_FROM_KilitYaratma",
  "generatedAtUtc": "2025-01-08T14:20:00Z"
}
```

- QR кодът се сканира от мобилно приложение (или web scanning UI).
- Мобилният клиент изпраща payload-а към API-то (напр. `POST /api/license/activate`), след което администраторът може да издаде/поднови лиценз.
- API връща „код за активиране“ (или автоматично активира) → операторът въвежда кода в полето „Активирай“ в WinForms формата.
- След успех `reg.regYaz(...)` записва локално състояние; при следващ старт WinForms-ът пак прави стандартната онлайн проверка.

> Забележка: Това е допълнителен канал – при желание API-то може директно да повдига флаг за лиценз и да върне активационен код през мобилното приложение.

---

Този документ може да се предаде на бекенд разработчика. При нужда от примерен ASP.NET Core контролер – кажи и ще подготвя шаблон.


