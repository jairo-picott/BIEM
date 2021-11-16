using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum CalculationTypeEnum
    {
        ClarkAllen,
        Brunt,
        Idso,
        BerdahlMartin,
        ScheduleValue,
        DifferenceScheduleDryBulbValue,
        DifferenceScheduleDewPointValue
    }
    public class WeatherPropertySkyTemperature
    {
        private string _name;
        private CalculationTypeEnum _calculationType = CalculationTypeEnum.ClarkAllen;
        private string _scheduleName;
        private YesNoType _useWeatherFileHorizontalIR = YesNoType.Yes;

        public string Name { get => _name; set => _name = value; }
        public CalculationTypeEnum CalculationType { get => _calculationType; set => _calculationType = value; }
        public string ScheduleName { get => _scheduleName; set => _scheduleName = value; }
        public YesNoType UseWeatherFileHorizontalIR { get => _useWeatherFileHorizontalIR; set => _useWeatherFileHorizontalIR = value; }

        public WeatherPropertySkyTemperature() { }

        private static List<WeatherPropertySkyTemperature> list = new List<WeatherPropertySkyTemperature>();

        public static void Add(WeatherPropertySkyTemperature weatherPropertySkyTemperature)
        {
            list.Add(weatherPropertySkyTemperature);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WeatherProperty:SkyTemperature,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].CalculationType}" + ",").PadRight(27, ' ') + " !-Calculation Type\n" +
                    ($"  {list[i].ScheduleName}" + ",").PadRight(27, ' ') + " !-Schedule Name\n" +
                    ($"  {list[i].UseWeatherFileHorizontalIR}" + ";").PadRight(27, ' ') + " !-Use Weather File Horizontal IR";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WeatherPropertySkyTemperature.Read())
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
