using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    /// <summary>
    /// Represents biomatter, with mass, nutrients (C,N,P,K), etc
    /// </summary>
    public class Biomatter
    {
        public double Mass_kg { get; set; }
        public double H2o_kg { get; set; }
        public double NitrogenUrea_kg { get; set; }
        public double NitrogenAmmonical_kg { get; set; }
        public double NitrogenOrganic_kg { get; set; }
        public double CarbonInorganic_kg { get; set; }
        public double CarbonOrganicFast_kg { get; set; }
        public double CarbonOrganicSlow_kg { get; set; }
        public double CarbonOrganicResilient_kg { get; set; }
        public double PhosphorusInorganic_kg { get; set; }
        public double PhosphorusOrganic_kg { get; set; }
        public double PotassiumInorganic_kg { get; set; }
        public double PotassiumOrganic_kg { get; set; }
        public double DecompositionConstantFast { get; set; }
        public double DecompositionConstantSlow { get; set; }
        public double DecompositionConstantResilient { get; set; }
    }
}
