using Nursan.Validations.ProcessServise.ProcesManager;

namespace Nursan.Validations.ProcessServise.ProcessService
{
    public interface IBarcodeState
    {
        void Next(ProccessOrder process);
        void PreviousState(ProccessOrder process);
        void BackState(ProccessOrder process);

    }
}
