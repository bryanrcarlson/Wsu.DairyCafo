using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.DataAccess.Tests.Helpers
{
    /// <summary>
    /// Helper class that returns different objects for use in tests.
    /// Also has readonly as expected values that are outside of the scope of
    /// the returned classes.
    /// <note>It is important to keep the readonly values in sync with
    /// values from the returned classes.</note>
    /// </summary>
    public class Injector
    {
        public readonly int NumberSeparatorsComplex = 4;
        public readonly int NumberSeparatorsSimple = 1;
        public readonly int NumberStorageComplex = 4;
        public readonly int NumberStorageSimple = 1;

        public Scenario GetComplexScenario()
        {
            Scenario s = new Scenario()
            {
                DetailsUrl = @"http://foo.io",
                Description = "This is just a test",
                PathToWeatherFile = @"D:\WsuData\NIFA\Simulation\Database\Weather\paterson.UED",
                StartDate = new DateTime(1979, 1, 1),
                StopDate = new DateTime(1979, 12, 31),
                Accumulations = 3,
                SimulationPeriodMode = "multiyear",
                IrrigationPumpModel = "",
                ParameterizedScenario = 1,
                Barn = new Barn()
                {
                    Id = "barn",
                    Enabled = true,
                    ManureAlleyArea_m2 = 7650,
                    FlushSystem = true,
                    Bedding = "sand",
                    BeddingAddition = 1.5,
                    NumberCows_cnt = 3000,
                    CowDescription = "herd",
                    CleaningFrequency = 3,
                    DrinkingWaterPump = ""
                },
                AnaerobicDigester = new AnaerobicDigester()
                {
                    Id = "AD",
                    Enabled = true,
                    Style = "anaerobic digester",
                    SourceFacility = "barn",
                    LiquidFacility = "AD tank",
                    SolidFacility = "<off-site>"
                },
                CourseSeparator = new CourseSeparator()
                {
                    Id = "course sep",
                    Enabled = true,
                    Style = "fiber separator",
                    SourceFacility = "AD tank",
                    LiquidFacility = "CS tank",
                    SolidFacility = "<off-site>"
                },
                FineSeparator = new FineSeparator()
                {
                    Id = "fine sep",
                    Enabled = true,
                    Style = "dissolved air flotation",
                    SourceFacility = "CS tank",
                    LiquidFacility = "FS tank",
                    SolidFacility = "<off-site>"
                },
                NutrientRecovery = new NutrientRecovery()
                {
                    Id = "nut rec",
                    Enabled = true,
                    Style = "ammonia stripper",
                    SourceFacility = "FS tank",
                    LiquidFacility = "lagoon",
                    SolidFacility = "<off-site>"
                },
                Lagoon = new Lagoon()
                {
                    Id = "lagoon",
                    Enabled = true,
                    Style = "lagoon_aerobic",
                    DoesContainFreshManure = true,
                    SurfaceArea_m2 = 38250.0,
                    VolumeMax_m3 = 153000.0,
                    PH_mol_L = 8.5
                },
                ReceiveOffFarmBiomass = new ReceiveOffFarmBiomass()
                {
                    Id = "init_lagoon",
                    Enabled = true,
                    ApplicationDate = DateTime.MinValue,
                    DestinationFacilityID = "lagoon",
                    BiomatterLabel = "",
                    Biomatter = new Biomatter()
                    {
                        Mass_kg = 28672.3,
                        H2o_kg = 1099210,
                        NitrogenUrea_kg = 0,
                        NitrogenAmmonical_kg = 1620.69,
                        NitrogenOrganic_kg = 1280.05,
                        CarbonInorganic_kg = 549.35,
                        CarbonOrganicFast_kg = 9543.61,
                        CarbonOrganicSlow_kg = 3073.16,
                        CarbonOrganicResilient_kg = 1743.25,
                        PhosphorusInorganic_kg = 695.841,
                        PhosphorusOrganic_kg = 53.41,
                        PotassiumInorganic_kg = 2050.08,
                        PotassiumOrganic_kg = 0,
                        DecompositionConstantFast = 0.254867,
                        DecompositionConstantSlow = 0.015624,
                        DecompositionConstantResilient = 2.74e-6
                    }
                },
                Fertigation = new Fertigation()
                {
                    Id = "fert-management",
                    Enabled = true,
                    ApplicationDate_date = new DateTime(2015, 12, 31),
                    AmountRemoved_percent = 90,
                    Repetition_d = 365,
                    SourceFacility_id = "lagoon",
                    TargetField_id = "corn"
                },
                Cow = new Cow()
                {
                    Id = "herd",
                    Enabled = true,
                    BodyMass_kg = 600.00,
                    DryMatterIntake_kg_d = 17.50,
                    MilkProduction_kg_d = 21.80,
                    CrudeProteinDiet_percent = 16.8,
                    StarchDiet_percent = 0.0,
                    AcidDetergentFiberDiet_percent = 0,
                    IsLactating = true,
                    MetabolizableEnergyDiet_MJ_d = 150,
                    PhManure_mol_L = 8
                },
                Field = new Field()
                {
                    Id = "field",
                    Enabled = false,
                    Area_ha = 2000
                }
            };

            return s;
        }
        public Scenario GetSimpleScenario()
        {
            Scenario s = new Scenario()
            {
                DetailsUrl = @"http://foo.io",
                Description = "This is just a test",
                PathToWeatherFile = @"D:\WsuData\NIFA\Simulation\Database\Weather\paterson.UED",
                StartDate = new DateTime(1979, 1, 1),
                StopDate = new DateTime(1979, 12, 31),
                Accumulations = 3,
                SimulationPeriodMode = "multiyear",
                IrrigationPumpModel = "",
                ParameterizedScenario = 1,
                Barn = new Barn()
                {
                    Id = "barn",
                    Enabled = true,
                    ManureAlleyArea_m2 = 3000,
                    FlushSystem = true,
                    Bedding = "sand",
                    BeddingAddition = 1.5,
                    NumberCows_cnt = 1000,
                    CowDescription = "herd",
                    CleaningFrequency = 3,
                    DrinkingWaterPump = ""
                },
                AnaerobicDigester = new AnaerobicDigester()
                {
                    Id = "AD",
                    Enabled = false,
                    Style = "anaerobic digester",
                    SourceFacility = "barn",
                    LiquidFacility = "AD tank",
                    SolidFacility = "<off-site>"
                },
                CourseSeparator = new CourseSeparator()
                {
                    Id = "course sep",
                    Enabled = true,
                    Style = "fiber separator",
                    SourceFacility = "barn",
                    LiquidFacility = "lagoon",
                    SolidFacility = "<off-site>"
                },
                FineSeparator = new FineSeparator()
                {
                    Id = "fine sep",
                    Enabled = false,
                    Style = "dissolved air flotation",
                    SourceFacility = "CS tank",
                    LiquidFacility = "FS tank",
                    SolidFacility = "<off-site>"
                },
                NutrientRecovery = new NutrientRecovery()
                {
                    Id = "nut rec",
                    Enabled = false,
                    Style = "ammonia stripper",
                    SourceFacility = "FS tank",
                    LiquidFacility = "lagoon",
                    SolidFacility = "<off-site>"
                },
                Lagoon = new Lagoon()
                {
                    Id = "lagoon",
                    Enabled = true,
                    Style = "lagoon_aerobic",
                    DoesContainFreshManure = true,
                    SurfaceArea_m2 = 142250,
                    VolumeMax_m3 = 519213,
                    PH_mol_L = 8.5
                },
                ReceiveOffFarmBiomass = new ReceiveOffFarmBiomass()
                {
                    Id = "init_lagoon",
                    Enabled = false,
                    ApplicationDate = DateTime.MinValue,
                    DestinationFacilityID = "lagoon",
                    BiomatterLabel = "",
                    Biomatter = new Biomatter()
                    {
                        Mass_kg = 28672.3,
                        H2o_kg = 1099210,
                        NitrogenUrea_kg = 0,
                        NitrogenAmmonical_kg = 1620.69,
                        NitrogenOrganic_kg = 1280.05,
                        CarbonInorganic_kg = 549.35,
                        CarbonOrganicFast_kg = 9543.61,
                        CarbonOrganicSlow_kg = 3073.16,
                        CarbonOrganicResilient_kg = 1743.25,
                        PhosphorusInorganic_kg = 695.841,
                        PhosphorusOrganic_kg = 53.41,
                        PotassiumInorganic_kg = 2050.08,
                        PotassiumOrganic_kg = 0,
                        DecompositionConstantFast = 0.254867,
                        DecompositionConstantSlow = 0.015624,
                        DecompositionConstantResilient = 2.74e-6
                    }
                },
                Fertigation = new Fertigation()
                {
                    Id = "fert-management",
                    Enabled = false,
                    ApplicationDate_date = new DateTime(2015, 12, 31),
                    AmountRemoved_percent = 90,
                    Repetition_d = 365,
                    SourceFacility_id = "lagoon",
                    TargetField_id = "corn"
                },
                Cow = new Cow()
                {
                    Id = "herd",
                    Enabled = true,
                    BodyMass_kg = 635,
                    DryMatterIntake_kg_d = 24,
                    MilkProduction_kg_d = 34,
                    CrudeProteinDiet_percent = 17.6,
                    StarchDiet_percent = 0.0,
                    AcidDetergentFiberDiet_percent = 0,
                    IsLactating = true,
                    MetabolizableEnergyDiet_MJ_d = 150,
                    PhManure_mol_L = 8
                },
                Field = new Field()
                {
                    Id = "field",
                    Enabled = false,
                    Area_ha = 2000
                }
            };

            return s;
        }
    }
}
