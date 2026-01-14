using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;

namespace Nursan.Licenzing
{
    public partial class Lisanslama : Form
    {
        private readonly KilitYaratma klt = new KilitYaratma();
        private readonly RegistrileriAra reg = new RegistrileriAra();
        private readonly FingerPrint fng = new FingerPrint();
        private bool _isCheckingLicense = false;

        public Lisanslama()
        {
            InitializeComponent();
        }

        private void btnActiv_Click(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("yyy-MM-dd");
            string currentFingerprint = fng.Value();
            string providedCode = txtActivaciq.Text?.Trim() ?? string.Empty;

            if (string.Equals(currentFingerprint, providedCode, StringComparison.OrdinalIgnoreCase))
            {
                if (reg.regYaz(date, klt.Value()) == "hello")
                {
                    if (MessageBox.Show("Активацията е успешна.", "Активация:", MessageBoxButtons.OK,
                            MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Hide();
                    }
                }
            }
            else
            {
                MessageBox.Show("Кодът за активиране е грешен!", "Грешка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Lisanslama_Load(object sender, EventArgs e)
        {
            try
            {
                RenderActivationQr();
            }
            catch (Exception ex)
            {
                // Ако има проблем с генерирането на QR кода, все пак показваме формата
                // и записваме грешката в текстовото поле
                string errorMsg = $"Грешка при генериране на QR код: {ex.Message}";
                if (txtSeriNomer != null)
                {
                    txtSeriNomer.Text = errorMsg;
                }
                // Не показваме MessageBox тук, за да не блокираме формата
            }

            // Проверка на текущия лиценз по старата логика (fallback)
            try
            {
                var regResult = reg.regBac(klt.Value());
                if (!string.IsNullOrWhiteSpace(regResult) && !string.Equals(regResult, "yes!!!", StringComparison.OrdinalIgnoreCase))
                {
                    if (txtSeriNomer != null)
                    {
                        txtSeriNomer.AppendText($"\r\n\r\nREG: {regResult}");
                    }
                }

                if (!string.Equals(regResult, "yes!!!", StringComparison.OrdinalIgnoreCase))
                {
                    // Не показваме MessageBox тук, за да не блокираме формата при зареждане
                    // Потребителят ще види информацията в текстовото поле
                }
            }
            catch (Exception ex)
            {
                // Ако има проблем с проверката на лиценза, записваме грешката
                if (txtSeriNomer != null)
                {
                    txtSeriNomer.AppendText($"\r\n\r\nГрешка при проверка на лиценз: {ex.Message}");
                }
            }

            // Стартираме периодичната проверка на лиценза (всеки 3 секунди)
            if (timerLicenseCheck != null)
            {
                timerLicenseCheck.Start();
            }
        }

        private void RenderActivationQr()
        {
            if (fng == null || klt == null)
            {
                throw new InvalidOperationException("FingerPrint или KilitYaratma не са инициализирани.");
            }

            var payload = new ActivationPayload
            {
                MachineId = fng.Value(),
                SerialKey = klt.Value(),
                GeneratedAtUtc = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
            {
                WriteIndented = false  // Компактен формат за QR кода
            });

            // Не показваме JSON-а в текстовото поле - само QR кода
            // Ако искаш да скриеш текстовото поле напълно, може да го направиш невидимо в Designer
            if (txtSeriNomer != null)
            {
                txtSeriNomer.Text = $"Машинен ID: {payload.MachineId}\r\n\r\nСканирайте QR кода по-долу със служебното приложение за активация на лиценза.";
                txtSeriNomer.ReadOnly = true;
            }

            using var generator = new QRCodeGenerator();
            using QRCodeData qrCodeData = generator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            using QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrBitmap = qrCode.GetGraphic(20);

            if (pictureBoxQr != null)
            {
                pictureBoxQr.Image?.Dispose();
                pictureBoxQr.Image = qrBitmap;
            }
        }

        private void TimerLicenseCheck_Tick(object sender, EventArgs e)
        {
            // Предотвратяваме множествени едновременни проверки
            if (_isCheckingLicense)
            {
                return;
            }

            _isCheckingLicense = true;
            
            try
            {
                // Проверяваме дали лицензът е активиран чрез API
                bool isActivated = LicenseStatusChecker.CheckStatus(fng.Value(), out string? message);
                
                if (isActivated)
                {
                    // Лицензът е активиран - спираме Timer-а и затваряме формата
                    timerLicenseCheck?.Stop();
                    
                    // Активираме лиценза локално (запазваме в регистъра)
                    try
                    {
                        string date = DateTime.Now.ToString("yyy-MM-dd");
                        reg.regYaz(date, klt.Value());
                    }
                    catch
                    {
                        // Ако има проблем с локалната активация, продължаваме
                    }
                    
                    // Показваме съобщение и затваряме формата
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            MessageBox.Show("Лицензът е активиран успешно!", "Активация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            DialogResult = DialogResult.OK;
                            Close();
                        }));
                    }
                    else
                    {
                        MessageBox.Show("Лицензът е активиран успешно!", "Активация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
                else
                {
                    // Лицензът все още не е активиран - обновяваме статуса в текстовото поле
                    if (txtSeriNomer != null && InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            UpdateStatusMessage(message ?? "Очакване на активация...");
                        }));
                    }
                    else if (txtSeriNomer != null)
                    {
                        UpdateStatusMessage(message ?? "Очакване на активация...");
                    }
                }
            }
            catch (Exception ex)
            {
                // При грешка при проверката, не показваме съобщение, за да не блокираме формата
                // Просто продължаваме да проверяваме
            }
            finally
            {
                _isCheckingLicense = false;
            }
        }

        private void UpdateStatusMessage(string message)
        {
            if (txtSeriNomer == null) return;
            
            string currentText = txtSeriNomer.Text;
            int lastNewLine = currentText.LastIndexOf("\r\n\r\n");
            
            if (lastNewLine >= 0)
            {
                // Заменяме само последното съобщение
                txtSeriNomer.Text = currentText.Substring(0, lastNewLine) + $"\r\n\r\n{DateTime.Now:HH:mm:ss} - {message}";
            }
            else
            {
                // Добавяме ново съобщение
                txtSeriNomer.AppendText($"\r\n\r\n{DateTime.Now:HH:mm:ss} - {message}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Спираме Timer-а при затваряне на формата
            timerLicenseCheck?.Stop();
            base.OnFormClosing(e);
        }

        private sealed class ActivationPayload
        {
            public string MachineId { get; set; } = string.Empty;
            public string SerialKey { get; set; } = string.Empty;
            public DateTime GeneratedAtUtc { get; set; }
        }
    }

    /// <summary>
    /// Клас за проверка на статуса на лиценза чрез API без зависимост от Nursan.UI
    /// </summary>
    internal static class LicenseStatusChecker
    {
        private const string ApiUrl = "https://license.example.comlicense/check"; // TODO: замени с реалното API
        private static readonly HttpClient Http = CreateHttpClient();

        internal static bool CheckStatus(string machineId, out string? message)
        {
            message = null;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var result = CheckStatusAsync(machineId, cts.Token).GetAwaiter().GetResult();
                
                if (result.IsValid)
                {
                    message = "Лицензът е активиран успешно!";
                    return true;
                }
                
                message = result.Message ?? "Лицензът все още не е активиран.";
                return false;
            }
            catch (Exception ex)
            {
                message = $"Грешка при проверка: {ex.Message}";
                return false;
            }
        }

        private static async Task<LicenseStatusResponse> CheckStatusAsync(string machineId, CancellationToken cancellationToken)
        {
            var requestModel = new
            {
                machineId = machineId,
                appVersion = Application.ProductVersion ?? "1.0.0",
                timestampUtc = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(requestModel, JsonOptions);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await Http.PostAsync(ApiUrl, content, cancellationToken).ConfigureAwait(false);
            var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                return new LicenseStatusResponse
                {
                    IsValid = false,
                    Message = $"API върна грешка: {response.StatusCode}"
                };
            }

            var result = JsonSerializer.Deserialize<LicenseStatusResponse>(body, JsonOptions);
            return result ?? new LicenseStatusResponse
            {
                IsValid = false,
                Message = "Некоректен отговор от лицензния сървър."
            };
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) => true
            };
            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        private sealed class LicenseStatusResponse
        {
            [JsonPropertyName("isValid")]
            public bool IsValid { get; set; }

            [JsonPropertyName("message")]
            public string? Message { get; set; }
        }
    }
}