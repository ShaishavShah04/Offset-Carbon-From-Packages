using CarbonOffset.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using Microsoft.Net.Http.Headers;
using System.Text.Json;


namespace CarbonOffset.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

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
        public async Task<IActionResult> Result(Result resultObject)
        {
            string carrier = resultObject.Carrier;
            string trackingNumber = resultObject.TrackingNumber;
            string path = $"https://api.goshippo.com/tracks/{carrier}/{trackingNumber}";


            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, path)
            {
                Headers =
                {
                    { HeaderNames.Authorization, "ShippoToken shippo_test_ac3dd19acaa600c13da4b0bde6220d3da0c690b0" }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            Result trackingObject = new Result();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using (HttpContent content = httpResponseMessage.Content)
                {

                    RawTracking rawDetails = await content.ReadFromJsonAsync<RawTracking>();

                    trackingObject.Carrier = carrier;
                    trackingObject.TrackingNumber = trackingNumber;
                    trackingObject.StartingCity = rawDetails.address_from.city;
                    trackingObject.StartCountry = rawDetails.address_from.country;
                    trackingObject.DestinationCity = rawDetails.address_to.city;
                    trackingObject.DestinationCountry = rawDetails.address_to.country;

                }
            }
            else
            {
                trackingObject.Carrier = "UPS";
                trackingObject.TrackingNumber = "gOCVJC2oNInUnDGarQbJ+sr3qZQeTtLbQuM=";
                trackingObject.StartingCity = "Edmonton";
                trackingObject.StartCountry = "Canada";
                trackingObject.DestinationCity = "Victoria";
                trackingObject.DestinationCountry = "Canada";

                Console.WriteLine("{0} ({1})", (int)httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
            }
            return RedirectToAction("Index", "Result", trackingObject);
        }

        public class locationInfo
        { 
            public string city { get; set; }
            public string country { get; set; }

        }

        public class RawTracking
        {
            public locationInfo? address_from { get; set; }
            public locationInfo? address_to { get; set; }

            public locationInfo GetOriginInfo()
            {
                return address_from;
            }
        }
    }
}