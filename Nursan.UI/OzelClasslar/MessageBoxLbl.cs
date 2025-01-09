using Nursan.Core.PcInterface;
using Nursan.Core.Printing;

namespace Nursan.UI.OzelClasslar
{
    internal class MessageBoxLbl
    {

        public static void MessageBas(string v, Color defaultForeColor, Label lblMessage)
        {
            lblMessage.Text = v; lblMessage.ForeColor = defaultForeColor;
            System.Threading.Timer t = new System.Threading.Timer(TimerCallback, lblMessage, 0, XMLIslemi.XmlSaniye());
            t.ConfigureAwait(true);
        }
        static void TimerCallback(Object o)
        {
            o = null;
        }

    }
}
