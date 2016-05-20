using System;
using System.Collections.Generic;
using System.Device.Location;
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
        /// <summary>
        /// <note>Converts a negative longitude to positive in order to
        /// work with filenames that do not have negatives.  We assume
        ///locations are within the USA</note>
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public string GetWeather(double latitude, double longitude)
        {
            if (!Directory.Exists(pathToWeatherDatabase))
                throw new ArgumentException("Unable to locate weather database");

            if (longitude < 0) longitude *= -1;

            List<GeoCoordinate> locations = getLocations(pathToWeatherDatabase);

            if(locations.Count > 0)
            {
                var coord = new GeoCoordinate(latitude, longitude);
                var nearest = locations
                    .Select(x => new GeoCoordinate(x.Latitude, x.Longitude))
                    .OrderBy(x => x.GetDistanceTo(coord))
                    .First();

                string nearestFile = convertGeoCoordinateToFilename(nearest);

                if(nearestFile == "")
                    throw new FileNotFoundException(
                        "Unable to find weather file");

                return Path.Combine(pathToWeatherDatabase, nearestFile);
            }

            throw new FileNotFoundException("Unable to find weather file");
        }
        private List<GeoCoordinate> getLocations(string pathToWeatherDir)
        {
            List<GeoCoordinate> locations = new List<GeoCoordinate>();

            // Parses files that are named with their geocoordinates
            // 457211864.UED

            string[] files = Directory.GetFiles(pathToWeatherDir);

            foreach(string file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                double lat = Convert.ToDouble(filename.Substring(0, 4)) / 100;
                double lon = Convert.ToDouble(filename.Substring(4, 5)) / 100;

                locations.Add(new GeoCoordinate(lat, lon));
            }

            return locations;
        }

        private string convertGeoCoordinateToFilename(GeoCoordinate geoCoordinate)
        {
            string lat = geoCoordinate.Latitude.ToString("N2");
            string lon = geoCoordinate.Longitude.ToString("N2");

            string filename = lat.Replace(".", "") + lon.Replace(".", "") + ".UED";

            if (File.Exists(Path.Combine(pathToWeatherDatabase, filename)))
                return filename;
            else
                return "";
        }
    }
}
