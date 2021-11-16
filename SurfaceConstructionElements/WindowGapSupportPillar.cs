using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public class WindowGapSupportPillar
    {
        private string _name;
        private double _spacing = 0.04;
        private double _radius = 0.0004;

        public string Name { get => _name; set => _name = value; }
        public double Spacing 
        { 
            get => _spacing; 
            set
            {
                if (value > 0)
                {
                    _spacing = value;
                }
            }
        }
        public double Radius 
        { 
            get => _radius;
            set
            {
                if (value > 0)
                {
                    _radius = value;
                }
            }
        }

        public WindowGapSupportPillar() { }

        private static List<WindowGapSupportPillar> list = new List<WindowGapSupportPillar>();

        public static void Add(WindowGapSupportPillar windowGapSupportPillar)
        {
            list.Add(windowGapSupportPillar);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowGap:SupportPillar,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Spacing}" + ",").PadRight(27, ' ') + " !-Spacing{{ m }}\n" +
                    ($"  {list[i].Radius}" + ";").PadRight(27, ' ') + " !-Radius{{ m }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowGapSupportPillar.Read())
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
