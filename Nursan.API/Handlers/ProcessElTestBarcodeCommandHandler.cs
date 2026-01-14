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
    /// Handler за обработка на ElTest баркодове
    /// </summary>
    public class ProcessElTestBarcodeCommandHandler : IRequestHandler<ProcessElTestBarcodeCommand, TorkResultDto>
    {
        private readonly UretimOtomasyonContext _context;
        private readonly IMapper _mapper;

        public ProcessElTestBarcodeCommandHandler(UretimOtomasyonContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TorkResultDto> Handle(ProcessElTestBarcodeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Създаване на UnitOfWork
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

                // Извикване на TorkService
                var result = torkService.GetElTestDonanimBarcode(request.Barcodes);

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
