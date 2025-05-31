using System.Xml;

namespace Nursan.XMLTools
{
    public class XMLSeverIp
    {
        public XMLSeverIp()
        {
        }
        public static bool ElTestCount()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("eltestcount");
            var result = xmlNodes.Attributes["EltestCount"].InnerText;
            return Convert.ToBoolean(result);
        }
        public static String XmlServerIP()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("server");
            var result = xmlNodes.Attributes["Server"].InnerText;
            return result;
        }
        public static int XmlSaniye()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("saniye");
            var result = xmlNodes.Attributes["Saniye"].InnerText;
            return Convert.ToInt16(result);
        }
        public static bool SayiGoster()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("sayi");
            var result = xmlNodes.Attributes["Sayi"].InnerText;
            return Convert.ToBoolean(result);
        }
    }
}
