using System;
using System.Collections.Generic;
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
        public DateTime EndDate { get; set; }

        public Cow Cow { get; set; }
        public Barn Barn { get; set; }
        public AnaerobicDigester AnaerobicDigester { get; set; }
        public CourseSeparator CourseSeparator { get; set; }
        public FineSeparator FineSeparator { get; set; }
        public NutrientRecovery NutrientRecovery { get; set; }
        public Lagoon Lagoon { get; set; }
        public Field Field { get; set; }
    }
}
