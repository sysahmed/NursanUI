using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;

namespace Nursan.Validations.ProcessServise.ProcessService
{
    public interface IProcessService
    {
        Task<SuccessDataResults<SyBarcodeInput>> ProcessAsync(List<SyBarcodeInput> entity);
    }
}
