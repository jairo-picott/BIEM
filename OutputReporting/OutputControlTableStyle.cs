using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{
    public enum ColumnSparatorEnum
    {
        Comma,
        Tab,
        Fixed,
        HTML,
        XML,
        CommaAndHTML,
        CommaAndXML,
        TabAndHTML,
        XMLAndHTML,
        All

    }

    public enum UnitConversionEnum
    {
        None,
        JtoKWH,
        JtoMJ,
        JtoGJ,
        InchPound
    }
    public class OutputControlTableStyle
    {
        private ColumnSparatorEnum _columnSeparator = ColumnSparatorEnum.Comma;
        private UnitConversionEnum _unitConversion = UnitConversionEnum.None;

        public ColumnSparatorEnum ColumnSeparator { get => _columnSeparator; set => _columnSeparator = value; }
        public UnitConversionEnum UnitConversion { get => _unitConversion; set => _unitConversion = value; }

        public OutputControlTableStyle() { }

        private static List<OutputControlTableStyle> list = new List<OutputControlTableStyle>();

        public static void Add(OutputControlTableStyle output)
        {
            list.Add(output);

        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"OutputControl:Table:Style,\n" +
                    ($"  {list[i].ColumnSeparator}" + ",").PadRight(27, ' ') + " !-Column Separator\n" +
                    ($"  {list[i].UnitConversion}" + ";").PadRight(27, ' ') + " !-Unit Conversion";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputControlTableStyle.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputControlTableStyle.Read())
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
