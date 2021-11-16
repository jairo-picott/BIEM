using System.IO;
using System.Collections.Generic;

namespace BIEM.OutputReporting
{
    public enum KeyFieldType
    {
        regular,
        IDF
    }

    public enum SortOptionType
    {
        Name,
        Unsorted
    }
    public class OutputVariableDictionary
    {

        private KeyFieldType? _keyField;
        public KeyFieldType? KeyField
        {
            get => _keyField;
            set => _keyField = value;
        }

        private SortOptionType? _sortOption;
        public SortOptionType? SortOption
        {
            get => _sortOption;
            set => _sortOption = value;
        }

        public OutputVariableDictionary() { }

        private static List<OutputVariableDictionary> list = new List<OutputVariableDictionary>();

        public static void Add(OutputVariableDictionary outputVariableDictionary)
        {
            list.Add(outputVariableDictionary);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:VariableDictionary,\n" +
                    ($"  {list[i].KeyField}" + ",").PadRight(27, ' ') + " !-Key Field\n" +
                    ($"  {list[i].SortOption}" + ";").PadRight(27, ' ') + " !-Sort Option";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputVariableDictionary.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(OutputReporting.OutputVariableDictionary outputVariableDictionary)
        {
            outputVariableDictionary.KeyField = null;
            outputVariableDictionary.SortOption = null;
        }

    }

}
