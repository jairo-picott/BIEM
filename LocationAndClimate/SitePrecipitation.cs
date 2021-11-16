using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BIEM.LocationAndClimate
{
    public enum PrecipitationModelTypeEnum
    {
        ScheduleAndDesignLevel
    }
    public class SitePrecipitation
    {
        private PrecipitationModelTypeEnum _precipitationModelType;
        private double _designLevelforTotalAnnualPrecipitation;
        private string _precipitationRatesScheduleName;
        private double _averageTotalAnnualPrecipitation;

        public PrecipitationModelTypeEnum PrecipitationModelType { get => _precipitationModelType; set => _precipitationModelType = value; }
        public double DesignLevelforTotalAnnualPrecipitation { get => _designLevelforTotalAnnualPrecipitation; set => _designLevelforTotalAnnualPrecipitation = value; }
        public string PrecipitationRatesScheduleName { get => _precipitationRatesScheduleName; set => _precipitationRatesScheduleName = value; }
        public double AverageTotalAnnualPrecipitation 
        { 
            get => _averageTotalAnnualPrecipitation; 
            set
            {
                if (value >= 0)
                {
                    _averageTotalAnnualPrecipitation = value;
                }
            }
        }

        public SitePrecipitation() { }

        private static List<SitePrecipitation> list = new List<SitePrecipitation>();

        public static void Add(SitePrecipitation sitePrecipitation)
        {
            list.Add(sitePrecipitation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:Precipitation,\n" +
                    ($"  {list[i].PrecipitationModelType}" + ",").PadRight(27, ' ') + " !-Precipitation Model Type\n" +
                    ($"  {list[i].DesignLevelforTotalAnnualPrecipitation}" + ",").PadRight(27, ' ') + " !-Design Level for Total Annual Precipitation {{ m/yr }}\n" +
                    ($"  {list[i].PrecipitationRatesScheduleName}" + ",").PadRight(27, ' ') + " !-Precipitation Rates Schedule Name\n" +
                    ($"  {list[i].AverageTotalAnnualPrecipitation}" + ";").PadRight(27, ' ') + " !-Average Total Annual Precipitation {{ m/yr }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SitePrecipitation.Read())
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
