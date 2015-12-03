using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class Fertigation : Entity
    {
        public DateTime ApplicationDate_date { get; set; }
        public double AmountRemoved_percent { get; set; }
        public string SourceFacility_id { get; set; }
        public string TargetField_id { get; set; }
        public int Repetition_d { get; set; }

        // removal_units
        // application_method
    }
}
