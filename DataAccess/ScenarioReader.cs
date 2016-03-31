using System;
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
        public void Load(string filePath)
        {
            if(!dDp.Load(filePath))
            {
                throw new ArgumentException("File path not valid");
            }
            if(!String.IsNullOrEmpty(dDp.LoadedPath))
            {
                // Load Field scenario
                string dairyDir = Path.GetDirectoryName(filePath);
                string fieldDir = Path.Combine(dairyDir, fieldDirName);

                //TODO: Create a FieldsDirectory (or name it more generic?) class that
                //has the following functions.  Ultimatly, should only return
                //a string to be used to set/load fDp.
                if(!Directory.Exists(fieldDir))
                {
                    //Directory.CreateDirectory(fieldDir);
                    throw new NullReferenceException("Directory \"" +
                        fieldDirName + "\" does not exist so we created one for you.  Please add a dir with a CropSyst scenario to it.");
                }

                string[] fields = Directory.GetDirectories(fieldDir);

                if(fields.Length > 0)
                {
                    // Currently only supporting one field
                    string fieldFile = Path.Combine(fields[0], ".CropSyst_scenario");
                    fDp.Load(fieldFile);
                }
            }
            
        }
        public Scenario Parse()
        {
            Scenario s = new Scenario();

            #region Scenario
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
            #endregion

            parseBarn(s);
            parseCow(s);
            parseManureSeparators(s, manureSeparatorCount);
            parseLagoon(s, manureStorageCount);
            parseFertigationManagement(s);
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

            s.Barn = new Barn()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
                ManureAlleyArea_m2 = Convert.ToDouble(surface_area),
                NumberCows_cnt = Convert.ToDouble(cows)
            };
        }
        private void parseCow(Scenario s)
        {
            string sect = "cow_description:1";
            string id = dDp.GetValue(sect, "ID");
            string enabled = dDp.GetValueOnly(sect, "enable");
            string mass = dDp.GetValueOnly(sect, "body_mass");
            string dmi = dDp.GetValueOnly(sect, "milk_production");
            string milk = dDp.GetValueOnly(sect, "dry_matter_intake");
            string protein = dDp.GetValueOnly(sect, "diet_crude_protein");

            s.Cow = new Cow()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
                BodyMass_kg = Convert.ToDouble(mass),
                CrudeProteinDiet_percent = Convert.ToDouble(protein),
                DryMatterIntake_kg_d = Convert.ToDouble(dmi),
                MilkProduction_kg_d = Convert.ToDouble(milk)
            };
        }
        private void parseManureSeparators(Scenario s, int manureSeparatorCount)
        {
            for (int i = 0; i < manureSeparatorCount; i++)
            {
                string sect = "manure_separator:" + (i + 1).ToString();

                string id = dDp.GetValue(sect, "ID");
                string enabled = dDp.GetValueOnly(sect, "enable");
                string style = dDp.GetValue(sect, "style");
                string source = dDp.GetValueOnly(sect, "source_facility");
                string liquid = dDp.GetValueOnly(sect, "liquid_facility");
                string solids = dDp.GetValueOnly(sect, "solids_facility");

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
            string date_string = dDp.GetValueOnly(sect, "application_date");

            
            string remove_per = dDp.GetValueOnly(sect, "removal");
            string repetition = dDp.GetValueOnly(sect, "num_days_to_repeat");
            string source = dDp.GetValueOnly(sect, "from_storage");
            string field = dDp.GetValueOnly(sect, "to_field");

            s.Fertigation = new Fertigation()
            {
                Id = id,
                Enabled = Convert.ToBoolean(enabled),
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
                    string sa = dDp.GetValueOnly(sect, "surface_area");
                    string vol = dDp.GetValueOnly(sect, "volume_max");

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
        private void parseField(Scenario s)
        {
            if(String.IsNullOrEmpty(fDp.LoadedPath))
            {
                s.Field = new Field()
                {
                    Enabled = false,
                    Id = ""
                };
            }
            else
            {
                string id = new DirectoryInfo(fDp.LoadedPath).Parent.ToString();
                string area = fDp.GetValueOnly("field", "size");

                s.Field = new Field()
                {
                    Id = id,
                    Enabled = true,
                    Area_ha = Convert.ToDouble(area)
                };
            }
            
        }

        private DateTime parseDateFromIniFile(string date)
        {
            int year = Convert.ToInt16(date.Substring(0, 4));
            int doy = Convert.ToInt16(date.Substring(4, 3));
            DateTime dt = new DateTime(year, 1, 1).AddDays(doy - 1);

            return dt;
        }
    }
}
