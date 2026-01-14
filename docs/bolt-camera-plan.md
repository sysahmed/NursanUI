## План: Интеграция на камера и поетапно стягане на болтове в Tork

### Цели
- Визуализация на RTSP камера по време на процес в `Tork` без нарушаване на текущата логика.
- Водене по стъпки за стягане на болтове (Bolt 1 → Bolt 2 → Bolt 3).
- Потвърждаване на всяка стъпка чрез сигнал от серийния порт; опционално визуално потвърждение (ROI motion stop) в бъдеще.
- Автоматично пускане на етикет след последната стъпка, използвайки текущия поток.

### Входни данни и източници
- Идентификация на модел: `OrHarnessModel.HarnessModelName` (пример: `Prefix-Family-Suffix`).
- Зареждане на „рецепта на болтовете“ по име на модел (без FK към `OrHarnessConfig`, по изискване).
  - Таблица: `dbo.OrBoltProcess` (нова, създава се ръчно с предоставените SQL заявки).
  - Ключ: `HarnessModelName` + `StepIndex` (уникални).
  - Полетa: `RoiX/RoiY/RoiWidth/RoiHeight`, `CameraRtspUrl`, `TorqueTarget/Min/Max`, `AngleTarget/Min/Max`, `IsEnabled`.
- При липса на записи: дефолт 3 болта (статично поведение).

### RTSP камера (примери)
- Примерни URL-и за устройство на `10.168.0.211`:
  - Hikvision: `rtsp://admin:admin@10.168.0.211:554/Streaming/Channels/101`
  - Dahua: `rtsp://admin:admin@10.168.0.211:554/cam/realmonitor?channel=1&subtype=0`
  - Generic: `rtsp://admin:admin@10.168.0.211:554/h264/ch1/main/av_stream`
- Първи етап: само визуализация (preview) в `Tork` (`PictureBox`).
- Втори етап (по избор): запис на видео в папка и/или motion-валидация по ROI.

### Точки за интеграция в приложението
1) Зареждане на рецептата (след четене на баркодовете):
   - Място: `Tork.cs`, в `txtBarcode_KeyUpAsync`, непосредствено след като имаме `_id` и достъп до модела.
   - Действия:
     - Извличане на `HarnessModelName` от `OrHarnessModel` (чрез наличния `HarnesModelId`).
     - SELECT към `dbo.OrBoltProcess` по `HarnessModelName` (виж секция SQL), `ORDER BY StepIndex`.
     - Ако 0 реда → подготвяме 3 дефолтни стъпки в памет.
     - Инициализираме state: `CurrentModelName`, `BoltSteps`, `CurrentBoltIndex=1`.
     - Показваме подсказка „Стегни болт 1“ (UI label).

2) Управление по сериен порт (потвърждение на болт):
   - Място: `Tork.cs`, метод `GelenVeriDegerle(string veri)`.
   - Сигнали (примерна интерпретация):
     - `"1"` → Потвърждение за текущ болт (torque OK) → `CurrentBoltIndex++` и подсказка „Стегни болт X“.
     - `"2"` или `"6"` → Край на процес → остава текущият поток (`plc_StopWOrk(Barcode)`), след което зануляваме и bolt-state.
     - `"3"/"4"/"5"` → Reset/Cancel → зануляваме bolt-state.
   - Забележка: точните кодове може да се потвърдят с процесния екип; логиката е конфигурируема.

3) UI промени (ненатрапчиво):
   - Добавяне на `PictureBox` за live камера в `Tork` (в панела с логото), скрит по подразбиране.
   - Добавяне на label за подсказка „Стегни болт N от M“.
   - Флаг „CameraEnabled“ (app setting) за включване при нужда.

### Дизайн на състоянието (в памет)
- `CurrentModelName: string`
- `BoltSteps: List<BoltStep>` (StepIndex, ROI, RtspUrl, Torque/Angle цели – за бъдеща валидация)
- `CurrentBoltIndex: int`
- `CameraEnabled: bool`

