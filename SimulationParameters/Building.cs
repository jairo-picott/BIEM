using System.IO;
using System.Collections.Generic;

namespace BIEM.SimulationParameters
{
    public enum TerrainType
    {
        NotDefined,
        Suburbs,
        Country,
        City,
        Ocean,
        Urban
    }

    public enum SolarDistributionType
    {
        NotDefined,
        FullExterior,
        MinimalShadowing,
        FullInteriorAndExterior,
        FullExteriorWithReflections,
        FullInteriorAndExteriorWithReflections
    }
    public class Building
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        private double? _northaxis = 0;
        public double? NorthAxis
        {
            get => _northaxis;
            set => _northaxis = value;
        }
        private TerrainType _terrain = TerrainType.Suburbs;
        public TerrainType Terrain
        { 
            get => _terrain; 
            set => _terrain = value; 
        }
        private double? _loadsConvergenceToleranceValue = 0.04;
        public double? LoadsConvergenceToleranceValue
        {
            get => _loadsConvergenceToleranceValue;
            set
            {
                if (value > 0 && value <= 0.5)
                {
                    _loadsConvergenceToleranceValue = value;
                }
            }
        }
        private double? _temperatureConvergenceToleranceValue = 0.4;
        public double? TemperatureConvergenceToleranceValue
        {
            get => _temperatureConvergenceToleranceValue;
            set
            {
                if(value > 0 && value <= 0.5)
                {
                    _temperatureConvergenceToleranceValue = value;
                }
            }
        }
        private SolarDistributionType _solarDistribution = SolarDistributionType.FullExterior;
        public SolarDistributionType SolarDistribution
        { 
            get=>_solarDistribution; 
            set=>_solarDistribution = value; 
        }
        public int? MaximumNumberofWarmupDays { get; set; }
        public int? MinimumNumberofWarmupDays { get; set; }


        public Building() { }

        private static List<Building> list = new List<Building>();

        public static void Add(Building building)
        {
            list.Add(building);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Building,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].NorthAxis}" + ",").PadRight(27, ' ') + " !-North Axis{{ deg }}\n" +
                    ($"  {list[i].Terrain}" + ",").PadRight(27, ' ') + " !-Terrain\n" +
                    ($"  {list[i].LoadsConvergenceToleranceValue}" + ",").PadRight(27, ' ') + " !-Loads Convergence Tolerance Value{{ W }}\n" +
                    ($"  {list[i].TemperatureConvergenceToleranceValue}" + ",").PadRight(27, ' ') + " !-Temperature Convergence Tolerance Value{{ deltaC }}\n" +
                    ($"  {list[i].SolarDistribution}" + ",").PadRight(27, ' ') + " !-Solar Distribution\n" +
                    ($"  {list[i].MaximumNumberofWarmupDays}" + ",").PadRight(27, ' ') + " !-Maximum Number of Warmup Days\n" +
                    ($"  {list[i].MinimumNumberofWarmupDays}" + ";").PadRight(27, ' ') + " !-Minimum Number of Warmup Days";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in Building.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        
    }
}
