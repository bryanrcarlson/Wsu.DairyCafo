using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.DairyCafo.UI.PresentationLogic.Helper;

namespace Wsu.DairyCafo.UI.PresentationLogic.Model
{
    public class ScenarioModel : ObservableObject
    {
        #region Fields
        //       private string pathToWeatherFile;
        //       private DateTime startDate;
        //       private DateTime endDate;
        private Scenario scenario;
        #endregion //Fields

        #region Properties
        public string PathToWeatherFile
        {
            get { return scenario.PathToWeatherFile; }
            set
            {
                if (value != scenario.PathToWeatherFile)
                {
                    scenario.PathToWeatherFile = value;
                    OnPropertyChanged("PathToWeatherFile");
                }
            }
        }
        public DateTime StartDate
        {
            get { return scenario.StartDate; }
            set
            {
                if (value != scenario.StartDate)
                {
                    scenario.StartDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }
        public DateTime EndDate
        {
            get { return scenario.EndDate; }
            set
            {
                if (value != scenario.EndDate)
                {
                    scenario.EndDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        //== Cow
        public double CowBodyMass
        {
            get { return scenario.Cow.BodyMass_kg; }
            set
            {
                if (value != scenario.Cow.BodyMass_kg)
                {
                    scenario.Cow.BodyMass_kg = value;
                    OnPropertyChanged("CowBodyMass");
                }
            }
        }
        public double CowDryMatterIntake
        {
            get { return scenario.Cow.DryMatterIntake_kg_d; }
            set
            {
                if (value != scenario.Cow.DryMatterIntake_kg_d)
                {
                    scenario.Cow.DryMatterIntake_kg_d = value;
                    OnPropertyChanged("CowDryMatterIntake");
                }
            }
        }
        public double CowMilkProduction
        {
            get { return scenario.Cow.MilkProduction_kg_d; }
            set
            {
                if (value != scenario.Cow.MilkProduction_kg_d)
                {
                    scenario.Cow.MilkProduction_kg_d = value;
                    OnPropertyChanged("CowMilkProduction");
                }
            }
        }
        public double CowCrudeProteinDiet
        {
            get { return scenario.Cow.CrudeProteinDiet_kg_d; }
            set
            {
                if (value != scenario.Cow.CrudeProteinDiet_kg_d)
                {
                    scenario.Cow.CrudeProteinDiet_kg_d = value;
                    OnPropertyChanged("CowCrudeProteinDiet");
                }
            }
        }


        //== Barn
        public double BarnManureAlleyArea
        {
            get { return scenario.Barn.Manure_alley_area_m2; }
            set
            {
                if (value != scenario.Barn.Manure_alley_area_m2)
                {
                    scenario.Barn.Manure_alley_area_m2 = value;
                    OnPropertyChanged("BarnManureAlleyArea");
                }
            }
        }
        public double BarnNumberCows
        {
            get { return scenario.Barn.Number_cows_cnt; }
            set
            {
                if (value != scenario.Barn.Number_cows_cnt)
                {
                    scenario.Barn.Number_cows_cnt = value;
                    OnPropertyChanged("BarnNumberCows");
                }
            }
        }

        //== Separators
        public bool AnaerobicDigesterEnabled
        {
            get { return scenario.AnaerobicDigester.Enabled; }
            set
            {
                if (value != scenario.AnaerobicDigester.Enabled)
                {
                    scenario.AnaerobicDigester.Enabled = value;
                    OnPropertyChanged("AnaerobicDigesterEnabled");
                }
            }
        }
        public bool CourseSeparatorEnabled
        {
            get { return scenario.CourseSeparator.Enabled; }
            set
            {
                if (value != scenario.CourseSeparator.Enabled)
                {
                    scenario.CourseSeparator.Enabled = value;
                    OnPropertyChanged("CourseSeparatorEnabled");
                }
            }
        }
        public bool FineSeparatorEnabled
        {
            get { return scenario.FineSeparator.Enabled; }
            set
            {
                if (value != scenario.FineSeparator.Enabled)
                {
                    scenario.FineSeparator.Enabled = value;
                    OnPropertyChanged("FineSeparatorEnabled");
                }
            }
        }
        public bool NutrientRecoveryEnabled
        {
            get { return scenario.NutrientRecovery.Enabled; }
            set
            {
                if (value != scenario.NutrientRecovery.Enabled)
                {
                    scenario.NutrientRecovery.Enabled = value;
                    OnPropertyChanged("NutrientRecoveryEnabled");
                }
            }
        }

        //== Lagoon
        public double LagoonSurfaceArea
        {
            get { return scenario.Lagoon.SurfaceArea_m2; }
            set
            {
                if (value != scenario.Lagoon.SurfaceArea_m2)
                {
                    scenario.Lagoon.SurfaceArea_m2 = value;
                    OnPropertyChanged("LagoonSurfaceArea");
                }
            }
        }
        public double LagoonVolumeMax
        {
            get { return scenario.Lagoon.VolumeMax_m3; }
            set
            {
                if (value != scenario.Lagoon.VolumeMax_m3)
                {
                    scenario.Lagoon.VolumeMax_m3 = value;
                    OnPropertyChanged("LagoonVolumeMax");
                }
            }
        }

        //== Field
        public double FieldArea
        {
            get { return scenario.Field.Area_ha; }
            set
            {
                if (value != scenario.Field.Area_ha)
                {
                    scenario.Field.Area_ha = value;
                    OnPropertyChanged("FieldArea");
                }
            }
        }

        #endregion // Properties
        #region 'structors
        public ScenarioModel(Scenario scenario)
        {
            this.scenario = scenario;
        }
        #endregion
    }
}
