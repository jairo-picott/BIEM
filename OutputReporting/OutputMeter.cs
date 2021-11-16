using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{

    public enum ReportingFrequencyMeterEnum
    {
        Hourly,
        Detailed,
        Timestep,
        Daily,
        Monthly,
        RunPeriod,
        Environment,
        Annual

    }
    public class OutputMeter
    {
        private string _keyName;
        private ReportingFrequencyMeterEnum _reportingFrequency = ReportingFrequencyMeterEnum.Hourly;

        public string KeyName { get => _keyName; set => _keyName = value; }
        public ReportingFrequencyMeterEnum ReportingFrequency { get => _reportingFrequency; set => _reportingFrequency = value; }

        public OutputMeter() { }

        private static List<OutputMeter> list = new List<OutputMeter>();

        public static void Add(OutputMeter output)
        {
            list.Add(output);

            Errors er = new Errors();
            if (output.KeyName == null)
            {
                er.Class = "Output:Meter";
                er.Field = "KeyName";
                Errors.Add(er);
            }
        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:Meter,\n" +
                    ($"  {list[i].KeyName}" + ",").PadRight(27, ' ') + " !-Key Name\n" +
                    ($"  {list[i].ReportingFrequency}" + ";").PadRight(27, ' ') + " !-Reporting Frequency";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputMeter.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputMeter.Read())
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
