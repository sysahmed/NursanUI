# Веб API и Приложение за QR Активация на Лиценз

## Преглед

Този документ описва:
1. **Веб API endpoints** за активация и проверка на лиценз след сканиране на QR кода
2. **Веб приложение** (мобилно/десктоп) за сканиране на QR кода и комуникация с API-то

---

## 1. QR Код Структура

### 1.1 Съдържание на QR кода

QR кодът, генериран от WinForms приложението (`Lisanslama` формата), съдържа JSON с следната структура:

```json
{
  "machineId": "FD0C-0C0F-F87A-6C20-CB63-D8A6-7DBA-9568",
  "serialKey": "p8aeiDehJyhmfrREyQLkgGjB7xCTRvreBWYMGZ1paCyBSbjudqGtBd276pF2/kolbYjuKzdGwfkD4etYRv8UHSXBN8wVea7GlkXx364dxErqX+jnt2Y51FpA/t4RNUoZ7C49Vtk+skFGrJxoAkcP0rjY0Qvnyl8vqvGp7WvOl40duNLCOVZf1aIpy1ehTiH/TML8+lP9Gbue4pvUtMX/jW5mEvSs4sXp11t2Z0YyrwjcySJF5xnw9M6QB+xSUG0ZYZyxY2QpagbjwGq1+R5cDg==",
  "generatedAtUtc": "2025-11-07T07:35:40.5797587Z"
}
```

### 1.2 Полета

| Поле | Тип | Описание |
|------|-----|----------|
| `machineId` | string | Уникален идентификатор на машината (SHA256 хеш). Използва се като основен ключ за лиценза. |
| `serialKey` | string | Сериен ключ, генериран от `KilitYaratma` класа. Използва се за допълнителна валидация. |
| `generatedAtUtc` | ISO8601 | UTC време на генериране на QR кода. Използва се за валидация на срок на годност (напр. 5 минути). |

---

## 2. Веб API Endpoints

### 2.1 POST /api/license/activate

Активира лиценз след сканиране на QR кода от мобилното приложение.

#### Request

**URL:** `POST https://{domain}/api/license/activate`

**Headers:**
```
Content-Type: application/json
Authorization: Bearer {admin_token}  // Опционално, ако изискваш автентикация
```

**Body:**
```json
{
  "machineId": "FD0C-0C0F-F87A-6C20-CB63-D8A6-7DBA-9568",
  "serialKey": "p8aeiDehJyhmfrREyQLkgGjB7xCTRvreBWYMGZ1paCyBSbjudqGtBd276pF2/kolbYjuKzdGwfkD4etYRv8UHSXBN8wVea7GlkXx364dxErqX+jnt2Y51FpA/t4RNUoZ7C49Vtk+skFGrJxoAkcP0rjY0Qvnyl8vqvGp7WvOl40duNLCOVZf1aIpy1ehTiH/TML8+lP9Gbue4pvUtMX/jW5mEvSs4sXp11t2Z0YyrwjcySJF5xnw9M6QB+xSUG0ZYZyxY2QpagbjwGq1+R5cDg==",
  "generatedAtUtc": "2025-11-07T07:35:40.5797587Z",
  "activatedBy": "admin@example.com",  // Опционално - кой е активирал
  "expiryDays": 365  // Опционално - за колко дни да е валиден лицензът (по подразбиране 365)
}
```

#### Response

**Успех (200 OK):**
```json
{
  "success": true,
  "message": "Лицензът е активиран успешно",
  "licenseId": "guid-here",
  "expiryUtc": "2026-11-07T07:35:40.5797587Z",
  "activationCode": "ABC123XYZ"  // Опционално - код за ръчна активация (ако е нужен)
}
```

**Грешка (400 Bad Request):**
```json
{
  "success": false,
  "message": "QR кодът е изтекъл или невалиден",
  "errorCode": "QR_EXPIRED"
}
```

**Грешка (404 Not Found):**
```json
{
  "success": false,
  "message": "Машината не е намерена в системата",
  "errorCode": "MACHINE_NOT_FOUND"
}
```

#### Сървърна логика (псевдо-код)

