using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteWeatherStation
    {
        private double _windSensorHeightAboveGround = 10;
        private double _windSpeedProfileExponent = 0.14;
        private double _windSpeedProfileBoundaryLayerThickness = 270;
        private double _airTemperatureSensorHeightAboveGround = 1.5;

        public double WindSensorHeightAboveGround
        {
            get => _windSensorHeightAboveGround;
            set
            {
                if (value > 0)
                {
                    _windSensorHeightAboveGround = value;
                }
            }
        }

        public double WindSpeedProfileExponent 
        { 
            get => _windSpeedProfileExponent; 
            set
            {
                if (value >= 0)
                {
                    _windSpeedProfileExponent = value;
                }
            }
        }


        public double WindSpeedProfileBoundaryLayerThickness
        {
            get => _windSpeedProfileBoundaryLayerThickness;
            set
            {
                if (value >= 0)
                {
                    _windSpeedProfileBoundaryLayerThickness = value;
                }
            }
        }

        public double AirTemperatureSensorHeightAboveGround
        {
            get => _airTemperatureSensorHeightAboveGround;
            set
            {
                if (value >= 0)
                {
                    _airTemperatureSensorHeightAboveGround = value;
                }
            }
        }

        public SiteWeatherStation() { }

        private static List<SiteWeatherStation> list = new List<SiteWeatherStation>();

        public static void Add(SiteWeatherStation siteWeatherStation)
        {
            list.Add(siteWeatherStation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:WeatherStation,\n" +
                    ($"  {list[i].WindSensorHeightAboveGround}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].WindSpeedProfileExponent}" + ",").PadRight(27, ' ') + " !-Latitude {{ deg }}\n" +
                    ($"  {list[i].WindSpeedProfileBoundaryLayerThickness}" + ",").PadRight(27, ' ') + " !-Longitude {{ deg }}\n" +
                    ($"  {list[i].AirTemperatureSensorHeightAboveGround}" + ";").PadRight(27, ' ') + " !-Elevation {{ m }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteWeatherStation.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Clear()
        {
            list.Clear();
        }
    }
}
