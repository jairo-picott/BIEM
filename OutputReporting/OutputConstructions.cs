using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{
    public enum DetailsTypeEnum
    {
        Constructions,
        Materials
    }
    public class OutputConstructions
    {
        private DetailsTypeEnum? _detailsType1;
        private DetailsTypeEnum? _detailsType2;

        public DetailsTypeEnum? DetailsType1 { get => _detailsType1; set => _detailsType1 = value; }
        public DetailsTypeEnum? DetailsType2 { get => _detailsType2; set => _detailsType2 = value; }

        public OutputConstructions() { }

        private static List<OutputConstructions> list = new List<OutputConstructions>();

        public static void Add(OutputConstructions outputConstructions)
        {
            list.Add(outputConstructions);

          
        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:Constructions,\n" +
                    ($"  {list[i].DetailsType1}" + ",").PadRight(27, ' ') + " !-Details Type 1\n" +
                    ($"  {list[i].DetailsType2}" + ";").PadRight(27, ' ') + " !-Details Type 2";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputConstructions.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputConstructions.Read())
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
