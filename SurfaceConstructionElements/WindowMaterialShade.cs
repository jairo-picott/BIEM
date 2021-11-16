using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public class WindowMaterialShade
    {
        private string _name;
        private double _solarTransmittance;
        private double _solarReflectance;
        private double _visibleTransmittance;
        private double _visibleReflectance;
        private double _infraredHemisphericalEmissivity;
        private double _infraredTransmittance;
        private double _thickness;
        private double _conductivity;
        private double _shadetoGlassDistance = 0.05;
        private double _topOpeningMultiplier = 0.5;
        private double _bottomOpeningMultiplier = 0.5;
        private double _leftSideOpeningMultiplier = 0.5;
        private double _rightSideOpeningMultiplier = 0.5;
        private double _airflowPermeability;

        public string Name { get => _name; set => _name = value; }
        public double SolarTransmittance 
        { 
            get => _solarTransmittance; 
            set
            {
                if (value >= 0 && value < 1)
                {
                    _solarTransmittance = value;
                }
            }
        }
        public double SolarReflectance 
        { 
            get => _solarReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _solarReflectance = value;
                }
            }
        }
        public double VisibleTransmittance 
        { 
            get => _visibleTransmittance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _visibleTransmittance = value;
                }
            }
        }
        public double VisibleReflectance 
        { 
            get => _visibleReflectance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _visibleReflectance = value;
                }
            }
        }
        public double InfraredTransmittance 
        { 
            get => _infraredTransmittance;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _infraredTransmittance = value;
                }
            }
        }
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
        public double ShadetoGlassDistance 
        { 
            get => _shadetoGlassDistance;
            set
            {
                if (value >= 0.001 && value <= 1)
                {
                    _shadetoGlassDistance = value;
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
        public double AirflowPermeability { get => _airflowPermeability; set => _airflowPermeability = value; }
        public double InfraredHemisphericalEmissivity 
        { 
            get => _infraredHemisphericalEmissivity;
            set
            {
                if (value > 0 && value < 1)
                {
                    _infraredHemisphericalEmissivity = value;
                }
            }
        }

        public WindowMaterialShade() { }

        private static List<WindowMaterialShade> list = new List<WindowMaterialShade>();

        public static void Add(WindowMaterialShade windowMaterialShade)
        {
            list.Add(windowMaterialShade);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:Shade,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SolarTransmittance}" + ",").PadRight(27, ' ') + " !-Solar Transmittance\n" +
                    ($"  {list[i].SolarReflectance}" + ",").PadRight(27, ' ') + " !-Solar Reflectance\n" +
                    ($"  {list[i].VisibleTransmittance}" + ",").PadRight(27, ' ') + " !-Visible Transmittance\n" +
                    ($"  {list[i].VisibleReflectance}" + ",").PadRight(27, ' ') + " !-Visible Reflectance\n" +
                    ($"  {list[i].InfraredHemisphericalEmissivity}" + ",").PadRight(27, ' ') + " !-Infrared Hemispherical Emissivity\n" +
                    ($"  {list[i].InfraredTransmittance}" + ",").PadRight(27, ' ') + " !-Infrared Transmittance\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].Conductivity}" + ",").PadRight(27, ' ') + " !-Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].ShadetoGlassDistance}" + ",").PadRight(27, ' ') + " !-Shade to Glass Distance\n" +
                    ($"  {list[i].TopOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Top Opening Multiplier\n" +
                    ($"  {list[i].BottomOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Bottom Opening Multiplier\n" +
                    ($"  {list[i].LeftSideOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Left Side Opening Multiplier\n" +
                    ($"  {list[i].RightSideOpeningMultiplier}" + ",").PadRight(27, ' ') + " !-Right Side Opening Multiplier\n" +
                    ($"  {list[i].AirflowPermeability}" + ";").PadRight(27, ' ') + " !-Airflow Permeability";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialShade.Read())
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
