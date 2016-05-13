using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Tests.Stubs;
using Wsu.DairyCafo.DataAccess.Core;
using System.IO;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.DairyCafo.DataAccess.Tests.Helpers;
using System.Collections.Generic;

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

        [TestMethod]
        public void Write_ComplexScenario_WritesAllValues()
        {
            // Arrange
            var i = new Injector();
            var s = i.GetComplexScenario();

            Directory.CreateDirectory(scenDirPath);

            var field = new ScenarioFile();
            var dairy = new ScenarioFile();
            var scenDefaults = new ScenarioDefaults();

            var sut = new ScenarioWriter(dairy, field, scenDefaults);

            // Act
            sut.Write(s, scenDirPath);

            // Assert
            bool filesMatch = checkFilesMatch(
                Path.Combine(scenDirPath, ".NIFA_dairy_scenario").ToString(),
                @"Assets\complexScenario.NIFA_dairy_scenario");
            Assert.IsTrue(filesMatch);
        }
        [TestMethod]
        public void Write_SimpleScenario_WritesAllValues()
        {
            // Arrange
            var i = new Injector();
            var s = i.GetSimpleScenario();

            Directory.CreateDirectory(scenDirPath);

            var field = new ScenarioFile();
            var dairy = new ScenarioFile();
            var scenDefaults = new ScenarioDefaults();

            var sut = new ScenarioWriter(dairy, field, scenDefaults);

            // Act
            sut.Write(s, scenDirPath);

            // Assert
            bool filesMatch = checkFilesMatch(
                Path.Combine(scenDirPath, ".NIFA_dairy_scenario").ToString(),
                @"Assets\simpleScenario.NIFA_dairy_scenario");
            Assert.IsTrue(filesMatch);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(scenDirPath))
                Directory.Delete(scenDirPath, true);
        }
        private bool checkFilesMatch(string file1, string file2)
        {
            bool filesDoMatch = true;

            using (StreamReader f1 = new StreamReader(file1))
            using (StreamReader f2 = new StreamReader(file2))
            {
                while(true)
                {
                    if (f1.EndOfStream || f2.EndOfStream)
                        break;
                    string l1 = f1.ReadLine();
                    string l2 = f2.ReadLine();

                    if (l1 != l2)
                    {
                        filesDoMatch = false;
                        break;
                    }
                }
            }

            return filesDoMatch;
        }
    }
}
