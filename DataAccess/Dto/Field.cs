using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class Field : Entity
    {
        public double Area_ha { get; set; }
        public string Crop { get; set; }
    }
}
