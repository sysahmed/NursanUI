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
    /// Handler за печат на баркод
    /// </summary>
    public class PrintBarcodeCommandHandler : IRequestHandler<PrintBarcodeCommand, TorkResultDto>
    {
        private readonly UretimOtomasyonContext _context;
        private readonly IMapper _mapper;

        public PrintBarcodeCommandHandler(UretimOtomasyonContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TorkResultDto> Handle(PrintBarcodeCommand request, CancellationToken cancellationToken)
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
                var barcodes = _mapper.Map<List<SyBarcodeInput>>(request.Barcodes);
                
                var result = torkService.GitBarcodeBas(barcodes);

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
