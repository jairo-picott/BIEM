using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{

    public class MaterialNoMass
    {
        private string _name;
        private RoughnessType _roughness;
        private double _thermalResistance;
        private double _thermalAbsorptance = 0.9;
        private double _solarAbsorptance = 0.7;
        private double _visibleAbsorptance = 0.7;

        public string Name { get => _name; set => _name = value; }
        public RoughnessType Roughness { get => _roughness; set => _roughness = value; }
        public double ThermalResistance 
        { 
            get => _thermalResistance;
            set
            {
                if (value >= 0.001)
                {
                    _thermalResistance = value;

                }
            }
        }
        public double ThermalAbsorptance 
        { 
            get => _thermalAbsorptance; 
            set
            {
                if (value > 0 && value <= 0.99999)
                {
                    _thermalAbsorptance = value;

                }
            }
        }
        public double SolarAbsorptance 
        { 
            get => _solarAbsorptance;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _solarAbsorptance = value;

                }
            }
        }
        public double VisibleAbsorptance 
        { 
            get => _visibleAbsorptance;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _visibleAbsorptance = value;

                }
            }
        }

        public MaterialNoMass() { }

        private static List<MaterialNoMass> list = new List<MaterialNoMass>();

        public static void Add(MaterialNoMass materialNoMass)
        {
            list.Add(materialNoMass);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Material:NoMass,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Roughness}" + ",").PadRight(27, ' ') + " !-Roughness\n" +
                    ($"  {list[i].ThermalResistance}" + ",").PadRight(27, ' ') + " !-Thermal Resistance {{ m2-K/W }}\n" +
                    ($"  {list[i].ThermalAbsorptance}" + ",").PadRight(27, ' ') + " !-Thermal Absorptance\n" +
                    ($"  {list[i].SolarAbsorptance}" + ",").PadRight(27, ' ') + " !-Solar Absorptance\n" +
                    ($"  {list[i].VisibleAbsorptance}" + ";").PadRight(27, ' ') + " !-Visible Absorptance";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in MaterialNoMass.Read())
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
