using Microsoft.Win32;

namespace Nursan.Licenzing
{
    public class RegistrileriAra
    {
        public string regYaz(string date, string license)
        {
            Criptirane crp = new Criptirane();
            Microsoft.Win32.RegistryKey key;
            Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\NBGITTimBulder\\");
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\NBGITTimBulder\\Software");
            key.SetValue("SoftwareLicense", crp.KilitKilitleme(DateTime.Now.ToString("yyyy-MM-dd")));
            key.Close();
            //---------------------------------------------------------------------------------------------------------------
            Microsoft.Win32.RegistryKey key1;
            Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\NBGITTimBulder\\");
            key1 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\NBGITTimBulder\\License");
            key1.SetValue("License", license);
            key1.Close();
            //---------------------------------------------------------------------------------------------------------------
            Microsoft.Win32.RegistryKey key2;
            Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\NBGITTimBulder\\");
            key2 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\NBGITTimBulder\\Machine");
            key2.SetValue("MachinLicense", crp.KilitKilitleme(Environment.MachineName));
            key2.Close();
            return "hello";
        }

        public string regBac(string serial)
        {
            KilitYaratma klt = new KilitYaratma();
            Criptirane crp = new Criptirane();
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(@"Software\NBGITTimBulder\Software");
            RegistryKey registryKey1 = Registry.CurrentUser.OpenSubKey(@"Software\NBGITTimBulder\License");
            RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(@"Software\NBGITTimBulder\Machine");
            try
            {
                var key = (string)registryKey.GetValue("SoftwareLicense");
                var key1 = (string)registryKey1.GetValue("License");
                var key2 = (string)registryKey2.GetValue("MachinLicense");
                string license = key1.ToString();
                string user = crp.KilitAcma(key2.ToString());
                int admin = int.Parse(getLicense(crp.KilitAcma(key.ToString())));
                if (key1 != null && key != null && key2 != null)
                {
                    if (admin < 360 && license.TrimEnd() == serial.TrimEnd() && user.TrimEnd() == Environment.MachineName.TrimEnd())
                    {
                        return "yes!!!";
                    }
                    else
                    {
                        Deregister();
                        return klt.Value();
                    }
                }
                else
                {
                    Deregister();
                    return klt.Value();
                }
            }
            catch (Exception)
            {
                Deregister();
                return klt.Value();
            }
        }

        public void Deregister()
        {
            try
            {
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("Software\\NBGITTimBulder\\Software");
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("Software\\NBGITTimBulder\\License");
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKey("Software\\NBGITTimBulder\\Machine");
            }
            catch (Exception)
            {
            }
        }

        private string getLicense(string veri)
        {
            DateTime d1 = DateTime.Now;
            DateTime d2 = Convert.ToDateTime(veri).AddDays(-1);//DateTime.Now.AddDays(-180);
            TimeSpan t = d1 - d2;
            string gun = t.TotalDays.ToString().Substring(0, 3);
            //int res =  ( d1 - d2).TotalDays;
            return Math.Truncate(Convert.ToDecimal(gun)).ToString();
        }
    }
}
