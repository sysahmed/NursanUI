using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursan.API.Commands;
using Nursan.API.DTOs;
using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;
using Nursan.Validations.ValidationCode;

namespace Nursan.API.Handlers
{
    /// <summary>
    /// Handler за проверка на системата за ElTest
    /// </summary>
    public class CheckSystemElTestCommandHandler : IRequestHandler<CheckSystemElTestCommand, TorkResultDto>
    {
        private readonly UretimOtomasyonContext _context;
        private readonly IMapper _mapper;

        public CheckSystemElTestCommandHandler(UretimOtomasyonContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TorkResultDto> Handle(CheckSystemElTestCommand request, CancellationToken cancellationToken)
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
                var barcode = _mapper.Map<SyBarcodeInput>(request.Barcode);
                
                var result = torkService.GitSytemeSayiElTestBack(barcode);

                return new TorkResultDto
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = null
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
