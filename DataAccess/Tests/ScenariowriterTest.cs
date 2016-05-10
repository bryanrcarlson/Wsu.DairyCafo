using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Tests.Stubs;
using Wsu.DairyCafo.DataAccess.Core;
using System.IO;

namespace Wsu.DairyCafo.DataAccess.Tests
{
    [TestClass]
    public class ScenarioWriterTest
    {
        static string scenDirPath = @"Assets\ScenarioDir";

        [TestMethod]
        public void SetupDir_EmptyValidPath_CreatesDirs()
        {
            // Arrange
            var field = new ScenarioFileStub();
            var dairy = new ScenarioFileStub();
            var scenDefaults = new ScenarioDefaults();
            //var scenDirPath = @"Assets\ScenarioDir";
            
            Directory.CreateDirectory(scenDirPath);

            var sut = new ScenarioWriter(field, dairy, scenDefaults);
            // Act
            sut.SetupDir(scenDirPath);

            // Assert
            var outputExists = Directory.Exists(Path.Combine(scenDirPath, "Output"));
            var fieldsExists = Directory.Exists(Path.Combine(scenDirPath, "Fields"));

            Assert.IsTrue(outputExists);
            Assert.IsTrue(fieldsExists);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (Directory.Exists(scenDirPath))
                Directory.Delete(scenDirPath, true);
        }
    }
}
