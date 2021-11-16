using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BIEM.SimulationParameters
{
    public enum HVACSystemRootFindingAlgorithmEnum
    {
        RegulaFalsi,
        Bisection,
        BisectionThenTegulaFalsi,
        RegulaFalsiThenBisection,
        Alternation
    }
    public class HVACSystemRootFindingAlgorithm
    {
        private HVACSystemRootFindingAlgorithmEnum _algorithm = HVACSystemRootFindingAlgorithmEnum.RegulaFalsi;
        public HVACSystemRootFindingAlgorithmEnum Algorithm { get => _algorithm; set => _algorithm = value; }

        private double _numberofIterationsBeforeAlgorithmSwitch = 5;
        public double NumberofIterationsBeforeAlgorithmSwitch { get => _numberofIterationsBeforeAlgorithmSwitch; set => _numberofIterationsBeforeAlgorithmSwitch = value; }

        public HVACSystemRootFindingAlgorithm() { }

        private static List<HVACSystemRootFindingAlgorithm> list = new List<HVACSystemRootFindingAlgorithm>();

        public static void Add(HVACSystemRootFindingAlgorithm hVACSystemRootFindingAlgorithm)
        {
            list.Add(hVACSystemRootFindingAlgorithm);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"HVACSystemRootFindingAlgorithm,\n" +
                    ($"  {list[i].Algorithm}" + ",").PadRight(27, ' ') + " !-Algorithm\n" +
                    ($"  {list[i].NumberofIterationsBeforeAlgorithmSwitch}" + ";").PadRight(27, ' ') + " !-Number of Iterations Before Algorithm Switch";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in HVACSystemRootFindingAlgorithm.Read())
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
