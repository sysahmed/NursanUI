# Как да генерираш API Key

## Метод 1: Чрез Swagger UI (Препоръчително) ⭐

1. **Стартирай API-то** и отиди на: `http://localhost:5226/swagger`

2. **Логни се:**
   - Намери `/api/auth/login` endpoint
   - Натисни "Try it out"
   - Попълни:
     ```json
     {
       "username": "syscom@mail.bg",
       "password": "твоята-парола"
     }
   - Натисни "Execute"
   - Копирай `token` от отговора

3. **Авторизирай се в Swagger:**
   - Най-горе в Swagger UI кликни на зеления бутон **"Authorize"**
   - В полето "Value" въведи: `Bearer твоя-токен-тук`
   - Пример: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`
   - Натисни "Authorize", след това "Close"

4. **Генерирай API Key:**
   - Намери `/api/auth/generate-api-key` endpoint
   - Натисни "Try it out"
   - Попълни:
     ```json
     {
       "deviceId": "123",
       "deviceName": "AHMEDLAPTOP",
       "description": "Моят компютър",
       "overwrite": true
     }
     ```
   - Натисни "Execute"
   - Копирай `apiKey` от отговора (показва се само веднъж!)

## Метод 2: Чрез MVC Interface

1. **Отиди на:** `http://localhost:5226/Home/Login`

2. **Влез** с потребител от Ambar базата

3. **Отиди на:** `http://localhost:5226/ApiKeyManagement/Create`

4. **Попълни формата:**
   - Device ID: `123` (или каквото искаш)
   - Device Name: `AHMEDLAPTOP`
   - Описание: `Моят компютър`

5. **Натисни "Създай API Key"**

6. **Копирай API Key-а** (показва се само веднъж!)

## Метод 3: Чрез cURL (ако трябва)

1. **Първо вземи JWT токен:**
   ```bash
   curl -X 'POST' \
     'http://localhost:5226/api/auth/login' \
     -H 'Content-Type: application/json' \
     -d '{
     "username": "syscom@mail.bg",
     "password": "твоята-парола"
   }'
   ```

2. **Копирай токена** от отговора

3. **Генерирай API Key с токена:**
   ```bash
   curl -X 'POST' \
     'http://localhost:5226/api/auth/generate-api-key' \
     -H 'Content-Type: application/json' \
     -H 'Authorization: Bearer ТВОЯ-TOKEN-ТУК' \
     -d '{
     "deviceId": "123",
     "deviceName": "AHMEDLAPTOP",
     "description": "Моят компютър",
     "overwrite": true
   }'
   ```

## След генериране:

1. **Копирай API Key-а** (показва се само веднъж!)

2. **Добави го в Baglanti.xml:**
   ```xml
   <apiKey Value="ВАШИЯ-API-KEY-ТУК" DeviceId="123" />
   ```

3. **Рестартирай ElTestvApi приложението**

## Важно:

- API Key се записва автоматично в базата данни
- DeviceId трябва да е уникален за всяка станция
- API Key работи постоянно (не се изтича)
- Можеш да деактивираш или регенерираш от MVC интерфейса