```csharp
[HttpPost("activate")]
public async Task<IActionResult> ActivateLicense([FromBody] ActivateLicenseRequest request)
{
    // 1. Валидация на времето (QR кодът не трябва да е по-стар от 5 минути)
    var qrAge = DateTime.UtcNow - request.GeneratedAtUtc;
    if (qrAge.TotalMinutes > 5)
    {
        return BadRequest(new { success = false, message = "QR кодът е изтекъл", errorCode = "QR_EXPIRED" });
    }

    // 2. Проверка дали машината съществува в базата
    var machine = await _dbContext.Machines
        .FirstOrDefaultAsync(m => m.MachineId == request.MachineId);
    
    if (machine == null)
    {
        // Опционално: създаване на нова машина
        machine = new Machine
        {
            MachineId = request.MachineId,
            SerialKey = request.SerialKey,
            CreatedAt = DateTime.UtcNow
        };
        _dbContext.Machines.Add(machine);
    }

    // 3. Проверка дали лицензът вече е активиран
    var existingLicense = await _dbContext.Licenses
        .FirstOrDefaultAsync(l => l.MachineId == request.MachineId && !l.IsRevoked);
    
    if (existingLicense != null && existingLicense.ExpiryUtc > DateTime.UtcNow)
    {
        return Ok(new { 
            success = true, 
            message = "Лицензът вече е активиран",
            licenseId = existingLicense.Id,
            expiryUtc = existingLicense.ExpiryUtc
        });
    }

    // 4. Създаване/обновяване на лиценз
    var expiryUtc = DateTime.UtcNow.AddDays(request.ExpiryDays ?? 365);
    
    if (existingLicense != null)
    {
        existingLicense.ExpiryUtc = expiryUtc;
        existingLicense.IsRevoked = false;
        existingLicense.ActivatedAt = DateTime.UtcNow;
        existingLicense.ActivatedBy = request.ActivatedBy;
    }
    else
    {
        var newLicense = new License
        {
            MachineId = request.MachineId,
            ExpiryUtc = expiryUtc,
            ActivatedAt = DateTime.UtcNow,
            ActivatedBy = request.ActivatedBy,
            IsRevoked = false
        };
        _dbContext.Licenses.Add(newLicense);
    }

    await _dbContext.SaveChangesAsync();

    return Ok(new
    {
        success = true,
        message = "Лицензът е активиран успешно",
        licenseId = existingLicense?.Id ?? newLicense.Id,
        expiryUtc = expiryUtc
    });
}
```

---

### 2.2 POST /api/license/check

Проверява дали лицензът е активиран. Използва се от WinForms приложението за периодична проверка.

#### Request

**URL:** `POST https://{domain}/api/license/check`

**Headers:**
```
Content-Type: application/json
```

**Body:**
```json
{
  "machineId": "FD0C-0C0F-F87A-6C20-CB63-D8A6-7DBA-9568",
  "appVersion": "1.0.0",
  "timestampUtc": "2025-11-07T08:00:00.000Z"
}
```

#### Response

**Успех - Лицензът е валиден (200 OK):**
```json
{
  "isValid": true,
  "message": "OK",
  "expiryUtc": "2026-11-07T07:35:40.5797587Z",
  "licenseToken": "base64-encoded-token",
  "encryptedSqlUser": "base64-aes-encrypted",
  "encryptedSqlPassword": "base64-aes-encrypted"
}
```

**Успех - Лицензът не е активиран (200 OK):**
```json
{
  "isValid": false,
  "message": "Лицензът все още не е активиран",
  "expiryUtc": null,
  "licenseToken": null,
  "encryptedSqlUser": null,
  "encryptedSqlPassword": null
}
```

**Успех - Лицензът е изтекъл (200 OK):**
```json
{
  "isValid": false,
  "message": "Лицензът е изтекъл",
  "expiryUtc": "2025-11-06T07:35:40.5797587Z",
  "licenseToken": null,
  "encryptedSqlUser": null,
  "encryptedSqlPassword": null
}
```

#### Сървърна логика (псевдо-код)

