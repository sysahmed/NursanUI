using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursan.API.Queries;
using Nursan.Domain.Entity;

namespace Nursan.API.Handlers
{
    /// <summary>
    /// Обработчик за заявка за получаване на модели на кабелни комплекти
    /// </summary>
    public class GetHarnessModelsQueryHandler : IRequestHandler<GetHarnessModelsQuery, List<OrHarnessModel>>
    {
        private readonly UretimOtomasyonContext _context;

        public GetHarnessModelsQueryHandler(UretimOtomasyonContext context)
        {
            _context = context;
        }

        public async Task<List<OrHarnessModel>> Handle(GetHarnessModelsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.OrHarnessModels.AsQueryable();

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.Activ == request.IsActive.Value);
            }

            if (request.FamilyId.HasValue)
            {
                query = query.Where(x => x.FamilyId == request.FamilyId.Value);
            }

            return await query
                .Include(x => x.FamilyNavigation)
                .OrderBy(x => x.HarnessModelName)
                .ToListAsync(cancellationToken);
        }
    }
}
