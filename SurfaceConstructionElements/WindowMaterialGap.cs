using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public class WindowMaterialGap
    {
        private string _name;
        private double _thickness;
        private string _gasorGasMixture;
        private double _pressure = 101325;
        private string _deflectionState;
        private string _supportPillar;

        public string Name { get => _name; set => _name = value; }
        public double Thickness 
        { 
            get => _thickness; 
            set
            {
                if (value > 0)
                {
                    _thickness = value;

                }
            }
        }
        public string GasorGasMixture { get => _gasorGasMixture; set => _gasorGasMixture = value; }
        public double Pressure { get => _pressure; set => _pressure = value; }
        public string DeflectionState { get => _deflectionState; set => _deflectionState = value; }
        public string SupportPillar { get => _supportPillar; set => _supportPillar = value; }

        public WindowMaterialGap() { }

        private static List<WindowMaterialGap> list = new List<WindowMaterialGap>();

        public static void Add(WindowMaterialGap windowMaterialGap)
        {
            list.Add(windowMaterialGap);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:Gap,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].GasorGasMixture}" + ",").PadRight(27, ' ') + " !-Gasor ((or Gas Mixture))\n" +
                    ($"  {list[i].Pressure}" + ",").PadRight(27, ' ') + " !-Pressure{{ Pa }}\n" +
                    ($"  {list[i].DeflectionState}" + ",").PadRight(27, ' ') + " !-Deflection State\n" +
                    ($"  {list[i].SupportPillar}" + ";").PadRight(27, ' ') + " !-Support Pillar";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialGap.Read())
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
