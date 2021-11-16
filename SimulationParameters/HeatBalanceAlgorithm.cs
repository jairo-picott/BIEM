using System.IO;
using System.Collections.Generic;

namespace BIEM.SimulationParameters
{
    public enum HeatBalanceAlgorithmType
    {
        ConductionTransferFunction,
        MoisturePenetrationDepthConductionTransferFunction,
        ConductionFiniteDifference,
        CombinedHeadAndMoistureFiniteElement
    }
    public class HeatBalanceAlgorithm
    {
        private HeatBalanceAlgorithmType? _algorithm;
        public HeatBalanceAlgorithmType? Algorithm
        {
            get => _algorithm;
            set => _algorithm = value;
        }

        private double? _surfaceTemperatureUpperLimit = 200;
        public double? SurfaceTemperatureUpperLimit
        {
            get => _surfaceTemperatureUpperLimit;
            set
            {
                if (value >= 200)
                {
                    _surfaceTemperatureUpperLimit = value;
                }
            }
        }

        private double? _minimumSurfaceConvectionHeatTransferCoefficientValue = 0.1;
        public double? MinimumSurfaceConvectionHeatTransferCoefficientValue
        {
            get => _minimumSurfaceConvectionHeatTransferCoefficientValue;
            set
            {
                if (value > 0)
                {
                    _minimumSurfaceConvectionHeatTransferCoefficientValue = value;
                }
            }
        }

        private double? _maximumSurfaceConvectionHeatTransferConefficientValue = 1000;
        public double? MaximumSurfaceConvectionHeatTransferCoefficientValue
        {
            get => _maximumSurfaceConvectionHeatTransferConefficientValue;
            set
            {
                if (value >= 1)
                {
                    _maximumSurfaceConvectionHeatTransferConefficientValue = value;
                }
            }
        }

        public HeatBalanceAlgorithm() { }

        private static List<HeatBalanceAlgorithm> list = new List<HeatBalanceAlgorithm>();

        public static void Add(HeatBalanceAlgorithm heatBalanceAlgorithm)
        {
            list.Add(heatBalanceAlgorithm);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"HeatBalanceAlgorithm,\n" +
                    ($"  {list[i].Algorithm}" + ",").PadRight(27, ' ') + " !-Algorithm\n" +
                    ($"  {list[i].SurfaceTemperatureUpperLimit}" + ",").PadRight(27, ' ') + " !-Surface Temperature Upper Limit{{ C }}\n" +
                    ($"  {list[i].MinimumSurfaceConvectionHeatTransferCoefficientValue}" + ",").PadRight(27, ' ') + " !-Minimum Surface Convection Heat Transfer Coefficient Value{{ W / m2-K }}\n" +
                    ($"  {list[i].MaximumSurfaceConvectionHeatTransferCoefficientValue}" + ";").PadRight(27, ' ') + " !-Maximum Surface Convection Heat Transfer Coefficient Value{{ W / m2-K }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in HeatBalanceAlgorithm.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(SimulationParameters.HeatBalanceAlgorithm heat)
        {
            heat.Algorithm = null;
            heat.SurfaceTemperatureUpperLimit = null;
            heat.MaximumSurfaceConvectionHeatTransferCoefficientValue = null;
            heat.MinimumSurfaceConvectionHeatTransferCoefficientValue = null;
        }
        
    }
}
