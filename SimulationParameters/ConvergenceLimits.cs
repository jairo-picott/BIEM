using System.Collections.Generic;
using System.IO;

namespace BIEM.SimulationParameters
{
    public class ConvergenceLimits
    {
        private double _minimumSystemTimestep;
        public double MinimunSystemTimestep
        {
            get => _minimumSystemTimestep;
            set
            {
                if (value >= 0 && value <= 60)
                {
                    _minimumSystemTimestep = value;
                }
            }
        }

        private double _maximumHVACIterations = 20;
        public double MaximumHVACIterations
        {
            get => _maximumHVACIterations;
            set
            {
                if (value >= 1)
                {
                    _maximumHVACIterations = value;
                }
            }
        }

        private double _minimumPlantIterations = 2;
        public double MinimumPlantIterations
        {
            get => _minimumPlantIterations;
            set
            {
                if (value >= 1)
                {
                    _minimumPlantIterations = value;
                }
            }
        }

        private double _maximumPlantIterations = 8;
        public double MaximumPlantIterations
        {
            get => _maximumPlantIterations;
            set
            {
                if (value >= 2)
                {
                    _maximumPlantIterations = value;
                }
            }
        }

        public ConvergenceLimits() { }

        private static List<ConvergenceLimits> list = new List<ConvergenceLimits>();

        public static void Add(ConvergenceLimits convergenceLimits)
        {
            list.Add(convergenceLimits);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ConvergenceLimits,\n" +
                    ($"  {list[i].MinimunSystemTimestep}" + ",").PadRight(27, ' ') + " !-Minimun System Timestep {{ minutes }}\n" +
                    ($"  {list[i].MaximumHVACIterations}" + ",").PadRight(27, ' ') + " !-Maximum HVAC Iterations\n" +
                    ($"  {list[i].MinimumPlantIterations}" + ",").PadRight(27, ' ') + " !-Minimum Plant Iterations\n" +
                    ($"  {list[i].MaximumPlantIterations}" + ";").PadRight(27, ' ') + " !-Maximum Plant Iterations";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ConvergenceLimits.Read())
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