### SQL шаблони
SELECT рецептата по модел:
```sql
DECLARE @HarnessModelName NVARCHAR(200) = @p_HarnessModelName;

SELECT Id, HarnessModelName, StepIndex,
       RoiX, RoiY, RoiWidth, RoiHeight,
       CameraRtspUrl,
       TorqueTarget, TorqueMin, TorqueMax,
       AngleTarget,  AngleMin,  AngleMax,
       IsEnabled, Notes
FROM dbo.OrBoltProcess WITH (NOLOCK)
WHERE HarnessModelName = @HarnessModelName
  AND IsEnabled = 1
ORDER BY StepIndex ASC;
```

Примерни INSERT-и (3 болта):
```sql
INSERT INTO dbo.OrBoltProcess
(HarnessModelName, StepIndex, RoiX, RoiY, RoiWidth, RoiHeight, CameraRtspUrl,
 TorqueTarget, TorqueMin, TorqueMax, AngleTarget, AngleMin, AngleMax, IsEnabled, Notes, CreatedBy)
VALUES
(N'HARNESS_A', 1, 100, 200, 220, 220, N'rtsp://admin:admin@10.168.0.211:554/Streaming/Channels/101',
 3.00, 2.50, 3.50, 25.0, 15.0, 35.0, 1, N'Болт 1', N'system'),
(N'HARNESS_A', 2, 360, 200, 220, 220, N'rtsp://admin:admin@10.168.0.211:554/Streaming/Channels/101',
 3.00, 2.50, 3.50, 25.0, 15.0, 35.0, 1, N'Болт 2', N'system'),
(N'HARNESS_A', 3, 620, 200, 220, 220, N'rtsp://admin:admin@10.168.0.211:554/Streaming/Channels/101',
 3.00, 2.50, 3.50, 25.0, 15.0, 35.0, 1, N'Болт 3', N'system');
```

### Безопасност и неизменност на текущия поток
- Не се променят съществуващи модели; новата таблица няма FK към `OrHarnessConfig`.
- Логиката за серийния порт (стартиране/спиране) остава непроменена.
- Новият „болтов state“ е изцяло в памет; занулява се при край/ресет сигналите, без странични ефекти.
- Камерата е изключена по подразбиране (feature flag).

### Тест план
1) Debug без камера: зареждане на дефолт 3 болта; сериен `"1"` циклите през 1→2→3; `"2"` пуска етикета (както днес).
2) С рецепта в БД: зарежда 3 стъпки по `HarnessModelName`; поведение идентично, но с визуални подсказки от рецептата.
3) Камера ON (по желание): визуализация на RTSP; проверка на стабилност/латентност.
4) Неочаквани сигнали: `"3"/"4"/"5"` → reset; повторно зареждане на `GetBarcodeInput()` остава непроменено.

### Рискове и минимизация
- Заключвания на DLL/Designer (наблюдавани): затваряне на дизайнери, Clean/Rebuild; избягване на промени в runtime критични класове.
- Производителност на RTSP: по подразбиране OFF; при ON – ограничаване на fps/резолюция.
- Неконсистентни имена на модел: нормализиране (Trim/ToUpperInvariant) при SELECT-а.

### Внедряване на етапи
1) Добавяне на таблици (SQL ръчно).
2) Зареждане на рецептата в апа и state водене 1→3 (без камера).
3) Визуализация на RTSP по флаг (без влияние на логиката).
4) Опционално: запис/ROI motion, ресурсни оптимизации.

### Конфигурации (appsettings/настройки)
- `CameraEnabled: false`
- `DefaultRtspUrl: "rtsp://admin:admin@10.168.0.211:554/Streaming/Channels/101"`
- `VideoOutputPath: "D:\\ProcessVideos"` (ако се активира запис)

### Какво остава да уточним
- Точният сериен код за „болт ОК“ (предложение: `"1"`).
- На кой сигнал считаме „край на процес“ (по текущото поведение: `"2"`/`"6"`).
- Потвърждение за това откъде четем `HarnessModelName` (от `OrHarnessModel` по `HarnesModelId`).


