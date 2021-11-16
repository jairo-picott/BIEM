using System.Collections.Generic;
using System.IO;

namespace BIEM.InternalGains
{
    public enum NumberofPeopleCalculationMethodType
    {
        People,
        PeopleArea,
        AreaPerson
    }

    public enum EnableASHRAE55ComfortWarningsType
    {
        No,
        Yes
    }

    public enum MeanRadiantTemperatureCalculationType
    {
        ZoneAveraged,
        SurfaceWeighted,
        AngleFactor

    }

    public enum ClothingIsulationCalculationMethodType
    {
        ClothingIsulationSchedule,
        DynamicClothingModelASHRAE55,
        CalculationMethodSchedule
    }

    public enum ThermalComfortModelType
    {
        Fanger,
        Pierce,
        KSU,
        AdaptiveASH55,
        AdaptiveCEN15251,
        CoolingEffectASH55,
        AnkleDraftASH55
    }
    //----------- People
    //PEOPLE CLASS COLLECT ACTIVITY SCHEDULE INFORMATION FOR EACH ZONE
    //AND GAINS PER PERSON, RELATED WITH THE CAPCITY OF THE ZONE
    //USING RELATION AREA/PERSON, PART OF THE ENERGY PROPERTY SET IN 
    //IFC SPACE AND RELATED WITH IFC ZONES
    public class People
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private string _zoneorZoneListName;
        public string ZoneorZoneListName
        {
            get => _zoneorZoneListName;
            set => _zoneorZoneListName = value;
        }

        private string _numberofPeopleScheduleName;
        public string NumberofpeopleScheduleName
        {
            get => _numberofPeopleScheduleName;
            set => _numberofPeopleScheduleName = value;
        }

        private NumberofPeopleCalculationMethodType? _numberofPeopleCalculationMethod = NumberofPeopleCalculationMethodType.People;
        public NumberofPeopleCalculationMethodType? NumberofPeopleCalculationMethod
        {
            get => _numberofPeopleCalculationMethod;
            set => _numberofPeopleCalculationMethod = value;
        }

        private double? _numberofPeople;
        public double? NumberofPeople
        {
            get => _numberofPeople;
            set
            {
                if (value >= 0)
                {
                    _numberofPeople = value;
                }
            }
        }

        private double? _peopleperZoneFloorArea;
        public double? PeopleperZoneFloorArea
        {
            get => _peopleperZoneFloorArea;
            set
            {
                if (value >= 0)
                {
                    _peopleperZoneFloorArea = value;
                }
            }
        }

        private double? _zoneFloorAreaperPerson;
        public double? ZoneFloorAreaperPerson
        {
            get => _zoneFloorAreaperPerson;
            set
            {
                if (value >= 0)
                {
                    _zoneFloorAreaperPerson = value;
                }
            }
        }

