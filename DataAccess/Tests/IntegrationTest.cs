using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.DairyCafo.DataAccess.Dto;
using System.IO;

namespace Wsu.DairyCafo.DataAccess.Tests
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void ReadComplexEditWriteSimple()
        {
            // Arrange
            ScenarioFile dairy = new ScenarioFile();
            ScenarioFile field = new ScenarioFile();
            ScenarioDefaults d = new ScenarioDefaults();
            WeatherGrabber g = new WeatherGrabber(@"Assets\Database\Weather");
            string readPath = @"Assets\complexScenario.NIFA_dairy_scenario";
            string writePath = @"Assets\IntegrationTest";
            string comparePath = @"Assets\simpleScenario.NIFA_dairy_scenario";
            ScenarioReader r = new ScenarioReader(dairy, field);
            ScenarioWriter w = new ScenarioWriter(dairy, field, d, g);

            Directory.CreateDirectory(writePath);

            // Act
            r.Load(readPath);
            Scenario scenario = r.Parse();
            scenario.Description = "";

            scenario.Cow.BodyMass_kg = 635;
            scenario.Cow.DryMatterIntake_kg_d = 24;
            scenario.Cow.MilkProduction_kg_d = 34;
            scenario.Cow.CrudeProteinDiet_percent = 17.6;
            scenario.Cow.StarchDiet_percent = 12.83;
            scenario.Cow.AcidDetergentFiberDiet_percent = 26.08;

            scenario.Barn.ManureAlleyArea_m2 = 3000;
            scenario.Barn.NumberCows_cnt = 1000;

            scenario.Lagoon.SurfaceArea_m2 = 142250;
            scenario.Lagoon.VolumeMax_m3 = 519213;

            scenario.Fertigation.Enabled = false;
            scenario.Field.Id = "field";
            scenario.AnaerobicDigester.Enabled = false;
            scenario.FineSeparator.Enabled = false;
            scenario.NutrientRecovery.Enabled = false;
            scenario.ReceiveOffFarmBiomass.Enabled = false;

            w.Write(scenario, writePath);

            // Assert
            Assert.IsTrue(
                checkFilesMatch(
                    comparePath, Path.Combine(writePath, ".NIFA_dairy_scenario")));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            string path = @"Assets\IntegrationTest";
            Directory.Delete(path, true);
        }
        private bool checkFilesMatch(string file1, string file2)
        {
            bool filesDoMatch = true;

            using (StreamReader f1 = new StreamReader(file1))
            using (StreamReader f2 = new StreamReader(file2))
            {
                while (true)
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
