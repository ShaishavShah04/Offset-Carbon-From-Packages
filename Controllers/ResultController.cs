﻿using CarbonOffset.Models;
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

            // Computing Distance Between 2 locations
            double DistanceKM = ApiHelper.GetDistance(OrgCityInfo.GetData(), DestCityInfo.GetData());

            // Computing Cost
            // ( 115g carbon / km ) -- https://www.carbonindependent.org/22.html 
            // ( $50/Ton of carbon ) -- https://www.canada.ca/en/environment-climate-change/services/climate-change/pricing-pollution-how-it-will-work/industry/pricing-carbon-pollution.html
            // Assume 1,000,000g = 1 Tons
            // $0.00575/km

            double Cost = 0.00575 * DistanceKM;
            // Testing purposes:
            
            // Creating the Result obj
            ResultObj.Distance = DistanceKM;
            ResultObj.Cost = Cost;

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
