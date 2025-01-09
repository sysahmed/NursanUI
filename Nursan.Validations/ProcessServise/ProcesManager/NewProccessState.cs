using Nursan.Validations.ProcessServise.ProcessService;

namespace Nursan.Validations.ProcessServise.ProcesManager
{
    record NewProccessState : IBarcodeState
    {
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
