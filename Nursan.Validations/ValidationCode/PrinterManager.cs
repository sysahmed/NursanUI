using Nursan.Domain.Entity;
using Nursan.Persistanse.UnitOfWork;

namespace Nursan.Validations.ValidationCode
{
    public class PrinterManager
    {
        private readonly UnitOfWork _repo;
        public PrinterManager(UnitOfWork repo)
        {
            _repo = repo;
        }
        public List<SyPrinter> GetAllPrinter()
        {
            return _repo.GetRepository<SyPrinter>().GetAll(null).Data;
        }
        public SyPrinter GetPrinter(string printername)
        {
            return _repo.GetRepository<SyPrinter>().Get(x => x.Name == printername).Data;
        }
        public bool AddPrinter(SyPrinter printer)
        {
            return _repo.GetRepository<SyPrinter>().Add(printer).Success;
        }
    }
}
