# Nursan.API

REST API проект за системата Nursan, използващ MediatR pattern за CQRS архитектура.

## Структура на проекта

```
Nursan.API/
├── Controllers/          # API контролери
├── Handlers/            # MediatR обработчици (Command/Query handlers)
├── Commands/            # Commands за промени (CQRS Write)
├── Queries/             # Queries за четене (CQRS Read)
├── DTOs/                # Data Transfer Objects
├── Mapping/             # AutoMapper профили
├── Extensions/          # Extension методи
└── Program.cs           # Точка на влизане и конфигурация
```

## Технологии

- **.NET 8.0** - Платформа
- **MediatR** - Mediator pattern за CQRS
- **Entity Framework Core 8.0** - ORM
- **AutoMapper** - Обектно мапиране
- **Swagger/OpenAPI** - API документация
- **SQL Server** - База данни

## Конфигурация

### Connection Strings

Connection strings се конфигурират в `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=UretimOtomasyon;...",
    "AmbarConnection": "Server=...;Database=Ambar;...",
    "PersonalConnection": "Server=...;Database=Personal;..."
  }
}
```

## Scaffold на модели от базата данни

### Scaffold на модел от UretimOtomasyon базата

```powershell
# От директорията на API проекта
dotnet ef dbcontext scaffold "Server=10.168.0.5;Database=UretimOtomasyon;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/UretimOtomasyon -c UretimOtomasyonContext -f --project .
```

### Scaffold на модел от Ambar базата

```powershell
dotnet ef dbcontext scaffold "Server=10.168.0.5;Database=Ambar;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/Ambar -c AmbarContext -f --project .
```

### Scaffold на модел от Personal базата

```powershell
dotnet ef dbcontext scaffold "Server=10.168.0.5;Database=Personal;User Id=sa;Password=wrjkd34mk22;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models/Personal -c PersonalContext -f --project .
```

**Важно:** При scaffold на нови модели, заменете съществуващите контексти или създайте нови с различни имена.

## Използване на модели от Nursan.Domain

Тъй като моделите вече са дефинирани в `Nursan.Domain` проекта, те могат да се използват директно в API проекта чрез референция към проекта.

### Пример за използване

```csharp
// В Handler
using Nursan.Domain.Entity;

public class GetHarnessModelsQueryHandler : IRequestHandler<GetHarnessModelsQuery, List<OrHarnessModel>>
{
    private readonly UretimOtomasyonContext _context;
    
    // ...
}
```

## Добавяне на нови endpoints

1. **Създаване на Query/Command**
   - За четене: Създайте клас в `Queries/` имплементиращ `IRequest<TResponse>`
   - За запис: Създайте клас в `Commands/` имплементиращ `IRequest<TResponse>`

2. **Създаване на Handler**
   - Създайте клас в `Handlers/` имплементиращ `IRequestHandler<TRequest, TResponse>`

3. **Добавяне на Controller endpoint**
   - Използвайте `IMediator.Send()` за изпращане на заявка/команда

## Примерен endpoint

```csharp
[HttpGet]
public async Task<ActionResult<List<HarnessModelDto>>> GetHarnessModels()
{
    var query = new GetHarnessModelsQuery();
    var result = await _mediator.Send(query);
    var dto = _mapper.Map<List<HarnessModelDto>>(result);
    return Ok(dto);
}
```

## Стартиране на проекта

```powershell
dotnet run --project Nursan.API
```

API ще бъде достъпен на: `https://localhost:5001` или `http://localhost:5000`

Swagger документация: `https://localhost:5001/swagger`

## Забележки

- Проектът използва **NoTracking** режим по подразбиране за по-добра производителност
- **CORS** е конфигуриран за всички origins в Development режим
- В Production режим, конфигурирайте CORS подходящо

