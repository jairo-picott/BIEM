using System.Collections.Generic;
using System.IO;

namespace BIEM.SimulationParameters
{
    public enum AdjustZoneMixingandReturnForAirMassFlowBalanceEnum
    {
        None,
        AdjustMixingOnly,
        AdjustReturnOnly,
        AdjustMixingThenReturn,
        AdjustReturnThenMixing
    }

    public enum InfiltrationBalancingMethodEnum
    {
        None,
        AddInfiltrationFlow,
        AdjustInfiltrationFlow
    }

    public enum InfiltrationBalancingZonesEnum
    {
        MixingSourceZonesOnly,
        AllZones
    }
    public class ZoneAirMassFlowConservation
    {
        private AdjustZoneMixingandReturnForAirMassFlowBalanceEnum _adjustZoneMixingandReturnForAirMassFlowBalance = AdjustZoneMixingandReturnForAirMassFlowBalanceEnum.None;
        public AdjustZoneMixingandReturnForAirMassFlowBalanceEnum AdjustZoneMixingandReturnForAirMassFlowBalance { get => _adjustZoneMixingandReturnForAirMassFlowBalance; set => _adjustZoneMixingandReturnForAirMassFlowBalance = value; }

        private InfiltrationBalancingMethodEnum _infiltrationBalancingMethod = InfiltrationBalancingMethodEnum.AddInfiltrationFlow;
        public InfiltrationBalancingMethodEnum InfiltrationBalancingMethod { get => _infiltrationBalancingMethod; set => _infiltrationBalancingMethod = value; }

        private InfiltrationBalancingZonesEnum _infiltrationBalancingZones = InfiltrationBalancingZonesEnum.MixingSourceZonesOnly;
        public InfiltrationBalancingZonesEnum InfiltrationBalancingZones { get => _infiltrationBalancingZones; set => _infiltrationBalancingZones = value; }

        public ZoneAirMassFlowConservation() { }

        private static List<ZoneAirMassFlowConservation> list = new List<ZoneAirMassFlowConservation>();

        public static void Add(ZoneAirMassFlowConservation zoneAirMassFlowConservation)
        {
            list.Add(zoneAirMassFlowConservation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneAirMassFlowConservationn,\n" +
                    ($"  {list[i].AdjustZoneMixingandReturnForAirMassFlowBalance}" + ",").PadRight(27, ' ') + " !-Adjust Zone Mixing and Return For Air Mass Flow Balance\n" +
                    ($"  {list[i].InfiltrationBalancingMethod}" + ",").PadRight(27, ' ') + " !-Infiltration Balancing Method\n" +
                    ($"  {list[i].InfiltrationBalancingZones}" + ";").PadRight(27, ' ') + " !-Infiltration Balancing Zones";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneAirMassFlowConservation.Read())
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