```csharp
[HttpPost("check")]
public async Task<IActionResult> CheckLicense([FromBody] CheckLicenseRequest request)
{
    var license = await _dbContext.Licenses
        .FirstOrDefaultAsync(l => l.MachineId == request.MachineId && !l.IsRevoked);

    if (license == null)
    {
        return Ok(new
        {
            isValid = false,
            message = "Лицензът все още не е активиран",
            expiryUtc = (DateTime?)null,
            licenseToken = (string?)null,
            encryptedSqlUser = (string?)null,
            encryptedSqlPassword = (string?)null
        });
    }

    if (license.ExpiryUtc < DateTime.UtcNow)
    {
        return Ok(new
        {
            isValid = false,
            message = "Лицензът е изтекъл",
            expiryUtc = license.ExpiryUtc,
            licenseToken = (string?)null,
            encryptedSqlUser = (string?)null,
            encryptedSqlPassword = (string?)null
        });
    }

    // Генериране на временни SQL креденшъли (ако е нужно)
    var sqlCredentials = await _sqlAccountService.EnsureTemporaryCredentialsAsync(request.MachineId, license);

    var response = new
    {
        isValid = true,
        message = "OK",
        expiryUtc = license.ExpiryUtc,
        licenseToken = GenerateJwtOrGuid(request, license),
        encryptedSqlUser = Encrypt(sqlCredentials.User),
        encryptedSqlPassword = Encrypt(sqlCredentials.Password)
    };

    return Ok(response);
}
```

---

## 3. База данни - Таблици

### 3.1 Таблица: Machines

```sql
CREATE TABLE Machines
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    MachineId NVARCHAR(128) NOT NULL UNIQUE,
    SerialKey NVARCHAR(MAX) NULL,
    CustomerId INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    LastSeenAt DATETIME2 NULL,
    Notes NVARCHAR(MAX) NULL
);

CREATE INDEX IX_Machines_MachineId ON Machines(MachineId);
```

### 3.2 Таблица: Licenses

```sql
CREATE TABLE Licenses
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    MachineId NVARCHAR(128) NOT NULL,
    ExpiryUtc DATETIME2 NOT NULL,
    ActivatedAt DATETIME2 NOT NULL,
    ActivatedBy NVARCHAR(256) NULL,
    IsRevoked BIT NOT NULL DEFAULT 0,
    RevokedAt DATETIME2 NULL,
    RevokedBy NVARCHAR(256) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2 NULL,
    Notes NVARCHAR(MAX) NULL,
    
    CONSTRAINT FK_Licenses_Machines FOREIGN KEY (MachineId) REFERENCES Machines(MachineId)
);

CREATE INDEX IX_Licenses_MachineId ON Licenses(MachineId);
CREATE INDEX IX_Licenses_ExpiryUtc ON Licenses(ExpiryUtc);
CREATE INDEX IX_Licenses_IsRevoked ON Licenses(IsRevoked);
```

### 3.3 Таблица: LicenseActivationLogs

```sql
CREATE TABLE LicenseActivationLogs
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    MachineId NVARCHAR(128) NOT NULL,
    ActivationMethod NVARCHAR(50) NOT NULL,  -- 'QR_CODE', 'MANUAL', 'API'
    ActivatedBy NVARCHAR(256) NULL,
    ActivationData NVARCHAR(MAX) NULL,  -- JSON с допълнителни данни
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

CREATE INDEX IX_LicenseActivationLogs_MachineId ON LicenseActivationLogs(MachineId);
CREATE INDEX IX_LicenseActivationLogs_CreatedAt ON LicenseActivationLogs(CreatedAt);
```

---

## 4. Веб Приложение за Сканиране на QR Код

### 4.1 Технологии

Препоръчителни технологии:
- **Frontend:** React, Vue.js или Angular
- **Mobile:** React Native, Flutter или Progressive Web App (PWA)
- **QR Scanner:** `html5-qrcode` (за web), `react-qr-reader` (за React), `zxing` (за мобилни)

### 4.2 Функционалности

1. **Сканиране на QR код**
   - Отваряне на камерата
   - Сканиране на QR кода
   - Парсване на JSON данните

2. **Изпращане към API**
   - POST заявка към `/api/license/activate`
   - Показване на резултат (успех/грешка)

3. **UI Елементи**
   - Бутон "Сканирай QR код"
   - Преглед на сканираните данни
   - Статус на активацията
   - История на активациите (опционално)

### 4.3 Примерен код (React + html5-qrcode)

