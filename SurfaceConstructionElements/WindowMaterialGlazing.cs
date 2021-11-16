using System.IO;
using System.Collections.Generic;

namespace BIEM.SurfaceConstructionElements
{
    public enum OpticalDataType
    {
        SpectralAverage,
        Spectral,
        BSDF,
        SpectralAndAngle
    }

    public enum SolarDiffusingType
    {
        Yes,
        No
    }
    public class WindowMaterialGlazing
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private OpticalDataType? _opticalDataType;
        public OpticalDataType? OpticalDataType
        {
            get => _opticalDataType;
            set => _opticalDataType = value;
        }

        private string _windowGlassSpectralDataSetName;
        public string WindowGlassSpectralDataSetName
        {
            get => _windowGlassSpectralDataSetName;
            set => _windowGlassSpectralDataSetName= value;
        }

        private double? _thickness;
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

        private double? _solarTransmittanceatNormalIncidence;
        public double? SolarTransmittanceatNormalIncidence
        {
            get => _solarTransmittanceatNormalIncidence;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _solarTransmittanceatNormalIncidence = value;
                }
            }
        }

        private double? _frontSideSolarReflectanceatNormalIncidence;
        public double? FrontSideSolarReflectanceatNormalIncidence
        {
            get => _frontSideSolarReflectanceatNormalIncidence;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _frontSideSolarReflectanceatNormalIncidence = value;
                }
            }
        }

        private double? _backSideSolarReflectancearNormalIncidence;
        public double? BackSideSolarReflectanceatNormalIncidence
        {
            get => _backSideSolarReflectancearNormalIncidence;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _backSideSolarReflectancearNormalIncidence= value;
                }
            }
        }

        private double? _visibleTransmittanceatNormalIncidence;
        public double? VisibleTransmittanceatNormalIncidence
        {
            get => _visibleTransmittanceatNormalIncidence;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _visibleTransmittanceatNormalIncidence= value;
                }
            }
        }

        private double? _frontSideVisibleReflectanceatNormalIncidence;
        public double? FrontSideVisibleReflectanceatNormalIncidence
        {
            get => _frontSideVisibleReflectanceatNormalIncidence;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _frontSideVisibleReflectanceatNormalIncidence= value;
                }
            }
        }

        private double? _backSideVisibleReflectanceatNormalIncidence;
        public double? BackSideVisibleReflectanceatNormalIncidence
        {
            get => _backSideVisibleReflectanceatNormalIncidence;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _backSideVisibleReflectanceatNormalIncidence= value;
                }
            }
        }

        private double? _infraredTransmittanceHemisphericalEmissivity = 0;
        public double? InfraredTransmittanceatNormalIncidence
        {
            get => _infraredTransmittanceHemisphericalEmissivity;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _infraredTransmittanceHemisphericalEmissivity= value;
                }
            }
        }

        private double? _frontSideInfraredHemisphericalEmissivity = 0.84;
        public double? FrontSideInfraredHemisphericalEmissivity
        {
            get => _frontSideInfraredHemisphericalEmissivity;
            set
            {
                if (value > 0 && value < 1)
                {
                    _frontSideInfraredHemisphericalEmissivity = value;
                }
            }
        }

        private double? _backSideInfraredHemisphericalEmissivity = 0.84;
        public double? BackSideInfraredHemisphericalEmissivity
        {
            get => _backSideInfraredHemisphericalEmissivity;
            set
            {
                if (value > 0 && value < 1)
                {
                    _backSideInfraredHemisphericalEmissivity = value;
                }
            }
        }

        private double? _conductivity = 0.9;
        public double? Conductivity
        {
            get => _conductivity;
            set
            {
                if (value > 0)
                {
                    _conductivity= value;
                }
            }
        }

        private double? _dirtCorrectionFactorforSolarandVisibleTransmittance = 1;
        public double? DirtCorrectionFactorforSolarandVisibleTranmittance
        {
            get => _dirtCorrectionFactorforSolarandVisibleTransmittance;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _dirtCorrectionFactorforSolarandVisibleTransmittance= value;
                }
            }
        }

        private SolarDiffusingType? _solarDiffusing = SolarDiffusingType.No;
        public SolarDiffusingType? SolarDiffusing
        {
            get => _solarDiffusing;
            set => _solarDiffusing = value;
            
        }

        private double? _youngModulus = 72000000000;
        public double? YoungsModulus
        {
            get => _youngModulus;
            set
            {
                if (value > 0)
                {
                    _youngModulus= value;
                }
            }
        }

        private double? _poissonsRatio = 0.22;
        public double? PoissonsRatio
        {
            get => _poissonsRatio;
            set
            {
                if (value > 0 && value < 1)
                {
                    _poissonsRatio= value;
                }
            }
        }

        private string _windowGlassSpectralandIncidentAngleTransmittance;
        public string WindowGlassSpectralandIncidentAngleTransmittance
        {
            get => _windowGlassSpectralandIncidentAngleTransmittance;
            set => _windowGlassSpectralandIncidentAngleTransmittance = value;
        }

        private string _windowGlassSpectralandIncidentAngleFrontReflectance;
        public string WindowGlassSpectralandIncidentAngleFrontReflectance
        {
            get => _windowGlassSpectralandIncidentAngleFrontReflectance;
            set => _windowGlassSpectralandIncidentAngleFrontReflectance = value;
        }

        private string _windowGlassSpectralandIncidentAngleBackReflectance;
        public string WindowGlassSpectralandIncidentAngleBackReflectance
        {
            get => _windowGlassSpectralandIncidentAngleBackReflectance;
            set => _windowGlassSpectralandIncidentAngleBackReflectance = value;
        }

        public WindowMaterialGlazing() { }

        private static List<WindowMaterialGlazing> list = new List<WindowMaterialGlazing>();

        public static void Add(WindowMaterialGlazing windowMaterialGlazing)
        {
            list.Add(windowMaterialGlazing);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:Glazing,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].OpticalDataType}" + ",").PadRight(27, ' ') + " !-Optical Data Type\n" +
                    ($"  {list[i].WindowGlassSpectralDataSetName}" + ",").PadRight(27, ' ') + " !-Window Glass Spectral Data Set Name\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].SolarTransmittanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Solar Transmittance at Normal Incidence\n" +
                    ($"  {list[i].FrontSideSolarReflectanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Front Side Solar Reflectance at Normal Incidence\n" +
                    ($"  {list[i].BackSideSolarReflectanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Back Side Solar Reflectance at Normal Incidence\n" +
                    ($"  {list[i].VisibleTransmittanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Visible Transmittance at Normal Incidence\n" +
                    ($"  {list[i].FrontSideVisibleReflectanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Front Side Visible Reflectance at Normal Incidence\n" +
                    ($"  {list[i].BackSideVisibleReflectanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Back Side Visible Reflectance at Normal Incidence\n" +
                    ($"  {list[i].InfraredTransmittanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Infrared Transmittance at Normal Incidence\n" +
                    ($"  {list[i].FrontSideInfraredHemisphericalEmissivity}" + ",").PadRight(27, ' ') + " !-Front Side Infrared Hemispherical Emissivity\n" +
                    ($"  {list[i].BackSideInfraredHemisphericalEmissivity}" + ",").PadRight(27, ' ') + " !-Back Side Infrared Hemispherical Emissivity\n" +
                    ($"  {list[i].Conductivity}" + ",").PadRight(27, ' ') + " !-Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].DirtCorrectionFactorforSolarandVisibleTranmittance}" + ",").PadRight(27, ' ') + " !-Dirt Correction Factor for Solar and Visible Tranmittance\n" +
                    ($"  {list[i].SolarDiffusing}" + ",").PadRight(27, ' ') + " !-Solar Diffusing\n" +
                    ($"  {list[i].YoungsModulus}" + ",").PadRight(27, ' ') + " !-Youngs Modulus{{ Pa }}\n" +
                    ($"  {list[i].PoissonsRatio}" + ",").PadRight(27, ' ') + " !-Poissons Ratio\n" +
                    ($"  {list[i].WindowGlassSpectralandIncidentAngleTransmittance}" + ",").PadRight(27, ' ') + " !-Window Glass Spectral and Incident Angle Transmittance\n" +
                    ($"  {list[i].WindowGlassSpectralandIncidentAngleFrontReflectance}" + ",").PadRight(27, ' ') + " !-Window Glass Spectral and Incident Angle Front Reflectance\n" +
                    ($"  {list[i].WindowGlassSpectralandIncidentAngleBackReflectance}" + ";").PadRight(27, ' ') + " !-Window Glass Spectral and Incident Angle Back Reflectance";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialGlazing.Read())
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
