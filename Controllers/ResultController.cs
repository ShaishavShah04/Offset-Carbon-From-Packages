using Microsoft.AspNetCore.Mvc;

namespace CarbonOffset.Controllers
{
    public class ResultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
