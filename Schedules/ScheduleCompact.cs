using System.IO;
using System.Collections.Generic;

namespace BIEM.Schedules
{
    //-----------Schedule Compact
    //SCHEDULE COMPACT DESCRIBE BASIC SCHEDULES FOR EACH ZONE, INCLUDING
    //TEMPERATURE SET POINTS, AND VARIATION OF THE SYSTEM USAGE DURING
    //THE DAY. THIS MUST BE ADJUSTED TO ALLOW THE USER SELECT SPECIFIC
    //SCHEDULE WITH VARIATION ALONG THE DAY.
    public class ScheduleCompact
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

        private string _field1;
        public string Field1
        {
            get => _field1;
            set => _field1 = value;
        }

        private string _field2;
        public string Field2
        {
            get => _field2;
            set => _field2 = value;
        }

        private string _field3;
        public string Field3
        {
            get => _field3;
            set => _field3 = value;
        }

        private string _field4;
        public string Field4
        {
            get => _field4;
            set => _field4 = value;
        }

        private string _field5;
        public string Field5
        {
            get => _field5;
            set => _field5 = value;
        }

        private string _field6;
        public string Field6
        {
            get => _field6;
            set => _field6 = value;
        }

        private string _field7;
        public string Field7
        {
            get => _field7;
            set => _field7 = value;
        }

        private string _field8;
        public string Field8
        {
            get => _field8;
            set => _field8 = value;
        }

        private string _field9;
        public string Field9
        {
            get => _field9;
            set => _field9 = value;
        }

        private string _field10;
        public string Field10
        {
            get => _field10;
            set => _field10 = value;
        }

        public ScheduleCompact() { }

        private static List<ScheduleCompact> list = new List<ScheduleCompact>();

        public static void Add(ScheduleCompact scheduleCompact)
        {
            list.Add(scheduleCompact);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:Compact,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ScheduleTypeLimitsName}" + ",").PadRight(27, ' ') + " !-Schedule Type Limits Name\n" +
                    ($"  {list[i].Field1}" + ",").PadRight(27, ' ') + " !-Field 1\n" +
                    ($"  {list[i].Field2}" + ",").PadRight(27, ' ') + " !-Field 2\n" +
                    ($"  {list[i].Field3}" + ",").PadRight(27, ' ') + " !-Field 3\n" +
                    ($"  {list[i].Field4}" + ",").PadRight(27, ' ') + " !-Field 4\n" +
                    ($"  {list[i].Field5}" + ",").PadRight(27, ' ') + " !-Field 5\n" +
                    ($"  {list[i].Field6}" + ",").PadRight(27, ' ') + " !-Field 6\n" +
                    ($"  {list[i].Field7}" + ",").PadRight(27, ' ') + " !-Field 7\n" +
                    ($"  {list[i].Field8}" + ",").PadRight(27, ' ') + " !-Field 8\n" +
                    ($"  {list[i].Field9}" + ",").PadRight(27, ' ') + " !-Field 9\n" +
                    ($"  {list[i].Field10}" + ";").PadRight(27, ' ') + " !-Field 10";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleCompact.Read())
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
