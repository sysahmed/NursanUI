using Nursan.Domain.Entity;
using Nursan.Logging.Messages;
using Nursan.Persistanse.Result;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Personal.Valadation
{
    public class PersonalValidasyonu
    {
        private readonly IUnitOfWorkPersonal _repoPersonal;
        private readonly IUnitOfWork _repo;
        public PersonalValidasyonu(IUnitOfWorkPersonal repoPersonal, IUnitOfWork repo)
        {
            _repoPersonal = repoPersonal;
            _repo = repo;
        }
        //private IEnumerable<Nursan.Domain.Entity.UrPersonalTakib> PersonalTakib;
        //private IEnumerable<Nursan.Domain.Personal.Personal> Personals;

        public IDataResult<List<Nursan.Domain.Entity.UrPersonalTakib>> GetPersonalTakib(UrIstasyon urIstasyon)
        {
            try
            {
                return _repo.GetRepository<Nursan.Domain.Entity.UrPersonalTakib>().GetAll(x => x.UrIstasyonId == urIstasyon.Id);//
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz("GetPersonalTakib:" + ex.Message);
                return new SuccessDataResults<List<Nursan.Domain.Entity.UrPersonalTakib>>(ex.Message);
            }
        }
        public IDataResult<Nursan.Domain.Entity.UrPersonalTakib> GetPersonalAndSicilTakib(UrIstasyon urIstasyon, String sicil)
        {
            try
            {
                return _repo.GetRepository<Nursan.Domain.Entity.UrPersonalTakib>().Get(x => x.Sicil == sicil && x.UrIstasyonId == urIstasyon.Id);
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz("GetPersonalAndSicilTakib:" + ex.Message);
                return new SuccessDataResults<Nursan.Domain.Entity.UrPersonalTakib>(ex.Message);
            }

        }
        public IDataResult<UrPersonalTakib> GetPersonalAndSicilTakibTek(String sicil)
        {
            try
            {
                return _repo.GetRepository<UrPersonalTakib>().Get(x => x.Sicil == sicil);
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz("GetPersonalAndSicilTakibTek:" + ex.Message);
                return new SuccessDataResults<UrPersonalTakib>(ex.Message);
            }

        }
        public IDataResult<Nursan.Domain.Personal.Personal> GetPersonalGromCardID(string cardId)
        {
            try
            {
                var result = _repoPersonal.GetPersonalRepository<Nursan.Domain.Personal.Personal>().Get(x => x.CARD_ID == int.Parse(cardId));
                return result;
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz("GetPersonalGromCardID:" + ex.Message);
                return new SuccessDataResults<Nursan.Domain.Personal.Personal>(ex.Message);
            }
        }
        public IDataResult<Nursan.Domain.Personal.Personal> GetPersonal(string sicilId)
        {
            try
            {
                var result = _repoPersonal.GetPersonalRepository<Nursan.Domain.Personal.Personal>().Get(x => x.USER_CODE == sicilId);
                return result;
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz("GetPersonal:" + ex.Message);
                return new SuccessDataResults<Nursan.Domain.Personal.Personal>(ex.Message);
            }
        }
        public void ADDPersonalTakib(UrPersonalTakib pTakib)
        {
            try
            {
                var result = _repo.GetRepository<UrPersonalTakib>().Add(pTakib);
                // return new SuccessDataResults<bool>(result.Success, result.Message);
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz("ADDPersonalTakib:" + ex.Message);
                //return new SuccessDataResults<bool>(false, ex.Message);
            }
        }
        public void UpdatePersonalTakib(UrPersonalTakib pTakib) => _repo.GetRepository<UrPersonalTakib>().Update(pTakib);
        public SuccessDataResults<bool> DeletePersonalTakib(UrPersonalTakib pers, string veriler)
        {
            try
            {
                var result = _repo.GetRepository<UrPersonalTakib>().Delete(pers);
                return new SuccessDataResults<bool>(result.Success, result.Message);
            }
            catch (Exception ex)
            {
                Messaglama.MessagYaz("DeletePersonalTakib:" + ex.Message);
                return new SuccessDataResults<bool>(false, ex.Message);
            }
        }
    }
}
