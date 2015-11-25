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
        public string LoadedPath { get; private set; }
        public ScenarioFile() : base() { }
        public ScenarioFile(string pathToFile) : base(pathToFile) { }
        public string GetValueOnly(string sectionName, string key)
        {
            string s = base.GetValue(sectionName, key);
            string val = cleanStr(s);

            return val;
        }
        public override bool Load(string filename)
        {
            bool isLoaded = base.Load(filename);
            if (isLoaded) LoadedPath = filename;
            return isLoaded;
        }
        private string cleanStr(string iniString)
        {
            string[] s = iniString.Split(' ');
            return s[0];
        }
    }
}
