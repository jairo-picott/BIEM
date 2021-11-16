using System.Collections.Generic;
using System.IO;

namespace BIEM.SimulationParameters
{
    public enum CarbonDioxideConcentrationEnum
    {
        Yes,
        No
    }

    public enum GenerciContaminantConcentrationEnum
    {
        Yes,
        No
    }
    public class ZoneAirContaminantBalance
    {
        private CarbonDioxideConcentrationEnum _carbonDioxideConcentration = CarbonDioxideConcentrationEnum.No;
        public CarbonDioxideConcentrationEnum CarbonDioxideConcentration { get => _carbonDioxideConcentration; set => _carbonDioxideConcentration = value; }

        private string _outdoorCarbonDioxideScheduleName;
        public string OutdoorCarbonDioxideScheduleName { get => _outdoorCarbonDioxideScheduleName; set => _outdoorCarbonDioxideScheduleName = value; }

        private GenerciContaminantConcentrationEnum _genericContaminantConcentration = GenerciContaminantConcentrationEnum.No;
        public GenerciContaminantConcentrationEnum GenericContaminantConcentration { get => _genericContaminantConcentration; set => _genericContaminantConcentration = value; }

        private string _outdoorGenericContaminantScheduleName;
        public string OutdoorGenericContaminantScheduleName { get => _outdoorGenericContaminantScheduleName; set => _outdoorGenericContaminantScheduleName = value; }

        public ZoneAirContaminantBalance() { }

        private static List<ZoneAirContaminantBalance> list = new List<ZoneAirContaminantBalance>();

        public static void Add(ZoneAirContaminantBalance zoneAirContaminantBalance)
        {
            list.Add(zoneAirContaminantBalance);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneAirContaminantBalance,\n" +
                    ($"  {list[i].CarbonDioxideConcentration}" + ",").PadRight(27, ' ') + " !-Carbon Dioxide Concentration\n" +
                    ($"  {list[i].OutdoorCarbonDioxideScheduleName}" + ",").PadRight(27, ' ') + " !-Outdoor Carbon Dioxide Schedule Name\n" +
                    ($"  {list[i].GenericContaminantConcentration}" + ",").PadRight(27, ' ') + " !-Generic Contaminant Concentration\n" +
                    ($"  {list[i].OutdoorGenericContaminantScheduleName}" + ";").PadRight(27, ' ') + " !-Outdoor Generic Contaminant Schedule Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneAirContaminantBalance.Read())
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
