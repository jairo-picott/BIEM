using System.IO;
using System.Collections.Generic;

namespace BIEM.ZoneAirFlow
{
    public enum VentilationTypeEnum
    {
        Natural,
        Intake,
        Exhaust,
        Balanced
    }
    public class ZoneVentilationDesignFlowRate
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private string _zoneorZoneListName;
        public string ZoneorZoneListName { get=>_zoneorZoneListName; set=>_zoneorZoneListName=value; }

        private string _scheduleName;
        public string ScheduleName { get=>_scheduleName; set=>_scheduleName=value; }

        private string _designFloweRateCalculationMethod;
        public string DesignFlowRateCalculationMethod { get=>_designFloweRateCalculationMethod; set=>_designFloweRateCalculationMethod=value; }

        private double? _designFlowRate=0;
        public double? DesignFlowRate
        {
            get => _designFlowRate;
            set
            {
                if (value >= 0)
                {
                    _designFlowRate = value;
                }
            }
        }

        private double? _flowRateperZoneFloorArea=0;
        public double? FlowRateperZoneFloorArea
        {
            get => _flowRateperZoneFloorArea;
            set
            {
                if (value >= 0)
                {
                    _flowRateperZoneFloorArea = value;
                }
            }
        }

        private double? _flowRateperPerson=0;
        public double? FlowRateperPerson
        {
            get => _flowRateperPerson;
            set
            {
                if (value >= 0)
                {
                    _flowRateperPerson = value;
                }
            }
        }

        private double? _airChangesperHour = 0;
        public double? AirChangesperHour
        {
            get => _airChangesperHour;
            set
            {
                if (value >= 0)
                {
                    _airChangesperHour = value;
                }
            }
        }

        private VentilationTypeEnum? _ventilationType;
        public VentilationTypeEnum? VentilationType { get=>_ventilationType; set=>_ventilationType=value; }

        private double? _fanPressureRise = 0;
        public double? FanPressureRise
        {
            get => _fanPressureRise;
            set
            {
                if (value >= 0)
                {
                    _fanPressureRise = value;
                }
            }
        }

        private double? _fanTotalEfficiency = 1;
        public double? FanTotalEfficiency
        {
            get => _fanTotalEfficiency;
            set
            {
                if (value > 0)
                {
                    _fanTotalEfficiency = value;
                }
            }
        }

        private double? _constantTermCoeefficient = 1;
        public double? ConstantTermCoefficient
        {
            get => _constantTermCoeefficient;
            set => _constantTermCoeefficient = value;
        }

        private double? _temperatureTermCoefficient = 0;
        public double? TemperatureTermCoefficient
        {
            get => _temperatureTermCoefficient;
            set => _temperatureTermCoefficient = value;
        }

        private double? _velocityTermCoefficient = 0;
        public double? VelocityTermCoefficient { get=>_velocityTermCoefficient; set=>_velocityTermCoefficient=value; }

        private double? _velocitySquaredTermCoefficient = 0;
        public double? VelocitySquaredTermCoefficient { get=>_velocitySquaredTermCoefficient; set=>_velocitySquaredTermCoefficient=value; }

        private double? _minimumIndoorTemperature = -100;
        public double? MinimumIndoorTemperature
        {
            get => _minimumIndoorTemperature;
            set
            {
                if (value>=-100 && value<=100)
                {
                    _minimumIndoorTemperature = value;
                }
            }
        }

        private string _minimumIndoorTemperatureScheduleName;
        public string MinimumIndoorTemperatureScheduleName { get=>_minimumIndoorTemperatureScheduleName; set=>_minimumIndoorTemperatureScheduleName=value; }

        private double? _maximumIndoorTemperature = 100;
        public double? MaximumIndoorTemperature
        {
            get => _maximumIndoorTemperature;
            set
            {
                if (value >= -100 && value <= 100)
                {
                    _maximumIndoorTemperature = value;
                }
            }
        }

