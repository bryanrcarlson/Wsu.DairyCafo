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


            dDp.SetSection("version", defaults.GetVersionDefaults());
            var sVals = defaults.GetScenarioDefaults();
            sVals.Add("weather", s.PathToWeatherFile.ToString());
            sVals.Add("start_date", getYYYYDOYString(s.StartDate));
            sVals.Add("stop_date", getYYYYDOYString(s.StopDate));
            
            writeCow(s);
            writeBarn(s);
            writeLagoon(s);

            int numSeparators = writeSeparatorsAndStorage(s);
            int numStorageTanks = numSeparators > 0 ? numSeparators : 1; // lagoon + tanks between separators

            writeFertigationManagement(s);

            int numFertigations = writeFertigations(s);

            sVals.Add("manure_separator:count", numSeparators.ToString());
            sVals.Add("manure_storage:count", numStorageTanks.ToString());
            sVals.Add("fertigation:count", numFertigations.ToString());

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
            var vals = defaults.GetBarnDefaults();
            vals.Add("manure_alley_surface_area", 
                s.Barn.ManureAlleyArea_m2.ToString());
            vals.Add("cow_population",
                s.Barn.NumberCows_cnt.ToString());

            dDp.SetSection("barn:1", vals);
        }
        private void writeCow(Scenario s)
        {
            var vals = defaults.GetCowDefaults();
            vals.Add("body_mass", s.Cow.BodyMass_kg.ToString());
            vals.Add("dry_matter_intake", s.Cow.DryMatterIntake_kg_d.ToString());
            vals.Add("milk_production", s.Cow.MilkProduction_kg_d.ToString());
            vals.Add("diet_crude_protein", s.Cow.CrudeProteinDiet_percent.ToString());

            dDp.SetSection("cow_description:1", vals);
        }
        private void writeLagoon(Scenario s)
        {
            var vals = defaults.GetLagoonDefaults();
            vals.Add("surface_area", s.Lagoon.SurfaceArea_m2.ToString());
            vals.Add("volume_max", s.Lagoon.VolumeMax_m3.ToString());

            // Lagoon is always the first manure storage; before holding tanks
            dDp.SetSection("manure_storage:1", vals);
        }
        private int writeSeparatorsAndStorage(Scenario s)
        {
            SortedList<int,ManureSeparator> seps = sortManureSeparators(s);
            int sepsWritten = 0;
            // Last in list will pass manure to lagoon
            //seps[seps.Count - 1].LiquidFacility = s.Lagoon.Id;

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
                    string sect = "manure_storage:" + (i + 2).ToString(); // +2 because zerobased and lagoon is 1
                    var d = defaults.GetHoldingTankDefaults();
                    d.Add("ID", seps[i].LiquidFacility);
                    dDp.SetSection(sect, d);
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
                vals.Add("enable",seps[i].Enabled.ToString());
                vals.Add("style", seps[i].Style);
                vals.Add("source_facility", seps[i].SourceFacility);
                vals.Add("liquid_facility", seps[i].LiquidFacility);
                vals.Add("solids_facility", seps[i].SolidFacility);

                dDp.SetSection(manSepCnt, vals);

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

            // Expand fert management to instances of ferts
            DateTime currDt = s.Fertigation.ApplicationDate_date;
            int i = 0;
            do
            {
                i++;
                writeFertigation(s.Fertigation, i, getYYYYDOYString(currDt));
                currDt = currDt.AddDays(s.Fertigation.Repetition_d);
            } while (currDt < s.StopDate);

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
