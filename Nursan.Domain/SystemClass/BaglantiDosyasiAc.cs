using System.Xml;

namespace Nursan.Domain.SystemClass
{
    public static class BaglantiDosyasiAc
    {
        public static XmlDocument GitDosyaAc()
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load("Baglanti.xml");
            }
            catch (Exception)
            {

            }
            return xmlDocument;
        }
    }
}
