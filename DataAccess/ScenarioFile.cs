using System;
using System.Collections.Generic;
using System.IO;
using Wsu.DairyCafo.DataAccess.Core;
using Wsu.IO.DataAccess;

namespace Wsu.DairyCafo.DataAccess
{
    public class ScenarioFile : IniFile, IScenarioFile
    {
        public string LoadedPath { get; private set; }

        public ScenarioFile() : base() { }
        public ScenarioFile(string pathToFile) : base(pathToFile) { }
        public string GetValueOnly(string sectionName, string key)
        {
            string s = base.GetValue(sectionName, key);
            string val = cleanStr(s);
            if (val == null) val = "";
            return val;
        }
        /// <summary>
        /// Overrides base to set isLoaded
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override bool Load(string filename)
        {
            bool isLoaded = base.Load(filename);
            if (isLoaded)
            {
                LoadedPath = filename;
            }
            return isLoaded;
        }
        /// <summary>
        /// Overrides base to make a backup of original before writing
        /// 
        /// Write the in-memory contents to the specified file in the ini format 
        /// and write [Sections] in the order specified by orderedSections.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="orderedSections"></param>
        /// <returns>True if copy, delete, and save successful</returns>
        public override bool Save(string filename, List<string> orderedSections)
        {
            // Make backup of current file by renaming, then save
            try
            {
                File.Delete(filename + ".bak");
                File.Copy(filename, filename + ".bak");
                File.Delete(filename);
            }
            catch
            {
                return false;
            }
            return base.Save(filename, orderedSections);
            
        }
        public override Dictionary<string, Dictionary<string, string>> Clear(
            string sectionName)
        {
            Dictionary<string, Dictionary<string, string>> clearedContent;

            // Clear any sections that are not numbered
            clearedContent = base.Clear(sectionName);

            // Did not find section, so perhaps it's numbered
            if (clearedContent.Count == 0)
                clearedContent = clearNumberedSection(sectionName);
            
            return clearedContent;
        }
        private Dictionary<string, Dictionary<string, string>> 
            clearNumberedSection(string sectionType)
        {
            int i = 1;
            bool doContinue = true;
            Dictionary<string, Dictionary<string, string>> clearedContent = 
                new Dictionary<string, Dictionary<string, string>>();

            do
            {
                string sectionName = sectionType + ":" + i;
                Dictionary<string, string> sectVals = 
                    this.GetSection(sectionName);

                if (sectVals.Count > 0)
                {
                    clearedContent[sectionName] = sectVals;
                    Clear(sectionName);
                    i++;
                }
                else { doContinue = false; }
            } while (doContinue);

            return clearedContent;
        }
        private string cleanStr(string iniString)
        {
            if (String.IsNullOrEmpty(iniString))
                return null;

            string[] s = iniString.Split(' ');
            return s[0];
        }
    }
}
