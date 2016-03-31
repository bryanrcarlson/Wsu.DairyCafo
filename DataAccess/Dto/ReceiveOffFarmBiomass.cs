using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    /// <summary>
    /// Represents an event to add biomass from off farm to a facility
    /// <remark>
    /// Represented as a section in the scenario ini file as, e.g.:
    /// [receive_off_farm_biomass:1]
    /// ID=init_lagoon
    /// manure_pump =
    /// enable = true
    /// application_date=0
    /// destination_facility_ID=lagoon
    /// mass = 28672.3
    /// mass_units=
    /// biomatter=
    /// h2o_kg=1099210
    /// nitrogen_urea_kg=0
    /// nitrogen_ammonical_kg=1620.69
    /// nitrogen_organic_kg=1280.05
    /// carbon_inorganic_kg=549.35
    /// carbon_organic_fast_kg=9543.61
    /// carbon_organic_slow_kg=3073.16
    /// carbon_organic_resilient_kg=1743.25
    /// phosphorus_inorganic_kg=695.841
    /// phosphorus_organic_kg=53.41
    /// potassium_inorganic_kg=2050.08
    /// potassium_organic_kg=0
    /// decomposition_constant_fast=0.254867
    /// decomposition_constant_slow=0.015624
    /// decomposition_constant_resilient=2.74e-6
    /// </remark>
    /// </summary>
    public class ReceiveOffFarmBiomass : Entity
    {
        public Biomatter Biomatter { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string DestinationFacilityID { get; set; }
        public string BiomatterLabel { get; set; }
    }
}
