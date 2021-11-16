using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{
    public enum ReportTypeOutputSurfacesListEnum
    {
        Details,
        Vertices,
        DetailsWithVertices,
        ViewFactorInfo,
        Lines,
        CostInfo,
        DecayCurvesFromComponentLoadsSummary
    }
    public enum ReportSpecificationsEnum
    {
        IDF
    }
    public class OutputSurfacesList
    {
        private ReportTypeOutputSurfacesListEnum? _reportType;
        private ReportSpecificationsEnum? _reportSpecifications;

        public ReportTypeOutputSurfacesListEnum? ReportType { get => _reportType; set => _reportType = value; }
        public ReportSpecificationsEnum? ReportSpecifications { get => _reportSpecifications; set => _reportSpecifications = value; }

        public OutputSurfacesList() { }

        private static List<OutputSurfacesList> list = new List<OutputSurfacesList>();

        public static void Add(OutputSurfacesList outputSurfacesList)
        {
            list.Add(outputSurfacesList);

            Errors er = new Errors();
            if (outputSurfacesList.ReportType == null)
            {
                er.Class = "Output:SurfacesList";
                er.Field = "ReportType";
                Errors.Add(er);
            }
        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:SurfacesList,\n" +
                    ($"  {list[i].ReportType}" + ",").PadRight(27, ' ') + " !-Report Type\n" +
                    ($"  {list[i].ReportSpecifications}" + ";").PadRight(27, ' ') + " !-Report Specifications";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputSurfacesList.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputSurfacesList.Read())
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
