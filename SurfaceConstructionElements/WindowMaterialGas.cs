using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public enum GasTypeEnum
    {
        Air,
        Argon,
        Krypton,
        Xenon,
        Custom
    }
    public class WindowMaterialGas
    {
        private string _name;
        private GasTypeEnum _gasType;
        private double _thickness;
        private double _conductivityCoefficientA;
        private double _conductivityCoefficientB;
        private double _condcutivityCoefficientC;
        private double _viscocityCoefficientA;
        private double _viscocityCoefficientB;
        private double _viscocityCoefficientC;
        private double _specificHeatCoefficientA;
        private double _specificHeatCoefficientB;
        private double _specificHeatCoefficientC;
        private double _molecularWeight;
        private double _specificHeatRatio;

        public string Name { get => _name; set => _name = value; }
        public GasTypeEnum GasType { get => _gasType; set => _gasType = value; }
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
        public double ConductivityCoefficientA { get => _conductivityCoefficientA; set => _conductivityCoefficientA = value; }
        public double ConductivityCoefficientB { get => _conductivityCoefficientB; set => _conductivityCoefficientB = value; }
        public double CondcutivityCoefficientC { get => _condcutivityCoefficientC; set => _condcutivityCoefficientC = value; }
        public double ViscocityCoefficientA 
        { 
            get => _viscocityCoefficientA;
            set
            {
                if (value > 0)
                {
                    _viscocityCoefficientA = value;
                }
            }
        }
        public double ViscocityCoefficientB { get => _viscocityCoefficientB; set => _viscocityCoefficientB = value; }
        public double ViscocityCoefficientC { get => _viscocityCoefficientC; set => _viscocityCoefficientC = value; }
        public double SpecificHeatCoefficientA 
        { 
            get => _specificHeatCoefficientA;
            set
            {
                if (value > 0)
                {
                    _specificHeatCoefficientA = value;
                }
            }
        }
        public double SpecificHeatCoefficientB { get => _specificHeatCoefficientB; set => _specificHeatCoefficientB = value; }
        public double SpecificHeatCoefficientC { get => _specificHeatCoefficientC; set => _specificHeatCoefficientC = value; }
        public double MolecularWeight 
        { 
            get => _molecularWeight;
            set
            {
                if (value >= 20 && value <=200)
                {
                    _molecularWeight = value;
                }
            }
        }
        public double SpecificHeatRatio 
        { 
            get => _specificHeatRatio;
            set
            {
                if (value > 1)
                {
                    _specificHeatRatio = value;
                }
            }
        }

        public WindowMaterialGas() { }

        private static List<WindowMaterialGas> list = new List<WindowMaterialGas>();

        public static void Add(WindowMaterialGas windowMaterialGas)
        {
            list.Add(windowMaterialGas);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:Gas,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].GasType}" + ",").PadRight(27, ' ') + " !-Gas Type\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].ConductivityCoefficientA}" + ",").PadRight(27, ' ') + " !-Conductivity Coefficient A{{ W / m-K }}\n" +
                    ($"  {list[i].ConductivityCoefficientB}" + ",").PadRight(27, ' ') + " !-Conductivity Coefficient B{{ W / m-K2 }}\n" +
                    ($"  {list[i].CondcutivityCoefficientC}" + ",").PadRight(27, ' ') + " !-Condcutivity Coefficient C{{ W / m-K3 }}\n" +
                    ($"  {list[i].ViscocityCoefficientA}" + ",").PadRight(27, ' ') + " !-Viscocity Coefficient A {{ kg / m-s }}\n" +
                    ($"  {list[i].ViscocityCoefficientB}" + ",").PadRight(27, ' ') + " !-Viscocity Coefficient B {{ kg / m-s2 }}\n" +
                    ($"  {list[i].ViscocityCoefficientC}" + ",").PadRight(27, ' ') + " !-Viscocity Coefficient C {{ kg / m-s3 }}\n" +
                    ($"  {list[i].SpecificHeatCoefficientA}" + ",").PadRight(27, ' ') + " !-Specific Heat Coefficient A {{ J / kg-K }}\n" +
                    ($"  {list[i].SpecificHeatCoefficientB}" + ",").PadRight(27, ' ') + " !-Specific Heat Coefficient B {{ J / kg-K2 }}\n" +
                    ($"  {list[i].SpecificHeatCoefficientC}" + ",").PadRight(27, ' ') + " !-Specific Heat Coefficient C {{ J / kg-K3 }}\n" +
                    ($"  {list[i].MolecularWeight}" + ",").PadRight(27, ' ') + " !-Molecular Weight {{ g / mol }}\n" +
                    ($"  {list[i].SpecificHeatRatio}" + ";").PadRight(27, ' ') + " !-Specific Heat Ratio";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialGas.Read())
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
