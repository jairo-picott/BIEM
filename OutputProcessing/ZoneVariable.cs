using System;
using System.Collections.Generic;
using System.Text;

namespace BIEM.OutputProcessing
{
    public class ZoneVariable
    {
        private string _zoneName;
        private string _variableName;
        private int _index;

        public string ZoneName { get => _zoneName; set => _zoneName = value; }
        public string VariableName { get => _variableName; set => _variableName = value; }
        public int Index { get => _index; set => _index = value; }

        public ZoneVariable() { }
        private static List<ZoneVariable> list = new List<ZoneVariable>();

        public static void Add(ZoneVariable zone)
        {
            list.Add(zone);

        }

     

        public static List<ZoneVariable> GetListOfZonesAndVariables()
        {
            return list;
        }
    }
}