        private double? _fractionRadiant = 0.3;
        public double? FractionRadiant
        {
            get => _fractionRadiant;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _fractionRadiant = value;
                }
            }
        }
        private double? _sensibleHeatFraction;
        public double? SensibleHeatFraction
        {
            get => _sensibleHeatFraction;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _sensibleHeatFraction = value;
                }
            }
        }

        private string _activityLevelScheduleName;
        public string ActivityLevelScheduleName
        {
            get => _activityLevelScheduleName;
            set => _activityLevelScheduleName = value;
        }

        private double? _carbonDioxideGenerationRate = 0.0000000382;
        public double? CarbonDioxideGenerationRate
        {
            get => _carbonDioxideGenerationRate;
            set
            {
                if (value >= 0 && value <= 0.000000382)
                {
                    _carbonDioxideGenerationRate = value;
                }
            }
        }

        private EnableASHRAE55ComfortWarningsType? _enableASHRAE55ComfortWarnings;
        public EnableASHRAE55ComfortWarningsType? EnableASHRAE55ComfortWarning
        {
            get => _enableASHRAE55ComfortWarnings;
            set => _enableASHRAE55ComfortWarnings = value;
        }

        private MeanRadiantTemperatureCalculationType? _meanRadianteTemperatureCalculationType;
        public MeanRadiantTemperatureCalculationType? MeanRadiantTemperatureCalculationType
        {
            get => _meanRadianteTemperatureCalculationType;
            set => _meanRadianteTemperatureCalculationType = value;
        }

        private string _surfaceNameAngleFactorListName;
        public string SurfaceNameAngleFactorListName
        {
            get => _surfaceNameAngleFactorListName;
            set => _surfaceNameAngleFactorListName = value;
        }

        private string _workEfficiencyScheduleName;
        public string WorkEfficiencyScheduleName
        {
            get => _workEfficiencyScheduleName;
            set => _workEfficiencyScheduleName = value;
        }

        private ClothingIsulationCalculationMethodType? _clothingIsulationCalculationMethod;
        public ClothingIsulationCalculationMethodType? ClothingInsulationCalculationMethod
        {
            get => _clothingIsulationCalculationMethod;
            set => _clothingIsulationCalculationMethod = value;
        }

        private string _clothingIsulationCalculationMethodScheduleName;
        public string ClothingInsulationCalculationMethodScheduleName
        {
            get => _clothingIsulationCalculationMethodScheduleName;
            set => _clothingIsulationCalculationMethodScheduleName = value;
        }

        private string _clothingIsulationScheduleName;
        public string ClothingInsulationScheduleName
        {
            get => _clothingIsulationScheduleName;
            set => _clothingIsulationScheduleName = value;
        }

        private string _airVelocityScheduleName;
        public string AirVelocityScheduleName
        {
            get => _airVelocityScheduleName;
            set => _airVelocityScheduleName = value;
        }
        private ThermalComfortModelType? _thermalComfortModel1Type;
        public ThermalComfortModelType? ThermalComfortModel1Type
        {
            get => _thermalComfortModel1Type;
            set => _thermalComfortModel1Type = value;
        }

        private ThermalComfortModelType? _thermalComfortModel2Type;
        public ThermalComfortModelType? ThermalComfortModel2Type
        {
            get => _thermalComfortModel2Type;
            set => _thermalComfortModel2Type = value;
        }

        private ThermalComfortModelType? _thermalComfortModel3Type;
        public ThermalComfortModelType? ThermalComfortModel3Type
        {
            get => _thermalComfortModel3Type;
            set => _thermalComfortModel3Type = value;
        }

        private ThermalComfortModelType? _thermalComfortModel4Type;
        public ThermalComfortModelType? ThermalComfortModel4Type
        {
            get => _thermalComfortModel4Type;
            set => _thermalComfortModel4Type = value;
        }

        private ThermalComfortModelType? _thermalComfortModel5Type;
        public ThermalComfortModelType? ThermalComfortModel5Type
        {
            get => _thermalComfortModel5Type;
            set => _thermalComfortModel5Type = value;
        }

        private ThermalComfortModelType? _thermalComfortModel6Type;
        public ThermalComfortModelType? ThermalComfortModel6Type
        {
            get => _thermalComfortModel6Type;
            set => _thermalComfortModel6Type = value;
        }

        private ThermalComfortModelType? _thermalComfortModel7Type;
        public ThermalComfortModelType? ThermalComfortModel7Type
        {
            get => _thermalComfortModel7Type;
            set => _thermalComfortModel7Type = value;
        }

        private string _ankleLevelAirVelocityScheduleName;
        public string AnkleLevelAirVelocityScheduleName
        {
            get => _ankleLevelAirVelocityScheduleName;
            set => _ankleLevelAirVelocityScheduleName = value;
        }

        public People() { }

        private static List<People> PeopleList = new List<People>();

        public static void Add(People people)
        {
            PeopleList.Add(people);
        }
        private static string[] Read()
        {
            string[] print = new string[PeopleList.Count];
            for (int i = 0; i < PeopleList.Count; i++)
            {
                print[i] = $"!!--------------------------------------------------------\n" +
                    $"People,\n" +
                    ($"  {PeopleList[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {PeopleList[i].ZoneorZoneListName}" + ",").PadRight(27, ' ') + " !-Zone or ZoneList Name\n" +
                    ($"  {PeopleList[i].NumberofpeopleScheduleName}" + ",").PadRight(27, ' ') + " !-Number of People Schedule Name\n" +
                    ($"  {PeopleList[i].NumberofPeopleCalculationMethod}" + ",").PadRight(27, ' ') + " !-Number of People Calculation Method\n" +
                    ($"  {PeopleList[i].NumberofPeople}" + ",").PadRight(27, ' ') + " !-Number of People\n" +
                    ($"  {PeopleList[i].PeopleperZoneFloorArea}" + ",").PadRight(27, ' ') + " !-People per Zone Floor Area {{ person / m2}}\n" +
                    ($"  {PeopleList[i].ZoneFloorAreaperPerson}" + ",").PadRight(27, ' ') + " !-Zone Floor Area per Person {{ m2 / person}}\n" +
                    ($"  {PeopleList[i].FractionRadiant}" + ",").PadRight(27, ' ') + " !-Fraction Radiant\n" +
                    ($"  {PeopleList[i].SensibleHeatFraction}" + ",").PadRight(27, ' ') + " !-Sensible Heat Fraction\n" +
                    ($"  {PeopleList[i].ActivityLevelScheduleName}" + ",").PadRight(27, ' ') + " !-Activity Level Schedule Name\n" +
                    ($"  {PeopleList[i].CarbonDioxideGenerationRate}" + ",").PadRight(27, ' ') + " !-Carbon Dioxide Generation Rate{{ m3 / s-W}}\n" +
                    ($"  {PeopleList[i].EnableASHRAE55ComfortWarning}" + ",").PadRight(27, ' ') + " !-Enable ASHRAE55 Comfort Warning\n" +
                    ($"  {PeopleList[i].MeanRadiantTemperatureCalculationType}" + ",").PadRight(27, ' ') + " !-Mean Radiant Temperature Calculation Type\n" +
                    ($"  {PeopleList[i].SurfaceNameAngleFactorListName}" + ",").PadRight(27, ' ') + " !-Surface Name Angle Factor List Name\n" +
                    ($"  {PeopleList[i].WorkEfficiencyScheduleName}" + ",").PadRight(27, ' ') + " !-Work Efficiency Schedule Name\n" +
                    ($"  {PeopleList[i].ClothingInsulationCalculationMethod}" + ",").PadRight(27, ' ') + " !-Clothing Insulation Calculation Method\n" +
                    ($"  {PeopleList[i].ClothingInsulationCalculationMethodScheduleName}" + ",").PadRight(27, ' ') + " !-Clothing Insulation Calculation Method Schedule Name\n" +
                    ($"  {PeopleList[i].ClothingInsulationScheduleName}" + ",").PadRight(27, ' ') + " !-Clothing Insulation Schedule Name\n" +
                    ($"  {PeopleList[i].AirVelocityScheduleName}" + ",").PadRight(27, ' ') + " !-Air Velocity Schedule Name\n" +
                    ($"  {PeopleList[i].ThermalComfortModel1Type}" + ",").PadRight(27, ' ') + " !-Thermal Comfort Model 1 Type\n" +
                    ($"  {PeopleList[i].ThermalComfortModel2Type}" + ",").PadRight(27, ' ') + " !-Thermal Comfort Model 2 Type\n" +
                    ($"  {PeopleList[i].ThermalComfortModel3Type}" + ",").PadRight(27, ' ') + " !-Thermal Comfort Model 3 Type\n" +
                    ($"  {PeopleList[i].ThermalComfortModel4Type}" + ",").PadRight(27, ' ') + " !-Thermal Comfort Model 4 Type\n" +
                    ($"  {PeopleList[i].ThermalComfortModel5Type}" + ",").PadRight(27, ' ') + " !-Thermal Comfort Model 5 Type\n" +
                    ($"  {PeopleList[i].ThermalComfortModel6Type}" + ",").PadRight(27, ' ') + " !-Thermal Comfort Model 6 Type\n" +
                    ($"  {PeopleList[i].ThermalComfortModel7Type}" + ",").PadRight(27, ' ') + " !-Thermal Comfort Model 7 Type\n" +
                    ($"  {PeopleList[i].AnkleLevelAirVelocityScheduleName}" + ";").PadRight(27, ' ') + " !-Ankle Level Air Velocity Schedule Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in People.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(InternalGains.People people)
        {
            people.Name = null;
            people.ZoneorZoneListName = null;
            people.NumberofpeopleScheduleName = null;
            people.NumberofPeopleCalculationMethod = null;
            people.NumberofPeople = null;
            people.PeopleperZoneFloorArea = null;
            people.ZoneFloorAreaperPerson = null;
            people.FractionRadiant = null;
            people.SensibleHeatFraction = null;
            people.ActivityLevelScheduleName = null;
            people.CarbonDioxideGenerationRate = null;
            people.EnableASHRAE55ComfortWarning = null;
            people.MeanRadiantTemperatureCalculationType = null;
            people.SurfaceNameAngleFactorListName = null;
            people.WorkEfficiencyScheduleName = null;
            people.ClothingInsulationCalculationMethod = null;
            people.ClothingInsulationCalculationMethodScheduleName = null;
            people.ClothingInsulationScheduleName = null;
            people.AirVelocityScheduleName = null;
            people.ThermalComfortModel1Type = null;
            people.ThermalComfortModel2Type = null;
            people.ThermalComfortModel3Type = null;
            people.ThermalComfortModel4Type = null;
            people.ThermalComfortModel5Type = null;
        }

    }
}
