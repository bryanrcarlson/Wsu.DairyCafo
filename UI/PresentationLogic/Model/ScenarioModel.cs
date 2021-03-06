﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
        private Scenario scenario;
        #endregion //Fields

        #region Properties
        public Scenario GetScenario()
        {
            return this.scenario;
        }
        public bool OutputEnabled
        {
            get { return scenario.OutputEnabled; }
            set
            {
                if (value != scenario.OutputEnabled)
                {
                    scenario.OutputEnabled = value;
                    OnPropertyChanged("OutputEnabled");
                }
            }
        }
        public double Latitude
        {
            get { return scenario.Latitude; }
            set
            {
                if (value != scenario.Latitude)
                {
                    scenario.Latitude = value;
                    OnPropertyChanged("Latitude");
                }
            }
        }
        public double Longitude
        {
            get { return scenario.Longitude; }
            set
            {
                if (value != scenario.Longitude)
                {
                    scenario.Longitude = value;
                    OnPropertyChanged("Longitude");
                }
            }
        }
        //public string PathToWeatherFile
        //{
        //get { return scenario.PathToWeatherFile; }
        //set
        //{
        //if (value != scenario.PathToWeatherFile)
        //{
        //scenario.PathToWeatherFile = value;
        //OnPropertyChanged("PathToWeatherFile");
        //}
        //}
        //}
        public int SimulationYear
        {
            get
            {
                return getSimulationYear();
            }
            set
            {
                if (value != getSimulationYear())
                {
                    StartDate = new DateTime(value, 1, 1);
                    StopDate = new DateTime(value, 12, 31);
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
        public DateTime StopDate
        {
            get { return scenario.StopDate; }
            set
            {
                if (value != scenario.StopDate)
                {
                    scenario.StopDate = value;
                    OnPropertyChanged("StopDate");
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
            get { return scenario.Cow.CrudeProteinDiet_percent; }
            set
            {
                if (value != scenario.Cow.CrudeProteinDiet_percent)
                {
                    scenario.Cow.CrudeProteinDiet_percent = value;
                    OnPropertyChanged("CowCrudeProteinDiet");
                }
            }
        }
        public double StarchDiet_percent
        {
            get { return scenario.Cow.StarchDiet_percent; }
            set
            {
                if (value != scenario.Cow.StarchDiet_percent)
                {
                    scenario.Cow.StarchDiet_percent = value;
                    OnPropertyChanged("StarchDiet_percent");
                }
            }
        }
        public double AcidDetergentFiberDiet_percent
        {
            get { return scenario.Cow.AcidDetergentFiberDiet_percent; }
            set
            {
                if (value != scenario.Cow.AcidDetergentFiberDiet_percent)
                {
                    scenario.Cow.AcidDetergentFiberDiet_percent = value;
                    OnPropertyChanged("AcidDetergentFiberDiet_percent");
                }
            }
        }
        public double MetabolizableEnergyDiet_MJ_d
        {
            get { return scenario.Cow.MetabolizableEnergyDiet_MJ_d; }
            set
            {
                if (value != scenario.Cow.MetabolizableEnergyDiet_MJ_d)
                {
                    scenario.Cow.MetabolizableEnergyDiet_MJ_d = value;
                    OnPropertyChanged("MetabolizableEnergyDiet_MJ_d");
                }
            }
        }
        public double PhManure_mol_L
        {
            get { return scenario.Cow.PhManure_mol_L; }
            set
            {
                if (value != scenario.Cow.PhManure_mol_L)
                {
                    scenario.Cow.PhManure_mol_L = value;
                    OnPropertyChanged("PhManure_mol_L");
                }
            }
        }
        
        //== Barn
        public double BarnManureAlleyArea
        {
            get { return scenario.Barn.ManureAlleyArea_m2; }
            set
            {
                if (value != scenario.Barn.ManureAlleyArea_m2)
                {
                    scenario.Barn.ManureAlleyArea_m2 = value;
                    OnPropertyChanged("BarnManureAlleyArea");
                }
            }
        }
        public double BarnNumberCows
        {
            get { return scenario.Barn.NumberCows_cnt; }
            set
            {
                if (value != scenario.Barn.NumberCows_cnt)
                {
                    scenario.Barn.NumberCows_cnt = value;
                    OnPropertyChanged("BarnNumberCows");
                }
            }
        }
        public int CleaningFrequency
        {
            get { return scenario.Barn.CleaningFrequency; }
            set
            {
                if (value != scenario.Barn.CleaningFrequency)
                {
                    scenario.Barn.CleaningFrequency = value;
                    OnPropertyChanged("CleaningFrequency");
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
        public double PH_mol_L
        {
            get { return scenario.Lagoon.PH_mol_L; }
            set
            {
                if (value != scenario.Lagoon.PH_mol_L)
                {
                    scenario.Lagoon.PH_mol_L = value;
                    OnPropertyChanged("PH_mol_L");
                }
            }
        }

        //== Fertigation
        public DateTime FertigationDate
        {
            get { return scenario.Fertigation.ApplicationDate_date; }
            set
            {
                if(value != scenario.Fertigation.ApplicationDate_date)
                {
                    // Override to always make same year as Simulation
                    value = new DateTime(
                        scenario.StartDate.Year,
                        value.Month,
                        value.Day);
                    scenario.Fertigation.ApplicationDate_date = value;
                    OnPropertyChanged("FertigationDate");
                }
            }
        }
        public int FertigationDayOfYear
        {
            get { return scenario.Fertigation.ApplicationDate_date.DayOfYear; }
            set {
                if (value != scenario.Fertigation.ApplicationDate_date.DayOfYear)
                {
                    DateTime newDt = new DateTime(scenario.StartDate.Year, 1, 1)
                        .AddDays(value - 1);
                    scenario.Fertigation.ApplicationDate_date = newDt;
                }
            }
        }
        //public string FertigationMonth
        //{
        //    get { return scenario.Fertigation.ApplicationDate_date.Month.ToString("MMMM"); }
        //    set
        //    {
        //        if(value != scenario.Fertigation.ApplicationDate_date.Month.ToString("en-US"))
        //        {
        //            int month = DateTime.ParseExact(value, "MMMM", new CultureInfo("en-US")).Month;

        //            DateTime newDt = new DateTime(scenario.StartDate.Year, month, scenario.Fertigation.ApplicationDate_date.Day);
        //            scenario.Fertigation.ApplicationDate_date = newDt;
        //        }
        //    }
        //}
        public double FertigationAmnt
        {
            get { return scenario.Fertigation.AmountRemoved_percent; }
            set
            {
                if (value != scenario.Fertigation.AmountRemoved_percent)
                {
                    scenario.Fertigation.AmountRemoved_percent = value;
                    OnPropertyChanged("FertigationAmnt");
                }
            }
        }
        public int FertigationRepetition
        {
            get { return scenario.Fertigation.Repetition_d; }
            set
            {
                if (value != scenario.Fertigation.Repetition_d)
                {
                    scenario.Fertigation.Repetition_d = value;
                    OnPropertyChanged("FertigationRepetition");
                }
            }
        }
        public bool FertigationEnabled
        {
            get { return scenario.Fertigation.Enabled; }
            set
            {
                if (value != scenario.Fertigation.Enabled)
                {
                    scenario.Fertigation.Enabled = value;
                    OnPropertyChanged("FertigationEnabled");
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
        public bool FieldEnabled
        {
            get { return scenario.Field.Enabled; }
            set
            {
                if (value != scenario.Field.Enabled)
                {
                    scenario.Field.Enabled = value;
                    OnPropertyChanged("FieldEnabled");
                }
            }
        }
        public string FieldCrop
        {
            get { return scenario.Field.Crop; }
            set
            {
                if(value != scenario.Field.Crop)
                {
                    scenario.Field.Crop = value;
                    scenario.Field.Id = value;
                    OnPropertyChanged("FieldCrop");
                }
            }
        }
        public string[] FieldCrops
        {
            get { return new string[] { "alfalfa", "barley", "corn"  }; }
        }
        #endregion // Properties
        #region 'structors
        public ScenarioModel(Scenario scenario)
        {
            this.scenario = scenario;
        }
        #endregion

        #region Helpers
        private int getSimulationYear()
        {
            int startYear = scenario.StartDate.Year;
            int stopYear = scenario.StopDate.Year;

            if (startYear == stopYear)
            {
                return startYear;
            }
            else
            {
                throw new InvalidOperationException(
                    "Simulation must start and stop in the same year");
            }
        }
        #endregion
    }
}
