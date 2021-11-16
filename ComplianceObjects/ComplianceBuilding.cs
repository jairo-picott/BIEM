using System.Collections.Generic;
using System.IO;

namespace BIEM.ComplianceObjects
{
    public class ComplianceBuilding
    {
        private double _buildingRotationforAppendixG;

        public double BuildingRotationforAppendixG { get => _buildingRotationforAppendixG; set => _buildingRotationforAppendixG = value; }

        public ComplianceBuilding() { }

        private static List<ComplianceBuilding> list = new List<ComplianceBuilding>();

        public static void Add(ComplianceBuilding complianceBuilding)
        {
            list.Add(complianceBuilding);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Compliance:Building,\n" +
                    ($"  {list[i].BuildingRotationforAppendixG}" + ";").PadRight(27, ' ') + " !-Building Rotation for Appendix G {{ deg }} ";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ComplianceBuilding.Read())
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
