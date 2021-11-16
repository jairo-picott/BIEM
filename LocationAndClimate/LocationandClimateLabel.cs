using System.IO;

namespace BIEM.LocationAndClimate
{
    public class LocationandClimateLabel
    {
        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                
                sw.WriteLine("!!----ALL THE OBJECTS OF LOCATION AND CLIMATE----");

            }

        }
    }
}
