using System.IO;
using System.Collections.Generic;

namespace BIEM.SimulationParameters
{
    public enum SurfaceConvectionAlgorithmInsideType
    {
        Simple,
        TARP,
        CeilingDiffuser,
        AdaptiveConvectionAlgorithm,
        ASTMC1340

    }
    public class SurfaceConvectionAlgorithmInside
    {
        private SurfaceConvectionAlgorithmInsideType? _algorithm;
        public SurfaceConvectionAlgorithmInsideType? Algorithm
        {
            get => _algorithm;
            set => _algorithm = value;
        }
        public SurfaceConvectionAlgorithmInside() { }

        private static List<SurfaceConvectionAlgorithmInside> list = new List<SurfaceConvectionAlgorithmInside>();

        public static void Add(SurfaceConvectionAlgorithmInside surfaceConvectionAlgorithmInside)
        {
            list.Add(surfaceConvectionAlgorithmInside);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"SurfaceConvectionAlgorithm:Inside,\n" +
                    ($"  {list[i].Algorithm}" + ";").PadRight(27, ' ') + " !-Algorithm";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SurfaceConvectionAlgorithmInside.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(SimulationParameters.SurfaceConvectionAlgorithmInside surface)
        {
            surface.Algorithm = null;
        }

    }
}
