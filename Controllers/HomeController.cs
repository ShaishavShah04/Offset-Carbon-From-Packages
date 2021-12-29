using CarbonOffset.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarbonOffset.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Result(string trackingNumber, string carrier)
        {
            Console.WriteLine(carrier);
            /*
            using (var client = new HttpClient())
            { 
                var uri = new Uri("https://api.goshippo.com/tracks/shippo/SHIPPO_TRANSIT")
            }
            */
            return RedirectToAction("Index", "Result");
        }
    }
}