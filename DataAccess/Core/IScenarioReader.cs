using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.DataAccess.Core
{
    public interface IScenarioReader
    {
        Scenario Parse();
    }
}
