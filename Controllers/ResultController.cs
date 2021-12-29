using CarbonOffset.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarbonOffset.Controllers
{
    public class ResultController : Controller
    {
        
       
        public IActionResult Index()
        {
            var testResult = new Result();
            return View(testResult);
        }
    }
}
