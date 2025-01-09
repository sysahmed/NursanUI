using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;


namespace Nursan.Validations.ValidationCode
{
    public class DonanimService
    {
        protected UnitOfWork _repo;

        public DonanimService(UnitOfWork repo)
        {
            _repo = repo;
        }
        #region Alert
        public Nursan.Domain.Entity.OrAlert AddAlert(OrAlert alert)
        {
            var result = _repo.GetRepository<OrAlert>().Add(alert).Data;
            _repo.SaveChanges();
            return result;
        }

        public List<OrAlert> GetAlertAll()
        {
            return _repo.GetRepository<OrAlert>().GetAll(null).Data.ToList();
        }
        public void UpdateAlert(OrAlert alertim)
        {
            _repo.GetRepository<OrAlert>().Update(alertim);
            _repo.SaveChanges();
        }
        #endregion

        #region ALertBaglanti
        public void GitAlertBaglantiYap(OrAlertBaglanti baglanti)
        {
            var result = _repo.GetRepository<OrAlertBaglanti>().Add(baglanti).Data;
            _repo.SaveChanges();
        }
        public void GitAlertBaglantiGuncelle(OrAlertBaglanti baglanti)
        {
            var result = _repo.GetRepository<OrAlertBaglanti>().Update(baglanti).Data;
            _repo.SaveChanges();
        }
        public OrAlert GetAlert(string alert)
        {
            return _repo.GetRepository<OrAlert>().Get(x => x.Name == alert).Data;
        }
        public List<OrAlertBaglanti> GetAllAlertBaglanti(OrAlertBaglanti baglanti)
        {
            return _repo.GetRepository<OrAlertBaglanti>().GetAll(x => x.HarnessId == baglanti.HarnessId && x.AlertId == baglanti.AlertId).Data;
        }
        public List<OrAlertBaglanti> GetAllBaglanti()
        {
            return _repo.GetRepository<OrAlertBaglanti>().GetAll(null).Data;
        }
        public OrAlert GetAllAlertBaglanti(int harnessId)
        {
            try
            {
                var verues = _repo.GetRepository<OrAlert>().Get(x => x.AlertNumber == harnessId).Data;
                return verues;
            }
            catch (Exception ex)
            {

                return null;
            }


        }
        #endregion

        #region Family
        public List<OrFamily> GetFamily()
        {
            return _repo.GetRepository<OrFamily>().GetAll(null).Data.ToList();
        }
        public OrFamily AddFamily(OrFamily family)
        {
            var result = _repo.GetRepository<OrFamily>().Add(family).Data;
            _repo.SaveChanges();
            return result;
        }

        public List<OrFamily> GetAllFamily()
        {
            var result = _repo.GetRepository<OrFamily>().GetAll(null).Data;
            return result;
        }
        #endregion

        #region Harnes
        /// <summary>
        /// Harnes Instalation
        /// </summary>
        /// <returns></returns>
        /// 
        public OrHarnessModel GetDonanimHarnesOne(int id)
        {
            try
            {
                return _repo.GetRepository<OrHarnessModel>().GetById(id).Data;
            }
            catch (Exception)
            {
                return null;

            }
        }
        public List<OrHarnessModel> GetDonanimHarness()
        {
            try
            {
                return _repo.GetRepository<OrHarnessModel>().GetAll(null).Data;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public OrHarnessModel AddUrHarness(Nursan.Domain.Entity.OrHarnessModel urHarnessModel)
        {
            try
            {
                var result = _repo.GetRepository<OrHarnessModel>().Add(urHarnessModel).Data;
                _repo.SaveChanges();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void UpdateUrHarness(OrHarnessModel hrModel)
        {
            try
            {
                _repo.GetRepository<OrHarnessModel>().Update(hrModel);
                _repo.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
        public List<OrAlertBaglanti> GetBaglanti(int? referansID)
        {
            try
            {
                return _repo.GetRepository<OrAlertBaglanti>().GetAll(x => x.HarnessId == referansID).Data;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<OrHarnessConfig> GetConfigGetir()
        {
            try
            {
                return _repo.GetRepository<OrHarnessConfig>().GetAll(null).Data;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public OrHarnessConfig AddConfigGetir(OrHarnessConfig config)
        {
            try
            {
                return _repo.GetRepository<OrHarnessConfig>().Add(config).Data;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

    }
}
