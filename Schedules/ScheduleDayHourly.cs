using System.Collections.Generic;
using System.IO;

namespace BIEM.Schedules
{
    public class ScheduleDayHourly
    {
        private string _name;
        private string _scheduleTypeLimitsName;
        private string _hour1;
        private string _hour2;
        private string _hour3;
        private string _hour4;
        private string _hour5;
        private string _hour6;
        private string _hour7;
        private string _hour8;
        private string _hour9;
        private string _hour10;
        private string _hour11;
        private string _hour12;
        private string _hour13;
        private string _hour14;
        private string _hour15;
        private string _hour16;
        private string _hour17;
        private string _hour18;
        private string _hour19;
        private string _hour20;
        private string _hour21;
        private string _hour22;
        private string _hour23;
        private string _hour24;

        public string Name { get => _name; set => _name = value; }
        public string ScheduleTypeLimitsName { get => _scheduleTypeLimitsName; set => _scheduleTypeLimitsName = value; }
        public string Hour1 { get => _hour1; set => _hour1 = value; }
        public string Hour2 { get => _hour2; set => _hour2 = value; }
        public string Hour3 { get => _hour3; set => _hour3 = value; }
        public string Hour4 { get => _hour4; set => _hour4 = value; }
        public string Hour5 { get => _hour5; set => _hour5 = value; }
        public string Hour6 { get => _hour6; set => _hour6 = value; }
        public string Hour7 { get => _hour7; set => _hour7 = value; }
        public string Hour8 { get => _hour8; set => _hour8 = value; }
        public string Hour9 { get => _hour9; set => _hour9 = value; }
        public string Hour10 { get => _hour10; set => _hour10 = value; }
        public string Hour11 { get => _hour11; set => _hour11 = value; }
        public string Hour12 { get => _hour12; set => _hour12 = value; }
        public string Hour13 { get => _hour13; set => _hour13 = value; }
        public string Hour14 { get => _hour14; set => _hour14 = value; }
        public string Hour15 { get => _hour15; set => _hour15 = value; }
        public string Hour16 { get => _hour16; set => _hour16 = value; }
        public string Hour17 { get => _hour17; set => _hour17 = value; }
        public string Hour18 { get => _hour18; set => _hour18 = value; }
        public string Hour19 { get => _hour19; set => _hour19 = value; }
        public string Hour20 { get => _hour20; set => _hour20 = value; }
        public string Hour21 { get => _hour21; set => _hour21 = value; }
        public string Hour22 { get => _hour22; set => _hour22 = value; }
        public string Hour23 { get => _hour23; set => _hour23 = value; }
        public string Hour24 { get => _hour24; set => _hour24 = value; }

        public ScheduleDayHourly() { }

        private static List<ScheduleDayHourly> list = new List<ScheduleDayHourly>();

        public static void Add(ScheduleDayHourly scheduleDayHourly)
        {
            list.Add(scheduleDayHourly);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:Day:Hourly,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ScheduleTypeLimitsName}" + ",").PadRight(27, ' ') + " !-Schedule Type Limits Name\n" +
                    ($"  {list[i].Hour1}" + ",").PadRight(27, ' ') + " !-Hour 1\n" +
                    ($"  {list[i].Hour2}" + ",").PadRight(27, ' ') + " !-Hour 2\n" +
                    ($"  {list[i].Hour3}" + ",").PadRight(27, ' ') + " !-Hour 3\n" +
                    ($"  {list[i].Hour4}" + ",").PadRight(27, ' ') + " !-Hour 4\n" +
                    ($"  {list[i].Hour5}" + ",").PadRight(27, ' ') + " !-Hour 5\n" +
                    ($"  {list[i].Hour6}" + ",").PadRight(27, ' ') + " !-Hour 6\n" +
                    ($"  {list[i].Hour7}" + ",").PadRight(27, ' ') + " !-Hour 7\n" +
                    ($"  {list[i].Hour8}" + ",").PadRight(27, ' ') + " !-Hour 8\n" +
                    ($"  {list[i].Hour9}" + ",").PadRight(27, ' ') + " !-Hour 9\n" +
                    ($"  {list[i].Hour10}" + ",").PadRight(27, ' ') + " !-Hour 10\n" +
                    ($"  {list[i].Hour11}" + ",").PadRight(27, ' ') + " !-Hour 11\n" +
                    ($"  {list[i].Hour12}" + ",").PadRight(27, ' ') + " !-Hour 12\n" +
                    ($"  {list[i].Hour13}" + ",").PadRight(27, ' ') + " !-Hour 13\n" +
                    ($"  {list[i].Hour14}" + ",").PadRight(27, ' ') + " !-Hour 14\n" +
                    ($"  {list[i].Hour15}" + ",").PadRight(27, ' ') + " !-Hour 15\n" +
                    ($"  {list[i].Hour16}" + ",").PadRight(27, ' ') + " !-Hour 16\n" +
                    ($"  {list[i].Hour17}" + ",").PadRight(27, ' ') + " !-Hour 17\n" +
                    ($"  {list[i].Hour18}" + ",").PadRight(27, ' ') + " !-Hour 18\n" +
                    ($"  {list[i].Hour19}" + ",").PadRight(27, ' ') + " !-Hour 19\n" +
                    ($"  {list[i].Hour20}" + ",").PadRight(27, ' ') + " !-Hour 20\n" +
                    ($"  {list[i].Hour21}" + ",").PadRight(27, ' ') + " !-Hour 21\n" +
                    ($"  {list[i].Hour22}" + ",").PadRight(27, ' ') + " !-Hour 22\n" +
                    ($"  {list[i].Hour23}" + ",").PadRight(27, ' ') + " !-Hour 23\n" +
                    ($"  {list[i].Hour24}" + ";").PadRight(27, ' ') + " !-Hour 24";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleDayHourly.Read())
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
