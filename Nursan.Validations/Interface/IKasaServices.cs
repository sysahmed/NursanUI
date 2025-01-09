using Nursan.Persistanse.Repository;

namespace Nursan.Validations.Interface
{
    public interface IKasaServices<T> : INewRepository<T> where T : class
    {
       
    }
}
