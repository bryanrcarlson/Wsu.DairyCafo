using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Core
{
    public interface IScenarioDefaults
    {
        Dictionary<string, string> GetScenarioDefaults();
        Dictionary<string, string> GetVersionDefaults();
        Dictionary<string, string> GetBarnDefaults();
        Dictionary<string, string> GetCowDefaults();
        Dictionary<string, string> GetLagoonDefaults();
        Dictionary<string, string> GetHoldingTankDefaults();
        Dictionary<string, string> GetFertigationDefaults();
    }
}
