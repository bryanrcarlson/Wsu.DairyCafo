using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class Scenario
    {
        public string PathToWeatherFile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }

        public Cow Cow { get; set; }
        public Barn Barn { get; set; }
        public AnaerobicDigester AnaerobicDigester { get; set; }
        public CourseSeparator CourseSeparator { get; set; }
        public FineSeparator FineSeparator { get; set; }
        public NutrientRecovery NutrientRecovery { get; set; }
        public Lagoon Lagoon { get; set; }
        public Field Field { get; set; }

        //public Collection<ManureSeparator> ManureSeparators { get; set; }

        public Scenario()
        {
            PathToWeatherFile = "";
            StartDate = DateTime.Today;
            StopDate = DateTime.Today;

            this.Cow = new Cow();
            this.Barn = new Barn();
            this.AnaerobicDigester = new AnaerobicDigester();
            this.CourseSeparator = new CourseSeparator();
            this.FineSeparator = new FineSeparator();
            this.NutrientRecovery = new NutrientRecovery();
            this.Lagoon = new Lagoon();
            this.Field = new Field();
        }
    }
}
