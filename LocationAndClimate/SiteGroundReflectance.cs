using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteGroundReflectance
    {
        private double? _january = 0.2;
        public double? January
        {
            get => _january;
            set
            {
                if (value >=0 && value <=1)
                {
                    _january = value;
                }
            }
        }

        private double? _february = 0.2;
        public double? February
        {
            get => _february;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _february = value;
                }
            }
        }

        private double? _march = 0.2;
        public double? March
        {
            get => _march;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _march = value;
                }
            }
        }

        private double? _april = 0.2;
        public double? April
        {
            get => _april;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _april = value;
                }
            }
        }

        private double? _may = 0.2;
        public double? May
        {
            get => _may;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _may = value;
                }
            }
        }

        private double? _june = 0.2;
        public double? June
        {
            get => _june;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _june = value;
                }
            }
        }

        private double? _july = 0.2;
        public double? July
        {
            get => _july;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _july = value;
                }
            }
        }

        private double? _august = 0.2;
        public double? August
        {
            get => _august;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _august = value;
                }
            }
        }

        private double? _septembre = 0.2;
        public double? September
        {
            get => _septembre;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _septembre = value;
                }
            }
        }

        private double? _october = 0.2;
        public double? October
        {
            get => _october;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _october = value;
                }
            }
        }

        private double? _november = 0.2;
        public double? November
        {
            get => _november;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _november = value;
                }
            }
        }

        private double? _december = 0.2;
        public double? December
        {
            get => _december;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _december = value;
                }
            }
        }

        public SiteGroundReflectance() { }

        private static List<SiteGroundReflectance> list = new List<SiteGroundReflectance>();

        public static void Add(SiteGroundReflectance siteGroundReflectance)
        {
            list.Add(siteGroundReflectance);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundReflectance,\n" +
                    ($"  {list[i].January}" + ",").PadRight(27, ' ') + " !-January\n" +
                    ($"  {list[i].February}" + ",").PadRight(27, ' ') + " !-February {{ deg }}\n" +
                    ($"  {list[i].March}" + ",").PadRight(27, ' ') + " !-March\n" +
                    ($"  {list[i].April}" + ",").PadRight(27, ' ') + " !-April\n" +
                    ($"  {list[i].May}" + ",").PadRight(27, ' ') + " !-May\n" +
                    ($"  {list[i].June}" + ",").PadRight(27, ' ') + " !-June\n" +
                    ($"  {list[i].July}" + ",").PadRight(27, ' ') + " !-July\n" +
                    ($"  {list[i].August}" + ",").PadRight(27, ' ') + " !-August\n" +
                    ($"  {list[i].September}" + ",").PadRight(27, ' ') + " !-September\n" +
                    ($"  {list[i].October}" + ",").PadRight(27, ' ') + " !-October\n" +
                    ($"  {list[i].November}" + ",").PadRight(27, ' ') + " !-November\n" +
                    ($"  {list[i].December}" + ";").PadRight(27, ' ') + " !-December";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundReflectance.Read())
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
