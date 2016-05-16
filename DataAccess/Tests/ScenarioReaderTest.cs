using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wsu.DairyCafo.DataAccess.Tests.Stubs;
using Wsu.DairyCafo.DataAccess.Dto;
using Wsu.DairyCafo.DataAccess.Tests.Helpers;

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
            var field = new ScenarioFile();
            var dairy = new ScenarioFile();

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

            var injector = new Injector();
            var s = injector.GetSimpleScenario();

            /// Act
            reader.Load(@"Assets\simpleScenario.NIFA_dairy_scenario");
            sut = reader.Parse();

            /// Assert 
            //(Only testing subset of all data)

            // [scenario]
            Assert.AreEqual(s.DetailsUrl, sut.DetailsUrl);
            Assert.AreEqual(s.StartDate.Year, sut.StartDate.Year);
            Assert.AreEqual(
                s.PathToWeatherFile, 
                sut.PathToWeatherFile);
            
            // [cow_description]
            Assert.AreEqual(s.Cow.BodyMass_kg, sut.Cow.BodyMass_kg);
            Assert.AreEqual(s.Cow.IsLactating, sut.Cow.IsLactating);
            Assert.AreEqual(s.Cow.PhManure_mol_L, sut.Cow.PhManure_mol_L);
            Assert.AreEqual(s.Cow.MetabolizableEnergyDiet_MJ_d, sut.Cow.MetabolizableEnergyDiet_MJ_d);

            // [barn]
            Assert.AreEqual(s.Barn.Bedding, sut.Barn.Bedding);
            Assert.AreEqual(s.Barn.BeddingAddition, sut.Barn.BeddingAddition);
            Assert.AreEqual(s.Barn.CleaningFrequency, sut.Barn.CleaningFrequency);

            // [manure_storage] (lagoon)
            Assert.AreEqual(s.Lagoon.SurfaceArea_m2, sut.Lagoon.SurfaceArea_m2);
            Assert.AreEqual(s.Lagoon.VolumeMax_m3, sut.Lagoon.VolumeMax_m3);
            Assert.AreEqual(s.Lagoon.PH_mol_L, sut.Lagoon.PH_mol_L);

            // [fertigation_management]
            Assert.AreEqual(s.Fertigation.ApplicationMethod, sut.Fertigation.ApplicationMethod);
            Assert.AreEqual(s.Fertigation.AmountRemoved_percent, sut.Fertigation.AmountRemoved_percent);
            Assert.AreEqual(s.Fertigation.TargetField_id, sut.Fertigation.TargetField_id);
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

            var injector = new Injector();
            var s = injector.GetComplexScenario();

            var sut = new Scenario();

            /// Act
            reader.Load(@"Assets\complexScenario.NIFA_dairy_scenario");
            sut = reader.Parse();

            /// Assert 
            //(Only testing subset of all data)

            // [scenario]
            Assert.AreEqual(s.Description, sut.Description);
            Assert.AreEqual(s.StartDate.Year, sut.StartDate.Year);
            Assert.AreEqual(
                s.SimulationPeriodMode,
                sut.SimulationPeriodMode);

            // [barn]
            Assert.AreEqual(s.Barn.NumberCows_cnt, sut.Barn.NumberCows_cnt);
            Assert.AreEqual(s.Barn.CleaningFrequency, sut.Barn.CleaningFrequency);

            // [manure_storage] (lagoon)
            Assert.AreEqual(s.Lagoon.SurfaceArea_m2, sut.Lagoon.SurfaceArea_m2);
            Assert.AreEqual(s.Lagoon.VolumeMax_m3, sut.Lagoon.VolumeMax_m3);
            Assert.AreEqual(s.Lagoon.PH_mol_L, sut.Lagoon.PH_mol_L);

            // [manure_separator] (AD)
            Assert.AreEqual(s.AnaerobicDigester.Id, sut.AnaerobicDigester.Id);
            Assert.AreEqual(s.AnaerobicDigester.Style, sut.AnaerobicDigester.Style);
            Assert.AreEqual(s.AnaerobicDigester.SourceFacility, sut.AnaerobicDigester.SourceFacility);
            Assert.AreEqual(s.AnaerobicDigester.LiquidFacility, sut.AnaerobicDigester.LiquidFacility);
            Assert.AreEqual(s.AnaerobicDigester.SolidFacility, sut.AnaerobicDigester.SolidFacility);

            // [manure_separator] (course sep)
            Assert.AreEqual(s.CourseSeparator.Id              ,sut.CourseSeparator.Id              );
            Assert.AreEqual(s.CourseSeparator.Style           ,sut.CourseSeparator.Style           );
            Assert.AreEqual(s.CourseSeparator.SourceFacility  ,sut.CourseSeparator.SourceFacility  );
            Assert.AreEqual(s.CourseSeparator.LiquidFacility  ,sut.CourseSeparator.LiquidFacility  );
            Assert.AreEqual(s.CourseSeparator.SolidFacility   ,sut.CourseSeparator.SolidFacility   );

            // [manure_separator] (fine sep)
            Assert.AreEqual(s.FineSeparator.Id              ,sut.FineSeparator.Id                );
            Assert.AreEqual(s.FineSeparator.Style           ,sut.FineSeparator.Style             );
            Assert.AreEqual(s.FineSeparator.SourceFacility  ,sut.FineSeparator.SourceFacility    );
            Assert.AreEqual(s.FineSeparator.LiquidFacility  ,sut.FineSeparator.LiquidFacility    );
            Assert.AreEqual(s.FineSeparator.SolidFacility   ,sut.FineSeparator.SolidFacility     );

            // [manure_separator] (nut rec)
            Assert.AreEqual(s.NutrientRecovery.Id                 ,sut.NutrientRecovery.Id             );
            Assert.AreEqual(s.NutrientRecovery.Style              ,sut.NutrientRecovery.Style          );
            Assert.AreEqual(s.NutrientRecovery.SourceFacility     ,sut.NutrientRecovery.SourceFacility );
            Assert.AreEqual(s.NutrientRecovery.LiquidFacility     ,sut.NutrientRecovery.LiquidFacility );
            Assert.AreEqual(s.NutrientRecovery.SolidFacility      ,sut.NutrientRecovery.SolidFacility  );

            // [receive_off_farm_biomass:1]
            Assert.AreEqual(s.ReceiveOffFarmBiomass.ApplicationDate.Year                   ,sut.ReceiveOffFarmBiomass.ApplicationDate.Year                  );
            Assert.AreEqual(s.ReceiveOffFarmBiomass.Biomatter.Mass_kg                      ,sut.ReceiveOffFarmBiomass.Biomatter.Mass_kg                     );
            Assert.AreEqual(s.ReceiveOffFarmBiomass.Biomatter.CarbonOrganicResilient_kg    ,sut.ReceiveOffFarmBiomass.Biomatter.CarbonOrganicResilient_kg   );

            // [fertigation_management]
            Assert.AreEqual(s.Fertigation.ApplicationMethod,        sut.Fertigation.ApplicationMethod);
            Assert.AreEqual(s.Fertigation.AmountRemoved_percent,    sut.Fertigation.AmountRemoved_percent);
            Assert.AreEqual(s.Fertigation.TargetField_id,           sut.Fertigation.TargetField_id);
        }
    }
}
