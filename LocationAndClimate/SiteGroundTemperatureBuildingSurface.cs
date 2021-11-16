using System.IO;
using System.Collections.Generic;

namespace BIEM.LocationAndClimate
{
    //----------------------SIte:GroundTemperatur:BuildingSurface
    //THIS CLASS DEFINED GROUND TEMPERATURE ALONG THE YEAR, BY NOW HAVE
    //FIXED VALUES, HOWEVER IS NECCESARY AS WELL AS IN DESIGN DAYS,
    //CREATE AN INTERFACE FOR USER INTERACTION.
    public class SiteGroundTemperatureBuildingSurface
    {
        private double? _january = 18;
        public double? January
        {
            get => _january;
            set => _january = value;
        }

        private double? _february = 18;
        public double? February
        {
            get => _february;
            set => _february = value;
        }

        private double? _march = 18;
        public double? March
        {
            get => _march;
            set => _march = value;
        }

        private double? _april = 18;
        public double? April
        {
            get => _april;
            set => _april = value;
        }

        private double? _may = 18;
        public double? May
        {
            get => _may;
            set => _may = value;
        }

        private double? _june = 18;
        public double? June
        {
            get => _june;
            set => _june = value;
        }

        private double? _july = 18;
        public double? July
        {
            get => _july;
            set => _july = value;
        }

        private double? _august = 18;
        public double? August
        {
            get => _august;
            set => _august = value;
        }

        private double? _septembre = 18;
        public double? September
        {
            get => _septembre;
            set => _septembre = value;
        }

        private double? _october = 18;
        public double? October
        {
            get => _october;
            set => _october = value;
        }

        private double? _november = 18;
        public double? November
        {
            get => _november;
            set => _november = value;
        }

        private double? _december = 18;
        public double? December
        {
            get => _december;
            set => _december = value;
        }

        public SiteGroundTemperatureBuildingSurface() { }
        private static List<SiteGroundTemperatureBuildingSurface> list = new List<SiteGroundTemperatureBuildingSurface>();

        public static void Add(SiteGroundTemperatureBuildingSurface siteGroundTemperatureBuildingSurface)
        {
            list.Add(siteGroundTemperatureBuildingSurface);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundTemperature:BuildingSurface,\n" +
                    ($"  {list[i].January}" + ",").PadRight(27, ' ') + " !-January{{ C }}\n" +
                    ($"  {list[i].February}" + ",").PadRight(27, ' ') + " !-February{{ C }}\n" +
                    ($"  {list[i].March}" + ",").PadRight(27, ' ') + " !-March{{ C }}\n" +
                    ($"  {list[i].April}" + ",").PadRight(27, ' ') + " !-April{{ C }}\n" +
                    ($"  {list[i].May}" + ",").PadRight(27, ' ') + " !-May{{ C }}\n" +
                    ($"  {list[i].June}" + ",").PadRight(27, ' ') + " !-June{{ C }}\n" +
                    ($"  {list[i].July}" + ",").PadRight(27, ' ') + " !-July{{ C }}\n" +
                    ($"  {list[i].August}" + ",").PadRight(27, ' ') + " !-August{{ C }}\n" +
                    ($"  {list[i].September}" + ",").PadRight(27, ' ') + " !-September{{ C }}\n" +
                    ($"  {list[i].October}" + ",").PadRight(27, ' ') + " !-October{{ C }}\n" +
                    ($"  {list[i].November}" + ",").PadRight(27, ' ') + " !-November{{ C }}\n" +
                    ($"  {list[i].December}" + ";").PadRight(27, ' ') + " !-December{{ C }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundTemperatureBuildingSurface.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(LocationAndClimate.SiteGroundTemperatureBuildingSurface siteGroundTemperatureBuildingSurface)
        {
            siteGroundTemperatureBuildingSurface.January = null;
            siteGroundTemperatureBuildingSurface.February = null;
            siteGroundTemperatureBuildingSurface.March = null;
            siteGroundTemperatureBuildingSurface.April = null;
            siteGroundTemperatureBuildingSurface.May = null;
            siteGroundTemperatureBuildingSurface.June = null;
            siteGroundTemperatureBuildingSurface.July = null;
            siteGroundTemperatureBuildingSurface.August = null;
            siteGroundTemperatureBuildingSurface.September = null;
            siteGroundTemperatureBuildingSurface.October = null;
            siteGroundTemperatureBuildingSurface.November = null;
            siteGroundTemperatureBuildingSurface.December = null;
        }

    }
}
