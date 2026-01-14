using System.IO;
using System.Xml;
using Nursan.Domain.Entity;

namespace Nursan.API.Services
{
    /// <summary>
    /// Сервис за синхронизация на API Keys между базата данни и XML файлове
    /// </summary>
    public class XmlSyncService
    {
        private readonly ILogger<XmlSyncService> _logger;

        public XmlSyncService(ILogger<XmlSyncService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Записва API Key в XML файл за конкретно устройство
        /// Това се извиква когато API Key се генерира или регенерира от MVC View
        /// </summary>
        public async Task<bool> SaveApiKeyToXmlForDeviceAsync(string deviceId, string apiKey, string? uiAppPath = null)
        {
            try
            {
                // Ако не е предоставен път, опитваме се да намерим UI приложението
                // В production това може да се конфигурира
                string xmlPath;
                
                if (!string.IsNullOrEmpty(uiAppPath))
                {
                    xmlPath = Path.Combine(uiAppPath, "Baglanti.xml");
                }
                else
                {
                    // Опитваме се да намерим по стандартен път
                    // Това трябва да се конфигурира според средата
                    xmlPath = Path.Combine(@"D:\RepoDell\Nursan.UI\NursanUI\Nursan.UI", "Baglanti.xml");
                }

                if (!File.Exists(xmlPath))
                {
                    _logger.LogWarning($"XML файл не е намерен на пътя: {xmlPath}");
                    return false;
                }

                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                {
                    _logger.LogWarning("XML файлът няма 'config' node");
                    return false;
                }

                // Проверяваме дали вече има apiKey node за това устройство
                var apiKeyNode = configNode.SelectSingleNode($"apiKey[@DeviceId='{deviceId}']");
                
                if (apiKeyNode == null)
                {
                    // Създаваме нов apiKey node
                    apiKeyNode = doc.CreateElement("apiKey");
                    var valueAttr = doc.CreateAttribute("Value");
                    valueAttr.Value = apiKey;
                    apiKeyNode.Attributes.Append(valueAttr);
                    
                    var deviceIdAttr = doc.CreateAttribute("DeviceId");
                    deviceIdAttr.Value = deviceId;
                    apiKeyNode.Attributes.Append(deviceIdAttr);
                    
                    configNode.AppendChild(apiKeyNode);
                }
                else
                {
                    // Актуализираме съществуващия
                    if (apiKeyNode.Attributes["Value"] == null)
                    {
                        var valueAttr = doc.CreateAttribute("Value");
                        valueAttr.Value = apiKey;
                        apiKeyNode.Attributes.Append(valueAttr);
                    }
                    else
                    {
                        apiKeyNode.Attributes["Value"].Value = apiKey;
                    }
                }

                doc.Save(xmlPath);
                _logger.LogInformation($"API Key за устройство '{deviceId}' е записан в XML файл: {xmlPath}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Грешка при запис на API Key в XML за устройство '{deviceId}'");
                return false;
            }
        }

        /// <summary>
        /// Чете API Key от XML файл за конкретно устройство
        /// </summary>
        public string? GetApiKeyFromXmlForDevice(string deviceId, string? xmlPath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(xmlPath))
                {
                    xmlPath = Path.Combine(@"D:\RepoDell\Nursan.UI\NursanUI\Nursan.UI", "Baglanti.xml");
                }

                if (!File.Exists(xmlPath))
                    return null;

                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var apiKeyNode = doc.SelectSingleNode($"config/apiKey[@DeviceId='{deviceId}']");
                return apiKeyNode?.Attributes?["Value"]?.InnerText;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Грешка при четене на API Key от XML за устройство '{deviceId}'");
                return null;
            }
        }
    }
}

