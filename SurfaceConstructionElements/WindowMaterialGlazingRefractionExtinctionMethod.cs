using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public enum SolarDiffusingEnum
    {
        Yes,
        No
    }
    public class WindowMaterialGlazingRefractionExtinctionMethod
    {
        private string _name;
        private double _thickness;
        private double _solarIndexofRefraction;
        private double _solarExtinctionCoefficient;
        private double _visibleIndexofRefraction;
        private double _visibleExtinctionCoefficient;
        private double _InfraredTransmittanceatNormalIncidence;
        private double _infraredHemisphericalEmissivity = 0.84;
        private double _conductivity = 0.9;
        private double _dirtCorrectionFactorforSolarandVisibleTransmittance = 1;
        private SolarDiffusingEnum _solarDiffusing = SolarDiffusingEnum.No;

        public string Name { get => _name; set => _name = value; }
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
        public double SolarIndexofRefraction 
        { 
            get => _solarIndexofRefraction;
            set
            {
                if (value > 1)
                {
                    _solarIndexofRefraction= value;

                }
            }
        }
        public double SolarExtinctionCoefficient 
        { 
            get => _solarExtinctionCoefficient;
            set
            {
                if (value > 0)
                {
                    _solarExtinctionCoefficient= value;

                }
            }
        }
        public double VisibleIndexofRefraction 
        { 
            get => _visibleIndexofRefraction;
            set
            {
                if (value > 1)
                {
                    _visibleIndexofRefraction= value;

                }
            }
        }
        public double VisibleExtinctionCoefficient 
        { 
            get => _visibleExtinctionCoefficient;
            set
            {
                if (value > 0)
                {
                    _visibleExtinctionCoefficient= value;

                }
            }
        }
        public double InfraredTransmittanceatNormalIncidence 
        { 
            get => _InfraredTransmittanceatNormalIncidence;
            set
            {
                if (value >= 0 && value < 1)
                {
                    _InfraredTransmittanceatNormalIncidence= value;

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
        public double DirtCorrectionFactorforSolarandVisibleTransmittance 
        { 
            get => _dirtCorrectionFactorforSolarandVisibleTransmittance;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _dirtCorrectionFactorforSolarandVisibleTransmittance = value;

                }
            }
        }
        public SolarDiffusingEnum SolarDiffusing { get => _solarDiffusing; set => _solarDiffusing = value; }
        public double InfraredHemisphericalEmissivity 
        { 
            get => _infraredHemisphericalEmissivity;
            set
            {
                if (value > 0 && value < 1)
                {
                    _infraredHemisphericalEmissivity= value;

                }
            }
        }

        public WindowMaterialGlazingRefractionExtinctionMethod() { }

        private static List<WindowMaterialGlazingRefractionExtinctionMethod> list = new List<WindowMaterialGlazingRefractionExtinctionMethod>();

        public static void Add(WindowMaterialGlazingRefractionExtinctionMethod windowMaterialGlazingRefractionExtinctionMethod)
        {
            list.Add(windowMaterialGlazingRefractionExtinctionMethod);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:Glazing:RefractionExtinctionMethod,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].SolarIndexofRefraction}" + ",").PadRight(27, ' ') + " !-Solar Index of Refraction\n" +
                    ($"  {list[i].SolarExtinctionCoefficient}" + ",").PadRight(27, ' ') + " !-Solar Extinction Coefficient{{ 1 / m }}\n" +
                    ($"  {list[i].VisibleIndexofRefraction}" + ",").PadRight(27, ' ') + " !-Visible Index of Refraction\n" +
                    ($"  {list[i].VisibleExtinctionCoefficient}" + ",").PadRight(27, ' ') + " !-Visible Extinction Coefficient {{ 1 / m}}\n" +
                    ($"  {list[i].InfraredTransmittanceatNormalIncidence}" + ",").PadRight(27, ' ') + " !-Infrared Transmittance at Normal Incidence\n" +
                    ($"  {list[i].InfraredHemisphericalEmissivity}" + ",").PadRight(27, ' ') + " !-Infrared Hemispherical Emissivity\n" +
                    ($"  {list[i].Conductivity}" + ",").PadRight(27, ' ') + " !-Conductivity{{ W/m-K }}\n" +
                    ($"  {list[i].DirtCorrectionFactorforSolarandVisibleTransmittance}" + ",").PadRight(27, ' ') + " !-Dirt Correction Factor for Solar and Visible Transmittance\n" +
                    ($"  {list[i].SolarDiffusing}" + ";").PadRight(27, ' ') + " !-Solar Diffusing";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialGlazingRefractionExtinctionMethod.Read())
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
