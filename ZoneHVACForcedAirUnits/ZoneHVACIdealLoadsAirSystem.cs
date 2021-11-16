using System.IO;
using System.Collections.Generic;

namespace BIEM.ZoneHVACForcedAirUnits
{
    public enum HeatRecoveryTypeEnum
    {
        None,
        Sensible,
        Enthalpy
    }
    public enum OutdoorAirEconomizerTypeEnum
    {
        NoEconomizer,
        DifferentialDryBulb,
        DifferentialEnthalpy
    }
    public enum DemandControlledVentilationTypeEnum
    {
        None,
        OccupancySchedule,
        CO2Setpoint
    }
    public enum HumidificationControlTypeEnum
    {
        ConstantSupplyHumidityRatio,
        Humidistat,
        None
    }
    public enum DehumidificationControlTypeEnum
    {
        ConstantSupplyHumidityRatio,
        ConstantSensibleHeatRatio,
        Humidistat,
        None
    }
    public enum HeatingLimitEnum
    {
        NoLimit,
        LimitFlowRate,
        LimitCapacity,
        LimitFlowRateAndCapacity
    }

    public enum CoolingLimitEnum
    {
        NoLimit,
        LimitFlowRate,
        LimitCapacity,
        LimitFlowRateAndCapacity
    }
    public class ZoneHVACIdealLoadsAirSystem
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private string _availabilityScheduleName;
        public string AvailabilityScheduleName { get=>_availabilityScheduleName; set=>_availabilityScheduleName=value; }

        private string _zoneSupplyAirNodeName;
        public string ZoneSupplyAirNodeName { get=>_zoneSupplyAirNodeName; set=>_zoneSupplyAirNodeName=value; }

        private string _zoneExhaustAirNodeName;
        public string ZoneExhaustAirNodeName { get=>_zoneExhaustAirNodeName; set=>_zoneExhaustAirNodeName=value; }

        private string _systemInletAirNodeName;
        public string SystemInletAirNodeName { get=>_systemInletAirNodeName; set=>_systemInletAirNodeName=value; }

        private double? _maximumHeatingSupplyAirTemperature = 50;
        public double? MaximumHeatingSupplyAirTemperature
        {
            get => _maximumHeatingSupplyAirTemperature;
            set
            {
                if (value > 0 && value < 100)
                {
                    _maximumHeatingSupplyAirTemperature = value;
                }
            }
        }

        private double? _minimumCoolingSupplyAirTemperature = 13;
        public double? MinimumCoolingSupplyAirTemperature
        {
            get => _minimumCoolingSupplyAirTemperature;
            set
            {
                if (value > -100 && value < 50)
                {
                    _minimumCoolingSupplyAirTemperature = value;
                }
            }
        }

        private double? _maximumHeatingSupplyAirHumidityRatio = 0.0156;
        public double? MaximumHeatingSupplyAirHumidityRatio
        {
            get => _maximumHeatingSupplyAirHumidityRatio;
            set
            {
                if (value > 0)
                {
                    _maximumHeatingSupplyAirHumidityRatio = value;
                }
            }
        }

        private double? _minimumCoolingSupplyAirHumidityRatio = 0.0077;
        public double? MinimumCoolingSupplyAirHumidityRatio
        {
            get => _minimumCoolingSupplyAirHumidityRatio;
            set
            {
                if (value > 0)
                {
                    _minimumCoolingSupplyAirHumidityRatio = value;
                }
            }
        }

        private HeatingLimitEnum? _heatingLimit = HeatingLimitEnum.NoLimit;
        public HeatingLimitEnum? HeatingLimit { get=>_heatingLimit; set=>_heatingLimit=value; }

        private double? _maximumHeatingAirFlowRate;
        public double? MaximumHeatingAirFlowRate
        {
            get => _maximumHeatingAirFlowRate;
            set
            {
                if (value >= 0)
                {
                    _maximumHeatingAirFlowRate = value;
                }
            }
        }

