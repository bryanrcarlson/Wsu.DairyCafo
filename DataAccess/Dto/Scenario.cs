﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    /// <summary>
    /// Represents a Dairy-CropSyst scenario
    /// <remark>
    /// Scenario ini file is in the ini format of:
    /// [dairy scenario]
    /// details_URL=
    /// description=
    /// weather=D:\WsuData\NIFA\Simulation\Database\Weather\paterson.UED
    /// start_date = 1979001(January 1(001))
    /// stop_date=1979365 (December 31(365))
    /// accumulations=3
    /// simulation_period_mode=multiyear
    /// irrigation_pump_model =
    /// parameterized_scenario = 1
    /// barn:count=1
    /// anaerobic_digester:count=0
    /// manure_separator:count=4
    /// manure_storage:count=4
    /// cow_description:count=1
    /// set_population:count=0
    /// set_bedding:count=0
    /// route_manure:count=0
    /// fertigation:count=15
    /// digester_operation:count=0
    /// deactivation:count=0
    /// receive_off_farm_biomass:count=0
    /// </remark>
    /// </summary>
    public class Scenario
    {
        public string DetailsUrl { get; set; }
        public string Description { get; set; }
        public string PathToWeatherFile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public int Accumulations { get; set; }
        public string SimulationPeriodMode { get; set; }
        public string IrrigationPumpModel { get; set; }
        public int ParameterizedScenario { get; set; }


        public Cow Cow { get; set; }
        public Barn Barn { get; set; }
        public AnaerobicDigester AnaerobicDigester { get; set; }
        public CourseSeparator CourseSeparator { get; set; }
        public FineSeparator FineSeparator { get; set; }
        public NutrientRecovery NutrientRecovery { get; set; }
        public Lagoon Lagoon { get; set; }

        public Fertigation Fertigation { get; set; }

        public Field Field { get; set; }

        //public Collection<ManureSeparator> ManureSeparators { get; set; }

        public Scenario()
        {
            PathToWeatherFile = "";
            StartDate = DateTime.Today;
            StopDate = DateTime.Today;
            DetailsUrl = "";
            Description = "Scenario generated by Dairy-CropSyst Editor";
            Accumulations = 3;
            SimulationPeriodMode = "multiyear";
            IrrigationPumpModel = "";
            ParameterizedScenario = 1;

            this.Cow = new Cow();
            this.Barn = new Barn();
            this.AnaerobicDigester = new AnaerobicDigester();
            this.CourseSeparator = new CourseSeparator();
            this.FineSeparator = new FineSeparator();
            this.NutrientRecovery = new NutrientRecovery();
            this.Lagoon = new Lagoon();
            this.Fertigation = new Fertigation();
            this.Fertigation.ApplicationDate_date = DateTime.Today;
            this.Field = new Field();
        }
    }
}
