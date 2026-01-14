using System.Xml;
using System.IO;

namespace Nursan.XMLTools
{
    public static class BaglantiDosyasiAc
    {
        public static XmlDocument GitDosyaAc()
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                xmlDocument.Load(xmlPath);
            }
            catch (Exception)
            {

            }
            return xmlDocument;
        }
    }
}
