using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Nursan.Domain.VideoModels;
using Nursan.UI.DTOs;
using Nursan.XMLTools;

namespace Nursan.UI.Services
{
    /// <summary>
    /// API сервис за видео операции
    /// </summary>
    public class VideoApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string? _apiToken;

        public VideoApiService()
        {
            // Настройваме SSL да игнорира грешки (за development)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            // Вземаме Video API URL от XML конфигурацията (вече включва протокол)
            string videoApiAddress = XMLSeverIp.XmlVideoApiAddress();
            
            // Премахваме наклонена черта в края ако има
            videoApiAddress = videoApiAddress.TrimEnd('/');
            
            // Добавяме /api в края
            _baseUrl = $"{videoApiAddress}/api";
            
            // Вземаме Video API Token от XML конфигурацията
            _apiToken = XMLSeverIp.XmlVideoApiToken();
            
            // Добавяме токена в default headers ако е наличен
            if (!string.IsNullOrEmpty(_apiToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);
            }
        }

        /// <summary>
        /// Получава видео по ID от API
        /// </summary>
        public async Task<Video?> GetVideoByIdAsync(int videoId)
        {
            try
            {
                string url = $"{_baseUrl}/Videos/{videoId}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<Video>();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Получава URL за стрийминг на видео файл по ID
        /// </summary>
        /// <remarks>
        /// ВАЖНО: Този endpoint трябва да поддържа Range requests (HTTP 206 Partial Content) за стрийминг.
        /// Endpoint-ът трябва да:
        /// 1. Връща правилни headers: Accept-Ranges: bytes
        /// 2. Поддържа Range header в заявката: Range: bytes=0-1023
        /// 3. Връща HTTP 206 (Partial Content) с Content-Range header
        /// 4. Връща Content-Type: video/mp4 (или друг видео формат)
        /// </remarks>
        public string GetVideoFileUrl(int videoId)
        {
            // Конструираме URL за стрийминг на видео файла
            // Използваме главния API като прокси, който предава заявката към видео API
            // Endpoint: /api/Videos/{id}/file (в главния API, който проксира към видео API)
            
            // За сега използваме директно видео API-то
            // В бъдеще може да използваме главния API като прокси
            string baseUrlWithoutApi = _baseUrl.Replace("/api", "");
            return $"{baseUrlWithoutApi}/api/Videos/{videoId}/file";
        }

        /// <summary>
        /// Търси видеа по заглавие (title)
        /// </summary>
        public async Task<List<Video>?> SearchVideosByTitleAsync(string title, int page = 1, int pageSize = 12)
        {
            try
            {
                string url = $"{_baseUrl}/videos/search?title={Uri.EscapeDataString(title)}&page={page}&pageSize={pageSize}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                // Парсваме response-а от видео API
                var searchResponse = await response.Content.ReadFromJsonAsync<VideoSearchResponseDto>();
                if (searchResponse == null || searchResponse.Items == null || searchResponse.Items.Count == 0)
                {
                    return null;
                }

                // Конвертираме VideoItemDto към Video
                return searchResponse.Items.Select(item => new Video
                {
                    Id = item.Id,
                    Title = item.Title ?? string.Empty,
                    Description = item.Description,
                    FileName = item.FileName ?? string.Empty,
                    VideoUrl = item.Url ?? string.Empty,
                    AbsoluteVideoUrl = item.Url,
                    UploadedBy = item.UploadedBy ?? string.Empty,
                    UploadDate = item.UploadDate,
                    IsActive = item.IsActive,
                    IsMobileOptimized = item.IsMobileOptimized
                }).ToList();
            }
            catch (Exception ex)
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
