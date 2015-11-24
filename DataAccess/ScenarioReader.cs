using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.IO.DataAccess;

namespace Wsu.DairyCafo.DataAccess
{
    public class ScenarioReader : IScenarioReader
    {
        private readonly IniFile dp;

        public ScenarioReader(IniFile iniFile)
        {
            this.dp = iniFile;
        }

        public Scenario Parse()
        {
            Scenario s = new Scenario();
            string[] startDate =
                dp.GetValue("dairy scenario", "start_date").Split();
            int year = Convert.ToInt16(startDate[0].Substring(0, 4));
            int doy = Convert.ToInt16(startDate[0].Substring(4,3));
            DateTime dt = new DateTime(year, 1, 1).AddDays(doy - 1);
            s.StartDate = dt;

            return s;
        }
    }
}
