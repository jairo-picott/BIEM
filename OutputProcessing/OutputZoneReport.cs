using System.Collections.Generic;

namespace BIEM.OutputProcessing
{

    public class OutputZoneReport
    {
        private string _zoneName;
        private double _clo = 1;
        private double _met = 1;
        private double _wme = 0;
        private double _ta;
        private double _tr;
        private double _vel;
        private double _rh;
        private string _time;

        public string ZoneName { get => _zoneName; set => _zoneName = value; }
        public double Clo { get => _clo; set => _clo = value; }
        public double Met { get => _met; set => _met = value; }
        public double Wme { get => _wme; set => _wme = value; }
        public double Ta { get => _ta; set => _ta = value; }
        public double Tr { get => _tr; set => _tr = value; }
        public double Vel { get => _vel; set => _vel = value; }
        public double Rh { get => _rh; set => _rh = value; }
        public string Time { get => _time; set => _time = value; }


        public OutputZoneReport() { }
        private static List<OutputZoneReport> list = new List<OutputZoneReport>();

        public static void Add(OutputZoneReport zone)
        {
            list.Add(zone);

        }

        public static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Zone {list[i].ZoneName}, TA = {list[i].Ta}, Time = {list[i].Time}, Vel = {list[i].Vel}"; 
            }

            return print;
        }

        public static string GetPPDPMV(OutputZoneReport outputZone)
        {
            var _pmv = AnalyticalComfortZoneMethod.GetPMVPPD(outputZone.Clo, outputZone.Met, outputZone.Wme, outputZone.Ta, outputZone.Tr, outputZone.Vel, outputZone.Rh, "PMV");
            var _ppd = AnalyticalComfortZoneMethod.GetPMVPPD(outputZone.Clo, outputZone.Met, outputZone.Wme, outputZone.Ta, outputZone.Tr, outputZone.Vel, outputZone.Rh, "PPD");

            string result = $"The thermal zone {outputZone.ZoneName}, has a PPD = {_ppd} and a PMV = {_pmv}, in the month of {outputZone.Time}";
            return result;
        }

        public static List<OutputZoneReport> GetListofOutputZoneReport()
        {
            return list;
        }
    }
}  
