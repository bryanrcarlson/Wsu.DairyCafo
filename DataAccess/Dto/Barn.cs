using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class Barn : Entity
    {
        public double Manure_alley_area_m2 { get; set; }
        public double Number_cows_cnt { get; set; }
    }
}
