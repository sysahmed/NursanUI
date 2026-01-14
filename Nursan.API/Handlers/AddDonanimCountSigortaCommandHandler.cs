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
    /// Handler за добавяне на донаним (сигурност)
    /// </summary>
    public class AddDonanimCountSigortaCommandHandler : IRequestHandler<AddDonanimCountSigortaCommand, TorkResultDto>
    {
        private readonly UretimOtomasyonContext _context;
        private readonly IMapper _mapper;

        public AddDonanimCountSigortaCommandHandler(UretimOtomasyonContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TorkResultDto> Handle(AddDonanimCountSigortaCommand request, CancellationToken cancellationToken)
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

                var istasyonResult = unitOfWork.GetRepository<UrIstasyon>().GetById(request.IstasyonId);
                if (istasyonResult.Data == null)
                {
                    return new TorkResultDto
                    {
                        Success = false,
                        Message = $"Станция с ID '{request.IstasyonId}' не е намерена!"
                    };
                }

                var vardiya = vardiyaResult.Data;
                var istasyon = istasyonResult.Data;
                var torkService = new TorkService(unitOfWork, vardiya);
                var barcode = _mapper.Map<SyBarcodeInput>(request.Barcode);
                var generateId = _mapper.Map<IzGenerateId>(request.GenerateId);
                
                var result = torkService.AddDonanimCountSigorta(barcode, generateId, istasyon);

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