        private string _maximumIndoorTemperatureScheduleName;
        public string MaximumIndoorTemperatureScheduleName { get=>_maximumIndoorTemperatureScheduleName; set=>_maximumIndoorTemperatureScheduleName=value; }

        private double? _deltaTemperature = -100;
        public double? DeltaTemperature
        {
            get => _deltaTemperature;
            set
            {
                if (value>=-100)
                {
                    _deltaTemperature = value;
                }
            }
        }

        private string _deltaTemperatureScheduleName;
        public string DeltaTemperatureScheduleName { get=>_deltaTemperatureScheduleName; set=>_deltaTemperatureScheduleName=value; }

        private double? _minimumOutdoorTemperature = -100;
        public double? MinimumOutdoorTemperature
        {
            get => _minimumIndoorTemperature;
            set
            {
                if (value>=-100 && value <=100)
                {
                    _minimumIndoorTemperature = value;
                }
            }
        }

        private string _minimumOutdoorTemperatureScheduleName;
        public string MinimumOutdoorTemperatureScheduleName { get=>_minimumOutdoorTemperatureScheduleName; set=>_minimumOutdoorTemperatureScheduleName=value; }

        private double? _maximumOutdoorTemperature = 100;
        public double? MaximumOutdoorTemperature
        {
            get => _maximumOutdoorTemperature;
            set
            {
                if (value>=-100 && value<=100)
                {
                    _maximumOutdoorTemperature = value;
                }
            }
        }

        private string _maximumOutdoorTemperatureScheduleName;
        public string MaximumOutdoorTemperatureScheduleName { get=>_maximumOutdoorTemperatureScheduleName; set=>_maximumOutdoorTemperatureScheduleName=value; }

        private double? _maximumWindSpeed = 40;
        public double? MaximumWindSpeed
        {
            get => _maximumWindSpeed;
            set
            {
                if (value>=0 && value<=40)
                {
                    _maximumWindSpeed = value;
                }
            }
        }
        public ZoneVentilationDesignFlowRate() { }

        private static List<ZoneVentilationDesignFlowRate> list = new List<ZoneVentilationDesignFlowRate>();

