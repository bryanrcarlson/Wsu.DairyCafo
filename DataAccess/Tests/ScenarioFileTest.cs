using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Wsu.DairyCafo.DataAccess.Tests
{
    [TestClass]
    public class ScenarioFileTest
    {
        [TestMethod]
        public void Clear_SectionNotPresent_ReturnsEmpty()
        {
            // Arrange
            ScenarioFile sut = new ScenarioFile();
            string sect = "not-a-section";
            // Act
            var actual = sut.Clear(sect);

            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void Clear_UnumberedSectionPresent_ReturnsSectionAndCleared()
        {
            // Arrange
            ScenarioFile sut = new ScenarioFile();
            Dictionary<string, string> sect1 = new Dictionary<string, string>();
            sect1.Add("key1", "val1");
            Dictionary<string, string> sect2 = new Dictionary<string, string>();
            sect2.Add("key2", "val2");
            sut.SetSection("sect1", sect1);
            sut.SetSection("sect2", sect2);

            // Act
            var actual = sut.Clear("sect2");

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(0, sut.GetSection("sect2").Count);
            Assert.AreEqual(sect2["key2"], actual["sect2"]["key2"]);
        }

        [TestMethod]
        public void ClearNumberedSectionPresent_ReturnsSectionAndCleared()
        {
            // Arrange
            ScenarioFile sut = new ScenarioFile();
            Dictionary<string, string> sect1 = new Dictionary<string, string>();
            sect1.Add("key1", "val1");
            Dictionary<string, string> sect2 = new Dictionary<string, string>();
            sect2.Add("key2", "val2");
            sut.SetSection("sect:1", sect1);
            sut.SetSection("sect:2", sect2);

            // Act
            var actual = sut.Clear("sect");

            // Assert
            Assert.AreEqual(0, sut.SaveContents().Length);
            Assert.AreEqual(sect2["key2"], actual["sect:2"]["key2"]);
        }
    }
}
