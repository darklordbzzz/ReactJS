using System.Collections.Generic;

namespace RocksAPI.Models
{
    public class RockData
    {
        public List<Mineral>? Minerals { get; set; }
        public List<Rock>? Rocks { get; set; }
        public List<Sample>? Samples { get; set; }
        public List<Locality>? Localities { get; set; }
        public List<Collector>? Collectors { get; set; }
    }
}