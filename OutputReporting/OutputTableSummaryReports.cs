using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{
    //todo: Complete de list of report names
    public enum ReportNameEnum
    {
        AnnualBuildingUtilityPerformanceSummary,
        InputVerificationResultsSummary,
        ClimaticDataSummary,
        EnvelopeSummary
    }
    public class OutputTableSummaryReports
    {
        private ReportNameEnum _report1Name;
        private ReportNameEnum _report2Name;
        private ReportNameEnum _report3Name;
        private ReportNameEnum _report4Name;
        private ReportNameEnum _report5Name;
        private ReportNameEnum _report6Name;
        private ReportNameEnum _report7Name;
        private ReportNameEnum _report8Name;
        private ReportNameEnum _report9Name;
        private ReportNameEnum _report10Name;
        private ReportNameEnum _report11Name;
        private ReportNameEnum _report12Name;
        private ReportNameEnum _report13Name;
        private ReportNameEnum _report14Name;
        private ReportNameEnum _report15Name;
        private ReportNameEnum _report16Name;
        private ReportNameEnum _report17Name;
        private ReportNameEnum _report18Name;
        private ReportNameEnum _report19Name;
        private ReportNameEnum _report20Name;
        private ReportNameEnum _report21Name;
        private ReportNameEnum _report22Name;
        private ReportNameEnum _report23Name;
        private ReportNameEnum _report24Name;
        private ReportNameEnum _report25Name;

        public ReportNameEnum Report1Name { get => _report1Name; set => _report1Name = value; }
        public ReportNameEnum Report2Name { get => _report2Name; set => _report2Name = value; }
        public ReportNameEnum Report3Name { get => _report3Name; set => _report3Name = value; }
        public ReportNameEnum Report4Name { get => _report4Name; set => _report4Name = value; }
        public ReportNameEnum Report5Name { get => _report5Name; set => _report5Name = value; }
        public ReportNameEnum Report6Name { get => _report6Name; set => _report6Name = value; }
        public ReportNameEnum Report7Name { get => _report7Name; set => _report7Name = value; }
        public ReportNameEnum Report8Name { get => _report8Name; set => _report8Name = value; }
        public ReportNameEnum Report9Name { get => _report9Name; set => _report9Name = value; }
        public ReportNameEnum Report10Name { get => _report10Name; set => _report10Name = value; }
        public ReportNameEnum Report11Name { get => _report11Name; set => _report11Name = value; }
        public ReportNameEnum Report12Name { get => _report12Name; set => _report12Name = value; }
        public ReportNameEnum Report13Name { get => _report13Name; set => _report13Name = value; }
        public ReportNameEnum Report14Name { get => _report14Name; set => _report14Name = value; }
        public ReportNameEnum Report15Name { get => _report15Name; set => _report15Name = value; }
        public ReportNameEnum Report16Name { get => _report16Name; set => _report16Name = value; }
        public ReportNameEnum Report17Name { get => _report17Name; set => _report17Name = value; }
        public ReportNameEnum Report18Name { get => _report18Name; set => _report18Name = value; }
        public ReportNameEnum Report19Name { get => _report19Name; set => _report19Name = value; }
        public ReportNameEnum Report20Name { get => _report20Name; set => _report20Name = value; }
        public ReportNameEnum Report21Name { get => _report21Name; set => _report21Name = value; }
        public ReportNameEnum Report22Name { get => _report22Name; set => _report22Name = value; }
        public ReportNameEnum Report23Name { get => _report23Name; set => _report23Name = value; }
        public ReportNameEnum Report24Name { get => _report24Name; set => _report24Name = value; }
        public ReportNameEnum Report25Name { get => _report25Name; set => _report25Name = value; }

        public OutputTableSummaryReports() { }

        private static List<OutputTableSummaryReports> list = new List<OutputTableSummaryReports>();

        public static void Add(OutputTableSummaryReports output)
        {
            list.Add(output);

        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:Table:SummaryReports,\n" +
                    ($"  {list[i].Report1Name}" + ",").PadRight(27, ' ') + " !-Report 1 Name\n" +
                    ($"  {list[i].Report2Name}" + ",").PadRight(27, ' ') + " !-Report 2 Name\n" +
                    ($"  {list[i].Report3Name}" + ",").PadRight(27, ' ') + " !-Report 3 Name\n" +
                    ($"  {list[i].Report4Name}" + ",").PadRight(27, ' ') + " !-Report 4 Name\n" +
                    ($"  {list[i].Report5Name}" + ",").PadRight(27, ' ') + " !-Report 5 Name\n" +
                    ($"  {list[i].Report6Name}" + ",").PadRight(27, ' ') + " !-Report 6 Name\n" +
                    ($"  {list[i].Report7Name}" + ",").PadRight(27, ' ') + " !-Report 7 Name\n" +
                    ($"  {list[i].Report8Name}" + ",").PadRight(27, ' ') + " !-Report 8 Name\n" +
                    ($"  {list[i].Report9Name}" + ",").PadRight(27, ' ') + " !-Report 9 Name\n" +
                    ($"  {list[i].Report10Name}" + ",").PadRight(27, ' ') + " !-Report 10 Name\n" +
                    ($"  {list[i].Report11Name}" + ",").PadRight(27, ' ') + " !-Report 11 Name\n" +
                    ($"  {list[i].Report12Name}" + ",").PadRight(27, ' ') + " !-Report 12 Name\n" +
                    ($"  {list[i].Report13Name}" + ",").PadRight(27, ' ') + " !-Report 13 Name\n" +
                    ($"  {list[i].Report14Name}" + ",").PadRight(27, ' ') + " !-Report 14 Name\n" +
                    ($"  {list[i].Report15Name}" + ",").PadRight(27, ' ') + " !-Report 15 Name\n" +
                    ($"  {list[i].Report16Name}" + ",").PadRight(27, ' ') + " !-Report 16 Name\n" +
                    ($"  {list[i].Report17Name}" + ",").PadRight(27, ' ') + " !-Report 17 Name\n" +
                    ($"  {list[i].Report18Name}" + ",").PadRight(27, ' ') + " !-Report 18 Name\n" +
                    ($"  {list[i].Report19Name}" + ",").PadRight(27, ' ') + " !-Report 19 Name\n" +
                    ($"  {list[i].Report20Name}" + ",").PadRight(27, ' ') + " !-Report 20 Name\n" +
                    ($"  {list[i].Report21Name}" + ",").PadRight(27, ' ') + " !-Report 21 Name\n" +
                    ($"  {list[i].Report22Name}" + ",").PadRight(27, ' ') + " !-Report 22 Name\n" +
                    ($"  {list[i].Report23Name}" + ",").PadRight(27, ' ') + " !-Report 23 Name\n" +
                    ($"  {list[i].Report24Name}" + ",").PadRight(27, ' ') + " !-Report 24 Name\n" +
                    ($"  {list[i].Report25Name}" + ";").PadRight(27, ' ') + " !-Report 25 Name";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputTableSummaryReports.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputTableSummaryReports.Read())
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
