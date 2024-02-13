using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
