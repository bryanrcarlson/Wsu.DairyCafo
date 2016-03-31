using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    /// <summary>
    /// Represents a cow.  
    /// <remarks>Implemented in the scenario ini file as e.g.:
    /// [cow_description:1]
    /// ID=herd
    /// enable=true
    /// body_mass=600.00  Hint
    /// body_mass_units=
    /// dry_matter_intake=17.50  Dry matter intake will typically be about 1.5% of body weight
    /// dry_matter_intake_units=
    /// milk_production=21.80  Hint
    /// milk_production_units=
    /// diet_crude_protein=16.8  Hint
    /// diet_crude_protein_units=
    /// diet_starch=0.0  Hint
    /// diet_starch_units=
    /// diet_ADF=0.0  ADF should be about 19% of the ration up to 21% in the first 3 weeks of lactation.
    /// diet_ADF_units=
    /// lactating=true
    /// diet_ME_intake=150
    /// manure_pH=8</remarks>
    /// </summary>
    public class Cow : Entity
    {
        public double BodyMass_kg { get; set; }
        public double DryMatterIntake_kg_d { get; set; }
        public double MilkProduction_kg_d { get; set; }
        public double CrudeProteinDiet_percent { get; set; }
        public double StarchDiet_percent { get; set; } 
        public double AcidDetergentFiberDiet_percent { get; set; }
        public bool IsLactating { get; set; }
        public double MetabolizableEnergyDiet_MJ_d { get; set; }
        public double PhManure_mol_L { get; set; }
        
        public Cow()
        {
            Id = "herd";
        }
    }
}
