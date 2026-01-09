using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Nursan.API.Commands;
using Nursan.UI.DTOs;
using Nursan.Domain.Entity;
using Nursan.Persistanse.Result;
using Nursan.XMLTools;
using Nursan.API.DTOs;

namespace Nursan.UI.Services
{
    /// <summary>
    /// API сервис за ElTest операции
    /// </summary>
    public class ElTestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string? _apiKey;
        private readonly string? _deviceId;

        public ElTestApiService()
        {
            // Настройваме SSL да игнорира грешки (за development)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            // Вземаме Master API URL от XML конфигурацията
            // Това е основният API адрес за всички операции (ElTest, Tork и др.)
            // Тикетите имат свой отделен адрес в ticketTracking node
            string masterApiAddress = XMLSeverIp.XmlMasterApiAddress();
            
            // Определяме протокола: за localhost използваме http, за останалото https
            string protocol = (masterApiAddress.StartsWith("localhost", StringComparison.OrdinalIgnoreCase) || 
                              masterApiAddress.StartsWith("127.0.0.1")) ? "http" : "https";
            _baseUrl = $"{protocol}://{masterApiAddress}/api";
            
            // Вземаме API Key от XML конфигурацията
            _apiKey = ApiKeyManager.GetApiKey();
            
            // Вземаме DeviceId от XML конфигурацията (или генерираме от машината)
            _deviceId = ApiKeyManager.GetDeviceId();
            
            // Проверяваме дали има API Key - ако няма, хвърляме грешка
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("API Key не е намерен в конфигурацията! Моля, генерирайте API Key от MVC интерфейса на API-то.");
            }
            
            // Добавяме API Key и DeviceId в default headers
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
            if (!string.IsNullOrEmpty(_deviceId))
            {
                _httpClient.DefaultRequestHeaders.Add("X-Device-Id", _deviceId);
            }
        }

        /// <summary>
        /// Проверява дали API Key е валиден преди извършване на заявка
        /// </summary>
        private void EnsureApiKeyIsValid()
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("API Key не е намерен. Приложението не може да работи без валиден API Key.");
            }
        }

        /// <summary>
        /// Зарежда данни в системата (еквивалент на GitSystemeYukle)
        /// </summary>
        public async Task<TorkResultDto> GitSystemeYukle(string[] barcodes, string vardiyaName)
        {
            EnsureApiKeyIsValid();
            
            try
            {
                var command = new ProcessElTestBarcodeCommand
                {
                    Barcodes = barcodes,
                    VardiyaName = vardiyaName
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/tork/process-eltest-barcode", command);
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<TorkResultDto>() 
                    ?? new TorkResultDto { Success = false, Message = "Грешка при десериализация" };
            }
            catch (Exception ex)
            {
                return new TorkResultDto
                {
                    Success = false,
                    Message = $"Грешка при API заявка: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Проверява системата за ElTest (еквивалент на GitSytemeSayiElTestBack)
        /// </summary>
        public async Task<TorkResultDto> CheckSystemElTest(string barcodeIcerik, string vardiyaName)
        {
            EnsureApiKeyIsValid();
            
            try
            {
                var barcodeDto = new BarcodeInputDto
                {
                    BarcodeIcerik = barcodeIcerik
                };

                var command = new CheckSystemElTestCommand
                {
                    Barcode = barcodeDto,
                    VardiyaName = vardiyaName
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/tork/check-system-eltest", command);
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<TorkResultDto>() 
                    ?? new TorkResultDto { Success = false, Message = "Грешка при десериализация" };
            }
            catch (Exception ex)
            {
                return new TorkResultDto
                {
                    Success = false,
                    Message = $"Грешка при API заявка: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Проверява системата (еквивалент на GitSytemeSayiBac)
        /// </summary>
        public async Task<TorkResultDto> CheckSystem(string barcodeIcerik, string vardiyaName)
        {
            EnsureApiKeyIsValid();
            
            try
            {
                var barcodeDto = new BarcodeInputDto
                {
                    BarcodeIcerik = barcodeIcerik
                };

                var command = new CheckSystemCommand
                {
                    Barcode = barcodeDto,
                    VardiyaName = vardiyaName
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/tork/check-system", command);
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadFromJsonAsync<TorkResultDto>() 
                    ?? new TorkResultDto { Success = false, Message = "Грешка при десериализация" };
            }
            catch (Exception ex)
            {
                return new TorkResultDto
                {
                    Success = false,
                    Message = $"Грешка при API заявка: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Получава PC ID от API (еквивалент на GetPcId)
        /// TODO: Трябва да се създаде endpoint в API за това
        /// </summary>
        public async Task<decimal> GetPcId()
        {
            EnsureApiKeyIsValid();
            
            try
            {
                // TODO: Създай endpoint в API: GET /api/pc/get-pc-id?machineName={machineName}
                var machineName = Environment.MachineName;
                var response = await _httpClient.GetAsync($"{_baseUrl}/pc/get-pc-id?machineName={machineName}");
                
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (decimal.TryParse(result, out decimal pcId))
                    {
                        return pcId;
                    }
                }
            }
            catch
            {
                // Игнорираме грешките
            }
            
            return 0;
        }

        /// <summary>
        /// Зарежда bootstrap конфигурация за станцията (контекст + правила) по machineName.
        /// </summary>
        public async Task<Nursan.UI.DTOs.StationBootstrapDto?> GetStationBootstrapAsync(string machineName)
        {
            EnsureApiKeyIsValid();

            if (string.IsNullOrWhiteSpace(machineName))
            {
                return null;
            }

            try
            {
                string url = $"{_baseUrl}/station/bootstrap?machineName={Uri.EscapeDataString(machineName)}";
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<Nursan.UI.DTOs.StationBootstrapDto>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получава видео по ID от API
        /// </summary>
        public async Task<Nursan.Domain.VideoModels.Video?> GetVideoByIdAsync(int videoId)
        {
            EnsureApiKeyIsValid();

            try
            {
                string url = $"{_baseUrl}/Videos/{videoId}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<Nursan.Domain.VideoModels.Video>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Търси видеа по референс (в Title или Description)
        /// </summary>
        public async Task<List<Nursan.Domain.VideoModels.Video>?> SearchVideosByReferenceAsync(string reference)
        {
            EnsureApiKeyIsValid();

            try
            {
                string url = $"{_baseUrl}/Videos/search?reference={Uri.EscapeDataString(reference)}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<List<Nursan.Domain.VideoModels.Video>>();
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
