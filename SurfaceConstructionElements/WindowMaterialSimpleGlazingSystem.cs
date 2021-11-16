using System.IO;
using System.Collections.Generic;


namespace BIEM.SurfaceConstructionElements
{
    public class WindowMaterialSimpleGlazingSystem
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private double? _uFactor;
        public double? UFactor
        {
            get => _uFactor;
            set
            {
                if (value >0 && value<=7)
                {
                    _uFactor = value;
                }
            }
        }

        private double? _solarHeatGainCoefficient;
        public double? SolarHeatGainCoefficient
        {
            get => _solarHeatGainCoefficient;
            set
            {
                if (value > 0 && value < 1)
                {
                    _solarHeatGainCoefficient= value;
                }
            }
        }

        private double? _visibleTransmittance;
        public double? VisibleTransmittance
        {
            get => _visibleTransmittance;
            set
            {
                if (value > 0 && value < 1)
                {
                    _visibleTransmittance= value;
                }
            }
        }

        public WindowMaterialSimpleGlazingSystem() { }

        private static List<WindowMaterialSimpleGlazingSystem> list = new List<WindowMaterialSimpleGlazingSystem>();

        public static void Add(WindowMaterialSimpleGlazingSystem windowMaterialSimpleGlazingSystem)
        {
            var alreadyExist = list.Find(x => x.Name == windowMaterialSimpleGlazingSystem.Name);
            if (alreadyExist == null)
            {
                list.Add(windowMaterialSimpleGlazingSystem);
            }
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:SimpleGlazingSystem,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].UFactor}" + ",").PadRight(27, ' ') + " !-U Factor{{ W/m2-k }}\n" +
                    ($"  {list[i].SolarHeatGainCoefficient}" + ",").PadRight(27, ' ') + " !-Solar Heat Gain Coefficient\n" +
                    ($"  {list[i].VisibleTransmittance}" + ";").PadRight(27, ' ') + " !-Visible Transmittance";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialSimpleGlazingSystem.Read())
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
