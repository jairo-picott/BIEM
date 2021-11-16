using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteHeightVariation
    {
        private double _windSpeedProfileExponent = 0.22;
        private double _windSpeedProfileBoundaryLayerThickness = 370;
        private double _airTemperatureGradienteCoefficient = 0.0065;

        public double WindSpeedProfileExponent 
        {
            get => _windSpeedProfileExponent; 
            set
            {
                if (value >= 0.22)
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
                if (value > 0)
                {
                    _windSpeedProfileBoundaryLayerThickness = value;

                }
            }
        }
        public double AirTemperatureGradienteCoefficient 
        { 
            get => _airTemperatureGradienteCoefficient;
            set
            {
                if (value > 0)
                {
                    _airTemperatureGradienteCoefficient = value;

                }
            }
        }

        public SiteHeightVariation() { }

        private static List<SiteHeightVariation> list = new List<SiteHeightVariation>();

        public static void Add(SiteHeightVariation siteHeightVariation)
        {
            list.Add(siteHeightVariation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:HeightVariation,\n" +
                    ($"  {list[i].WindSpeedProfileExponent}" + ",").PadRight(27, ' ') + " !-Wind Speed Profile Exponent\n" +
                    ($"  {list[i].WindSpeedProfileBoundaryLayerThickness}" + ",").PadRight(27, ' ') + " !-Wind Speed Profile Boundary Layer Thickness {{ m }}\n" +
                    ($"  {list[i].AirTemperatureGradienteCoefficient}" + ";").PadRight(27, ' ') + " !-Air Temperature Gradiente Coefficient {{ K/m }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteHeightVariation.Read())
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
