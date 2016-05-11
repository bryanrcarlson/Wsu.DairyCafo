using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Tests.Stubs;
using Wsu.DairyCafo.DataAccess.Dto;

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

        /// <summary>
        /// Tests whether a simple scenario is correctly loaded and parsed
        /// <remark>
        /// Scenario file includes scenario, cow, barn, course sep, and lagoon
        /// </remark>
        /// </summary>
        [TestMethod]
        public void LoadParse_SimpleScenario_ParsesCorrectly()
        {
            /// Arrange
            var dairy = new ScenarioFile();
            var field = new ScenarioFile();
            var reader = new ScenarioReader(dairy, field);
            var sut = new Scenario();

            /// Act
            reader.Load(@"Assets\simpleScenario.NIFA_dairy_scenario");
            sut = reader.Parse();

            /// Assert 
            //(Only testing subset of all data)

            // [scenario]
            Assert.AreEqual(@"http://foo.io", sut.DetailsUrl);
            Assert.AreEqual(2005, sut.StartDate.Year);
            Assert.AreEqual(
                @"C:\Simulation\Projects\example\Database\Weather\ID-CH4.UED", 
                sut.PathToWeatherFile);
            
            // [cow_description]
            Assert.AreEqual(635, sut.Cow.BodyMass_kg);
            Assert.AreEqual(true, sut.Cow.IsLactating);
            Assert.AreEqual(8, sut.Cow.PhManure_mol_L);
            Assert.AreEqual(150, sut.Cow.MetabolizableEnergyDiet_MJ_d);

            // [barn]
            Assert.AreEqual("sand", sut.Barn.Bedding);
            Assert.AreEqual(1.5, sut.Barn.BeddingAddition);
            Assert.AreEqual(3, sut.Barn.CleaningFrequency);

            // [manure_storage] (lagoon)
            Assert.AreEqual(142250, sut.Lagoon.SurfaceArea_m2);
            Assert.AreEqual(519213, sut.Lagoon.VolumeMax_m3);
            Assert.AreEqual(8.5, sut.Lagoon.PH_mol_L);
        }

        /// <summary>
        /// Tests whether a complex scenario is correctly loaded and parsed
        /// <remark>
        /// Scenario file includes scenario, cow, barn, AD, course sep, 
        /// fine sep, nutrient recovery, lagoon (and all required storage tanks
        /// for routing), fertigation, and apply outside biomatter
        /// </remark>
        /// </summary>
        [TestMethod]
        public void LoadParse_ComplexScenario_ParsesCorrectly()
        {
            /// Arrange
            var dairy = new ScenarioFile();
            var field = new ScenarioFile();
            var reader = new ScenarioReader(dairy, field);
            var sut = new Scenario();

            /// Act
            reader.Load(@"Assets\complexScenario.NIFA_dairy_scenario");
            sut = reader.Parse();

            /// Assert 
            //(Only testing subset of all data)

            // [scenario]
            Assert.AreEqual(@"This is just a test", sut.Description);
            Assert.AreEqual(1979, sut.StartDate.Year);
            Assert.AreEqual(
                @"multiyear",
                sut.SimulationPeriodMode);

            // [barn]
            Assert.AreEqual(3000, sut.Barn.NumberCows_cnt);
            Assert.AreEqual(3, sut.Barn.CleaningFrequency);

            // [manure_storage] (lagoon)
            Assert.AreEqual(38250.0, sut.Lagoon.SurfaceArea_m2);
            Assert.AreEqual(153000.0, sut.Lagoon.VolumeMax_m3);
            Assert.AreEqual(8.5, sut.Lagoon.PH_mol_L);

            // [manure_separator] (AD)
            Assert.AreEqual("AD", sut.AnaerobicDigester.Id);
            Assert.AreEqual("anaerobic digester", sut.AnaerobicDigester.Style);
            Assert.AreEqual("barn", sut.AnaerobicDigester.SourceFacility);
            Assert.AreEqual("AD tank", sut.AnaerobicDigester.LiquidFacility);
            Assert.AreEqual("<off-site>", sut.AnaerobicDigester.SolidFacility);

            // [manure_separator] (course sep)
            Assert.AreEqual("course sep", sut.CourseSeparator.Id);
            Assert.AreEqual("fiber separator", sut.CourseSeparator.Style);
            Assert.AreEqual("AD tank", sut.CourseSeparator.SourceFacility);
            Assert.AreEqual("CS tank", sut.CourseSeparator.LiquidFacility);
            Assert.AreEqual("<off-site>", sut.CourseSeparator.SolidFacility);

            // [manure_separator] (fine sep)
            Assert.AreEqual("fine sep", sut.FineSeparator.Id);
            Assert.AreEqual("dissolved air flotation", sut.FineSeparator.Style);
            Assert.AreEqual("CS tank", sut.FineSeparator.SourceFacility);
            Assert.AreEqual("FS tank", sut.FineSeparator.LiquidFacility);
            Assert.AreEqual("<off-site>", sut.FineSeparator.SolidFacility);

            // [manure_separator] (nut rec)
            Assert.AreEqual("nut rec", sut.NutrientRecovery.Id);
            Assert.AreEqual("ammonia stripper", sut.NutrientRecovery.Style);
            Assert.AreEqual("FS tank", sut.NutrientRecovery.SourceFacility);
            Assert.AreEqual("lagoon", sut.NutrientRecovery.LiquidFacility);
            Assert.AreEqual("<off-site>", sut.NutrientRecovery.SolidFacility);

            // [receive_off_farm_biomass:1]
            Assert.AreEqual(1, sut.ReceiveOffFarmBiomass.ApplicationDate.Year);
            Assert.AreEqual(28672.3, 
                sut.ReceiveOffFarmBiomass.Biomatter.Mass_kg);
            Assert.AreEqual(1743.25, 
                sut.ReceiveOffFarmBiomass.Biomatter.CarbonOrganicResilient_kg);
        }
    }
}
