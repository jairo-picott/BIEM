using System.IO;
using System.Collections.Generic;
using System;

namespace BIEM.ThermalZonesandSurfaces
{
    public enum SurfaceTypeEnum
    {
        Floor,
        Wall,
        Ceiling,
        Roof
    }

    public enum OutsideBoundaryConditionEnum
    {
        Outdoors,
        Adiabatic,
        Surface,
        Zone,
        Foundation,
        Ground,
        GroundFCFactorMethod,
        OtherSideCoefficients,
        OtherSideConditionsModel,
        GroundSlabPreprocessorAverage,
        GroundSlabProprocessorCore,
        GroundSlabPreprocessorPerimeter,
        GroundBasementPreprocessorAverageWall

    }

    public enum SunExposureEnum
    {
        SunExposed,
        NoSun
    }

    public enum WindExposureEnum
    {
        WindExposed,
        NoWind
    }
    //----------Building Surfaces Detailed
    //RELATE AND COLLECT THE IFCELEMENTASSEMBLY WITH THEIR ASSEMBLY TO GET
    //VERTICES FROM PRIVATE CLASSES
    public class BuildingSurfaceDetailed
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        private SurfaceTypeEnum? _surfaceType;
        public SurfaceTypeEnum? SurfaceType { get=>_surfaceType; set=>_surfaceType=value; }

        private string _constructionName;
        public string ConstructionName { get=>_constructionName; set=>_constructionName=value; }

        private string _zoneName;
        public string ZoneName { get => _zoneName; set => _zoneName = value; }


        private OutsideBoundaryConditionEnum? _outsideBoundaryCondition = OutsideBoundaryConditionEnum.Outdoors;
        public OutsideBoundaryConditionEnum? OutsideBoundaryCondition { get=>_outsideBoundaryCondition; set=>_outsideBoundaryCondition=value; }

        private string _outsideBoundaryConditionObject;
        public string OutsideBoundaryConditionObject { get =>_outsideBoundaryConditionObject; set=>_outsideBoundaryConditionObject=value; }

        private SunExposureEnum? _sunExposure;
        public SunExposureEnum? SunExposure { get=>_sunExposure; set=>_sunExposure = value; }

        private WindExposureEnum? _windExposure;
        public WindExposureEnum? WindExposure { get=>_windExposure; set=>_windExposure=value; }

        private string _viewFactortoGround = "autocalculate";
        public string ViewFactortoGround { get=>_viewFactortoGround; set=>_viewFactortoGround=value; }

        private string _numberofVertices = "autocalculate";
        public string NumberofVertices { get=>_numberofVertices; set=>_numberofVertices = value; }

        private double? _vertex1XCoordinate;
        public double? Vertex1XCoordinate { get=>_vertex1XCoordinate; set=>_vertex1XCoordinate = value; }

        private double? _vertex1YCoordinate;
        public double? Vertex1YCoordinate { get=>_vertex1YCoordinate; set=>_vertex1YCoordinate=value; }

        private double? _vertex1ZCoordinate;
        public double? Vertex1ZCoordinate { get=>_vertex1ZCoordinate; set=>_vertex1ZCoordinate=value; }

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

        private double? _vertex5XCoordinate;
        public double? Vertex5XCoordinate { get => _vertex5XCoordinate; set => _vertex5XCoordinate = value; }

        private double? _vertex5YCoordinate;
        public double? Vertex5YCoordinate { get => _vertex5YCoordinate; set => _vertex5YCoordinate = value; }

        private double? _vertex5ZCoordinate;
        public double? Vertex5ZCoordinate { get => _vertex5ZCoordinate; set => _vertex5ZCoordinate = value; }

        private double? _vertex6XCoordinate;
        public double? Vertex6XCoordinate { get => _vertex6XCoordinate; set => _vertex6XCoordinate = value; }

        private double? _vertex6YCoordinate;
        public double? Vertex6YCoordinate { get => _vertex6YCoordinate; set => _vertex6YCoordinate = value; }

        private double? _vertex6ZCoordinate;
        public double? Vertex6ZCoordinate { get => _vertex6ZCoordinate; set => _vertex6ZCoordinate = value; }

        private double? _vertex7XCoordinate;
        public double? Vertex7XCoordinate { get => _vertex7XCoordinate; set => _vertex7XCoordinate = value; }

        private double? _vertex7YCoordinate;
        public double? Vertex7YCoordinate { get => _vertex7YCoordinate; set => _vertex7YCoordinate = value; }

        private double? _vertex7ZCoordinate;
        public double? Vertex7ZCoordinate { get => _vertex7ZCoordinate; set => _vertex7ZCoordinate = value; }

        private double? _vertex8XCoordinate;
        public double? Vertex8XCoordinate { get => _vertex8XCoordinate; set => _vertex8XCoordinate = value; }

