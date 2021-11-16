using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public class SiteGroundDomainBasement
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
        private string _basementFloorBoundaryConditionModelName;
        private HorizontalInsulationEnum _horizontalInsulation = HorizontalInsulationEnum.No;
        private string _horizontalInsulationMaterialName;
        private HorizontalInsulationExtentsEnum _horizontalInsulationExtents = HorizontalInsulationExtentsEnum.Full;
        private double _perimeterHorizontalInsulationWidth;
        private double _basementWallDepth;
        private string _basementWallBoundaryConditionModelName;
        private VerticalInsulationEnum _verticalInsulation = VerticalInsulationEnum.No;
        private string _basementWallVerticalInsulationMaterialName;
        private double _verticalInsulationDepth;
        private SimulationTimestepEnum _simulationTimestep = SimulationTimestepEnum.Hourly;
        private double _meshDensityParameter = 4;

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

        public HorizontalInsulationEnum HorizontalInsulation { get => _horizontalInsulation; set => _horizontalInsulation = value; }
        public string HorizontalInsulationMaterialName { get => _horizontalInsulationMaterialName; set => _horizontalInsulationMaterialName = value; }
        public HorizontalInsulationExtentsEnum HorizontalInsulationExtents { get => _horizontalInsulationExtents; set => _horizontalInsulationExtents = value; }
        public double PerimeterHorizontalInsulationWidth
        {
            get => _perimeterHorizontalInsulationWidth;
            set
            {
                if (value > 0)
                {
                    _perimeterHorizontalInsulationWidth = value;
                }
            }
        }

        public VerticalInsulationEnum VerticalInsulation { get => _verticalInsulation; set => _verticalInsulation = value; }
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

        public string BasementFloorBoundaryConditionModelName { get => _basementFloorBoundaryConditionModelName; set => _basementFloorBoundaryConditionModelName = value; }
        public string BasementWallVerticalInsulationMaterialName { get => _basementWallVerticalInsulationMaterialName; set => _basementWallVerticalInsulationMaterialName = value; }
        public double BasementWallDepth 
        { 
            get => _basementWallDepth;
            set
            {
                if (value > 0)
                {
                    _basementWallDepth = value;
                }
            }
        }

        public string BasementWallBoundaryConditionModelName { get => _basementWallBoundaryConditionModelName; set => _basementWallBoundaryConditionModelName = value; }

        public SiteGroundDomainBasement() { }

        private static List<SiteGroundDomainBasement> list = new List<SiteGroundDomainBasement>();

        public static void Add(SiteGroundDomainBasement siteGroundDomainBasement)
        {
            list.Add(siteGroundDomainBasement);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Site:GroundDomain:Basement,\n" +
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
                    ($"  {list[i].BasementFloorBoundaryConditionModelName}" + ",").PadRight(27, ' ') + " !-Basement Floor Boundary Condotion Model Name\n" +
                    ($"  {list[i].HorizontalInsulation}" + ",").PadRight(27, ' ') + " !-Horizontal Insulation\n" +
                    ($"  {list[i].HorizontalInsulationMaterialName}" + ",").PadRight(27, ' ') + " !-Horizontal Insulation Material Name\n" +
                    ($"  {list[i].HorizontalInsulationExtents}" + ",").PadRight(27, ' ') + " !-Horizontal Insulation Extents\n" +
                    ($"  {list[i].PerimeterHorizontalInsulationWidth}" + ",").PadRight(27, ' ') + " !-Perimeter Horizontal Insulation Width {{ m }}\n" +
                    ($"  {list[i].BasementWallDepth}" + ",").PadRight(27, ' ') + " !-Basement Wall Depth {{ m }}\n" +
                    ($"  {list[i].BasementWallBoundaryConditionModelName}" + ",").PadRight(27, ' ') + " !-Basement Wall Boundary Condition Model Name\n" +
                    ($"  {list[i].VerticalInsulation}" + ",").PadRight(27, ' ') + " !-Vertical Insulation\n" +
                    ($"  {list[i].BasementWallVerticalInsulationMaterialName}" + ",").PadRight(27, ' ') + " !-Basement Wall Vertical Insulation Material Name\n" +
                    ($"  {list[i].VerticalInsulationDepth}" + ",").PadRight(27, ' ') + " !-Vertical Insulation Depth {{ m }}\n" +
                    ($"  {list[i].SimulationTimestep}" + ",").PadRight(27, ' ') + " !-Simulation Timestep\n" +
                    ($"  {list[i].MeshDensityParameter}" + ";").PadRight(27, ' ') + " !-Mesh Density Parameter";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SiteGroundDomainBasement.Read())
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
