using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.DataAccess.Tests.Stubs
{
    public class ScenarioWriterStub : IScenarioWriter
    {
        public void SetupDir(string dirPath)
        {
            throw new NotImplementedException();
        }

        public void Write(Scenario s)
        {
            throw new NotImplementedException();
        }

        public void Write(Scenario s, string dirPath)
        {
            throw new NotImplementedException();
        }

        public void WriteField(Scenario s)
        {
            throw new NotImplementedException();
        }
    }
}
