using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public abstract class Entity
    {
        public string Id { get; set; }
        public bool Enabled { get; set; }
    }
}
