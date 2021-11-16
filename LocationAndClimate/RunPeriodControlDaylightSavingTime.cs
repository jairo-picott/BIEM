using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class RunPeriodControlDaylightSavingTime
    {
        private string _startDate;
        private string _endDate;

        public string StartDate { get => _startDate; set => _startDate = value; }
        public string EndDate { get => _endDate; set => _endDate = value; }

        public RunPeriodControlDaylightSavingTime() { }

        private static List<RunPeriodControlDaylightSavingTime> list = new List<RunPeriodControlDaylightSavingTime>();

        public static void Add(RunPeriodControlDaylightSavingTime runPeriodControlDaylightSavingTime)
        {
            list.Add(runPeriodControlDaylightSavingTime);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"RunPeriodControl:DaylightSavingTime,\n" +
                    ($"  {list[i].StartDate}" + ",").PadRight(27, ' ') + " !-Start Date\n" +
                    ($"  {list[i].EndDate}" + ";").PadRight(27, ' ') + " !-End Date";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in RunPeriodControlDaylightSavingTime.Read())
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
