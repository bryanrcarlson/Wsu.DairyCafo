using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wsu.DairyCafo.DataAccess.Core
{
    public interface IWeatherExtractor
    {
        string GetWeather(double latitude, double longitude);
    }
}
