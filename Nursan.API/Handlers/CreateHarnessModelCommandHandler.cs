using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursan.API.Commands;
using Nursan.Domain.Entity;
using Nursan.Domain.SystemClass;

namespace Nursan.API.Handlers
{
    /// <summary>
    /// Обработчик за команда за създаване на модел на кабелен комплект
    /// </summary>
    public class CreateHarnessModelCommandHandler : IRequestHandler<CreateHarnessModelCommand, OrHarnessModel>
    {
        private readonly UretimOtomasyonContext _context;

        public CreateHarnessModelCommandHandler(UretimOtomasyonContext context)
        {
            _context = context;
        }

        public async Task<OrHarnessModel> Handle(CreateHarnessModelCommand request, CancellationToken cancellationToken)
        {
            var harnessModel = new OrHarnessModel
            {
                Prefix = request.Prefix,
                Family = request.Family,
                Suffix = request.Suffix,
                Release = request.Release,
                FamilyId = request.FamilyId,
                Activ = request.Activ ?? true,
                HarnessModelName = $"{request.Prefix}-{request.Family}-{request.Suffix}",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };

            _context.OrHarnessModels.Add(harnessModel);
            await _context.SaveChangesAsync(cancellationToken);

            return await _context.OrHarnessModels
                .Include(x => x.FamilyNavigation)
                .FirstOrDefaultAsync(x => x.Id == harnessModel.Id, cancellationToken);
        }
    }
}
