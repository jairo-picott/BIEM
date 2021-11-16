using System.IO;
using System.Collections.Generic;
using System;

namespace BIEM.ThermalZonesandSurfaces
{
    public enum SurfaceTypeEnumFSD
    {
        Window,
        Door,
        GlassDoor,
        TubularDaylightDome,
        TubularDaylightDiffuser

    }
    public class FenestrationSurfaceDetailed
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private SurfaceTypeEnumFSD? _surfaceType;
        public SurfaceTypeEnumFSD? SurfaceType { get =>_surfaceType; set=>_surfaceType=value; }

        private string _constructionName;
        public string ConstructionName { get=>_constructionName; set=>_constructionName=value; }

        private string _outsideBoundaryConditionObject;
        public string OutsideBoundaryConditionObject { get => _outsideBoundaryConditionObject; set => _outsideBoundaryConditionObject = value; }

        private string _buildingSurfaceName;
        public string BuildingSurfaceName { get=>_buildingSurfaceName; set=>_buildingSurfaceName=value; }

        private string _viewFactortoGround = "autocalculate";
        public string ViewFactortoGround { get=>_viewFactortoGround; set=>_viewFactortoGround=value; }

        private string _frameandDividerName;
        public string FrameandDividerName { get=>_frameandDividerName; set=>_frameandDividerName=value; }

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

        private string _numberofVertices = "autocalculate";
        public string NumberofVertices { get=>_numberofVertices; set=>_numberofVertices=value; }


        private double? _vertex1XCoordinate;
        public double? Vertex1XCoordinate { get => _vertex1XCoordinate; set => _vertex1XCoordinate = value; }

        private double? _vertex1YCoordinate;
        public double? Vertex1YCoordinate { get => _vertex1YCoordinate; set => _vertex1YCoordinate = value; }

        private double? _vertex1ZCoordinate;
        public double? Vertex1ZCoordinate { get => _vertex1ZCoordinate; set => _vertex1ZCoordinate = value; }

        private double? _vertex2XCoordinate;
        public double? Vertex2XCoordinate { get => _vertex2XCoordinate; set => _vertex2XCoordinate = value; }

        private double? _vertex2YCoordinate;
        public double? Vertex2YCoordinate { get => _vertex2YCoordinate; set => _vertex2YCoordinate = value; }

        private double? _vertex2ZCoordinate;
        public double? Vertex2ZCoordinate { get => _vertex2ZCoordinate; set => _vertex2ZCoordinate = value; }

        private double? _vertex3XCoordinate;
        public double? Vertex3XCoordinate { get => _vertex3XCoordinate; set => _vertex3XCoordinate = value; }

        private double? _vertex3YCoordinate;
        public double? Vertex3YCoordinate { get => _vertex3YCoordinate; set => _vertex3YCoordinate = value; }

        private double? _vertex3ZCoordinate;
        public double? Vertex3ZCoordinate { get => _vertex3ZCoordinate; set => _vertex3ZCoordinate = value; }

        private double? _vertex4XCoordinate;
        public double? Vertex4XCoordinate { get => _vertex4XCoordinate; set => _vertex4XCoordinate = value; }

        private double? _vertex4YCoordinate;
        public double? Vertex4YCoordinate { get => _vertex4YCoordinate; set => _vertex4YCoordinate = value; }

        private double? _vertex4ZCoordinate;
        public double? Vertex4ZCoordinate { get => _vertex4ZCoordinate; set => _vertex4ZCoordinate = value; }

        public FenestrationSurfaceDetailed() { }

        private static List<FenestrationSurfaceDetailed> list = new List<FenestrationSurfaceDetailed>();

