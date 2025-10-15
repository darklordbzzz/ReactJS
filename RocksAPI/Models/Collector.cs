using System.Collections.Generic;

namespace RocksAPI.Models
{
    public class Collector
    {
        public string? CollectorId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? MemberSince { get; set; }
        public List<string>? PreferredCollectionTypes { get; set; }
        public string? Notes { get; set; }
    }
}