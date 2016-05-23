using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.DataAccess
{
    public class ScenarioWriter : IScenarioWriter
    {
        private readonly IScenarioFile dDp;
        private readonly IScenarioFile fDp;
        private readonly IScenarioDefaults defaults;
        private readonly IWeatherExtractor weatherExtractor;
        private readonly string fieldDirName;
        private readonly string dairyScenarioFilename;
        private readonly string fieldScenarioFilename;

        public ScenarioWriter(
            IScenarioFile dairyScenario,
            IScenarioFile cropSystScenario,
            IScenarioDefaults defaults,
            IWeatherExtractor weatherExtractor,
            string fieldDirName = "Fields",
            string dairyScenarioFilename = ".NIFA_dairy_scenario",
            string fieldScenarioFilename = ".CropSyst_scenario"
            )
        {
            this.dDp = dairyScenario;
            this.fDp = cropSystScenario;
            this.defaults = defaults;
            this.weatherExtractor = weatherExtractor;
            this.fieldDirName = fieldDirName;
            this.dairyScenarioFilename = dairyScenarioFilename;
            this.fieldScenarioFilename = fieldScenarioFilename;
        }
        /// <summary>
        /// Sets up a Dairy-CropSyst scenario directory
        /// </summary>
        /// <param name="dirPath">
        ///  Path where scenario files/folder are created
        /// </param>
        public void SetupDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                throw new NullReferenceException("Dir does not exist");

            Directory.CreateDirectory(Path.Combine(dirPath, "Output"));
            Directory.CreateDirectory(Path.Combine(dirPath, "Fields"));
        }
        public void Write(Scenario s, string dirPath)
        {
            if (!Directory.Exists(dirPath))
                throw new NullReferenceException("Dir does not exist");

            string filePath = Path.Combine(dirPath, dairyScenarioFilename);

            if (File.Exists(filePath))
                throw new InvalidOperationException("File already exists, aborting");

            File.Create(filePath).Close();

            dDp.Load(filePath);

            Write(s);
        }

        /// <summary>
        /// Write a Wsu.DairyCafo.DataAccess.Dto.Scenario to a Dairy-CropSyst
        /// parameter file.
        /// <note>If file specified by Scenario exists, then overwrites values.
        /// This function deletes all [fertigation:x] and [manure_storage:x]
        /// sections before writing new ones.</note>
        /// </summary>
        /// <param name="scenario"></param>
        public void Write(Scenario scenario)
        {
            if (String.IsNullOrEmpty(dDp.LoadedPath))
                throw new NullReferenceException("Cannot write, no scenario file loaded");

            // Clear scenario file before writing
            dDp.Clear();
            // NOTE: Use 'clean()' if user needs custom sections defined
            //clean();

            dDp.SetSection("version", defaults.GetVersionDefaults());

            //var sVals = defaults.GetScenarioDefaults();
            var sVals = new Dictionary<string, string>();
            sVals.Add("details_URL", scenario.DetailsUrl);
            sVals.Add("description", scenario.Description);
            sVals.Add("weather", scenario.PathToWeatherFile.ToString());
            sVals.Add("latitude", scenario.Latitude.ToString());
            sVals.Add("longitude", scenario.Longitude.ToString());
            sVals.Add("start_date", getYYYYDOYString(scenario.StartDate));
            sVals.Add("stop_date", getYYYYDOYString(scenario.StopDate));
            sVals.Add("accumulations", scenario.Accumulations.ToString());
            sVals.Add("simulation_period_mode", scenario.SimulationPeriodMode.ToString());
            sVals.Add("irrigation_pump_model", scenario.IrrigationPumpModel.ToString());
            sVals.Add("parameterized_scenario", scenario.ParameterizedScenario.ToString());
            sVals.Add("cow_description:count", scenario.GetCountCow().ToString());
            sVals.Add("barn:count", scenario.GetCountBarn().ToString());
            sVals.Add("manure_separator:count", scenario.GetCountManureSeparator().ToString());
            sVals.Add("manure_storage:count", scenario.GetCountManureStorage().ToString());
            sVals.Add("fertigation:count", scenario.GetCountFertigation().ToString());
            sVals.Add("receive_off_farm_biomass:count", scenario.GetCountReceiveOffFarmBiomass().ToString());
            dDp.SetSection("dairy scenario", sVals);

            writeCow(scenario);
            writeBarn(scenario);
            
            writeSeparatorsAndStorage(scenario);
            int numSeparators = scenario.GetCountManureSeparator();
            int numStorageTanks = scenario.GetCountManureStorage();
            writeLagoon(scenario);
            int numReceiveOffFarmBiomass = writeReceiveOffFarmBiomass(scenario);
            writeFertigationManagement(scenario);

            int numFertigations = writeFertigations(scenario);
 
            if (!dDp.Save(dDp.LoadedPath, getSectionOrder(scenario)))
            {
                throw new OperationCanceledException("Failed to save file"); 
            }
                  
        }
        public string SetupWeather(Scenario scenario)
        {
            // Find current weather file
            // Copy files from database to Fields directory if not already present
            
            //throw new NotImplementedException();
            string nearestWeather = weatherExtractor.GetWeather(
                    scenario.Latitude, 
                    scenario.Longitude);

            return nearestWeather;
        }
        public bool clean()
        {
            // Clean [fertigation:x] and [manure_storage:x], keep lagoon
            Dictionary<string, string> lagoon = new Dictionary<string, string>();

            if (dDp.GetSection("manure_storage:1").Count > 0)
            {
                bool isLagoon = false;
                int count = 1;

                do
                {
                    string sect = "manure_storage:" + count;
                    string ID = dDp.GetValue(sect, "ID");
                    lagoon = dDp.GetSection(sect);
                    if (ID == "lagoon") isLagoon = true;
                    count++;
                } while (!isLagoon);
            }

            // Clean
            dDp.Clear("fertigation");
            dDp.Clear("manure_storage");
            dDp.Clear("manure_separator");

            // Re-add lagoon
            if (lagoon.Count > 0)
                dDp.SetSection("manure_storage:1", lagoon);

            return true;
        }
        public void WriteField(Scenario s)
        {
            try
            {
                copyField(s);
                writeField(s);
            }
            catch (NullReferenceException e)
            {
                throw e;
            }
        }
        private void writeBarn(Scenario s)
        {
            //var vals = defaults.GetBarnDefaults();
            var vals = new Dictionary<string, string>();
            vals.Add("ID", s.Barn.Id);
            vals.Add("enable", s.Barn.Enabled.ToString().ToLower());
            vals.Add("manure_alley_surface_area",
                s.Barn.ManureAlleyArea_m2.ToString());
            vals.Add("flush_system", s.Barn.FlushSystem.ToString().ToLower());
            vals.Add("bedding", s.Barn.Bedding);
            vals.Add("bedding_addition", s.Barn.BeddingAddition.ToString());
            vals.Add("cow_population",
                s.Barn.NumberCows_cnt.ToString());
            vals.Add("cow_description", s.Barn.CowDescription);
            vals.Add("cleaning_frequency", s.Barn.CleaningFrequency.ToString());

            dDp.SetSection("barn:1", vals);
        }
        private void writeCow(Scenario s)
        {
            var vals = new Dictionary<string, string>();

            //var vals = defaults.GetCowDefaults();
            vals.Add("ID", s.Cow.Id);
            vals.Add("enable", s.Cow.Enabled.ToString().ToLower());
            vals.Add("body_mass", s.Cow.BodyMass_kg.ToString());
            vals.Add("dry_matter_intake", s.Cow.DryMatterIntake_kg_d.ToString());
            vals.Add("milk_production", s.Cow.MilkProduction_kg_d.ToString());
            vals.Add("diet_crude_protein", s.Cow.CrudeProteinDiet_percent.ToString());

            vals.Add("diet_starch", s.Cow.StarchDiet_percent.ToString());
            vals.Add("diet_ADF", s.Cow.AcidDetergentFiberDiet_percent.ToString());
            vals.Add("lactating", s.Cow.IsLactating.ToString().ToLower());
            vals.Add("diet_ME_intake", s.Cow.MetabolizableEnergyDiet_MJ_d.ToString());
            vals.Add("manure_pH", s.Cow.PhManure_mol_L.ToString());

            dDp.SetSection("cow_description:1", vals);
        }
        private void writeLagoon(Scenario s)
        {
            //var vals = defaults.GetLagoonDefaults();
            var vals = new Dictionary<string, string>();
            vals.Add("ID", s.Lagoon.Id);
            vals.Add("enable", s.Lagoon.Enabled.ToString().ToLower());
            vals.Add("style", s.Lagoon.Style);
            vals.Add("fresh_manure", 
                s.Lagoon.DoesContainFreshManure.ToString().ToLower());
            vals.Add("surface_area", s.Lagoon.SurfaceArea_m2.ToString());
            vals.Add("volume_max", s.Lagoon.VolumeMax_m3.ToString());
            vals.Add("pH", s.Lagoon.PH_mol_L.ToString());
            // Lagoon is always the last manure storage
            int numStorage = s.GetCountManureStorage();
            dDp.SetSection("manure_storage:"+numStorage.ToString(), vals);
        }
        private int writeReceiveOffFarmBiomass(Scenario s)
        {
            if (s.ReceiveOffFarmBiomass.Enabled)
            {
                var vals = new Dictionary<string, string>();
                vals.Add("ID", s.ReceiveOffFarmBiomass.Id);
                vals.Add("enable", s.ReceiveOffFarmBiomass.Enabled.ToString().ToLower());
                // Determine application date -- assume DateTime.Min means 0
                if (s.ReceiveOffFarmBiomass.ApplicationDate == DateTime.MinValue)
                    vals.Add("application_date", "0");
                else
                    vals.Add("application_date", getYYYYDOYString(s.ReceiveOffFarmBiomass.ApplicationDate));
                vals.Add("destination_facility_ID", s.ReceiveOffFarmBiomass.DestinationFacilityID);
                vals.Add("mass", s.ReceiveOffFarmBiomass.Biomatter.Mass_kg.ToString());
                vals.Add("biomatter", s.ReceiveOffFarmBiomass.BiomatterLabel);
                vals.Add("h2o_kg", s.ReceiveOffFarmBiomass.Biomatter.H2o_kg.ToString());
                vals.Add("nitrogen_urea_kg", s.ReceiveOffFarmBiomass.Biomatter.NitrogenUrea_kg.ToString());
                vals.Add("nitrogen_ammonical_kg", s.ReceiveOffFarmBiomass.Biomatter.NitrogenAmmonical_kg.ToString());
                vals.Add("nitrogen_organic_kg", s.ReceiveOffFarmBiomass.Biomatter.NitrogenOrganic_kg.ToString());
                vals.Add("carbon_inorganic_kg", s.ReceiveOffFarmBiomass.Biomatter.CarbonInorganic_kg.ToString());
                vals.Add("carbon_organic_fast_kg", s.ReceiveOffFarmBiomass.Biomatter.CarbonOrganicFast_kg.ToString());
                vals.Add("carbon_organic_slow_kg", s.ReceiveOffFarmBiomass.Biomatter.CarbonOrganicSlow_kg.ToString());
                vals.Add("carbon_organic_resilient_kg", s.ReceiveOffFarmBiomass.Biomatter.CarbonOrganicResilient_kg.ToString());
                vals.Add("phosphorus_inorganic_kg", s.ReceiveOffFarmBiomass.Biomatter.PhosphorusInorganic_kg.ToString());
                vals.Add("phosphorus_organic_kg", s.ReceiveOffFarmBiomass.Biomatter.PhosphorusOrganic_kg.ToString());
                vals.Add("potassium_inorganic_kg", s.ReceiveOffFarmBiomass.Biomatter.PotassiumInorganic_kg.ToString());
                vals.Add("potassium_organic_kg", s.ReceiveOffFarmBiomass.Biomatter.PotassiumOrganic_kg.ToString());
                vals.Add("decomposition_constant_fast", s.ReceiveOffFarmBiomass.Biomatter.DecompositionConstantFast.ToString());
                vals.Add("decomposition_constant_slow", s.ReceiveOffFarmBiomass.Biomatter.DecompositionConstantSlow.ToString());
                vals.Add("decomposition_constant_resilient", s.ReceiveOffFarmBiomass.Biomatter.DecompositionConstantResilient.ToString().ToLower());

                dDp.SetSection("receive_off_farm_biomass:1", vals);

                return 1;
            }
            else { return 0;  }
        }
        private int writeSeparatorsAndStorage(Scenario s)
        {
            SortedList<int,ManureSeparator> seps = sortManureSeparators(s);
            int sepsWritten = 0;
            Dictionary<string, string> manureStorageDict = 
                new Dictionary<string, string>();
            string currSectionId = "";

            // First in list will always get manure from barn
            string sourceId = s.Barn.Id;

            for(int i = 0; i < seps.Count; i++)
            {
                seps[i].SourceFacility = sourceId;
               
                if (seps.Count > (i+1))
                {
                    // There is another separator, so pass to holding tank
                    string liqId = getHoldingTankId(seps[i].Style);
                    seps[i].LiquidFacility = liqId;

                    // Set source for next separator
                    sourceId = liqId;

                    // Write holding tank
                    string sect = "manure_storage:" + (i + 1).ToString(); // +1 because zerobased
                   
                    var d = new Dictionary<string, string>();
                    d.Add("ID", seps[i].LiquidFacility);
                    // Merges the default dictionary into the curr dictionary
                    //avoiding duplicates
                    foreach(var item in defaults.GetHoldingTankDefaults())
                    {
                        if (!d.ContainsKey(item.Key))
                            d.Add(item.Key, item.Value);
                    }

                    manureStorageDict = d;
                    currSectionId = sect;

                    
                }
                else // This is last separator, so pass to lagoon
                {
                    seps[i].LiquidFacility = s.Lagoon.Id;
                }

                // All separators export solids off-site for now
                seps[i].SolidFacility = "<off-site>";

                // Write separator
                string manSepCnt = "manure_separator:" + (i + 1).ToString();
                Dictionary<string, string> vals = new Dictionary<string, string>();
                vals.Add("ID", seps[i].Id);
                vals.Add("enable",seps[i].Enabled.ToString().ToLower());
                vals.Add("style", seps[i].Style);
                vals.Add("source_facility", seps[i].SourceFacility);
                vals.Add("liquid_facility", seps[i].LiquidFacility);
                vals.Add("solids_facility", seps[i].SolidFacility);

                dDp.SetSection(manSepCnt, vals);
                if(manureStorageDict.Count > 0)
                    dDp.SetSection(currSectionId,manureStorageDict);

                sepsWritten++;
            }

            return sepsWritten;
        }
        private void writeFertigationManagement(Scenario s)
        {
            // Write fert management (unique to this application)
            //var vals = defaults.GetFertigationDefaults();
            var vals = new Dictionary<string, string>();
            vals.Add("ID", s.Fertigation.Id);
            vals.Add("enable", s.Fertigation.Enabled.ToString().ToLower());
            vals.Add("application_method", s.Fertigation.ApplicationMethod);
            vals.Add("application_date",
                getYYYYDOYString(s.Fertigation.ApplicationDate_date));
            vals.Add("removal", s.Fertigation.AmountRemoved_percent.ToString());
            vals.Add("num_days_to_repeat", s.Fertigation.Repetition_d.ToString());
            vals.Add("from_storage", s.Lagoon.Id);
            vals.Add("to_field", String.IsNullOrEmpty(s.Field.Id) 
                ? "" 
                : s.Field.Id);

            dDp.SetSection("fertigation_management", vals);
        }
        private int writeFertigations(Scenario s)
        {
            // Wire up values

            if (!s.Fertigation.Enabled)
                return 0;

            s.Fertigation.SourceFacility_id = s.Lagoon.Id;
            s.Fertigation.TargetField_id = 
                String.IsNullOrEmpty(s.Field.Id) 
                    ? "<off-site>"
                    : s.Field.Id;

            int i = 0;
            if(s.Fertigation.Repetition_d > 0)
            {
                // Expand fert management to instances of ferts
                DateTime currDt = s.Fertigation.ApplicationDate_date;
                
                do
                {
                    i++;
                    writeFertigation(s.Fertigation, i, getYYYYDOYString(currDt));
                    currDt = currDt.AddDays(s.Fertigation.Repetition_d);
                } while (currDt < s.StopDate);
            }
            else
            {
                // Just write one fert
                i++;
                writeFertigation(s.Fertigation, i, getYYYYDOYString(
                    s.Fertigation.ApplicationDate_date));
                
            }

            return i;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="f">Fertigation to write</param>
        /// <param name="IdCount">Not zero based, starts at 1</param>
        private void writeFertigation(Fertigation f, int IdCount, string date)
        {
            string sect = "fertigation:" + IdCount.ToString();

            //var vals = defaults.GetFertigationDefaults();
            var vals = new Dictionary<string, string>();
            vals.Add("ID", "fert" + date);
            vals.Add("enable", f.Enabled.ToString().ToLower());
            vals.Add("application_method", f.ApplicationMethod);
            vals.Add("application_date", date);        
            vals.Add("removal", f.AmountRemoved_percent.ToString());
            vals.Add("from_storage", f.SourceFacility_id);
            vals.Add("to_field", f.TargetField_id);

            dDp.SetSection(sect, vals);
        }
        private void copyField(Scenario s)
        {
            if (String.IsNullOrEmpty(fDp.LoadedPath))
                throw new NullReferenceException("Error setting up field; path not loaded");

            string dirName = 
                new DirectoryInfo(Path.GetDirectoryName(fDp.LoadedPath)).Name;

            if(s.Field.Enabled && dirName != s.Field.Crop)
            {
                // Burn current field dir and copy a new one from templates
                string SourcePath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\Fields", s.Field.Crop);
                if (!Directory.Exists(SourcePath))
                    throw new ArgumentException("Error setting up field; specified crop does not exist");
                string currFieldDir = Path.GetDirectoryName(fDp.LoadedPath);
                string DestinationPath = Path.Combine(Directory.GetParent(currFieldDir).ToString(), s.Field.Crop);
                
                Directory.Delete(Path.GetDirectoryName(fDp.LoadedPath), true);
                Directory.CreateDirectory(DestinationPath);

                //Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

                // Reload file
                fDp.Load(Path.Combine(DestinationPath, fieldScenarioFilename));
            }
        }
        private void writeField(Scenario s)
        {
            if (String.IsNullOrEmpty(fDp.LoadedPath))
                throw new NullReferenceException("Cannot write field scenario; no file loaded");

            // Delete fertigation file
            string fertPath = Path.Combine(
                Path.GetDirectoryName(fDp.LoadedPath),
                //"Database",
                //"Management",
                "fertigation.CS_management");
            if (File.Exists(fertPath))
                File.Delete(fertPath);

            // Set field area
            fDp.SetValue("field", "size", s.Field.Area_ha.ToString());

            // Set enabled
            fDp.SetValue("field", "enable", s.Field.Enabled.ToString().ToLower());

            // Set fixed management to apply fertigation from manure storage (lagoon)
            string fixedManagementPath = "";
            if (s.Fertigation.Enabled) fixedManagementPath = fertPath;
            fDp.SetValue(
                    "parameter_filenames", 
                    "fixed_management", 
                    fixedManagementPath);

            
            if (File.Exists(s.PathToWeatherFile))
            {
                // Set weather
                fDp.SetValue(
                    "parameter_filenames",
                    "weather_database",
                    s.PathToWeatherFile);

                // Set soil
                DirectoryInfo weatherDatabase = Directory.GetParent(s.PathToWeatherFile);
                string pathToSoilFile = Path.Combine(
                    weatherDatabase.Parent.FullName.ToString(), 
                    "Soil", 
                    Path.GetFileNameWithoutExtension(s.PathToWeatherFile) + ".CS_soil");
                fDp.SetValue(
                    "parameter_filenames",
                    "soil",
                    pathToSoilFile);
            }

            fDp.Save(fDp.LoadedPath);
        }
        //private void clearSection()
        private List<string> getSectionOrder(Scenario s)
        {
            // TODO: Actually count number of sects/storage and set values manually
            List<string> sectionOrder = new List<string>()
            {
                "version", "dairy scenario"
            };

            if (s.GetCountCow() > 0)
            {
                for (int i = 1; i <= s.GetCountCow(); i++)
                {
                    sectionOrder.Add("cow_description:" + i);
                }
            }
            if (s.GetCountBarn() > 0)
            {
                for (int i = 1; i <= s.GetCountBarn(); i++)
                {
                    sectionOrder.Add("barn:" + i);
                }
            }
            if (s.GetCountManureSeparator() > 0)
            {
                for (int i = 1; i <= s.GetCountManureSeparator(); i++)
                {
                    sectionOrder.Add("manure_separator:" + i);
                }
            }
            if (s.GetCountManureStorage() > 0)
            {
                for (int i = 1; i <= s.GetCountManureStorage(); i++)
                {
                    sectionOrder.Add("manure_storage:" + i);
                }
            }
            if (s.GetCountReceiveOffFarmBiomass() > 0)
            {
                for (int i = 1; i <= s.GetCountReceiveOffFarmBiomass(); i++)
                {
                    sectionOrder.Add("receive_off_farm_biomass:" + i);
                }
            }
            sectionOrder.Add("fertigation_management");
            if(s.GetCountFertigation() > 0)
            {
                for (int i = 1; i <= s.GetCountFertigation(); i++)
                {
                    sectionOrder.Add("fertigation:" + i);
                }
            }
            return sectionOrder;
        }
        private SortedList<int,ManureSeparator> sortManureSeparators(Scenario s)
        {
            SortedList<int, ManureSeparator> seps = 
                new SortedList<int, ManureSeparator>();
            int count = 0;
            if (s.AnaerobicDigester != null && s.AnaerobicDigester.Enabled)
            {
                seps.Add(count, s.AnaerobicDigester);
                count++;
            }

            if (s.CourseSeparator != null && s.CourseSeparator.Enabled)
            {
                seps.Add(count, s.CourseSeparator);
                count++;
            }

            if (s.FineSeparator != null && s.FineSeparator.Enabled)
            {
                seps.Add(count, s.FineSeparator);
                count++;
            }

            if (s.NutrientRecovery != null && s.NutrientRecovery.Enabled)
            {
                seps.Add(count, s.NutrientRecovery);
            }

            return seps;
        }
        private int getSeparatorsCount(Scenario s)
        {
            int count = 0;
            if (s.AnaerobicDigester != null && s.AnaerobicDigester.Enabled)
                count++;
            if (s.CourseSeparator != null && s.CourseSeparator.Enabled)
                count++;
            if (s.FineSeparator != null && s.FineSeparator.Enabled)
                count++;
            if (s.NutrientRecovery != null && s.NutrientRecovery.Enabled)
                count++;
            return count;
        }
        private int getFertigationsCount(Scenario s)
        {
            int v = 0;
            if(s.Fertigation.Repetition_d > 0)
            {
                DateTime end = s.StopDate;
                DateTime start = s.Fertigation.ApplicationDate_date;

                double numDays = end.ToOADate() - start.ToOADate();

                v = Convert.ToInt16(numDays / s.Fertigation.Repetition_d);
            }
            else
            {
                v = 1;
            }

            return v;
        }
        private string getHoldingTankId(string style)
        {
            string tankId;

            if (style == ManureSeperatorStyles.AnaerobicDigester)
                tankId = "AD tank";
            else if (style == ManureSeperatorStyles.CourseSeparator)
                tankId = "CS tank";
            else if (style == ManureSeperatorStyles.FineSeparator)
                tankId = "FS tank";
            else if (style == ManureSeperatorStyles.NutrientRecovery)
                tankId = "NR tank";
            else
                throw new ArgumentException("Style is not valid");

            return tankId;
        }
        private string getYYYYDOYString(DateTime dt)
        {
            string v = dt.Year.ToString("0000") + dt.DayOfYear.ToString("000");
            return v;
        }
    }
}
