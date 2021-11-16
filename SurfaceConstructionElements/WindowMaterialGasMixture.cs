using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{

    public class WindowMaterialGasMixture
    {
        private string _name;
        private double _thickness;
        private double _numberofGasesinMixture;
        private GasTypeEnum _gas1Type;
        private double _gas1Fraction;
        private GasTypeEnum _gas2Type;
        private double _gas2Fraction;
        private GasTypeEnum _gas3Type;
        private double _gas3Fraction;
        private GasTypeEnum _gas4Type;
        private double _gas4Fraction;

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
        public double NumberofGasesinMixture 
        { 
            get => _numberofGasesinMixture;
            set
            {
                if (value >= 1 && value <= 4)
                {
                    _numberofGasesinMixture = value;

                }
            }
        }
        public GasTypeEnum Gas1Type { get => _gas1Type; set => _gas1Type = value; }
        public double Gas1Fraction 
        { 
            get => _gas1Fraction;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _gas1Fraction= value;

                }
            }
        }
        public GasTypeEnum Gas2Type { get => _gas2Type; set => _gas2Type = value; }
        public double Gas2Fraction
        {
            get => _gas2Fraction;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _gas2Fraction = value;

                }
            }
        }
        public GasTypeEnum Gas3Type { get => _gas3Type; set => _gas3Type = value; }
        public double Gas3Fraction
        {
            get => _gas3Fraction;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _gas3Fraction = value;

                }
            }
        }
        public GasTypeEnum Gas4Type { get => _gas4Type; set => _gas4Type = value; }
        public double Gas4Fraction
        {
            get => _gas4Fraction;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _gas4Fraction = value;

                }
            }
        }

        public WindowMaterialGasMixture() { }

        private static List<WindowMaterialGasMixture> list = new List<WindowMaterialGasMixture>();

        public static void Add(WindowMaterialGasMixture windowMaterialGasMixture)
        {
            list.Add(windowMaterialGasMixture);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:GasMixture,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].Thickness}" + ",").PadRight(27, ' ') + " !-Thickness{{ m }}\n" +
                    ($"  {list[i].NumberofGasesinMixture}" + ",").PadRight(27, ' ') + " !-Number of Gases in Mixture\n" +
                    ($"  {list[i].Gas1Type}" + ",").PadRight(27, ' ') + " !-Gas 1 Type\n" +
                    ($"  {list[i].Gas1Fraction}" + ",").PadRight(27, ' ') + " !-Gas 1 Fraction\n" +
                    ($"  {list[i].Gas2Type}" + ",").PadRight(27, ' ') + " !-Gas 2 Type\n" +
                    ($"  {list[i].Gas2Fraction}" + ",").PadRight(27, ' ') + " !-Gas 2 Fraction\n" +
                    ($"  {list[i].Gas3Type}" + ",").PadRight(27, ' ') + " !-Gas 2 Type\n" +
                    ($"  {list[i].Gas3Fraction}" + ",").PadRight(27, ' ') + " !-Gas 3 Fraction\n" +
                    ($"  {list[i].Gas4Type}" + ",").PadRight(27, ' ') + " !-Gas 2 Type\n" +
                    ($"  {list[i].Gas4Fraction}" + ";").PadRight(27, ' ') + " !-Gas 4 Fraction";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialGasMixture.Read())
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
