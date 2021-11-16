using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    //------------Site:Location
    //THIS CLASS USE IFC BUILDING AND IFC SITE TO DESCRIBE THE LOCATION OF
    //THE BUILDING, LATITUDE, LONGITUDE AND ELEVATION CAN BE OBTAIN FROM
    //PROJECT COORDINATE IN THE IFC FILE. STILL IS NECCSARY TO SET THE
    //TIME ZONE.
    public class SiteLocation
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private double? _latitude = 0;
        public double? Latitude
        {
            get => _latitude;
            set
            {
                if (value>=-90 && value<=90)
                {
                    _latitude = value;
                }
            }
        }

        private double? _longitude = 0;
        public double? Longitude
        {
            get => _longitude;
            set
            {
                if (value >= -180 && value <= 180)
                {
                    _longitude = value;
                }
            }
        }

        private double? _timeZone = 0;
        public double? TimeZone
        {
            get => _timeZone;
            set
            {
                if (value >= -12 && value <= 14)
                {
                    _timeZone = value;
                }
            }
        }

        private double? _elevation = 0;
        public double? Elevation
        {
            get => _elevation;
            set
            {
                if (value >= -300 && value <= 8900)
                {
                    _elevation = value;
                }
            }
        }

        public SiteLocation() { }

        private static List<SiteLocation> list = new List<SiteLocation>();

        public static void Add(SiteLocation siteLocation)
        {
            list.Add(siteLocation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:Location,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Latitude}" + ",").PadRight(27, ' ') + " !-Latitude {{ deg }}\n" +
                    ($"  {list[i].Longitude}" + ",").PadRight(27, ' ') + " !-Longitude {{ deg }}\n" +
                    ($"  {list[i].TimeZone}" + ",").PadRight(27, ' ') + " !-TimeZone {{ hr }}\n" +
                    ($"  {list[i].Elevation}" + ";").PadRight(27, ' ') + " !-Elevation {{ m }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteLocation.Read())
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
