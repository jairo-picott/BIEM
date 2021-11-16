using System.IO;
using System.Collections.Generic;
using System;

namespace BIEM.ThermalZonesandSurfaces
{
    public enum ZoneInsideConvectionAlgorithmEnum
    {
        Simple,
        TARP,
        CeilingDiffuser,
        AdaptiveConvectionAlgorithm,
        TrombeWall,
        ASTMC1340
    }

    public enum ZoneOutsideConvectionAlgorithmEnum
    {
        SimpleCombined,
        TARP,
        DOE2,
        MoWiTT,
        AdaptiveConvectionAlgorithm
    }

    public enum PartofTotalAreEnum
    {
        Yes,
        No
    }
    //------------Zones
    //COLLECT DATA FROM IFCZONE, AND BASIC PROPERTIES AS VOLUME, AREA
    // CEILING HEIGH, ALSO IDENTIFY IF THE ZONE REPRESENT PLENUM OR NOT
    public class Zone
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private double? _directionofRelativeNorth = 0;
        public double? DirectionofRelativeNorth { get=>_directionofRelativeNorth; set=>_directionofRelativeNorth=value; }

        private double? _xOrigin = 0;
        public double? XOrigin { get=>_xOrigin; set=>_xOrigin=value; }

        private double? _yOrigin = 0;
        public double? YOrigin { get=>_yOrigin; set=>_yOrigin=value; }

        private double? _zOrigin = 0;
        public double? ZOrigin { get=>_zOrigin; set=>_zOrigin=value; }

        private int? _type = 1;
        public int? Type 
        { 
            get =>_type; 
            set
            {
                if (value <=1 && value >=1)
                {
                    _type = value;
                }
            }
        }

        private int? _multiplier = 1;
        public int? Multiplier
        {
            get => _multiplier;
            set
            {
                if (value >= 1)
                {
                    _multiplier = value;
                }
            }
        }

        private string _ceilingHeight = "autocalculate";
        public string CeilingHeight { get=>_ceilingHeight; set=>_ceilingHeight=value; }

        private string _volume = "autocalculate";
        public string Volume { get=>_volume; set=>_volume=value; }

        private string _floorArea = "autocalculate";
        public string FloorArea { get=>_floorArea; set=>_floorArea=value; }

        private ZoneInsideConvectionAlgorithmEnum? _zoneInsideConvectionAlgorithm;
        public ZoneInsideConvectionAlgorithmEnum? ZoneInsideConvectionAlgorithm { get=>_zoneInsideConvectionAlgorithm; set=>_zoneInsideConvectionAlgorithm=value; }

        private ZoneOutsideConvectionAlgorithmEnum? _zoneOutsideConvectionAlgorithm;
        public ZoneOutsideConvectionAlgorithmEnum? ZoneOutsideConvectionAlgorithm { get=>_zoneOutsideConvectionAlgorithm; set=>_zoneOutsideConvectionAlgorithm=value; }

        private PartofTotalAreEnum? _partofTotalArea;
        public PartofTotalAreEnum? PartofTotalFloorArea { get=>_partofTotalArea; set=>_partofTotalArea=value; }

        public Zone() { }

        private static List<Zone> list = new List<Zone>();

        public static void Add(Zone zone)
        {
            list.Add(zone);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Zone,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].DirectionofRelativeNorth}" + ",").PadRight(27, ' ') + " !-Direction of Relative North\n" +
                    ($"  {list[i].XOrigin}" + ",").PadRight(27, ' ') + " !-X Origin\n" +
                    ($"  {list[i].YOrigin}" + ",").PadRight(27, ' ') + " !-Y Origin\n" +
                    ($"  {list[i].ZOrigin}" + ",").PadRight(27, ' ') + " !-Z Origin\n" +
                    ($"  {list[i].Type}" + ",").PadRight(27, ' ') + " !-Type\n" +
                    ($"  {list[i].Multiplier}" + ",").PadRight(27, ' ') + " !-Multiplier\n" +
                    ($"  {list[i].CeilingHeight}" + ",").PadRight(27, ' ') + " !-Ceiling Height\n" +
                    ($"  {list[i].Volume}" + ",").PadRight(27, ' ') + " !-Volume\n" +
                    ($"  {list[i].FloorArea}" + ",").PadRight(27, ' ') + " !-Floor Area\n" +
                    ($"  {list[i].ZoneInsideConvectionAlgorithm}" + ",").PadRight(27, ' ') + " !-Zone Inside Convection Algorithm\n" +
                    ($"  {list[i].ZoneOutsideConvectionAlgorithm}" + ",").PadRight(27, ' ') + " !-Zone Outside Convection Algorithm\n" +
                    ($"  {list[i].PartofTotalFloorArea}" + ";").PadRight(27, ' ') + " !-Part of Total Floor Area";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in Zone.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {
           
            foreach (string line in Zone.Read())
            {
                Console.WriteLine(line);
            }
            

        }
        public static void Collect(Zone zone)
        {
            zone.Name = null;
            zone.DirectionofRelativeNorth = null;
            zone.XOrigin = null;
            zone.YOrigin = null;
            zone.ZOrigin = null;
            zone.Type = null;
            zone.Multiplier = null;
            zone.CeilingHeight = null;
            zone.Volume = null;
            zone.FloorArea = null;
            zone.ZoneInsideConvectionAlgorithm = null;
            zone.ZoneOutsideConvectionAlgorithm = null;
            zone.PartofTotalFloorArea = null;
        }

    }
}
