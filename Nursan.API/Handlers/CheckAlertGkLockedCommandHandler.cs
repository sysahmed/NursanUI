using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursan.API.Commands;
using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;

namespace Nursan.API.Handlers
{
    /// <summary>
    /// Handler за проверка дали Alert е заключен
    /// </summary>
    public class CheckAlertGkLockedCommandHandler : IRequestHandler<CheckAlertGkLockedCommand, CheckAlertGkLockedResult>
    {
        private readonly UretimOtomasyonContext _context;

        public CheckAlertGkLockedCommandHandler(UretimOtomasyonContext context)
        {
            _context = context;
        }

        public async Task<CheckAlertGkLockedResult> Handle(CheckAlertGkLockedCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var unitOfWork = new UnitOfWork(_context);
                var vardiyaResult = unitOfWork.GetRepository<UrVardiya>().Get(x => x.Name == request.VardiyaName);
                
                if (vardiyaResult.Data == null)
                {
                    return new CheckAlertGkLockedResult
                    {
                        IsLocked = false,
                        Message = $"Смяна с име '{request.VardiyaName}' не е намерена!"
                    };
                }

                var vardiya = vardiyaResult.Data;
                var torkService = new TorkService(unitOfWork, vardiya);
                
                var isLocked = torkService.IsAlertGkLocked(request.Barcode);

                return new CheckAlertGkLockedResult
                {
                    IsLocked = isLocked,
                    Message = isLocked ? "Alert е заключен" : "Alert не е заключен"
                };
            }
            catch (Exception ex)
            {
                return new CheckAlertGkLockedResult
                {
                    IsLocked = false,
                    Message = $"Грешка при обработка: {ex.Message}"
                };
            }
        }
    }
}
