using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.IO.DataAccess;

namespace Wsu.DairyCafo.DataAccess
{
    public class ScenarioFile : IniFile
    {
        public ScenarioFile(string pathToFile) : base(pathToFile)
        {

        }
        public string GetValueOnly(string sectionName, string key)
        {
            string s = base.GetValue(sectionName, key);
            string val = cleanStr(s);

            return val;
        }

        private string cleanStr(string iniString)
        {
            string[] s = iniString.Split(' ');
            return s[0];
        }
    }
}
