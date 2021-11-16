using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum PeriodSelectionEnum
    {
        SummerExtreme,
        SummerTypical,
        WinterExtreme,
        WinterTypical,
        AutumnTypical,
        SpringTypical,
        WetSeason,
        DrySeason,
        NoDrySeason,
        NoWetSeason,
        TropicalHot,
        TropicalCold,
        NoDrySeasonMax,
        NoDrySeasonMin,
        NoWetSeasonMax,
        NoWetSeasonMin
    }
    public class SizingPeriodWeatherFileConditionType
    {
        private string _name;
        private PeriodSelectionEnum _periodSelection;
        private DayofWeekforStartDayType _dayofWeekforStartDay = DayofWeekforStartDayType.Monday;
        private YesNoType _useWeatherFileDaylightSavingPeriod = YesNoType.Yes;
        private YesNoType _useWeatherFileRainandSnowIndicators = YesNoType.Yes;

        public string Name { get => _name; set => _name = value; }
        public DayofWeekforStartDayType DayofWeekforStartDay { get => _dayofWeekforStartDay; set => _dayofWeekforStartDay = value; }
        public YesNoType UseWeatherFileDaylightSavingPeriod { get => _useWeatherFileDaylightSavingPeriod; set => _useWeatherFileDaylightSavingPeriod = value; }
        public YesNoType UseWeatherFileRainandSnowIndicators { get => _useWeatherFileRainandSnowIndicators; set => _useWeatherFileRainandSnowIndicators = value; }
        public PeriodSelectionEnum PeriodSelection { get => _periodSelection; set => _periodSelection = value; }

        public SizingPeriodWeatherFileConditionType() { }

        private static List<SizingPeriodWeatherFileConditionType> list = new List<SizingPeriodWeatherFileConditionType>();

        public static void Add(SizingPeriodWeatherFileConditionType sizingPeriodWeatherFileConditionType)
        {
            list.Add(sizingPeriodWeatherFileConditionType);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"SizingPeriod:WeatherFileConditionType,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].PeriodSelection}" + ",").PadRight(27, ' ') + " !-Period Selection\n" +
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
                foreach (string line in SizingPeriodWeatherFileConditionType.Read())
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
