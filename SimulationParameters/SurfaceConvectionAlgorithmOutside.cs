using System.IO;
using System.Collections.Generic;

namespace BIEM.SimulationParameters
{
    public enum SurfaceConvectionAlgorithmOutsideType
    {
        SimpleCombined,
        DOE2,
        TARP,
        MoWiTT,
        AdaptiveConvectionAlgorithm
    }
    public class SurfaceConvectionAlgorithmOutside
    {
        private SurfaceConvectionAlgorithmOutsideType? _algorithm;
        public SurfaceConvectionAlgorithmOutsideType? Algorithm
        {
            get => _algorithm;
            set => _algorithm = value;
        }

        public SurfaceConvectionAlgorithmOutside() { }

        private static List<SurfaceConvectionAlgorithmOutside> list = new List<SurfaceConvectionAlgorithmOutside>();

        public static void Add(SurfaceConvectionAlgorithmOutside surfaceConvectionAlgorithmOutside)
        {
            list.Add(surfaceConvectionAlgorithmOutside);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"SurfaceConvectionAlgorithm:Outside,\n" +
                    $"  {list[i].Algorithm};          !-Algorithm";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SurfaceConvectionAlgorithmOutside.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(SimulationParameters.SurfaceConvectionAlgorithmOutside surface)
        {
            surface.Algorithm = null;
        }

    }
}
