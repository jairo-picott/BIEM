using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public class WindowGapDeflectionState
    {
        private string _name;
        private double _deflectedThickness = 0;
        private double _initialTemperature = 25;
        private double _initialPressure = 101325;

        public string Name { get => _name; set => _name = value; }
        public double DeflectedThickness 
        { 
            get => _deflectedThickness; 
            set
            {
                if ( value >= 0)
                {
                    _deflectedThickness = value;

                }
            }
        }
        public double InitialTemperature 
        { 
            get => _initialTemperature;
            set
            {
                if (value >= 0)
                {
                    _initialTemperature= value;

                }
            }
        }
        public double InitialPressure 
        { 
            get => _initialPressure;
            set
            {
                if (value >= 0)
                {
                    _initialPressure= value;

                }
            }
        }

        public WindowGapDeflectionState() { }

        private static List<WindowGapDeflectionState> list = new List<WindowGapDeflectionState>();

        public static void Add(WindowGapDeflectionState windowGapDeflectionState)
        {
            list.Add(windowGapDeflectionState);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowGap:DeflectionState,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].DeflectedThickness}" + ",").PadRight(27, ' ') + " !-Deflected Thickness{{ m }}\n" +
                    ($"  {list[i].InitialTemperature}" + ",").PadRight(27, ' ') + " !-Initial Temperature{{ C }}\n" +
                    ($"  {list[i].InitialPressure}" + ";").PadRight(27, ' ') + " !-Initial Pressure{{ Pa }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowGapDeflectionState.Read())
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
