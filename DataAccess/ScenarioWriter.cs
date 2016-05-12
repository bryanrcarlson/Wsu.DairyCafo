﻿using System;
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
        private readonly string fieldDirName;

        public ScenarioWriter(
            IScenarioFile dairyScenario,
            IScenarioFile cropSystScenario,
            IScenarioDefaults defaults,
            string fieldDirName = "Fields"
            )
        {
            this.dDp = dairyScenario;
            this.fDp = cropSystScenario;
            this.defaults = defaults;
            this.fieldDirName = fieldDirName;
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

            string filePath = Path.Combine(dirPath, ".NIFA_dairy_scenario");

            if (File.Exists(filePath))
                throw new InvalidOperationException("File already exists, aborting");

            File.Create(filePath).Close();

            dDp.Load(filePath);

            Write(s);
        }
        public void Write(Scenario s)
        {
            if (String.IsNullOrEmpty(dDp.LoadedPath))
                throw new NullReferenceException("Cannot write, no scenario file loaded");

            // Clear contents
            // TODO: do something with the backup
            //            Dictionary<string, Dictionary<string, string>> backup =
            //                dDp.Clear();
            //            File.WriteAllText(dDp.LoadedPath, String.Empty);

            var d = new DefaultScenario();

            dDp.SetSection("version", defaults.GetVersionDefaults());

            //var sVals = defaults.GetScenarioDefaults();
            var sVals = new Dictionary<string, string>();
            sVals.Add("details_URL", s.DetailsUrl);
            sVals.Add("description", s.Description);
            sVals.Add("weather", s.PathToWeatherFile.ToString());
            sVals.Add("start_date", getYYYYDOYString(s.StartDate));
            sVals.Add("stop_date", getYYYYDOYString(s.StopDate));
            //TODO: replace these default values with the actual scenario values -- defaults should be set at scenario creation.  Just because the UI doesn't show some values doesn't mean the model doesn't have them!
            sVals.Add("accumulations", d.s.Accumulations.ToString());
            sVals.Add("simulation_period_mode", d.s.SimulationPeriodMode.ToString());
            sVals.Add("irrigation_pump_model", d.s.IrrigationPumpModel.ToString());
            sVals.Add("parameterized_scenario", d.s.ParameterizedScenario.ToString());
            sVals.Add("cow_description:count", d.s.GetCountCow().ToString());
            sVals.Add("barn:count", d.s.GetCountBarn().ToString());

            writeCow(s);
            writeBarn(s);
            

            
            //int numSeparators = writeSeparatorsAndStorage(s);
            writeSeparatorsAndStorage(s);
            int numSeparators = s.GetCountManureSeparator();
            //int numStorageTanks = numSeparators > 0 ? numSeparators : 1; // lagoon + tanks between separators
            int numStorageTanks = s.GetCountManureStorage();
            writeLagoon(s);
            int numReceiveOffFarmBiomass = writeReceiveOffFarmBiomass(s);
            writeFertigationManagement(s);

            int numFertigations = writeFertigations(s);

            sVals.Add("manure_separator:count", numSeparators.ToString());
            sVals.Add("manure_storage:count", numStorageTanks.ToString());
            sVals.Add("fertigation:count", numFertigations.ToString());
            sVals.Add("receive_off_farm_biomass:count", numReceiveOffFarmBiomass.ToString());

            dDp.SetSection("dairy scenario", sVals);

            List<string> sectionOrder = new List<string>()
            {
                "version", "dairy scenario"
            };
            if (!dDp.Save(dDp.LoadedPath, sectionOrder))
            {
                throw new OperationCanceledException("Failed to save file"); 
            }
                  
        }
        public void WriteField(Scenario s)
        {
            try
            {
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
            vals.Add("decomposition_constant_resilient", s.ReceiveOffFarmBiomass.Biomatter.DecompositionConstantResilient.ToString());

            dDp.SetSection("receive_off_farm_biomass:1", vals);

            return s.ReceiveOffFarmBiomass.Enabled ? 1 : 0;
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
            var vals = defaults.GetFertigationDefaults();
            vals.Add("ID", "fert-management");
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
                    ? ""
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

            var vals = defaults.GetFertigationDefaults();
            vals.Add("ID", "fert" + date);
            vals.Add("application_date", date);
            vals.Add("removal", f.AmountRemoved_percent.ToString());
            vals.Add("from_storage", f.SourceFacility_id);
            vals.Add("to_field", f.TargetField_id);

            dDp.SetSection(sect, vals);
        }
        private void writeField(Scenario s)
        {
            if (String.IsNullOrEmpty(fDp.LoadedPath))
                throw new NullReferenceException("Cannot write field scenario, no file loaded");

            // Delete fertigation file
            string fertPath = Path.Combine(
                Path.GetDirectoryName(fDp.LoadedPath),
                "Database",
                "Management",
                "fertigation.CS_management");
            if (File.Exists(fertPath))
                File.Delete(fertPath);

            // Set field area
            fDp.SetValue("field", "size", s.Field.Area_ha.ToString());

            // Set fixed management to apply fertigation from manure storage (lagoon)
            string fixedManagementPath = "";
            if (s.Fertigation.Enabled) fixedManagementPath = fertPath;
            fDp.SetValue(
                    "parameter_filenames", 
                    "fixed_management", 
                    fixedManagementPath);

            fDp.Save(fDp.LoadedPath);
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
