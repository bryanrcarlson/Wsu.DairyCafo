using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    /// <summary>
    /// Represents a barn
    /// <remark>
    /// Data is represented as a section in the scenario ini file as e.g.:
    /// [barn:1]
    /// ID=barn
    /// manure_pump=
    /// enable=true
    /// manure_alley_surface_area=7650  This is the area where animals stand and deficate, it does not include stalls.
    /// manure_alley_surface_area_units=
    /// flush_system=true
    /// bedding=sand
    /// bedding_addition= 1.5  per animal
    /// bedding_addition_units=
    /// cow_population=3000  cows
    /// cow_population_units=
    /// cow_description=herd
    /// drinking_water_pump=
    /// cleaning_frequency=3
    /// </remark>
    /// </summary>
    public class Barn : Entity
    {
        public double ManureAlleyArea_m2 { get; set; }
        public double NumberCows_cnt { get; set; }
        public bool FlushSystem { get; set; }
        public string Bedding { get; set; }
        public double BeddingAddition { get; set; }
        public string CowDescription { get; set; }
        public string DrinkingWaterPump { get; set; }
        public int CleaningFrequency { get; set; }

        public Barn()
        {
            Id = "barn";
        }
    }
}
