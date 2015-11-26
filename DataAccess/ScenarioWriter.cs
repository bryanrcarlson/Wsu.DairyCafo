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
        private readonly ScenarioFile dDp;
        private readonly ScenarioFile fDp;
        private readonly IScenarioDefaults defaults;
        private readonly string fieldDirName;

        public ScenarioWriter(
            ScenarioFile dairyScenario,
            ScenarioFile cropSystScenario,
            IScenarioDefaults defaults,
            string fieldDirName = "Fields"
            )
        {
            this.dDp = dairyScenario;
            this.fDp = cropSystScenario;
            this.defaults = defaults;
            this.fieldDirName = fieldDirName;
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
                throw new NullReferenceException("Cannot write, no file loaded");

            dDp.SetSection("version", defaults.GetVersionDefaults());
            var sVals = defaults.GetScenarioDefaults();
            sVals.Add("weather", s.PathToWeatherFile.ToString());
            sVals.Add("start_date", s.StartDate.Year.ToString("0000") + s.StartDate.DayOfYear.ToString("000"));
            sVals.Add("stop_date", s.StopDate.Year.ToString("0000") + s.StopDate.DayOfYear.ToString("000"));

            dDp.SetSection("dairy scenario", sVals);
            
            writeCow(s);
            writeBarn(s);
            writeLagoon(s);

            writeSeparatorsAndStorage(s);

            dDp.Save(dDp.LoadedPath);
        }
        private void writeBarn(Scenario s)
        {
            var vals = defaults.GetBarnDefaults();
            vals.Add("manure_alley_surface_area", 
                s.Barn.Manure_alley_area_m2.ToString());
            vals.Add("cow_population",
                s.Barn.Number_cows_cnt.ToString());

            dDp.SetSection("barn:1", vals);
        }
        private void writeCow(Scenario s)
        {
            var vals = defaults.GetCowDefaults();
            vals.Add("body_mass", s.Cow.BodyMass_kg.ToString());
            vals.Add("dry_matter_intake", s.Cow.DryMatterIntake_kg_d.ToString());
            vals.Add("milk_production", s.Cow.MilkProduction_kg_d.ToString());
            vals.Add("diet_crude_protein", s.Cow.CrudeProteinDiet_kg_d.ToString());

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
        private void writeSeparatorsAndStorage(Scenario s)
        {
            SortedList<int,ManureSeparator> seps = sortManureSeparators(s);

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
            }


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
    }
}
