using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public enum SlatOrientationEnum
    {
        Horizontal,
        Vertical
    }
    public class WindowMaterialBlind
    {
        private string _name;
        private SlatOrientationEnum _slatOrientation = SlatOrientationEnum.Horizontal;
        private double _slatWidth;
        private double _slatSeparation;
        private double _slatThickness = 0.00025;
        private double _slatAngle = 45;
        private double _slatConductivity = 221;
        private double _slatBeamSolarTransmittance;
        private double _frontSideSlatBeamSolarReflectance;
        private double _backSideSlatBeamSolarReflectance;
        private double _slatDiffuseSolarTransmittance;
        private double _frontSideSlatDiffuseSolarReflectance;
        private double _backSideSlatDiffuseSolarReflectance;
        private double _slatBeamVisibleTransmittance;
        private double _frontSideSlatBeamVisibleReflectance;
        private double _backSideSlatBeamVisibleReflectance;
        private double _slatDiffuseVisibleTransmittance;
        private double _frontSideSlatDiffuseVisibleReflectance;
        private double _backSideSlatDiffuseVisibleReflectance;
        private double _slatInfraredHemisphericalTransmittance;
        private double _frontSideSlatInfraretHemisphericalEmissivity = 0.9;
        private double _backSideSlatInfraredHemisphericalEmissivity = 0.9;
        private double _blindtoGlassDistance = 0.05;
        private double _blindTopOpeningMultiplier = 0.5;
        private double _blindBottomOpeningMultiplier;
        private double _blindLeftSideOpeningMultiplier = 0.5;
        private double _blindRightSideOpeningMultiplier = 0.5;
        private double _minimumSlatAngle;
        private double _maximumSlatAngle = 180;

        public string Name { get => _name; set => _name = value; }
        public SlatOrientationEnum SlatOrientation { get => _slatOrientation; set => _slatOrientation = value; }
        public double SlatWidth 
        { 
            get => _slatWidth; 
            set
            {
                if (value > 0 && value <=1)
                {
                    _slatWidth = value;
                }
            }
        }
        public double SlatSeparation 
        { 
            get => _slatSeparation;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _slatSeparation = value;
                }
            }
        }
        public double SlatThickness 
        { 
            get => _slatThickness;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _slatThickness = value;
                }
            }
        }
        public double SlatConductivity 
        { 
            get => _slatConductivity;
            set
            {
                if (value > 0)
                {
                    _slatConductivity = value;
                }
            }
        }
        public double SlatBeamSolarTransmittance 
        { 
            get => _slatBeamSolarTransmittance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _slatBeamSolarTransmittance = value;
                }
            }
        }
        public double FrontSideSlatBeamSolarReflectance 
        { 
            get => _frontSideSlatBeamSolarReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _frontSideSlatBeamSolarReflectance = value;
                }
            }
        }
        public double BackSideSlatBeamSolarReflectance 
        { 
            get => _backSideSlatBeamSolarReflectance;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _backSideSlatBeamSolarReflectance = value;
                }
            }
        }
        public double SlatDiffuseSolarTransmittance 
        { 
            get => _slatDiffuseSolarTransmittance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _slatDiffuseSolarTransmittance = value;
                }
            }
        }
        public double FrontSideSlatDiffuseSolarReflectance 
        { 
            get => _frontSideSlatDiffuseSolarReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _frontSideSlatDiffuseSolarReflectance = value;
                }
            }
        }
        public double BackSideSlatDiffuseSolarReflectance 
        { 
            get => _backSideSlatDiffuseSolarReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _backSideSlatDiffuseSolarReflectance = value;
                }
            }
        }
        public double SlatDiffuseVisibleTransmittance 
        { 
            get => _slatDiffuseVisibleTransmittance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _slatDiffuseVisibleTransmittance = value;
                }
            }
        }
        public double FrontSideSlatDiffuseVisibleReflectance 
        { 
            get => _frontSideSlatDiffuseVisibleReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _frontSideSlatDiffuseVisibleReflectance = value;
                }
            }
        }
        public double BackSideSlatDiffuseVisibleReflectance 
        { 
            get => _backSideSlatDiffuseVisibleReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _backSideSlatDiffuseVisibleReflectance = value;
                }
            }
        }
        public double SlatInfraredHemisphericalTransmittance 
        { 
            get => _slatInfraredHemisphericalTransmittance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _slatInfraredHemisphericalTransmittance= value;
                }
            }
        }
        public double FrontSideSlatInfraretHemisphericalEmissivity 
        { 
            get => _frontSideSlatInfraretHemisphericalEmissivity;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _frontSideSlatInfraretHemisphericalEmissivity = value;
                }
            }
        }
        public double BackSideSlatInfraredHemisphericalEmissivity 
        { 
            get => _backSideSlatInfraredHemisphericalEmissivity;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _backSideSlatInfraredHemisphericalEmissivity = value;
                }
            }
        }
        public double BlindtoGlassDistance 
        { 
            get => _blindtoGlassDistance;
            set
            {
                if (value >= 0.01 && value <= 1)
                {
                    _blindtoGlassDistance = value;
                }
            }
        }
        public double BlindTopOpeningMultiplier 
        { 
            get => _blindTopOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _blindTopOpeningMultiplier = value;
                }
            }
        }
        public double BlindBottomOpeningMultiplier 
        { 
            get => _blindBottomOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _blindBottomOpeningMultiplier = value;
                }
            }
        }
        public double BlindLeftSideOpeningMultiplier 
        { 
            get => _blindLeftSideOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _blindLeftSideOpeningMultiplier = value;
                }
            }
        }
        public double BlindRightSideOpeningMultiplier 
        { 
            get => _blindRightSideOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _blindRightSideOpeningMultiplier = value;
                }
            }
        }
        public double MinimumSlatAngle 
        { 
            get => _minimumSlatAngle;
            set
            {
                if (value >= 0 && value <= 180)
                {
                    _minimumSlatAngle = value;
                }
            }
        }
        public double MaximumSlatAngle 
        { 
            get => _maximumSlatAngle;
            set
            {
                if (value >= 0 && value <= 180)
                {
                    _maximumSlatAngle = value;
                }
            }
        }
        public double SlatAngle 
        { 
            get => _slatAngle;
            set
            {
                if (value >= 0 && value <= 180)
                {
                    _slatAngle = value;
                }
            }
        }

        public double SlatBeamVisibleTransmittance 
        { 
            get => _slatBeamVisibleTransmittance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _slatBeamVisibleTransmittance = value;
                }
            }
        }

        public double FrontSideSlatBeamVisibleReflectance 
        { 
            get => _frontSideSlatBeamVisibleReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _frontSideSlatBeamVisibleReflectance = value;
                }
            }
        }
        public double BackSideSlatBeamVisibleReflectance 
        { 
            get => _backSideSlatBeamVisibleReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _backSideSlatBeamVisibleReflectance = value;
                }
            }
        }

        public WindowMaterialBlind() { }

        private static List<WindowMaterialBlind> list = new List<WindowMaterialBlind>();

        public static void Add(WindowMaterialBlind windowMaterialBlind)
        {
            list.Add(windowMaterialBlind);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:Blind,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SlatOrientation}" + ",").PadRight(27, ' ') + " !-Slat Orientation\n" +
                    ($"  {list[i].SlatWidth}" + ",").PadRight(27, ' ') + " !-Slat Width{{ m }}\n" +
                    ($"  {list[i].SlatSeparation}" + ",").PadRight(27, ' ') + " !-Slat Separation{{ m }}\n" +
                    ($"  {list[i].SlatThickness}" + ",").PadRight(27, ' ') + " !-Slat Thickness{{ m }}\n" +
                    ($"  {list[i].SlatAngle}" + ",").PadRight(27, ' ') + " !-Slat Angle{{ deg }}\n" +
                    ($"  {list[i].SlatConductivity}" + ",").PadRight(27, ' ') + " !-Slat Conductivity {{ W / m-K}}\n" +
                    ($"  {list[i].SlatBeamSolarTransmittance}" + ",").PadRight(27, ' ') + " !-Slat Beam Solar Transmittance\n" +
                    ($"  {list[i].FrontSideSlatBeamSolarReflectance}" + ",").PadRight(27, ' ') + " !-Front Side Slat Beam Solar Reflectance\n" +
                    ($"  {list[i].BackSideSlatBeamSolarReflectance}" + ",").PadRight(27, ' ') + " !-Back Side Slat Beam Solar Reflectance\n" +
                    ($"  {list[i].SlatDiffuseSolarTransmittance}" + ",").PadRight(27, ' ') + " !-Slat Diffuse Solar Transmittance\n" +
                    ($"  {list[i].FrontSideSlatDiffuseSolarReflectance}" + ",").PadRight(27, ' ') + " !-Front Side Slat Diffuse Solar Reflectance\n" +
                    ($"  {list[i].BackSideSlatDiffuseSolarReflectance}" + ",").PadRight(27, ' ') + " !-Back Side Slat Diffuse Solar Reflectance\n" +
                    ($"  {list[i].SlatBeamVisibleTransmittance}" + ",").PadRight(27, ' ') + " !-Slat Beam Visible Transmittance\n" +
                    ($"  {list[i].FrontSideSlatBeamVisibleReflectance}" + ",").PadRight(27, ' ') + " !-Front Side Slat Beam Visible Reflectance\n" +
                    ($"  {list[i].BackSideSlatBeamVisibleReflectance}" + ",").PadRight(27, ' ') + " !-Back Side Slat Beam Visible Reflectance\n" +
                    ($"  {list[i].SlatDiffuseVisibleTransmittance}" + ",").PadRight(27, ' ') + " !-Slat Diffuse Visible Transmittance\n" +
                    ($"  {list[i].FrontSideSlatDiffuseVisibleReflectance}" + ",").PadRight(27, ' ') + " !-Front Side Slat Diffuse Visible Reflectance\n" +
                    ($"  {list[i].BackSideSlatDiffuseVisibleReflectance}" + ",").PadRight(27, ' ') + " !-Back Side Slat Diffuse Visible Reflectance\n" +
                    ($"  {list[i].SlatInfraredHemisphericalTransmittance}" + ",").PadRight(27, ' ') + " !-Slat Infrared Hemispherical Transmittance\n" +
                    ($"  {list[i].FrontSideSlatInfraretHemisphericalEmissivity}" + ",").PadRight(27, ' ') + " !-Front Side Slat Infraret Hemispherical Emissivity\n" +
                    ($"  {list[i].BackSideSlatInfraredHemisphericalEmissivity}" + ",").PadRight(27, ' ') + " !-Back Side Slat Infrared Hemispherical Emissivity\n" +
                    ($"  {list[i].BlindtoGlassDistance}" + ",").PadRight(27, ' ') + " !-Blind to Glass Distance{{ m }}\n" +
                    ($"  {list[i].BlindTopOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Blind Top Opening Multiplier\n" +
                    ($"  {list[i].BlindBottomOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Blind Bottom Opening Multiplier\n" +
                    ($"  {list[i].BlindLeftSideOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Blind Left Side Opening Multiplier\n" +
                    ($"  {list[i].BlindRightSideOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Blind Right Side Opening Multiplier\n" +
                    ($"  {list[i].MinimumSlatAngle}" + ",").PadRight(27, ' ') + " !-Minimum Slat Angle{{ deg }}\n" +
                    ($"  {list[i].MaximumSlatAngle}" + ";").PadRight(27, ' ') + " !-Maximum Slat Angle{{ deg }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialBlind.Read())
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
