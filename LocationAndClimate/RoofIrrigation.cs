using System.Collections.Generic;
using System.IO;

namespace BIEM.LocationAndClimate
{
    public enum IrrigationModelTypeEnum
    {
        Schedule,
        SmartSchedule
    }
    public class RoofIrrigation
    {
        private IrrigationModelTypeEnum _irrigationModelType;
        private string _irrigationRateScheduleName;
        private double _irrigationMaximumSaturationThreshold = 40;

        public IrrigationModelTypeEnum IrrigationModelType { get => _irrigationModelType; set => _irrigationModelType = value; }
        public string IrrigationRateScheduleName { get => _irrigationRateScheduleName; set => _irrigationRateScheduleName = value; }
        public double IrrigationMaximumSaturationThreshold 
        { 
            get => _irrigationMaximumSaturationThreshold; 
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _irrigationMaximumSaturationThreshold = value;
                }
            }
        }

        public RoofIrrigation() { }

        private static List<RoofIrrigation> list = new List<RoofIrrigation>();

        public static void Add(RoofIrrigation roofIrrigation)
        {
            list.Add(roofIrrigation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"RoofIrrigation,\n" +
                    ($"  {list[i].IrrigationModelType}" + ",").PadRight(27, ' ') + " !-Irrigation Model Type\n" +
                    ($"  {list[i].IrrigationRateScheduleName}" + ",").PadRight(27, ' ') + " !-Irrigation Rate Schedule Name\n" +
                    ($"  {list[i].IrrigationMaximumSaturationThreshold}" + ";").PadRight(27, ' ') + " !-Irrigation Maximum Saturation Threshold {{ % }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in RoofIrrigation.Read())
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
