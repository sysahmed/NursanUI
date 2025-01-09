namespace Nursan.Licenzing
{
    public class LisanGet
    {
        private KilitYaratma klt = new KilitYaratma();

        private Criptirane crp = new Criptirane();

        private RegistrileriAra reg = new RegistrileriAra();

        private Lisanslama ls = new Lisanslama();

        private FingerPrint fng = new FingerPrint();

        public LisanGet()
        {
        }

        public bool LisanBak(string proc)
        {
            bool flag;
            string str = DateTime.Now.Day.ToString();
            this.crp.KilitKilitleme(str);
            if (this.reg.regBac(this.klt.Value()) != "yes!!!")
            {
                this.ls.ShowDialog();
                flag = false;
            }
            else
            {
                flag = true;
            }
            return flag;
        }
    }
}
