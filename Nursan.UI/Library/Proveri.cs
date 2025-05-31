namespace Nursan.UI.Library
{
    public class Proveri //: ValidationCode
    {
        public Label MessageAyarla(string text, Color color, Label label)
        {
            label.Text = text;
            label.ForeColor = color;
            return label;
        }
    }
}
