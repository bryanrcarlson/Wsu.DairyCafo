using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class NutrientRecovery : ManureSeparator
    {
        public NutrientRecovery() : base()
        {
            this.Style = ManureSeperatorStyles.NutrientRecovery;
            this.Id = "nut rec";
        }
        public NutrientRecovery(ManureSeparator toCopy)
        {
            if (toCopy.Style != ManureSeperatorStyles.NutrientRecovery)
                throw new ArgumentException("toCopy.Style is not of proper type");

            Copy(toCopy);
        }
    }
}
