using System.IO;
using System.Collections.Generic;


namespace BIEM.Schedules
{
    public enum NumericType
    {
        Continuous,
        Discrete
    }

    public enum UnitType
    {
        Dimensionless,
        Temperature,
        DeltaTemperature,
        PrecipitationRate,
        Angle,
        ConvectionCoefficient,
        ActivityLevel,
        Velocity,
        Capacity,
        Power,
        Availability,
        Percent,
        Control
    }
    
    //------------Schedule Type Limits
    //DEFINE THE BASIC TYPE OF SHEDULES USED FOR CREATING THE SCHEDULES 
    //FOR THE ENERGETIC ANALYSIS, DATA IS FIXED DUE TO THIS TYPES ARE THE
    //MOST USED, THEN ARE SELECTED AND DESCRIBED EACH SCHEDULES DEPENDING
    //TIPOLOGY.
    public class ScheduleTypeLimits
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private double? _lowerLimitValue;
        public double? LowerLimitValue
        {
            get => _lowerLimitValue;
            set => _lowerLimitValue = value;
        }

        private double? _upperLimitValue;
        public double? UpperLimitValue
        {
            get => _upperLimitValue;
            set => _upperLimitValue = value;
        }

        private NumericType? _numericType;
        public NumericType? NumericType
        {
            get => _numericType;
            set => _numericType = value;
        }

        private UnitType? _unitType;
        public UnitType? UnitType
        {
            get => _unitType;
            set => _unitType = value;
        }

        public ScheduleTypeLimits() { }

        private static List<ScheduleTypeLimits> list = new List<ScheduleTypeLimits>();

        public static void Add(ScheduleTypeLimits scheduleTypeLimits)
        {
            list.Add(scheduleTypeLimits);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ScheduleTypeLimits,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].LowerLimitValue}" + ",").PadRight(27, ' ') + " !-Lower Limit Value\n" +
                    ($"  {list[i].UpperLimitValue}" + ",").PadRight(27, ' ') + " !-Upper Limit Value\n" +
                    ($"  {list[i].NumericType}" + ",").PadRight(27, ' ') + " !-Numeric Type\n" +
                    ($"  {list[i].UnitType}" + ";").PadRight(27, ' ') + " !-Unit Type";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ScheduleTypeLimits.Read())
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
