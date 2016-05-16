using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;

namespace Wsu.DairyCafo.DataAccess.Tests.Stubs
{
    public class ScenarioFileStub : IScenarioFile
    {
        public string LoadedPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Dictionary<string, Dictionary<string, string>> Clear()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, Dictionary<string, string>> Clear(string sectionName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetSection(string sectionName)
        {
            throw new NotImplementedException();
        }

        public string GetValue(string sectionName, string key)
        {
            throw new NotImplementedException();
        }

        public string GetValueOnly(string sectionName, string key)
        {
            throw new NotImplementedException();
        }

        public bool Load(string filename)
        {
            return false;
        }

        public bool LoadContents(string fileContents)
        {
            throw new NotImplementedException();
        }

        public bool Save(string filename)
        {
            throw new NotImplementedException();
        }

        public bool Save(string filename, List<string> orderedSections)
        {
            throw new NotImplementedException();
        }

        public string SaveContents()
        {
            throw new NotImplementedException();
        }

        public string SaveContents(List<string> orderedSections)
        {
            throw new NotImplementedException();
        }

        public void SetSection(string sectionName, IDictionary<string, string> sectionValues)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string sectionName, string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
