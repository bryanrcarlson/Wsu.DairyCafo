using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.IO.DataAccess;

namespace Wsu.DairyCafo.DataAccess
{
    public class ScenarioReader : IScenarioReader
    {
        private readonly ScenarioFile dp;

        public ScenarioReader(ScenarioFile iniFile)
        {
            this.dp = iniFile;
        }

        public Scenario Parse()
        {
            Scenario s = new Scenario();

            #region Scenario
            string sd = dp.GetValueOnly("dairy scenario", "start_date");
            s.StartDate = sd != null ? parseDateFromIniFile(sd) : DateTime.Now;

            string ed = dp.GetValueOnly("dairy scenario", "stop_date");
            s.EndDate = ed != null ? parseDateFromIniFile(ed) : DateTime.Now;

            string w = dp.GetValueOnly("dairy scenario", "weather");
            s.PathToWeatherFile = w;

            int manureSeparatorCount = 
                Convert.ToInt16(dp.GetValueOnly("dairy scenario", "manure_separator:count"));
            int manureStorageCount =
                Convert.ToInt16(dp.GetValueOnly("dairy scenario", "manure_storage:count"));
            #endregion

            parseBarn(s);
            parseCow(s);
            parseManureSeparators(s, manureSeparatorCount);
            parseLagoon(s, manureStorageCount);

            return s;
        }
        private void parseBarn(Scenario s)
        {
            string sect = "barn:1";
            string id = dp.GetValueOnly(sect, "ID");
            string enabled = dp.GetValueOnly(sect, "enable");
            string surface_area = dp.GetValueOnly(sect, "manure_alley_surface_area");
            string cows = dp.GetValueOnly(sect, "cow_population");

            s.Barn = new Barn()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
                Manure_alley_area_m2 = Convert.ToDouble(surface_area),
                Number_cows_cnt = Convert.ToDouble(cows)
            };
        }
        private void parseCow(Scenario s)
        {
            string sect = "cow_description:1";
            string id = dp.GetValueOnly(sect, "ID");
            string enabled = dp.GetValueOnly(sect, "enable");
            string mass = dp.GetValueOnly(sect, "body_mass");
            string dmi = dp.GetValueOnly(sect, "milk_production");
            string milk = dp.GetValueOnly(sect, "dry_matter_intake");
            string protein = dp.GetValueOnly(sect, "diet_crude_protein");

            s.Cow = new Cow()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
                BodyMass_kg = Convert.ToDouble(mass),
                CrudeProteinDiet_kg_d = Convert.ToDouble(protein),
                DryMatterIntake_kg_d = Convert.ToDouble(dmi),
                MilkProduction_kg_d = Convert.ToDouble(milk)
            };
        }
        private void parseManureSeparators(Scenario s, int manureSeparatorCount)
        {
            for (int i = 0; i < manureSeparatorCount; i++)
            {
                string sect = "manure_separator:" + (i + 1).ToString();

                string id = dp.GetValueOnly(sect, "ID");
                string enabled = dp.GetValueOnly(sect, "enable");
                string style = dp.GetValue(sect, "style");
                string source = dp.GetValueOnly(sect, "source_facility");
                string liquid = dp.GetValueOnly(sect, "liquid_facility");
                string solids = dp.GetValueOnly(sect, "solids_facility");

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
        private void parseLagoon(Scenario s, int manureStorageCount)
        {
            for(int i = 0; i < manureStorageCount; i++)
            {
                string sect = "manure_storage:" + (i + 1).ToString();

                string style = dp.GetValueOnly(sect, "style");
                if(style == "lagoon_aerobic")
                {
                    string id = dp.GetValueOnly(sect, "ID");
                    string enabled = dp.GetValueOnly(sect, "enable");
                    string sa = dp.GetValueOnly(sect, "surface_area");
                    string vol = dp.GetValueOnly(sect, "volume_max");

                    s.Lagoon = new Lagoon()
                    {
                        Id = id,
                        Enabled = Convert.ToBoolean(enabled),
                        SurfaceArea_m2 = Convert.ToDouble(sa),
                        VolumeMax_m3 = Convert.ToDouble(vol)
                    };

                    break;
                }
            }
        }


        private DateTime parseDateFromIniFile(string date)
        {
            int year = Convert.ToInt16(date.Substring(0, 4));
            int doy = Convert.ToInt16(date.Substring(4, 3));
            DateTime dt = new DateTime(year, 1, 1).AddDays(doy - 1);

            return dt;
        }
        //private string cleanStr(string iniString)
        //{
        //    string[] s = iniString.Split(' ');
        //    return s[0];
        //}
    }
}
