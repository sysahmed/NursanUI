
using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.ValidationCode
{
    public class SqlValidation
    {
        protected UnitOfWork _repo;
        public SqlValidation(UnitOfWork repo)
        {
            _repo = repo;
        }
        #region Alert
        public void GetAlert(int Id)
        {
            var item = _repo.GetRepository<OrAlert>().GetById(Id);
        }
        #endregion

        #region Family
        public OrFamily GetFamily(string familyName)
        {
            var result = _repo.GetRepository<OrFamily>().Get(x => x.FamilyName.Equals(familyName,StringComparison.OrdinalIgnoreCase)).Data;
            return result;
        }
        #endregion

        #region HarnesDonanim
        public OrHarnessModel GetHarnessModel(OrHarnessModel harnessModel)
        {
            var result = _repo.GetRepository<OrHarnessModel>().Get(x => x.HarnessModelName == harnessModel.HarnessModelName).Data;
            return result;
        }
        public OrHarnessModel GetHarnessModelID(int ID)
        {
            var result = _repo.GetRepository<OrHarnessModel>().Get(x => x.Id == ID).Data;
            return result;
        }
        #endregion
    }
}
