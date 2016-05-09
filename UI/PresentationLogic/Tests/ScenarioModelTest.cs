using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.DairyCafo.UI.PresentationLogic.Model;

namespace Wsu.DairyCafo.PresentationLogic.Tests
{
    [TestClass]
    public class ScenarioModelTest
    {
        [TestMethod]
        public void SimulationYearGet_DatesSameYear_ReturnsCorrectYear()
        {
            /// Arrange
            int y = 2011;
            var scenario = new Scenario();
            scenario.StartDate = new DateTime(y, 1, 1);
            scenario.StopDate = new DateTime(y, 12, 31);

            var sut = new ScenarioModel(scenario);

            /// Act
            var simYear = sut.SimulationYear;

            /// Assert
            Assert.AreEqual(y, simYear);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SimulationYearGet_DatesDiffYear_ThrowsException()
        {
            /// Arrange
            var scenario = new Scenario();
            scenario.StartDate = new DateTime(2011, 1, 1);
            scenario.StopDate = new DateTime(2012, 12, 31);

            var sut = new ScenarioModel(scenario);

            /// Act
            var simYear = sut.SimulationYear;

            /// Assert - Expect exception
        }

        [TestMethod]
        public void SimulationYearSet_ValidYear_SetsCorrectYear()
        {
            /// Arrange
            int y = 2011;
            var scenario = new Scenario();
            scenario.StartDate = new DateTime(y, 1, 1);
            scenario.StopDate = new DateTime(y, 12, 31);
            int simYear = 2012;

            var sut = new ScenarioModel(scenario);

            /// Act
            sut.SimulationYear = simYear;

            /// Assert
            Assert.AreEqual(simYear, sut.StartDate.Year);
            Assert.AreEqual(simYear, sut.StopDate.Year);
        }
    }
}
