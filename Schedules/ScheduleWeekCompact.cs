using System.Collections.Generic;
using System.IO;

namespace BIEM.Schedules
{
    public enum DayTypeListEnum
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Holiday,
        SummerDesignDay,
        WinterDesignDay,
        CustomDay1,
        CustomDay2,
        AllDays,
        AllOtherDays,
        Weekdays,
        Weekends
    }
    public class ScheduleWeekCompact
    {
        private string _name;
        private DayTypeListEnum _dayTypeList1;
        private string _scheduleDayName1;
        private DayTypeListEnum _dayTypeList2;
        private string _scheduleDayName2;
        private DayTypeListEnum _dayTypeList3;
        private string _scheduleDayName3;
        private DayTypeListEnum _dayTypeList4;
        private string _scheduleDayName4;
        private DayTypeListEnum _dayTypeList5;
        private string _scheduleDayName5;

        public string Name { get => _name; set => _name = value; }
        public DayTypeListEnum DayTypeList1 { get => _dayTypeList1; set => _dayTypeList1 = value; }
        public string ScheduleDayName1 { get => _scheduleDayName1; set => _scheduleDayName1 = value; }
        public DayTypeListEnum DayTypeList2 { get => _dayTypeList2; set => _dayTypeList2 = value; }
        public string ScheduleDayName2 { get => _scheduleDayName2; set => _scheduleDayName2 = value; }
        public DayTypeListEnum DayTypeList3 { get => _dayTypeList3; set => _dayTypeList3 = value; }
        public string ScheduleDayName3 { get => _scheduleDayName3; set => _scheduleDayName3 = value; }
        public DayTypeListEnum DayTypeList4 { get => _dayTypeList4; set => _dayTypeList4 = value; }
        public string ScheduleDayName4 { get => _scheduleDayName4; set => _scheduleDayName4 = value; }
        public DayTypeListEnum DayTypeList5 { get => _dayTypeList5; set => _dayTypeList5 = value; }
        public string ScheduleDayName5 { get => _scheduleDayName5; set => _scheduleDayName5 = value; }

        public ScheduleWeekCompact() { }

        private static List<ScheduleWeekCompact> list = new List<ScheduleWeekCompact>();

        public static void Add(ScheduleWeekCompact scheduleWeekCompact)
        {
            list.Add(scheduleWeekCompact);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:Week:Compact,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].DayTypeList1}" + ",").PadRight(27, ' ') + " !-Day Type List 1\n" +
                    ($"  {list[i].ScheduleDayName1}" + ",").PadRight(27, ' ') + " !-Schedule Day Name 1\n" +
                    ($"  {list[i].DayTypeList2}" + ",").PadRight(27, ' ') + " !-Day Type List 2\n" +
                    ($"  {list[i].ScheduleDayName2}" + ",").PadRight(27, ' ') + " !-Schedule Day Name 2\n" +
                    ($"  {list[i].DayTypeList3}" + ",").PadRight(27, ' ') + " !-Day Type List 3\n" +
                    ($"  {list[i].ScheduleDayName3}" + ",").PadRight(27, ' ') + " !-Schedule Day Name 3\n" +
                    ($"  {list[i].DayTypeList4}" + ",").PadRight(27, ' ') + " !-Day Type List 4\n" +
                    ($"  {list[i].ScheduleDayName4}" + ",").PadRight(27, ' ') + " !-Schedule Day Name 4\n" +
                    ($"  {list[i].DayTypeList5}" + ",").PadRight(27, ' ') + " !-Day Type List 5\n" +
                    ($"  {list[i].ScheduleDayName5}" + ";").PadRight(27, ' ') + " !-Schedule Day Name 5";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleWeekCompact.Read())
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
