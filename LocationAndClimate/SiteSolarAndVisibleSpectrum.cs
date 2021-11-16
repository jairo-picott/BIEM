using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum SpectrumDataMethodEnum
    {
        Default,
        UserDefined
    }
    public class SiteSolarAndVisibleSpectrum
    {
        private string _name;
        private SpectrumDataMethodEnum _spectrumDataMethod = SpectrumDataMethodEnum.Default;
        private string _solarSpectrumDataObjectName;
        private string _visibleSpectrumDataObjectName;

        public string Name { get => _name; set => _name = value; }
        public SpectrumDataMethodEnum SpectrumDataMethod { get => _spectrumDataMethod; set => _spectrumDataMethod = value; }
        public string SolarSpectrumDataObjectName { get => _solarSpectrumDataObjectName; set => _solarSpectrumDataObjectName = value; }
        public string VisibleSpectrumDataObjectName { get => _visibleSpectrumDataObjectName; set => _visibleSpectrumDataObjectName = value; }

        public SiteSolarAndVisibleSpectrum() { }

        private static List<SiteSolarAndVisibleSpectrum> list = new List<SiteSolarAndVisibleSpectrum>();

        public static void Add(SiteSolarAndVisibleSpectrum siteSolarAndVisibleSpectrum)
        {
            list.Add(siteSolarAndVisibleSpectrum);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:SolarAndVisibleSpectrum,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SpectrumDataMethod}" + ",").PadRight(27, ' ') + " !-Spectrum Data Method\n" +
                    ($"  {list[i].SolarSpectrumDataObjectName}" + ",").PadRight(27, ' ') + " !-Solar Spectrum Data Object Name\n" +
                    ($"  {list[i].VisibleSpectrumDataObjectName}" + ";").PadRight(27, ' ') + " !-Visible Spectrum Data Object Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteSolarAndVisibleSpectrum.Read())
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
