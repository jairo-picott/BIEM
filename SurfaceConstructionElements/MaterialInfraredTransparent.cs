using System.Collections.Generic;
using System.IO;

namespace BIEM.SurfaceConstructionElements
{
    public class MaterialInfraredTransparent
    {
        private string _name;

        public string Name { get => _name; set => _name = value; }

        public MaterialInfraredTransparent() { }

        private static List<MaterialInfraredTransparent> list = new List<MaterialInfraredTransparent>();

        public static void Add(MaterialInfraredTransparent materialInfraredTransparent)
        {
            list.Add(materialInfraredTransparent);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Material:InfraredTransparent,\n" +
                    ($"  {list[i].Name}" + ";").PadRight(27, ' ') + " !-Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in MaterialInfraredTransparent.Read())
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
