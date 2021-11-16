using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum SlabLocationEnum
    {
        InGrade,
        OnGrade
    }
    public enum HorizontalInsulationEnum
    {
        Yes,
        No
    }
    public enum HorizontalInsulationExtentsEnum
    {
        Full,
        Perimeter
    }
    public enum VerticalInsulationEnum
    {
        Yes,
        No
    }
    public enum SimulationTimestepEnum
    {
        Hourly,
        Timestep
    }
    public class SiteGroundDomainSlab
    {
        private string _name;
        private double _groundDomainDepth = 10;
        private double _aspectRatio = 1;
        private double _perimeterOffset = 5;
        private double _soilThermalConductivity = 1.5;
        private double _soilDensity = 2800;
        private double _soilSpecificHeat = 850;
        private double _soilMoistureContentVolumeFraction = 30;
        private double _soilMoistureContentVolumeFractionatSaturation = 50;
        private string _undisturbedGroundTemperatureModelType;
        private string _undisturbedGroundTemperatureModelName;
        private double _evapotranspirationGroundCoverParameter = 0.4;
        private string _slabBoundaryConditionModelName;
        private SlabLocationEnum _slabLocation;
        private string _slabMaterialName;
        private HorizontalInsulationEnum _horizontalInsulation = HorizontalInsulationEnum.No;
        private string _horizontalInsulationMaterialName;
        private HorizontalInsulationExtentsEnum _horizontalInsulationExtents = HorizontalInsulationExtentsEnum.Full;
        private double _perimeterInsulationWidth;
        private VerticalInsulationEnum _verticalInsulation = VerticalInsulationEnum.No;
        private string _verticalInsulationMaterialName;
        private double _verticalInsulationDepth;
        private SimulationTimestepEnum _simulationTimestep = SimulationTimestepEnum.Hourly;
        private double _geometricMeshCoefficient = 1.6;
        private double _meshDensityParameter = 6;
        
        public string Name { get => _name; set => _name = value; }
        public double GroundDomainDepth 
        { 
            get => _groundDomainDepth; 
            set
            {
                if (value > 0)
                {
                    _groundDomainDepth = value;
                }
            }
        }
        public double AspectRatio { get => _aspectRatio; set => _aspectRatio = value; }
        public double PerimeterOffset 
        { 
            get => _perimeterOffset; 
            set
            {
                if (value > 0)
                {
                    _perimeterOffset = value;
                }
            }
        }

        public double SoilThermalConductivity 
        { 
            get => _soilThermalConductivity;
            set
            {
                if (value > 0)
                {
                    _soilThermalConductivity = value;
                }
            }
        }

        public double SoilDensity 
        { 
            get => _soilDensity;
            set
            {
                if (value > 0)
                {
                    _soilDensity = value;
                }
            }
        }

        public double SoilSpecificHeat 
        { 
            get => _soilSpecificHeat;
            set
            {
                if (value > 0)
                {
                    _soilSpecificHeat = value;
                }
            }
        }

        public double SoilMoistureContentVolumeFraction 
        { 
            get => _soilMoistureContentVolumeFraction;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _soilMoistureContentVolumeFraction = value;
                }
            }
        }

        public double SoilMoistureContentVolumeFractionatSaturation 
        { 
            get => _soilMoistureContentVolumeFractionatSaturation;
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _soilMoistureContentVolumeFractionatSaturation = value;
                }
            }
        }

        public string UndisturbedGroundTemperatureModelType { get => _undisturbedGroundTemperatureModelType; set => _undisturbedGroundTemperatureModelType = value; }
        public string UndisturbedGroundTemperatureModelName { get => _undisturbedGroundTemperatureModelName; set => _undisturbedGroundTemperatureModelName = value; }
        public double EvapotranspirationGroundCoverParameter 
        { 
            get => _evapotranspirationGroundCoverParameter;
            set
            {
                if (value >= 0 && value <= 1.5)
                {
                    _evapotranspirationGroundCoverParameter = value;
                }
            }
        }

        public string SlabBoundaryConditionModelName { get => _slabBoundaryConditionModelName; set => _slabBoundaryConditionModelName = value; }
        public SlabLocationEnum SlabLocation { get => _slabLocation; set => _slabLocation = value; }
        public string SlabMaterialName { get => _slabMaterialName; set => _slabMaterialName = value; }
        public HorizontalInsulationEnum HorizontalInsulation { get => _horizontalInsulation; set => _horizontalInsulation = value; }
        public string HorizontalInsulationMaterialName { get => _horizontalInsulationMaterialName; set => _horizontalInsulationMaterialName = value; }
        public HorizontalInsulationExtentsEnum HorizontalInsulationExtents { get => _horizontalInsulationExtents; set => _horizontalInsulationExtents = value; }
        public double PerimeterInsulationWidth 
        { 
            get => _perimeterInsulationWidth;
            set
            {
                if (value > 0)
                {
                    _perimeterInsulationWidth = value;
                }
            }
        }

        public VerticalInsulationEnum VerticalInsulation { get => _verticalInsulation; set => _verticalInsulation = value; }
        public string VerticalInsulationMaterialName { get => _verticalInsulationMaterialName; set => _verticalInsulationMaterialName = value; }
        public double VerticalInsulationDepth 
        { 
            get => _verticalInsulationDepth;
            set
            {
                if (value > 0)
                {
                    _verticalInsulationDepth = value;
                }
            }
        }

        public SimulationTimestepEnum SimulationTimestep { get => _simulationTimestep; set => _simulationTimestep = value; }
        public double GeometricMeshCoefficient 
        { 
            get => _geometricMeshCoefficient; 
            set
            {
                if (value >= 1 && value <= 2)
                {
                    _geometricMeshCoefficient = value;
                }
            }
        }

        public double MeshDensityParameter 
        { 
            get => _meshDensityParameter;
            set
            {
                if (value >= 4)
                {
                    _meshDensityParameter = value;
                }
            }
        }

        public SiteGroundDomainSlab() { }

        private static List<SiteGroundDomainSlab> list = new List<SiteGroundDomainSlab>();

        public static void Add(SiteGroundDomainSlab siteGroundDomainSlab)
        {
            list.Add(siteGroundDomainSlab);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundDomain:Slab,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].GroundDomainDepth}" + ",").PadRight(27, ' ') + " !-Ground Domain Depth {{ m }}\n" +
                    ($"  {list[i].AspectRatio}" + ",").PadRight(27, ' ') + " !-Aspect Ratio\n" +
                    ($"  {list[i].PerimeterOffset}" + ",").PadRight(27, ' ') + " !-PerimeterOffset {{ m }}\n" +
                    ($"  {list[i].SoilThermalConductivity}" + ",").PadRight(27, ' ') + " !-Soil Thermal Conductivity {{ W/m-K }}\n" +
                    ($"  {list[i].SoilDensity}" + ",").PadRight(27, ' ') + " !-Soil Density {{ kg/m3 }}\n" +
                    ($"  {list[i].SoilSpecificHeat}" + ",").PadRight(27, ' ') + " !-Soil Specific Heat {{ J/kg-K }}\n" +
                    ($"  {list[i].SoilMoistureContentVolumeFraction}" + ",").PadRight(27, ' ') + " !-Soil Moisture Content Volume Fraction {{ % }}\n" +
                    ($"  {list[i].SoilMoistureContentVolumeFractionatSaturation}" + ",").PadRight(27, ' ') + " !-Soil Moisture Content Volume Fraction at Saturation {{ % }}\\n" +
                    ($"  {list[i].UndisturbedGroundTemperatureModelType}" + ",").PadRight(27, ' ') + " !-Undisturbed Ground Temperature Mode l Type \n" +
                    ($"  {list[i].UndisturbedGroundTemperatureModelName}" + ",").PadRight(27, ' ') + " !-Undisturbed Ground Temperature Mode l Name\n" +
                    ($"  {list[i].EvapotranspirationGroundCoverParameter}" + ",").PadRight(27, ' ') + " !-Evapotranspiration Ground Cover Parameter\n" +
                    ($"  {list[i].SlabBoundaryConditionModelName}" + ",").PadRight(27, ' ') + " !-Slab Boundary Condition Model Name\n" +
                    ($"  {list[i].SlabLocation}" + ",").PadRight(27, ' ') + " !-Slab Location\n" +
                    ($"  {list[i].SlabMaterialName}" + ",").PadRight(27, ' ') + " !-Slab Material Name\n" +
                    ($"  {list[i].HorizontalInsulation}" + ",").PadRight(27, ' ') + " !-Horizontal Insulation\n" +
                    ($"  {list[i].HorizontalInsulationMaterialName}" + ",").PadRight(27, ' ') + " !-Horizontal Insulation Material Name\n" +
                    ($"  {list[i].HorizontalInsulationExtents}" + ",").PadRight(27, ' ') + " !-Horizontal Insulation Extents\n" +
                    ($"  {list[i].PerimeterInsulationWidth}" + ",").PadRight(27, ' ') + " !-Perimeter Insulation Width {{ m }}\n" +
                    ($"  {list[i].VerticalInsulation}" + ",").PadRight(27, ' ') + " !-Vertical Insulation\n" +
                    ($"  {list[i].VerticalInsulationMaterialName}" + ",").PadRight(27, ' ') + " !-Vertical Insulation Material Name\n" +
                    ($"  {list[i].VerticalInsulationDepth}" + ",").PadRight(27, ' ') + " !-Vertical Insulation Depth{{ m }}\n" +
                    ($"  {list[i].SimulationTimestep}" + ",").PadRight(27, ' ') + " !-Simulation Timestep\n" +
                    ($"  {list[i].GeometricMeshCoefficient}" + ",").PadRight(27, ' ') + " !-Geometric Mesh Coefficient\n" +
                    ($"  {list[i].MeshDensityParameter}" + ";").PadRight(27, ' ') + " !-Mesh Density Parameter";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundDomainSlab.Read())
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
