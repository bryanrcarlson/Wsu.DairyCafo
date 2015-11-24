using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class Cow : Entity
    {
        public double BodyMass_kg { get; set; }
        public double DryMatterIntake_kg_d { get; set; }
        public double MilkProduction_kg_d { get; set; }
        public double CrudeProteinDiet_kg_d { get; set; }
    }
}
