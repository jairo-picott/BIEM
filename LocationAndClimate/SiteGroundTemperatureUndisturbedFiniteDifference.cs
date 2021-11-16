using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteGroundTemperatureUndisturbedFiniteDifference
    {
        private string _name;
        private double _soilThermalConductivity;
        private double _soilDensity;
        private double _soilSpecificHeat;
        private double _soilMoistureContentVolumeFraction = 30;
        private double _soilMoistureContentVolumeFractionatSaturation = 50;
        private double _evapotranspirationGroundCoverParameter = 0.4;

        public string Name { get => _name; set => _name = value; }
        public double SoilThermalConductivity 
        { 
            get => _soilThermalConductivity; 
            set
            {
                if (value > 0)
                {
                    _soilThermalConductivity = value;
                }
            }
        }
        public double SoilDensity 
        { 
            get => _soilDensity; 
            set
            {
                if (value > 0)
                {
                    _soilDensity = value;
                }
            }
        }
        public double SoilSpecificHeat 
        { 
            get => _soilSpecificHeat;
            set
            {
                if (value > 0)
                {
                    _soilSpecificHeat = value;
                }
            }
        }
        public double SoilMoistureContentVolumeFraction 
        { 
            get => _soilMoistureContentVolumeFraction;
            set
            {
                if (value >=0 && value <= 100)
                {
                    _soilMoistureContentVolumeFraction = value;
                }
            }
        }
        public double SoilMoistureContentVolumeFractionatSaturation 
        { 
            get => _soilMoistureContentVolumeFractionatSaturation;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _soilMoistureContentVolumeFractionatSaturation = value;
                }
            }
        }
        public double EvapotranspirationGroundCoverParameter 
        { 
            get => _evapotranspirationGroundCoverParameter;
            set
            {
                if (value >= 0 && value <= 1.5)
                {
                    _evapotranspirationGroundCoverParameter = value;
                }
            }
        }

        public SiteGroundTemperatureUndisturbedFiniteDifference() { }
        private static List<SiteGroundTemperatureUndisturbedFiniteDifference> list = new List<SiteGroundTemperatureUndisturbedFiniteDifference>();

        public static void Add(SiteGroundTemperatureUndisturbedFiniteDifference siteGroundTemperatureUndisturbedFiniteDifference)
        {
            list.Add(siteGroundTemperatureUndisturbedFiniteDifference);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundTemperature:Undisturbed:FiniteDifference,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SoilThermalConductivity}" + ",").PadRight(27, ' ') + " !-Soil Thermal Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].SoilDensity}" + ",").PadRight(27, ' ') + " !-Soil Density{{ kg/m3 }}\n" +
                    ($"  {list[i].SoilSpecificHeat}" + ",").PadRight(27, ' ') + " !-Soil Specific Heat{{ J/kg-K }}\n" +
                    ($"  {list[i].SoilMoistureContentVolumeFraction}" + ",").PadRight(27, ' ') + " !-Soil Moisture Content Volume Fraction{{ % }}\n" +
                    ($"  {list[i].SoilMoistureContentVolumeFractionatSaturation}" + ",").PadRight(27, ' ') + " !-Soil Moisture Content Volume Fraction at Saturation{{ % }}\n" +
                    ($"  {list[i].EvapotranspirationGroundCoverParameter}" + ";").PadRight(27, ' ') + " !-Evapotranspiration Ground Cover Parameter{{ dimensionless }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundTemperatureUndisturbedFiniteDifference.Read())
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
