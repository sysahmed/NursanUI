using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nursan.UI.Security
{
    internal static class LicenseContext
    {
        private static readonly object Sync = new object();
        private static LicenseSession? _current;

        internal static LicenseSession Current
        {
            get
            {
                lock (Sync)
                {
                    return _current ?? throw new InvalidOperationException("Няма активна лицензна сесия.");
                }
            }
        }

        internal static bool TryInitialize(out string errorMessage)
        {
            lock (Sync)
            {
                if (_current != null)
                {
                    errorMessage = string.Empty;
                    return true;
                }

                try
                {
                    var session = LicenseValidator.EnsureLicense();
                    _current = session;
                    errorMessage = string.Empty;
                    return true;
                }
                catch (LicenseException lex)
                {
                    errorMessage = lex.Message;
                    return false;
                }
            }
        }
    }

    internal static class LicenseValidator
    {
        private const string ApiUrl = "https://license.example.com/api/license/check"; // TODO: замени с реалното API
        private static readonly HttpClient Http = CreateHttpClient();

        internal static LicenseSession EnsureLicense()
        {
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                var session = ValidateAsync(cts.Token).GetAwaiter().GetResult();
                if (session.ExpiryUtc.HasValue && session.ExpiryUtc.Value < DateTime.UtcNow)
                {
                    throw new LicenseException("Лицензът е изтекъл.");
                }

                return session;
            }
            catch (LicenseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LicenseException("Неуспешна проверка на лиценз от API.", ex);
            }
        }

        /// <summary>
        /// Проверява дали лицензът е активиран без да хвърля изключение. Използва се за периодична проверка.
        /// </summary>
        internal static bool CheckLicenseStatus(out string? message)
        {
            message = null;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var session = ValidateAsync(cts.Token).GetAwaiter().GetResult();
                
                if (session.IsValid && (!session.ExpiryUtc.HasValue || session.ExpiryUtc.Value >= DateTime.UtcNow))
                {
                    message = "Лицензът е активиран успешно!";
                    return true;
                }
                
                message = session.Message ?? "Лицензът все още не е активиран.";
                return false;
            }
            catch (Exception ex)
            {
                message = $"Грешка при проверка: {ex.Message}";
                return false;
            }
        }

        private static async Task<LicenseSession> ValidateAsync(CancellationToken cancellationToken)
        {
            var requestModel = new LicenseRequest
            {
                MachineId = LicenseFingerprint.Generate(),
                AppVersion = Application.ProductVersion,
                TimestampUtc = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(requestModel, JsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await Http.PostAsync(ApiUrl, content, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                throw new LicenseException($"API върна грешка: {response.StatusCode}");
            }

            var session = JsonSerializer.Deserialize<LicenseSession>(body, JsonOptions);
            if (session == null)
            {
                throw new LicenseException("Некоректен отговор от лицензния сървър.");
            }

            if (!session.IsValid)
            {
                throw new LicenseException(session.Message ?? "Лицензът е невалиден.");
            }

            session.DecryptSensitiveData();
            return session;
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) => true
            };
            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    internal static class LicenseFingerprint
    {
        internal static string Generate()
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append(Environment.MachineName);
                sb.Append('|');
                sb.Append(Environment.UserDomainName);
                sb.Append('|');
                sb.Append(Environment.OSVersion);

                var mac = GetPrimaryMacAddress();
                if (!string.IsNullOrEmpty(mac))
                {
                    sb.Append('|');
                    sb.Append(mac);
                }

                using var sha = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(sb.ToString());
                var hash = sha.ComputeHash(bytes);
                return Convert.ToHexString(hash);
            }
            catch
            {
                // fallback
                return Environment.MachineName;
            }
        }

        private static string? GetPrimaryMacAddress()
        {
            try
            {
                return NetworkInterface.GetAllNetworkInterfaces()
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    .OrderBy(nic => nic.Speed)
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault(address => !string.IsNullOrWhiteSpace(address));
            }
            catch
            {
                return null;
            }
        }
    }

    internal sealed class LicenseRequest
    {
        public string MachineId { get; set; } = string.Empty;
        public string AppVersion { get; set; } = string.Empty;
        public DateTime TimestampUtc { get; set; }
    }

    internal sealed class LicenseSession
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public DateTime? ExpiryUtc { get; set; }
        public string? LicenseToken { get; set; }
        public string? EncryptedSqlUser { get; set; }
        public string? EncryptedSqlPassword { get; set; }

        [JsonIgnore]
        public string? SqlUser { get; private set; }

        [JsonIgnore]
        public string? SqlPassword { get; private set; }

        internal void DecryptSensitiveData()
        {
            if (!string.IsNullOrWhiteSpace(EncryptedSqlUser))
            {
                SqlUser = CryptoHelper.TryDecrypt(EncryptedSqlUser);
            }

            if (!string.IsNullOrWhiteSpace(EncryptedSqlPassword))
            {
                SqlPassword = CryptoHelper.TryDecrypt(EncryptedSqlPassword);
            }
        }
    }

    internal static class CryptoHelper
    {
        // Примерни ключ и IV. Замени с твой секрет и/или зареждане от защитено място.
        private static readonly byte[] Key = Convert.FromBase64String("pDg5Y3Z5bVZPTU1rM1lHT0h2c2ZOWnFIU0VoUGpSUkQ=");
        private static readonly byte[] Iv = Convert.FromBase64String("Q0dPRkhZSkxQR01QbWpoYQ==");

        internal static string? TryDecrypt(string base64Cipher)
        {
            try
            {
                var cipher = Convert.FromBase64String(base64Cipher);
                using var aes = Aes.Create();
                aes.Key = Key;
                aes.IV = Iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var ms = new MemoryStream();
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipher, 0, cipher.Length);
                    cs.FlushFinalBlock();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return null;
            }
        }
    }

    internal sealed class LicenseException : Exception
    {
        public LicenseException(string message) : base(message) { }
        public LicenseException(string message, Exception innerException) : base(message, innerException) { }
    }
}


