using Nursan.Core.PcInterface;
using Nursan.XMLTools;
using System.Xml;

namespace Nursan.Core.Printing
{
    public class XMLIslemi
    {
        public XMLIslemi()
        {
        }

        public static string XmlDensity()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("density");
            return xmlNodes.Attributes["Density"].InnerText.ToString();
        }

        //public int XmlSaniye()
        //{
        //    BaglantiDosyasiAc.GitDosyaAc() BaglantiDosyasiAc.GitDosyaAc() = new BaglantiDosyasiAc.GitDosyaAc()();
        //    BaglantiDosyasiAc.GitDosyaAc().Load("Baglanti.xml");
        //    XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("saniye");
        //    return int.Parse(xmlNodes.Attributes["Saniye"].InnerText.ToString());
        //}
        public static string XmlFormOku()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("form");
            return xmlNodes.Attributes["Form"].InnerText.ToString();
        }

        public static string XmlCongigOku()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("form");
            return xmlNodes.Attributes["Form"].InnerText.ToString();
        }

        public static string XmlPLSOku()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config");
            return xmlNodes.Attributes["PLS"].InnerText.ToString();
        }

        public static string XmlPrintOku()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config");
            return xmlNodes.Attributes["print"].InnerText.ToString();
        }

        public XmlDosya XmlRakkamOku()
        {
            XmlDosya xmlDosya = new XmlDosya();

            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config");
            xmlDosya.hata = int.Parse(xmlNodes.Attributes["Hata"].InnerText);
            if (xmlDosya.hata != 1)
            {
                xmlDosya.sicil = xmlNodes.Attributes["sicil"].InnerText;
            }
            else
            {
                xmlDosya.id = int.Parse(xmlNodes.Attributes["id"].InnerText);
                xmlDosya.tarih = new DateTime?(DateTime.Parse(xmlNodes.Attributes["tarih"].InnerText));
                xmlDosya.sicil = xmlNodes.Attributes["sicil"].InnerText;
                xmlSicil(xmlDosya);
            }
            return xmlDosya;
        }

        public static String XmlServerIP()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("server");
            return xmlNodes.Attributes["server"].InnerText;
        }

        public static int XmlSaniye()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("saniye");
            return int.Parse(xmlNodes.Attributes["Saniye"].InnerText.ToString().PadRight(4, '0'));
        }
        public static int XmlScreenSaniye()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("screenSaniye");
            return int.Parse(xmlNodes.Attributes["ScreenSaniye"].InnerText.ToString().PadRight(4, '0'));
        }

        public bool xmlSicil(XmlDosya xmlcik)
        {
            XmlNode str = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config");
            str.Attributes["id"].InnerText = xmlcik.id.ToString();
            str.Attributes["tarih"].InnerText = xmlcik.tarih.ToString();
            str.Attributes["sicil"].InnerText = xmlcik.sicil;
            str.Attributes["Hata"].InnerText = xmlcik.hata.ToString();
            BaglantiDosyasiAc.GitDosyaAc().Save("Baglanti.xml");
            return true;
        }

        public void xmlSifirla()
        {
            XmlRakkamOku();

            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config");
            xmlNodes.Attributes["id"].InnerText = "0";
            xmlNodes.Attributes["sicil"].InnerText = "0";
            xmlNodes.Attributes["Hata"].InnerText = "0";
            xmlNodes.Attributes["tarih"].InnerText = "0";
            BaglantiDosyasiAc.GitDosyaAc().Save("Baglanti.xml");
        }

        public XmlDosya xmlTolamRakkam(DateTime dtarih, string sicil, int hata, int id)
        {
            XmlRakkamOku();

            XmlNode str = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config");
            str.Attributes["id"].InnerText = id.ToString();
            str.Attributes["sicil"].InnerText = sicil;
            str.Attributes["Hata"].InnerText = hata.ToString();
            str.Attributes["tarih"].InnerText = dtarih.ToString();
            BaglantiDosyasiAc.GitDosyaAc().Save("Baglanti.xml");
            return XmlRakkamOku();
        }
    }
}