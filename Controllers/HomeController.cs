using CarbonOffset.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Newtonsoft.Json;

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

        //static HttpClient client = new HttpClient();

        // private IHttpClientFactory _httpClientFactory;
        [HttpPost]
        public async Task<IActionResult> Result(Result resultObject)
        {
            string carrier = resultObject.Carrier;
            string trackingNumber = resultObject.TrackingNumber;
            string path = $"https://api.goshippo.com/tracks/{carrier}/{trackingNumber}";
            Console.WriteLine(resultObject.Carrier);
            Console.WriteLine(resultObject.TrackingNumber);

            // testing purposes URL
            // https://api.goshippo.com/tracks/shippo/SHIPPO_TRANSIT

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(path);

            client.DefaultRequestHeaders.Add("Authorization", "ShippoToken shippo_test_ac3dd19acaa600c13da4b0bde6220d3da0c690b0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            HttpResponseMessage response = client.GetAsync("").Result;  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            if (response.IsSuccessStatusCode)
            {
                using (HttpContent content = response.Content) 
                {
                    Task<string> result = content.ReadAsStringAsync();
                    var trackingDetails = JsonConvert.DeserializeObject(result.Result);
                    Console.WriteLine(trackingDetails);
                    
                    Result trackingObject = new Result();
                    trackingObject.Carrier = carrier;
                    trackingObject.TrackingNumber = trackingNumber;
                    
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            client.Dispose();

            return RedirectToAction("Index", "Result");
        }
    }
}