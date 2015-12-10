using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.IO.Core;

namespace Wsu.DairyCafo.DataAccess.Core
{
    public interface IScenarioFile : IIniProvider
    {
        string GetValueOnly(string sectionName, string key);
        string LoadedPath { get; }
    }
}
