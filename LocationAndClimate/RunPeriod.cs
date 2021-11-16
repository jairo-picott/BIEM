
using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum DayofWeekforStartDayType
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }

    public enum YesNoType
    {
        Yes,
        No
    }
    //---------------------RunPeriod
    //GET FROM USER THE PERIOD THAT MUST BE SIMULATED, HAVING AS DEFAULT
    //365 DAYS, STARTING FROM THE FIRST DA OF THE FIRST MONTH, FINISHING
    //THE LAST DAY OF THE LAST MONTH.
    public class RunPeriod
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private int? _beginMonth;
        public int? BeginMonth
        {
            get => _beginMonth;
            set
            {
                if (value>=1 && value<=12)
                {
                    _beginMonth = value;
                }
            }
        }

        private int? _beignDayofMonth;
        public int? BeginDayofMonth
        {
            get => _beignDayofMonth;
            set
            {
                if (value >= 1 && value <= 31)
                {
                    _beignDayofMonth = value;
                }
            }
        }

        private int? _beginYear;
        public int? BeginYear
        {
            get => _beginYear;
            set => _beginYear = value;
        }

        private int? _endMonth;
        public int? EndMonth
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

        private int? _endDayofMonth;
        public int? EndDayofMonth
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

        private int? _endYear;
        public int? EndYear
        {
            get => _endYear;
            set => _endYear = value;
        }

        private DayofWeekforStartDayType? _dayofWeekforStartDay;
        public DayofWeekforStartDayType? DayofWeekforStartDay
        {
            get => _dayofWeekforStartDay;
            set => _dayofWeekforStartDay = value;
        }

        private YesNoType? _useWeatherFileHolidaysandSpecialDays;
        public YesNoType? UseWeatherFileHolidaysandSpecialDays
        {
            get => _useWeatherFileHolidaysandSpecialDays;
            set => _useWeatherFileHolidaysandSpecialDays = value;
        }

        private YesNoType? _useWeatherFileDaylightSavingPeriod;
        public YesNoType? UseWeatherFileDaylightSavingPeriod
        {
            get => _useWeatherFileDaylightSavingPeriod;
            set => _useWeatherFileDaylightSavingPeriod = value;
        }

        private YesNoType? _applyWeekendHolidayRule;
        public YesNoType? ApplyWeekendHolidayRule
        {
            get => _applyWeekendHolidayRule;
            set => _applyWeekendHolidayRule = value;
        }

        private YesNoType? _useWeatherFileRainIndicators;
        public YesNoType? UseWeatherFileRainIndicators
        {
            get => _useWeatherFileRainIndicators;
            set => _useWeatherFileRainIndicators = value;
        }

        private YesNoType? _useWeatherFileSnowIndicators;
        public YesNoType? UseWeatherFileSnowIndicators
        {
            get => _useWeatherFileSnowIndicators;
            set => _useWeatherFileSnowIndicators = value;
        }

        private YesNoType? _treatWeatherasActual;
        public YesNoType? TreatWeatherasActual
        {
            get => _treatWeatherasActual;
            set => _treatWeatherasActual = value;
        }

        public RunPeriod() { }

        private static List<RunPeriod> list = new List<RunPeriod>();

        public static void Add(RunPeriod runPeriod)
        {
            list.Add(runPeriod);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"RunPeriod,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].BeginMonth}" + ",").PadRight(27, ' ') + " !-Zone or ZoneList Name\n" +
                    ($"  {list[i].BeginDayofMonth}" + ",").PadRight(27, ' ') + " !-Schedule Name\n" +
                    ($"  {list[i].BeginYear}" + ",").PadRight(27, ' ') + " !-Design Level Calculation Method\n" +
                    ($"  {list[i].EndMonth}" + ",").PadRight(27, ' ') + " !-Design Level{{ W }}\n" +
                    ($"  {list[i].EndDayofMonth}" + ",").PadRight(27, ' ') + " !-Watts per Zone Floor Area {{ W / m2}}\n" +
                    ($"  {list[i].EndYear}" + ",").PadRight(27, ' ') + " !-Watts per Person {{ W / person}}\n" +
                    ($"  {list[i].DayofWeekforStartDay}" + ",").PadRight(27, ' ') + " !-Fraction Latent\n" +
                    ($"  {list[i].UseWeatherFileHolidaysandSpecialDays}" + ",").PadRight(27, ' ') + " !-Fraction Radiant\n" +
                    ($"  {list[i].UseWeatherFileDaylightSavingPeriod}" + ",").PadRight(27, ' ') + " !-Fraction Lost\n" +
                    ($"  {list[i].ApplyWeekendHolidayRule}" + ",").PadRight(27, ' ') + " !-Fraction Latent\n" +
                    ($"  {list[i].UseWeatherFileRainIndicators}" + ",").PadRight(27, ' ') + " !-Fraction Radiant\n" +
                    ($"  {list[i].UseWeatherFileSnowIndicators}" + ",").PadRight(27, ' ') + " !-Fraction Lost\n" +
                    ($"  {list[i].TreatWeatherasActual}" + ";").PadRight(27, ' ') + " !-End Use Subcategory";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in RunPeriod.Read())
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
