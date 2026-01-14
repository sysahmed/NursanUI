using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.Opsionlar
{
    public class ToplamV769Services
    {
        private readonly IUnitOfWork _repo;
        public ToplamV769Services(IUnitOfWork repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(_repo));
        }
        public DataResult<IzToplamV769> AddToplamV769(IzToplamV769 id)
        {
            try
            {
                if (id == null)
                {
                    return new ErrorDataResults<IzToplamV769>("Данните за добавяне са null!");
                }

                var result = _repo.GetRepository<IzToplamV769>().Add(id);
                if (result?.Success == true && result.Data != null)
                {
                    return new SuccessDataResults<IzToplamV769>(result.Data, result.Message);
                }
                return new ErrorDataResults<IzToplamV769>(result?.Message ?? "Грешка при добавяне на ToplamV769");
            }
            catch (Exception ex)
            {
                return new ErrorDataResults<IzToplamV769>($"Грешка при добавяне: {ex.Message}");
            }
        }

        public DataResult<IzToplamV769> UpdateToplamV769(IzToplamV769 data)
        {
            try
            {
                if (data == null)
                {
                    return new ErrorDataResults<IzToplamV769>("Данните за актуализация са null!");
                }

                var result = _repo.GetRepository<IzToplamV769>().Update(data);
                if (result?.Success == true && result.Data != null)
                {
                    return new SuccessDataResults<IzToplamV769>(result.Data, result.Message);
                }
                return new ErrorDataResults<IzToplamV769>(result?.Message ?? "Грешка при актуализация на ToplamV769");
            }
            catch (Exception ex)
            {
                return new ErrorDataResults<IzToplamV769>($"Грешка при актуализация: {ex.Message}");
            }
        }
    }
}
