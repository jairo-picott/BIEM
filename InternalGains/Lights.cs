using System.Collections.Generic;
using System.IO;

namespace BIEM.InternalGains
{
    public enum DesignLevelCalculationMethodLightingLevelType
    {
        LightingLevel,
        WattsArea,
        WattsPerson
    }
    public enum ReturnAirFractionCalculatedfromPlenumTemperatureType
    {
        Yes,
        No
    }
    //Lights
    //USE THE ACTIVITY SCHEDULES FROM EACH ZONE TO DESCRIBE THE USAGE OF
    //LIGHTS TO CALCULATE THE INTERNAL GAINS FROM LIGHTING, IT IS ALSO
    //OBTAINED FROM THE ENERGY PROPERTY SET OF IFC SPACES, THEN CONNECTED
    //WITH THE RESPECTIVELY IFC ZONE
    public class Lights
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
        private string _scheduleName;
        public string ScheduleName
        {
            get => _scheduleName;
            set => _scheduleName = value;
        }
        private DesignLevelCalculationMethodLightingLevelType? _designLevelCalculationMethod = DesignLevelCalculationMethodLightingLevelType.LightingLevel;
        public DesignLevelCalculationMethodLightingLevelType? DesignLevelCalculationMethod
        {
            get => _designLevelCalculationMethod;
            set => _designLevelCalculationMethod = value;
        }
        private double? _lightingLevel;
        public double? LightingLevel
        {
            get => _lightingLevel;
            set
            {
                if (value >= 0)
                {
                    _lightingLevel = value;
                }
            }
        }
        private double? _wattsperZoneFloorArea;
        public double? WattsperZoneFloorArea
        {
            get => _wattsperZoneFloorArea;
            set
            {
                if (value >= 0)
                {
                    _wattsperZoneFloorArea = value;
                }

            }
        }
        private double? _wattsperPerson;
        public double? WattsperPerson
        {
            get => _wattsperPerson;
            set
            {
                if (value >= 0)
                {
                    _wattsperPerson = value;
                }

            }
        }
        private double? _returnAirFraction = 0;
        public double? ReturnAirFraction
        {
            get => _returnAirFraction;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _returnAirFraction = value;
                }

            }
        }
        private double? _fractionRadiant = 0;
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
        private double? _fractionVisible = 0;
        public double? FractionVisible
        {
            get => _fractionVisible;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _fractionVisible = value;
                }

            }
        }
        private double? _fractionReplaceable = 1;
        public double? FractionReplaceable
        {
            get => _fractionReplaceable;
            set
            {
                if (value >= 0 && value <= 1)
                {
                    _fractionReplaceable = value;
                }

            }
        }
        private string _endUseSubCategory;
        public string EndUseSubcategory
        {
            get => _endUseSubCategory;
            set => _endUseSubCategory = value;
        }
        private ReturnAirFractionCalculatedfromPlenumTemperatureType? _returnAirFractionCalculatedfromPlenumTemperature = ReturnAirFractionCalculatedfromPlenumTemperatureType.No;
        public ReturnAirFractionCalculatedfromPlenumTemperatureType? ReturnAirFractionCalculatedfromPlenumTemperature
        {
            get => _returnAirFractionCalculatedfromPlenumTemperature;
            set => _returnAirFractionCalculatedfromPlenumTemperature = value;
        }
        private double? _returnAirFractionFunctionofPlenumTemperatureCoefficient1 = 0;
        public double? ReturnAirFractionFunctionofPlenumTemperatureCoefficient1
        {
            get => _returnAirFractionFunctionofPlenumTemperatureCoefficient1;
            set
            {
                if (value >= 0)
                {
                    _returnAirFractionFunctionofPlenumTemperatureCoefficient1 = value;
                }

            }
        }
        private double? _returnAirFractionFunctionofPlenumTemperatureCoefficient2 = 0;
        public double? ReturnAirFractionFunctionofPlenumTemperatureCoefficient2
        {
            get => _returnAirFractionFunctionofPlenumTemperatureCoefficient2;
            set
            {
                if (value >= 0)
                {
                    _returnAirFractionFunctionofPlenumTemperatureCoefficient2 = value;
                }

            }
        }
        private string _returnAirHeatGainNodeName;
        public string ReturnAirHeatGainNodeName
        {
            get => _returnAirHeatGainNodeName;
            set => _returnAirHeatGainNodeName = value;
        }

        public Lights() { }
        private static List<Lights> list = new List<Lights>();

        public static void Add(Lights lights)
        {
            list.Add(lights);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Lights,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ZoneorZoneListName}" + ",").PadRight(27, ' ') + " !-Zone or ZoneList Name\n" +
                    ($"  {list[i].ScheduleName}" + ",").PadRight(27, ' ') + " !-Schedule Name\n" +
                    ($"  {list[i].DesignLevelCalculationMethod}" + ",").PadRight(27, ' ') + " !-Design Level Calculation Method\n" +
                    ($"  {list[i].LightingLevel}" + ",").PadRight(27, ' ') + " !-Lightin Level{{ W }}\n" +
                    ($"  {list[i].WattsperZoneFloorArea}" + ",").PadRight(27, ' ') + " !-Watts per Zone Floor Area {{ W / m2}}\n" +
                    ($"  {list[i].WattsperPerson}" + ",").PadRight(27, ' ') + " !-Watts per Person {{ W / person}}\n" +
                    ($"  {list[i].ReturnAirFraction}" + ",").PadRight(27, ' ') + " !-Return Air Fraction\n" +
                    ($"  {list[i].FractionRadiant}" + ",").PadRight(27, ' ') + " !-Fraction Radiant\n" +
                    ($"  {list[i].FractionVisible}" + ",").PadRight(27, ' ') + " !-Fraction Visible\n" +
                    ($"  {list[i].FractionReplaceable}" + ",").PadRight(27, ' ') + " !-Fraction Replaceable\n" +
                    ($"  {list[i].EndUseSubcategory}" + ",").PadRight(27, ' ') + " !-End Use Subcategory\n" +
                    ($"  {list[i].ReturnAirFractionCalculatedfromPlenumTemperature}" + ",").PadRight(27, ' ') + " !-Return Air Fraction Calculated from Plenum Temperature\n" +
                    ($"  {list[i].ReturnAirFractionFunctionofPlenumTemperatureCoefficient1}" + ",").PadRight(27, ' ') + " !-Return Air Fraction Function of Plenum Temperature Coefficient 1\n" +
                    ($"  {list[i].ReturnAirFractionFunctionofPlenumTemperatureCoefficient2}" + ",").PadRight(27, ' ') + " !-Return Air Fraction Function of Plenum Temperature Coefficient 2{{ 1 / K}}\n" +
                    ($"  {list[i].ReturnAirHeatGainNodeName}" + ";").PadRight(27, ' ') + " !-Return Air Heat Gain Node Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in Lights.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(InternalGains.Lights lights)
        {
            lights.Name = null;
            lights.ZoneorZoneListName = null;
            lights.ScheduleName = null;
            lights.DesignLevelCalculationMethod = null;
            lights.LightingLevel = null;
            lights.WattsperZoneFloorArea = null;
            lights.WattsperPerson = null;
            lights.ReturnAirFraction = null;
            lights.FractionRadiant = null;
            lights.FractionVisible = null;
            lights.FractionReplaceable = null;
            lights.EndUseSubcategory = null;
            lights.ReturnAirFractionCalculatedfromPlenumTemperature = null;
            lights.ReturnAirFractionFunctionofPlenumTemperatureCoefficient1 = null;
            lights.ReturnAirFractionFunctionofPlenumTemperatureCoefficient2 = null;
            lights.ReturnAirHeatGainNodeName = null;
        }
    }
}

