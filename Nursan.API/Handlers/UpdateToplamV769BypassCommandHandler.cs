using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursan.API.Commands;
using Nursan.API.DTOs;
using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;

namespace Nursan.API.Handlers
{
    /// <summary>
    /// Handler за актуализация на ToplamV769 с bypass
    /// </summary>
    public class UpdateToplamV769BypassCommandHandler : IRequestHandler<UpdateToplamV769BypassCommand, TorkResultDto>
    {
        private readonly UretimOtomasyonContext _context;

        public UpdateToplamV769BypassCommandHandler(UretimOtomasyonContext context)
        {
            _context = context;
        }

        public async Task<TorkResultDto> Handle(UpdateToplamV769BypassCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var unitOfWork = new UnitOfWork(_context);
                var vardiyaResult = unitOfWork.GetRepository<UrVardiya>().Get(x => x.Name == request.VardiyaName);
                
                if (vardiyaResult.Data == null)
                {
                    return new TorkResultDto
                    {
                        Success = false,
                        Message = $"Смяна с име '{request.VardiyaName}' не е намерена!"
                    };
                }

                var vardiya = vardiyaResult.Data;
                var torkService = new TorkService(unitOfWork, vardiya);
                
                DataResult<IzToplamV769> result = torkService.GitDegerleHerseySToplamV769IsBaypass(request.Barcode);

                return new TorkResultDto
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result.Data
                };
            }
            catch (Exception ex)
            {
                return new TorkResultDto
                {
                    Success = false,
                    Message = $"Грешка при обработка: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
