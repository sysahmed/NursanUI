using System.Collections.Generic;
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
        public static bool WebApiTrue()
        {
            try
            {
                XmlDocument doc = BaglantiDosyasiAc.GitDosyaAc();
                if (doc == null)
                {
                    Console.WriteLine("XMLSeverIp.WebApiTrue: XML документът е null!");
                    return false;
                }

                XmlNode configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                {
                    Console.WriteLine("XMLSeverIp.WebApiTrue: config node е null!");
                    return false;
                }

                XmlNode xmlNodes = configNode.SelectSingleNode("webapitrue");
                if (xmlNodes == null)
                {
                    Console.WriteLine("XMLSeverIp.WebApiTrue: webapitrue node е null!");
                    return false;
                }

                if (xmlNodes.Attributes == null || xmlNodes.Attributes["WebApiTrue"] == null)
                {
                    Console.WriteLine("XMLSeverIp.WebApiTrue: WebApiTrue attribute е null!");
                    return false;
                }

                string result = xmlNodes.Attributes["WebApiTrue"].InnerText;
                bool webApiTrue = Convert.ToBoolean(result);
                Console.WriteLine($"XMLSeverIp.WebApiTrue: {webApiTrue}");
                return webApiTrue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XMLSeverIp.WebApiTrue грешка: {ex.Message}");
                return false;
            }
        }
        public static String XmlServerIP()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("server");
            var result = xmlNodes.Attributes["Server"].InnerText;
            return result;
        }
        public static String XmlWebApiIP()
        {
            XmlNode xmlNodes = BaglantiDosyasiAc.GitDosyaAc().SelectSingleNode("config").SelectSingleNode("webapi");
            return xmlNodes.Attributes["WebApi"].InnerText;
        }

        /// <summary>
        /// Взема Master API Address от XML (за всички API заявки, освен тикетите)
        /// Това е основният API адрес за ElTest, Tork и други бизнес операции
        /// Тикетите имат свой отделен адрес в ticketTracking node
        /// Връща само адреса (без протокол), например: "localhost:5226" или "10.168.0.252:5226"
        /// </summary>
        public static String XmlMasterApiAddress()
        {
            try
            {
                XmlDocument doc = BaglantiDosyasiAc.GitDosyaAc();
                if (doc == null)
                    return "localhost:5226"; // Fallback

                XmlNode configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                    return "localhost:5226"; // Fallback

                // Опитваме се да вземем masterApiAddress
                XmlNode masterApiNode = configNode.SelectSingleNode("masterApiAddress");
                if (masterApiNode != null && masterApiNode.Attributes?["Address"] != null)
                {
                    string address = masterApiNode.Attributes["Address"].InnerText.Trim();
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        // Премахваме протокол ако е включен (http:// или https://)
                        if (address.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
                        {
                            address = address.Substring(7); // Премахваме "http://"
                        }
                        else if (address.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                        {
                            address = address.Substring(8); // Премахваме "https://"
                        }
                        
                        // Премахваме наклонена черта в началото и накрая (ако има)
                        address = address.TrimStart('/').TrimEnd('/');
                        
                        return address;
                    }
                }

                // Fallback към localhost за development
                return "localhost:5226";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XMLSeverIp.XmlMasterApiAddress грешка: {ex.Message}");
                return "localhost:5226"; // Fallback
            }
        }

        /// <summary>
        /// Взема Video API Address от XML (за видео API заявки)
        /// Това е отделен API адрес за видео системата
        /// Връща пълния URL с протокол, например: "http://localhost:12012" или "https://10.168.0.252:12012"
        /// </summary>
        public static String XmlVideoApiAddress()
        {
            try
            {
                XmlDocument doc = BaglantiDosyasiAc.GitDosyaAc();
                if (doc == null)
                    return "http://localhost:12012"; // Fallback

                XmlNode configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                    return "http://localhost:12012"; // Fallback

                // Опитваме се да вземем videoApiAddress
                XmlNode videoApiNode = configNode.SelectSingleNode("videoApiAddress");
                if (videoApiNode != null && videoApiNode.Attributes?["Address"] != null)
                {
                    string address = videoApiNode.Attributes["Address"].InnerText.Trim();
                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        // Премахваме наклонена черта в началото и накрая (ако има)
                        address = address.TrimStart('/').TrimEnd('/');
                        
                        // Ако адресът вече има протокол, връщаме го директно
                        if (address.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                            address.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                        {
                            return address;
                        }
                        
                        // Ако няма протокол, добавяме http:// (за обратна съвместимост)
                        // Използваме HTTP вместо HTTPS за да избегнем SSL сертификатни проблеми с WebBrowser
                        return $"http://{address}";
                    }
                }

                // Fallback към http://localhost:12012 за development
                return "http://localhost:12012";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XMLSeverIp.XmlVideoApiAddress грешка: {ex.Message}");
                return "http://localhost:12012"; // Fallback
            }
        }

        /// <summary>
        /// Взема Video API Token от XML (за автентикация на видео API заявки)
        /// </summary>
        public static String? XmlVideoApiToken()
        {
            try
            {
                XmlDocument doc = BaglantiDosyasiAc.GitDosyaAc();
                if (doc == null)
                    return null;

                XmlNode configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                    return null;

                XmlNode videoApiNode = configNode.SelectSingleNode("videoApiAddress");
                if (videoApiNode != null && videoApiNode.Attributes?["Token"] != null)
                {
                    string token = videoApiNode.Attributes["Token"].InnerText.Trim();
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        return token;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XMLSeverIp.XmlVideoApiToken грешка: {ex.Message}");
                return null;
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

        /// <summary>
        /// Взема API Key от XML конфигурацията
        /// </summary>
        public static string? GetApiKey()
        {
            try
            {
                XmlDocument doc = BaglantiDosyasiAc.GitDosyaAc();
                if (doc == null)
                    return null;

                XmlNode configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                    return null;

                XmlNode apiKeyNode = configNode.SelectSingleNode("apiKey");
                return apiKeyNode?.Attributes?["Value"]?.InnerText;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Връща списък с разрешените тикет ID-та от Baglanti.xml.
        /// Празен списък означава, че няма ограничение и всички тикети са видими.
        /// </summary>
        public static HashSet<int> VisibleTicketTypeIds()
        {
            HashSet<int> visibleIds = new HashSet<int>();
            try
            {
                XmlDocument document = BaglantiDosyasiAc.GitDosyaAc();
                XmlNode configNode = document?.SelectSingleNode("config");
                XmlNode visibilityNode = configNode?.SelectSingleNode("ticketVisibility");

                string? rawValue = visibilityNode?.Attributes?["VisibleIds"]?.InnerText;
                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    return visibleIds;
                }

                char[] separators = new[] { ',', ';', '|', ' ' };
                string[] parts = rawValue.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    if (int.TryParse(part.Trim(), out int id))
                    {
                        visibleIds.Add(id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XMLSeverIp.VisibleTicketTypeIds грешка: {ex.Message}");
            }

            return visibleIds;
        }
    }
}
