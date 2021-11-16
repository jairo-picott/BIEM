using System.IO;
using System.Collections.Generic;

namespace BIEM.ZoneHVACConstrolsandThermostats
{
    public class ZoneControlThermostat
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private string _zoneorZoneListName;
        public string ZoneorZoneListName { get=>_zoneorZoneListName; set=>_zoneorZoneListName=value; }

        private string _controlTypeScheduleName;
        public string ControlTypeScheduleName { get=>_controlTypeScheduleName; set=>_controlTypeScheduleName=value; }

        private string _control1ObjectType;
        public string Control1ObjectType { get=>_control1ObjectType; set=>_control1ObjectType=value; }

        private string _control1Name;
        public string Control1Name { get=>_control1Name; set=>_control1Name=value; }

        private string _control2ObjectType;
        public string Control2ObjectType { get => _control2ObjectType; set => _control2ObjectType = value; }

        private string _control2Name;
        public string Control2Name { get => _control2Name; set => _control2Name = value; }

        private string _control3ObjectType;
        public string Control3ObjectType { get => _control3ObjectType; set => _control3ObjectType = value; }

        private string _control3Name;
        public string Control3Name { get => _control3Name; set => _control3Name = value; }

        private string _control4ObjectType;
        public string Control4ObjectType { get => _control4ObjectType; set => _control4ObjectType = value; }

        private string _control4Name;
        public string Control4Name { get => _control4Name; set => _control4Name = value; }

        private double? _temperatureDifferenceBetweenCutoutAndSetpoint;
        public double? TemperatureDifferenceBetweenCutoutandSetpoint { get=>_temperatureDifferenceBetweenCutoutAndSetpoint; set=>_temperatureDifferenceBetweenCutoutAndSetpoint=value; }


        public ZoneControlThermostat() { }

        private static List<ZoneControlThermostat> list = new List<ZoneControlThermostat>();

        public static void Add(ZoneControlThermostat zoneControlThermostat)
        {
            list.Add(zoneControlThermostat);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneControl:Thermostat,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ZoneorZoneListName}" + ",").PadRight(27, ' ') + " !-Zone or Zone List Name\n" +
                    ($"  {list[i].ControlTypeScheduleName}" + ",").PadRight(27, ' ') + " !-Control Type Schedule Name\n" +
                    ($"  {list[i].Control1ObjectType}" + ",").PadRight(27, ' ') + " !-Control 1 Object Type\n" +
                    ($"  {list[i].Control1Name}" + ",").PadRight(27, ' ') + " !-Control 1 Name\n" +
                    ($"  {list[i].Control2ObjectType}" + ",").PadRight(27, ' ') + " !-Control 2 Object Type\n" +
                    ($"  {list[i].Control2Name}" + ",").PadRight(27, ' ') + " !-Control 2 Name\n" +
                    ($"  {list[i].Control3ObjectType}" + ",").PadRight(27, ' ') + " !-Control 3 ObjectType\n" +
                    ($"  {list[i].Control3Name}" + ",").PadRight(27, ' ') + " !-Control 3 Name\n" +
                    ($"  {list[i].Control4ObjectType}" + ",").PadRight(27, ' ') + " !-Control 4 Object Type\n" +
                    ($"  {list[i].Control4Name}" + ",").PadRight(27, ' ') + " !-Control 4 Name\n" +
                    ($"  {list[i].TemperatureDifferenceBetweenCutoutandSetpoint}" + ";").PadRight(27, ' ') + " !-Temperature Difference Between Cutout and Setpoint{{ deltaC }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneControlThermostat.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(ZoneControlThermostat zoneControl)
        {
            zoneControl.Name = null;
            zoneControl.ZoneorZoneListName = null;
            zoneControl.ControlTypeScheduleName = null;
            zoneControl.Control1ObjectType = null;
            zoneControl.Control1Name = null;
            zoneControl.Control2ObjectType = null;
            zoneControl.Control2Name = null;
            zoneControl.Control3ObjectType = null;
            zoneControl.Control3Name = null;
            zoneControl.Control4ObjectType = null;
            zoneControl.Control4Name = null;
            zoneControl.TemperatureDifferenceBetweenCutoutandSetpoint = null;

        }

    }
}
