using System.Collections.Generic;

namespace RocksAPI.Models
{
    public class Mineral
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Formula { get; set; }
        public string? Variety { get; set; }
        public Crystallography? Crystallography { get; set; }
        public PhysicalProperties? PhysicalProperties { get; set; }
        public OpticalProperties? OpticalProperties { get; set; }
        public OriginOccurrence? OriginOccurrence { get; set; }
        public string? MindatUrl { get; set; }
        public string? Notes { get; set; }
    }

    public class Crystallography
    {
        public string? CrystalSystem { get; set; }
        public string? Habit { get; set; }
    }

    public class PhysicalProperties
    {
        public string? HardnessMohs { get; set; }
        public string? SpecificGravity { get; set; }
        public string? Luster { get; set; }
        public string? Streak { get; set; }
        public string? Cleavage { get; set; }
        public string? Fracture { get; set; }
    }

    public class OpticalProperties
    {
        public string? Diaphaneity { get; set; }
        public List<string>? Color { get; set; }
        public string? Pleochroism { get; set; }
    }

    public class OriginOccurrence
    {
        public string? Type { get; set; }
        public List<string>? AssociatedMinerals { get; set; }
        public List<LocalityInfo>? Localities { get; set; }
    }

    public class LocalityInfo
    {
        public string? LocalityId { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
    }
}