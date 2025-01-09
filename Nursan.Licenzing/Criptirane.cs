using System.Security.Cryptography;
using System.Text;

namespace Nursan.Licenzing
{
    public class Criptirane
    {
        public string KilitKilitleme(string clearText)
        {
            string pcname = Environment.MachineName;
            if (sqlGitBakLidansVarmi(pcname) != "LicenseOK")
            {
                string EncryptionKey = "abc123";
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            else
            {
                return "LicenseOK";
            }
        }
        public string sqlGitBakLidansVarmi(string pcname)
        {
            //LicenseDBDataContext rm = new LicenseDBDataContext();
            //try
            //{
            //    var idi = (from U in rm.usernms
            //               where U.pc == pcname
            //               select new
            //               {
            //                   U.pc,
            //                   U.License,
            //                   U.Tarih,
            //                   U.username
            //               }).FirstOrDefault();
            //    return idi.License;


            //}
            //catch (Exception)
            //{
            //    return "False";
            //}
            return "True";
        }
        public string KilitAcma(string cipherText1)
        {
            string cipherText;
            string EncryptionKey = "abc123";
            cipherText1 = cipherText1.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText1);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
