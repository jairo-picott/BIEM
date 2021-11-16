using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum SpectrumDataTypeEnum
    {
        Solar,
        Visible
    }
    public class SiteSpectrumData
    {
        private string _name;
        private SpectrumDataTypeEnum _spectrumDataType;
        private double _wavelenght;
        private double _spectrum;
        private double _wavelenght2;
        private double _spectrum2;
        private double _wavelenght3;
        private double _spectrum3;
        private double _n7;
        private double _n8;
        private double _n9;
        private double _n10;

        public string Name { get => _name; set => _name = value; }
        public SpectrumDataTypeEnum SpectrumDataType { get => _spectrumDataType; set => _spectrumDataType = value; }
        public double Wavelenght { get => _wavelenght; set => _wavelenght = value; }
        public double Spectrum { get => _spectrum; set => _spectrum = value; }
        public double Wavelenght2 { get => _wavelenght2; set => _wavelenght2 = value; }
        public double Spectrum2 { get => _spectrum2; set => _spectrum2 = value; }
        public double Wavelenght3 { get => _wavelenght3; set => _wavelenght3 = value; }
        public double Spectrum3 { get => _spectrum3; set => _spectrum3 = value; }
        public double N7 { get => _n7; set => _n7 = value; }
        public double N8 { get => _n8; set => _n8 = value; }
        public double N9 { get => _n9; set => _n9 = value; }
        public double N10 { get => _n10; set => _n10 = value; }

        public SiteSpectrumData() { }

        private static List<SiteSpectrumData> list = new List<SiteSpectrumData>();

        public static void Add(SiteSpectrumData siteSpectrumData)
        {
            list.Add(siteSpectrumData);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:SpectrumData,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SpectrumDataType}" + ",").PadRight(27, ' ') + " !-Spectrum Data Type\n" +
                    ($"  {list[i].Wavelenght}" + ",").PadRight(27, ' ') + " !-Wavelenght {{ micron }}\n" +
                    ($"  {list[i].Spectrum}" + ",").PadRight(27, ' ') + " !-Spectrum\n" +
                    ($"  {list[i].Wavelenght2}" + ",").PadRight(27, ' ') + " !-Wavelenght 2 {{ micron }}\n" +
                    ($"  {list[i].Spectrum2}" + ",").PadRight(27, ' ') + " !-Spectrum 2\n" +
                    ($"  {list[i].Wavelenght3}" + ",").PadRight(27, ' ') + " !-Wavelenght 3 {{ micron }}\n" +
                    ($"  {list[i].Spectrum3}" + ",").PadRight(27, ' ') + " !-Spectrum 3\n" +
                    ($"  {list[i].N7}" + ",").PadRight(27, ' ') + " !-N7\n" +
                    ($"  {list[i].N8}" + ",").PadRight(27, ' ') + " !-N8\n" +
                    ($"  {list[i].N9}" + ",").PadRight(27, ' ') + " !-N9\n" +
                    ($"  {list[i].N10}" + ";").PadRight(27, ' ') + " !-N10";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteSpectrumData.Read())
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
