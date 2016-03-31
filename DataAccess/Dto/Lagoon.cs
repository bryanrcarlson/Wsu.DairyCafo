using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class Lagoon : ManureStorage
    {
        //public double SurfaceArea_m2 { get; set; }
        //public double VolumeMax_m3 { get; set; }

        public Lagoon()
        {
            this.Id = "lagoon";
        }
    }
}
