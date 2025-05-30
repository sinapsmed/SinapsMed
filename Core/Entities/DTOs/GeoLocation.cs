namespace Core.Entities.DTOs
{
    public class GeoLocation
    {
        public string Status { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
    }
}