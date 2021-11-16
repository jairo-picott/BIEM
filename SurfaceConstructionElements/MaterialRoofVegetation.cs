using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BIEM.SurfaceConstructionElements
{
    
    public enum MoistureDiffusionCalculationmethodEnum
    {
        Advanced,
        Simple
    }
    public class MaterialRoofVegetation
    {
        private string _name;
        private double _heightofPlants = 0.2;
        private double _leafAreaIndex = 1;
        private double _leafReflectivity = 0.22;
        private double _leafEmissivity = 0.95;
        private double _minimumStomatalResistance = 180;
        private string _soilLayerName = "Green Roof Soil";
        private RoughnessType _roughness = RoughnessType.MediumRough;
        private double _thickness = 0.1;
        private double _conductivityofDrySoil = 0.35;
        private double _densityofDrySoil = 1100;
        private double _specificHeatofDrySoil = 1200;
        private double _thermalAbsorptance = 0.9;
        private double _solarAbsorptance = 0.7;
        private double _visibleAbsorptance = 0.75;
        private double _saturationVolumetricMoistureContentoftheSoilLayer = 0.3;
        private double _residualVolumetricMoistureContentoftheSoilLayer = 0.01;
        private double _initialVolumetricMoistureContentoftheSoilLayer = 0.1;
        private MoistureDiffusionCalculationmethodEnum _moistureDiffusionCalculationMethod = MoistureDiffusionCalculationmethodEnum.Advanced;

        public string Name { get => _name; set => _name = value; }
        public double HeightofPlants 
        { 
            get => _heightofPlants; 
            set
            {
                if (value >= 0.005 && value <= 1)
                {
                    _heightofPlants = value;
                }
            }
        }
        public double LeafAreaIndex 
        { 
            get => _leafAreaIndex;
            set
            {
                if (value >= 0.001 && value <= 5)
                {
                    _leafAreaIndex = value;
                }
            }
        }
        public double LeafReflectivity 
        { 
            get => _leafReflectivity;
            set
            {
                if (value >= 0.05 && value <= 0.5)
                {
                    _leafReflectivity = value;
                }
            }
        }
        public double LeafEmissivity 
        { 
            get => _leafEmissivity;
            set
            {
                if (value >= 0.8 && value <= 1)
                {
                    _leafEmissivity = value;
                }
            }
        }
        public double MinimumStomatalResistance 
        { 
            get => _minimumStomatalResistance;
            set
            {
                if (value >= 50 && value <= 300)
                {
                    _minimumStomatalResistance = value;
                }
            }
        }
        public string SoilLayerName { get => _soilLayerName; set => _soilLayerName = value; }
        public RoughnessType Roughness { get => _roughness; set => _roughness = value; }
        public double Thickness 
        { 
            get => _thickness;
            set
            {
                if (value >= 0.05 && value <= 0.7)
                {
                    _thickness = value;
                }
            }
        }
        public double ConductivityofDrySoil 
        { 
            get => _conductivityofDrySoil;
            set
            {
                if (value >= 0.2 && value <= 1.5)
                {
                    _conductivityofDrySoil = value;
                }
            }
        }
        public double DensityofDrySoil 
        { 
            get => _densityofDrySoil;
            set
            {
                if (value >= 300 && value <= 2000)
                {
                    _densityofDrySoil = value;
                }
            }
        }
        public double SpecificHeatofDrySoil 
        { 
            get => _specificHeatofDrySoil;
            set
            {
                if (value >= 500 && value <= 2000)
                {
                    _specificHeatofDrySoil = value;
                }
            }
        }
        public double ThermalAbsorptance 
        { 
            get => _thermalAbsorptance;
            set
            {
                if (value > 0.8 && value <= 1)
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
                if (value >= 0.4 && value <= 0.9)
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
                if (value > 0.5 && value <= 1)
                {
                    _visibleAbsorptance = value;
                }
            }
        }
        public double SaturationVolumetricMoistureContentoftheSoilLayer 
        { 
            get => _saturationVolumetricMoistureContentoftheSoilLayer;
            set
            {
                if (value > 0.1 && value <= 0.5)
                {
                    _saturationVolumetricMoistureContentoftheSoilLayer= value;
                }
            }
        }
        public double ResidualVolumetricMoistureContentoftheSoilLayer 
        { 
            get => _residualVolumetricMoistureContentoftheSoilLayer;
            set
            {
                if (value >= 0.01 && value <= 0.1)
                {
                    _residualVolumetricMoistureContentoftheSoilLayer = value;
                }
            }
        }
        public double InitialVolumetricMoistureContentoftheSoilLayer 
        { 
            get => _initialVolumetricMoistureContentoftheSoilLayer;
            set
            {
                if (value >= 0.05 && value <= 0.5)
                {
                    _initialVolumetricMoistureContentoftheSoilLayer = value;
                }
            }
        }
        public MoistureDiffusionCalculationmethodEnum MoistureDiffusionCalculationMethod { get => _moistureDiffusionCalculationMethod; set => _moistureDiffusionCalculationMethod = value; }

        public MaterialRoofVegetation() { }

        private static List<MaterialRoofVegetation> list = new List<MaterialRoofVegetation>();

        public static void Add(MaterialRoofVegetation material)
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
                print[i] = $"Material:RoofVegetation,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].HeightofPlants}" + ",").PadRight(27, ' ') + " !-Height of Plants {{ m }}\n" +
                    ($"  {list[i].LeafAreaIndex}" + ",").PadRight(27, ' ') + " !-Leaf Area Index {{ dimensionless }}\n" +
                    ($"  {list[i].LeafReflectivity}" + ",").PadRight(27, ' ') + " !-Leaf Reflectivity {{ dimensionless }}\n" +
                    ($"  {list[i].LeafEmissivity}" + ",").PadRight(27, ' ') + " !-Leaf Emissivity \n" +
                    ($"  {list[i].MinimumStomatalResistance}" + ",").PadRight(27, ' ') + " !-Minimum Stomatal Resistance{{ s/m }}\n" +
                    ($"  {list[i].SoilLayerName}" + ",").PadRight(27, ' ') + " !-Soil Layer Name\n" +
                    ($"  {list[i].Roughness}" + ",").PadRight(27, ' ') + " !-Roughness\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness {{ m }}\n" +
                    ($"  {list[i].ConductivityofDrySoil}" + ",").PadRight(27, ' ') + " !-Conductivity of Dry Soil {{ W/m-K }}\n" +
                    ($"  {list[i].DensityofDrySoil}" + ",").PadRight(27, ' ') + " !-Density of Dry Soil{{ kg/m3 }}\n" +
                    ($"  {list[i].SpecificHeatofDrySoil}" + ",").PadRight(27, ' ') + " !-Specific Heat of Dry Soil{{ J/kg-K }}\n" +
                    ($"  {list[i].ThermalAbsorptance}" + ",").PadRight(27, ' ') + " !-Thermal Absorptance\n" +
                    ($"  {list[i].SolarAbsorptance}" + ",").PadRight(27, ' ') + " !-Solar Absorptance\n" +
                    ($"  {list[i].VisibleAbsorptance}" + ",").PadRight(27, ' ') + " !-Visible Absorptance\n" +
                    ($"  {list[i].SaturationVolumetricMoistureContentoftheSoilLayer}" + ",").PadRight(27, ' ') + " !-Saturation Volumetric Moisture Content of the Soil Layer\n" +
                    ($"  {list[i].ResidualVolumetricMoistureContentoftheSoilLayer}" + ",").PadRight(27, ' ') + " !-Residual Volumetric Moisture Content of the Soil Layer\n" +
                    ($"  {list[i].InitialVolumetricMoistureContentoftheSoilLayer}" + ",").PadRight(27, ' ') + " !-Initial Volumetric Moisture Content of the Soil Layer\n" +
                    ($"  {list[i].MoistureDiffusionCalculationMethod}" + ";").PadRight(27, ' ') + " !-Moisture Diffusion Calculation Method";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in MaterialRoofVegetation.Read())
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
