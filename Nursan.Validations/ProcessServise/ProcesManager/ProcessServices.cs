using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.Validations.ProcessServise.ProcessService;

namespace Nursan.Validations.ProcessServise.ProcesManager
{
    public class ProcessServices : IProcessService
    {

        public async Task<SuccessDataResults<SyBarcodeInput>> ProcessAsync(List<SyBarcodeInput> entity)
        {
            //var procces = new ProccessOrder();
            //procces.NextState();
            foreach (var item in entity)
            {
                switch (item.Name)
                {
                    case "Sicil":

                        break;
                    case "First":

                        break;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}
