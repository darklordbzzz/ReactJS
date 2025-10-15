using System.Collections.Generic;

namespace RocksAPI.Models
{
    public class Rock
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? SubType { get; set; }
        public string? Texture { get; set; }
        public List<MineralComposition>? MineralComposition { get; set; }
        public string? Origin { get; set; }
        public string? Notes { get; set; }
    }

    public class MineralComposition
    {
        public string? MineralId { get; set; }
        public string? Percentage { get; set; }
        public string? Name { get; set; }
    }
}