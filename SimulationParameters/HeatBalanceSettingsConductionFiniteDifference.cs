using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BIEM.SimulationParameters
{
    public enum DifferenceSchemeEnum
    {
        FullyImplicitFirstOrder,
        CrankNicholsonSecondOrder
    }

    public class HeatBalanceSettingsConductionFiniteDifference
    {

        private DifferenceSchemeEnum? _differenceScheme = DifferenceSchemeEnum.FullyImplicitFirstOrder;
        public DifferenceSchemeEnum? DifferenceScheme { get => _differenceScheme; set => _differenceScheme = value; }

        private double _spaceDiscretizationConstant = 3;
        public double SpaceDiscretizationConstant { get => _spaceDiscretizationConstant; set => _spaceDiscretizationConstant = value; }

        private double _relaxationFactor = 1;
        public double RelaxationFactor
        {
            get => _relaxationFactor;
            set
            {
                if (value >= 0.01 && value <= 1)
                {
                    _relaxationFactor = value;
                }
            }
        }

        private double _insideFaceSurfaceTemperatureConvergenceCriteria = 0.002;
        public double InsideFaceSurfaceTemperatureConvergenceCriteria
        {
            get => _insideFaceSurfaceTemperatureConvergenceCriteria;
            set
            {
                if (value >= 0.0000001 && value <= 0.01)
                {
                    _insideFaceSurfaceTemperatureConvergenceCriteria = value;
                }
            }
        }

        public HeatBalanceSettingsConductionFiniteDifference() { }

        private static List<HeatBalanceSettingsConductionFiniteDifference> list = new List<HeatBalanceSettingsConductionFiniteDifference>();

        public static void Add(HeatBalanceSettingsConductionFiniteDifference heatBalanceSettingsConductionFiniteDifference)
        {
            list.Add(heatBalanceSettingsConductionFiniteDifference);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"HeatBalanceSettings:ConductionFiniteDifference,\n" +
                    ($"  {list[i].DifferenceScheme}" + ",").PadRight(27, ' ') + " !-Difference Scheme\n" +
                    ($"  {list[i].SpaceDiscretizationConstant}" + ",").PadRight(27, ' ') + " !-Space Discretization Constant\n" +
                    ($"  {list[i].RelaxationFactor}" + ",").PadRight(27, ' ') + " !-Relaxation Factor\n" +
                    ($"  {list[i].InsideFaceSurfaceTemperatureConvergenceCriteria}" + ";").PadRight(27, ' ') + " !-Inside Face Surface Temperature Convergence Criteria";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in HeatBalanceSettingsConductionFiniteDifference.Read())
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
