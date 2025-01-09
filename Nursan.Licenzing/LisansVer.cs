namespace Nursan.Licenzing
{
    public class LisansVer
    {
        public bool LisansVarmi()
        {
            Lisanslama lisan = new Lisanslama();
            Criptirane crp = new Criptirane();
            string date = DateTime.Now.ToString("yyy-MM-dd");
            string kilit = crp.KilitKilitleme(date);
            if (kilit == "LicenseOK")
            {
                return true;
            }
            else
            {
                lisan.ShowDialog();
                return false;
            }
        }
    }
}
