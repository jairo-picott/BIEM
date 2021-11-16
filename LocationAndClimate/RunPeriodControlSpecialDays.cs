using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum SpecialDayTypeEnum
    {
        Holiday,
        SummerDesignDay,
        WinterDesignDay,
        CustomDay1,
        CustomDay2
    }
    public class RunPeriodControlSpecialDays
    {
        private string _name;
        private string _startDate;
        private int _duration = 1;
        private SpecialDayTypeEnum _specialDayType = SpecialDayTypeEnum.Holiday;

        public string Name { get => _name; set => _name = value; }
        public string StartDate { get => _startDate; set => _startDate = value; }
        public int Duration
        {
            get => _duration;
            set
            {
                if (value>=1 && value <= 366)
                {
                    _duration = value;
                }
            }
        }
        public SpecialDayTypeEnum SpecialDayType { get => _specialDayType; set => _specialDayType = value; }

        public RunPeriodControlSpecialDays() { }

        private static List<RunPeriodControlSpecialDays> list = new List<RunPeriodControlSpecialDays>();

        public static void Add(RunPeriodControlSpecialDays runPeriodControlSpecialDays)
        {
            list.Add(runPeriodControlSpecialDays);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"RunPeriodControl:SpecialDays,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].StartDate}" + ",").PadRight(27, ' ') + " !-Start Date\n" +
                    ($"  {list[i].Duration}" + ",").PadRight(27, ' ') + " !-Duration\n" +
                    ($"  {list[i].SpecialDayType}" + ";").PadRight(27, ' ') + " !-Special Day Type";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in RunPeriodControlSpecialDays.Read())
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
