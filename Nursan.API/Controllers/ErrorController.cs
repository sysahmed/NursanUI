using Microsoft.AspNetCore.Mvc;

namespace Nursan.API.Controllers
{
    /// <summary>
    /// Контролер за обработка на грешки
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Показва страница за грешка
        /// </summary>
        [HttpGet]
        [Route("/Error")]
        [Route("/Error/{statusCode}")]
        public IActionResult Error(int? statusCode = null)
        {
            ViewBag.StatusCode = statusCode ?? 500;
            return View();
        }
    }
}

