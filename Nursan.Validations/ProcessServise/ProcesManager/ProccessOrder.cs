using Nursan.Validations.ProcessServise.ProcessService;

namespace Nursan.Validations.ProcessServise.ProcesManager
{
    public class ProccessOrder
    {
        public IBarcodeState State { get; set; }

        public ProccessOrder()
        {
            State = new NewProccessState();
        }

        public void NextState()
        {
            State.Next(this);
        }

        public void PreviousState()
        {
            State.PreviousState(this);
        }

        public void BackState()
        {
            State.BackState(this);
        }
    }
}