```jsx
import { Html5Qrcode } from "html5-qrcode";
import { useState } from "react";

function QRScanner() {
    const [scanning, setScanning] = useState(false);
    const [result, setResult] = useState(null);
    const [error, setError] = useState(null);

    const startScanning = async () => {
        try {
            setScanning(true);
            setError(null);
            
            const html5QrCode = new Html5Qrcode("reader");
            
            await html5QrCode.start(
                { facingMode: "environment" }, // Задня камера
                {
                    fps: 10,
                    qrbox: { width: 250, height: 250 }
                },
                (decodedText, decodedResult) => {
                    handleQRCodeScanned(decodedText);
                    html5QrCode.stop();
                    setScanning(false);
                },
                (errorMessage) => {
                    // Игнорираме грешки при сканиране
                }
            );
        } catch (err) {
            setError("Грешка при отваряне на камерата: " + err.message);
            setScanning(false);
        }
    };

    const handleQRCodeScanned = async (qrData) => {
        try {
            // Парсване на JSON данните
            const qrPayload = JSON.parse(qrData);
            
            // Валидация
            if (!qrPayload.machineId || !qrPayload.serialKey || !qrPayload.generatedAtUtc) {
                setError("Невалиден QR код");
                return;
            }

            // Изпращане към API
            const response = await fetch("https://your-api.com/api/license/activate", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer YOUR_ADMIN_TOKEN" // Ако е нужно
                },
                body: JSON.stringify({
                    machineId: qrPayload.machineId,
                    serialKey: qrPayload.serialKey,
                    generatedAtUtc: qrPayload.generatedAtUtc,
                    activatedBy: "admin@example.com", // От текущия потребител
                    expiryDays: 365
                })
            });

            const data = await response.json();

            if (data.success) {
                setResult({
                    success: true,
                    message: data.message,
                    expiryUtc: data.expiryUtc
                });
            } else {
                setError(data.message || "Грешка при активация");
            }
        } catch (err) {
            setError("Грешка при обработка: " + err.message);
        }
    };

    return (
        <div>
            <h1>Активация на Лиценз</h1>
            
            {!scanning && (
                <button onClick={startScanning}>
                    Сканирай QR Код
                </button>
            )}

            {scanning && (
                <div>
                    <div id="reader" style={{ width: "100%", maxWidth: "500px" }}></div>
                    <button onClick={() => setScanning(false)}>Спри</button>
                </div>
            )}

            {result && (
                <div style={{ color: "green", marginTop: "20px" }}>
                    <h2>Успех!</h2>
                    <p>{result.message}</p>
                    <p>Валиден до: {new Date(result.expiryUtc).toLocaleString()}</p>
                </div>
            )}

            {error && (
                <div style={{ color: "red", marginTop: "20px" }}>
                    <h2>Грешка</h2>
                    <p>{error}</p>
                </div>
            )}
        </div>
    );
}

export default QRScanner;
```

### 4.4 Примерен код (ASP.NET Core Razor Page)