        private double? _vertex8YCoordinate;
        public double? Vertex8YCoordinate { get => _vertex8YCoordinate; set => _vertex8YCoordinate = value; }

        private double? _vertex8ZCoordinate;
        public double? Vertex8ZCoordinate { get => _vertex8ZCoordinate; set => _vertex8ZCoordinate = value; }

        private double? _vertex9XCoordinate;
        public double? Vertex9XCoordinate { get => _vertex9XCoordinate; set => _vertex9XCoordinate = value; }

        private double? _vertex9YCoordinate;
        public double? Vertex9YCoordinate { get => _vertex9YCoordinate; set => _vertex9YCoordinate = value; }

        private double? _vertex9ZCoordinate;
        public double? Vertex9ZCoordinate { get => _vertex9ZCoordinate; set => _vertex9ZCoordinate = value; }

        private double? _vertex10XCoordinate;
        public double? Vertex10XCoordinate { get => _vertex10XCoordinate; set => _vertex10XCoordinate = value; }

        private double? _vertex10YCoordinate;
        public double? Vertex10YCoordinate { get => _vertex10YCoordinate; set => _vertex10YCoordinate = value; }

        private double? _vertex10ZCoordinate;
        public double? Vertex10ZCoordinate { get => _vertex10ZCoordinate; set => _vertex10ZCoordinate = value; }
        public BuildingSurfaceDetailed() { }

        private static List<BuildingSurfaceDetailed> list = new List<BuildingSurfaceDetailed>();

        public static void Add(BuildingSurfaceDetailed buildingSurfaceDetailed)
        {
            list.Add(buildingSurfaceDetailed);
        }
        public static string FindSurface(string surfaceName)
        {
            var surface = list.Find(x => x.Name == surfaceName);
            if (surface != null)
            {

                return surfaceName;
            }
            else
            {
                string elementName = surfaceName.Substring(0, surfaceName.LastIndexOf("-"));

                var alternativeSurface = list.Find(x => x.Name.Substring(0, x.Name.LastIndexOf("-")) == elementName);

                if (alternativeSurface != null)
                {
                    return alternativeSurface.Name;
                }
                else
                {
                    return surfaceName;
                }


            }
        }

        public static string DuplicateNameFix(BuildingSurfaceDetailed buildingSurfaceDetailed)
        {
            var exist = list.Find(x => x.Name == buildingSurfaceDetailed.Name);
            if (exist != null)
            {
                return $"{buildingSurfaceDetailed.Name}-1";
            }
            else
            {
                return buildingSurfaceDetailed.Name;
            }
        }

        public static bool DuplicateCheck(BuildingSurfaceDetailed buildingSurfaceDetailed)
        {
            bool Exist = false;
            var c1 = list.Find(x => (x.Vertex1XCoordinate == buildingSurfaceDetailed.Vertex1XCoordinate)
            && (x.Vertex1YCoordinate == buildingSurfaceDetailed.Vertex1YCoordinate)
            && (x.Vertex1ZCoordinate == buildingSurfaceDetailed.Vertex1ZCoordinate)
            && (x.Vertex2XCoordinate == buildingSurfaceDetailed.Vertex2XCoordinate)
            && (x.Vertex2YCoordinate == buildingSurfaceDetailed.Vertex2YCoordinate)
            && (x.Vertex2ZCoordinate == buildingSurfaceDetailed.Vertex2ZCoordinate)
            && (x.Vertex3XCoordinate == buildingSurfaceDetailed.Vertex3XCoordinate)
            && (x.Vertex3YCoordinate == buildingSurfaceDetailed.Vertex3YCoordinate)
            && (x.Vertex3ZCoordinate == buildingSurfaceDetailed.Vertex3ZCoordinate)
            && (x.Vertex4XCoordinate == buildingSurfaceDetailed.Vertex4XCoordinate)
            && (x.Vertex4YCoordinate == buildingSurfaceDetailed.Vertex4YCoordinate)
            && (x.Vertex4ZCoordinate == buildingSurfaceDetailed.Vertex4ZCoordinate));



            if (c1 != null)
            {

                Exist = true;
                
            }
            return Exist;
        }

