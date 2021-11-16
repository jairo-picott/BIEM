using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteGroundReflectanceSnowModifier
    {
        private double _groundReflectanceSolarModifier = 1;
        private double _daylightingGroundReflectanceSolarModifier = 1;

        public double GroundReflectanceSolarModifier 
        { 
            get => _groundReflectanceSolarModifier; 
            set
            {
                if (value >= 0)
                {
                    _groundReflectanceSolarModifier = value;
                }
            }
        }
        public double DaylightingGroundReflectanceSolarModifier 
        {
            get => _daylightingGroundReflectanceSolarModifier;
            set
            {
                if (value >= 0)
                {
                    _daylightingGroundReflectanceSolarModifier = value;
                }
            }
        }

        public SiteGroundReflectanceSnowModifier() { }

        private static List<SiteGroundReflectanceSnowModifier> list = new List<SiteGroundReflectanceSnowModifier>();

        public static void Add(SiteGroundReflectanceSnowModifier siteGroundReflectanceSnowModifier)
        {
            list.Add(siteGroundReflectanceSnowModifier);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundReflectance:SnowModifier,\n" +
                    ($"  {list[i].GroundReflectanceSolarModifier}" + ",").PadRight(27, ' ') + " !-Ground Reflectance Solar Modifier\n" +
                    ($"  {list[i].DaylightingGroundReflectanceSolarModifier}" + ";").PadRight(27, ' ') + " !-Day lighting Ground Reflectance Solar Modifier";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundReflectanceSnowModifier.Read())
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
