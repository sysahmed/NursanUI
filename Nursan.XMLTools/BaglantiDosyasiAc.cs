using System.Xml;

namespace Nursan.XMLTools
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
