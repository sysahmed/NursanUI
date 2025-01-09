using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nursan.Domain.NursanBarcode;
using Nursan.Persistanse.UnitOfWork;


namespace Nursan.Persistanse.Extensions
{
    public class ConfigurationServices : IConfigurationFactory
    {
        private IConfiguration _configuration;
        private IUnitOfWorkKasa _unitOfWork;
        private DbContext uretimOtomasyonContext;
        public ConfigurationServices(IConfiguration configuration, IUnitOfWorkKasa unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = (UnitOfWork.UnitOfWorKasa?)unitOfWork;
        }
        public Microsoft.Extensions.Configuration.IConfiguration GetConfiguration()
        {
            return _configuration;
        }

        public IUnitOfWorkKasa GetUnitOfWorkKasa()
        {

            if (_unitOfWork == null)
                _unitOfWork = new UnitOfWorKasa(GetDbContext());
            return _unitOfWork;

        }
        public DbContext GetDbContext()
        {


            if (uretimOtomasyonContext == null)
            {
                uretimOtomasyonContext = new NbgUretim05Context();
            }
            return uretimOtomasyonContext;
        }
    }
}
