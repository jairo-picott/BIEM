using System.Collections.Generic;
using System.IO;

namespace BIEM.InternalGains
{
    public enum DesignLevelCalculationMethodElectricEquipmentType
    {
        EquipmentLevel,
        WattsArea,
        WattsPerson
    }
    public class ElectricEquipments
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
        private DesignLevelCalculationMethodElectricEquipmentType? _designLevelCalculationMethod = DesignLevelCalculationMethodElectricEquipmentType.EquipmentLevel;
        public DesignLevelCalculationMethodElectricEquipmentType? DesignLevelCalculationMethod
        {
            get => _designLevelCalculationMethod;
            set => _designLevelCalculationMethod = value;
        }
        private double? _designLevel;
        public double? DesignLevel
        {
            get => _designLevel;
            set
            {
                if (value >= 0)
                {
                    _designLevel = value;
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
        private double? _fractionLatent = 0;
        public double? FractionLatent
        {
            get => _fractionLatent;
            set
            {
                if (value <= 1 && value >= 0)
                {
                    _fractionLatent = value;
                }
            }
        }
        private double? _fractionRadiant = 0;
        public double? FractionRadiant
        {
            get => _fractionRadiant;
            set
            {
                if (value <= 1 && value >= 0)
                {
                    _fractionRadiant = value;
                }
            }
        }
        private double? _fractionLost = 0;
        public double? FractionLost
        {
            get => _fractionLost;
            set
            {
                if (value <= 1 && value >= 0)
                {
                    _fractionLost = value;
                }
            }
        }
        private string _endUseSubcategory;
        public string EndUseSubcategory
        {
            get => _endUseSubcategory;
            set => _endUseSubcategory = value;
        }

        public ElectricEquipments() { }

        private static List<ElectricEquipments> list = new List<ElectricEquipments>();

        public static void Add(ElectricEquipments electricEquipments)
        {
            list.Add(electricEquipments);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ElectricEquipment,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ZoneorZoneListName}" + ",").PadRight(27, ' ') + " !-Zone or ZoneList Name\n" +
                    ($"  {list[i].ScheduleName}" + ",").PadRight(27, ' ') + " !-Schedule Name\n" +
                    ($"  {list[i].DesignLevelCalculationMethod}" + ",").PadRight(27, ' ') + " !-Design Level Calculation Method\n" +
                    ($"  {list[i].DesignLevel}" + ",").PadRight(27, ' ') + " !-Design Level{{ W }}\n" +
                    ($"  {list[i].WattsperZoneFloorArea}" + ",").PadRight(27, ' ') + " !-Watts per Zone Floor Area {{ W / m2}}\n" +
                    ($"  {list[i].WattsperPerson}" + ",").PadRight(27, ' ') + " !-Watts per Person {{ W / person}}\n" +
                    ($"  {list[i].FractionLatent}" + ",").PadRight(27, ' ') + " !-Fraction Latent\n" +
                    ($"  {list[i].FractionRadiant}" + ",").PadRight(27, ' ') + " !-Fraction Radiant\n" +
                    ($"  {list[i].FractionLost}" + ",").PadRight(27, ' ') + " !-Fraction Lost\n" +
                    ($"  {list[i].EndUseSubcategory}" + ";").PadRight(27, ' ') + " !-End Use Subcategory";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ElectricEquipments.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(ElectricEquipments electricEquipments)
        {
            electricEquipments.Name = null;
            electricEquipments.ZoneorZoneListName = null;
            electricEquipments.ScheduleName = null;
            electricEquipments.DesignLevelCalculationMethod = null;
            electricEquipments.DesignLevel = null;
            electricEquipments.WattsperZoneFloorArea = null;
            electricEquipments.WattsperPerson = null;
            electricEquipments.FractionLatent = null;
            electricEquipments.FractionRadiant = null;
            electricEquipments.FractionLost = null;
            electricEquipments.EndUseSubcategory = null;
        }
    }
}