```csharp
@page
@model QRScannerModel

<div class="container">
    <h1>Активация на Лиценз</h1>
    
    <button id="startScanBtn" onclick="startScanning()">Сканирай QR Код</button>
    
    <div id="reader" style="width: 100%; max-width: 500px; display: none;"></div>
    
    <div id="result" style="display: none;"></div>
    <div id="error" style="display: none; color: red;"></div>
</div>

<script src="https://unpkg.com/html5-qrcode"></script>
<script>
    let html5QrCode;

    async function startScanning() {
        try {
            document.getElementById('startScanBtn').style.display = 'none';
            document.getElementById('reader').style.display = 'block';
            
            html5QrCode = new Html5Qrcode("reader");
            
            await html5QrCode.start(
                { facingMode: "environment" },
                {
                    fps: 10,
                    qrbox: { width: 250, height: 250 }
                },
                onScanSuccess,
                onScanFailure
            );
        } catch (err) {
            showError("Грешка при отваряне на камерата: " + err.message);
        }
    }

    function onScanSuccess(decodedText, decodedResult) {
        html5QrCode.stop();
        document.getElementById('reader').style.display = 'none';
        document.getElementById('startScanBtn').style.display = 'block';
        
        handleQRCodeScanned(decodedText);
    }

    function onScanFailure(error) {
        // Игнорираме грешки при сканиране
    }

    async function handleQRCodeScanned(qrData) {
        try {
            const qrPayload = JSON.parse(qrData);
            
            if (!qrPayload.machineId || !qrPayload.serialKey || !qrPayload.generatedAtUtc) {
                showError("Невалиден QR код");
                return;
            }

            const response = await fetch('/api/license/activate', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    machineId: qrPayload.machineId,
                    serialKey: qrPayload.serialKey,
                    generatedAtUtc: qrPayload.generatedAtUtc,
                    activatedBy: '@User.Identity.Name',
                    expiryDays: 365
                })
            });

            const data = await response.json();

            if (data.success) {
                showSuccess(data.message, data.expiryUtc);
            } else {
                showError(data.message || "Грешка при активация");
            }
        } catch (err) {
            showError("Грешка при обработка: " + err.message);
        }
    }

    function showSuccess(message, expiryUtc) {
        const resultDiv = document.getElementById('result');
        resultDiv.style.display = 'block';
        resultDiv.style.color = 'green';
        resultDiv.innerHTML = `
            <h2>Успех!</h2>
            <p>${message}</p>
            <p>Валиден до: ${new Date(expiryUtc).toLocaleString()}</p>
        `;
    }

    function showError(message) {
        const errorDiv = document.getElementById('error');
        errorDiv.style.display = 'block';
        errorDiv.innerHTML = `<h2>Грешка</h2><p>${message}</p>`;
    }
</script>
```

---

## 5. Безопасност

### 5.1 Валидация на QR кода

- **Срок на годност:** QR кодът не трябва да е по-стар от 5 минути
- **Формат:** Валидиране на JSON структурата
- **MachineId:** Проверка дали съществува в базата данни

### 5.2 Rate Limiting

Препоръчително е да се приложи rate limiting:
- Максимум 10 заявки на минута от един IP
- Максимум 3 активации на ден за един `machineId`

### 5.3 Автентикация (Опционално)

Ако искаш да ограничиш достъпа до активацията:
- JWT токен в `Authorization` header
- API ключ в header
- IP whitelist

---

## 6. Тестване

### 6.1 Тестови данни

```json
{
  "machineId": "TEST-MACHINE-001",
  "serialKey": "test-serial-key-12345",
  "generatedAtUtc": "2025-11-07T10:00:00.000Z"
}
```

### 6.2 Postman Collection

Създай Postman collection с:
- `POST /api/license/activate` - тестови данни
- `POST /api/license/check` - проверка на статуса

---

## 7. Деплоймент

### 7.1 API

- **Hosting:** Azure App Service, AWS Lambda, или собствен сървър
- **Database:** SQL Server, PostgreSQL, или MySQL
- **SSL:** Задължително HTTPS

### 7.2 Web App

- **Hosting:** Azure Static Web Apps, Netlify, Vercel, или собствен сървър
- **Domain:** `license-activate.yourdomain.com`

---

## 8. Мониторинг и Логване

### 8.1 Логване

Логвай всички:
- Сканирания на QR кодове
- Активации на лицензи
- Грешки при активация
- Проверки на статус

### 8.2 Метрики

Следвай:
- Брой активации на ден
- Брой неуспешни опити
- Средно време за активация
- Най-активни машини

---

## 9. Поддръжка

### 9.1 Често срещани проблеми

1. **QR кодът не се сканира**
   - Проверка на осветлението
   - Проверка на фокуса на камерата
   - Проверка на качеството на QR кода

2. **API връща грешка "QR_EXPIRED"**
   - Генерирай нов QR код в WinForms приложението

3. **Лицензът не се активира автоматично**
   - Проверка на интернет връзката
   - Проверка на API URL-а в `LicenseStatusChecker`

---

## 10. Следващи стъпки

1. ✅ Създаване на базата данни
2. ✅ Имплементация на API endpoints
3. ✅ Създаване на веб приложение за сканиране
4. ✅ Тестване с реални QR кодове
5. ✅ Деплоймент на production
6. ✅ Мониторинг и оптимизация

---

**Дата на създаване:** 2025-11-07  
**Версия:** 1.0  
**Автор:** AI Assistant

