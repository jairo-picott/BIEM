using System.Collections.Generic;
using System.IO;

namespace BIEM.ZoneHVACEquipmentConnections
{
    public class ZoneHVACEquipmentConnections
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private string _conditioningEquipmentListName;
        public string ConditioningEquipmentListName { get=>_conditioningEquipmentListName; set=>_conditioningEquipmentListName=value; }

        private string _airInletNodeorNodeListName;
        public string AirInletNodeorNodeListName { get=>_airInletNodeorNodeListName; set=>_airInletNodeorNodeListName=value; }

        private string _airExhaustNodeListName;
        public string AirExhaustNodeorNodeListName { get=>_airExhaustNodeListName; set=>_airExhaustNodeListName=value; }

        private string _airNodeName;
        public string AirNodeName { get=>_airNodeName; set=>_airNodeName=value; }

        private string _returnAirNodeorNodeListName;
        public string ReturnAirNodeorNodeListName { get=>_returnAirNodeorNodeListName; set=>_returnAirNodeorNodeListName=value; }

        private string _returnAirNode1FlowRateFractionScheduleName;
        public string ReturnAirNode1FlowRateFractionScheduleName { get=>_returnAirNode1FlowRateFractionScheduleName; set=>_returnAirNode1FlowRateFractionScheduleName=value; }

        private string _returnAirNode1FlowRateBasisNodeorNodeListName;
        public string ReturnAirNode1FlowRateBasisNodeorNodeListName { get=>_returnAirNode1FlowRateBasisNodeorNodeListName; set=>_returnAirNode1FlowRateBasisNodeorNodeListName=value; }

        public ZoneHVACEquipmentConnections() { }

        private static List<ZoneHVACEquipmentConnections> list = new List<ZoneHVACEquipmentConnections>();

        public static void Add(ZoneHVACEquipmentConnections zoneHVACEquipmentConnections)
        {
            list.Add(zoneHVACEquipmentConnections);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneHVAC:EquipmentConnections,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ConditioningEquipmentListName}" + ",").PadRight(27, ' ') + " !-Conditioning Equipment List Name\n" +
                    ($"  {list[i].AirInletNodeorNodeListName}" + ",").PadRight(27, ' ') + " !-Air Inlet Node or Node List Name\n" +
                    ($"  {list[i].AirExhaustNodeorNodeListName}" + ",").PadRight(27, ' ') + " !-Air Exhaust Node or Node List Name\n" +
                    ($"  {list[i].AirNodeName}" + ",").PadRight(27, ' ') + " !-Air Node Name\n" +
                    ($"  {list[i].ReturnAirNodeorNodeListName}" + ",").PadRight(27, ' ') + " !-Return Air Node or Node List Name\n" +
                    ($"  {list[i].ReturnAirNode1FlowRateFractionScheduleName}" + ",").PadRight(27, ' ') + " !-Return Air Node 1 Flow Rate Fraction Schedule Name\n" +
                    ($"  {list[i].ReturnAirNode1FlowRateBasisNodeorNodeListName}" + ";").PadRight(27, ' ') + " !-Return Air Node 1 Flow Rate Basis Node or Node List Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneHVACEquipmentConnections.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(ZoneHVACEquipmentConnections iZone)
        {
            iZone.Name = null;
            iZone.ConditioningEquipmentListName = null;
            iZone.AirInletNodeorNodeListName = null;
            iZone.AirExhaustNodeorNodeListName = null;
            iZone.AirNodeName = null;
            iZone.ReturnAirNodeorNodeListName = null;
            iZone.ReturnAirNode1FlowRateFractionScheduleName = null;
            iZone.ReturnAirNode1FlowRateBasisNodeorNodeListName = null;
        }
    }
}
