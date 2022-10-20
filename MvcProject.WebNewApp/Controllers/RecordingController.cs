using Microsoft.AspNetCore.Mvc;

namespace MvcProject.WebNewApp.Controllers
{
    public class RecordingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
