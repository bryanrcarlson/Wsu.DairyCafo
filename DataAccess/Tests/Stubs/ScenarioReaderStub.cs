using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;

namespace Wsu.DairyCafo.DataAccess.Tests.Stubs
{
    public class ScenarioReaderStub : IScenarioReader
    {
        public bool Clean()
        {
            throw new NotImplementedException();
        }

        public void Load(string filePath)
        {
            throw new NotImplementedException();
        }

        public Scenario Parse()
        {
            throw new NotImplementedException();
        }
    }
}
