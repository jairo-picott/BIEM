using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteGroundTemperatureUndisturbedXing
    {
        private string _name;
        private double _soilThermalConductivity;
        private double _soilDensity;
        private double _soilSpecificHeat;
        private double _averageSoilSurfaceTemperature;
        private double _soilSurfaceTemperatureAmplitude1;
        private double _soilSurfaceTemperatureAmplitude2;
        private double _phaseShiftofTemperatureAmplitude1;
        private double _phaseShiftofTemperatureAmplitude2;

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
        public double SoilSurfaceTemperatureAmplitude1 { get => _soilSurfaceTemperatureAmplitude1; set => _soilSurfaceTemperatureAmplitude1 = value; }
        public double SoilSurfaceTemperatureAmplitude2 { get => _soilSurfaceTemperatureAmplitude2; set => _soilSurfaceTemperatureAmplitude2 = value; }
        public double PhaseShiftofTemperatureAmplitude1 
        { 
            get => _phaseShiftofTemperatureAmplitude1;
            set
            {
                if (value < 365)
                {
                    _phaseShiftofTemperatureAmplitude1 = value;
                }
            }
        }
        public double PhaseShiftofTemperatureAmplitude2 
        { 
            get => _phaseShiftofTemperatureAmplitude2;
            set
            {
                if (value < 365)
                {
                    _phaseShiftofTemperatureAmplitude2 = value;
                }
            }
        }

        public SiteGroundTemperatureUndisturbedXing() { }
        private static List<SiteGroundTemperatureUndisturbedXing> list = new List<SiteGroundTemperatureUndisturbedXing>();

        public static void Add(SiteGroundTemperatureUndisturbedXing siteGroundTemperatureUndisturbedXing)
        {
            list.Add(siteGroundTemperatureUndisturbedXing);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundTemperature:Undisturbed:Xing,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SoilThermalConductivity}" + ",").PadRight(27, ' ') + " !-Soil Thermal Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].SoilDensity}" + ",").PadRight(27, ' ') + " !-Soil Density{{ kg/m3 }}\n" +
                    ($"  {list[i].SoilSpecificHeat}" + ",").PadRight(27, ' ') + " !-Soil Specific Heat{{ J/kg-K }}\n" +
                    ($"  {list[i].AverageSoilSurfaceTemperature}" + ",").PadRight(27, ' ') + " !-Average Soil Surface Temperature{{ C }}\n" +
                    ($"  {list[i].SoilSurfaceTemperatureAmplitude1}" + ",").PadRight(27, ' ') + " !-Soil Surface Temperature Amplitude 1{{ deltaC }}\n" +
                    ($"  {list[i].SoilSurfaceTemperatureAmplitude2}" + ",").PadRight(27, ' ') + " !-Soil Surface Temperature Amplitude 2{{ deltaC }}\n" +
                    ($"  {list[i].PhaseShiftofTemperatureAmplitude1}" + ",").PadRight(27, ' ') + " !-Phase Shift of Temperature Amplitude 1{{ days }}\n" +
                    ($"  {list[i].PhaseShiftofTemperatureAmplitude2}" + ";").PadRight(27, ' ') + " !-Phase Shift of Temperature Amplitude 2{{ days }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundTemperatureUndisturbedXing.Read())
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
