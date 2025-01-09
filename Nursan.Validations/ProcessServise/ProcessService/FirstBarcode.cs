using Nursan.Domain.Entity;
using Nursan.Validations.ProcessServise.ProcesManager;

namespace Nursan.Validations.ProcessServise.ProcessService
{
    record FirstBarcode : IBarcodeState
    {
        // List<SyBarcodeInput> _barcode;
        public FirstBarcode(List<SyBarcodeInput> barcode)
        {
            // _barcode = barcode;
        }
        public void BackState(ProccessOrder process)
        {
            process.BackState();
        }
        public void Next(ProccessOrder process)
        {
            process.NextState();
        }
        public void PreviousState(ProccessOrder process)
        {
            process.PreviousState();
        }
    }
}
