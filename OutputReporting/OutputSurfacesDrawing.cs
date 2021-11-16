using System.IO;
using System.Collections.Generic;

namespace BIEM.OutputReporting
{
    public enum ReportSpecifications1Type
    {
        Triangulate3DFace,
        ThickPolyline,
        RegularPolyline
    }

    public enum ReportType
    {
        DXF,
        VRML
    }
    public class OutputSurfaceDrawing
    {
        private ReportType? _reportType;
        public ReportType? ReporType
        {
            get => _reportType;
            set => _reportType = value;
        }

        private ReportSpecifications1Type? _reportSpecifications1;
        public ReportSpecifications1Type? ReportSpecifications1
        {
            get => _reportSpecifications1;
            set => _reportSpecifications1 = value;
        }

        private string _reportSpecifications2;
        public string ReportSpecification2
        {
            get => _reportSpecifications2;
            set => _reportSpecifications2 = value;
        }

        public OutputSurfaceDrawing() { }

        private static List<OutputSurfaceDrawing> list = new List<OutputSurfaceDrawing>();

        public static void Add(OutputSurfaceDrawing outputSurfaceDrawing)
        {
            list.Add(outputSurfaceDrawing);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:Surfaces:Drawing,\n" +
                    ($"  {list[i].ReporType}" + ",").PadRight(27, ' ') + " !-Report Type\n" +
                    ($"  {list[i].ReportSpecifications1}" + ",").PadRight(27, ' ') + " !-Report Specifications 1\n" +
                    ($"  {list[i].ReportSpecification2}" + ";").PadRight(27, ' ') + " !-Report Specifications 2";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputSurfaceDrawing.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(OutputSurfaceDrawing outputSurfaceDrawing)
        {
            outputSurfaceDrawing._reportType = null;
            outputSurfaceDrawing._reportSpecifications1 = null;
            outputSurfaceDrawing._reportType = null;
           
        }
    }
}
