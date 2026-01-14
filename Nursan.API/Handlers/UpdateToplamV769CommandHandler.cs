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
    /// Handler за актуализация на ToplamV769
    /// </summary>
    public class UpdateToplamV769CommandHandler : IRequestHandler<UpdateToplamV769Command, TorkResultDto>
    {
        private readonly UretimOtomasyonContext _context;

        public UpdateToplamV769CommandHandler(UretimOtomasyonContext context)
        {
            _context = context;
        }

        public async Task<TorkResultDto> Handle(UpdateToplamV769Command request, CancellationToken cancellationToken)
        {
            try
            {
                // Валидация на входните данни
                if (request?.Data == null)
                {
                    return new TorkResultDto
                    {
                        Success = false,
                        Message = "Данните за заявката са празни!"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Data.Barcode))
                {
                    return new TorkResultDto
                    {
                        Success = false,
                        Message = "Баркодът е задължителен!"
                    };
                }

                if (string.IsNullOrWhiteSpace(request.Data.VardiyaName))
                {
                    return new TorkResultDto
                    {
                        Success = false,
                        Message = "Името на смяната е задължително!"
                    };
                }

                // Създаване на UnitOfWork
                var unitOfWork = new UnitOfWork(_context);

                // Намиране на Vardiya
                var vardiyaResult = unitOfWork.GetRepository<UrVardiya>().Get(x => x.Name == request.Data.VardiyaName);
                if (vardiyaResult?.Data == null)
                {
                    return new TorkResultDto
                    {
                        Success = false,
                        Message = $"Смяна с име '{request.Data.VardiyaName}' не е намерена!"
                    };
                }

                var vardiya = vardiyaResult.Data;

                // Създаване на TorkService
                var torkService = new TorkService(unitOfWork, vardiya);

                // Извикване на подходящия метод според дали има Sicil
                DataResult<IzToplamV769> result = string.IsNullOrWhiteSpace(request.Data.Sicil)
                    ? torkService.GitDegerleHerseySToplamV769(request.Data.Barcode)
                    : torkService.GitDegerleHerseySToplamV769Gromet(request.Data.Barcode, request.Data.Sicil);

                // Проверка за null резултат (може да се случи при exception в TorkService)
                if (result == null)
                {
                    return new TorkResultDto
                    {
                        Success = false,
                        Message = "Грешка при обработка на баркода! Резултатът е null.",
                        Data = null
                    };
                }

                return new TorkResultDto
                {
                    Success = result.Success,
                    Message = result.Message ?? "Неизвестна грешка",
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