        public static void GetCenterLine()
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (i!=j)
                    {
                        BIEM.Plane plane1 = BIEM.Plane.EquationOfPlane3D(Convert.ToDouble(list[i].Vertex1XCoordinate), Convert.ToDouble(list[i].Vertex1YCoordinate), Convert.ToDouble(list[i].Vertex1ZCoordinate), Convert.ToDouble(list[i].Vertex2XCoordinate), Convert.ToDouble(list[i].Vertex2YCoordinate), Convert.ToDouble(list[i].Vertex2ZCoordinate),Convert.ToDouble(list[i].Vertex3XCoordinate),Convert.ToDouble(list[i].Vertex3YCoordinate), Convert.ToDouble(list[i].Vertex3ZCoordinate));
                        BIEM.Plane plane2 = BIEM.Plane.EquationOfPlane3D(Convert.ToDouble(list[j].Vertex1XCoordinate), Convert.ToDouble(list[j].Vertex1YCoordinate), Convert.ToDouble(list[j].Vertex1ZCoordinate), Convert.ToDouble(list[j].Vertex2XCoordinate), Convert.ToDouble(list[j].Vertex2YCoordinate), Convert.ToDouble(list[j].Vertex2ZCoordinate), Convert.ToDouble(list[j].Vertex3XCoordinate), Convert.ToDouble(list[j].Vertex3YCoordinate), Convert.ToDouble(list[j].Vertex3ZCoordinate));

                        if (BIEM.Plane.DistanceBetween2Planes(plane1, plane2) > 0 && BIEM.Plane.DistanceBetween2Planes(plane1, plane2) < 0.3)
                        {
                            Console.WriteLine($"{list[i].Name} in {list[i].ZoneName}\nAND\n{list[j].Name} in {list[j].ZoneName}\n--------------------------------");
                        }

                    }
                }
            }
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"BuildingSurface:Detailed,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].SurfaceType}" + ",").PadRight(27, ' ') + " !-Surface Type\n" +
                    ($"  {list[i].ConstructionName}" + ",").PadRight(27, ' ') + " !-Construction Name\n" +
                    ($"  {list[i].ZoneName}" + ",").PadRight(27, ' ') + " !-Zone Name\n" +
                    ($"  {list[i].OutsideBoundaryCondition}" + ",").PadRight(27, ' ') + " !-Outside Boundary Condition\n" +
                    ($"  {list[i].OutsideBoundaryConditionObject}" + ",").PadRight(27, ' ') + " !-Outside Boundary Condition Object\n" +
                    ($"  {list[i].SunExposure}" + ",").PadRight(27, ' ') + " !-Sun Exposure\n" +
                    ($"  {list[i].WindExposure}" + ",").PadRight(27, ' ') + " !-Wind Exposure\n" +
                    ($"  {list[i].ViewFactortoGround}" + ",").PadRight(27, ' ') + " !-View Factor to Ground\n" +
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
                    ($"  {list[i].Vertex4ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-4Z{{ m }}\n" +
                    ($"  {list[i].Vertex5XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-5X{{ m }}\n" +
                    ($"  {list[i].Vertex5YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-5Y{{ m }}\n" +
                    ($"  {list[i].Vertex5ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-5Z{{ m }}\n" +
                    ($"  {list[i].Vertex6XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-6X{{ m }}\n" +
                    ($"  {list[i].Vertex6YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-6Y{{ m }}\n" +
                    ($"  {list[i].Vertex6ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-6Z{{ m }}\n" +
                    ($"  {list[i].Vertex7XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-7X{{ m }}\n" +
                    ($"  {list[i].Vertex7YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-7Y{{ m }}\n" +
                    ($"  {list[i].Vertex7ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-7Z{{ m }}\n" +
                    ($"  {list[i].Vertex8XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-8X{{ m }}\n" +
                    ($"  {list[i].Vertex8YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-8Y{{ m }}\n" +
                    ($"  {list[i].Vertex8ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-8Z{{ m }}\n" +
                    ($"  {list[i].Vertex9XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-9X{{ m }}\n" +
                    ($"  {list[i].Vertex9YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-9Y{{ m }}\n" +
                    ($"  {list[i].Vertex9ZCoordinate:N2}" + ",").PadRight(27, ' ') + " !-9Z{{ m }}\n" +
                    ($"  {list[i].Vertex10XCoordinate:N2}" + ",").PadRight(27, ' ') + " !-10X{{ m }}\n" +
                    ($"  {list[i].Vertex10YCoordinate:N2}" + ",").PadRight(27, ' ') + " !-10Y{{ m }}\n" +
                    ($"  {list[i].Vertex10ZCoordinate:N2}" + ";").PadRight(27, ' ') + " !-10Z{{ m }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in BuildingSurfaceDetailed.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {
            foreach (string line in BuildingSurfaceDetailed.Read())
            {
                Console.WriteLine(line);
            }
        }

        /*
        public static string structuralElementinAssembly(IIfcElementAssembly assembly)
        {
            var assemblyName = assembly.GlobalId;
            var assemblyElement = assembly.IsDecomposedBy.ToList();
            foreach (var element in assemblyElement)
            {
                // Obtain elements in Assembly
                var objects = element.RelatedObjects.ToList();
                //This for the case assembly contains more than one element
                if (assemblyElement.Count > 1)
                {
                    foreach (var obj in objects)
                    {
                        //Get the element of the assembly that is the structure of it
                        var objType = obj.GetType().Name.ToString();
                        if (objType == "IfcWall")
                        {
                            var wall = getWallbyID(obj.GlobalId.ToString());
                            var hasStructuralMaterial = SurfaceConstructionElements.GetElementProperty(wall, "wallStructuralMaterial");
                            if (hasStructuralMaterial != null)
                            {

                                return wall.GlobalId;

                            }
                        }
                        else if (objType == "IfcSlab")
                        {
                            var slab = getSlabbyID(obj.GlobalId.ToString());
                            var hasStructuralMaterial = SurfaceConstructionElements.GetElementProperty(slab, "floorStructuralMaterial");
                            if (hasStructuralMaterial != null)
                            {
                                return slab.GlobalId;
                            }
                        }
                        else if (objType == "IfcCovering")
                        {
                            var ceiling = getCeilingbyID(obj.GlobalId.ToString());
                            var hasStructuralMaterial = SurfaceConstructionElements.GetElementProperty(ceiling, "ceilingStructuralMaterial");
                            if (hasStructuralMaterial != null)
                            {
                                return ceiling.GlobalId;
                            }
                        }
                        else if (objType == "IfcRoof")
                        {
                            var roof = getRoofbyID(obj.GlobalId.ToString());
                            var hasStructuralMaterial = SurfaceConstructionElements.GetElementProperty(roof, "roofStructuralMaterial");
                            if (hasStructuralMaterial != null)
                            {
                                return roof.GlobalId;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var obj in objects)
                    {
                        //Get the element of the assembly that is the structure of it
                        var objType = obj.GetType().Name.ToString();
                        if (objType == "IfcWall")
                        {
                            var wall = getWallbyID(obj.GlobalId.ToString());
                            if (wall != null)
                            {
                                return wall.GlobalId;
                            }
                        }
                        else if (objType == "IfcSlab")
                        {
                            var slab = getSlabbyID(obj.GlobalId.ToString());
                            if (slab != null)
                            {
                                return slab.GlobalId;
                            }
                        }
                        else if (objType == "IfcCovering")
                        {
                            var ceiling = getCeilingbyID(obj.GlobalId.ToString());
                            if (ceiling != null)
                            {
                                return ceiling.GlobalId;
                            }
                        }
                        else if (objType == "IfcRoof")
                        {
                            var roof = getRoofbyID(obj.GlobalId.ToString());
                            if (roof != null)
                            {
                                return roof.GlobalId;
                            }
                        }
                    }
                }
            }
            return null;

        }
        */
        public static void Collect(BuildingSurfaceDetailed buildingSurface)
        {
            buildingSurface.Name = null;
            buildingSurface.SurfaceType = null;
            buildingSurface.ConstructionName = null;
            buildingSurface.ZoneName = null;
            buildingSurface.OutsideBoundaryCondition = null;
            buildingSurface.OutsideBoundaryConditionObject = null;
            buildingSurface.SunExposure = null;
            buildingSurface.WindExposure = null;
            buildingSurface.ViewFactortoGround = null;
            buildingSurface.NumberofVertices = null;
            buildingSurface.Vertex1XCoordinate = null;
            buildingSurface.Vertex1YCoordinate = null;
            buildingSurface.Vertex1ZCoordinate = null;
            buildingSurface.Vertex2XCoordinate = null;
            buildingSurface.Vertex2YCoordinate = null;
            buildingSurface.Vertex2ZCoordinate = null;
            buildingSurface.Vertex3XCoordinate = null;
            buildingSurface.Vertex3YCoordinate = null;
            buildingSurface.Vertex3ZCoordinate = null;
            buildingSurface.Vertex4XCoordinate = null;
            buildingSurface.Vertex4YCoordinate = null;
            buildingSurface.Vertex4ZCoordinate = null;
            buildingSurface.Vertex5XCoordinate = null;
            buildingSurface.Vertex5YCoordinate = null;
            buildingSurface.Vertex5ZCoordinate = null;
            buildingSurface.Vertex6XCoordinate = null;
            buildingSurface.Vertex6YCoordinate = null;
            buildingSurface.Vertex6ZCoordinate = null;
            buildingSurface.Vertex7XCoordinate = null;
            buildingSurface.Vertex7YCoordinate = null;
            buildingSurface.Vertex7ZCoordinate = null;
            buildingSurface.Vertex8XCoordinate = null;
            buildingSurface.Vertex8YCoordinate = null;
            buildingSurface.Vertex8ZCoordinate = null;
            buildingSurface.Vertex9XCoordinate = null;
            buildingSurface.Vertex9YCoordinate = null;
            buildingSurface.Vertex9ZCoordinate = null;
            buildingSurface.Vertex10XCoordinate = null;
            buildingSurface.Vertex10YCoordinate = null;
            buildingSurface.Vertex10ZCoordinate = null;
        }
    }
}
