using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.IO;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// Контролер за управление на видеа - прокси към отделно видео API
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VideosController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VideosController> _logger;
        private readonly string _videoApiBaseUrl;
        private readonly string? _videoApiToken;

        public VideosController(
            IHttpClientFactory httpClientFactory,
            ILogger<VideosController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _logger = logger;
            _videoApiBaseUrl = GetVideoApiUrlFromXml();
            _videoApiToken = GetVideoApiTokenFromXml();
            
            // Добавяме токена в default headers ако е наличен
            if (!string.IsNullOrEmpty(_videoApiToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _videoApiToken);
            }
        }

        /// <summary>
        /// Чете видео API URL от Baglanti.xml
        /// </summary>
        private string GetVideoApiUrlFromXml()
        {
            try
            {
                string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                if (!System.IO.File.Exists(xmlPath))
                {
                    _logger.LogWarning("Baglanti.xml не е намерен, използвам fallback URL");
                    return "https://localhost:12012";
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);

                XmlNode configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                {
                    return "https://localhost:12012";
                }

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
                        return $"http://{address}";
                    }
                }

                return "http://localhost:12012";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при четене на видео API URL от Baglanti.xml");
                return "https://localhost:12012";
            }
        }

        /// <summary>
        /// Чете видео API Token от Baglanti.xml
        /// </summary>
        private string? GetVideoApiTokenFromXml()
        {
            try
            {
                string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Baglanti.xml");
                if (!System.IO.File.Exists(xmlPath))
                {
                    return null;
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(xmlPath);

                XmlNode configNode = doc.SelectSingleNode("config");
                if (configNode == null)
                {
                    return null;
                }

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
                _logger.LogError(ex, "Грешка при четене на видео API Token от Baglanti.xml");
                return null;
            }
        }

        /// <summary>
        /// Получава видео по ID от видео API
        /// </summary>
        /// <param name="id">ID на видеото</param>
        /// <returns>Видео обект</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVideo(int id)
        {
            try
            {
                string videoApiUrl = $"{_videoApiBaseUrl.TrimEnd('/')}/api/Videos/{id}";
                
                var response = await _httpClient.GetAsync(videoApiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Content(content, "application/json");
                }
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(new { Message = $"Видео с ID {id} не е намерено." });
                }
                
                _logger.LogWarning("Видео API върна статус {StatusCode} за видео ID {VideoId}", response.StatusCode, id);
                return StatusCode((int)response.StatusCode, new { Message = "Грешка при заявка към видео API." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при извличане на видео с ID {VideoId} от видео API", id);
                return StatusCode(500, new { Message = "Вътрешна грешка на сървъра при извличане на видео." });
            }
        }

        /// <summary>
        /// Търси видеа по заглавие в видео API
        /// </summary>
        /// <param name="title">Заглавие за търсене</param>
        /// <param name="page">Номер на страницата (по подразбиране 1)</param>
        /// <param name="pageSize">Брой елементи на страница (по подразбиране 12)</param>
        /// <returns>Списък с намерени видеа</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchVideos(
            [FromQuery] string title,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title))
                {
                    return BadRequest(new { Message = "Параметърът 'title' е задължителен." });
                }

                string videoApiUrl = $"{_videoApiBaseUrl.TrimEnd('/')}/api/Videos/search?title={Uri.EscapeDataString(title)}&page={page}&pageSize={pageSize}";
                
                var response = await _httpClient.GetAsync(videoApiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Content(content, "application/json");
                }
                
                _logger.LogWarning("Видео API върна статус {StatusCode} за търсене с заглавие {Title}", response.StatusCode, title);
                return StatusCode((int)response.StatusCode, new { Message = "Грешка при заявка към видео API." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при търсене на видеа с заглавие {Title} в видео API", title);
                return StatusCode(500, new { Message = "Вътрешна грешка на сървъра при търсене на видеа." });
            }
        }

        /// <summary>
        /// Получава видео файл за стрийминг по ID от видео API
        /// Поддържа Range requests (HTTP 206) за стрийминг
        /// </summary>
        /// <param name="id">ID на видеото</param>
        /// <returns>Видео файл за стрийминг</returns>
        [HttpGet("{id}/file")]
        public async Task<IActionResult> GetVideoFile(int id)
        {
            try
            {
                string videoApiUrl = $"{_videoApiBaseUrl.TrimEnd('/')}/api/Videos/{id}/file";
                
                // Предаваме Range header от клиента към видео API за стрийминг поддръжка
                var request = new HttpRequestMessage(HttpMethod.Get, videoApiUrl);
                
                if (Request.Headers.ContainsKey("Range"))
                {
                    request.Headers.Add("Range", Request.Headers["Range"].ToString());
                }
                
                var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                
                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.PartialContent)
                {
                    // Предаваме всички headers от видео API към клиента
                    var result = new FileStreamResult(
                        await response.Content.ReadAsStreamAsync(),
                        response.Content.Headers.ContentType?.MediaType ?? "video/mp4"
                    );
                    
                    // Важно: Предаваме Accept-Ranges header за стрийминг поддръжка
                    if (response.Headers.Contains("Accept-Ranges"))
                    {
                        Response.Headers.Add("Accept-Ranges", response.Headers.GetValues("Accept-Ranges").FirstOrDefault());
                    }
                    
                    // Предаваме Content-Range header ако е PartialContent (206)
                    if (response.StatusCode == System.Net.HttpStatusCode.PartialContent && 
                        response.Headers.Contains("Content-Range"))
                    {
                        Response.Headers.Add("Content-Range", response.Headers.GetValues("Content-Range").FirstOrDefault());
                        Response.StatusCode = 206; // Partial Content
                    }
                    
                    // Предаваме Content-Length header
                    if (response.Content.Headers.ContentLength.HasValue)
                    {
                        Response.ContentLength = response.Content.Headers.ContentLength.Value;
                    }
                    
                    return result;
                }
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound(new { Message = $"Видео файл с ID {id} не е намерен." });
                }
                
                _logger.LogWarning("Видео API върна статус {StatusCode} за видео файл ID {VideoId}", response.StatusCode, id);
                return StatusCode((int)response.StatusCode, new { Message = "Грешка при заявка към видео API." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Грешка при извличане на видео файл с ID {VideoId} от видео API", id);
                return StatusCode(500, new { Message = "Вътрешна грешка на сървъра при извличане на видео файл." });
            }
        }
    }
}
