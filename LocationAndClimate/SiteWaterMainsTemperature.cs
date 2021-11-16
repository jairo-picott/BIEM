using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum CalculationMethodEnum
    {
        CorrelationFromWeatherFile,
        Correlation,
        Schedule
    }
    public class SiteWaterMainsTemperature
    {
        private CalculationMethodEnum _calculationMethod = CalculationMethodEnum.CorrelationFromWeatherFile;
        private string _temperatureScheduleName;
        private double _annualAverageOutdoorAirTemperature;
        private double _maximumDifferenceInMonthlyAverageOutdoorAirTemperature;

        public CalculationMethodEnum CalculationMethod { get => _calculationMethod; set => _calculationMethod = value; }
        public string TemperatureScheduleName { get => _temperatureScheduleName; set => _temperatureScheduleName = value; }
        public double AnnualAverageOutdoorAirTemperature { get => _annualAverageOutdoorAirTemperature; set => _annualAverageOutdoorAirTemperature = value; }
        public double MaximumDifferenceInMonthlyAverageOutdoorAirTemperature 
        { 
            get => _maximumDifferenceInMonthlyAverageOutdoorAirTemperature; 
            set
            {
                if (value >= 0)
                {
                    _maximumDifferenceInMonthlyAverageOutdoorAirTemperature = value;
                }
            }
        }

        public SiteWaterMainsTemperature() { }

        private static List<SiteWaterMainsTemperature> list = new List<SiteWaterMainsTemperature>();

        public static void Add(SiteWaterMainsTemperature siteWaterMainsTemperature)
        {
            list.Add(siteWaterMainsTemperature);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:WaterMainsTemperature,\n" +
                    ($"  {list[i].CalculationMethod}" + ",").PadRight(27, ' ') + " !-Calculation Method\n" +
                    ($"  {list[i].TemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Temperature ScheduleName\n" +
                    ($"  {list[i].AnnualAverageOutdoorAirTemperature}" + ",").PadRight(27, ' ') + " !-Annual Average Outdoor Air Temperature {{ C }}\n" +
                    ($"  {list[i].MaximumDifferenceInMonthlyAverageOutdoorAirTemperature}" + ";").PadRight(27, ' ') + " !-Maximum Difference In Monthly Average Outdoor Air Temperature {{ deltaC }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteWaterMainsTemperature.Read())
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