        private double? _maximumSensibleHeatingCapacity;
        public double? MaximumSensibleHeatingCapacity
        {
            get => _maximumSensibleHeatingCapacity;
            set
            {
                if (value>0)
                {
                    _maximumSensibleHeatingCapacity = value;
                }
            }
        }

        private CoolingLimitEnum? _coolingLimit = CoolingLimitEnum.NoLimit;
        public CoolingLimitEnum? CoolingLimit { get=>_coolingLimit; set=>_coolingLimit=value; }

        private double? _maximumCoolingAirFlowRate;
        public double? MaximumCoolingAirFlowRate
        {
            get => _maximumCoolingAirFlowRate;
            set
            {
                if (value >= 0)
                {
                    _maximumCoolingAirFlowRate = value;
                }
            }
        }

        private double? _maximumTotalCoolingCapacity;
        public double? MaximumTotalCoolingCapacity
        {
            get => _maximumTotalCoolingCapacity;
            set
            {
                if (value >= 0)
                {
                    _maximumTotalCoolingCapacity = value;
                }
            }
        }

        private string _heatingAvailabilityScheduleName;
        public string HeatingAvailabilityScheduleName { get=>_heatingAvailabilityScheduleName; set=>_heatingAvailabilityScheduleName=value; }

        private string _coolingAvailabilityScheduleName;
        public string CoolingAvailabilityScheduleName { get=>_coolingAvailabilityScheduleName; set=>_coolingAvailabilityScheduleName=value; }

        private DehumidificationControlTypeEnum? _dehumidificationControlType;
        public DehumidificationControlTypeEnum? DehumidificationControlType { get=>_dehumidificationControlType; set=>_dehumidificationControlType=value; }

        private double? _coolingSensibleHeatRatio = 0.7;
        public double? CoolingSensibleHeatRatio
        {
            get => _coolingSensibleHeatRatio;
            set
            {
                if (value > 0 && value <= 1)
                {
                    _coolingSensibleHeatRatio = value;
                }
            }
        }

        private HumidificationControlTypeEnum? _humidificationControlType; 
        public HumidificationControlTypeEnum? HumidificationControlType { get=>_humidificationControlType; set=>_humidificationControlType=value; }

        private string _designSpecificationOutdoorAirObjectName;
        public string DesignSpecificationOutdoorAirObjectName { get=>_designSpecificationOutdoorAirObjectName; set=>_designSpecificationOutdoorAirObjectName=value; }

        private string _outdoorAirInletNodeName;
        public string OutdoorAirInletNodeName { get=>_outdoorAirInletNodeName; set=>_outdoorAirInletNodeName=value; }

        private DemandControlledVentilationTypeEnum? _demandControlledVentilationType;
        public DemandControlledVentilationTypeEnum? DemandControlledVentilationType { get=>_demandControlledVentilationType; set=>_demandControlledVentilationType=value; }

        private OutdoorAirEconomizerTypeEnum? _outdoorAirEconomizerType;
        public OutdoorAirEconomizerTypeEnum? OutdoorAirEconomizerType { get=>_outdoorAirEconomizerType; set=>_outdoorAirEconomizerType=value; }

        private HeatRecoveryTypeEnum? _heatRecoveryType;
        public HeatRecoveryTypeEnum? HeatRecoveryType { get=>_heatRecoveryType; set=>_heatRecoveryType=value; }

