using AutoMapper;
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
    /// Handler за обработка на Tork баркодове
    /// </summary>
    public class ProcessTorkBarcodeCommandHandler : IRequestHandler<ProcessTorkBarcodeCommand, TorkResultDto>
    {
        private readonly UretimOtomasyonContext _context;
        private readonly IMapper _mapper;

        public ProcessTorkBarcodeCommandHandler(UretimOtomasyonContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TorkResultDto> Handle(ProcessTorkBarcodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Създаване на UnitOfWork с контекста
                var unitOfWork = new UnitOfWork(_context);

                // Намиране на Vardiya
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

                // Създаване на TorkService
                var torkService = new TorkService(unitOfWork, vardiya);

                // Мапиране на DTOs към domain обекти
                var barcodeInputs = _mapper.Map<List<SyBarcodeInput>>(request.Barcodes);

                // Извикване на TorkService
                var result = torkService.GetTorkDonanimBarcode(barcodeInputs);

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
