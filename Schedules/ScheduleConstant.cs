using System.IO;
using System.Collections.Generic;

namespace BIEM.Schedules
{
    //--------Schedule Constant
    //THIS WAS CREATED TO PRINT UNIQUE SCHEDULES THAT ARE APPLICABLE FOR
    //ALL ZONES, THEN WILL NOT BE DUPLICATED, HERE SHCEDULES ALWAYS ON,
    //ALWAYS OFF AND DEFAULT ARE DEFINED.
    public class ScheduleConstant
    {

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private string _scheduleTypeLimitsName;
        public string ScheduleTypeLimitsName
        {
            get => _scheduleTypeLimitsName;
            set => _scheduleTypeLimitsName = value;
        }

        private double? _hourlyValue = 0;
        public double? HourlyValue
        {
            get => _hourlyValue;
            set => _hourlyValue = value;
        }

        public ScheduleConstant() { }

        private static List<ScheduleConstant> list = new List<ScheduleConstant>();

        public static void Add(ScheduleConstant scheduleConstant)
        {
            list.Add(scheduleConstant);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:Constant,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ScheduleTypeLimitsName}" + ",").PadRight(27, ' ') + " !-Schedule Type Limits Name\n" +
                    ($"  {list[i].HourlyValue}" + ";").PadRight(27, ' ') + " !-Hourly Value";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleConstant.Read())
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
