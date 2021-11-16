using System.IO;
using System.Collections.Generic;

namespace BIEM.ZoneHVACConstrolsandThermostats
{
    public class ThermostatSetpointDualSetpoint
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private string _heatingSetPointTemperatureScheduleName;
        public string HeatingSetpointTemperatureScheduleName { get=>_heatingSetPointTemperatureScheduleName; set=>_heatingSetPointTemperatureScheduleName=value; }

        private string _coolingSetpointTemperatureScheduleName;
        public string CoolingSetpointTemperatureScheduleName { get=>_coolingSetpointTemperatureScheduleName; set=>_coolingSetpointTemperatureScheduleName=value; }
        public ThermostatSetpointDualSetpoint() { }


        private static List<ThermostatSetpointDualSetpoint> list = new List<ThermostatSetpointDualSetpoint>();

        public static void Add(ThermostatSetpointDualSetpoint thermostatSetpointDualSetpoint)
        {
            list.Add(thermostatSetpointDualSetpoint);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ThermostatSetpoint:DualSetpoint,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].HeatingSetpointTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Heating Setpoint Temperature Schedule Name\n" +
                    ($"  {list[i].CoolingSetpointTemperatureScheduleName}" + ";").PadRight(27, ' ') + " !-Cooling Setpoint Temperature Schedule Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ThermostatSetpointDualSetpoint.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(ThermostatSetpointDualSetpoint thermostat)
        {
            thermostat.Name = null;
            thermostat.HeatingSetpointTemperatureScheduleName = null;
            thermostat.CoolingSetpointTemperatureScheduleName = null;

        }
    }
}