        public static void Add(FenestrationSurfaceDetailed fenestrationSurfaceDetailed)
        {
            list.Add(fenestrationSurfaceDetailed);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"FenestrationSurface:Detailed,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SurfaceType}" + ",").PadRight(27, ' ') + " !-Surface Type\n" +
                    ($"  {list[i].ConstructionName}" + ",").PadRight(27, ' ') + " !-Construction Name\n" +                    
                    ($"  {list[i].BuildingSurfaceName}" + ",").PadRight(27, ' ') + " !-Building Surface Name\n" +
                    ($"  {list[i].OutsideBoundaryConditionObject}" + ",").PadRight(27, ' ') + " !-Outside Boundary Condition\n" +
                    ($"  {list[i].ViewFactortoGround}" + ",").PadRight(27, ' ') + " !-View Factor to Ground\n" +
                    ($"  {list[i].FrameandDividerName}" + ",").PadRight(27, ' ') + " !-Frame and Divider Name\n" +
                    ($"  {list[i].Multiplier}" + ",").PadRight(27, ' ') + " !-Multiplier\n" +
                    ($"  {list[i].NumberofVertices}" + ",").PadRight(27, ' ') + " !-Number of Vertices\n" +
                    ($"  {list[i].Vertex1XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-1X{{ m }}\n" +
                    ($"  {list[i].Vertex1YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-1Y{{ m }}\n" +
                    ($"  {list[i].Vertex1ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-1Z{{ m }}\n" +
                    ($"  {list[i].Vertex2XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-2X{{ m }}\n" +
                    ($"  {list[i].Vertex2YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-2Y{{ m }}\n" +
                    ($"  {list[i].Vertex2ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-2Z{{ m }}\n" +
                    ($"  {list[i].Vertex3XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-3X{{ m }}\n" +
                    ($"  {list[i].Vertex3YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-3Y{{ m }}\n" +
                    ($"  {list[i].Vertex3ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-3Z{{ m }}\n" +
                    ($"  {list[i].Vertex4XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-4X{{ m }}\n" +
                    ($"  {list[i].Vertex4YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-4Y{{ m }}\n" +
                    ($"  {list[i].Vertex4ZCoordinate:N2}" + ";").PadRight(27, ' ') + " !-4Z{{ m }}";

            }

            return print;
        }
        public static string DuplicateNameFix(FenestrationSurfaceDetailed fenestration)
        {
            var exist = list.Find(x => x.Name == fenestration.Name);
            if (exist != null)
            {
                return $"{fenestration.Name}-1";
            }
            else
            {
                return fenestration.Name;
            }
        }
        public static bool DuplicateCheck(FenestrationSurfaceDetailed fenestrationSurfaceDetailed)
        {
            bool Exist = false;
            var c1 = list.Find(x => (x.Vertex1XCoordinate == fenestrationSurfaceDetailed.Vertex1XCoordinate)
            && (x.Vertex1YCoordinate == fenestrationSurfaceDetailed.Vertex1YCoordinate)
            && (x.Vertex1ZCoordinate == fenestrationSurfaceDetailed.Vertex1ZCoordinate)
            && (x.Vertex2XCoordinate == fenestrationSurfaceDetailed.Vertex2XCoordinate)
            && (x.Vertex2YCoordinate == fenestrationSurfaceDetailed.Vertex2YCoordinate)
            && (x.Vertex2ZCoordinate == fenestrationSurfaceDetailed.Vertex2ZCoordinate)
            && (x.Vertex3XCoordinate == fenestrationSurfaceDetailed.Vertex3XCoordinate)
            && (x.Vertex3YCoordinate == fenestrationSurfaceDetailed.Vertex3YCoordinate)
            && (x.Vertex3ZCoordinate == fenestrationSurfaceDetailed.Vertex3ZCoordinate)
            && (x.Vertex4XCoordinate == fenestrationSurfaceDetailed.Vertex4XCoordinate)
            && (x.Vertex4YCoordinate == fenestrationSurfaceDetailed.Vertex4YCoordinate)
            && (x.Vertex4ZCoordinate == fenestrationSurfaceDetailed.Vertex4ZCoordinate));


            var c2 = list.Find(x => x.Name == fenestrationSurfaceDetailed.Name);
            if (c1 != null)
            {

                Exist = true;
                
            }
            if (c2 != null)
            {

                Exist = true;

            }
            return Exist;
        }
        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in FenestrationSurfaceDetailed.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Get()
        {
            
                foreach (string line in FenestrationSurfaceDetailed.Read())
                {
                    Console.WriteLine(line);
                }
            

        }
        public static void Collect(FenestrationSurfaceDetailed fenestration)
        {
            fenestration.Name = null;
            fenestration.SurfaceType = null;
            fenestration.ConstructionName = null;
            fenestration.OutsideBoundaryConditionObject = null;
            fenestration.BuildingSurfaceName = null;
            fenestration.ViewFactortoGround = null;
            fenestration.FrameandDividerName = null;
            fenestration.Multiplier = null;
            fenestration.NumberofVertices = null;
            fenestration.Vertex1XCoordinate = null;
            fenestration.Vertex1YCoordinate = null;
            fenestration.Vertex1ZCoordinate = null;
            fenestration.Vertex2XCoordinate = null;
            fenestration.Vertex2YCoordinate = null;
            fenestration.Vertex2ZCoordinate = null;
            fenestration.Vertex3XCoordinate = null;
            fenestration.Vertex3YCoordinate = null;
            fenestration.Vertex3ZCoordinate = null;
            fenestration.Vertex4XCoordinate = null;
            fenestration.Vertex4YCoordinate = null;
            fenestration.Vertex4ZCoordinate = null;
        }
    }
}
