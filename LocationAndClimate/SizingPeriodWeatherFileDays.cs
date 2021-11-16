using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{

    public class SizingPeriodWeatherFileDays
    {
        private string _name;
        private int _beginMonth;
        private int _beginDayofMonth;
        private int _endMonth;
        private int _endDayofMonth;
        private DayofWeekforStartDayType _dayofWeekforStartDay = DayofWeekforStartDayType.Monday;
        private YesNoType _useWeatherFileDaylightSavingPeriod = YesNoType.Yes;
        private YesNoType _useWeatherFileRainandSnowIndicators = YesNoType.Yes;

        public string Name { get => _name; set => _name = value; }
        public int BeginMonth 
        { 
            get => _beginMonth; 
            set
            {
                if (value>=1 && value <= 12)
                {
                    _beginMonth = value;
                }
            }
        }

        public int BeginDayofMonth
        {
            get => _beginDayofMonth;
            set
            {
                if (value >= 1 && value <= 31)
                {
                    _beginDayofMonth = value;
                }
            }
        }

        public int EndMonth
        {
            get => _endMonth;
            set
            {
                if (value >= 1 && value <= 12)
                {
                    _endMonth = value;
                }
            }
        }
        public int EndDayofMonth
        {
            get => _endDayofMonth;
            set
            {
                if (value >= 1 && value <= 31)
                {
                    _endDayofMonth = value;
                }
            }
        }
        public DayofWeekforStartDayType DayofWeekforStartDay { get => _dayofWeekforStartDay; set => _dayofWeekforStartDay = value; }
        public YesNoType UseWeatherFileDaylightSavingPeriod { get => _useWeatherFileDaylightSavingPeriod; set => _useWeatherFileDaylightSavingPeriod = value; }
        public YesNoType UseWeatherFileRainandSnowIndicators { get => _useWeatherFileRainandSnowIndicators; set => _useWeatherFileRainandSnowIndicators = value; }

        public SizingPeriodWeatherFileDays() { }

        private static List<SizingPeriodWeatherFileDays> list = new List<SizingPeriodWeatherFileDays>();

        public static void Add(SizingPeriodWeatherFileDays sizingPeriodWeatherFileDays)
        {
            list.Add(sizingPeriodWeatherFileDays);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"SizingPeriod:WeatherFileDays,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].BeginMonth}" + ",").PadRight(27, ' ') + " !-Begin Month\n" +
                    ($"  {list[i].BeginDayofMonth}" + ",").PadRight(27, ' ') + " !-Begin Day of Month\n" +
                    ($"  {list[i].EndMonth}" + ",").PadRight(27, ' ') + " !-End Month\n" +
                    ($"  {list[i].EndDayofMonth}" + ",").PadRight(27, ' ') + " !-End Day of Month\n" +
                    ($"  {list[i].DayofWeekforStartDay}" + ",").PadRight(27, ' ') + " !-Day of Week for Start Day\n" +
                    ($"  {list[i].UseWeatherFileDaylightSavingPeriod}" + ",").PadRight(27, ' ') + " !-Use Weather File Daylight Saving Period\n" +
                    ($"  {list[i].UseWeatherFileRainandSnowIndicators}" + ";").PadRight(27, ' ') + " !-Use Weather File Rain and Snow Indicators";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SizingPeriodWeatherFileDays.Read())
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
