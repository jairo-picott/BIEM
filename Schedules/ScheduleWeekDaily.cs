using System.Collections.Generic;
using System.IO;

namespace BIEM.Schedules
{
    public class ScheduleWeekDaily
    {
        private string _name;
        private string _sundayScheduleDayName;
        private string _mondayScheduleDayName;
        private string _tuesdayScheduleDayName;
        private string _wednesdayScheduleDayName;
        private string _thursdayScheduleDayName;
        private string _fridayScheduleDayName;
        private string _saturdayScheduleDayName;
        private string _holidayScheduleDayName;
        private string _summerDesignDayScheduleDayName;
        private string _winterDesignDayScheduleDayName;
        private string _customDay1ScheduleDayName;
        private string _customDay2ScheduleDayName;

        public string Name { get => _name; set => _name = value; }
        public string SundayScheduleDayName { get => _sundayScheduleDayName; set => _sundayScheduleDayName = value; }
        public string MondayScheduleDayName { get => _mondayScheduleDayName; set => _mondayScheduleDayName = value; }
        public string TuesdayScheduleDayName { get => _tuesdayScheduleDayName; set => _tuesdayScheduleDayName = value; }
        public string WednesdayScheduleDayName { get => _wednesdayScheduleDayName; set => _wednesdayScheduleDayName = value; }
        public string ThursdayScheduleDayName { get => _thursdayScheduleDayName; set => _thursdayScheduleDayName = value; }
        public string FridayScheduleDayName { get => _fridayScheduleDayName; set => _fridayScheduleDayName = value; }
        public string SaturdayScheduleDayName { get => _saturdayScheduleDayName; set => _saturdayScheduleDayName = value; }
        public string HolidayScheduleDayName { get => _holidayScheduleDayName; set => _holidayScheduleDayName = value; }
        public string SummerDesignDayScheduleDayName { get => _summerDesignDayScheduleDayName; set => _summerDesignDayScheduleDayName = value; }
        public string WinterDesignDayScheduleDayName { get => _winterDesignDayScheduleDayName; set => _winterDesignDayScheduleDayName = value; }
        public string CustomDay1ScheduleDayName { get => _customDay1ScheduleDayName; set => _customDay1ScheduleDayName = value; }
        public string CustomDay2ScheduleDayName { get => _customDay2ScheduleDayName; set => _customDay2ScheduleDayName = value; }

        public ScheduleWeekDaily() { }

        private static List<ScheduleWeekDaily> list = new List<ScheduleWeekDaily>();

        public static void Add(ScheduleWeekDaily scheduleWeekDaily)
        {
            list.Add(scheduleWeekDaily);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:Week:Daily,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SundayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Sunday Schedule Day Name\n" +
                    ($"  {list[i].MondayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Monday Schedule Day Name\n" +
                    ($"  {list[i].TuesdayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Tuesday Schedule Day Name\n" +
                    ($"  {list[i].WednesdayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Wednesday Schedule Day Name\n" +
                    ($"  {list[i].ThursdayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Thursday Schedule Day Name\n" +
                    ($"  {list[i].FridayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Friday Schedule Day Name\n" +
                    ($"  {list[i].SaturdayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Saturday Schedule Day Name\n" +
                    ($"  {list[i].HolidayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Holiday Schedule Day Name\n" +
                    ($"  {list[i].SummerDesignDayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Summer Design Day Schedule Day Name\n" +
                    ($"  {list[i].WinterDesignDayScheduleDayName}" + ",").PadRight(27, ' ') + " !-Winter Design Day Schedule Day Name\n" +
                    ($"  {list[i].CustomDay1ScheduleDayName}" + ",").PadRight(27, ' ') + " !-Custom Day 1 Schedule Day Name\n" +
                    ($"  {list[i].CustomDay2ScheduleDayName}" + ";").PadRight(27, ' ') + " !-Custom Day 2 Schedule Day Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleWeekDaily.Read())
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
