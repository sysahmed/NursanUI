using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Nursan.API.Commands;
using Nursan.API.DTOs;
using Nursan.XMLTools;

namespace Nursan.UI.Services
{
    /// <summary>
    /// API клиент за TorkService операции
    /// </summary>
    public class TorkApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public TorkApiClient()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            // Вземаме API URL от XML конфигурацията или използваме default
            string serverIp = XMLSeverIp.XmlWebApiIP();
            _baseUrl = $"https://{serverIp}/api/tork";
            
            // Настройваме SSL да игнорира грешки (за development)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Проверява системата за ElTest
        /// </summary>
        public async Task<TorkResultDto> CheckSystemElTest(BarcodeInputDto barcode, string vardiyaName)
        {
            try
            {
                var command = new CheckSystemElTestCommand
                {
                    Barcode = barcode,
                    VardiyaName = vardiyaName
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/check-system-eltest", command);
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
        /// Проверява системата
        /// </summary>
        public async Task<TorkResultDto> CheckSystem(BarcodeInputDto barcode, string vardiyaName)
        {
            try
            {
                var command = new CheckSystemCommand
                {
                    Barcode = barcode,
                    VardiyaName = vardiyaName
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/check-system", command);
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
        /// Обработва ElTest баркодове
        /// </summary>
        public async Task<TorkResultDto> ProcessElTestBarcode(string[] barcodes, string vardiyaName)
        {
            try
            {
                var command = new ProcessElTestBarcodeCommand
                {
                    Barcodes = barcodes,
                    VardiyaName = vardiyaName
                };

                var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/process-eltest-barcode", command);
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

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
