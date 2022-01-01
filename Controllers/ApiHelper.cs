using System.Net.Http.Headers;

namespace CarbonOffset.Controllers
{
    public static class ApiHelper
    {
        private static readonly string _GeoCodingApiUrl = "http://api.positionstack.com/v1/forward?access_key=a96751e085c89f0bd2e65a1a130f4aca&query=";

        async public static Task<CityInfo> GetLongLat(HttpClient client, string cityName)
        {
            using (HttpResponseMessage response = await client.GetAsync(_GeoCodingApiUrl + cityName))
            {
                response.EnsureSuccessStatusCode();
                {
                    using (HttpContent content = response.Content)
                    {
                        return await content.ReadFromJsonAsync<CityInfo>();
                    }
                }
            }
        }

        public static double toRadians(double angle) => angle * (Math.PI / 180); 
        public static double GetDistance(LatLng p1, LatLng p2)
        {
            double lon1 = toRadians(p1.Longitude);
            double lon2 = toRadians(p2.Longitude);
            double lat1 = toRadians(p1.Latitude);
            double lat2 = toRadians(p2.Latitude);

            double R = 6378.137;
            double D_Lat = lat2 - lat1;
            double D_Long = lon2 - lon1;
            double a = Math.Pow(Math.Sin(D_Lat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(D_Long / 2), 2);
            double c = 2 * Math.Asin(Math.Sqrt(a));
            return c * R;
        }

    }
}