        private double? _sensibleHeatRecoveryEffectiveness = 0.7;
        public double? SensibleHeatRecoveryEffectiveness
        {
            get => _sensibleHeatRecoveryEffectiveness;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _sensibleHeatRecoveryEffectiveness = value;
                }
            }
        }

        private double? _latentHeatRecoveryEffectiveness = 0.65;
        public double? LatentHeatRecoveryEffectiveness
        {
            get => _latentHeatRecoveryEffectiveness;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _latentHeatRecoveryEffectiveness = value;
                }
            }
        }

        private string _designSpecificationZoneHVACSizingObjectname;
        public string DesignSpecificationZoneHVACSizingObjectName { get=>_designSpecificationZoneHVACSizingObjectname; set=>_designSpecificationZoneHVACSizingObjectname=value; }

        public ZoneHVACIdealLoadsAirSystem() { }


        private static List<ZoneHVACIdealLoadsAirSystem> list = new List<ZoneHVACIdealLoadsAirSystem>();

        public static void Add(ZoneHVACIdealLoadsAirSystem zoneHVACIdealLoadsAirSystem)
        {
            list.Add(zoneHVACIdealLoadsAirSystem);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneHVAC:IdealLoadsAirSystem,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].AvailabilityScheduleName}" + ",").PadRight(27, ' ') + " !-Availability Schedule Name\n" +
                    ($"  {list[i].ZoneSupplyAirNodeName}" + ",").PadRight(27, ' ') + " !-Zone Supply Air Node Name\n" +
                    ($"  {list[i].ZoneExhaustAirNodeName}" + ",").PadRight(27, ' ') + " !-Zone Exhaust Air Node Name\n" +
                    ($"  {list[i].SystemInletAirNodeName}" + ",").PadRight(27, ' ') + " !-System Inlet Air Node Name\n" +
                    ($"  {list[i].MaximumHeatingSupplyAirTemperature}" + ",").PadRight(27, ' ') + " !-Maximum Heating Supply Air Temperature{{ C }}\n" +
                    ($"  {list[i].MinimumCoolingSupplyAirTemperature}" + ",").PadRight(27, ' ') + " !-Minimum Cooling Supply Air Temperature{{ C }}\n" +
                    ($"  {list[i].MaximumHeatingSupplyAirHumidityRatio}" + ",").PadRight(27, ' ') + " !-Maximum Heating Supply Air Humidity Ratio{{ kgWater / kgDryAir }}\n" +
                    ($"  {list[i].MinimumCoolingSupplyAirHumidityRatio}" + ",").PadRight(27, ' ') + " !-Minimum Cooling Supply Air Humidity Ratio{{ kgWater / kgDryAir }}\n" +
                    ($"  {list[i].HeatingLimit}" + ",").PadRight(27, ' ') + " !-Heating Limit\n" +
                    ($"  {list[i].MaximumHeatingAirFlowRate}" + ",").PadRight(27, ' ') + " !-Maximum Heating Air Flow Rate{{ m3 / s }}\n" +
                    ($"  {list[i].MaximumSensibleHeatingCapacity}" + ",").PadRight(27, ' ') + " !-Maximum Sensible Heating Capacity{{ W }}\n" +
                    ($"  {list[i].CoolingLimit}" + ",").PadRight(27, ' ') + " !-Cooling Limit\n" +
                    ($"  {list[i].MaximumCoolingAirFlowRate}" + ",").PadRight(27, ' ') + " !-Maximum Cooling Air Flow Rate{{ m3 / s }}\n" +
                    ($"  {list[i].MaximumTotalCoolingCapacity}" + ",").PadRight(27, ' ') + " !-Maximum Total Cooling Capacity{{ W }}\n" +
                    ($"  {list[i].HeatingAvailabilityScheduleName}" + ",").PadRight(27, ' ') + " !-Heating Availability Schedule Name\n" +
                    ($"  {list[i].CoolingAvailabilityScheduleName}" + ",").PadRight(27, ' ') + " !-Cooling Availability Schedule Name\n" +
                    ($"  {list[i].DehumidificationControlType}" + ",").PadRight(27, ' ') + " !-Dehumidification Control Type\n" +
                    ($"  {list[i].CoolingSensibleHeatRatio}" + ",").PadRight(27, ' ') + " !-Cooling Sensible Heat Ratio{{ dimensionless }}\n" +
                    ($"  {list[i].HumidificationControlType}" + ",").PadRight(27, ' ') + " !-Humidification Control Type\n" +
                    ($"  {list[i].DesignSpecificationOutdoorAirObjectName}" + ",").PadRight(27, ' ') + " !-Design Specification Outdoor Air Object Name\n" +
                    ($"  {list[i].OutdoorAirInletNodeName}" + ",").PadRight(27, ' ') + " !-Outdoor Air Inlet Node Name\n" +
                    ($"  {list[i].DemandControlledVentilationType}" + ",").PadRight(27, ' ') + " !-Demand Controlled Ventilation Type\n" +
                    ($"  {list[i].OutdoorAirEconomizerType}" + ",").PadRight(27, ' ') + " !-Outdoor Air Economizer Type\n" +
                    ($"  {list[i].HeatRecoveryType}" + ",").PadRight(27, ' ') + " !-Heat Recovery Type\n" +
                    ($"  {list[i].SensibleHeatRecoveryEffectiveness}" + ",").PadRight(27, ' ') + " !-Sensible Heat Recovery Effectiveness{{ dimensionless }}\n" +
                    ($"  {list[i].LatentHeatRecoveryEffectiveness}" + ",").PadRight(27, ' ') + " !-Latent Heat Recovery Effectiveness{{ dimensionless }}\n" +
                    ($"  {list[i].DesignSpecificationZoneHVACSizingObjectName}" + ";").PadRight(27, ' ') + " !-Design Specification ZoneHVAC Sizing Object Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneHVACIdealLoadsAirSystem.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(ZoneHVACForcedAirUnits.ZoneHVACIdealLoadsAirSystem zoneHVACIdeal)
        {
            zoneHVACIdeal.Name = null;
            zoneHVACIdeal.AvailabilityScheduleName = null;
            zoneHVACIdeal.ZoneSupplyAirNodeName = null;
            zoneHVACIdeal.ZoneExhaustAirNodeName = null;
            zoneHVACIdeal.SystemInletAirNodeName = null;
            zoneHVACIdeal.MaximumHeatingSupplyAirTemperature = null;
            zoneHVACIdeal.MinimumCoolingSupplyAirTemperature = null;
            zoneHVACIdeal.MaximumHeatingSupplyAirHumidityRatio = null;
            zoneHVACIdeal.MinimumCoolingSupplyAirHumidityRatio = null;
            zoneHVACIdeal.HeatingLimit = null;
            zoneHVACIdeal.MaximumHeatingAirFlowRate = null;
            zoneHVACIdeal.MaximumSensibleHeatingCapacity = null;
            zoneHVACIdeal.CoolingLimit = null;
            zoneHVACIdeal.MaximumCoolingAirFlowRate = null;
            zoneHVACIdeal.MaximumTotalCoolingCapacity = null;
            zoneHVACIdeal.HeatingAvailabilityScheduleName = null;
            zoneHVACIdeal.CoolingAvailabilityScheduleName = null;
            zoneHVACIdeal.DehumidificationControlType = null;
            zoneHVACIdeal.CoolingSensibleHeatRatio = null;
            zoneHVACIdeal.HumidificationControlType = null;
            zoneHVACIdeal.DesignSpecificationOutdoorAirObjectName = null;
            zoneHVACIdeal.OutdoorAirInletNodeName = null;
            zoneHVACIdeal.DemandControlledVentilationType = null;
            zoneHVACIdeal.OutdoorAirEconomizerType = null;
            zoneHVACIdeal.HeatRecoveryType = null;
            zoneHVACIdeal.SensibleHeatRecoveryEffectiveness = null;
            zoneHVACIdeal.LatentHeatRecoveryEffectiveness = null;
            zoneHVACIdeal.DesignSpecificationZoneHVACSizingObjectName = null;

        }
    }
}
