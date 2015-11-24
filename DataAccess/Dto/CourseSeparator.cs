using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;

namespace Wsu.DairyCafo.DataAccess.Dto
{
    public class CourseSeparator : ManureSeparator
    {
        #region 'structors
        public CourseSeparator() : base()
        {
            this.Style = ManureSeperatorStyles.CourseSeparator;
        }
        public CourseSeparator(ManureSeparator toCopy)
        {
            if (toCopy.Style != ManureSeperatorStyles.CourseSeparator)
                throw new ArgumentException("toCopy.Style is not of proper type");

            Copy(toCopy);
        }
        #endregion
    }
}
