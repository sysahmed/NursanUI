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
    public class VideoApiService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string? _apiToken;
        private bool _disposed = false;

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
            if (_disposed)
                throw new ObjectDisposedException(nameof(VideoApiService));

            try
            {
                // Проверяваме дали HttpClient е disposed преди използване
                if (_httpClient == null)
                    return null;
                
                string url = $"{_baseUrl}/Videos/{videoId}";
                var response = await _httpClient.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<Video>();
            }
            catch (ObjectDisposedException)
            {
                // HttpClient е disposed, не можем да правим заявки
                return null;
            }
            catch (Exception ex)
            {
                // Логваме други грешки но не хвърляме exception
                System.Diagnostics.Debug.WriteLine($"VideoApiService.GetVideoByIdAsync грешка: {ex.Message}");
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
        /// 
        /// ВАЖНО: Конвертираме HTTPS в HTTP за да избегнем SSL сертификатни проблеми с WebBrowser (IE engine)
        /// </remarks>
        public string GetVideoFileUrl(int videoId)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(VideoApiService));

            // Конструираме URL за стрийминг на видео файла
            // Използваме главния API като прокси, който предава заявката към видео API
            // Endpoint: /api/Videos/{id}/file (в главния API, който проксира към видео API)
            
            // ВАЖНО: Проверяваме дали _baseUrl не е празен
            if (string.IsNullOrEmpty(_baseUrl))
            {
                throw new InvalidOperationException("Video API base URL е празен - проверете Baglanti.xml конфигурацията");
            }
            
            // За сега използваме директно видео API-то
            // В бъдеще може да използваме главния API като прокси
            string baseUrlWithoutApi = _baseUrl.Replace("/api", "").TrimEnd('/');
            string videoUrl = $"{baseUrlWithoutApi}/api/Videos/{videoId}/file";
            
            // ВАЖНО: Проверяваме дали videoUrl не е празен
            if (string.IsNullOrEmpty(videoUrl))
            {
                throw new InvalidOperationException($"Генерираният видео URL е празен за videoId: {videoId}");
            }
            
            // Debug: Логваме генерирания URL
            System.Diagnostics.Debug.WriteLine($"VideoApiService.GetVideoFileUrl: {videoUrl}");
            
            // ВАЖНО: Оставяме URL-а както е (HTTPS или HTTP)
            // За HTTPS да работи без диалог, SSL сертификатът трябва да е инсталиран в Windows Trusted Root Certificate Authorities
            // Инструкции: Отворете HTTPS URL в браузър → Преглед на сертификата → Копиране във файл → Инсталиране в Trusted Root Certification Authorities
            
            return videoUrl;
        }

        /// <summary>
        /// Търси видеа по заглавие (title)
        /// </summary>
        public async Task<List<Video>?> SearchVideosByTitleAsync(string title, int page = 1, int pageSize = 12)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(VideoApiService));

            try
            {
                // Проверяваме дали HttpClient е disposed преди използване
                if (_httpClient == null)
                    return null;
                
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
            catch (ObjectDisposedException)
            {
                // HttpClient е disposed, не можем да правим заявки
                return null;
            }
            catch (HttpRequestException httpEx)
            {
                // Мрежова грешка - API сървърът не е достъпен
                System.Diagnostics.Debug.WriteLine($"VideoApiService.SearchVideosByTitleAsync HTTP грешка: {httpEx.Message}");
                System.Diagnostics.Debug.WriteLine($"Video API URL: {_baseUrl}");
                
                // Хвърляме exception за да може Gromet.cs да покаже съобщение на потребителя
                throw new HttpRequestException($"Видео API сървърът не е достъпен на адрес: {_baseUrl}. Проверете дали API-то работи.", httpEx);
            }
            catch (Exception ex)
            {
                // Логваме други грешки но не хвърляме exception
                System.Diagnostics.Debug.WriteLine($"VideoApiService.SearchVideosByTitleAsync грешка: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Освобождава ресурсите
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Освобождава ресурсите
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
