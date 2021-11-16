using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteGroundTemperatureUndisturbedKusudaAchenbach
    {
        private string _name;
        private double _soilThermalConductivity;
        private double _soilDensity;
        private double _soilSpecificHeat;
        private double _averageSoilSurfaceTemperature;
        private double _averageAmplitudeofSurfaceTemperature;
        private double _phaseShiftofMinimumSurfaceTemperature;

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
        public double AverageSoilSurfaceTemperature { get => _averageSoilSurfaceTemperature; set => _averageSoilSurfaceTemperature = value; }
        public double AverageAmplitudeofSurfaceTemperature 
        { 
            get => _averageAmplitudeofSurfaceTemperature;
            set
            {
                if (value > 0)
                {
                    _averageAmplitudeofSurfaceTemperature = value;

                }
            }
        }
        public double PhaseShiftofMinimumSurfaceTemperature 
        { 
            get => _phaseShiftofMinimumSurfaceTemperature;
            set
            {
                if (value >= 0 && value < 365)
                {
                    _phaseShiftofMinimumSurfaceTemperature = value;

                }
            }
        }

        public SiteGroundTemperatureUndisturbedKusudaAchenbach() { }

        private static List<SiteGroundTemperatureUndisturbedKusudaAchenbach> list = new List<SiteGroundTemperatureUndisturbedKusudaAchenbach>();

        public static void Add(SiteGroundTemperatureUndisturbedKusudaAchenbach siteGroundTemperatureUndisturbedKusudaAchenbach)
        {
            list.Add(siteGroundTemperatureUndisturbedKusudaAchenbach);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundTemperature:Undisturbed:KusudaAchenbach,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SoilThermalConductivity}" + ",").PadRight(27, ' ') + " !-Soil Thermal Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].SoilDensity}" + ",").PadRight(27, ' ') + " !-Soil Density{{ kg/m3 }}\n" +
                    ($"  {list[i].SoilSpecificHeat}" + ",").PadRight(27, ' ') + " !-Soil Specific Heat{{ J/kg-K }}\n" +
                    ($"  {list[i].AverageSoilSurfaceTemperature}" + ",").PadRight(27, ' ') + " !-Average Soil Surface Temperature{{ C }}\n" +
                    ($"  {list[i].AverageAmplitudeofSurfaceTemperature}" + ",").PadRight(27, ' ') + " !-Average Amplitude of Surface Temperature{{ deltaC }}\n" +
                    ($"  {list[i].PhaseShiftofMinimumSurfaceTemperature}" + ";").PadRight(27, ' ') + " !-Phase Shift of Minimum Surface Temperature{{ days }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundTemperatureUndisturbedKusudaAchenbach.Read())
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
