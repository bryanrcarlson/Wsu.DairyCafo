﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.DataAccess.Core
{
    public class DefaultScenario
    {
        public readonly Scenario s;

        public DefaultScenario()
        {
            s = new Scenario()
            {
                DetailsUrl = "",
                Description = "Scenario generated by Dairy-CropSyst editor",
                PathToWeatherFile = "",
                Latitude = 0,
                Longitude = 0,
                StartDate = new DateTime(2010, 1, 1),
                StopDate = new DateTime(2010, 12, 31),
                Accumulations = 3,
                SimulationPeriodMode = "multiyear",
                IrrigationPumpModel = "",
                ParameterizedScenario = 1,
                Cow = new Cow()
                {
                    Id = "herd",
                    Enabled = true,
                    BodyMass_kg = 600,
                    DryMatterIntake_kg_d = 17.5,
                    MilkProduction_kg_d = 21.8,
                    CrudeProteinDiet_percent = 16.8,
                    StarchDiet_percent = 20,
                    AcidDetergentFiberDiet_percent = 30,
                    IsLactating = true,
                    MetabolizableEnergyDiet_MJ_d = 150,
                    PhManure_mol_L = 8
                },
                Barn = new Barn()
                {
                    Id = "barn",
                    Enabled = true,
                    ManureAlleyArea_m2 = 3010,
                    FlushSystem = true,
                    Bedding = "sand",
                    BeddingAddition = 1.5,
                    NumberCows_cnt = 900,
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
                    SourceFacility = "AD tank",
                    LiquidFacility = "CS tank",
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
                    SurfaceArea_m2 = 10000,
                    VolumeMax_m3 = 34900,
                    PH_mol_L = 8
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
                    Enabled = true,
                    ApplicationDate_date = new DateTime(2010, 4, 1),
                    AmountRemoved_percent = 90,
                    Repetition_d = 90,
                    SourceFacility_id = "lagoon",
                    TargetField_id = "corn"
                },
                Field = new Field()
                {
                    Id = "corn",
                    Enabled = true,
                    Area_ha = 2000,
                    Crop = "corn"
                }
            };
        }
    }
}
