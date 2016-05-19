using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wsu.DairyCafo.DataAccess.Core;

namespace Wsu.DairyCafo.DataAccess
{
    public class WeatherGrabber : IWeatherExtractor
    {
        private readonly string pathToWeatherDatabase;


        public WeatherGrabber(string pathToWeatherDatabase)
        {
            if (!Directory.Exists(pathToWeatherDatabase))
                throw new ArgumentException("Weather database does not exist");

            this.pathToWeatherDatabase = pathToWeatherDatabase;
        }
        public string GetWeather(double latitude, double longitude)
        {
            throw new NotImplementedException();
        }
    }
}
