namespace CarbonOffset.Models
{
    public class Result
    {
        // Properties
        public int Id { get; set; }
        public string TrackingNumber { get; set; }
        public string StartingCity { get; set; }
        public string DestinationCity { get; set; }
        public string StartCountry { get; set; }
        public string DestinationCountry { get; set; }
        public int Distance { get; set; }
        public int Cost { get; set; }
    }
}
