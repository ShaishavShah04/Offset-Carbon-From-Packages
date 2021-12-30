using CarbonOffset.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarbonOffset.Controllers
{
    public class ResultController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _GeoCodingApiUrl = "http://api.positionstack.com/v1/forward?access_key=a96751e085c89f0bd2e65a1a130f4aca&query=";
        public ResultController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        async public Task<IActionResult> Index(string TrackingNum, string OrgCity = "Edmonton,AB", string DestCity = "Victoria,BC")
        {
            var httpClient = _httpClientFactory.CreateClient("Distance");

            using (HttpResponseMessage response = await httpClient.GetAsync(_GeoCodingApiUrl + OrgCity))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (HttpContent content = response.Content)
                    {
                        CityInfo data = await content.ReadFromJsonAsync<CityInfo>();
                        Console.WriteLine(data);
                    }
                }
            }
            var testResult = new Result();
            return View(testResult);
        }
    }

    public class LatLng
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string? Label { get; set; }
    }

    public class CityInfo
    {
        public List<LatLng>? Data { get; set; }
    }

    //async private Task<CityInfo> GetLongLat(string cityName)
    //{

    //} 
}
