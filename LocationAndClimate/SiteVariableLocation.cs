using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteVariableLocation
    {
        private string _name;
        public string Name { get => _name; set => _name = value; }

        private string _buildingLocationLatitudeSchedule;
        public string BuildingLocationLatitudeSchedule { get => _buildingLocationLatitudeSchedule; set => _buildingLocationLatitudeSchedule = value; }
        public string BuildingLocationLongitudeSchedule { get => _buildingLocationLongitudeSchedule; set => _buildingLocationLongitudeSchedule = value; }
        public string BuildingLocationOrientationSchedule { get => _buildingLocationOrientationSchedule; set => _buildingLocationOrientationSchedule = value; }

        private string _buildingLocationLongitudeSchedule;

        private string _buildingLocationOrientationSchedule;

        public SiteVariableLocation() { }

        private static List<SiteVariableLocation> list = new List<SiteVariableLocation>();

        public static void Add(SiteVariableLocation siteVariableLocation)
        {
            list.Add(siteVariableLocation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:VariableLocation,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].BuildingLocationLatitudeSchedule}" + ",").PadRight(27, ' ') + " !-Building Location Latitude Schedule\n" +
                    ($"  {list[i].BuildingLocationLongitudeSchedule}" + ",").PadRight(27, ' ') + " !-Building Location Longitude Schedule\n" +
                    ($"  {list[i].BuildingLocationOrientationSchedule}" + ";").PadRight(27, ' ') + " !-Building Location Orientation Schedule";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteVariableLocation.Read())
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
