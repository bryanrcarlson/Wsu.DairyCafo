using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Core
{
    public class ScenarioDefaults : IScenarioDefaults
    {
        public Dictionary<string, string> GetScenarioDefaults()
        {
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                //{ "details_URL", "" },
                //{ "description", "" },
                { "accumulations", "3" },
                { "simulation_period_mode", "multiyear" },
                { "irrigation_pump_model", "" },
                { "parameterized_scenario", "1" },
                { "barn:count", "1" },
                { "cow_description:count", "1" }
            };

            return d;
        }
        public Dictionary<string, string> GetVersionDefaults()
        {
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                {"major", "0" },
                {"release", "0" },
                {"minor", "0" }
            };

            return d;
        }
        public Dictionary<string, string> GetBarnDefaults()
        {
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                { "ID", "barn" },
                { "enable", "true" },
                { "flush_system", "true" },
                { "bedding", "sand" },
                { "bedding_addition", "1.5" },
                { "cow_description", "<default cow>" }
            };

            return d;
        }
        public Dictionary<string, string> GetCowDefaults()
        {
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                { "ID", "herd" },
                { "enable", "true" },
                { "diet_starch", "0.0" },
                { "diet_ADF", "0.0" },
                { "lactating", "true" }
            };

            return d;
        }
        public Dictionary<string, string> GetLagoonDefaults()
        {
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                { "ID", "lagoon" },
                { "enable", "true" },
                { "style", "lagoon_aerobic" },
                { "fresh_manure", "true" }
            };

            return d;
        }
        public Dictionary<string, string> GetHoldingTankDefaults()
        {
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                { "enable", "true" },
                { "style", "tank" },
                { "fresh_manure", "true" },
                { "surface_area", "10000" },
                { "volume_max", "16000" }
            };

            return d;
        }
        public Dictionary<string, string> GetFertigationDefaults()
        {
            Dictionary<string, string> d = new Dictionary<string, string>()
            {
                { "enable", "true" },
                { "application_method", "surface_broadcast_no_incorporation" }
            };

            return d;
        }
    }
}
