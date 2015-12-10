using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Tests.Stubs;

namespace Wsu.DairyCafo.DataAccess.Tests
{
    [TestClass]
    public class ScenarioReaderTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Load_InvalidPath_ThrowsException()
        {
            // Arrange
            var field = new FalseLoadScenarioFileStub();
            var dairy = new FalseLoadScenarioFileStub();

            var sut = new ScenarioReader(field, dairy);

            // Act
            sut.Load("");

            // Assert - Expect exception

        }
    }
}
