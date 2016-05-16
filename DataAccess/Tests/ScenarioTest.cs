using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Tests.Helpers;

namespace Wsu.DairyCafo.DataAccess.Tests
{
    [TestClass]
    public class ScenarioTest
    {
        [TestMethod]
        public void GetCountManureSeparator_ComplexScenario_ReturnsCorrectNum()
        {
            // Arrange
            var injector = new Injector();
            var sut = injector.GetComplexScenario();

            // Act
            int seps = sut.GetCountManureSeparator();

            // Assert
            Assert.AreEqual(injector.NumberSeparatorsComplex, seps);
        }

        [TestMethod]
        public void GetCountManureSeparator_SimpleScenario_ReturnsCorrectNum()
        {
            // Arrange
            var injector = new Injector();
            var sut = injector.GetSimpleScenario();

            // Act
            int seps = sut.GetCountManureSeparator();

            // Assert
            Assert.AreEqual(injector.NumberSeparatorsSimple, seps);
        }

        [TestMethod]
        public void GetCountManureStorage_ComplexScenario_ReturnsCorrectNum()
        {
            // Arrange
            var injector = new Injector();
            var sut = injector.GetComplexScenario();

            // Act
            int storage = sut.GetCountManureStorage();

            // Assert
            Assert.AreEqual(injector.NumberStorageComplex, storage);
        }

        [TestMethod]
        public void GetCountManureStorage_SimpleScenario_ReturnsCorrectNum()
        {
            // Arrange
            var injector = new Injector();
            var sut = injector.GetSimpleScenario();

            // Act
            int storage = sut.GetCountManureStorage();

            // Assert
            Assert.AreEqual(injector.NumberStorageSimple, storage);
        }
    }
}
