namespace RocksAPI.Models
{
    public class Locality
    {
        public string? LocalityId { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? StateProvince { get; set; }
        public string? Region { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? MindatUrl { get; set; }
        public string? Notes { get; set; }
    }
}