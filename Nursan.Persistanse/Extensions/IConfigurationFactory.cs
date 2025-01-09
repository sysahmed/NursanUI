

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nursan.Persistanse.UnitOfWork;


namespace Nursan.Persistanse.Extensions
{
    public interface IConfigurationFactory
    {
        IConfiguration GetConfiguration();
        IUnitOfWorkKasa GetUnitOfWorkKasa();
        DbContext GetDbContext();
    }
}
