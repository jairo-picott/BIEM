using System.Collections.Generic;
using System.IO;

namespace BIEM.SimulationParameters
{
    public enum ZoneAirHeatBalanceAlgorithmEnum
    {
        ThirdOrderBackwardDifference,
        AnalyticalSolution,
        EulerMethod
    }
    public class ZoneAirHeatBalanceAlgorithm
    {
        private ZoneAirHeatBalanceAlgorithmEnum _algorithm = ZoneAirHeatBalanceAlgorithmEnum.ThirdOrderBackwardDifference;
        public ZoneAirHeatBalanceAlgorithmEnum Algorithm { get => _algorithm; set => _algorithm = value; }

        public ZoneAirHeatBalanceAlgorithm() { }

        private static List<ZoneAirHeatBalanceAlgorithm> list = new List<ZoneAirHeatBalanceAlgorithm>();

        public static void Add(ZoneAirHeatBalanceAlgorithm zoneAirHeatBalanceAlgorithm)
        {
            list.Add(zoneAirHeatBalanceAlgorithm);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneAirHeatBalanceAlgorithm,\n" +
                    ($"  {list[i].Algorithm}" + ";").PadRight(27, ' ') + " !-Algorithm";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneAirHeatBalanceAlgorithm.Read())
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
