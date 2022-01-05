using CarbonOffset.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CarbonOffset.Controllers
{
    public class ResultController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public ResultController(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        async public Task<IActionResult> Index(string TrackingNum, Result ResultObj)
        {

            // Fetching LongLat Info
            var httpClient = _httpClientFactory.CreateClient("LongLat");
            CityInfo OrgCityInfo = await ApiHelper.GetLongLat(httpClient,$"{ResultObj.StartingCity}, {ResultObj.StartCountry}");
            CityInfo DestCityInfo = await ApiHelper.GetLongLat(httpClient, $"{ResultObj.DestinationCity}, {ResultObj.DestinationCountry}");
            ResultObj.StartLat = OrgCityInfo.GetData().Latitude;
            ResultObj.StartLon = OrgCityInfo.GetData().Longitude;
            ResultObj.DestLat = DestCityInfo.GetData().Latitude;
            ResultObj.DestLon = DestCityInfo.GetData().Longitude;


            // Computing Distance Between 2 locations
            double DistanceKM = ApiHelper.GetDistance(OrgCityInfo.GetData(), DestCityInfo.GetData());
            DistanceKM = Math.Round(DistanceKM);
            // Computing Cost
            // ( 115g carbon / km ) -- https://www.carbonindependent.org/22.html 
            // ( $50/Ton of carbon ) -- https://www.canada.ca/en/environment-climate-change/services/climate-change/pricing-pollution-how-it-will-work/industry/pricing-carbon-pollution.html
            // Assume 1,000,000g = 1 Tons
            // $0.00575/km
            float CarbonDamage = (float)Math.Round(0.115 * DistanceKM);
            double Cost = Math.Round(0.00575 * DistanceKM, 2);
            // Map Url:
            // String to enter
            string queryString = $"{ResultObj.DestinationCity}, {ResultObj.DestinationCountry}";
            queryString = queryString.Replace(" ", "%20");
            string mapSrc = $"https://maps.google.com/maps?q={queryString}&t=&z=9&ie=UTF8&iwloc=&output=embed";

            // Creating the Result obj
            ResultObj.Distance = DistanceKM;
            ResultObj.Cost = Cost;
            ResultObj.CarbonEmissions = CarbonDamage;
            ResultObj.MapUrl = mapSrc;

            return View(ResultObj);
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
