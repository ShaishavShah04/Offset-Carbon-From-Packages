namespace CarbonOffset.Models
{
    public class Result
    {
        // Properties
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string StartingCity { get; set; }
        public string DestinationCity { get; set; }
        public string StartCountry { get; set; }
        public string DestinationCountry { get; set; }
        public float CarbonEmissions { get; set; }
        public float StartLat { get; set; }
        public float StartLon { get; set; }
        public float DestLat { get; set; }
        public float DestLon { get; set; }
        public double Distance { get; set; }
        public double Cost { get; set; }
        public string MapUrl { get; set; }
    }
}
