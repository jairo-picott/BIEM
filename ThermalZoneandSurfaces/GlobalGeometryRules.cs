using System.IO;
using System.Collections.Generic;

namespace BIEM.ThermalZonesandSurfaces
{
    public enum StartingVertexPositionEnum
    {
        UpperLeftCorner,
        LowerLeftCorner,
        UpperRightCorner,
        LoverRightCorner
    }

    public enum VertexEntryDirectionEnum
    {
        CounterClockwise,
        Clockwise
    }

    public enum CoordinateSystemEnum
    {
        Relative,
        World
    }

    public enum DaylightingReferencePointCoordinateSystemEnum
    {
        Relative,
        World
    }

    public enum RectangularSurfaceCoordinateSystemEnum
    {
        Relative,
        World
    }
    //--------------Global Geometry Rules
    //THIS CLASS DEFINE THE GEOMETRY RULES, IN ORDER TO CREATE A GENERIC
    //PROCESS FOR OBTAINING THE GEOMETRY, IS DEFINED AS FIXED VALUES
    //IN FUTURE CAN BE CHAGE TO GIVE THE OPTION TO THE USER, IT WILL 
    //CREATE THE NEED TO CHANGE THE ORDER FOR PRINTING VERTICES IN 
    //BUILDING SURFEACES.
    public class GlobalGeometryRules
    {
        private StartingVertexPositionEnum? _startingVertexPosition;
        public StartingVertexPositionEnum? StartingVertexPosition { get=>_startingVertexPosition; set=>_startingVertexPosition=value; }

        private VertexEntryDirectionEnum? _vertexEntryDirection;
        public VertexEntryDirectionEnum? VertexEntryDirection { get=>_vertexEntryDirection; set=>_vertexEntryDirection=value; }

        private CoordinateSystemEnum? _coordinateSystem;
        public CoordinateSystemEnum? CoordinateSystem { get=>_coordinateSystem; set=>_coordinateSystem=value; }

        private DaylightingReferencePointCoordinateSystemEnum? _daylightingReferencePointCoordinateSystem;
        public DaylightingReferencePointCoordinateSystemEnum? DaylightingReferencePointCoordinateSystem { get=>_daylightingReferencePointCoordinateSystem; set=>_daylightingReferencePointCoordinateSystem=value; }

        private RectangularSurfaceCoordinateSystemEnum? _rectangularSurfaceCoordinateSystem;
        public RectangularSurfaceCoordinateSystemEnum? RectangularSurfaceCoordinateSystem { get=>_rectangularSurfaceCoordinateSystem; set=>_rectangularSurfaceCoordinateSystem=value; }

        public GlobalGeometryRules() { }

        private static List<GlobalGeometryRules> list = new List<GlobalGeometryRules>();

        public static void Add(GlobalGeometryRules globalGeometryRules)
        {
            list.Add(globalGeometryRules);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"GlobalGeometryRules,\n" +
                    ($"  {list[i].StartingVertexPosition}" + ",").PadRight(27, ' ') + " !-Starting Vertex Position\n" +
                    ($"  {list[i].VertexEntryDirection}" + ",").PadRight(27, ' ') + " !-Vertex Entry Direction\n" +
                    ($"  {list[i].CoordinateSystem}" + ",").PadRight(27, ' ') + " !-Coordinate System\n" +
                    ($"  {list[i].DaylightingReferencePointCoordinateSystem}" + ",").PadRight(27, ' ') + " !-Daylighting Reference Point Coordinate System\n" +
                    ($"  {list[i].RectangularSurfaceCoordinateSystem}" + ";").PadRight(27, ' ') + " !-Rectangular Surface Coordinate System";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in GlobalGeometryRules.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Collect(GlobalGeometryRules geometryRules)
        {
            geometryRules.StartingVertexPosition = null;
            geometryRules.VertexEntryDirection = null;
            geometryRules.CoordinateSystem = null;
            geometryRules.DaylightingReferencePointCoordinateSystem = null;
            geometryRules.RectangularSurfaceCoordinateSystem = null;
        }
    }
}
