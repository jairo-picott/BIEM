using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{
    public enum KeyFieldEnum
    {
        Hourly,
        Timestep
    }
    public class OutputSchedules
    {
        private KeyFieldEnum? _keyField;

        public KeyFieldEnum? KeyField { get => _keyField; set => _keyField = value; }

        public OutputSchedules() { }

        private static List<OutputSchedules> list = new List<OutputSchedules>();

        public static void Add(OutputSchedules outputSchedules)
        {
            list.Add(outputSchedules);

            Errors er = new Errors();
            if (outputSchedules.KeyField == null)
            {
                er.Class = "Output:Schedules";
                er.Field = "KeyField";
                Errors.Add(er);
            }
        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:Schedules,\n" +
                    ($"  {list[i].KeyField}" + ";").PadRight(27, ' ') + " !-Key Field";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputSchedules.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputSchedules.Read())
            {
                Console.WriteLine(line);
            }


        }
        public static void Clear()
        {
            list.Clear();
        }
    }
}
