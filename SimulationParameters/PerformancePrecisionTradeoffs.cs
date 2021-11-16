using System.Collections.Generic;
using System.IO;

namespace BIEM.SimulationParameters
{
    public enum UseCoilDirectSolutionsEnum
    {
        Yes,
        No
    }

    public enum ZoneRadiantExchangeAlgorithmEnum
    {
        ScriptF,
        CarrollMRT
    }
    public enum OverrideModeEnum
    {
        Normal,
        Model01,
        Model02,
        Model03,
        Model04,
        Model05,
        Model06,
        Model07,
        Advanced
    }
    public class PerformancePrecisionTradeoffs
    {
        private UseCoilDirectSolutionsEnum? _useCoilDirectSolutions = UseCoilDirectSolutionsEnum.No;
        public UseCoilDirectSolutionsEnum? UseCoilDirectSolutions { get => _useCoilDirectSolutions; set => _useCoilDirectSolutions = value; }

        private ZoneRadiantExchangeAlgorithmEnum? _zoneRadiantExchangeAlgorithm = ZoneRadiantExchangeAlgorithmEnum.ScriptF;
        public ZoneRadiantExchangeAlgorithmEnum? ZoneRadiantExchangeAlgorithm { get => _zoneRadiantExchangeAlgorithm; set => _zoneRadiantExchangeAlgorithm = value; }

        private OverrideModeEnum? _overrideMode = OverrideModeEnum.Normal;
        public OverrideModeEnum? OverrideMode { get => _overrideMode; set => _overrideMode = value; }

        private double? _maxZoneTempDiff = 0.3;
        public double? MaxZoneTempDiff
        {
            get => _maxZoneTempDiff;
            set
            {
                if (value >= 0.1 && value <= 3)
                {
                    _maxZoneTempDiff = value;
                }
            }
        }

        private double? _maxAllowedDelTemp = 0.002;
        public double? MaxAllowedDelTemp
        {
            get => _maxAllowedDelTemp;
            set
            {
                if (value >= 0.002 && value <= 0.1)
                {
                    _maxAllowedDelTemp = value;
                }
            }
        }

        public PerformancePrecisionTradeoffs() { }

        private static List<PerformancePrecisionTradeoffs> list = new List<PerformancePrecisionTradeoffs>();

        public static void Add(PerformancePrecisionTradeoffs performancePrecisionTradeoffs)
        {
            list.Add(performancePrecisionTradeoffs);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"PerformancePrecisionTradeoffs,\n" +
                    ($"  {list[i].UseCoilDirectSolutions}" + ",").PadRight(27, ' ') + " !-Use Coil Direct Solutions\n" +
                    ($"  {list[i].ZoneRadiantExchangeAlgorithm}" + ",").PadRight(27, ' ') + " !-Zone Radiant Exchange Algorithm\n" +
                    ($"  {list[i].OverrideMode}" + ",").PadRight(27, ' ') + " !-Override Mode \n" +
                    ($"  {list[i].MaxZoneTempDiff}" + ",").PadRight(27, ' ') + " !-MaxZone TempDiff \n" +
                    ($"  {list[i].MaxAllowedDelTemp}" + ";").PadRight(27, ' ') + " !-MaxAllowed DelTemp ";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in PerformancePrecisionTradeoffs.Read())
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
