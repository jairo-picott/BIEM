using System.IO;
using System.Collections.Generic;


namespace BIEM.SimulationParameters
{
    public class ZoneCapacitanceMultiplierResearchSpecial
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private string _zoneorZoneListName;
        public string ZoneorZoneListName
        {
            get => _zoneorZoneListName;
            set => _zoneorZoneListName = value;
        }

        private int? _temperatureCapacityMultiplier = 1;
        public int? TemperatureCapacityMultiplier
        {
            get => _temperatureCapacityMultiplier;
            set
            {
                if (value>0)
                {
                    _temperatureCapacityMultiplier = value;
                }
            }
        }

        private int? _humidityCapacityMultiplier = 1;
        public int? HumidityCapacityMultiplier
        {
            get => _humidityCapacityMultiplier;
            set
            {
                if (value > 0)
                {
                    _humidityCapacityMultiplier = value;
                }
            }
        }

        private int? _carbonDioxideCapacityMultiplier = 1;
        public int? CarbonDioxideCapacityMultiplier
        {
            get => _carbonDioxideCapacityMultiplier;
            set
            {
                if (value > 0)
                {
                    _carbonDioxideCapacityMultiplier = value;
                }
            }
        }

        private int? _genericContaminantCapacityMultiplier = 1;
        public int? GenericContaminantCapacityMultiplier
        {
            get => _genericContaminantCapacityMultiplier;
            set
            {
                if (value > 0)
                {
                    _genericContaminantCapacityMultiplier = value;
                }
            }
        }

        public ZoneCapacitanceMultiplierResearchSpecial() { }

        private static List<ZoneCapacitanceMultiplierResearchSpecial> list = new List<ZoneCapacitanceMultiplierResearchSpecial>();

        public static void Add(ZoneCapacitanceMultiplierResearchSpecial zoneCapacitanceMultiplierResearchSpecial)
        {
            list.Add(zoneCapacitanceMultiplierResearchSpecial);

            Errors er = new Errors();
            if (zoneCapacitanceMultiplierResearchSpecial.Name == null)
            {
                er.Class = "ZoneCapacitanceMultiplier:ResearchSpecial";
                er.Field = "Name";
                Errors.Add(er);
            }

        }

        
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneCapacitanceMultiplier:ResearchSpecial,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ZoneorZoneListName}" + ",").PadRight(27, ' ') + " !-Zone or Zone List Name\n" +
                    ($"  {list[i].TemperatureCapacityMultiplier}" + ",").PadRight(27, ' ') + " !-Temperature Capacity Multiplier\n" +
                    ($"  {list[i].HumidityCapacityMultiplier}" + ",").PadRight(27, ' ') + " !-Humidity Capacity Multiplier\n" +
                    ($"  {list[i].CarbonDioxideCapacityMultiplier}" + ",").PadRight(27, ' ') + " !-Carbon Dioxide Capacity Multiplier\n" +
                    ($"  {list[i].GenericContaminantCapacityMultiplier}" + ";").PadRight(27, ' ') + " !-Generic Contaminant Capacity Multiplier";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneCapacitanceMultiplierResearchSpecial.Read())
                {
                    sw.WriteLine(line);
                    
                    
                }
            }



        }
        public static void Collect(SimulationParameters.ZoneCapacitanceMultiplierResearchSpecial zoneCapacitance)
        {
            zoneCapacitance.Name = null;
            zoneCapacitance.ZoneorZoneListName = null;
            zoneCapacitance.TemperatureCapacityMultiplier = null;
            zoneCapacitance.HumidityCapacityMultiplier = null;
            zoneCapacitance.CarbonDioxideCapacityMultiplier = null;
            zoneCapacitance.GenericContaminantCapacityMultiplier = null;
        }

    }
}
