using System.IO;
using System.Collections.Generic;

namespace BIEM.ZoneAirFlow
{
    public class ZoneInfiltrationDesignFlowRate
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private string _zoneorZoneListName;
        public string ZoneorZoneListName { get=>_zoneorZoneListName; set=>_zoneorZoneListName=value; }

        private string _scheduleName;
        public string ScheduleName { get=>_scheduleName; set=>_scheduleName=value; }

        private string _designFlowRateCalculationMethod;
        public string DesignFlowRateCalculationMethod { get=>_designFlowRateCalculationMethod; set=>_designFlowRateCalculationMethod=value; }

        private double? _designFlowRate;
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

        private double? _flowperZoneFloorArea;
        public double? FlowperZoneFloorArea
        {
            get => _flowperZoneFloorArea;
            set
            {

                if (value >= 0)
                {
                    _flowperZoneFloorArea = value;
                }
            }
        }

        private double? _flowperExteriorSurfaceArea;
        public double? FlowperExteriorSurfaceArea
        {
            get => _flowperExteriorSurfaceArea;
            set
            {

                if (value >= 0)
                {
                    _flowperExteriorSurfaceArea = value;
                }
            }
        }

        private double? _airChangesperHour;
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

        private double? _constantTermCoefficient = 1;
        public double? ConstantTermCoefficient { get=>_constantTermCoefficient; set=>_constantTermCoefficient=value; }

        private double? _temperatureTermCoefficient = 0;
        public double? TemperatureTermCoefficient { get=>_temperatureTermCoefficient; set=>_temperatureTermCoefficient=value; }

        private double? _velocityTermCoefficient = 0;
        public double? VelocityTermCoefficient { get=>_velocityTermCoefficient; set=>_velocityTermCoefficient=value; }

        private double? _velocitySquaredTermCoefficient = 0;
        public double? VelocitySquaredTermCoefficient { get=>_velocitySquaredTermCoefficient; set=>_velocitySquaredTermCoefficient=value; }


        public ZoneInfiltrationDesignFlowRate() { }

        private static List<ZoneInfiltrationDesignFlowRate> list = new List<ZoneInfiltrationDesignFlowRate>();

        public static void Add(ZoneInfiltrationDesignFlowRate zoneInfiltrationDesignFlowRate)
        {
            list.Add(zoneInfiltrationDesignFlowRate);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneInfiltration:DesignFlowRate,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ZoneorZoneListName}" + ",").PadRight(27, ' ') + " !-Zone or Zone List Name\n" +
                    ($"  {list[i].ScheduleName}" + ",").PadRight(27, ' ') + " !-Schedule Name\n" +
                    ($"  {list[i].DesignFlowRateCalculationMethod}" + ",").PadRight(27, ' ') + " !-Design Flow Rate Calculation Method\n" +
                    ($"  {list[i].DesignFlowRate}" + ",").PadRight(27, ' ') + " !-Design Flow Rate{{ m3 / s }}\n" +
                    ($"  {list[i].FlowperZoneFloorArea}" + ",").PadRight(27, ' ') + " !-Flow per Zone Floor Area{{ m3 / s-m2 }}\n" +
                    ($"  {list[i].FlowperExteriorSurfaceArea}" + ",").PadRight(27, ' ') + " !-Flow per Exterior Surface Area{{ m3 / s-m2 }}\n" +
                    ($"  {list[i].AirChangesperHour}" + ",").PadRight(27, ' ') + " !-Air Changes per Hour{{ 1 / hr }}\n" +
                    ($"  {list[i].ConstantTermCoefficient}" + ",").PadRight(27, ' ') + " !-Constant Term Coefficient\n" +
                    ($"  {list[i].TemperatureTermCoefficient}" + ",").PadRight(27, ' ') + " !-Temperature Term Coefficient\n" +
                    ($"  {list[i].VelocityTermCoefficient}" + ",").PadRight(27, ' ') + " !-Velocity Term Coefficient\n" +
                    ($"  {list[i].VelocitySquaredTermCoefficient}" + ";").PadRight(27, ' ') + " !-Velocity Squared Term Coefficient";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneInfiltrationDesignFlowRate.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }


        public static void Collect(ZoneInfiltrationDesignFlowRate zoneInfiltration)
        {
            zoneInfiltration.Name = null;
            zoneInfiltration.ZoneorZoneListName = null;
            zoneInfiltration.ScheduleName = null;
            zoneInfiltration.DesignFlowRateCalculationMethod = null;
            zoneInfiltration.DesignFlowRate = null;
            zoneInfiltration.FlowperZoneFloorArea = null;
            zoneInfiltration.FlowperExteriorSurfaceArea = null;
            zoneInfiltration.AirChangesperHour = null;
            zoneInfiltration.ConstantTermCoefficient = null;
            zoneInfiltration.TemperatureTermCoefficient = null;
            zoneInfiltration.VelocityTermCoefficient = null;
            zoneInfiltration.VelocitySquaredTermCoefficient = null;
        }
    }
}
