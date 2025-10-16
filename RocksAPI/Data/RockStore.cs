using RocksAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RocksAPI.Data
{
    public class RockStore
    {
        public RockData RockData { get; set; }

        public RockStore()
        {
            RockData = new RockData
            {
                Minerals = new List<Mineral>
                {
                    new Mineral
                    {
                        Id = "mindat-mineral-id-123",
                        Name = "Quartz",
                        Formula = "SiO2",
                        Variety = "Amethyst",
                        Crystallography = new Crystallography { CrystalSystem = "Trigonal", Habit = "Prismatic, drusy" },
                        PhysicalProperties = new PhysicalProperties { HardnessMohs = "7", SpecificGravity = "2.65", Luster = "Vitreous", Streak = "White", Cleavage = "None", Fracture = "Conchoidal" },
                        OpticalProperties = new OpticalProperties { Diaphaneity = "Transparent to translucent", Color = new List<string> { "Purple", "Violet" }, Pleochroism = "Weak" },
                        OriginOccurrence = new OriginOccurrence
                        {
                            Type = "Hydrothermal",
                            AssociatedMinerals = new List<string> { "Calcite", "Hematite" },
                            Localities = new List<LocalityInfo>
                            {
                                new LocalityInfo { LocalityId = "mindat-locality-id-456", Name = "Piedra Parada, Veracruz, Mexico", Country = "Mexico" }
                            }
                        },
                        MindatUrl = "https://www.mindat.org/min-3337.html",
                        Notes = "Commonly forms geode linings."
                    }
                },
                Rocks = new List<Rock>
                {
                    new Rock
                    {
                        Id = "rock-id-789",
                        Name = "Granite",
                        Type = "Igneous",
                        SubType = "Plutonic",
                        Texture = "Phaneritic",
                        MineralComposition = new List<MineralComposition>
                        {
                            new MineralComposition { MineralId = "mindat-mineral-id-123", Percentage = "25-35%", Name = "Quartz" },
                            new MineralComposition { MineralId = "mindat-mineral-id-987", Percentage = "30-40%", Name = "Feldspar (Orthoclase/Plagioclase)" },
                            new MineralComposition { MineralId = "mindat-mineral-id-654", Percentage = "5-15%", Name = "Biotite" }
                        },
                        Origin = "Formed from slow cooling magma underground.",
                        Notes = "Used extensively as a building material."
                    }
                },
                Samples = new List<Sample>
                {
                    new Sample
                    {
                        SampleId = "collector-sample-001",
                        CollectorId = "john-doe-01",
                        DateAcquired = "2023-10-26",
                        ItemType = "mineral",
                        MineralId = "mindat-mineral-id-123",
                        RockId = null,
                        LocalityId = "mindat-locality-id-456",
                        Description = "Stunning amethyst geode section, 15cm x 10cm, vibrant purple color.",
                        WeightGrams = 750.5,
                        DimensionsCm = new Dimensions { Length = 15, Width = 10, Height = 7 },
                        PricePaid = new Price { Amount = 120.00, Currency = "USD" },
                        Source = "Local mineral show, vendor: 'Gemstone Galore'",
                        Photos = new List<Photo>(),
                        Tags = new List<string> { "geode", "amethyst", "quartz", "display piece" },
                        Notes = "Great luster, minor damage on one edge."
                    },
                    new Sample
                    {
                        SampleId = "collector-sample-002",
                        CollectorId = "jane-smith-02",
                        DateAcquired = "2023-11-15",
                        ItemType = "rock",
                        MineralId = null,
                        RockId = "rock-id-789",
                        LocalityId = null,
                        Description = "Polished granite slab, excellent example of igneous texture.",
                        WeightGrams = 2500,
                        DimensionsCm = new Dimensions { Length = 30, Width = 20, Height = 2 },
                        PricePaid = new Price { Amount = 45.00, Currency = "USD" },
                        Source = "Online retailer: 'Rock Solid'",
                        Photos = new List<Photo>(),
                        Tags = new List<string> { "granite", "polished", "igneous" },
                        Notes = "Purchased for educational display."
                    }
                },
                Localities = new List<Locality>
                {
                    new Locality
                    {
                        LocalityId = "mindat-locality-id-456",
                        Name = "Piedra Parada, Veracruz",
                        Country = "Mexico",
                        StateProvince = "Veracruz",
                        Region = "Los Tuxtlas",
                        Latitude = 18.5,
                        Longitude = -95.0,
                        MindatUrl = "https://www.mindat.org/loc-2468.html",
                        Notes = "Famous for amethyst geodes and other volcanic minerals."
                    }
                },
                Collectors = new List<Collector>
                {
                    new Collector
                    {
                        CollectorId = "john-doe-01",
                        Name = "John Doe",
                        Email = "john.doe@example.com",
                        MemberSince = "2022-01-01",
                        PreferredCollectionTypes = new List<string> { "minerals", "fluorescent" },
                        Notes = "Focuses on display-quality minerals."
                    }
                }
            };
        }
    }
}