        public static void Add(ZoneVentilationDesignFlowRate zoneVentilationDesignFlowRate)
        {
            list.Add(zoneVentilationDesignFlowRate);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneVentilation:DesignFlowRate,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ZoneorZoneListName}" + ",").PadRight(27, ' ') + " !-Zone or Zone List Name\n" +
                    ($"  {list[i].ScheduleName}" + ",").PadRight(27, ' ') + " !-Schedule Name\n" +
                    ($"  {list[i].DesignFlowRateCalculationMethod}" + ",").PadRight(27, ' ') + " !-Design Flow Rate Calculation Method\n" +
                    ($"  {list[i].DesignFlowRate}" + ",").PadRight(27, ' ') + " !-Design Flow Rate{{ m3 / s }}\n" +
                    ($"  {list[i].FlowRateperZoneFloorArea}" + ",").PadRight(27, ' ') + " !-Flow Rate per Zone Floor Area{{ m3 / s-m2 }}\n" +
                    ($"  {list[i].FlowRateperPerson}" + ",").PadRight(27, ' ') + " !-Flow Rate per Person{{ m3 / s-person }}\n" +
                    ($"  {list[i].AirChangesperHour}" + ",").PadRight(27, ' ') + " !-Air Changes per Hour{{ 1 / hr }}\n" +
                    ($"  {list[i].VentilationType}" + ",").PadRight(27, ' ') + " !-Ventilation Type\n" +
                    ($"  {list[i].FanPressureRise}" + ",").PadRight(27, ' ') + " !-Fan Pressure Rise{{ Pa }}\n" +
                    ($"  {list[i].FanTotalEfficiency}" + ",").PadRight(27, ' ') + " !-Fan Total Efficiency\n" +
                    ($"  {list[i].ConstantTermCoefficient}" + ",").PadRight(27, ' ') + " !-Constant Term Coefficient\n" +
                    ($"  {list[i].TemperatureTermCoefficient}" + ",").PadRight(27, ' ') + " !-Temperature Term Coefficient\n" +
                    ($"  {list[i].VelocityTermCoefficient}" + ",").PadRight(27, ' ') + " !-Velocity Term Coefficient\n" +
                    ($"  {list[i].VelocitySquaredTermCoefficient}" + ",").PadRight(27, ' ') + " !-Velocity Squared Term Coefficient\n" +
                    ($"  {list[i].MinimumIndoorTemperature}" + ",").PadRight(27, ' ') + " !-Minimum Indoor Temperature{{ C }}\n" +
                    ($"  {list[i].MinimumIndoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Minimum Indoor Temperature Schedule Name\n" +
                    ($"  {list[i].MaximumIndoorTemperature}" + ",").PadRight(27, ' ') + " !-Maximum Indoor Temperature{{ C }}\n" +
                    ($"  {list[i].MaximumIndoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Maximum Indoor Temperature Schedule Name\n" +
                    ($"  {list[i].DeltaTemperature}" + ",").PadRight(27, ' ') + " !-Delta Temperature{{ deltaC }}\n" +
                    ($"  {list[i].DeltaTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Delta Temperature Schedule Name\n" +
                    ($"  {list[i].MinimumOutdoorTemperature}" + ",").PadRight(27, ' ') + " !-Minimum Outdoor Temperature{{ C }}\n" +
                    ($"  {list[i].MinimumOutdoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Minimum Outdoor Temperature Schedule Name\n" +
                    ($"  {list[i].MaximumOutdoorTemperature}" + ",").PadRight(27, ' ') + " !-Maximum Outdoor Temperature{{ C }}\n" +
                    ($"  {list[i].MaximumOutdoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Maximum Outdoor Temperature Schedule Name\n" +
                    ($"  {list[i].MaximumWindSpeed}" + ";").PadRight(27, ' ') + " !-Maximum Wind Speed{{ m / s }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneVentilationDesignFlowRate.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(ZoneVentilationDesignFlowRate zoneVentilation)
        {
            zoneVentilation.Name = null;
            zoneVentilation.ZoneorZoneListName = null;
            zoneVentilation.ScheduleName = null;
            zoneVentilation.DesignFlowRateCalculationMethod = null;
            zoneVentilation.DesignFlowRate = null;
            zoneVentilation.FlowRateperZoneFloorArea = null;
            zoneVentilation.FlowRateperPerson = null;
            zoneVentilation.AirChangesperHour = null;
            zoneVentilation.VentilationType = null;
            zoneVentilation.FanPressureRise = null;
            zoneVentilation.FanTotalEfficiency = null;
            zoneVentilation.ConstantTermCoefficient = null;
            zoneVentilation.TemperatureTermCoefficient = null;
            zoneVentilation.VelocityTermCoefficient = null;
            zoneVentilation.VelocitySquaredTermCoefficient = null;
            zoneVentilation.MinimumIndoorTemperature = null;
            zoneVentilation.MinimumIndoorTemperatureScheduleName = null;
            zoneVentilation.MaximumIndoorTemperature = null;
            zoneVentilation.MaximumIndoorTemperatureScheduleName = null;
            zoneVentilation.DeltaTemperature = null;
            zoneVentilation.DeltaTemperatureScheduleName = null;
            zoneVentilation.MinimumOutdoorTemperature = null;
            zoneVentilation.MinimumOutdoorTemperatureScheduleName = null;
            zoneVentilation.MaximumOutdoorTemperature = null;
            zoneVentilation.MaximumOutdoorTemperatureScheduleName = null;
            zoneVentilation.MaximumWindSpeed = null;
        }
    }
}
