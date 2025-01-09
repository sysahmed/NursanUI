using System.Xml;

namespace Nursan.Domain.SystemClass
{
    public class XMLSeverIp
    {
        public XMLSeverIp()
        {
        }

        public static String XmlServerIP()
        {
            try
            {
                XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("server");
                var result = xmlNodes.Attributes["Server"].InnerText;
                return result;
            }
            catch (Exception)
            {

                return string.Empty;
            }
        }
        public static String XmllServerIP()
        {
            try
            {
                XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("lserver");
                var result = xmlNodes.Attributes["LServer"].InnerText;
                return result;
            }
            catch (Exception)
            {

                return string.Empty;
            }
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
