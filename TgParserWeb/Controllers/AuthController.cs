using Microsoft.AspNetCore.Mvc;

namespace TgParserWeb.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
