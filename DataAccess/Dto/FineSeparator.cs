using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class FineSeparator : ManureSeparator
    {
        public FineSeparator() : base()
        {
            this.Style = ManureSeperatorStyles.FineSeparator;
            this.Id = "fine sep";
        }
        public FineSeparator(ManureSeparator toCopy)
        {
            if (toCopy.Style != ManureSeperatorStyles.FineSeparator)
                throw new ArgumentException("toCopy.Style is not of proper type");

            Copy(toCopy);
        }
    }
}
