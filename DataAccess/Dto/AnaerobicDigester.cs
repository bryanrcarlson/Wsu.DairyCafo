using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class AnaerobicDigester : ManureSeparator
    {
        public AnaerobicDigester() : base()
        {
            this.Style = ManureSeperatorStyles.AnaerobicDigester;
            this.Id = "AD";
        }
        public AnaerobicDigester(ManureSeparator toCopy)
        {
            if (toCopy.Style != ManureSeperatorStyles.AnaerobicDigester)
                throw new ArgumentException("toCopy.Style is not of proper type");

            Copy(toCopy);
        }
    }
}
