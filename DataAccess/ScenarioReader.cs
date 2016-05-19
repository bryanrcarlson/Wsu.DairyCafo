using System;
using System.Collections.Generic;
using System.IO;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.DataAccess
{
    public class ScenarioReader : IScenarioReader
    {
        private readonly IScenarioFile dDp;
        private readonly IScenarioFile fDp;
        private readonly string fieldDirName;

        public ScenarioReader(
            IScenarioFile dairyScenario,
            IScenarioFile cropSystScenario,
            string fieldDirName = "Fields")
        {
            this.dDp = dairyScenario;
            this.fDp = cropSystScenario;
            this.fieldDirName = fieldDirName;
        }

        /// <summary>
        /// Attempts to load a Dairy-CropSyst scenario, including the CafoDairy
        /// file (*.NIFA_dairy_scenario) and a CropSyst scenario
        /// (*.CropSyst_scenario).
        /// <remark>Only loads the first CropSyst scenario found in the
        /// field directory</remark>
        /// </summary>
        /// <param name="filePath">Path to CafoDairy scenario file *.NIFA_dairy_scenario</param>
        /// <exception cref="ArgumentException">Cannot find/load CafoDairy scenario</exception>
        public void Load(string filePath)
        {
            bool didLoad = dDp.Load(filePath);
            if (!didLoad)
            {
                throw new ArgumentException("File path not valid");
            }

            if (!String.IsNullOrEmpty(dDp.LoadedPath))
            {
                // Load Field scenario
                string dairyDir = Path.GetDirectoryName(filePath);
                string fieldDir = Path.Combine(dairyDir, fieldDirName);

                if(Directory.Exists(fieldDir))
                {
                    string[] fields = Directory.GetDirectories(fieldDir);

                    if (fields.Length > 0)
                    {
                        // Currently only supporting one field
                        string fieldFile = Path.Combine(fields[0], ".CropSyst_scenario");
                        fDp.Load(fieldFile);
                    }
                }
            }
        }
        public Scenario Parse()
        {
            Scenario s = new Scenario();

            #region Scenario
            s.DetailsUrl = dDp.GetValueOnly("dairy scenario", "details_URL");
            s.Description = dDp.GetValue("dairy scenario", "description");
            s.Accumulations = 
                Convert.ToInt16(dDp.GetValueOnly("dairy scenario", 
                "accumulations"));
            s.SimulationPeriodMode = dDp.GetValueOnly("dairy scenario", 
                "simulation_period_mode");
            s.IrrigationPumpModel = dDp.GetValueOnly("dairy scenario", 
                "irrigation_pump_model");
            s.ParameterizedScenario =
                Convert.ToInt16(dDp.GetValueOnly("dairy scenario", 
                "parameterized_scenario"));
            s.Latitude =
                Convert.ToDouble(dDp.GetValueOnly("dairy scenario",
                "latitude"));
            s.Longitude =
                Convert.ToDouble(dDp.GetValueOnly("dairy scenario",
                "longitude"));
            string sd = dDp.GetValueOnly("dairy scenario", "start_date");
            s.StartDate = !String.IsNullOrEmpty(sd) 
                ? parseDateFromIniFile(sd) 
                : DateTime.Now;

            string ed = dDp.GetValueOnly("dairy scenario", "stop_date");
            s.StopDate = !String.IsNullOrEmpty(ed) 
                ? parseDateFromIniFile(ed) 
                : DateTime.Now;

            string w = dDp.GetValueOnly("dairy scenario", "weather");
            s.PathToWeatherFile = !String.IsNullOrEmpty(w)
                ? w
                : "";

            int manureSeparatorCount = 
                Convert.ToInt16(dDp.GetValueOnly("dairy scenario", "manure_separator:count"));
            int manureStorageCount =
                Convert.ToInt16(dDp.GetValueOnly("dairy scenario", "manure_storage:count"));
            int receiveOffFarmBiomassCount =
                Convert.ToInt16(dDp.GetValueOnly("dairy scenario", "receive_off_farm_biomass:count"));
            #endregion

            parseBarn(s);
            parseCow(s);
            parseManureSeparators(s, manureSeparatorCount);
            parseLagoon(s, manureStorageCount);
            parseFertigationManagement(s);
            parseReceiveOffFarmBiomass(s, receiveOffFarmBiomassCount);
            parseField(s);
            return s;
        }
        private void parseBarn(Scenario s)
        {
            string sect = "barn:1";
            string id = dDp.GetValue(sect, "ID");
            string enabled = dDp.GetValueOnly(sect, "enable");
            string surface_area = dDp.GetValueOnly(sect, "manure_alley_surface_area");
            string cows = dDp.GetValueOnly(sect, "cow_population");

            string flush_system = dDp.GetValueOnly(sect, "flush_system");
            string bedding = dDp.GetValueOnly(sect, "bedding");
            string bedding_addition = dDp.GetValueOnly(sect, "bedding_addition");
            string cow_description = dDp.GetValueOnly(sect, "cow_description");
            string cleaning_frequency = dDp.GetValueOnly(sect, "cleaning_frequency");
            string drinking_water_pump = dDp.GetValueOnly(sect, "drinking_water_pump");

            s.Barn = new Barn()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
                ManureAlleyArea_m2 = Convert.ToDouble(surface_area),
                NumberCows_cnt = Convert.ToDouble(cows),
                FlushSystem = Convert.ToBoolean(flush_system),
                Bedding = bedding,
                BeddingAddition = Convert.ToDouble(bedding_addition),
                CowDescription = cow_description,
                DrinkingWaterPump = drinking_water_pump,
                CleaningFrequency = Convert.ToInt16(cleaning_frequency)
            };
        }
        private void parseCow(Scenario s)
        {
            string sect = "cow_description:1";
            string id = dDp.GetValue(sect, "ID");
            string enabled = dDp.GetValueOnly(sect, "enable");
            string body_mass = dDp.GetValueOnly(sect, "body_mass");
            string dry_matter_intake = dDp.GetValueOnly(sect, "dry_matter_intake");
            string milk_production = dDp.GetValueOnly(sect, "milk_production");
            string diet_crude_protein = dDp.GetValueOnly(sect, "diet_crude_protein");

            string diet_starch = dDp.GetValueOnly(sect, "diet_starch");
            string diet_ADF = dDp.GetValueOnly(sect, "diet_ADF");
            string lactating = dDp.GetValueOnly(sect, "lactating");
            string diet_ME_intake = dDp.GetValueOnly(sect, "diet_ME_intake");
            string manure_pH = dDp.GetValueOnly(sect, "manure_pH");
            s.Cow = new Cow()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
                BodyMass_kg = Convert.ToDouble(body_mass),
                CrudeProteinDiet_percent = Convert.ToDouble(diet_crude_protein),
                DryMatterIntake_kg_d = Convert.ToDouble(dry_matter_intake),
                MilkProduction_kg_d = Convert.ToDouble(milk_production),
                StarchDiet_percent = Convert.ToDouble(diet_starch),
                AcidDetergentFiberDiet_percent = Convert.ToDouble(diet_ADF),
                IsLactating = Convert.ToBoolean(lactating),
                MetabolizableEnergyDiet_MJ_d = Convert.ToDouble(diet_ME_intake),
                PhManure_mol_L = Convert.ToDouble(manure_pH)
            };
        }
        private void parseReceiveOffFarmBiomass(
            Scenario s, 
            int receiveOffFarmBiomassCount)
        {
            
            if (receiveOffFarmBiomassCount > 0)
            {
                // Warning: Only handle one at this time
                string sect = "receive_off_farm_biomass:1";

                string id = dDp.GetValue(sect, "ID");
                string enable = dDp.GetValue(sect, "enable");
                string application_date = 
                    dDp.GetValue(sect, "application_date");
                string destination_facility_ID = 
                    dDp.GetValue(sect, "destination_facility_ID");
                string biomatter = dDp.GetValue(sect, "biomatter");

                string mass = dDp.GetValueOnly(sect, "mass");
                string h2o_kg = dDp.GetValueOnly(sect, "h2o_kg");
                string nitrogen_urea_kg = 
                    dDp.GetValueOnly(sect, "nitrogen_urea_kg");
                string nitrogen_ammonical_kg = 
                    dDp.GetValueOnly(sect, "nitrogen_ammonical_kg");
                string nitrogen_organic_kg = 
                    dDp.GetValueOnly(sect, "nitrogen_organic_kg");
                string carbon_inorganic_kg = 
                    dDp.GetValueOnly(sect, "carbon_inorganic_kg");
                string carbon_organic_fast_kg = 
                    dDp.GetValueOnly(sect, "carbon_organic_fast_kg");
                string carbon_organic_slow_kg = 
                    dDp.GetValueOnly(sect, "carbon_organic_slow_kg");
                string carbon_organic_resilient_kg = 
                    dDp.GetValueOnly(sect, "carbon_organic_resilient_kg");
                string phosphorus_inorganic_kg = 
                    dDp.GetValueOnly(sect, "phosphorus_inorganic_kg");
                string phosphorus_organic_kg = 
                    dDp.GetValueOnly(sect, "phosphorus_organic_kg");
                string potassium_inorganic_kg = 
                    dDp.GetValueOnly(sect, "potassium_inorganic_kg");
                string potassium_organic_kg = 
                    dDp.GetValueOnly(sect, "potassium_organic_kg");
                string decomposition_constant_fast = 
                    dDp.GetValueOnly(sect, "decomposition_constant_fast");
                string decomposition_constant_slow = 
                    dDp.GetValueOnly(sect, "decomposition_constant_slow");
                string decomposition_constant_resilient = 
                    dDp.GetValueOnly(sect, "decomposition_constant_resilient");

                Biomatter manure = new Biomatter()
                {
                    Mass_kg = Convert.ToDouble(mass),
                    H2o_kg = Convert.ToDouble(h2o_kg),
                    NitrogenUrea_kg = Convert.ToDouble(nitrogen_urea_kg),
                    NitrogenAmmonical_kg = Convert.ToDouble(nitrogen_ammonical_kg),
                    NitrogenOrganic_kg = Convert.ToDouble(nitrogen_organic_kg),
                    CarbonInorganic_kg = Convert.ToDouble(carbon_inorganic_kg),
                    CarbonOrganicFast_kg = Convert.ToDouble(carbon_organic_fast_kg),
                    CarbonOrganicSlow_kg = Convert.ToDouble(carbon_organic_slow_kg),
                    CarbonOrganicResilient_kg = Convert.ToDouble(carbon_organic_resilient_kg),
                    PhosphorusInorganic_kg = Convert.ToDouble(phosphorus_inorganic_kg),
                    PhosphorusOrganic_kg = Convert.ToDouble(phosphorus_organic_kg),
                    PotassiumInorganic_kg = Convert.ToDouble(potassium_inorganic_kg),
                    PotassiumOrganic_kg = Convert.ToDouble(potassium_organic_kg),
                    DecompositionConstantFast = Convert.ToDouble(decomposition_constant_fast),
                    DecompositionConstantSlow = Convert.ToDouble(decomposition_constant_slow),
                    DecompositionConstantResilient = Convert.ToDouble(decomposition_constant_resilient)
                };

                DateTime applicationDate = parseDateFromIniFile(application_date);

                s.ReceiveOffFarmBiomass = new ReceiveOffFarmBiomass()
                {
                    Id = id,
                    Enabled = Convert.ToBoolean(enable),
                    DestinationFacilityID = destination_facility_ID,
                    ApplicationDate = String.IsNullOrEmpty(application_date) ?
                        DateTime.MinValue :
                        parseDateFromIniFile(application_date),
                    BiomatterLabel = biomatter,
                    Biomatter = manure
                };
            }


        }
        private void parseManureSeparators(Scenario s, int manureSeparatorCount)
        {
            for (int i = 0; i < manureSeparatorCount; i++)
            {
                string sect = "manure_separator:" + (i + 1).ToString();

                string id = dDp.GetValue(sect, "ID");
                string enabled = dDp.GetValueOnly(sect, "enable");
                string style = dDp.GetValue(sect, "style");
                string source = dDp.GetValue(sect, "source_facility");
                string liquid = dDp.GetValue(sect, "liquid_facility");
                string solids = dDp.GetValue(sect, "solids_facility");

                ManureSeparator m = new ManureSeparator()
                {
                    Id = id,
                    Enabled = Convert.ToBoolean(enabled),
                    Style = style,
                    SourceFacility = source,
                    LiquidFacility = liquid,
                    SolidFacility = solids
                };

                switch (style)
                {
                    case "anaerobic digester":
                        s.AnaerobicDigester = new AnaerobicDigester(m);
                        break;
                    case "fiber separator":
                        s.CourseSeparator = new CourseSeparator(m);
                        break;
                    case "dissolved air flotation":
                        s.FineSeparator = new FineSeparator(m);
                        break;
                    case "ammonia stripper":
                        s.NutrientRecovery = new NutrientRecovery(m);
                        break;
                }
            }
        }
        private void parseFertigationManagement(Scenario s)
        {
            string sect = "fertigation_management";
            string id = dDp.GetValue(sect, "ID");
            string enabled = dDp.GetValueOnly(sect, "enable");
            string applicationMethod 
                = dDp.GetValueOnly(sect, "application_method");
            string date_string = dDp.GetValueOnly(sect, "application_date");

            
            string remove_per = dDp.GetValueOnly(sect, "removal");
            string repetition = dDp.GetValueOnly(sect, "num_days_to_repeat");
            string source = dDp.GetValueOnly(sect, "from_storage");
            string field = dDp.GetValueOnly(sect, "to_field");

            s.Fertigation = new Fertigation()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
                ApplicationMethod = applicationMethod,
                ApplicationDate_date = !String.IsNullOrEmpty(date_string)
                    ? parseDateFromIniFile(date_string)
                    : DateTime.Now,
                AmountRemoved_percent = Convert.ToDouble(remove_per),
                Repetition_d = Convert.ToInt16(repetition),
                SourceFacility_id = source,
                TargetField_id = field
            };
        }
        private void parseLagoon(Scenario s, int manureStorageCount)
        {
            for(int i = 0; i < manureStorageCount; i++)
            {
                string sect = "manure_storage:" + (i + 1).ToString();

                string style = dDp.GetValueOnly(sect, "style");
                if(style == "lagoon_aerobic")
                {
                    string id = dDp.GetValue(sect, "ID");
                    string enabled = dDp.GetValueOnly(sect, "enable");
                    string surface_area = dDp.GetValueOnly(sect, "surface_area");
                    string volume_max = dDp.GetValueOnly(sect, "volume_max");

                    string pH = dDp.GetValueOnly(sect, "pH");
                    string fresh_manure = dDp.GetValueOnly(sect, "fresh_manure");

                    s.Lagoon = new Lagoon()
                    {
                        Id = id,
                        Enabled = Convert.ToBoolean(enabled),
                        SurfaceArea_m2 = Convert.ToDouble(surface_area),
                        VolumeMax_m3 = Convert.ToDouble(volume_max),
                        Style = style,
                        PH_mol_L = Convert.ToDouble(pH),
                        DoesContainFreshManure = Convert.ToBoolean(fresh_manure)
                    };

                    break;
                }
            }
        }
        private void parseField(Scenario s)
        {
            if(String.IsNullOrEmpty(fDp.LoadedPath))
            {
                s.Field = new Field()
                {
                    Enabled = false,
                    Id = "field"
                };
            }
            else
            {
                string id = new DirectoryInfo(fDp.LoadedPath).Parent.ToString();
                string area = fDp.GetValueOnly("field", "size");
                string isEnabled = fDp.GetValueOnly("field", "enable");
                if (String.IsNullOrEmpty(isEnabled)) isEnabled = "false";
                s.Field = new Field()
                {
                    Id = id,
                    Enabled = Convert.ToBoolean(isEnabled),
                    Area_ha = Convert.ToDouble(area),
                    Crop = id
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">Date string in format of yyyydoy but could
        /// also be 0</param>
        /// <returns>Date in the form of a DateTime</returns>
        private DateTime parseDateFromIniFile(string date)
        {
            DateTime dt;

            // TODO: Decide how to deal with this, needs to interface with the UI and the model
            if (date == "0")
            {
                dt = DateTime.MinValue;
            } 
            else
            {
                int year = Convert.ToInt16(date.Substring(0, 4));
                int doy = Convert.ToInt16(date.Substring(4, 3));
                dt = new DateTime(year, 1, 1).AddDays(doy - 1);
            }
            
            return dt;
        }
    }
}
