using System.IO;
using System.Collections.Generic;

namespace BIEM.ZoneHVACEquipmentConnections
{
    public enum LoadDistributionSchemeEnum
    {
        SequentialLoad,
        UniformLoad,
        UniformPLR,
        SequentialUniformPLR
    }
    public class ZoneHVACEquipmentList
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private LoadDistributionSchemeEnum? _loadDistributionScheme;
        public LoadDistributionSchemeEnum? LoadDistributionScheme { get=>_loadDistributionScheme; set=>_loadDistributionScheme=value; }

        private string _zoneEquipment1ObjectType;
        public string ZoneEquipment1ObjectType { get=>_zoneEquipment1ObjectType; set=>_zoneEquipment1ObjectType=value; }

        private string _zoneEquipment1Name;
        public string ZoneEquipment1Name { get=>_zoneEquipment1Name; set=>_zoneEquipment1Name=value; }

        private double? _zoneEquipment1CoolingSequence;
        public double? ZoneEquipment1CoolingSequence
        {
            get => _zoneEquipment1CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment1CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment1HeatingorNoLoadSequence;
        public double? ZoneEquipment1HeatingorNoLoadSequence
        {
            get => _zoneEquipment1HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment1HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment1SequentialCoolingFractionSchedule;        
        public string ZoneEquipment1SequentialCoolingFractionSchedule { get=>_zoneEquipment1SequentialCoolingFractionSchedule; set=>_zoneEquipment1SequentialCoolingFractionSchedule=value; }

        private string _zoneEquipment1SequentialHeatingFractionSchedule;
        public string ZoneEquipment1SequentialHeatingFractionSchedule { get=>_zoneEquipment1SequentialHeatingFractionSchedule; set=>_zoneEquipment1SequentialHeatingFractionSchedule=value; }

        private string _zoneEquipment2ObjectType;
        public string ZoneEquipment2ObjectType { get => _zoneEquipment2ObjectType; set => _zoneEquipment2ObjectType = value; }

        private string _zoneEquipment2Name;
        public string ZoneEquipment2Name { get => _zoneEquipment2Name; set => _zoneEquipment2Name = value; }

        private double? _zoneEquipment2CoolingSequence;
        public double? ZoneEquipment2CoolingSequence
        {
            get => _zoneEquipment2CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment2CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment2HeatingorNoLoadSequence;
        public double? ZoneEquipment2HeatingorNoLoadSequence
        {
            get => _zoneEquipment2HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment2HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment2SequentialCoolingFractionSchedule;
        public string ZoneEquipment2SequentialCoolingFractionSchedule { get => _zoneEquipment2SequentialCoolingFractionSchedule; set => _zoneEquipment2SequentialCoolingFractionSchedule = value; }

        private string _zoneEquipment2SequentialHeatingFractionSchedule;
        public string ZoneEquipment2SequentialHeatingFractionSchedule { get => _zoneEquipment2SequentialHeatingFractionSchedule; set => _zoneEquipment2SequentialHeatingFractionSchedule = value; }

        private string _zoneEquipment3ObjectType;
        public string ZoneEquipment3ObjectType { get => _zoneEquipment3ObjectType; set => _zoneEquipment3ObjectType = value; }

        private string _zoneEquipment3Name;
        public string ZoneEquipment3Name { get => _zoneEquipment3Name; set => _zoneEquipment3Name = value; }

        private double? _zoneEquipment3CoolingSequence;
        public double? ZoneEquipment3CoolingSequence
        {
            get => _zoneEquipment3CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment3CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment3HeatingorNoLoadSequence;
        public double? ZoneEquipment3HeatingorNoLoadSequence
        {
            get => _zoneEquipment3HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment3HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment3SequentialCoolingFractionSchedule;
        public string ZoneEquipment3SequentialCoolingFractionSchedule { get => _zoneEquipment3SequentialCoolingFractionSchedule; set => _zoneEquipment3SequentialCoolingFractionSchedule = value; }

        private string _zoneEquipment3SequentialHeatingFractionSchedule;
        public string ZoneEquipment3SequentialHeatingFractionSchedule { get => _zoneEquipment3SequentialHeatingFractionSchedule; set => _zoneEquipment3SequentialHeatingFractionSchedule = value; }

        private string _zoneEquipment4ObjectType;
        public string ZoneEquipment4ObjectType { get => _zoneEquipment4ObjectType; set => _zoneEquipment4ObjectType = value; }

        private string _zoneEquipment4Name;
        public string ZoneEquipment4Name { get => _zoneEquipment4Name; set => _zoneEquipment4Name = value; }

        private double? _zoneEquipment4CoolingSequence;
        public double? ZoneEquipment4CoolingSequence
        {
            get => _zoneEquipment4CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment4CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment4HeatingorNoLoadSequence;
        public double? ZoneEquipment4HeatingorNoLoadSequence
        {
            get => _zoneEquipment4HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment4HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment4SequentialCoolingFractionSchedule;
        public string ZoneEquipment4SequentialCoolingFractionSchedule { get => _zoneEquipment4SequentialCoolingFractionSchedule; set => _zoneEquipment4SequentialCoolingFractionSchedule = value; }

        private string _zoneEquipment4SequentialHeatingFractionSchedule;
        public string ZoneEquipment4SequentialHeatingFractionSchedule { get => _zoneEquipment4SequentialHeatingFractionSchedule; set => _zoneEquipment4SequentialHeatingFractionSchedule = value; }

        private string _zoneEquipment5ObjectType;
        public string ZoneEquipment5ObjectType { get => _zoneEquipment5ObjectType; set => _zoneEquipment5ObjectType = value; }

        private string _zoneEquipment5Name;
        public string ZoneEquipment5Name { get => _zoneEquipment5Name; set => _zoneEquipment5Name = value; }

        private double? _zoneEquipment5CoolingSequence;
        public double? ZoneEquipment5CoolingSequence
        {
            get => _zoneEquipment5CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment5CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment5HeatingorNoLoadSequence;
        public double? ZoneEquipment5HeatingorNoLoadSequence
        {
            get => _zoneEquipment5HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment5HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment5SequentialCoolingFractionSchedule;
        public string ZoneEquipment5SequentialCoolingFractionSchedule { get => _zoneEquipment5SequentialCoolingFractionSchedule; set => _zoneEquipment5SequentialCoolingFractionSchedule = value; }

        private string _zoneEquipment5SequentialHeatingFractionSchedule;
        public string ZoneEquipment5SequentialHeatingFractionSchedule { get => _zoneEquipment5SequentialHeatingFractionSchedule; set => _zoneEquipment5SequentialHeatingFractionSchedule = value; }

        private string _zoneEquipment6ObjectType;
        public string ZoneEquipment6ObjectType { get => _zoneEquipment6ObjectType; set => _zoneEquipment6ObjectType = value; }

        private string _zoneEquipment6Name;
        public string ZoneEquipment6Name { get => _zoneEquipment6Name; set => _zoneEquipment6Name = value; }

        private double? _zoneEquipment6CoolingSequence;
        public double? ZoneEquipment6CoolingSequence
        {
            get => _zoneEquipment6CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment6CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment6HeatingorNoLoadSequence;
        public double? ZoneEquipment6HeatingorNoLoadSequence
        {
            get => _zoneEquipment6HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment6HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment6SequentialCoolingFractionSchedule;
        public string ZoneEquipment6SequentialCoolingFractionSchedule { get => _zoneEquipment6SequentialCoolingFractionSchedule; set => _zoneEquipment6SequentialCoolingFractionSchedule = value; }

        private string _zoneEquipment6SequentialHeatingFractionSchedule;
        public string ZoneEquipment6SequentialHeatingFractionSchedule { get => _zoneEquipment6SequentialHeatingFractionSchedule; set => _zoneEquipment6SequentialHeatingFractionSchedule = value; }

        private string _zoneEquipment7ObjectType;
        public string ZoneEquipment7ObjectType { get => _zoneEquipment7ObjectType; set => _zoneEquipment7ObjectType = value; }

        private string _zoneEquipment7Name;
        public string ZoneEquipment7Name { get => _zoneEquipment7Name; set => _zoneEquipment7Name = value; }

        private double? _zoneEquipment7CoolingSequence;
        public double? ZoneEquipment7CoolingSequence
        {
            get => _zoneEquipment7CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment7CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment7HeatingorNoLoadSequence;
        public double? ZoneEquipment7HeatingorNoLoadSequence
        {
            get => _zoneEquipment7HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment7HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment7SequentialCoolingFractionSchedule;
        public string ZoneEquipment7SequentialCoolingFractionSchedule { get => _zoneEquipment7SequentialCoolingFractionSchedule; set => _zoneEquipment7SequentialCoolingFractionSchedule = value; }

        private string _zoneEquipment7SequentialHeatingFractionSchedule;
        public string ZoneEquipment7SequentialHeatingFractionSchedule { get => _zoneEquipment7SequentialHeatingFractionSchedule; set => _zoneEquipment7SequentialHeatingFractionSchedule = value; }

        private string _zoneEquipment8ObjectType;
        public string ZoneEquipment8ObjectType { get => _zoneEquipment8ObjectType; set => _zoneEquipment8ObjectType = value; }

        private string _zoneEquipment8Name;
        public string ZoneEquipment8Name { get => _zoneEquipment8Name; set => _zoneEquipment8Name = value; }

        private double? _zoneEquipment8CoolingSequence;
        public double? ZoneEquipment8CoolingSequence
        {
            get => _zoneEquipment8CoolingSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment8CoolingSequence = value;
                }
            }
        }

        private double? _zoneEquipment8HeatingorNoLoadSequence;
        public double? ZoneEquipment8HeatingorNoLoadSequence
        {
            get => _zoneEquipment8HeatingorNoLoadSequence;
            set
            {
                if (value >= 0)
                {
                    _zoneEquipment8HeatingorNoLoadSequence = value;
                }
            }
        }

        private string _zoneEquipment8SequentialCoolingFractionSchedule;
        public string ZoneEquipment8SequentialCoolingFractionSchedule { get => _zoneEquipment8SequentialCoolingFractionSchedule; set => _zoneEquipment8SequentialCoolingFractionSchedule = value; }

        private string _zoneEquipment8SequentialHeatingFractionSchedule;
        public string ZoneEquipment8SequentialHeatingFractionSchedule { get => _zoneEquipment8SequentialHeatingFractionSchedule; set => _zoneEquipment8SequentialHeatingFractionSchedule = value; }
        public ZoneHVACEquipmentList() { }


        private static List<ZoneHVACEquipmentList> list = new List<ZoneHVACEquipmentList>();

        public static void Add(ZoneHVACEquipmentList zoneHVACEquipmentList)
        {
            list.Add(zoneHVACEquipmentList);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneHVAC:EquipmentList,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].LoadDistributionScheme}" + ",").PadRight(27, ' ') + " !-Loa Distribution Scheme\n" +
                    ($"  {list[i].ZoneEquipment1ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 1 Object Type\n" +
                    ($"  {list[i].ZoneEquipment1Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 1 Name\n" +
                    ($"  {list[i].ZoneEquipment1CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 1 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment1HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 1 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment1SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 1 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment1SequentialHeatingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 1 Sequential Heating Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment2ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 2 Object Type\n" +
                    ($"  {list[i].ZoneEquipment2Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 2 Name\n" +
                    ($"  {list[i].ZoneEquipment2CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 2 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment2HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 2 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment2SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 2 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment2SequentialHeatingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 2 Sequential Heating Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment3ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 3 Object Type\n" +
                    ($"  {list[i].ZoneEquipment3Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 3 Name\n" +
                    ($"  {list[i].ZoneEquipment3CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 3 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment3HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 3 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment3SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 3 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment3SequentialHeatingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 3 Sequential Heating Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment4ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 4 Object Type\n" +
                    ($"  {list[i].ZoneEquipment4Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 4 Name\n" +
                    ($"  {list[i].ZoneEquipment4CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 4 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment4HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 4 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment4SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 4 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment4SequentialHeatingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 4 Sequential Heating Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment5ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 5 Object Type\n" +
                    ($"  {list[i].ZoneEquipment5Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 5 Name\n" +
                    ($"  {list[i].ZoneEquipment5CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 5 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment5HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 5 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment5SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 5 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment5SequentialHeatingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 5 Sequential Heating Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment6ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 6 Object Type\n" +
                    ($"  {list[i].ZoneEquipment6Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 6 Name\n" +
                    ($"  {list[i].ZoneEquipment6CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 6 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment6HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 6 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment6SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 6 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment6SequentialHeatingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 6 Sequential Heating Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment7ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 7 Object Type\n" +
                    ($"  {list[i].ZoneEquipment7Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 7 Name\n" +
                    ($"  {list[i].ZoneEquipment7CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 7 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment7HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 7 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment7SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 7 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment7SequentialHeatingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 7 Sequential Heating Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment8ObjectType}" + ",").PadRight(27, ' ') + " !-Zone Equipment 8 Object Type\n" +
                    ($"  {list[i].ZoneEquipment8Name}" + ",").PadRight(27, ' ') + " !-Zone Equipment 8 Name\n" +
                    ($"  {list[i].ZoneEquipment8CoolingSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 8 Cooling Sequence\n" +
                    ($"  {list[i].ZoneEquipment8HeatingorNoLoadSequence}" + ",").PadRight(27, ' ') + " !-Zone Equipment 8 Heating or No Load Sequence\n" +
                    ($"  {list[i].ZoneEquipment8SequentialCoolingFractionSchedule}" + ",").PadRight(27, ' ') + " !-Zone Equipment 8 Sequential Cooling Fraction Schedule\n" +
                    ($"  {list[i].ZoneEquipment8SequentialHeatingFractionSchedule}" + ";").PadRight(27, ' ') + " !-Zone Equipment 8 Sequential Heating Fraction Schedule";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneHVACEquipmentList.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(ZoneHVACEquipmentList zoneHVACEquipment)
        {
            zoneHVACEquipment.Name = null;
            zoneHVACEquipment.LoadDistributionScheme = null;
            zoneHVACEquipment.ZoneEquipment1ObjectType = null;
            zoneHVACEquipment.ZoneEquipment1Name = null;
            zoneHVACEquipment.ZoneEquipment1CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment1HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment1SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment1SequentialHeatingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment2ObjectType = null;
            zoneHVACEquipment.ZoneEquipment2Name = null;
            zoneHVACEquipment.ZoneEquipment2CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment2HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment2SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment2SequentialHeatingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment3ObjectType = null;
            zoneHVACEquipment.ZoneEquipment3Name = null;
            zoneHVACEquipment.ZoneEquipment3CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment3HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment3SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment3SequentialHeatingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment4ObjectType = null;
            zoneHVACEquipment.ZoneEquipment4Name = null;
            zoneHVACEquipment.ZoneEquipment4CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment4HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment4SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment4SequentialHeatingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment5ObjectType = null;
            zoneHVACEquipment.ZoneEquipment5Name = null;
            zoneHVACEquipment.ZoneEquipment5CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment5HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment5SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment5SequentialHeatingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment6ObjectType = null;
            zoneHVACEquipment.ZoneEquipment6Name = null;
            zoneHVACEquipment.ZoneEquipment6CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment6HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment6SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment6SequentialHeatingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment7ObjectType = null;
            zoneHVACEquipment.ZoneEquipment7Name = null;
            zoneHVACEquipment.ZoneEquipment7CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment7HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment7SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment7SequentialHeatingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment8ObjectType = null;
            zoneHVACEquipment.ZoneEquipment8Name = null;
            zoneHVACEquipment.ZoneEquipment8CoolingSequence = null;
            zoneHVACEquipment.ZoneEquipment8HeatingorNoLoadSequence = null;
            zoneHVACEquipment.ZoneEquipment8SequentialCoolingFractionSchedule = null;
            zoneHVACEquipment.ZoneEquipment8SequentialHeatingFractionSchedule = null;
        }
    }

}
