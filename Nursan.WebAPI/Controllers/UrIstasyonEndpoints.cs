using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Nursan.Domain.Entity;
namespace Nursan.WebAPI.Controllers;

public static class UrIstasyonEndpoints
{
    public static void MapUrIstasyonEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/UrIstasyon").WithTags(nameof(UrIstasyon));

        group.MapGet("/", async (UretimOtomasyonContext db) =>
        {
            return await db.UrIstasyons.ToListAsync();
        })
        .WithName("GetAllUrIstasyons")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<UrIstasyon>, NotFound>> (int id, UretimOtomasyonContext db) =>
        {
            return await db.UrIstasyons.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is UrIstasyon model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUrIstasyonById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, UrIstasyon urIstasyon, UretimOtomasyonContext db) =>
        {
            var affected = await db.UrIstasyons
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.Id, urIstasyon.Id)
                    .SetProperty(m => m.Name, urIstasyon.Name)
                    .SetProperty(m => m.ModulerYapiId, urIstasyon.ModulerYapiId)
                    .SetProperty(m => m.FabrikaId, urIstasyon.FabrikaId)
                    .SetProperty(m => m.MashinId, urIstasyon.MashinId)
                    .SetProperty(m => m.VardiyaId, urIstasyon.VardiyaId)
                    .SetProperty(m => m.Toplam, urIstasyon.Toplam)
                    .SetProperty(m => m.Calismasati, urIstasyon.Calismasati)
                    .SetProperty(m => m.Durus, urIstasyon.Durus)
                    .SetProperty(m => m.FamilyId, urIstasyon.FamilyId)
                    .SetProperty(m => m.Hedef, urIstasyon.Hedef)
                    .SetProperty(m => m.Orta, urIstasyon.Orta)
                    .SetProperty(m => m.Realadet, urIstasyon.Realadet)
                    .SetProperty(m => m.Sayi, urIstasyon.Sayi)
                    .SetProperty(m => m.Sayicarp, urIstasyon.Sayicarp)
                    .SetProperty(m => m.Sifirla, urIstasyon.Sifirla)
                    .SetProperty(m => m.Sonokuma, urIstasyon.Sonokuma)
                    .SetProperty(m => m.SyBarcodeOutId, urIstasyon.SyBarcodeOutId)
                    .SetProperty(m => m.UnikId, urIstasyon.UnikId)
                    .SetProperty(m => m.CreateDate, urIstasyon.CreateDate)
                    .SetProperty(m => m.UpdateDate, urIstasyon.UpdateDate)
                    .SetProperty(m => m.Activ, urIstasyon.Activ)
                    );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUrIstasyon")
        .WithOpenApi();

        group.MapPost("/", async (UrIstasyon urIstasyon, UretimOtomasyonContext db) =>
        {
            db.UrIstasyons.Add(urIstasyon);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/UrIstasyon/{urIstasyon.Id}",urIstasyon);
        })
        .WithName("CreateUrIstasyon")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, UretimOtomasyonContext db) =>
        {
            var affected = await db.UrIstasyons
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUrIstasyon")
        .WithOpenApi();
    }
}
