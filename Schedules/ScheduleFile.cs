using System.Collections.Generic;
using System.IO;

namespace BIEM.Schedules
{
    public enum ColumnSeparatorEnum
    {
        Comma,
        Tab,
        Space,
        Semicolon
    }
    
    public class ScheduleFile
    {
        private string _name;
        private string _scheduleTypeLimitsName;
        private string _fileName;
        private double _columnNumber;
        private double _rowstoSkipatTop;
        private double _numberofHoursofData = 8760;
        private ColumnSeparatorEnum _columnSeparator = ColumnSeparatorEnum.Comma;
        private InterpolateTimestepEnum _interpolatetoTimestep = InterpolateTimestepEnum.No;
        private double _minutesperItem;

        public string Name { get => _name; set => _name = value; }
        public string ScheduleTypeLimitsName { get => _scheduleTypeLimitsName; set => _scheduleTypeLimitsName = value; }
        public string FileName { get => _fileName; set => _fileName = value; }
        public double ColumnNumber 
        { 
            get => _columnNumber; 
            set
            {
                if (value >= 1)
                {
                    _columnNumber = value;
                }
            }
        }
        public double RowstoSkipatTop 
        { 
            get => _rowstoSkipatTop;
            set
            {
                if (value >= 0)
                {
                    _rowstoSkipatTop= value;
                }
            }
        }
        public double NumberofHoursofData 
        { 
            get => _numberofHoursofData;
            set
            {
                if (value >= 8760 && value <= 8784)
                {
                    _numberofHoursofData= value;
                }
            }
        }
        public ColumnSeparatorEnum ColumnSeparator { get => _columnSeparator; set => _columnSeparator = value; }
        public InterpolateTimestepEnum InterpolatetoTimestep { get => _interpolatetoTimestep; set => _interpolatetoTimestep = value; }
        public double MinutesperItem 
        { 
            get => _minutesperItem;
            set
            {
                if (value >= 1 && value <= 60)
                {
                    _minutesperItem= value;
                }
            }
        }

        public ScheduleFile() { }

        private static List<ScheduleFile> list = new List<ScheduleFile>();

        public static void Add(ScheduleFile scheduleFile)
        {
            list.Add(scheduleFile);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:File,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ScheduleTypeLimitsName}" + ",").PadRight(27, ' ') + " !-Schedule Type Limits Name\n" +
                    ($"  {list[i].FileName}" + ",").PadRight(27, ' ') + " !-File Name\n" +
                    ($"  {list[i].ColumnNumber}" + ",").PadRight(27, ' ') + " !-Column Number\n" +
                    ($"  {list[i].RowstoSkipatTop}" + ",").PadRight(27, ' ') + " !-Rows to Skip at Top\n" +
                    ($"  {list[i].NumberofHoursofData}" + ",").PadRight(27, ' ') + " !-Number of Hours of Data\n" +
                    ($"  {list[i].ColumnSeparator}" + ",").PadRight(27, ' ') + " !-Column Separator\n" +
                    ($"  {list[i].InterpolatetoTimestep}" + ",").PadRight(27, ' ') + " !-Interpolate to Timestep\n" +
                    ($"  {list[i].MinutesperItem}" + ";").PadRight(27, ' ') + " !-Minutes per Item";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleFile.Read())
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
