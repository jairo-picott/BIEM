using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public enum LayerTypeEnum
    {
        OtherShadingType,
        VenetianHorizontal,
        VenetianVertical,
        Woven,
        Perforated,
        BSDF
    }
    public class WindowMaterialComplexShade
    {
        private string _name;
        private LayerTypeEnum _layerType = LayerTypeEnum.OtherShadingType;
        private double _thickness = 0.002;
        private double _conductivity = 1;
        private double _iRTransmittance;
        private double _frontEmissivity = 0.84;
        private double _backEmissivity = 0.84;
        private double _topOpeningMultiplier;
        private double _bottomOpeningMultiplier;
        private double _leftSideOpeningMultiplier;
        private double _rightSideOpeningMultiplier;
        private double _frontOpeningMultiplier = 0.05;
        private double _slatWidth = 0.016;
        private double _slatSpacing = 0.012;
        private double _slatThickness = 0.0006;
        private double _slatAngle = 90;
        private double _slatConductivity = 160;
        private double _slatCurve;

        public string Name { get => _name; set => _name = value; }
        public LayerTypeEnum LayerType { get => _layerType; set => _layerType = value; }
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
        public double Conductivity 
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
        public double IRTransmittance 
        { 
            get => _iRTransmittance;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _iRTransmittance = value;
                }
            }
        }
        public double FrontEmissivity 
        { 
            get => _frontEmissivity;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _frontEmissivity = value;
                }
            }
        }
        public double BackEmissivity 
        { 
            get => _backEmissivity;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _backEmissivity = value;
                }
            }
        }
        public double TopOpeningMultiplier 
        { 
            get => _topOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _topOpeningMultiplier = value;
                }
            }
        }
        public double BottomOpeningMultiplier 
        { 
            get => _bottomOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _bottomOpeningMultiplier = value;
                }
            }
        }
        public double LeftSideOpeningMultiplier 
        { 
            get => _leftSideOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _leftSideOpeningMultiplier = value;
                }
            }
        }
        public double RightSideOpeningMultiplier 
        { 
            get => _rightSideOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _rightSideOpeningMultiplier = value;
                }
            }
        }
        public double FrontOpeningMultiplier 
        { 
            get => _frontOpeningMultiplier;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _frontOpeningMultiplier= value;
                }
            }
        }
        public double SlatWidth 
        { 
            get => _slatWidth;
            set
            {
                if (value > 0)
                {
                    _slatWidth = value;
                }
            }
        }
        public double SlatSpacing 
        { 
            get => _slatSpacing;
            set
            {
                if (value > 0)
                {
                    _slatSpacing = value;
                }
            }
        }
        public double SlatThickness 
        { 
            get => _slatThickness;
            set
            {
                if (value > 0)
                {
                    _slatThickness = value;
                }
            }
        }
        public double SlatAngle 
        { 
            get => _slatAngle;
            set
            {
                if (value >= -90 && value <= 90)
                {
                    _slatAngle = value;
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
        public double SlatCurve 
        { 
            get => _slatCurve;
            set
            {
                if (value >= 0)
                {
                    _slatCurve = value;
                }
            }
        }

        public WindowMaterialComplexShade() { }

        private static List<WindowMaterialComplexShade> list = new List<WindowMaterialComplexShade>();

        public static void Add(WindowMaterialComplexShade windowMaterialComplexShade)
        {
            list.Add(windowMaterialComplexShade);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:ComplexShade,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].LayerType}" + ",").PadRight(27, ' ') + " !-Layer Type\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].Conductivity}" + ",").PadRight(27, ' ') + " !-Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].IRTransmittance}" + ",").PadRight(27, ' ') + " !-IR Transmittance\n" +
                    ($"  {list[i].FrontEmissivity}" + ",").PadRight(27, ' ') + " !-Front Emissivity\n" +
                    ($"  {list[i].BackEmissivity}" + ",").PadRight(27, ' ') + " !-Back Emissivity\n" +
                    ($"  {list[i].TopOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-TopOpening Multiplier\n" +
                    ($"  {list[i].BottomOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Bottom Opening Multiplier\n" +
                    ($"  {list[i].LeftSideOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Left Side Opening Multiplier\n" +
                    ($"  {list[i].RightSideOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Right Side Opening Multiplier\n" +
                    ($"  {list[i].FrontOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Front Opening Multiplier\n" +
                    ($"  {list[i].SlatWidth}" + ",").PadRight(27, ' ') + " !-Slat Width{{ m }}\n" +
                    ($"  {list[i].SlatSpacing}" + ",").PadRight(27, ' ') + " !-Slat Spacing{{ m }}\n" +
                    ($"  {list[i].SlatThickness}" + ",").PadRight(27, ' ') + " !-Slat Thickness{{ m }}\n" +
                    ($"  {list[i].SlatAngle}" + ",").PadRight(27, ' ') + " !-Slat Angle{{ deg }}\n" +
                    ($"  {list[i].SlatConductivity}" + ",").PadRight(27, ' ') + " !-Slat Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].SlatCurve}" + ";").PadRight(27, ' ') + " !-Slat Curve{{ m }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialComplexShade.Read())
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
