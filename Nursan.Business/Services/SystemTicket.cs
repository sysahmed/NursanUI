using Nursan.Business.Logging;
using Nursan.Domain.AmbarModels;
using Nursan.XMLTools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Nursan.Business.Services
{
    public class SystemTicket
    {
        private readonly StructuredLogger structuredLogger;

        public SystemTicket()
        {
            structuredLogger = new StructuredLogger(nameof(SystemTicket));
        }

        public async Task<(bool success, string ticketId)> CreateTicket(string tiketName, string bolge, string lastScreenshotPath, int role = 5)
        {
            try
            {
                Dictionary<string, string> startContext = new Dictionary<string, string>
                {
                    { "TicketName", SensitiveDataMasker.MaskValue(tiketName) },
                    { "Bolge", SensitiveDataMasker.MaskValue(bolge) },
                    { "Machine", Environment.MachineName },
                    { "Role", role.ToString(CultureInfo.InvariantCulture) }
                };
                structuredLogger.LogInfo("TicketCreateStart", startContext);

                var handler = new HttpClientHandler()
                {
                    UseDefaultCredentials = false,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        structuredLogger.LogWarning(
                            "SslValidationBypassed",
                            new Dictionary<string, string>
                            {
                                { "SslErrors", errors.ToString() }
                            });
                        return true;
                    },
                    CheckCertificateRevocationList = false,
                    UseCookies = false
                };

                using var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(15); // По-кратък timeout
                using var form = new MultipartFormDataContent();
                form.Add(new StringContent(tiketName), "Ariza");
                form.Add(new StringContent(bolge), "Bolge");
                form.Add(new StringContent(Environment.MachineName), "PcName");
                form.Add(new StringContent(role.ToString()), "Role"); // Използваме role параметъра вместо хардкоднатото "5"
                form.Add(new StringContent("0"), "Sicil");
                //form.Add(new StreamContent(File.OpenRead(lastScreenshotPath)), "photos", "снимка1.jpg");


                if (File.Exists(lastScreenshotPath))
                {
                    var photoBytes = File.ReadAllBytes(lastScreenshotPath);
                    var photoContent = new ByteArrayContent(photoBytes);
                    if (lastScreenshotPath.EndsWith(".png"))
                    {
                        photoContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                        form.Add(photoContent, "photos", "screenshot.png");
                    }
                    else
                    {
                        photoContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                        form.Add(photoContent, "photos", "screenshot.jpg");
                    }
                    Dictionary<string, string> screenshotContext = new Dictionary<string, string>
                    {
                        { "ScreenshotName", SensitiveDataMasker.MaskPath(lastScreenshotPath) },
                        { "ScreenshotSizeBytes", photoBytes.Length.ToString(CultureInfo.InvariantCulture) }
                    };
                    structuredLogger.LogInfo("ScreenshotAttached", screenshotContext);
                }
                else
                {
                    structuredLogger.LogWarning(
                        "ScreenshotMissing",
                        new Dictionary<string, string>
                        {
                            { "RequestedScreenshot", SensitiveDataMasker.MaskPath(lastScreenshotPath) }
                        });
                }

                // Първо тестваме връзката
                string serverIp = XMLSeverIp.XmlWebApiIP();
                string ipOnly = serverIp.Split(':')[0].Split('/')[0];
                structuredLogger.LogInfo(
                    "ServerConfigurationLoaded",
                    new Dictionary<string, string>
                    {
                        { "ServerIp", SensitiveDataMasker.MaskIp(serverIp) }
                    });

                try 
                {
                    using var ping = new System.Net.NetworkInformation.Ping();
                    var reply = ping.Send(ipOnly, 5000);
                    if (reply.Status != System.Net.NetworkInformation.IPStatus.Success)
                    {
                        structuredLogger.LogWarning(
                            "PingFailed",
                            new Dictionary<string, string>
                            {
                                { "Status", reply.Status.ToString() }
                            });
                    }
                    else
                    {
                        structuredLogger.LogInfo(
                            "PingSuccess",
                            new Dictionary<string, string>
                            {
                                { "Status", reply.Status.ToString() },
                                { "RoundTripTimeMs", reply.RoundtripTime.ToString(CultureInfo.InvariantCulture) }
                            });
                    }
                }
                catch (Exception pingEx)
                {
                    structuredLogger.LogWarning(
                        "PingException",
                        new Dictionary<string, string>
                        {
                            { "Message", pingEx.Message }
                        });
                }

                // Опитваме първо HTTPS, ако не работи - HTTP
                // Добавяме /Bakim/ преди tickets/create за да съвпада с GetByRoleName endpoint структурата
                string basePath = serverIp.EndsWith("/") ? serverIp.TrimEnd('/') : serverIp;
                string httpsUrl = $"https://{basePath}/api/tickets/create";
                string httpUrl = $"http://{basePath}/api/tickets/create";

                structuredLogger.LogInfo(
                    "TicketEndpointResolved",
                    new Dictionary<string, string>
                    {
                        { "HttpsUrl", httpsUrl },
                        { "HttpFallbackUrl", httpUrl }
                    });

                HttpResponseMessage response = null;
                try 
                {
                    // Първо опитваме HTTPS
                    response = await client.PostAsync(httpsUrl, form);
                }
                catch (HttpRequestException httpsEx)
                {
                    // Ако HTTPS не работи, опитваме HTTP
                    structuredLogger.LogWarning(
                        "HttpsRequestFailed",
                        new Dictionary<string, string>
                        {
                            { "Message", httpsEx.Message }
                        });

                    response = await client.PostAsync(httpUrl, form);
                }
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Опитваме се да извлечем ticket ID от отговора
                    string ticketId = ExtractTicketIdFromResponse(result);
                    Dictionary<string, string> successContext = new Dictionary<string, string>
                    {
                        { "StatusCode", response.StatusCode.ToString() },
                        { "TicketId", ticketId }
                    };
                    structuredLogger.LogInfo("TicketCreateSuccess", successContext);
                    return (true, ticketId);
                }
                else
                {
                    Dictionary<string, string> failureContext = new Dictionary<string, string>
                    {
                        { "StatusCode", response.StatusCode.ToString() },
                        { "Reason", response.ReasonPhrase ?? string.Empty }
                    };
                    structuredLogger.LogError("TicketCreateFailed", failureContext);
                }
                return (false, null);
            }
            catch (HttpRequestException httpEx)
            {
                structuredLogger.LogError(
                    "TicketHttpException",
                    new Dictionary<string, string>
                    {
                        { "Message", httpEx.Message }
                    });
                return (false, null);
            }
            catch (TaskCanceledException timeoutEx)
            {
                structuredLogger.LogError(
                    "TicketTimeout",
                    new Dictionary<string, string>
                    {
                        { "Message", timeoutEx.Message }
                    });
                return (false, null);
            }
            catch (Exception ex)
            {
                structuredLogger.LogError(
                    "TicketUnhandledException",
                    new Dictionary<string, string>
                    {
                        { "Message", ex.Message }
                    });
                return (false, null);
            }
        }

        /// <summary>
        /// Извлича ticket ID от отговора на сървъра
        /// </summary>
        /// <param name="response">JSON отговор от сървъра</param>
        /// <returns>Ticket ID или fallback стойност</returns>
        private string ExtractTicketIdFromResponse(string response)
        {
            try
            {
                // Опитваме различни начини да извлечем ID
                
                // 1. Търсим JSON полета като "id", "ticketId", "Id"
                var patterns = new[]
                {
                    @"""id""\s*:\s*(\d+)",           // "id": 123
                    @"""Id""\s*:\s*(\d+)",           // "Id": 123  
                    @"""ticketId""\s*:\s*(\d+)",     // "ticketId": 123
                    @"""TicketId""\s*:\s*(\d+)",     // "TicketId": 123
                    @"""ticket_id""\s*:\s*(\d+)",    // "ticket_id": 123
                    @"\b(\d+)\b"                     // Всяко число
                };

                foreach (string pattern in patterns)
                {
                    var match = System.Text.RegularExpressions.Regex.Match(response, pattern);
                    if (match.Success)
                    {
                        structuredLogger.LogInfo(
                            "TicketIdExtracted",
                            new Dictionary<string, string>
                            {
                                { "Pattern", pattern },
                                { "TicketId", match.Groups[1].Value }
                            });
                        return match.Groups[1].Value;
                    }
                }

                structuredLogger.LogWarning(
                    "TicketIdMissing",
                    new Dictionary<string, string>
                    {
                        { "ResponseSnippet", response.Length > 120 ? response.Substring(0, 120) : response }
                    });
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            catch (Exception ex)
            {
                structuredLogger.LogError(
                    "TicketIdExtractionError",
                    new Dictionary<string, string>
                    {
                        { "Message", ex.Message }
                    });
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

        /// <summary>
        /// Извлича тикети от API според име на роля
        /// </summary>
        /// <param name="roleName">Име на ролята</param>
        /// <returns>Списък от тикети от тип TickedRolleNote</returns>
        public async Task<List<TickedRolleNote>> GetTicketsByRoleName()
        {
            try
            {
                //Dictionary<string, string> startContext = new Dictionary<string, string>
                //{
                //    { "RoleName", SensitiveDataMasker.MaskValue(roleName) }
                //};
               // structuredLogger.LogInfo("GetTicketsByRoleNameStart", startContext);

                //if (string.IsNullOrWhiteSpace(roleName))
                //{
                //    structuredLogger.LogWarning("GetTicketsByRoleNameEmptyRole", new Dictionary<string, string>());
                //    return new List<TickedRolleNote>();
                //}

                var handler = new HttpClientHandler()
                {
                    UseDefaultCredentials = false,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                    {
                        structuredLogger.LogWarning(
                            "SslValidationBypassed",
                            new Dictionary<string, string>
                            {
                                { "SslErrors", errors.ToString() }
                            });
                        return true;
                    },
                    CheckCertificateRevocationList = false,
                    UseCookies = false
                };

                using var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(15);

                string serverIp = XMLSeverIp.XmlWebApiIP();
               // string encodedRoleName = Uri.EscapeDataString(roleName);
                
                // Опитваме първо HTTPS, ако не работи - HTTP
                string httpsUrl = $"https://{serverIp}/api/tickets/TickedRolleNote/GetByRoleName";
                string httpUrl = $"http://{serverIp}/api/tickets/TickedRolleNote/GetByRoleName";

                structuredLogger.LogInfo(
                    "GetTicketsEndpointResolved",
                    new Dictionary<string, string>
                    {
                        { "HttpsUrl", httpsUrl },
                        { "HttpFallbackUrl", httpUrl }
                    });

                HttpResponseMessage response = null;
                try
                {
                    // Първо опитваме HTTPS
                    response = await client.GetAsync(httpsUrl);
                }
                catch (HttpRequestException httpsEx)
                {
                    // Ако HTTPS не работи, опитваме HTTP
                    structuredLogger.LogWarning(
                        "HttpsRequestFailed",
                        new Dictionary<string, string>
                        {
                            { "Message", httpsEx.Message }
                        });

                    response = await client.GetAsync(httpUrl);
                }

                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonOptions = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                    };

                    List<TickedRolleNote> tickets = JsonSerializer.Deserialize<List<TickedRolleNote>>(result, jsonOptions) ?? new List<TickedRolleNote>();
                    
                    // RoleName се попълва автоматично от API отговора чрез JsonPropertyName("rollerName")
                    // Ако има вложен обект Rollers, също го попълваме като fallback
                    foreach (var ticket in tickets)
                    {
                        if (string.IsNullOrEmpty(ticket.RoleName) && ticket.Rollers != null && !string.IsNullOrEmpty(ticket.Rollers.RoleName))
                        {
                            ticket.RoleName = ticket.Rollers.RoleName;
                        }
                    }
                    
                    // Филтрираме само активните тикети
                    //tickets = tickets.Where(t => t.Activ == true).ToList();
                    
                    Dictionary<string, string> successContext = new Dictionary<string, string>
                    {
                        { "StatusCode", response.StatusCode.ToString() },
                        { "TicketsCount", tickets.Count.ToString(CultureInfo.InvariantCulture) }
                    };
                    structuredLogger.LogInfo("GetTicketsByRoleNameSuccess", successContext);
                    
                    return tickets;
                }
                else
                {
                    Dictionary<string, string> failureContext = new Dictionary<string, string>
                    {
                        { "StatusCode", response.StatusCode.ToString() },
                        { "Reason", response.ReasonPhrase ?? string.Empty },
                        { "Response", result.Length > 200 ? result.Substring(0, 200) : result }
                    };
                    structuredLogger.LogError("GetTicketsByRoleNameFailed", failureContext);
                    return new List<TickedRolleNote>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                structuredLogger.LogError(
                    "GetTicketsHttpException",
                    new Dictionary<string, string>
                    {
                        { "Message", httpEx.Message }
                    });
                return new List<TickedRolleNote>();
            }
            catch (TaskCanceledException timeoutEx)
            {
                structuredLogger.LogError(
                    "GetTicketsTimeout",
                    new Dictionary<string, string>
                    {
                        { "Message", timeoutEx.Message }
                    });
                return new List<TickedRolleNote>();
            }
            catch (JsonException jsonEx)
            {
                structuredLogger.LogError(
                    "GetTicketsJsonException",
                    new Dictionary<string, string>
                    {
                        { "Message", jsonEx.Message }
                    });
                return new List<TickedRolleNote>();
            }
            catch (Exception ex)
            {
                structuredLogger.LogError(
                    "GetTicketsUnhandledException",
                    new Dictionary<string, string>
                    {
                        { "Message", ex.Message }
                    });
                return new List<TickedRolleNote>();
            }
        }
    }
}
