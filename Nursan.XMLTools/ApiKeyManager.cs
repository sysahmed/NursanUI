using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Nursan.XMLTools
{
    /// <summary>
    /// Мениджър за работа с API Key в XML конфигурацията
    /// </summary>
    public static class ApiKeyManager
    {
        private const string EncryptionMarkerDpapi = "dpapi";
        private const string EncryptionScopeLocalMachine = "localmachine";

        /// <summary>
        /// Ако apiKey в Baglanti.xml е записан в стар (plaintext) формат, презаписва го в DPAPI (LocalMachine) формат.
        /// Това е еднопосочна миграция: след нея Value става Base64 DPAPI blob + Enc/Scope атрибути.
        /// </summary>
        public static bool TryMigratePlaintextApiKeyToDpapi(string? deviceId = null)
        {
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                if (!File.Exists(xmlPath))
                {
                    return false;
                }

                var doc = new XmlDocument();
                doc.Load(xmlPath);

                XmlNode? configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                {
                    return false;
                }

                XmlNode? apiKeyNode = null;
                if (!string.IsNullOrEmpty(deviceId))
                {
                    apiKeyNode = configNode.SelectSingleNode($"apiKey[@DeviceId='{deviceId}']");
                }

                if (apiKeyNode == null)
                {
                    apiKeyNode = configNode.SelectSingleNode("apiKey");
                }

                if (apiKeyNode == null)
                {
                    return false;
                }

                string? enc = apiKeyNode.Attributes?["Enc"]?.InnerText;
                if (!string.IsNullOrEmpty(enc) && enc.Equals(EncryptionMarkerDpapi, StringComparison.OrdinalIgnoreCase))
                {
                    // Вече е DPAPI криптирано
                    return false;
                }

                string? value = apiKeyNode.Attributes?["Value"]?.InnerText;
                if (string.IsNullOrEmpty(value))
                {
                    return false;
                }

                // Ако не е маркирано като DPAPI, приемаме че е plaintext и го презаписваме чрез SaveApiKey (което ще го криптира и ще добави Enc/Scope)
                bool isActive = true;
                string? isActiveRaw = apiKeyNode.Attributes?["IsActive"]?.InnerText;
                if (!string.IsNullOrEmpty(isActiveRaw) && isActiveRaw.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    isActive = false;
                }

                return SaveApiKey(value, deviceId, isActive);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Генерира нов сигурен API Key
        /// </summary>
        public static string GenerateApiKey()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[32]; // 256 бита
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes)
                    .Replace("+", "-")
                    .Replace("/", "_")
                    .TrimEnd('=');
            }
        }

        /// <summary>
        /// Взема API Key от XML конфигурацията (само ако е активен)
        /// Поддържа и DeviceId за по-нова версия
        /// </summary>
        public static string? GetApiKey()
        {
            try
            {
                var doc = BaglantiDosyasiAc.GitDosyaAc();
                
                // Първо опитваме с DeviceId (по-нова версия)
                var deviceId = GetDeviceId();
                if (!string.IsNullOrEmpty(deviceId))
                {
                    var node = doc?.SelectSingleNode($"config/apiKey[@DeviceId='{deviceId}']");
                    if (node != null)
                    {
                        var isActiveAttr = node.Attributes?["IsActive"];
                        if (isActiveAttr == null || isActiveAttr.Value != "false")
                        {
                            return ReadApiKeyValueWithSelfHeal(node);
                        }
                    }
                }
                
                // Fallback към стария формат (без DeviceId)
                var nodeOld = doc?.SelectSingleNode("config/apiKey");
                if (nodeOld == null)
                    return null;

                // Проверяваме дали ключът е активен
                var isActiveAttrOld = nodeOld.Attributes?["IsActive"];
                if (isActiveAttrOld != null && isActiveAttrOld.Value == "false")
                    return null; // Ключът е деактивиран

                return ReadApiKeyValueWithSelfHeal(nodeOld);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Взема DeviceId от XML или генерира уникален на базата на машината
        /// </summary>
        public static string? GetDeviceId()
        {
            try
            {
                var doc = BaglantiDosyasiAc.GitDosyaAc();

                // 1) Ако има поне един apiKey node с DeviceId, взимаме първия такъв.
                // Това избягва случаи, в които има повече от един apiKey и първият няма DeviceId (legacy).
                var nodeWithDeviceId = doc?.SelectSingleNode("config/apiKey[@DeviceId and string-length(@DeviceId) > 0]");
                var deviceIdAttr = nodeWithDeviceId?.Attributes?["DeviceId"];
                if (deviceIdAttr != null && !string.IsNullOrEmpty(deviceIdAttr.Value))
                {
                    return deviceIdAttr.Value;
                }

                // 2) Fallback към първия apiKey node (ако има)
                var node = doc?.SelectSingleNode("config/apiKey");
                var deviceIdAttrOld = node?.Attributes?["DeviceId"];
                if (deviceIdAttrOld != null && !string.IsNullOrEmpty(deviceIdAttrOld.Value))
                {
                    return deviceIdAttrOld.Value;
                }
                
                // Ако няма DeviceId в XML, генерираме на базата на машината
                return GenerateDeviceIdFromMachine();
            }
            catch
            {
                return GenerateDeviceIdFromMachine();
            }
        }

        /// <summary>
        /// Генерира уникален DeviceId на базата на машината
        /// </summary>
        private static string GenerateDeviceIdFromMachine()
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                sb.Append(Environment.MachineName);
                sb.Append('|');
                sb.Append(Environment.UserDomainName);
                sb.Append('|');
                
                // Опитваме се да вземем MAC адрес
                try
                {
                    var interfaces = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
                    var macAddress = interfaces
                        .Where(nic => nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up 
                                   && nic.NetworkInterfaceType != System.Net.NetworkInformation.NetworkInterfaceType.Loopback)
                        .OrderByDescending(nic => nic.Speed)
                        .Select(nic => nic.GetPhysicalAddress().ToString())
                        .FirstOrDefault();
                    
                    if (!string.IsNullOrEmpty(macAddress))
                    {
                        sb.Append(macAddress);
                    }
                }
                catch
                {
                    // Игнорираме грешката
                }
                
                // Създаваме hash за уникален идентификатор
                using (var sha = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                    var hash = sha.ComputeHash(bytes);
                    return Convert.ToHexString(hash).Substring(0, 16); // Първите 16 символа
                }
            }
            catch
            {
                // Fallback към MachineName
                return Environment.MachineName;
            }
        }

        /// <summary>
        /// Взема статус на API Key (активен/неактивен, дата на създаване)
        /// </summary>
        public static ApiKeyStatus? GetApiKeyStatus()
        {
            try
            {
                var doc = BaglantiDosyasiAc.GitDosyaAc();
                var node = doc?.SelectSingleNode("config/apiKey");
                if (node == null)
                    return null;

                var value = node.Attributes?["Value"]?.InnerText;
                var isActiveAttr = node.Attributes?["IsActive"];
                var createdDateAttr = node.Attributes?["CreatedDate"];

                bool isActive = isActiveAttr == null || isActiveAttr.Value != "false";
                DateTime? createdDate = null;
                
                if (createdDateAttr != null && DateTime.TryParse(createdDateAttr.Value, out var parsedDate))
                {
                    createdDate = parsedDate;
                }

                return new ApiKeyStatus
                {
                    Value = value,
                    IsActive = isActive,
                    CreatedDate = createdDate
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Проверява дали API Key е активен
        /// </summary>
        public static bool IsApiKeyActive()
        {
            var status = GetApiKeyStatus();
            return status?.IsActive ?? false;
        }

        /// <summary>
        /// Записва API Key в XML конфигурацията (автоматично го активира)
        /// </summary>
        public static bool SaveApiKey(string apiKey, string? deviceId = null, bool isActive = true)
        {
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                
                if (!File.Exists(xmlPath))
                {
                    return false;
                }

                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                {
                    return false;
                }

                // Ако има DeviceId, търсим по него
                XmlNode? apiKeyNode = null;
                if (!string.IsNullOrEmpty(deviceId))
                {
                    apiKeyNode = configNode.SelectSingleNode($"apiKey[@DeviceId='{deviceId}']");
                }
                
                // Ако не е намерен, опитваме се без DeviceId
                if (apiKeyNode == null)
                {
                    apiKeyNode = configNode.SelectSingleNode("apiKey");
                }
                
                if (apiKeyNode == null)
                {
                    // Създаваме нов apiKey node
                    apiKeyNode = doc.CreateElement("apiKey");
                    var valueAttr = doc.CreateAttribute("Value");
                    valueAttr.Value = ProtectApiKeyValueDpapiLocalMachine(apiKey);
                    apiKeyNode.Attributes.Append(valueAttr);

                    var encAttr = doc.CreateAttribute("Enc");
                    encAttr.Value = EncryptionMarkerDpapi;
                    apiKeyNode.Attributes.Append(encAttr);

                    var scopeAttr = doc.CreateAttribute("Scope");
                    scopeAttr.Value = EncryptionScopeLocalMachine;
                    apiKeyNode.Attributes.Append(scopeAttr);
                    
                    if (!string.IsNullOrEmpty(deviceId))
                    {
                        var deviceIdAttr = doc.CreateAttribute("DeviceId");
                        deviceIdAttr.Value = deviceId;
                        apiKeyNode.Attributes.Append(deviceIdAttr);
                    }
                    
                    var isActiveAttr = doc.CreateAttribute("IsActive");
                    isActiveAttr.Value = isActive.ToString().ToLower();
                    apiKeyNode.Attributes.Append(isActiveAttr);
                    
                    var createdDateAttr = doc.CreateAttribute("CreatedDate");
                    createdDateAttr.Value = DateTime.UtcNow.ToString("o");
                    apiKeyNode.Attributes.Append(createdDateAttr);
                    
                    configNode.AppendChild(apiKeyNode);
                }
                else
                {
                    // Актуализираме съществуващия
                    if (apiKeyNode.Attributes["Value"] == null)
                    {
                        var valueAttr = doc.CreateAttribute("Value");
                        valueAttr.Value = ProtectApiKeyValueDpapiLocalMachine(apiKey);
                        apiKeyNode.Attributes.Append(valueAttr);
                    }
                    else
                    {
                        apiKeyNode.Attributes["Value"].Value = ProtectApiKeyValueDpapiLocalMachine(apiKey);
                    }

                    // Маркираме като DPAPI криптирано
                    if (apiKeyNode.Attributes["Enc"] == null)
                    {
                        var encAttr = doc.CreateAttribute("Enc");
                        encAttr.Value = EncryptionMarkerDpapi;
                        apiKeyNode.Attributes.Append(encAttr);
                    }
                    else
                    {
                        apiKeyNode.Attributes["Enc"].Value = EncryptionMarkerDpapi;
                    }

                    if (apiKeyNode.Attributes["Scope"] == null)
                    {
                        var scopeAttr = doc.CreateAttribute("Scope");
                        scopeAttr.Value = EncryptionScopeLocalMachine;
                        apiKeyNode.Attributes.Append(scopeAttr);
                    }
                    else
                    {
                        apiKeyNode.Attributes["Scope"].Value = EncryptionScopeLocalMachine;
                    }

                    // Актуализираме IsActive
                    if (apiKeyNode.Attributes["IsActive"] == null)
                    {
                        var isActiveAttr = doc.CreateAttribute("IsActive");
                        isActiveAttr.Value = isActive.ToString().ToLower();
                        apiKeyNode.Attributes.Append(isActiveAttr);
                    }
                    else
                    {
                        apiKeyNode.Attributes["IsActive"].Value = isActive.ToString().ToLower();
                    }

                    // Актуализираме CreatedDate само ако не съществува
                    if (apiKeyNode.Attributes["CreatedDate"] == null)
                    {
                        var createdDateAttr = doc.CreateAttribute("CreatedDate");
                        createdDateAttr.Value = DateTime.UtcNow.ToString("o");
                        apiKeyNode.Attributes.Append(createdDateAttr);
                    }
                }

                // Записваме промените
                doc.Save(xmlPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string? ReadApiKeyValueWithSelfHeal(XmlNode apiKeyNode)
        {
            string? value = apiKeyNode?.Attributes?["Value"]?.InnerText;
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            string? enc = apiKeyNode?.Attributes?["Enc"]?.InnerText;
            if (!string.IsNullOrEmpty(enc) && enc.Equals(EncryptionMarkerDpapi, StringComparison.OrdinalIgnoreCase))
            {
                // Опитваме DPAPI декриптиране (LocalMachine).
                // Ако Value не е валиден Base64/DPAPI blob (например плейн ключ е бил записан с Enc="dpapi"),
                // връщаме плейн стойността и се опитваме да я "поправим" (да я запишем правилно като DPAPI).
                string? decrypted = UnprotectApiKeyValueDpapiLocalMachine(value);
                if (!string.IsNullOrEmpty(decrypted))
                {
                    return decrypted;
                }

                // Self-heal: приемаме че текущото Value е плейн ключ и го презаписваме криптирано
                string? nodeDeviceId = apiKeyNode?.Attributes?["DeviceId"]?.InnerText;
                bool isActive = true;
                string? isActiveRaw = apiKeyNode?.Attributes?["IsActive"]?.InnerText;
                if (!string.IsNullOrEmpty(isActiveRaw) && isActiveRaw.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    isActive = false;
                }

                // Записваме плейн стойността правилно като DPAPI (LocalMachine)
                // Ако записът не успее, пак връщаме плейн стойността, за да може приложението да работи.
                SaveApiKey(value, nodeDeviceId, isActive);
                return value;
            }

            // Legacy plaintext
            return value;
        }

        /// <summary>
        /// Криптира API key за съхранение в Baglanti.xml чрез Windows DPAPI (LocalMachine).
        /// </summary>
        private static string ProtectApiKeyValueDpapiLocalMachine(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] protectedBytes = ProtectedData.Protect(plainBytes, null, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(protectedBytes);
        }

        /// <summary>
        /// Декриптира DPAPI стойност от Baglanti.xml чрез Windows DPAPI (LocalMachine).
        /// </summary>
        private static string? UnprotectApiKeyValueDpapiLocalMachine(string base64CipherText)
        {
            try
            {
                byte[] protectedBytes = Convert.FromBase64String(base64CipherText);
                byte[] plainBytes = ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(plainBytes);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Деактивира API Key (revoke)
        /// </summary>
        public static bool RevokeApiKey()
        {
            return SetApiKeyActiveStatus(false);
        }

        /// <summary>
        /// Активира API Key
        /// </summary>
        public static bool ActivateApiKey()
        {
            return SetApiKeyActiveStatus(true);
        }

        /// <summary>
        /// Задава статус на API Key (активен/неактивен)
        /// </summary>
        private static bool SetApiKeyActiveStatus(bool isActive)
        {
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                if (!File.Exists(xmlPath))
                    return false;

                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                    return false;

                var apiKeyNode = configNode.SelectSingleNode("apiKey");
                if (apiKeyNode == null)
                    return false;

                // Актуализираме IsActive
                if (apiKeyNode.Attributes["IsActive"] == null)
                {
                    var isActiveAttr = doc.CreateAttribute("IsActive");
                    isActiveAttr.Value = isActive.ToString().ToLower();
                    apiKeyNode.Attributes.Append(isActiveAttr);
                }
                else
                {
                    apiKeyNode.Attributes["IsActive"].Value = isActive.ToString().ToLower();
                }

                doc.Save(xmlPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Изтрива API Key от XML конфигурацията
        /// </summary>
        public static bool DeleteApiKey()
        {
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                if (!File.Exists(xmlPath))
                    return false;

                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                    return false;

                var apiKeyNode = configNode.SelectSingleNode("apiKey");
                if (apiKeyNode == null)
                    return false;

                configNode.RemoveChild(apiKeyNode);
                doc.Save(xmlPath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Генерира и записва нов API Key
        /// </summary>
        public static string? GenerateAndSaveApiKey()
        {
            var apiKey = GenerateApiKey();
            if (SaveApiKey(apiKey))
            {
                return apiKey;
            }
            return null;
        }
    }

    /// <summary>
    /// Статус на API Key
    /// </summary>
    public class ApiKeyStatus
    {
        public string? Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
