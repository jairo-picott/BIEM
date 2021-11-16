using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public class WindowMaterialGlazingGroupThermochromic
    {
        private string _name;
        private double _opticalDataTemperature1;
        private string _windowMaterialGlazingName1;
        private double _opticalDataTemperature2;
        private string _windowMaterialGlazingName2;
        private double _opticalDataTemperature3;
        private string _windowMaterialGlazingName3;
        private double _opticalDataTemperature4;
        private string _windowMaterialGlazingName4;
        private double _opticalDataTemperature5;
        private string _windowMaterialGlazingName5;
        private double _opticalDataTemperature6;
        private string _windowMaterialGlazingName6;
        private double _opticalDataTemperature7;
        private string _windowMaterialGlazingName7;
        private double _opticalDataTemperature8;
        private string _windowMaterialGlazingName8;
        private double _opticalDataTemperature9;
        private string _windowMaterialGlazingName9;
        private double _opticalDataTemperature10;
        private string _windowMaterialGlazingName10;
        private double _opticalDataTemperature11;
        private string _windowMaterialGlazingName11;
        private double _opticalDataTemperature12;
        private string _windowMaterialGlazingName12;
        private double _opticalDataTemperature13;
        private string _windowMaterialGlazingName13;
        private double _opticalDataTemperature14;
        private string _windowMaterialGlazingName14;
        private double _opticalDataTemperature15;
        private string _windowMaterialGlazingName15;
        private double _opticalDataTemperature16;
        private string _windowMaterialGlazingName16;
        private double _opticalDataTemperature17;
        private string _windowMaterialGlazingName17;
        private double _opticalDataTemperature18;
        private string _windowMaterialGlazingName18;
        private double _opticalDataTemperature19;
        private string _windowMaterialGlazingName19;
        private double _opticalDataTemperature20;
        private string _windowMaterialGlazingName20;
        private double _opticalDataTemperature21;
        private string _windowMaterialGlazingName21;
        private double _opticalDataTemperature22;
        private string _windowMaterialGlazingName22;
        private double _opticalDataTemperature23;
        private string _windowMaterialGlazingName23;
        private double _opticalDataTemperature24;
        private string _windowMaterialGlazingName24;
        private double _opticalDataTemperature25;
        private string _windowMaterialGlazingName25;
        private double _opticalDataTemperature26;
        private string _windowMaterialGlazingName26;
        private double _opticalDataTemperature27;
        private string _windowMaterialGlazingName27;
        private double _opticalDataTemperature28;
        private string _windowMaterialGlazingName28;
        private double _opticalDataTemperature29;
        private string _windowMaterialGlazingName29;
        private double _opticalDataTemperature30;
        private string _windowMaterialGlazingName30;
        private double _opticalDataTemperature31;
        private string _windowMaterialGlazingName31;
        private double _opticalDataTemperature32;
        private string _windowMaterialGlazingName32;
        private double _opticalDataTemperature33;
        private string _windowMaterialGlazingName33;
        private double _opticalDataTemperature34;
        private string _windowMaterialGlazingName34;
        private double _opticalDataTemperature35;
        private string _windowMaterialGlazingName35;
        private double _opticalDataTemperature36;
        private string _windowMaterialGlazingName36;
        private double _opticalDataTemperature37;
        private string _windowMaterialGlazingName37;
        private double _opticalDataTemperature38;
        private string _windowMaterialGlazingName38;
        private double _opticalDataTemperature39;
        private string _windowMaterialGlazingName39;
        private double _opticalDataTemperature40;
        private string _windowMaterialGlazingName40;
        private double _opticalDataTemperature41;
        private string _windowMaterialGlazingName41;
        private double _opticalDataTemperature42;
        private string _windowMaterialGlazingName42;
        private double _opticalDataTemperature43;
        private string _windowMaterialGlazingName43;
        private double _opticalDataTemperature44;
        private string _windowMaterialGlazingName44;
        private double _opticalDataTemperature45;
        private string _windowMaterialGlazingName45;

        public string Name { get => _name; set => _name = value; }
        public double OpticalDataTemperature1 { get => _opticalDataTemperature1; set => _opticalDataTemperature1 = value; }
        public string WindowMaterialGlazingName1 { get => _windowMaterialGlazingName1; set => _windowMaterialGlazingName1 = value; }
        public double OpticalDataTemperature2 { get => _opticalDataTemperature2; set => _opticalDataTemperature2 = value; }
        public string WindowMaterialGlazingName2 { get => _windowMaterialGlazingName2; set => _windowMaterialGlazingName2 = value; }
        public double OpticalDataTemperature3 { get => _opticalDataTemperature3; set => _opticalDataTemperature3 = value; }
        public string WindowMaterialGlazingName3 { get => _windowMaterialGlazingName3; set => _windowMaterialGlazingName3 = value; }
        public double OpticalDataTemperature4 { get => _opticalDataTemperature4; set => _opticalDataTemperature4 = value; }
        public string WindowMaterialGlazingName4 { get => _windowMaterialGlazingName4; set => _windowMaterialGlazingName4 = value; }
        public double OpticalDataTemperature5 { get => _opticalDataTemperature5; set => _opticalDataTemperature5 = value; }
        public string WindowMaterialGlazingName5 { get => _windowMaterialGlazingName5; set => _windowMaterialGlazingName5 = value; }
        public double OpticalDataTemperature6 { get => _opticalDataTemperature6; set => _opticalDataTemperature6 = value; }
        public string WindowMaterialGlazingName6 { get => _windowMaterialGlazingName6; set => _windowMaterialGlazingName6 = value; }
        public double OpticalDataTemperature7 { get => _opticalDataTemperature7; set => _opticalDataTemperature7 = value; }
        public string WindowMaterialGlazingName7 { get => _windowMaterialGlazingName7; set => _windowMaterialGlazingName7 = value; }
        public double OpticalDataTemperature8 { get => _opticalDataTemperature8; set => _opticalDataTemperature8 = value; }
        public string WindowMaterialGlazingName8 { get => _windowMaterialGlazingName8; set => _windowMaterialGlazingName8 = value; }
        public double OpticalDataTemperature9 { get => _opticalDataTemperature9; set => _opticalDataTemperature9 = value; }
        public string WindowMaterialGlazingName9 { get => _windowMaterialGlazingName9; set => _windowMaterialGlazingName9 = value; }
        public double OpticalDataTemperature10 { get => _opticalDataTemperature10; set => _opticalDataTemperature10 = value; }
        public string WindowMaterialGlazingName10 { get => _windowMaterialGlazingName10; set => _windowMaterialGlazingName10 = value; }
        public double OpticalDataTemperature11 { get => _opticalDataTemperature11; set => _opticalDataTemperature11 = value; }
        public string WindowMaterialGlazingName11 { get => _windowMaterialGlazingName11; set => _windowMaterialGlazingName11 = value; }
        public double OpticalDataTemperature12 { get => _opticalDataTemperature12; set => _opticalDataTemperature12 = value; }
        public string WindowMaterialGlazingName12 { get => _windowMaterialGlazingName12; set => _windowMaterialGlazingName12 = value; }
        public double OpticalDataTemperature13 { get => _opticalDataTemperature13; set => _opticalDataTemperature13 = value; }
        public string WindowMaterialGlazingName13 { get => _windowMaterialGlazingName13; set => _windowMaterialGlazingName13 = value; }
        public double OpticalDataTemperature14 { get => _opticalDataTemperature14; set => _opticalDataTemperature14 = value; }
        public string WindowMaterialGlazingName14 { get => _windowMaterialGlazingName14; set => _windowMaterialGlazingName14 = value; }
        public double OpticalDataTemperature15 { get => _opticalDataTemperature15; set => _opticalDataTemperature15 = value; }
        public string WindowMaterialGlazingName15 { get => _windowMaterialGlazingName15; set => _windowMaterialGlazingName15 = value; }
        public double OpticalDataTemperature16 { get => _opticalDataTemperature16; set => _opticalDataTemperature16 = value; }
        public string WindowMaterialGlazingName16 { get => _windowMaterialGlazingName16; set => _windowMaterialGlazingName16 = value; }
        public double OpticalDataTemperature17 { get => _opticalDataTemperature17; set => _opticalDataTemperature17 = value; }
        public string WindowMaterialGlazingName17 { get => _windowMaterialGlazingName17; set => _windowMaterialGlazingName17 = value; }
        public double OpticalDataTemperature18 { get => _opticalDataTemperature18; set => _opticalDataTemperature18 = value; }
        public string WindowMaterialGlazingName18 { get => _windowMaterialGlazingName18; set => _windowMaterialGlazingName18 = value; }
        public double OpticalDataTemperature19 { get => _opticalDataTemperature19; set => _opticalDataTemperature19 = value; }
        public string WindowMaterialGlazingName19 { get => _windowMaterialGlazingName19; set => _windowMaterialGlazingName19 = value; }
        public double OpticalDataTemperature20 { get => _opticalDataTemperature20; set => _opticalDataTemperature20 = value; }
        public string WindowMaterialGlazingName20 { get => _windowMaterialGlazingName20; set => _windowMaterialGlazingName20 = value; }
        public double OpticalDataTemperature21 { get => _opticalDataTemperature21; set => _opticalDataTemperature21 = value; }
        public string WindowMaterialGlazingName21 { get => _windowMaterialGlazingName21; set => _windowMaterialGlazingName21 = value; }
        public double OpticalDataTemperature22 { get => _opticalDataTemperature22; set => _opticalDataTemperature22 = value; }
        public string WindowMaterialGlazingName22 { get => _windowMaterialGlazingName22; set => _windowMaterialGlazingName22 = value; }
        public double OpticalDataTemperature23 { get => _opticalDataTemperature23; set => _opticalDataTemperature23 = value; }
        public string WindowMaterialGlazingName23 { get => _windowMaterialGlazingName23; set => _windowMaterialGlazingName23 = value; }
        public double OpticalDataTemperature24 { get => _opticalDataTemperature24; set => _opticalDataTemperature24 = value; }
        public string WindowMaterialGlazingName24 { get => _windowMaterialGlazingName24; set => _windowMaterialGlazingName24 = value; }
        public double OpticalDataTemperature25 { get => _opticalDataTemperature25; set => _opticalDataTemperature25 = value; }
        public string WindowMaterialGlazingName25 { get => _windowMaterialGlazingName25; set => _windowMaterialGlazingName25 = value; }
        public double OpticalDataTemperature26 { get => _opticalDataTemperature26; set => _opticalDataTemperature26 = value; }
        public string WindowMaterialGlazingName26 { get => _windowMaterialGlazingName26; set => _windowMaterialGlazingName26 = value; }
        public double OpticalDataTemperature27 { get => _opticalDataTemperature27; set => _opticalDataTemperature27 = value; }
        public string WindowMaterialGlazingName27 { get => _windowMaterialGlazingName27; set => _windowMaterialGlazingName27 = value; }
        public double OpticalDataTemperature28 { get => _opticalDataTemperature28; set => _opticalDataTemperature28 = value; }
        public string WindowMaterialGlazingName28 { get => _windowMaterialGlazingName28; set => _windowMaterialGlazingName28 = value; }
        public double OpticalDataTemperature29 { get => _opticalDataTemperature29; set => _opticalDataTemperature29 = value; }
        public string WindowMaterialGlazingName29 { get => _windowMaterialGlazingName29; set => _windowMaterialGlazingName29 = value; }
        public double OpticalDataTemperature30 { get => _opticalDataTemperature30; set => _opticalDataTemperature30 = value; }
        public string WindowMaterialGlazingName30 { get => _windowMaterialGlazingName30; set => _windowMaterialGlazingName30 = value; }
        public double OpticalDataTemperature31 { get => _opticalDataTemperature31; set => _opticalDataTemperature31 = value; }
        public string WindowMaterialGlazingName31 { get => _windowMaterialGlazingName31; set => _windowMaterialGlazingName31 = value; }
        public double OpticalDataTemperature32 { get => _opticalDataTemperature32; set => _opticalDataTemperature32 = value; }
        public string WindowMaterialGlazingName32 { get => _windowMaterialGlazingName32; set => _windowMaterialGlazingName32 = value; }
        public double OpticalDataTemperature33 { get => _opticalDataTemperature33; set => _opticalDataTemperature33 = value; }
        public string WindowMaterialGlazingName33 { get => _windowMaterialGlazingName33; set => _windowMaterialGlazingName33 = value; }
        public double OpticalDataTemperature34 { get => _opticalDataTemperature34; set => _opticalDataTemperature34 = value; }
        public string WindowMaterialGlazingName34 { get => _windowMaterialGlazingName34; set => _windowMaterialGlazingName34 = value; }
        public double OpticalDataTemperature35 { get => _opticalDataTemperature35; set => _opticalDataTemperature35 = value; }
        public string WindowMaterialGlazingName35 { get => _windowMaterialGlazingName35; set => _windowMaterialGlazingName35 = value; }
        public double OpticalDataTemperature36 { get => _opticalDataTemperature36; set => _opticalDataTemperature36 = value; }
        public string WindowMaterialGlazingName36 { get => _windowMaterialGlazingName36; set => _windowMaterialGlazingName36 = value; }
        public double OpticalDataTemperature37 { get => _opticalDataTemperature37; set => _opticalDataTemperature37 = value; }
        public string WindowMaterialGlazingName37 { get => _windowMaterialGlazingName37; set => _windowMaterialGlazingName37 = value; }
        public double OpticalDataTemperature38 { get => _opticalDataTemperature38; set => _opticalDataTemperature38 = value; }
        public string WindowMaterialGlazingName38 { get => _windowMaterialGlazingName38; set => _windowMaterialGlazingName38 = value; }
        public double OpticalDataTemperature39 { get => _opticalDataTemperature39; set => _opticalDataTemperature39 = value; }
        public string WindowMaterialGlazingName39 { get => _windowMaterialGlazingName39; set => _windowMaterialGlazingName39 = value; }
        public double OpticalDataTemperature40 { get => _opticalDataTemperature40; set => _opticalDataTemperature40 = value; }
        public string WindowMaterialGlazingName40 { get => _windowMaterialGlazingName40; set => _windowMaterialGlazingName40 = value; }
        public double OpticalDataTemperature41 { get => _opticalDataTemperature41; set => _opticalDataTemperature41 = value; }
        public string WindowMaterialGlazingName41 { get => _windowMaterialGlazingName41; set => _windowMaterialGlazingName41 = value; }
        public double OpticalDataTemperature42 { get => _opticalDataTemperature42; set => _opticalDataTemperature42 = value; }
        public string WindowMaterialGlazingName42 { get => _windowMaterialGlazingName42; set => _windowMaterialGlazingName42 = value; }
        public double OpticalDataTemperature43 { get => _opticalDataTemperature43; set => _opticalDataTemperature43 = value; }
        public string WindowMaterialGlazingName43 { get => _windowMaterialGlazingName43; set => _windowMaterialGlazingName43 = value; }
        public double OpticalDataTemperature44 { get => _opticalDataTemperature44; set => _opticalDataTemperature44 = value; }
        public string WindowMaterialGlazingName44 { get => _windowMaterialGlazingName44; set => _windowMaterialGlazingName44 = value; }
        public double OpticalDataTemperature45 { get => _opticalDataTemperature45; set => _opticalDataTemperature45 = value; }
        public string WindowMaterialGlazingName45 { get => _windowMaterialGlazingName45; set => _windowMaterialGlazingName45 = value; }

        public WindowMaterialGlazingGroupThermochromic() { }

        private static List<WindowMaterialGlazingGroupThermochromic> list = new List<WindowMaterialGlazingGroupThermochromic>();

        public static void Add(WindowMaterialGlazingGroupThermochromic windowMaterialGlazingGroupThermochromic)
        {
            list.Add(windowMaterialGlazingGroupThermochromic);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"WindowMaterial:GlazingGroup:Thermochromic,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].OpticalDataTemperature1}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 1\n" +
                    ($"  {list[i].WindowMaterialGlazingName1}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 1\n" +
                    ($"  {list[i].OpticalDataTemperature2}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 2\n" +
                    ($"  {list[i].WindowMaterialGlazingName2}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 2\n" +
                    ($"  {list[i].OpticalDataTemperature3}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 3\n" +
                    ($"  {list[i].WindowMaterialGlazingName3}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 3\n" +
                    ($"  {list[i].OpticalDataTemperature4}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 4\n" +
                    ($"  {list[i].WindowMaterialGlazingName4}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 4\n" +
                    ($"  {list[i].OpticalDataTemperature5}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 5\n" +
                    ($"  {list[i].WindowMaterialGlazingName5}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 5\n" +
                    ($"  {list[i].OpticalDataTemperature6}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 6\n" +
                    ($"  {list[i].WindowMaterialGlazingName6}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 6\n" +
                    ($"  {list[i].OpticalDataTemperature7}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 7\n" +
                    ($"  {list[i].WindowMaterialGlazingName7}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 7\n" +
                    ($"  {list[i].OpticalDataTemperature8}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 8\n" +
                    ($"  {list[i].WindowMaterialGlazingName8}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 8\n" +
                    ($"  {list[i].OpticalDataTemperature9}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 9\n" +
                    ($"  {list[i].WindowMaterialGlazingName9}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 9\n" +
                    ($"  {list[i].OpticalDataTemperature10}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 10\n" +
                    ($"  {list[i].WindowMaterialGlazingName10}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 10\n" +
                    ($"  {list[i].OpticalDataTemperature11}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 11\n" +
                    ($"  {list[i].WindowMaterialGlazingName11}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 11\n" +
                    ($"  {list[i].OpticalDataTemperature12}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 12\n" +
                    ($"  {list[i].WindowMaterialGlazingName12}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 12\n" +
                    ($"  {list[i].OpticalDataTemperature13}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 13\n" +
                    ($"  {list[i].WindowMaterialGlazingName13}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 13\n" +
                    ($"  {list[i].OpticalDataTemperature14}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 14\n" +
                    ($"  {list[i].WindowMaterialGlazingName14}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 14\n" +
                    ($"  {list[i].OpticalDataTemperature15}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 15\n" +
                    ($"  {list[i].WindowMaterialGlazingName15}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 15\n" +
                    ($"  {list[i].OpticalDataTemperature16}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 16\n" +
                    ($"  {list[i].WindowMaterialGlazingName16}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 16\n" +
                    ($"  {list[i].OpticalDataTemperature17}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 17\n" +
                    ($"  {list[i].WindowMaterialGlazingName17}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 17\n" +
                    ($"  {list[i].OpticalDataTemperature18}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 18\n" +
                    ($"  {list[i].WindowMaterialGlazingName18}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 18\n" +
                    ($"  {list[i].OpticalDataTemperature19}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 19\n" +
                    ($"  {list[i].WindowMaterialGlazingName19}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 19\n" +
                    ($"  {list[i].OpticalDataTemperature20}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 20\n" +
                    ($"  {list[i].WindowMaterialGlazingName20}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 20\n" +
                    ($"  {list[i].OpticalDataTemperature21}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 21\n" +
                    ($"  {list[i].WindowMaterialGlazingName21}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 21\n" +
                    ($"  {list[i].OpticalDataTemperature22}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 22\n" +
                    ($"  {list[i].WindowMaterialGlazingName22}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 22\n" +
                    ($"  {list[i].OpticalDataTemperature23}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 23\n" +
                    ($"  {list[i].WindowMaterialGlazingName23}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 23\n" +
                    ($"  {list[i].OpticalDataTemperature24}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 24\n" +
                    ($"  {list[i].WindowMaterialGlazingName24}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 24\n" +
                    ($"  {list[i].OpticalDataTemperature25}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 25\n" +
                    ($"  {list[i].WindowMaterialGlazingName25}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 25\n" +
                    ($"  {list[i].OpticalDataTemperature26}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 26\n" +
                    ($"  {list[i].WindowMaterialGlazingName26}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 26\n" +
                    ($"  {list[i].OpticalDataTemperature27}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 27\n" +
                    ($"  {list[i].WindowMaterialGlazingName27}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 27\n" +
                    ($"  {list[i].OpticalDataTemperature28}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 28\n" +
                    ($"  {list[i].WindowMaterialGlazingName28}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 28\n" +
                    ($"  {list[i].OpticalDataTemperature29}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 29\n" +
                    ($"  {list[i].WindowMaterialGlazingName29}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 29\n" +
                    ($"  {list[i].OpticalDataTemperature30}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 30\n" +
                    ($"  {list[i].WindowMaterialGlazingName30}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 30\n" +
                    ($"  {list[i].OpticalDataTemperature31}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 31\n" +
                    ($"  {list[i].WindowMaterialGlazingName31}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 31\n" +
                    ($"  {list[i].OpticalDataTemperature32}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 32\n" +
                    ($"  {list[i].WindowMaterialGlazingName32}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 32\n" +
                    ($"  {list[i].OpticalDataTemperature33}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 33\n" +
                    ($"  {list[i].WindowMaterialGlazingName33}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 33\n" +
                    ($"  {list[i].OpticalDataTemperature34}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 34\n" +
                    ($"  {list[i].WindowMaterialGlazingName34}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 34\n" +
                    ($"  {list[i].OpticalDataTemperature35}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 35\n" +
                    ($"  {list[i].WindowMaterialGlazingName35}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 35\n" +
                    ($"  {list[i].OpticalDataTemperature36}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 36\n" +
                    ($"  {list[i].WindowMaterialGlazingName36}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 36\n" +
                    ($"  {list[i].OpticalDataTemperature37}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 37\n" +
                    ($"  {list[i].WindowMaterialGlazingName37}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 37\n" +
                    ($"  {list[i].OpticalDataTemperature38}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 38\n" +
                    ($"  {list[i].WindowMaterialGlazingName38}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 38\n" +
                    ($"  {list[i].OpticalDataTemperature39}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 39\n" +
                    ($"  {list[i].WindowMaterialGlazingName39}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 39\n" +
                    ($"  {list[i].OpticalDataTemperature40}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 40\n" +
                    ($"  {list[i].WindowMaterialGlazingName40}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 40\n" +
                    ($"  {list[i].OpticalDataTemperature41}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 41\n" +
                    ($"  {list[i].WindowMaterialGlazingName41}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 41\n" +
                    ($"  {list[i].OpticalDataTemperature42}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 42\n" +
                    ($"  {list[i].WindowMaterialGlazingName42}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 42\n" +
                    ($"  {list[i].OpticalDataTemperature43}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 43\n" +
                    ($"  {list[i].WindowMaterialGlazingName43}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 43\n" +
                    ($"  {list[i].OpticalDataTemperature44}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 44\n" +
                    ($"  {list[i].WindowMaterialGlazingName44}" + ",").PadRight(27, ' ') + " !-Window Material Glazing Name 44\n" +
                    ($"  {list[i].OpticalDataTemperature45}" + ",").PadRight(27, ' ') + " !-Optical Data Temperature 45\n" +
                    ($"  {list[i].WindowMaterialGlazingName45}" + ";").PadRight(27, ' ') + " !-Window Material Glazing Name 45";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in WindowMaterialGlazingGroupThermochromic.Read())
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
