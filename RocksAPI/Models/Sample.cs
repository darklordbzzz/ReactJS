using System.Collections.Generic;

namespace RocksAPI.Models
{
    public class Sample
    {
        public string? SampleId { get; set; }
        public string? CollectorId { get; set; }
        public string? DateAcquired { get; set; }
        public string? ItemType { get; set; }
        public string? MineralId { get; set; }
        public string? RockId { get; set; }
        public string? LocalityId { get; set; }
        public string? Description { get; set; }
        public double WeightGrams { get; set; }
        public Dimensions? DimensionsCm { get; set; }
        public Price? PricePaid { get; set; }
        public string? Source { get; set; }
        public List<Photo>? Photos { get; set; }
        public List<string>? Tags { get; set; }
        public string? Notes { get; set; }
    }

    public class Dimensions
    {
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Price
    {
        public double Amount { get; set; }
        public string? Currency { get; set; }
    }

    public class Photo
    {
        public string? Url { get; set; }
        public string? Caption { get; set; }
    }
}