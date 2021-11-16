using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum DayType
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
        CustomDay2
    }

    public enum DryBulbTemperatureRangeModifierType
    {
        DefaultMultipliers,
        MultiplierSchedule,
        DifferenceSchedule,
        TemperatureProfileSchedule
    }

    public enum HumidityConditionType
    {
        WetBulb,
        DewPoint,
        HumidityRatio,
        Enthalpy,
        RelativeHumiditySchedule,
        WetBulbProfileMultiplierSchedule,
        WetBulbProfileDifferenceSchedule,
        WetBulbProfileDefaultMultipliers

    }

    public enum SolarModelIndicatorType
    {
        ASHRAEClearSky,
        ZhangHuang,
        Schedule,
        ASHRAETau,
        ASHRAETau2017
    }

    public enum BeginEnvironmentResetModeType
    {
        FullResetAtBeginEnvironment,
        SuppressAllBeginEnvironmentResets
    }
    //---------------------SizingPeriod:DesignDay
    //DESIGN DAY IS USED BY ENERGY PLUS TO SIMULATE THE CONDITIONS OF 
    //HEATING OR COOLING DEPENDING OF THE DAY TYPE, USE A SHEET WHERE
    //IS SAVED INFORMATION OF MAXIMUM DRY-BULB TEMPERATURE, WIND SPEED,
    //WIND DIRECTION AND OTHER BASIC DATA, THE SHEET FILE IS IN AN SPECIFIC
    //LOCATION HOWEVER IS NECCESARY TO DEFINE A INTERFACE FOR DATA
    //INTRODUCTION FROM THE USER.
    public class SizingPeriodDesignDay
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private int? _month;
        public int? Month
        {
            get => _month;
            set
            {
                if (value<=12 && value >=1)
                {
                    _month = value;
                }
            }
        }

        private int? _dayofMonth;
        public int? DayofMonth
        {
            get => _dayofMonth;
            set
            {
                if (value <= 31 && value >= 1)
                {
                    _dayofMonth = value;
                }
            }
        }

        private DayType? _dayType;
        public DayType? DayType
        {
            get => _dayType;
            set => _dayType = value;
        }

        private double? _maximumDryBulbTemperature;
        public double? MaximumDryBulbTemperature
        {
            get => _maximumDryBulbTemperature;
            set
            {
                if (value <= 70 && value >= -90)
                {
                    _maximumDryBulbTemperature = value;
                }
            }
        }

        private double? _dailyDryBulbTemperatureRange = 0;
        public double? DailyDryBulbTemperatureRange
        {
            get => _dailyDryBulbTemperatureRange;
            set
            {
                if (value >= -0)
                {
                    _dailyDryBulbTemperatureRange = value;
                }
            }
        }

        private DryBulbTemperatureRangeModifierType? _dryBulbTemperatureRangeModifierType;
        public DryBulbTemperatureRangeModifierType? DryBulbTemperatureRangeModifierType
        {
            get => _dryBulbTemperatureRangeModifierType;
            set => _dryBulbTemperatureRangeModifierType = value;
        }

        private string _dryBulbTemperatureRangeModifierDayScheduleName;
        public string DryBulbTemperatureRangeModifierDayScheduleName
        {
            get => _dryBulbTemperatureRangeModifierDayScheduleName;
            set => _dryBulbTemperatureRangeModifierDayScheduleName = value;
        }

        private HumidityConditionType? _humidityConditionType;
        public HumidityConditionType? HumidityConditionType
        {
            get => _humidityConditionType;
            set => _humidityConditionType = value;
        }

        private double? _wetBulborDewPointatMaximumDryBulb;
        public double? WetbulborDewPointatMaximumDryBulb
        {
            get => _wetBulborDewPointatMaximumDryBulb;
            set => _wetBulborDewPointatMaximumDryBulb = value;
        }

        private string _humidityConditionDayScheduleName;
        public string HumidityConditionDayScheduleName
        {
            get => _humidityConditionDayScheduleName;
            set => _humidityConditionDayScheduleName = value;
        }

        private double? _humidityRatioatMaximumDryBulb;
        public double? HumidityRatioatMaximumDryBulb
        {
            get => _humidityRatioatMaximumDryBulb;
            set => _humidityRatioatMaximumDryBulb = value;
        }

        private double? _enthalpyatMaximumDryBulb;
        public double? EnthalphyatMaximumDryBulb
        {
            get => _enthalpyatMaximumDryBulb;
            set => _enthalpyatMaximumDryBulb = value;
        }

        private double? _dailyWetBulbTemperatureRange;
        public double? DailyWetBulbTemperatureRange
        {
            get => _dailyWetBulbTemperatureRange;
            set => _dailyWetBulbTemperatureRange = value;
        }

        private double? _barometricPreassure;
        public double? BarometricPressure
        {
            get => _barometricPreassure;
            set
            {
                if (value <= 120000 && value >= 31000)
                {
                    _barometricPreassure = value;
                }
            }
        }

        private double? _windSpeed;
        public double? WindSpeed
        {
            get => _windSpeed;
            set
            {
                if (value <= 40 && value >= 0)
                {
                    _windSpeed = value;
                }
            }
        }

        private double? _windDirection;
        public double? WindDirection
        {
            get => _windDirection;
            set
            {
                if (value <= 360 && value >= 0)
                {
                    _windDirection = value;
                }
            }
        }

        private YesNoType? _rainIndicator;
        public YesNoType? RainIndicator
        {
            get => _rainIndicator;
            set => _rainIndicator = value;
        }

        private YesNoType? _snowIndicator;
        public YesNoType? SnowIndicator
        {
            get => _snowIndicator;
            set => _snowIndicator = value;
        }

        private YesNoType? _daylightSavingTimeIndicator;
        public YesNoType? DaylightSavingTimeIndicator
        {
            get => _daylightSavingTimeIndicator;
            set => _daylightSavingTimeIndicator = value;
        }

        private SolarModelIndicatorType? _solarModelIndicator;
        public SolarModelIndicatorType? SolarModelIndicator
        {
            get => _solarModelIndicator;
            set => _solarModelIndicator = value;
        }

        private string _beamSolarDayScheduleName;
        public string BeamSolarDayScheduleName
        {
            get => _beamSolarDayScheduleName;
            set => _beamSolarDayScheduleName = value;
        }

        private string _diffuseSolarDayScheduleName;
        public string DiffuseSolarDayScheduleName
        {
            get => _diffuseSolarDayScheduleName;
            set => _diffuseSolarDayScheduleName = value;
        }

        private double? _ASHRAEClearSkyOpticalDepthforBeamIrradiance = 0;
        public double? ASHRAEClearSkyOpticalDepthforBeamIrradiance
        {
            get => _ASHRAEClearSkyOpticalDepthforBeamIrradiance;
            set
            {
                if (value <= 1.2 && value >= 0)
                {
                    _ASHRAEClearSkyOpticalDepthforBeamIrradiance = value;
                }
            }
        }

        private double? _ASHRAEClearSkyOpticalDepthforDiffuseIrradiance = 0;
        public double? ASHRAEClearSkyOpticalDepthforDiffuseIrradiance
        {
            get => _ASHRAEClearSkyOpticalDepthforBeamIrradiance;
            set
            {
                if (value <= 3 && value >= 0)
                {
                    _ASHRAEClearSkyOpticalDepthforBeamIrradiance = value;
                }
            }
        }

        private double? _skyClearness = 0;
        public double? SkyClearness
        {
            get => _skyClearness;
            set
            {
                if (value <= 1.2 && value >= 0)
                {
                    _skyClearness = value;
                }
            }
        }

        private double? _maximumNumberWarmupDays;
        public double? MaximumNumberWarmupDays
        {
            get => _maximumNumberWarmupDays;
            set => _maximumNumberWarmupDays = value;
        }

        private BeginEnvironmentResetModeType? _beginEnvironmentResetMode;
        public BeginEnvironmentResetModeType? BeginEnvironmentResetMode
        {
            get => _beginEnvironmentResetMode;
            set => _beginEnvironmentResetMode = value;
        }

        public SizingPeriodDesignDay() { }

        private static List<SizingPeriodDesignDay> list = new List<SizingPeriodDesignDay>();

        public static void Add(SizingPeriodDesignDay sizingPeriodDesignDay)
        {
            list.Add(sizingPeriodDesignDay);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"SizingPeriod:DesignDay,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Month}" + ",").PadRight(27, ' ') + " !-Month\n" +
                    ($"  {list[i].DayofMonth}" + ",").PadRight(27, ' ') + " !-Day of Month\n" +
                    ($"  {list[i].DayType}" + ",").PadRight(27, ' ') + " !-Day Type\n" +
                    ($"  {list[i].MaximumDryBulbTemperature}" + ",").PadRight(27, ' ') + " !-Maximum Dry Bulb Temperature{{ C }}\n" +
                    ($"  {list[i].DailyDryBulbTemperatureRange}" + ",").PadRight(27, ' ') + " !-Daily Dry Bulb Temperature Range {{ deltaC }}\n" +
                    ($"  {list[i].DryBulbTemperatureRangeModifierType}" + ",").PadRight(27, ' ') + " !-Dry Bulb Temperature Range Modifier Type\n" +
                    ($"  {list[i].DryBulbTemperatureRangeModifierDayScheduleName}" + ",").PadRight(27, ' ') + " !-Dry Bulb Temperature Range Modifier Day Schedule Name\n" +
                    ($"  {list[i].HumidityConditionType}" + ",").PadRight(27, ' ') + " !-Humidity Condition Type\n" +
                    ($"  {list[i].WetbulborDewPointatMaximumDryBulb}" + ",").PadRight(27, ' ') + " !-Wetbulb or Dew Point at Maximum Dry Bulb{{ C }}\n" +
                    ($"  {list[i].HumidityConditionDayScheduleName}" + ",").PadRight(27, ' ') + " !-Humidity Condition Day Schedule Name\n" +
                    ($"  {list[i].HumidityRatioatMaximumDryBulb}" + ",").PadRight(27, ' ') + " !-Humidity Ratio at Maximum Dry Bulb{{ kgWater / kgDryAir }}\n" +
                    ($"  {list[i].EnthalphyatMaximumDryBulb}" + ",").PadRight(27, ' ') + " !-Enthalphy at Maximum Dry Bulb{{ J / kg }}\n" +
                    ($"  {list[i].DailyWetBulbTemperatureRange}" + ",").PadRight(27, ' ') + " !-Daily Wet Bulb Temperature Range{{ deltaC }}\n" +
                    ($"  {list[i].BarometricPressure}" + ",").PadRight(27, ' ') + " !-Barometric Pressure {{ Pa }}\n" +
                    ($"  {list[i].WindSpeed}" + ",").PadRight(27, ' ') + " !-Wind Speed {{ m / s }}\n" +
                    ($"  {list[i].WindDirection}" + ",").PadRight(27, ' ') + " !-Wind Direction {{ deg }}\n" +
                    ($"  {list[i].RainIndicator}" + ",").PadRight(27, ' ') + " !-Rain Indicator\n" +
                    ($"  {list[i].SnowIndicator}" + ",").PadRight(27, ' ') + " !-Snow Indicator\n" +
                    ($"  {list[i].DaylightSavingTimeIndicator}" + ",").PadRight(27, ' ') + " !-Daylight Saving Time Indicator\n" +
                    ($"  {list[i].SolarModelIndicator}" + ",").PadRight(27, ' ') + " !-Solar Model Indicator\n" +
                    ($"  {list[i].BeamSolarDayScheduleName}" + ",").PadRight(27, ' ') + " !-Beam Solar Day Schedule Name\n" +
                    ($"  {list[i].DiffuseSolarDayScheduleName}" + ",").PadRight(27, ' ') + " !-Diffuse Solar Day Schedule Name\n" +
                    ($"  {list[i].ASHRAEClearSkyOpticalDepthforBeamIrradiance}" + ",").PadRight(27, ' ') + " !-ASHRAE Clear Sky Optical Depth for Beam Irradiance\n" +
                    ($"  {list[i].ASHRAEClearSkyOpticalDepthforDiffuseIrradiance}" + ",").PadRight(27, ' ') + " !-ASHRAE Clear Sky Optical Depth for Diffuse Irradiance\n" +
                    ($"  {list[i].SkyClearness}" + ",").PadRight(27, ' ') + " !-Sky Clearness\n" +
                    ($"  {list[i].MaximumNumberWarmupDays}" + ",").PadRight(27, ' ') + " !-Maximum Number Warmup Days\n" +
                    ($"  {list[i].BeginEnvironmentResetMode}" + ";").PadRight(27, ' ') + " !-Begin Environment Reset Mode";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SizingPeriodDesignDay.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(LocationAndClimate.SizingPeriodDesignDay sizingPeriodDesignDay)
        {
            sizingPeriodDesignDay.Name = null;
            sizingPeriodDesignDay.Month = null;
            sizingPeriodDesignDay.DayofMonth = null;
            sizingPeriodDesignDay.DayType = null;
            sizingPeriodDesignDay.MaximumDryBulbTemperature = null;
            sizingPeriodDesignDay.DailyDryBulbTemperatureRange = null;
            sizingPeriodDesignDay.DryBulbTemperatureRangeModifierType = null;
            sizingPeriodDesignDay.DryBulbTemperatureRangeModifierDayScheduleName = null;
            sizingPeriodDesignDay.HumidityConditionType = null;
            sizingPeriodDesignDay.WetbulborDewPointatMaximumDryBulb = null;
            sizingPeriodDesignDay.HumidityConditionDayScheduleName = null;
            sizingPeriodDesignDay.HumidityRatioatMaximumDryBulb = null;
            sizingPeriodDesignDay.EnthalphyatMaximumDryBulb = null;
            sizingPeriodDesignDay.DailyWetBulbTemperatureRange = null;
            sizingPeriodDesignDay.BarometricPressure = null;
            sizingPeriodDesignDay.WindSpeed = null;
            sizingPeriodDesignDay.WindDirection = null;
            sizingPeriodDesignDay.RainIndicator = null;
            sizingPeriodDesignDay.SnowIndicator = null;
            sizingPeriodDesignDay.DaylightSavingTimeIndicator = null;
            sizingPeriodDesignDay.SolarModelIndicator = null;
            sizingPeriodDesignDay.BeamSolarDayScheduleName = null;
            sizingPeriodDesignDay.DiffuseSolarDayScheduleName = null;
            sizingPeriodDesignDay.ASHRAEClearSkyOpticalDepthforBeamIrradiance = null;
            sizingPeriodDesignDay.ASHRAEClearSkyOpticalDepthforDiffuseIrradiance = null;
            sizingPeriodDesignDay.SkyClearness = null;
            sizingPeriodDesignDay.MaximumNumberWarmupDays = null;
            sizingPeriodDesignDay.BeginEnvironmentResetMode = null;
        }
 
    }
}
