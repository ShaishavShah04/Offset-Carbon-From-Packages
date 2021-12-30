using CarbonOffset.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarbonOffset.Controllers
{
    public class ResultController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public ResultController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        async public Task<IActionResult> Index(string TrackingNum, string OrgCity = "Edmonton,AB", string DestCity = "Victoria,BC")
        {
            // Fetching LongLat Info

            var httpClient = _httpClientFactory.CreateClient("LongLat");
            CityInfo OrgCityInfo = await ApiHelper.GetLongLat(httpClient, OrgCity);
            CityInfo DestCityInfo = await ApiHelper.GetLongLat(httpClient, DestCity);

            // Computing Distance Between 2 locations
            double DistanceKM = ApiHelper.GetDistance(OrgCityInfo.GetData(), DestCityInfo.GetData());

            // Creating the Result obj
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

        public LatLng GetData() => Data[0];
    }

}
