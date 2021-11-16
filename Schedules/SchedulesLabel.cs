using System.IO;

namespace BIEM.Schedules
{
    public class SchedulesLabel
    {
        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {

                sw.WriteLine("!!----ALL THE OBJECTS OF SCHEDULES----");

            }

        }
    }
}
