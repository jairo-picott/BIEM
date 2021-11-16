using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public class MaterialAirGap
    {
        private string _name;
        private double _thermalResistance;

        public double ThermalResistance 
        { 
            get => _thermalResistance; 
            set
            {
                if (value > 0)
                {
                    _thermalResistance = value;

                }
            }
                
        }
        public string Name { get => _name; set => _name = value; }

        public MaterialAirGap() { }

        private static List<MaterialAirGap> list = new List<MaterialAirGap>();

        public static void Add(MaterialAirGap materialAirGap)
        {
            list.Add(materialAirGap);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Material:AirGap,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ThermalResistance}" + ";").PadRight(27, ' ') + " !-Thermal Resistance {{ m2-K/W }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in MaterialAirGap.Read())
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
