using System;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.IO.DataAccess;

namespace Wsu.DairyCafo.DataAccess
{
    public class ScenarioFile : IniFile, IScenarioFile
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
            if (String.IsNullOrEmpty(iniString))
                return null;

            string[] s = iniString.Split(' ');
            return s[0];
        }
    }
}
