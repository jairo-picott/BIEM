using System.Collections.Generic;
using System.IO;

namespace BIEM.Schedules
{
    public class ScheduleFileShading
    {
        private string _fileName;

        public string FileName { get => _fileName; set => _fileName = value; }

        public ScheduleFileShading() { }

        private static List<ScheduleFileShading> list = new List<ScheduleFileShading>();

        public static void Add(ScheduleFileShading scheduleFileShading)
        {
            list.Add(scheduleFileShading);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Schedule:File:Shading,\n" +
                    ($"  {list[i].FileName}" + ";").PadRight(27, ' ') + " !-File Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleFileShading.Read())
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
