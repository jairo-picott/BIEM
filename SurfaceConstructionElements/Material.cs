using System.IO;
using System.Collections.Generic;
using System;

namespace BIEM.SurfaceConstructionElements
{
    public enum RoughnessType
    {
        VeryRough,
        Rough,
        MediumRough,
        MediumSmooth,
        Smooth,
        VerySmooth
    }
    //------------Material
    //WHEN THE MODELING IS FOLLOWED AS RECOMENDED, THIS CLASS COLLECT
    //IFCBUILDINGELEMENTS AND ITS PROPERTYSET CONTAINING THE ENERGY 
    //CHARACTERISTICS OF EACH ONE TO CREATE THE MATERIAL CLASS FOR 
    //ENERGYPLUS, IFCBUILDINGELEMENT CAN BE, WALL, SLAB, CEILIG, ROOF,
    //COLUMN, BEAM, WINDOWS, DOORS, ETC
    public class Material
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private RoughnessType? _roughness;
        public RoughnessType? Roughness
        {
            get => _roughness;
            set => _roughness = value;
        }

        private double? _thickness = 0.00001;
        public double? Thickness
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

        private double? _conductivity;
        public double? Conductivity
        {
            get => _conductivity;
            set
            {
                if (value > 0)
                {
                    _conductivity = value;
                }
            }
        }

        private double? _density;
        public double? Density
        {
            get => _density;
            set
            {
                if (value > 0)
                {
                    _density = value;
                }
            }
        }

        private double? _specificHeat;
        public double? SpecificHeat
        {
            get => _specificHeat;
            set
            {
                if (value >= 100)
                {
                    _specificHeat = value;
                }
            }
        }

        private double? _thermalAbsorptance = 0.9;
        public double? ThermalAbsortance
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

        private double? _solarAbsorptance = 0.7;
        public double? SolarAbsorptance
        {
            get => _solarAbsorptance;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _solarAbsorptance = value;
                }
            }
        }

        private double? _visibleAbsorptance = 0.7;
        public double? VisibleAbsorptance
        {
            get => _visibleAbsorptance;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _visibleAbsorptance = value;
                }
            }
        }

        public Material() { }

        private static List<Material> list = new List<Material>();

        public static void Add(Material material)
        {
            var alreadyExist = list.Find(x => x.Name == material.Name);
            if (alreadyExist == null)
            {
                list.Add(material);
            }
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Material,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Roughness}" + ",").PadRight(27, ' ') + " !-Roughness\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].Conductivity}" + ",").PadRight(27, ' ') + " !-Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].Density}" + ",").PadRight(27, ' ') + " !-Density{{ kg/m3 }}\n" +
                    ($"  {list[i].SpecificHeat}" + ",").PadRight(27, ' ') + " !-Specific Heat{{ J/kg-K }}\n" +
                    ($"  {list[i].ThermalAbsortance}" + ",").PadRight(27, ' ') + " !-Thermal Absortance\n" +
                    ($"  {list[i].SolarAbsorptance}" + ",").PadRight(27, ' ') + " !-Solar Absorptance\n" +
                    ($"  {list[i].VisibleAbsorptance}" + ";").PadRight(27, ' ') + " !-Visible Absorptance";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in Material.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Get()
        {
            
            foreach (string line in Material.Read())
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
