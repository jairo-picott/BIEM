using System.IO;
using System.Collections.Generic;
using System;

namespace BIEM.SurfaceConstructionElements
{
    //-----------------Construction
    //COLLECT IFCELEMENTASSEMBLY AND ITS ELEMENTS, TO BUILT THE SETS
    //FOR CONSTRUCTION CLASS IN ENERGYPLUS, AS WELL AS IFCWINDOW AND 
    //IFC DOOR
    public class Construction
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private string _outsideLayer;
        public string OutsideLayer
        {
            get => _outsideLayer;
            set => _outsideLayer = value;
        }

        private string _layer2;
        public string Layer2
        {
            get => _layer2;
            set => _layer2 = value;
        }

        private string _layer3;
        public string Layer3
        {
            get => _layer3;
            set => _layer3 = value;
        }

        private string _layer4;
        public string Layer4
        {
            get => _layer4;
            set => _layer4 = value;
        }

        private string _layer5;
        public string Layer5
        {
            get => _layer5;
            set => _layer5 = value;
        }

        private string _layer6;
        public string Layer6
        {
            get => _layer6;
            set => _layer6 = value;
        }

        private string _layer7;
        public string Layer7
        {
            get => _layer7;
            set => _layer7 = value;
        }

        private string _layer8;
        public string Layer8
        {
            get => _layer8;
            set => _layer8 = value;
        }

        private string _layer9;
        public string Layer9
        {
            get => _layer9;
            set => _layer9 = value;
        }

        private string _layer10;
        public string Layer10
        {
            get => _layer10;
            set => _layer10 = value;
        }

        public Construction() { }

        private static List<Construction> list = new List<Construction>();

        public static void Add(Construction construction)
        {
            var alreadyExist = list.Find(x => x.Name == construction.Name);
            if (alreadyExist == null)
            {
                list.Add(construction);
            }
        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Construction,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].OutsideLayer}" + ",").PadRight(27, ' ') + " !-Outside Layer\n" +
                    ($"  {list[i].Layer2}" + ",").PadRight(27, ' ') + " !-Layer 2\n" +
                    ($"  {list[i].Layer3}" + ",").PadRight(27, ' ') + " !-Layer 3\n" +
                    ($"  {list[i].Layer4}" + ",").PadRight(27, ' ') + " !-Layer 4\n" +
                    ($"  {list[i].Layer5}" + ",").PadRight(27, ' ') + " !-Layer 5\n" +
                    ($"  {list[i].Layer6}" + ",").PadRight(27, ' ') + " !-Layer 6\n" +
                    ($"  {list[i].Layer7}" + ",").PadRight(27, ' ') + " !-Layer 7\n" +
                    ($"  {list[i].Layer8}" + ",").PadRight(27, ' ') + " !-Layer 8\n" +
                    ($"  {list[i].Layer9}" + ",").PadRight(27, ' ') + " !-Layer 9\n" +
                    ($"  {list[i].Layer10}" + ";").PadRight(27, ' ') + " !-Layer 10";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in Construction.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {
            
            foreach (string line in Construction.Read())
            {
                Console.WriteLine(line);
            }
            

        }
        public static void Clear()
        {
            list.Clear();
        }
    }
}
