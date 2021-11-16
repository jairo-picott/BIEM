using System.IO;
using System.Collections.Generic;

namespace BIEM.SimulationParameters
{
    public class Timestep
    {
        private int? _numberofTimestepsperHour = 6;
        public int? NumberofTimestepsperHour
        {
            get => _numberofTimestepsperHour;
            set
            {
                if (value>=1 && value <=60)
                {
                    _numberofTimestepsperHour = value;
                }
            }
        }

        public Timestep() { }

        private static List<Timestep> list = new List<Timestep>();

        public static void Add(Timestep timestep)
        {
            list.Add(timestep);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Timestep,\n" +
                    ($"  {list[i].NumberofTimestepsperHour}" + ";").PadRight(27, ' ') + " !-Number of Timesteps per Hour";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in Timestep.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(SimulationParameters.Timestep timestep)
        {
            timestep.NumberofTimestepsperHour = null;
        }

    }
}
