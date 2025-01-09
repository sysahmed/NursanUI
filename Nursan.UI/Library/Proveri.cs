namespace Nursan.UI.Library
{
    public class Proveri //: ValidationCode
    {
        //public readonly struct KeyValuePair<TKey, TValue> { }
        //public Proveri(string gelenBarcode) : base(new Nursan.Persistanse.UnitOfWork.UnitOfWork(new UretimContext()), gelenBarcode)
        //{

        //}
        public Label MessageAyarla(string text, Color color, Label label)
        {
            //UretimContext uretimContext = new();
            //var veri = uretimContext.OrHarnessModels.FromSqlInterpolated($"select * from OrHarnessModel");
            label.Text = text;
            label.ForeColor = color;
            return label;
        }
    }
}
