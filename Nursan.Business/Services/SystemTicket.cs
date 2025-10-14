using Nursan.XMLTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection.Emit;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Nursan.Business.Services
{
    public class SystemTicket
    {
       public async Task<(bool success, string ticketId)> CreateTicket(string tiketName, string bolge, string lastScreenshotPath, int role = 5)
        {
            try
            {
                // Модерен SSL bypass за .NET 8 - поддържа и HTTP и HTTPS
                var handler = new HttpClientHandler()
                {
                    UseDefaultCredentials = false,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => {
                        Console.WriteLine($"SSL Validation: {errors}");
                        return true; // Приемаме всички сертификати
                    },
                    CheckCertificateRevocationList = false,
                    UseCookies = false
                };

                // Създаваме нов тикет
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
                    Console.WriteLine($"Намерен скрийншот: {lastScreenshotPath}");
                    var photoBytes = File.ReadAllBytes(lastScreenshotPath);
                    Console.WriteLine($"Размер на снимката: {photoBytes.Length} bytes");
                    
                    var photoContent = new ByteArrayContent(photoBytes);
                    
                    // Правилно задаваме content type според файла
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
                    
                    Console.WriteLine("Снимката е добавена към заявката");
                }
                else
                {
                    Console.WriteLine($"Скрийншот файлът не съществува: {lastScreenshotPath}");
                }

                // Първо тестваме връзката
                string serverIp = XMLSeverIp.XmlWebApiIP();
                Console.WriteLine($"Тестваме връзката към сървър: {serverIp}");
                
                try 
                {
                    using var ping = new System.Net.NetworkInformation.Ping();
                    var reply = ping.Send(serverIp, 5000);
                    Console.WriteLine($"Ping резултат: {reply.Status}");
                    
                    if (reply.Status != System.Net.NetworkInformation.IPStatus.Success)
                    {
                        Console.WriteLine($"Сървърът не е достъпен: {reply.Status}");
                        return (false, null);
                    }
                }
                catch (Exception pingEx)
                {
                    Console.WriteLine($"Ping грешка: {pingEx.Message}");
                    // Продължаваме въпреки ping грешката
                }

                // Опитваме първо HTTPS, ако не работи - HTTP
                string httpsUrl = $"https://{serverIp}/api/tickets/create";
                string httpUrl = $"http://{serverIp}/api/tickets/create";
                
                Console.WriteLine($"Опитваме HTTPS заявка към: {httpsUrl}");
                
                HttpResponseMessage response = null;
                try 
                {
                    // Първо опитваме HTTPS
                    response = await client.PostAsync(httpsUrl, form);
                }
                catch (HttpRequestException httpsEx)
                {
                    Console.WriteLine($"HTTPS неуспешно: {httpsEx.Message}");
                    Console.WriteLine($"Преминаваме към HTTP: {httpUrl}");
                    
                    // Ако HTTPS не работи, опитваме HTTP
                    response = await client.PostAsync(httpUrl, form);
                }
                string result = await response.Content.ReadAsStringAsync();
                
                Console.WriteLine($"HTTP Status: {response.StatusCode}");
                Console.WriteLine($"Response: {result}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Тикетът е изпратен успешно!");
                    Console.WriteLine($"Отговор от сървъра: {result}");
                    
                    // Опитваме се да извлечем ticket ID от отговора
                    string ticketId = ExtractTicketIdFromResponse(result);
                    Console.WriteLine($"Извлечен ticket ID: {ticketId}");
                    
                    return (true, ticketId);
                }
                else
                {
                    Console.WriteLine($"Грешка при изпращане: {response.StatusCode} - {response.ReasonPhrase}");
                    Console.WriteLine($"Отговор: {result}");
                }
                return (false, null);
            }
            catch (HttpRequestException httpEx)
            {
                // Логваме HTTP грешки
                Console.WriteLine($"HTTP грешка: {httpEx.Message}");
                return (false, null);
            }
            catch (TaskCanceledException timeoutEx)
            {
                // Логваме timeout грешки
                Console.WriteLine($"Timeout грешка: {timeoutEx.Message}");
                return (false, null);
            }
            catch (Exception ex)
            {
                // Логваме общи грешки
                Console.WriteLine($"Обща грешка: {ex.Message}");
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
                        Console.WriteLine($"Намерен ID с pattern '{pattern}': {match.Groups[1].Value}");
                        return match.Groups[1].Value;
                    }
                }

                // 2. Ако няма число, използваме timestamp като fallback
                Console.WriteLine("Не е намерен ID в отговора, използваме timestamp");
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка при извличане на ticket ID: {ex.Message}");
                return DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

    }
}
