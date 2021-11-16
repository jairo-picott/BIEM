using System.Linq;
using System.Text;
using Xbim.Ifc4.Interfaces;

namespace BIEM.Tools
{
    public class Tool
    {
        public static string GetMaterialProperty(IIfcMaterial material, string name)
        {
            var materialProperties = material.HasProperties.ToList();
            foreach (var property in materialProperties)
            {
                var propertySingleValue = property.Properties
                    .OfType<IIfcPropertySingleValue>().ToList();

                foreach (var layer in propertySingleValue)
                {
                    //property.GetType();
                    if (layer.Name == name)
                    {
                        return layer.NominalValue.ToString();

                    }
                }
            }
                
            

            return null;

        }
   
        public static string GetSpaceProperty(IIfcSpace space, string name)
        {
            var spaceProperty = space.IsDefinedBy.ToList()
                .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                .OfType<IIfcPropertySingleValue>();
            foreach (var property in spaceProperty)
            {
                //property.GetType();
                if (property.Name == name)
                {
                    return property.NominalValue.ToString();

                }
            }

            return null;

        }
        public static IIfcValue GetElementProperty(IIfcBuildingElement element, string name) //REGRESA EL VALOR NOMINAL DE LA PROPIEDAD SOLICITADA
        {
            var spaceProperty = element.IsDefinedBy.ToList()
                .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                .OfType<IIfcPropertySingleValue>();
            foreach (var property in spaceProperty)
            {
                //property.GetType();
                if (property.Name == name)
                {
                    return property.NominalValue;
                }
            }

            return null;

        }
        public static IIfcValue GetWallTypeProperty(IIfcWallType element, string name)
        {
            var pSets = element.HasPropertySets.OfType<IIfcPropertySet>().ToList();
            foreach (var pSet in pSets)
            {
                var _pSet = pSet as IIfcPropertySet;
                var properties = _pSet.HasProperties.ToList().OfType<IIfcPropertySingleValue>().ToList();
                foreach (var property in properties)
                {
                    if (property.Name == name)
                    {
                        return property.NominalValue;
                    }
                }
            }

            return null;

        }
        public static IIfcValue GetCeilingTypeProperty(IIfcCoveringType element, string name)
        {
            var pSets = element.HasPropertySets.OfType<IIfcPropertySet>().ToList();
            foreach (var pSet in pSets)
            {
                var _pSet = pSet as IIfcPropertySet;
                var properties = _pSet.HasProperties.ToList().OfType<IIfcPropertySingleValue>().ToList();
                foreach (var property in properties)
                {
                    if (property.Name == name)
                    {
                        return property.NominalValue;
                    }
                }
            }

            return null;

        }
        public static IIfcValue GetDoorTypeProperty(IIfcDoorType element, string name)
        {
            var pSets = element.HasPropertySets.OfType<IIfcPropertySet>().ToList();
            foreach (var pSet in pSets)
            {
                var _pSet = pSet as IIfcPropertySet;
                var properties = _pSet.HasProperties.OfType<IIfcPropertySingleValue>().ToList();
                foreach (var property in properties)
                {
                    if (property.Name == name)
                    {
                        return property.NominalValue;
                    }
                }
            }

            return null;

        }
        public static IIfcValue GetWindowTypeProperty(IIfcWindowType element, string name)
        {
            var pSets = element.HasPropertySets.OfType<IIfcPropertySet>().ToList();
            foreach (var pSet in pSets)
            {
                var _pSet = pSet as IIfcPropertySet;
                var properties = _pSet.HasProperties.ToList().OfType<IIfcPropertySingleValue>().ToList();
                foreach (var property in properties)
                {
                    if (property.Name == name)
                    {
                        return property.NominalValue;
                    }
                }
            }

            return null;

        }
        public static IIfcValue GetFloorTypeProperty(IIfcSlabType element, string name)
        {
            var pSets = element.HasPropertySets.OfType<IIfcPropertySet>().ToList();
            foreach (var pSet in pSets)
            {
                var _pSet = pSet as IIfcPropertySet;
                var properties = _pSet.HasProperties.ToList().OfType<IIfcPropertySingleValue>().ToList();
                foreach (var property in properties)
                {
                    if (property.Name == name)
                    {
                        return property.NominalValue;
                    }
                }
            }

            return null;

        }
        public static bool IsItPlenum(IIfcZone zone)
        {
            var group = zone.IsGroupedBy.ToList();
            foreach (var obj in group)
            {
                var objs = obj.RelatedObjects.ToList();
                foreach (var ob in objs)
                {
                    var _ob = ob as IIfcSpace;

                    var spaceProperty = _ob.IsDefinedBy.ToList()
                        .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                        .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                        .OfType<IIfcPropertySingleValue>();
                    foreach (var property in spaceProperty)
                    {
                        //property.GetType();
                        if (property.Name == "Plenum")
                        {
                            var value = property.NominalValue.ToString();
                            bool itisPlenum = false;
                            if (value == "true")
                            {
                                itisPlenum = true;
                            }
                            if (itisPlenum)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        return false;

                    }
                    return false;

                }
                return false;

            }

            return false;


        }
        public static bool isExternal(IIfcBuildingElement element)
        {

            var spaceProperty = element.IsDefinedBy.ToList()
                   .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                   .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                   .OfType<IIfcPropertySingleValue>();
            foreach (var property in spaceProperty)
            {

                //property.GetType();
                if (property.Name.Value.ToString() == "IsExternal")
                {
                    var ans = property.NominalValue.ToString();
                    if (ans != "false")
                    {


                        return true;


                    }
                    else
                    {
                        return false;

                    }

                }

            }
            return false;

        }
        public static double[] GetVerticeSpace(IIfcSpace element)
        {
            var objectPlacement = element.ObjectPlacement;
            var _objectPlacement = objectPlacement as IIfcLocalPlacement;
            IIfcAxis2Placement3D _relativePlacement;
            IIfcCartesianPoint location;
            double[] v1 = new double[3];

            if (_objectPlacement.PlacementRelTo != null)
            {
                var relativePlacement = _objectPlacement.RelativePlacement;
                _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                location = _relativePlacement.Location;
                v1[0] = v1[0] + location.X;
                v1[1] = v1[1] + location.Y;
                v1[2] = v1[2] + location.Z;

                objectPlacement = _objectPlacement.PlacementRelTo;
                _objectPlacement = objectPlacement as IIfcLocalPlacement;


                if (_objectPlacement.PlacementRelTo != null)
                {
                    relativePlacement = _objectPlacement.RelativePlacement;
                    _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                    location = _relativePlacement.Location;
                    v1[0] = v1[0] + location.X;
                    v1[1] = v1[1] + location.Y;
                    v1[2] = v1[2] + location.Z;

                    objectPlacement = _objectPlacement.PlacementRelTo;
                    _objectPlacement = objectPlacement as IIfcLocalPlacement;

                    if (_objectPlacement.PlacementRelTo != null)
                    {
                        relativePlacement = _objectPlacement.RelativePlacement;
                        _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                        location = _relativePlacement.Location;
                        v1[0] = v1[0] + location.X;
                        v1[1] = v1[1] + location.Y;
                        v1[2] = v1[2] + location.Z;

                        objectPlacement = _objectPlacement.PlacementRelTo;
                        _objectPlacement = objectPlacement as IIfcLocalPlacement;

                        if (_objectPlacement.PlacementRelTo != null)
                        {
                            relativePlacement = _objectPlacement.RelativePlacement;
                            _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                            location = _relativePlacement.Location;
                            v1[0] = v1[0] + location.X;
                            v1[1] = v1[1] + location.Y;
                            v1[2] = v1[2] + location.Z;

                            objectPlacement = _objectPlacement.PlacementRelTo;
                            _objectPlacement = objectPlacement as IIfcLocalPlacement;

                            if (_objectPlacement.PlacementRelTo != null)
                            {
                                relativePlacement = _objectPlacement.RelativePlacement;
                                _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                                location = _relativePlacement.Location;
                                v1[0] = v1[0] + location.X;
                                v1[1] = v1[1] + location.Y;
                                v1[2] = v1[2] + location.Z;

                                objectPlacement = _objectPlacement.PlacementRelTo;
                                _objectPlacement = objectPlacement as IIfcLocalPlacement;

                                if (_objectPlacement.PlacementRelTo != null)
                                {
                                    relativePlacement = _objectPlacement.RelativePlacement;
                                    _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                                    location = _relativePlacement.Location;
                                    v1[0] = v1[0] + location.X;
                                    v1[1] = v1[1] + location.Y;
                                    v1[2] = v1[2] + location.Z;

                                    objectPlacement = _objectPlacement.PlacementRelTo;
                                    _objectPlacement = objectPlacement as IIfcLocalPlacement;

                                    if (_objectPlacement.PlacementRelTo != null)
                                    {
                                        relativePlacement = _objectPlacement.RelativePlacement;
                                        _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                                        location = _relativePlacement.Location;
                                        v1[0] = v1[0] + location.X;
                                        v1[1] = v1[1] + location.Y;
                                        v1[2] = v1[2] + location.Z;

                                        objectPlacement = _objectPlacement.PlacementRelTo;
                                        _objectPlacement = objectPlacement as IIfcLocalPlacement;

                                        return v1;
                                    }
                                    else
                                    {
                                        relativePlacement = _objectPlacement.RelativePlacement;
                                        _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                                        location = _relativePlacement.Location;
                                        v1[0] = v1[0] + location.X;
                                        v1[1] = v1[1] + location.Y;
                                        v1[2] = v1[2] + location.Z;
                                        return v1;
                                    }
                                }
                                else
                                {
                                    relativePlacement = _objectPlacement.RelativePlacement;
                                    _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                                    location = _relativePlacement.Location;
                                    v1[0] = v1[0] + location.X;
                                    v1[1] = v1[1] + location.Y;
                                    v1[2] = v1[2] + location.Z;
                                    return v1;
                                }
                            }
                            else
                            {
                                relativePlacement = _objectPlacement.RelativePlacement;
                                _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                                location = _relativePlacement.Location;
                                v1[0] = v1[0] + location.X;
                                v1[1] = v1[1] + location.Y;
                                v1[2] = v1[2] + location.Z;
                                return v1;
                            }
                        }
                        else
                        {
                            relativePlacement = _objectPlacement.RelativePlacement;
                            _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                            location = _relativePlacement.Location;
                            v1[0] = v1[0] + location.X;
                            v1[1] = v1[1] + location.Y;
                            v1[2] = v1[2] + location.Z;
                            return v1;
                        }
                    }
                    else
                    {
                        relativePlacement = _objectPlacement.RelativePlacement;
                        _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                        location = _relativePlacement.Location;
                        v1[0] = v1[0] + location.X;
                        v1[1] = v1[1] + location.Y;
                        v1[2] = v1[2] + location.Z;
                        return v1;
                    }

                }
                else
                {
                    relativePlacement = _objectPlacement.RelativePlacement;
                    _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                    location = _relativePlacement.Location;
                    v1[0] = v1[0] + location.X;
                    v1[1] = v1[1] + location.Y;
                    v1[2] = v1[2] + location.Z;
                    return v1;
                }


            }
            else
            {
                var relativePlacement = _objectPlacement.RelativePlacement;
                _relativePlacement = relativePlacement as IIfcAxis2Placement3D;
                location = _relativePlacement.Location;
                v1[0] = v1[0] + location.X;
                v1[1] = v1[1] + location.Y;
                v1[2] = v1[2] + location.Z;
                return v1;
            }
        }
        public static double[] GetVerticeBuildingElement(IIfcBuildingElement element)
        {
            var objectPlacement = element.ObjectPlacement;
            var _objectPlacement = objectPlacement as IIfcLocalPlacement;
            double[] v1 = new double[3];
            Xbim.Common.Geometry.XbimVector3D p0;
            Xbim.Common.Geometry.XbimVector3D p1;
            Xbim.Common.Geometry.XbimVector3D p2;
            if (_objectPlacement.PlacementRelTo != null)
            {
               

                var objectPlacement1 = _objectPlacement.PlacementRelTo;
                var _objectPlacement1 = objectPlacement1 as IIfcLocalPlacement;


                if (_objectPlacement1.PlacementRelTo != null)
                {
                   
                    var objectPlacement2 = _objectPlacement1.PlacementRelTo;
                    var _objectPlacement2 = objectPlacement2 as IIfcLocalPlacement;

                    if (_objectPlacement2.PlacementRelTo != null)
                    {
                       
                        var objectPlacement3 = _objectPlacement2.PlacementRelTo;
                        var _objectPlacement3 = objectPlacement3 as IIfcLocalPlacement;

                        if (_objectPlacement3.PlacementRelTo != null)
                        {
                            
                            var objectPlacement4 = _objectPlacement3.PlacementRelTo;
                            var _objectPlacement4 = objectPlacement4 as IIfcLocalPlacement;

                            if (_objectPlacement4.PlacementRelTo != null)
                            {
                                
                                var objectPlacement5 = _objectPlacement4.PlacementRelTo;
                                var _objectPlacement5 = objectPlacement5 as IIfcLocalPlacement;

                                if (_objectPlacement5.PlacementRelTo != null)
                                {
                                   

                                    var objectPlacement6 = _objectPlacement5.PlacementRelTo;
                                    var _objectPlacement6 = objectPlacement6 as IIfcLocalPlacement;

                                    if (_objectPlacement6.PlacementRelTo != null)
                                    {
                                        
                                        var objectPlacement7 = _objectPlacement6.PlacementRelTo;
                                        var _objectPlacement7 = objectPlacement7 as IIfcLocalPlacement;
                                        if (_objectPlacement7.PlacementRelTo != null)
                                        {

                                            var objectPlacement8 = _objectPlacement7.PlacementRelTo;
                                            var _objectPlacement8 = objectPlacement8 as IIfcLocalPlacement;

                                            if (_objectPlacement8.PlacementRelTo != null)
                                            {

                                                var objectPlacement9 = _objectPlacement8.PlacementRelTo;
                                                var _objectPlacement9 = objectPlacement9 as IIfcLocalPlacement;

                                                if (_objectPlacement9.PlacementRelTo != null)
                                                {

                                                    var relativePlacement9 = _objectPlacement9.RelativePlacement as IIfcAxis2Placement3D;

                                                    p0 = relativePlacement9.P[0];
                                                    p1 = relativePlacement9.P[1];
                                                    p2 = relativePlacement9.P[2];

                                                 

                                                    var moveAxisX10 = relativePlacement9.Location.X * p0;
                                                    var moveAxisY10 = relativePlacement9.Location.Y * p1;
                                                    var moveAxisZ10 = relativePlacement9.Location.Z * p2;

                                                    var vectorMove9 = moveAxisX10 + moveAxisY10 + moveAxisZ10;

                                                    Xbim.Common.Geometry.XbimVector3D axisX9 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY9 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ9 = p2;

                                                    
                                                    var relativePlacement8 = _objectPlacement8.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX9 = relativePlacement8.Location.X * p0;
                                                    var moveAxisY9 = relativePlacement8.Location.Y * p1;
                                                    var moveAxisZ9 = relativePlacement8.Location.Z * p2;

                                                    var vectorMove8 = moveAxisX9 + moveAxisY9 + moveAxisZ9;

                                                    #region AxisTransformation
                                                    if (relativePlacement8.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement8.P[0].X * axisX9;
                                                    }
                                                    else if (relativePlacement8.P[0].Y != 0 )
                                                    {
                                                        p0 = relativePlacement8.P[0].Y * axisY9;
                                                    }
                                                    else if (relativePlacement8.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement8.P[0].Z * axisZ9;
                                                    }
                                                    if (relativePlacement8.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement8.P[1].X * axisX9;
                                                    }
                                                    else if (relativePlacement8.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement8.P[1].Y * axisY9;
                                                    }
                                                    else if (relativePlacement8.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement8.P[1].Z * axisZ9;
                                                    }
                                                    if (relativePlacement8.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement8.P[2].X * axisX9;
                                                    }
                                                    else if (relativePlacement8.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement8.P[2].Y * axisY9;
                                                    }
                                                    else if (relativePlacement8.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement8.P[2].Z * axisZ9;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX8 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY8 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ8 = p2;

                                                    var relativePlacement7 = _objectPlacement7.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX8 = relativePlacement7.Location.X * p0;
                                                    var moveAxisY8 = relativePlacement7.Location.Y * p1;
                                                    var moveAxisZ8 = relativePlacement7.Location.Z * p2;

                                                    var vectorMove7 = moveAxisX8 + moveAxisY8 + moveAxisZ8;

                                                    #region AxisTransformation
                                                    if (relativePlacement7.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement7.P[0].X * axisX8;
                                                    }
                                                    else if (relativePlacement7.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement7.P[0].Y * axisY8;
                                                    }
                                                    else if (relativePlacement7.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement7.P[0].Z * axisZ8;
                                                    }
                                                    if (relativePlacement7.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement7.P[1].X * axisX8;
                                                    }
                                                    else if (relativePlacement7.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement7.P[1].Y * axisY8;
                                                    }
                                                    else if (relativePlacement7.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement7.P[1].Z * axisZ8;
                                                    }
                                                    if (relativePlacement7.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement7.P[2].X * axisX8;
                                                    }
                                                    else if (relativePlacement7.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement7.P[2].Y * axisY8;
                                                    }
                                                    else if (relativePlacement7.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement7.P[2].Z * axisZ8;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX7 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY7 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ7 = p2;

                                                    var relativePlacement6 = _objectPlacement6.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX7 = relativePlacement6.Location.X * p0;
                                                    var moveAxisY7 = relativePlacement6.Location.Y * p1;
                                                    var moveAxisZ7 = relativePlacement6.Location.Z * p2;

                                                    var vectorMove6 = moveAxisX7 + moveAxisY7 + moveAxisZ7;

                                                    #region AxisTransformation
                                                    if (relativePlacement6.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement6.P[0].X * axisX7;
                                                    }
                                                    else if (relativePlacement6.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement6.P[0].Y * axisY7;
                                                    }
                                                    else if (relativePlacement6.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement6.P[0].Z * axisZ7;
                                                    }
                                                    if (relativePlacement6.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement6.P[1].X * axisX7;
                                                    }
                                                    else if (relativePlacement6.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement6.P[1].Y * axisY7;
                                                    }
                                                    else if (relativePlacement6.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement6.P[1].Z * axisZ7;
                                                    }
                                                    if (relativePlacement6.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement6.P[2].X * axisX7;
                                                    }
                                                    else if (relativePlacement6.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement6.P[2].Y * axisY7;
                                                    }
                                                    else if (relativePlacement6.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement6.P[2].Z * axisZ7;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX6 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY6 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ6 = p2;

                                                    var relativePlacement5 = _objectPlacement5.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX6 = relativePlacement5.Location.X * p0;
                                                    var moveAxisY6 = relativePlacement5.Location.Y * p1;
                                                    var moveAxisZ6 = relativePlacement5.Location.Z * p2;

                                                    var vectorMove5 = moveAxisX6 + moveAxisY6 + moveAxisZ6;

                                                    #region AxisTransformation
                                                    if (relativePlacement5.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement5.P[0].X * axisX6;
                                                    }
                                                    else if (relativePlacement5.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement5.P[0].Y * axisY6;
                                                    }
                                                    else if (relativePlacement5.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement5.P[0].Z * axisZ6;
                                                    }
                                                    if (relativePlacement5.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement5.P[1].X * axisX6;
                                                    }
                                                    else if (relativePlacement5.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement5.P[1].Y * axisY6;
                                                    }
                                                    else if (relativePlacement5.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement5.P[1].Z * axisZ6;
                                                    }
                                                    if (relativePlacement5.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement5.P[2].X * axisX6;
                                                    }
                                                    else if (relativePlacement5.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement5.P[2].Y * axisY6;
                                                    }
                                                    else if (relativePlacement5.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement5.P[2].Z * axisZ6;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX5 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY5 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ5 = p2;

                                                    var relativePlacement4 = _objectPlacement4.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX5 = relativePlacement4.Location.X * p0;
                                                    var moveAxisY5 = relativePlacement4.Location.Y * p1;
                                                    var moveAxisZ5 = relativePlacement4.Location.Z * p2;

                                                    var vectorMove4 = moveAxisX5 + moveAxisY5 + moveAxisZ5;

                                                    #region AxisTransformation
                                                    if (relativePlacement4.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement4.P[0].X * axisX5;
                                                    }
                                                    else if (relativePlacement4.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement4.P[0].Y * axisY5;
                                                    }
                                                    else if (relativePlacement4.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement4.P[0].Z * axisZ5;
                                                    }
                                                    if (relativePlacement4.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement4.P[1].X * axisX5;
                                                    }
                                                    else if (relativePlacement4.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement4.P[1].Y * axisY5;
                                                    }
                                                    else if (relativePlacement4.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement4.P[1].Z * axisZ5;
                                                    }
                                                    if (relativePlacement4.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement4.P[2].X * axisX5;
                                                    }
                                                    else if (relativePlacement4.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement4.P[2].Y * axisY5;
                                                    }
                                                    else if (relativePlacement4.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement4.P[2].Z * axisZ5;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX4 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY4 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ4 = p2;

                                                    var relativePlacement3 = _objectPlacement3.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX4 = relativePlacement3.Location.X * p0;
                                                    var moveAxisY4 = relativePlacement3.Location.Y * p1;
                                                    var moveAxisZ4 = relativePlacement3.Location.Z * p2;

                                                    var vectorMove3 = moveAxisX4 + moveAxisY4 + moveAxisZ4;

                                                    #region AxisTransformation
                                                    if (relativePlacement3.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement3.P[0].X * axisX4;
                                                    }
                                                    else if (relativePlacement3.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement3.P[0].Y * axisY4;
                                                    }
                                                    else if (relativePlacement3.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement3.P[0].Z * axisZ4;
                                                    }
                                                    if (relativePlacement3.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement3.P[1].X * axisX4;
                                                    }
                                                    else if (relativePlacement3.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement3.P[1].Y * axisY4;
                                                    }
                                                    else if (relativePlacement3.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement3.P[1].Z * axisZ4;
                                                    }
                                                    if (relativePlacement3.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement3.P[2].X * axisX4;
                                                    }
                                                    else if (relativePlacement3.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement3.P[2].Y * axisY4;
                                                    }
                                                    else if (relativePlacement3.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement3.P[2].Z * axisZ4;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX3 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY3 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ3 = p2;

                                                    var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX3 = relativePlacement2.Location.X * p0;
                                                    var moveAxisY3 = relativePlacement2.Location.Y * p1;
                                                    var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                                                    var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                                                    #region AxisTransformation
                                                    if (relativePlacement2.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement2.P[0].X * axisX3;
                                                    }
                                                    else if (relativePlacement2.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement2.P[0].Y * axisY3;
                                                    }
                                                    else if (relativePlacement2.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement2.P[0].Z * axisZ3;
                                                    }
                                                    if (relativePlacement2.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement2.P[1].X * axisX3;
                                                    }
                                                    else if (relativePlacement2.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement2.P[1].Y * axisY3;
                                                    }
                                                    else if (relativePlacement2.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement2.P[1].Z * axisZ3;
                                                    }
                                                    if (relativePlacement2.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement2.P[2].X * axisX3;
                                                    }
                                                    else if (relativePlacement2.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement2.P[2].Y * axisY3;
                                                    }
                                                    else if (relativePlacement2.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement2.P[2].Z * axisZ3;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                                                    var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX2 = relativePlacement1.Location.X * p0;
                                                    var moveAxisY2 = relativePlacement1.Location.Y * p1;
                                                    var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                                                    var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                                                    #region AxisTransformation
                                                    if (relativePlacement1.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement1.P[0].X * axisX2;
                                                    }
                                                    else if (relativePlacement1.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement1.P[0].Y * axisY2;
                                                    }
                                                    else if (relativePlacement1.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement1.P[0].Z * axisZ2;
                                                    }
                                                    if (relativePlacement1.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement1.P[1].X * axisX2;
                                                    }
                                                    else if (relativePlacement1.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement1.P[1].Y * axisY2;
                                                    }
                                                    else if (relativePlacement1.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement1.P[1].Z * axisZ2;
                                                    }
                                                    if (relativePlacement1.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement1.P[2].X * axisX2;
                                                    }
                                                    else if (relativePlacement1.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement1.P[2].Y * axisY2;
                                                    }
                                                    else if (relativePlacement1.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement1.P[2].Z * axisZ2;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                                                    var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX1 = relativePlacement.Location.X * p0;
                                                    var moveAxisY1 = relativePlacement.Location.Y * p1;
                                                    var moveAxisZ1 = relativePlacement.Location.Z * p2;

                                                    var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                                                    var finalVector = vectorMove + vectorMove1 + vectorMove2 + vectorMove3 + vectorMove4 + vectorMove5 + vectorMove6 + vectorMove7 + vectorMove8 + vectorMove9;

                                                    v1[0] = v1[0] + finalVector.X;
                                                    v1[1] = v1[1] + finalVector.Y;
                                                    v1[2] = v1[2] + finalVector.Z;

                                                    return v1;


                                                }
                                                else
                                                {
                                                    var relativePlacement8 = _objectPlacement8.RelativePlacement as IIfcAxis2Placement3D;

                                                    p0 = relativePlacement8.P[0];
                                                    p1 = relativePlacement8.P[1];
                                                    p2 = relativePlacement8.P[2];



                                                    var moveAxisX9 = relativePlacement8.Location.X * p0;
                                                    var moveAxisY9 = relativePlacement8.Location.Y * p1;
                                                    var moveAxisZ9 = relativePlacement8.Location.Z * p2;

                                                    var vectorMove8 = moveAxisX9 + moveAxisY9 + moveAxisZ9;

                                                    Xbim.Common.Geometry.XbimVector3D axisX8 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY8 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ8 = p2;


                                                    var relativePlacement7 = _objectPlacement7.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX8 = relativePlacement7.Location.X * p0;
                                                    var moveAxisY8 = relativePlacement7.Location.Y * p1;
                                                    var moveAxisZ8 = relativePlacement7.Location.Z * p2;

                                                    var vectorMove7 = moveAxisX8 + moveAxisY8 + moveAxisZ8;

                                                    #region AxisTransformation
                                                    if (relativePlacement7.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement7.P[0].X * axisX8;
                                                    }
                                                    else if (relativePlacement7.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement7.P[0].Y * axisY8;
                                                    }
                                                    else if (relativePlacement7.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement7.P[0].Z * axisZ8;
                                                    }
                                                    if (relativePlacement7.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement7.P[1].X * axisX8;
                                                    }
                                                    else if (relativePlacement7.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement7.P[1].Y * axisY8;
                                                    }
                                                    else if (relativePlacement7.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement7.P[1].Z * axisZ8;
                                                    }
                                                    if (relativePlacement7.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement7.P[2].X * axisX8;
                                                    }
                                                    else if (relativePlacement7.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement7.P[2].Y * axisY8;
                                                    }
                                                    else if (relativePlacement7.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement7.P[2].Z * axisZ8;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX7 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY7 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ7 = p2;

                                                    var relativePlacement6 = _objectPlacement6.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX7 = relativePlacement6.Location.X * p0;
                                                    var moveAxisY7 = relativePlacement6.Location.Y * p1;
                                                    var moveAxisZ7 = relativePlacement6.Location.Z * p2;

                                                    var vectorMove6 = moveAxisX7 + moveAxisY7 + moveAxisZ7;

                                                    #region AxisTransformation
                                                    if (relativePlacement6.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement6.P[0].X * axisX7;
                                                    }
                                                    else if (relativePlacement6.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement6.P[0].Y * axisY7;
                                                    }
                                                    else if (relativePlacement6.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement6.P[0].Z * axisZ7;
                                                    }
                                                    if (relativePlacement6.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement6.P[1].X * axisX7;
                                                    }
                                                    else if (relativePlacement6.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement6.P[1].Y * axisY7;
                                                    }
                                                    else if (relativePlacement6.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement6.P[1].Z * axisZ7;
                                                    }
                                                    if (relativePlacement6.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement6.P[2].X * axisX7;
                                                    }
                                                    else if (relativePlacement6.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement6.P[2].Y * axisY7;
                                                    }
                                                    else if (relativePlacement6.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement6.P[2].Z * axisZ7;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX6 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY6 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ6 = p2;

                                                    var relativePlacement5 = _objectPlacement5.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX6 = relativePlacement5.Location.X * p0;
                                                    var moveAxisY6 = relativePlacement5.Location.Y * p1;
                                                    var moveAxisZ6 = relativePlacement5.Location.Z * p2;

                                                    var vectorMove5 = moveAxisX6 + moveAxisY6 + moveAxisZ6;

                                                    #region AxisTransformation
                                                    if (relativePlacement5.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement5.P[0].X * axisX6;
                                                    }
                                                    else if (relativePlacement5.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement5.P[0].Y * axisY6;
                                                    }
                                                    else if (relativePlacement5.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement5.P[0].Z * axisZ6;
                                                    }
                                                    if (relativePlacement5.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement5.P[1].X * axisX6;
                                                    }
                                                    else if (relativePlacement5.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement5.P[1].Y * axisY6;
                                                    }
                                                    else if (relativePlacement5.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement5.P[1].Z * axisZ6;
                                                    }
                                                    if (relativePlacement5.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement5.P[2].X * axisX6;
                                                    }
                                                    else if (relativePlacement5.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement5.P[2].Y * axisY6;
                                                    }
                                                    else if (relativePlacement5.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement5.P[2].Z * axisZ6;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX5 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY5 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ5 = p2;

                                                    var relativePlacement4 = _objectPlacement4.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX5 = relativePlacement4.Location.X * p0;
                                                    var moveAxisY5 = relativePlacement4.Location.Y * p1;
                                                    var moveAxisZ5 = relativePlacement4.Location.Z * p2;

                                                    var vectorMove4 = moveAxisX5 + moveAxisY5 + moveAxisZ5;

                                                    #region AxisTransformation
                                                    if (relativePlacement4.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement4.P[0].X * axisX5;
                                                    }
                                                    else if (relativePlacement4.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement4.P[0].Y * axisY5;
                                                    }
                                                    else if (relativePlacement4.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement4.P[0].Z * axisZ5;
                                                    }
                                                    if (relativePlacement4.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement4.P[1].X * axisX5;
                                                    }
                                                    else if (relativePlacement4.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement4.P[1].Y * axisY5;
                                                    }
                                                    else if (relativePlacement4.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement4.P[1].Z * axisZ5;
                                                    }
                                                    if (relativePlacement4.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement4.P[2].X * axisX5;
                                                    }
                                                    else if (relativePlacement4.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement4.P[2].Y * axisY5;
                                                    }
                                                    else if (relativePlacement4.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement4.P[2].Z * axisZ5;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX4 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY4 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ4 = p2;

                                                    var relativePlacement3 = _objectPlacement3.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX4 = relativePlacement3.Location.X * p0;
                                                    var moveAxisY4 = relativePlacement3.Location.Y * p1;
                                                    var moveAxisZ4 = relativePlacement3.Location.Z * p2;

                                                    var vectorMove3 = moveAxisX4 + moveAxisY4 + moveAxisZ4;

                                                    #region AxisTransformation
                                                    if (relativePlacement3.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement3.P[0].X * axisX4;
                                                    }
                                                    else if (relativePlacement3.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement3.P[0].Y * axisY4;
                                                    }
                                                    else if (relativePlacement3.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement3.P[0].Z * axisZ4;
                                                    }
                                                    if (relativePlacement3.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement3.P[1].X * axisX4;
                                                    }
                                                    else if (relativePlacement3.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement3.P[1].Y * axisY4;
                                                    }
                                                    else if (relativePlacement3.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement3.P[1].Z * axisZ4;
                                                    }
                                                    if (relativePlacement3.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement3.P[2].X * axisX4;
                                                    }
                                                    else if (relativePlacement3.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement3.P[2].Y * axisY4;
                                                    }
                                                    else if (relativePlacement3.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement3.P[2].Z * axisZ4;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX3 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY3 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ3 = p2;

                                                    var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX3 = relativePlacement2.Location.X * p0;
                                                    var moveAxisY3 = relativePlacement2.Location.Y * p1;
                                                    var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                                                    var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                                                    #region AxisTransformation
                                                    if (relativePlacement2.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement2.P[0].X * axisX3;
                                                    }
                                                    else if (relativePlacement2.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement2.P[0].Y * axisY3;
                                                    }
                                                    else if (relativePlacement2.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement2.P[0].Z * axisZ3;
                                                    }
                                                    if (relativePlacement2.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement2.P[1].X * axisX3;
                                                    }
                                                    else if (relativePlacement2.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement2.P[1].Y * axisY3;
                                                    }
                                                    else if (relativePlacement2.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement2.P[1].Z * axisZ3;
                                                    }
                                                    if (relativePlacement2.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement2.P[2].X * axisX3;
                                                    }
                                                    else if (relativePlacement2.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement2.P[2].Y * axisY3;
                                                    }
                                                    else if (relativePlacement2.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement2.P[2].Z * axisZ3;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                                                    var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX2 = relativePlacement1.Location.X * p0;
                                                    var moveAxisY2 = relativePlacement1.Location.Y * p1;
                                                    var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                                                    var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                                                    #region AxisTransformation
                                                    if (relativePlacement1.P[0].X != 0)
                                                    {
                                                        p0 = relativePlacement1.P[0].X * axisX2;
                                                    }
                                                    else if (relativePlacement1.P[0].Y != 0)
                                                    {
                                                        p0 = relativePlacement1.P[0].Y * axisY2;
                                                    }
                                                    else if (relativePlacement1.P[0].Z != 0)
                                                    {
                                                        p0 = relativePlacement1.P[0].Z * axisZ2;
                                                    }
                                                    if (relativePlacement1.P[1].X != 0)
                                                    {
                                                        p1 = relativePlacement1.P[1].X * axisX2;
                                                    }
                                                    else if (relativePlacement1.P[1].Y != 0)
                                                    {
                                                        p1 = relativePlacement1.P[1].Y * axisY2;
                                                    }
                                                    else if (relativePlacement1.P[1].Z != 0)
                                                    {
                                                        p1 = relativePlacement1.P[1].Z * axisZ2;
                                                    }
                                                    if (relativePlacement1.P[2].X != 0)
                                                    {
                                                        p2 = relativePlacement1.P[2].X * axisX2;
                                                    }
                                                    else if (relativePlacement1.P[2].Y != 0)
                                                    {
                                                        p2 = relativePlacement1.P[2].Y * axisY2;
                                                    }
                                                    else if (relativePlacement1.P[2].Z != 0)
                                                    {
                                                        p2 = relativePlacement1.P[2].Z * axisZ2;
                                                    }
                                                    #endregion AxisTransformation
                                                    Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                                                    Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                                                    Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                                                    var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                                                    var moveAxisX1 = relativePlacement.Location.X * p0;
                                                    var moveAxisY1 = relativePlacement.Location.Y * p1;
                                                    var moveAxisZ1 = relativePlacement.Location.Z * p2;

                                                    var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                                                    var finalVector = vectorMove + vectorMove1 + vectorMove2 + vectorMove3 + vectorMove4 + vectorMove5 + vectorMove6 + vectorMove7 + vectorMove8;

                                                    v1[0] = v1[0] + finalVector.X;
                                                    v1[1] = v1[1] + finalVector.Y;
                                                    v1[2] = v1[2] + finalVector.Z;

                                                    return v1;

                                                }
                                            }
                                            else
                                            {
                                                var relativePlacement7 = _objectPlacement7.RelativePlacement as IIfcAxis2Placement3D;

                                                p0 = relativePlacement7.P[0];
                                                p1 = relativePlacement7.P[1];
                                                p2 = relativePlacement7.P[2];



                                                var moveAxisX8 = relativePlacement7.Location.X * p0;
                                                var moveAxisY8 = relativePlacement7.Location.Y * p1;
                                                var moveAxisZ8 = relativePlacement7.Location.Z * p2;

                                                var vectorMove7 = moveAxisX8 + moveAxisY8 + moveAxisZ8;

                                                Xbim.Common.Geometry.XbimVector3D axisX7 = p0;
                                                Xbim.Common.Geometry.XbimVector3D axisY7 = p1;
                                                Xbim.Common.Geometry.XbimVector3D axisZ7 = p2;

                                                var relativePlacement6 = _objectPlacement6.RelativePlacement as IIfcAxis2Placement3D;

                                                var moveAxisX7 = relativePlacement6.Location.X * p0;
                                                var moveAxisY7 = relativePlacement6.Location.Y * p1;
                                                var moveAxisZ7 = relativePlacement6.Location.Z * p2;

                                                var vectorMove6 = moveAxisX7 + moveAxisY7 + moveAxisZ7;

                                                #region AxisTransformation
                                                if (relativePlacement6.P[0].X != 0)
                                                {
                                                    p0 = relativePlacement6.P[0].X * axisX7;
                                                }
                                                else if (relativePlacement6.P[0].Y != 0)
                                                {
                                                    p0 = relativePlacement6.P[0].Y * axisY7;
                                                }
                                                else if (relativePlacement6.P[0].Z != 0)
                                                {
                                                    p0 = relativePlacement6.P[0].Z * axisZ7;
                                                }
                                                if (relativePlacement6.P[1].X != 0)
                                                {
                                                    p1 = relativePlacement6.P[1].X * axisX7;
                                                }
                                                else if (relativePlacement6.P[1].Y != 0)
                                                {
                                                    p1 = relativePlacement6.P[1].Y * axisY7;
                                                }
                                                else if (relativePlacement6.P[1].Z != 0)
                                                {
                                                    p1 = relativePlacement6.P[1].Z * axisZ7;
                                                }
                                                if (relativePlacement6.P[2].X != 0)
                                                {
                                                    p2 = relativePlacement6.P[2].X * axisX7;
                                                }
                                                else if (relativePlacement6.P[2].Y != 0)
                                                {
                                                    p2 = relativePlacement6.P[2].Y * axisY7;
                                                }
                                                else if (relativePlacement6.P[2].Z != 0)
                                                {
                                                    p2 = relativePlacement6.P[2].Z * axisZ7;
                                                }
                                                #endregion AxisTransformation
                                                Xbim.Common.Geometry.XbimVector3D axisX6 = p0;
                                                Xbim.Common.Geometry.XbimVector3D axisY6 = p1;
                                                Xbim.Common.Geometry.XbimVector3D axisZ6 = p2;

                                                var relativePlacement5 = _objectPlacement5.RelativePlacement as IIfcAxis2Placement3D;

                                                var moveAxisX6 = relativePlacement5.Location.X * p0;
                                                var moveAxisY6 = relativePlacement5.Location.Y * p1;
                                                var moveAxisZ6 = relativePlacement5.Location.Z * p2;

                                                var vectorMove5 = moveAxisX6 + moveAxisY6 + moveAxisZ6;

                                                #region AxisTransformation
                                                if (relativePlacement5.P[0].X != 0)
                                                {
                                                    p0 = relativePlacement5.P[0].X * axisX6;
                                                }
                                                else if (relativePlacement5.P[0].Y != 0)
                                                {
                                                    p0 = relativePlacement5.P[0].Y * axisY6;
                                                }
                                                else if (relativePlacement5.P[0].Z != 0)
                                                {
                                                    p0 = relativePlacement5.P[0].Z * axisZ6;
                                                }
                                                if (relativePlacement5.P[1].X != 0)
                                                {
                                                    p1 = relativePlacement5.P[1].X * axisX6;
                                                }
                                                else if (relativePlacement5.P[1].Y != 0)
                                                {
                                                    p1 = relativePlacement5.P[1].Y * axisY6;
                                                }
                                                else if (relativePlacement5.P[1].Z != 0)
                                                {
                                                    p1 = relativePlacement5.P[1].Z * axisZ6;
                                                }
                                                if (relativePlacement5.P[2].X != 0)
                                                {
                                                    p2 = relativePlacement5.P[2].X * axisX6;
                                                }
                                                else if (relativePlacement5.P[2].Y != 0)
                                                {
                                                    p2 = relativePlacement5.P[2].Y * axisY6;
                                                }
                                                else if (relativePlacement5.P[2].Z != 0)
                                                {
                                                    p2 = relativePlacement5.P[2].Z * axisZ6;
                                                }
                                                #endregion AxisTransformation
                                                Xbim.Common.Geometry.XbimVector3D axisX5 = p0;
                                                Xbim.Common.Geometry.XbimVector3D axisY5 = p1;
                                                Xbim.Common.Geometry.XbimVector3D axisZ5 = p2;

                                                var relativePlacement4 = _objectPlacement4.RelativePlacement as IIfcAxis2Placement3D;

                                                var moveAxisX5 = relativePlacement4.Location.X * p0;
                                                var moveAxisY5 = relativePlacement4.Location.Y * p1;
                                                var moveAxisZ5 = relativePlacement4.Location.Z * p2;

                                                var vectorMove4 = moveAxisX5 + moveAxisY5 + moveAxisZ5;

                                                #region AxisTransformation
                                                if (relativePlacement4.P[0].X != 0)
                                                {
                                                    p0 = relativePlacement4.P[0].X * axisX5;
                                                }
                                                else if (relativePlacement4.P[0].Y != 0)
                                                {
                                                    p0 = relativePlacement4.P[0].Y * axisY5;
                                                }
                                                else if (relativePlacement4.P[0].Z != 0)
                                                {
                                                    p0 = relativePlacement4.P[0].Z * axisZ5;
                                                }
                                                if (relativePlacement4.P[1].X != 0)
                                                {
                                                    p1 = relativePlacement4.P[1].X * axisX5;
                                                }
                                                else if (relativePlacement4.P[1].Y != 0)
                                                {
                                                    p1 = relativePlacement4.P[1].Y * axisY5;
                                                }
                                                else if (relativePlacement4.P[1].Z != 0)
                                                {
                                                    p1 = relativePlacement4.P[1].Z * axisZ5;
                                                }
                                                if (relativePlacement4.P[2].X != 0)
                                                {
                                                    p2 = relativePlacement4.P[2].X * axisX5;
                                                }
                                                else if (relativePlacement4.P[2].Y != 0)
                                                {
                                                    p2 = relativePlacement4.P[2].Y * axisY5;
                                                }
                                                else if (relativePlacement4.P[2].Z != 0)
                                                {
                                                    p2 = relativePlacement4.P[2].Z * axisZ5;
                                                }
                                                #endregion AxisTransformation
                                                Xbim.Common.Geometry.XbimVector3D axisX4 = p0;
                                                Xbim.Common.Geometry.XbimVector3D axisY4 = p1;
                                                Xbim.Common.Geometry.XbimVector3D axisZ4 = p2;

                                                var relativePlacement3 = _objectPlacement3.RelativePlacement as IIfcAxis2Placement3D;

                                                var moveAxisX4 = relativePlacement3.Location.X * p0;
                                                var moveAxisY4 = relativePlacement3.Location.Y * p1;
                                                var moveAxisZ4 = relativePlacement3.Location.Z * p2;

                                                var vectorMove3 = moveAxisX4 + moveAxisY4 + moveAxisZ4;

                                                #region AxisTransformation
                                                if (relativePlacement3.P[0].X != 0)
                                                {
                                                    p0 = relativePlacement3.P[0].X * axisX4;
                                                }
                                                else if (relativePlacement3.P[0].Y != 0)
                                                {
                                                    p0 = relativePlacement3.P[0].Y * axisY4;
                                                }
                                                else if (relativePlacement3.P[0].Z != 0)
                                                {
                                                    p0 = relativePlacement3.P[0].Z * axisZ4;
                                                }
                                                if (relativePlacement3.P[1].X != 0)
                                                {
                                                    p1 = relativePlacement3.P[1].X * axisX4;
                                                }
                                                else if (relativePlacement3.P[1].Y != 0)
                                                {
                                                    p1 = relativePlacement3.P[1].Y * axisY4;
                                                }
                                                else if (relativePlacement3.P[1].Z != 0)
                                                {
                                                    p1 = relativePlacement3.P[1].Z * axisZ4;
                                                }
                                                if (relativePlacement3.P[2].X != 0)
                                                {
                                                    p2 = relativePlacement3.P[2].X * axisX4;
                                                }
                                                else if (relativePlacement3.P[2].Y != 0)
                                                {
                                                    p2 = relativePlacement3.P[2].Y * axisY4;
                                                }
                                                else if (relativePlacement3.P[2].Z != 0)
                                                {
                                                    p2 = relativePlacement3.P[2].Z * axisZ4;
                                                }
                                                #endregion AxisTransformation
                                                Xbim.Common.Geometry.XbimVector3D axisX3 = p0;
                                                Xbim.Common.Geometry.XbimVector3D axisY3 = p1;
                                                Xbim.Common.Geometry.XbimVector3D axisZ3 = p2;

                                                var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                                                var moveAxisX3 = relativePlacement2.Location.X * p0;
                                                var moveAxisY3 = relativePlacement2.Location.Y * p1;
                                                var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                                                var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                                                #region AxisTransformation
                                                if (relativePlacement2.P[0].X != 0)
                                                {
                                                    p0 = relativePlacement2.P[0].X * axisX3;
                                                }
                                                else if (relativePlacement2.P[0].Y != 0)
                                                {
                                                    p0 = relativePlacement2.P[0].Y * axisY3;
                                                }
                                                else if (relativePlacement2.P[0].Z != 0)
                                                {
                                                    p0 = relativePlacement2.P[0].Z * axisZ3;
                                                }
                                                if (relativePlacement2.P[1].X != 0)
                                                {
                                                    p1 = relativePlacement2.P[1].X * axisX3;
                                                }
                                                else if (relativePlacement2.P[1].Y != 0)
                                                {
                                                    p1 = relativePlacement2.P[1].Y * axisY3;
                                                }
                                                else if (relativePlacement2.P[1].Z != 0)
                                                {
                                                    p1 = relativePlacement2.P[1].Z * axisZ3;
                                                }
                                                if (relativePlacement2.P[2].X != 0)
                                                {
                                                    p2 = relativePlacement2.P[2].X * axisX3;
                                                }
                                                else if (relativePlacement2.P[2].Y != 0)
                                                {
                                                    p2 = relativePlacement2.P[2].Y * axisY3;
                                                }
                                                else if (relativePlacement2.P[2].Z != 0)
                                                {
                                                    p2 = relativePlacement2.P[2].Z * axisZ3;
                                                }
                                                #endregion AxisTransformation
                                                Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                                                Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                                                Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                                                var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                                                var moveAxisX2 = relativePlacement1.Location.X * p0;
                                                var moveAxisY2 = relativePlacement1.Location.Y * p1;
                                                var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                                                var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                                                #region AxisTransformation
                                                if (relativePlacement1.P[0].X != 0)
                                                {
                                                    p0 = relativePlacement1.P[0].X * axisX2;
                                                }
                                                else if (relativePlacement1.P[0].Y != 0)
                                                {
                                                    p0 = relativePlacement1.P[0].Y * axisY2;
                                                }
                                                else if (relativePlacement1.P[0].Z != 0)
                                                {
                                                    p0 = relativePlacement1.P[0].Z * axisZ2;
                                                }
                                                if (relativePlacement1.P[1].X != 0)
                                                {
                                                    p1 = relativePlacement1.P[1].X * axisX2;
                                                }
                                                else if (relativePlacement1.P[1].Y != 0)
                                                {
                                                    p1 = relativePlacement1.P[1].Y * axisY2;
                                                }
                                                else if (relativePlacement1.P[1].Z != 0)
                                                {
                                                    p1 = relativePlacement1.P[1].Z * axisZ2;
                                                }
                                                if (relativePlacement1.P[2].X != 0)
                                                {
                                                    p2 = relativePlacement1.P[2].X * axisX2;
                                                }
                                                else if (relativePlacement1.P[2].Y != 0)
                                                {
                                                    p2 = relativePlacement1.P[2].Y * axisY2;
                                                }
                                                else if (relativePlacement1.P[2].Z != 0)
                                                {
                                                    p2 = relativePlacement1.P[2].Z * axisZ2;
                                                }
                                                #endregion AxisTransformation
                                                Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                                                Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                                                Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                                                var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                                                var moveAxisX1 = relativePlacement.Location.X * p0;
                                                var moveAxisY1 = relativePlacement.Location.Y * p1;
                                                var moveAxisZ1 = relativePlacement.Location.Z * p2;

                                                var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                                                var finalVector = vectorMove + vectorMove1 + vectorMove2 + vectorMove3 + vectorMove4 + vectorMove5 + vectorMove6 + vectorMove7;

                                                v1[0] = v1[0] + finalVector.X;
                                                v1[1] = v1[1] + finalVector.Y;
                                                v1[2] = v1[2] + finalVector.Z;

                                                return v1;
                                            }
                                        }
                                        else
                                        {
                                            var relativePlacement6 = _objectPlacement6.RelativePlacement as IIfcAxis2Placement3D;

                                            p0 = relativePlacement6.P[0];
                                            p1 = relativePlacement6.P[1];
                                            p2 = relativePlacement6.P[2];



                                            var moveAxisX7 = relativePlacement6.Location.X * p0;
                                            var moveAxisY7 = relativePlacement6.Location.Y * p1;
                                            var moveAxisZ7 = relativePlacement6.Location.Z * p2;

                                            var vectorMove6 = moveAxisX7 + moveAxisY7 + moveAxisZ7;

                                            Xbim.Common.Geometry.XbimVector3D axisX6 = p0;
                                            Xbim.Common.Geometry.XbimVector3D axisY6 = p1;
                                            Xbim.Common.Geometry.XbimVector3D axisZ6 = p2;

                                            var relativePlacement5 = _objectPlacement5.RelativePlacement as IIfcAxis2Placement3D;

                                            var moveAxisX6 = relativePlacement5.Location.X * p0;
                                            var moveAxisY6 = relativePlacement5.Location.Y * p1;
                                            var moveAxisZ6 = relativePlacement5.Location.Z * p2;

                                            var vectorMove5 = moveAxisX6 + moveAxisY6 + moveAxisZ6;

                                            #region AxisTransformation
                                            if (relativePlacement5.P[0].X != 0)
                                            {
                                                p0 = relativePlacement5.P[0].X * axisX6;
                                            }
                                            else if (relativePlacement5.P[0].Y != 0)
                                            {
                                                p0 = relativePlacement5.P[0].Y * axisY6;
                                            }
                                            else if (relativePlacement5.P[0].Z != 0)
                                            {
                                                p0 = relativePlacement5.P[0].Z * axisZ6;
                                            }
                                            if (relativePlacement5.P[1].X != 0)
                                            {
                                                p1 = relativePlacement5.P[1].X * axisX6;
                                            }
                                            else if (relativePlacement5.P[1].Y != 0)
                                            {
                                                p1 = relativePlacement5.P[1].Y * axisY6;
                                            }
                                            else if (relativePlacement5.P[1].Z != 0)
                                            {
                                                p1 = relativePlacement5.P[1].Z * axisZ6;
                                            }
                                            if (relativePlacement5.P[2].X != 0)
                                            {
                                                p2 = relativePlacement5.P[2].X * axisX6;
                                            }
                                            else if (relativePlacement5.P[2].Y != 0)
                                            {
                                                p2 = relativePlacement5.P[2].Y * axisY6;
                                            }
                                            else if (relativePlacement5.P[2].Z != 0)
                                            {
                                                p2 = relativePlacement5.P[2].Z * axisZ6;
                                            }
                                            #endregion AxisTransformation
                                            Xbim.Common.Geometry.XbimVector3D axisX5 = p0;
                                            Xbim.Common.Geometry.XbimVector3D axisY5 = p1;
                                            Xbim.Common.Geometry.XbimVector3D axisZ5 = p2;

                                            var relativePlacement4 = _objectPlacement4.RelativePlacement as IIfcAxis2Placement3D;

                                            var moveAxisX5 = relativePlacement4.Location.X * p0;
                                            var moveAxisY5 = relativePlacement4.Location.Y * p1;
                                            var moveAxisZ5 = relativePlacement4.Location.Z * p2;

                                            var vectorMove4 = moveAxisX5 + moveAxisY5 + moveAxisZ5;

                                            #region AxisTransformation
                                            if (relativePlacement4.P[0].X != 0)
                                            {
                                                p0 = relativePlacement4.P[0].X * axisX5;
                                            }
                                            else if (relativePlacement4.P[0].Y != 0)
                                            {
                                                p0 = relativePlacement4.P[0].Y * axisY5;
                                            }
                                            else if (relativePlacement4.P[0].Z != 0)
                                            {
                                                p0 = relativePlacement4.P[0].Z * axisZ5;
                                            }
                                            if (relativePlacement4.P[1].X != 0)
                                            {
                                                p1 = relativePlacement4.P[1].X * axisX5;
                                            }
                                            else if (relativePlacement4.P[1].Y != 0)
                                            {
                                                p1 = relativePlacement4.P[1].Y * axisY5;
                                            }
                                            else if (relativePlacement4.P[1].Z != 0)
                                            {
                                                p1 = relativePlacement4.P[1].Z * axisZ5;
                                            }
                                            if (relativePlacement4.P[2].X != 0)
                                            {
                                                p2 = relativePlacement4.P[2].X * axisX5;
                                            }
                                            else if (relativePlacement4.P[2].Y != 0)
                                            {
                                                p2 = relativePlacement4.P[2].Y * axisY5;
                                            }
                                            else if (relativePlacement4.P[2].Z != 0)
                                            {
                                                p2 = relativePlacement4.P[2].Z * axisZ5;
                                            }
                                            #endregion AxisTransformation
                                            Xbim.Common.Geometry.XbimVector3D axisX4 = p0;
                                            Xbim.Common.Geometry.XbimVector3D axisY4 = p1;
                                            Xbim.Common.Geometry.XbimVector3D axisZ4 = p2;

                                            var relativePlacement3 = _objectPlacement3.RelativePlacement as IIfcAxis2Placement3D;

                                            var moveAxisX4 = relativePlacement3.Location.X * p0;
                                            var moveAxisY4 = relativePlacement3.Location.Y * p1;
                                            var moveAxisZ4 = relativePlacement3.Location.Z * p2;

                                            var vectorMove3 = moveAxisX4 + moveAxisY4 + moveAxisZ4;

                                            #region AxisTransformation
                                            if (relativePlacement3.P[0].X != 0)
                                            {
                                                p0 = relativePlacement3.P[0].X * axisX4;
                                            }
                                            else if (relativePlacement3.P[0].Y != 0)
                                            {
                                                p0 = relativePlacement3.P[0].Y * axisY4;
                                            }
                                            else if (relativePlacement3.P[0].Z != 0)
                                            {
                                                p0 = relativePlacement3.P[0].Z * axisZ4;
                                            }
                                            if (relativePlacement3.P[1].X != 0)
                                            {
                                                p1 = relativePlacement3.P[1].X * axisX4;
                                            }
                                            else if (relativePlacement3.P[1].Y != 0)
                                            {
                                                p1 = relativePlacement3.P[1].Y * axisY4;
                                            }
                                            else if (relativePlacement3.P[1].Z != 0)
                                            {
                                                p1 = relativePlacement3.P[1].Z * axisZ4;
                                            }
                                            if (relativePlacement3.P[2].X != 0)
                                            {
                                                p2 = relativePlacement3.P[2].X * axisX4;
                                            }
                                            else if (relativePlacement3.P[2].Y != 0)
                                            {
                                                p2 = relativePlacement3.P[2].Y * axisY4;
                                            }
                                            else if (relativePlacement3.P[2].Z != 0)
                                            {
                                                p2 = relativePlacement3.P[2].Z * axisZ4;
                                            }
                                            #endregion AxisTransformation
                                            Xbim.Common.Geometry.XbimVector3D axisX3 = p0;
                                            Xbim.Common.Geometry.XbimVector3D axisY3 = p1;
                                            Xbim.Common.Geometry.XbimVector3D axisZ3 = p2;

                                            var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                                            var moveAxisX3 = relativePlacement2.Location.X * p0;
                                            var moveAxisY3 = relativePlacement2.Location.Y * p1;
                                            var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                                            var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                                            #region AxisTransformation
                                            if (relativePlacement2.P[0].X != 0)
                                            {
                                                p0 = relativePlacement2.P[0].X * axisX3;
                                            }
                                            else if (relativePlacement2.P[0].Y != 0)
                                            {
                                                p0 = relativePlacement2.P[0].Y * axisY3;
                                            }
                                            else if (relativePlacement2.P[0].Z != 0)
                                            {
                                                p0 = relativePlacement2.P[0].Z * axisZ3;
                                            }
                                            if (relativePlacement2.P[1].X != 0)
                                            {
                                                p1 = relativePlacement2.P[1].X * axisX3;
                                            }
                                            else if (relativePlacement2.P[1].Y != 0)
                                            {
                                                p1 = relativePlacement2.P[1].Y * axisY3;
                                            }
                                            else if (relativePlacement2.P[1].Z != 0)
                                            {
                                                p1 = relativePlacement2.P[1].Z * axisZ3;
                                            }
                                            if (relativePlacement2.P[2].X != 0)
                                            {
                                                p2 = relativePlacement2.P[2].X * axisX3;
                                            }
                                            else if (relativePlacement2.P[2].Y != 0)
                                            {
                                                p2 = relativePlacement2.P[2].Y * axisY3;
                                            }
                                            else if (relativePlacement2.P[2].Z != 0)
                                            {
                                                p2 = relativePlacement2.P[2].Z * axisZ3;
                                            }
                                            #endregion AxisTransformation
                                            Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                                            Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                                            Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                                            var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                                            var moveAxisX2 = relativePlacement1.Location.X * p0;
                                            var moveAxisY2 = relativePlacement1.Location.Y * p1;
                                            var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                                            var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                                            #region AxisTransformation
                                            if (relativePlacement1.P[0].X != 0)
                                            {
                                                p0 = relativePlacement1.P[0].X * axisX2;
                                            }
                                            else if (relativePlacement1.P[0].Y != 0)
                                            {
                                                p0 = relativePlacement1.P[0].Y * axisY2;
                                            }
                                            else if (relativePlacement1.P[0].Z != 0)
                                            {
                                                p0 = relativePlacement1.P[0].Z * axisZ2;
                                            }
                                            if (relativePlacement1.P[1].X != 0)
                                            {
                                                p1 = relativePlacement1.P[1].X * axisX2;
                                            }
                                            else if (relativePlacement1.P[1].Y != 0)
                                            {
                                                p1 = relativePlacement1.P[1].Y * axisY2;
                                            }
                                            else if (relativePlacement1.P[1].Z != 0)
                                            {
                                                p1 = relativePlacement1.P[1].Z * axisZ2;
                                            }
                                            if (relativePlacement1.P[2].X != 0)
                                            {
                                                p2 = relativePlacement1.P[2].X * axisX2;
                                            }
                                            else if (relativePlacement1.P[2].Y != 0)
                                            {
                                                p2 = relativePlacement1.P[2].Y * axisY2;
                                            }
                                            else if (relativePlacement1.P[2].Z != 0)
                                            {
                                                p2 = relativePlacement1.P[2].Z * axisZ2;
                                            }
                                            #endregion AxisTransformation
                                            Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                                            Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                                            Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                                            var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                                            var moveAxisX1 = relativePlacement.Location.X * p0;
                                            var moveAxisY1 = relativePlacement.Location.Y * p1;
                                            var moveAxisZ1 = relativePlacement.Location.Z * p2;

                                            var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                                            var finalVector = vectorMove + vectorMove1 + vectorMove2 + vectorMove3 + vectorMove4 + vectorMove5 + vectorMove6;

                                            v1[0] = v1[0] + finalVector.X;
                                            v1[1] = v1[1] + finalVector.Y;
                                            v1[2] = v1[2] + finalVector.Z;

                                            return v1;
                                        }
                                        
                                    }
                                    else
                                    {
                                        var relativePlacement5 = _objectPlacement5.RelativePlacement as IIfcAxis2Placement3D;

                                        p0 = relativePlacement5.P[0];
                                        p1 = relativePlacement5.P[1];
                                        p2 = relativePlacement5.P[2];



                                        var moveAxisX6 = relativePlacement5.Location.X * p0;
                                        var moveAxisY6 = relativePlacement5.Location.Y * p1;
                                        var moveAxisZ6 = relativePlacement5.Location.Z * p2;

                                        var vectorMove5 = moveAxisX6 + moveAxisY6 + moveAxisZ6;

                                        Xbim.Common.Geometry.XbimVector3D axisX5 = p0;
                                        Xbim.Common.Geometry.XbimVector3D axisY5 = p1;
                                        Xbim.Common.Geometry.XbimVector3D axisZ5 = p2;

                                        var relativePlacement4 = _objectPlacement4.RelativePlacement as IIfcAxis2Placement3D;

                                        var moveAxisX5 = relativePlacement4.Location.X * p0;
                                        var moveAxisY5 = relativePlacement4.Location.Y * p1;
                                        var moveAxisZ5 = relativePlacement4.Location.Z * p2;

                                        var vectorMove4 = moveAxisX5 + moveAxisY5 + moveAxisZ5;

                                        #region AxisTransformation
                                        if (relativePlacement4.P[0].X != 0)
                                        {
                                            p0 = relativePlacement4.P[0].X * axisX5;
                                        }
                                        else if (relativePlacement4.P[0].Y != 0)
                                        {
                                            p0 = relativePlacement4.P[0].Y * axisY5;
                                        }
                                        else if (relativePlacement4.P[0].Z != 0)
                                        {
                                            p0 = relativePlacement4.P[0].Z * axisZ5;
                                        }
                                        if (relativePlacement4.P[1].X != 0)
                                        {
                                            p1 = relativePlacement4.P[1].X * axisX5;
                                        }
                                        else if (relativePlacement4.P[1].Y != 0)
                                        {
                                            p1 = relativePlacement4.P[1].Y * axisY5;
                                        }
                                        else if (relativePlacement4.P[1].Z != 0)
                                        {
                                            p1 = relativePlacement4.P[1].Z * axisZ5;
                                        }
                                        if (relativePlacement4.P[2].X != 0)
                                        {
                                            p2 = relativePlacement4.P[2].X * axisX5;
                                        }
                                        else if (relativePlacement4.P[2].Y != 0)
                                        {
                                            p2 = relativePlacement4.P[2].Y * axisY5;
                                        }
                                        else if (relativePlacement4.P[2].Z != 0)
                                        {
                                            p2 = relativePlacement4.P[2].Z * axisZ5;
                                        }
                                        #endregion AxisTransformation
                                        Xbim.Common.Geometry.XbimVector3D axisX4 = p0;
                                        Xbim.Common.Geometry.XbimVector3D axisY4 = p1;
                                        Xbim.Common.Geometry.XbimVector3D axisZ4 = p2;

                                        var relativePlacement3 = _objectPlacement3.RelativePlacement as IIfcAxis2Placement3D;

                                        var moveAxisX4 = relativePlacement3.Location.X * p0;
                                        var moveAxisY4 = relativePlacement3.Location.Y * p1;
                                        var moveAxisZ4 = relativePlacement3.Location.Z * p2;

                                        var vectorMove3 = moveAxisX4 + moveAxisY4 + moveAxisZ4;

                                        #region AxisTransformation
                                        if (relativePlacement3.P[0].X != 0)
                                        {
                                            p0 = relativePlacement3.P[0].X * axisX4;
                                        }
                                        else if (relativePlacement3.P[0].Y != 0)
                                        {
                                            p0 = relativePlacement3.P[0].Y * axisY4;
                                        }
                                        else if (relativePlacement3.P[0].Z != 0)
                                        {
                                            p0 = relativePlacement3.P[0].Z * axisZ4;
                                        }
                                        if (relativePlacement3.P[1].X != 0)
                                        {
                                            p1 = relativePlacement3.P[1].X * axisX4;
                                        }
                                        else if (relativePlacement3.P[1].Y != 0)
                                        {
                                            p1 = relativePlacement3.P[1].Y * axisY4;
                                        }
                                        else if (relativePlacement3.P[1].Z != 0)
                                        {
                                            p1 = relativePlacement3.P[1].Z * axisZ4;
                                        }
                                        if (relativePlacement3.P[2].X != 0)
                                        {
                                            p2 = relativePlacement3.P[2].X * axisX4;
                                        }
                                        else if (relativePlacement3.P[2].Y != 0)
                                        {
                                            p2 = relativePlacement3.P[2].Y * axisY4;
                                        }
                                        else if (relativePlacement3.P[2].Z != 0)
                                        {
                                            p2 = relativePlacement3.P[2].Z * axisZ4;
                                        }
                                        #endregion AxisTransformation
                                        Xbim.Common.Geometry.XbimVector3D axisX3 = p0;
                                        Xbim.Common.Geometry.XbimVector3D axisY3 = p1;
                                        Xbim.Common.Geometry.XbimVector3D axisZ3 = p2;

                                        var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                                        var moveAxisX3 = relativePlacement2.Location.X * p0;
                                        var moveAxisY3 = relativePlacement2.Location.Y * p1;
                                        var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                                        var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                                        #region AxisTransformation
                                        if (relativePlacement2.P[0].X != 0)
                                        {
                                            p0 = relativePlacement2.P[0].X * axisX3;
                                        }
                                        else if (relativePlacement2.P[0].Y != 0)
                                        {
                                            p0 = relativePlacement2.P[0].Y * axisY3;
                                        }
                                        else if (relativePlacement2.P[0].Z != 0)
                                        {
                                            p0 = relativePlacement2.P[0].Z * axisZ3;
                                        }
                                        if (relativePlacement2.P[1].X != 0)
                                        {
                                            p1 = relativePlacement2.P[1].X * axisX3;
                                        }
                                        else if (relativePlacement2.P[1].Y != 0)
                                        {
                                            p1 = relativePlacement2.P[1].Y * axisY3;
                                        }
                                        else if (relativePlacement2.P[1].Z != 0)
                                        {
                                            p1 = relativePlacement2.P[1].Z * axisZ3;
                                        }
                                        if (relativePlacement2.P[2].X != 0)
                                        {
                                            p2 = relativePlacement2.P[2].X * axisX3;
                                        }
                                        else if (relativePlacement2.P[2].Y != 0)
                                        {
                                            p2 = relativePlacement2.P[2].Y * axisY3;
                                        }
                                        else if (relativePlacement2.P[2].Z != 0)
                                        {
                                            p2 = relativePlacement2.P[2].Z * axisZ3;
                                        }
                                        #endregion AxisTransformation
                                        Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                                        Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                                        Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                                        var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                                        var moveAxisX2 = relativePlacement1.Location.X * p0;
                                        var moveAxisY2 = relativePlacement1.Location.Y * p1;
                                        var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                                        var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                                        #region AxisTransformation
                                        if (relativePlacement1.P[0].X != 0)
                                        {
                                            p0 = relativePlacement1.P[0].X * axisX2;
                                        }
                                        else if (relativePlacement1.P[0].Y != 0)
                                        {
                                            p0 = relativePlacement1.P[0].Y * axisY2;
                                        }
                                        else if (relativePlacement1.P[0].Z != 0)
                                        {
                                            p0 = relativePlacement1.P[0].Z * axisZ2;
                                        }
                                        if (relativePlacement1.P[1].X != 0)
                                        {
                                            p1 = relativePlacement1.P[1].X * axisX2;
                                        }
                                        else if (relativePlacement1.P[1].Y != 0)
                                        {
                                            p1 = relativePlacement1.P[1].Y * axisY2;
                                        }
                                        else if (relativePlacement1.P[1].Z != 0)
                                        {
                                            p1 = relativePlacement1.P[1].Z * axisZ2;
                                        }
                                        if (relativePlacement1.P[2].X != 0)
                                        {
                                            p2 = relativePlacement1.P[2].X * axisX2;
                                        }
                                        else if (relativePlacement1.P[2].Y != 0)
                                        {
                                            p2 = relativePlacement1.P[2].Y * axisY2;
                                        }
                                        else if (relativePlacement1.P[2].Z != 0)
                                        {
                                            p2 = relativePlacement1.P[2].Z * axisZ2;
                                        }
                                        #endregion AxisTransformation
                                        Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                                        Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                                        Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                                        var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                                        var moveAxisX1 = relativePlacement.Location.X * p0;
                                        var moveAxisY1 = relativePlacement.Location.Y * p1;
                                        var moveAxisZ1 = relativePlacement.Location.Z * p2;

                                        var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                                        var finalVector = vectorMove + vectorMove1 + vectorMove2 + vectorMove3 + vectorMove4 + vectorMove5;

                                        v1[0] = v1[0] + finalVector.X;
                                        v1[1] = v1[1] + finalVector.Y;
                                        v1[2] = v1[2] + finalVector.Z;

                                        return v1;
                                    }
                                }
                                else
                                {
                                    var relativePlacement4 = _objectPlacement4.RelativePlacement as IIfcAxis2Placement3D;

                                    p0 = relativePlacement4.P[0];
                                    p1 = relativePlacement4.P[1];
                                    p2 = relativePlacement4.P[2];



                                    var moveAxisX5 = relativePlacement4.Location.X * p0;
                                    var moveAxisY5 = relativePlacement4.Location.Y * p1;
                                    var moveAxisZ5 = relativePlacement4.Location.Z * p2;

                                    var vectorMove4 = moveAxisX5 + moveAxisY5 + moveAxisZ5;

                                    Xbim.Common.Geometry.XbimVector3D axisX4 = p0;
                                    Xbim.Common.Geometry.XbimVector3D axisY4 = p1;
                                    Xbim.Common.Geometry.XbimVector3D axisZ4 = p2;

                                    var relativePlacement3 = _objectPlacement3.RelativePlacement as IIfcAxis2Placement3D;

                                    var moveAxisX4 = relativePlacement3.Location.X * p0;
                                    var moveAxisY4 = relativePlacement3.Location.Y * p1;
                                    var moveAxisZ4 = relativePlacement3.Location.Z * p2;

                                    var vectorMove3 = moveAxisX4 + moveAxisY4 + moveAxisZ4;

                                    #region AxisTransformation
                                    if (relativePlacement3.P[0].X != 0)
                                    {
                                        p0 = relativePlacement3.P[0].X * axisX4;
                                    }
                                    else if (relativePlacement3.P[0].Y != 0)
                                    {
                                        p0 = relativePlacement3.P[0].Y * axisY4;
                                    }
                                    else if (relativePlacement3.P[0].Z != 0)
                                    {
                                        p0 = relativePlacement3.P[0].Z * axisZ4;
                                    }
                                    if (relativePlacement3.P[1].X != 0)
                                    {
                                        p1 = relativePlacement3.P[1].X * axisX4;
                                    }
                                    else if (relativePlacement3.P[1].Y != 0)
                                    {
                                        p1 = relativePlacement3.P[1].Y * axisY4;
                                    }
                                    else if (relativePlacement3.P[1].Z != 0)
                                    {
                                        p1 = relativePlacement3.P[1].Z * axisZ4;
                                    }
                                    if (relativePlacement3.P[2].X != 0)
                                    {
                                        p2 = relativePlacement3.P[2].X * axisX4;
                                    }
                                    else if (relativePlacement3.P[2].Y != 0)
                                    {
                                        p2 = relativePlacement3.P[2].Y * axisY4;
                                    }
                                    else if (relativePlacement3.P[2].Z != 0)
                                    {
                                        p2 = relativePlacement3.P[2].Z * axisZ4;
                                    }
                                    #endregion AxisTransformation
                                    Xbim.Common.Geometry.XbimVector3D axisX3 = p0;
                                    Xbim.Common.Geometry.XbimVector3D axisY3 = p1;
                                    Xbim.Common.Geometry.XbimVector3D axisZ3 = p2;

                                    var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                                    var moveAxisX3 = relativePlacement2.Location.X * p0;
                                    var moveAxisY3 = relativePlacement2.Location.Y * p1;
                                    var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                                    var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                                    #region AxisTransformation
                                    if (relativePlacement2.P[0].X != 0)
                                    {
                                        p0 = relativePlacement2.P[0].X * axisX3;
                                    }
                                    else if (relativePlacement2.P[0].Y != 0)
                                    {
                                        p0 = relativePlacement2.P[0].Y * axisY3;
                                    }
                                    else if (relativePlacement2.P[0].Z != 0)
                                    {
                                        p0 = relativePlacement2.P[0].Z * axisZ3;
                                    }
                                    if (relativePlacement2.P[1].X != 0)
                                    {
                                        p1 = relativePlacement2.P[1].X * axisX3;
                                    }
                                    else if (relativePlacement2.P[1].Y != 0)
                                    {
                                        p1 = relativePlacement2.P[1].Y * axisY3;
                                    }
                                    else if (relativePlacement2.P[1].Z != 0)
                                    {
                                        p1 = relativePlacement2.P[1].Z * axisZ3;
                                    }
                                    if (relativePlacement2.P[2].X != 0)
                                    {
                                        p2 = relativePlacement2.P[2].X * axisX3;
                                    }
                                    else if (relativePlacement2.P[2].Y != 0)
                                    {
                                        p2 = relativePlacement2.P[2].Y * axisY3;
                                    }
                                    else if (relativePlacement2.P[2].Z != 0)
                                    {
                                        p2 = relativePlacement2.P[2].Z * axisZ3;
                                    }
                                    #endregion AxisTransformation
                                    Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                                    Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                                    Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                                    var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                                    var moveAxisX2 = relativePlacement1.Location.X * p0;
                                    var moveAxisY2 = relativePlacement1.Location.Y * p1;
                                    var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                                    var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                                    #region AxisTransformation
                                    if (relativePlacement1.P[0].X != 0)
                                    {
                                        p0 = relativePlacement1.P[0].X * axisX2;
                                    }
                                    else if (relativePlacement1.P[0].Y != 0)
                                    {
                                        p0 = relativePlacement1.P[0].Y * axisY2;
                                    }
                                    else if (relativePlacement1.P[0].Z != 0)
                                    {
                                        p0 = relativePlacement1.P[0].Z * axisZ2;
                                    }
                                    if (relativePlacement1.P[1].X != 0)
                                    {
                                        p1 = relativePlacement1.P[1].X * axisX2;
                                    }
                                    else if (relativePlacement1.P[1].Y != 0)
                                    {
                                        p1 = relativePlacement1.P[1].Y * axisY2;
                                    }
                                    else if (relativePlacement1.P[1].Z != 0)
                                    {
                                        p1 = relativePlacement1.P[1].Z * axisZ2;
                                    }
                                    if (relativePlacement1.P[2].X != 0)
                                    {
                                        p2 = relativePlacement1.P[2].X * axisX2;
                                    }
                                    else if (relativePlacement1.P[2].Y != 0)
                                    {
                                        p2 = relativePlacement1.P[2].Y * axisY2;
                                    }
                                    else if (relativePlacement1.P[2].Z != 0)
                                    {
                                        p2 = relativePlacement1.P[2].Z * axisZ2;
                                    }
                                    #endregion AxisTransformation
                                    Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                                    Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                                    Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                                    var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                                    var moveAxisX1 = relativePlacement.Location.X * p0;
                                    var moveAxisY1 = relativePlacement.Location.Y * p1;
                                    var moveAxisZ1 = relativePlacement.Location.Z * p2;

                                    var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                                    var finalVector = vectorMove + vectorMove1 + vectorMove2 + vectorMove3 + vectorMove4;

                                    v1[0] = v1[0] + finalVector.X;
                                    v1[1] = v1[1] + finalVector.Y;
                                    v1[2] = v1[2] + finalVector.Z;

                                    return v1;
                                }
                            }
                            else
                            {
                                var relativePlacement3 = _objectPlacement3.RelativePlacement as IIfcAxis2Placement3D;

                                p0 = relativePlacement3.P[0];
                                p1 = relativePlacement3.P[1];
                                p2 = relativePlacement3.P[2];

                                var moveAxisX4 = relativePlacement3.Location.X * p0;
                                var moveAxisY4 = relativePlacement3.Location.Y * p1;
                                var moveAxisZ4 = relativePlacement3.Location.Z * p2;

                                var vectorMove3 = moveAxisX4 + moveAxisY4 + moveAxisZ4;

                                Xbim.Common.Geometry.XbimVector3D axisX3 = p0;
                                Xbim.Common.Geometry.XbimVector3D axisY3 = p1;
                                Xbim.Common.Geometry.XbimVector3D axisZ3 = p2;

                                var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                                var moveAxisX3 = relativePlacement2.Location.X * p0;
                                var moveAxisY3 = relativePlacement2.Location.Y * p1;
                                var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                                var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                                #region AxisTransformation
                                if (relativePlacement2.P[0].X != 0)
                                {
                                    p0 = relativePlacement2.P[0].X * axisX3;
                                }
                                else if (relativePlacement2.P[0].Y != 0)
                                {
                                    p0 = relativePlacement2.P[0].Y * axisY3;
                                }
                                else if (relativePlacement2.P[0].Z != 0)
                                {
                                    p0 = relativePlacement2.P[0].Z * axisZ3;
                                }
                                if (relativePlacement2.P[1].X != 0)
                                {
                                    p1 = relativePlacement2.P[1].X * axisX3;
                                }
                                else if (relativePlacement2.P[1].Y != 0)
                                {
                                    p1 = relativePlacement2.P[1].Y * axisY3;
                                }
                                else if (relativePlacement2.P[1].Z != 0)
                                {
                                    p1 = relativePlacement2.P[1].Z * axisZ3;
                                }
                                if (relativePlacement2.P[2].X != 0)
                                {
                                    p2 = relativePlacement2.P[2].X * axisX3;
                                }
                                else if (relativePlacement2.P[2].Y != 0)
                                {
                                    p2 = relativePlacement2.P[2].Y * axisY3;
                                }
                                else if (relativePlacement2.P[2].Z != 0)
                                {
                                    p2 = relativePlacement2.P[2].Z * axisZ3;
                                }
                                #endregion AxisTransformation
                                Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                                Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                                Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                                var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                                var moveAxisX2 = relativePlacement1.Location.X * p0;
                                var moveAxisY2 = relativePlacement1.Location.Y * p1;
                                var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                                var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                                #region AxisTransformation
                                if (relativePlacement1.P[0].X != 0)
                                {
                                    p0 = relativePlacement1.P[0].X * axisX2;
                                }
                                else if (relativePlacement1.P[0].Y != 0)
                                {
                                    p0 = relativePlacement1.P[0].Y * axisY2;
                                }
                                else if (relativePlacement1.P[0].Z != 0)
                                {
                                    p0 = relativePlacement1.P[0].Z * axisZ2;
                                }
                                if (relativePlacement1.P[1].X != 0)
                                {
                                    p1 = relativePlacement1.P[1].X * axisX2;
                                }
                                else if (relativePlacement1.P[1].Y != 0)
                                {
                                    p1 = relativePlacement1.P[1].Y * axisY2;
                                }
                                else if (relativePlacement1.P[1].Z != 0)
                                {
                                    p1 = relativePlacement1.P[1].Z * axisZ2;
                                }
                                if (relativePlacement1.P[2].X != 0)
                                {
                                    p2 = relativePlacement1.P[2].X * axisX2;
                                }
                                else if (relativePlacement1.P[2].Y != 0)
                                {
                                    p2 = relativePlacement1.P[2].Y * axisY2;
                                }
                                else if (relativePlacement1.P[2].Z != 0)
                                {
                                    p2 = relativePlacement1.P[2].Z * axisZ2;
                                }
                                #endregion AxisTransformation
                                Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                                Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                                Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                                var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                                var moveAxisX1 = relativePlacement.Location.X * p0;
                                var moveAxisY1 = relativePlacement.Location.Y * p1;
                                var moveAxisZ1 = relativePlacement.Location.Z * p2;

                                var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                                var finalVector = vectorMove + vectorMove1 + vectorMove2 + vectorMove3;

                                v1[0] = v1[0] + finalVector.X;
                                v1[1] = v1[1] + finalVector.Y;
                                v1[2] = v1[2] + finalVector.Z;

                                return v1;
                            }
                        }
                        else
                        {
                            var relativePlacement2 = _objectPlacement2.RelativePlacement as IIfcAxis2Placement3D;

                            p0 = relativePlacement2.P[0];
                            p1 = relativePlacement2.P[1];
                            p2 = relativePlacement2.P[2];

                            var moveAxisX3 = relativePlacement2.Location.X * p0;
                            var moveAxisY3 = relativePlacement2.Location.Y * p1;
                            var moveAxisZ3 = relativePlacement2.Location.Z * p2;

                            var vectorMove2 = moveAxisX3 + moveAxisY3 + moveAxisZ3;

                            Xbim.Common.Geometry.XbimVector3D axisX2 = p0;
                            Xbim.Common.Geometry.XbimVector3D axisY2 = p1;
                            Xbim.Common.Geometry.XbimVector3D axisZ2 = p2;

                            var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                            var moveAxisX2 = relativePlacement1.Location.X * p0;
                            var moveAxisY2 = relativePlacement1.Location.Y * p1;
                            var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                            var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                            #region AxisTransformation
                            if (relativePlacement1.P[0].X != 0)
                            {
                                p0 = relativePlacement1.P[0].X * axisX2;
                            }
                            else if (relativePlacement1.P[0].Y != 0)
                            {
                                p0 = relativePlacement1.P[0].Y * axisY2;
                            }
                            else if (relativePlacement1.P[0].Z != 0)
                            {
                                p0 = relativePlacement1.P[0].Z * axisZ2;
                            }
                            if (relativePlacement1.P[1].X != 0)
                            {
                                p1 = relativePlacement1.P[1].X * axisX2;
                            }
                            else if (relativePlacement1.P[1].Y != 0)
                            {
                                p1 = relativePlacement1.P[1].Y * axisY2;
                            }
                            else if (relativePlacement1.P[1].Z != 0)
                            {
                                p1 = relativePlacement1.P[1].Z * axisZ2;
                            }
                            if (relativePlacement1.P[2].X != 0)
                            {
                                p2 = relativePlacement1.P[2].X * axisX2;
                            }
                            else if (relativePlacement1.P[2].Y != 0)
                            {
                                p2 = relativePlacement1.P[2].Y * axisY2;
                            }
                            else if (relativePlacement1.P[2].Z != 0)
                            {
                                p2 = relativePlacement1.P[2].Z * axisZ2;
                            }
                            #endregion AxisTransformation
                            Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                            Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                            Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                            var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                            var moveAxisX1 = relativePlacement.Location.X * p0;
                            var moveAxisY1 = relativePlacement.Location.Y * p1;
                            var moveAxisZ1 = relativePlacement.Location.Z * p2;

                            var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                            var finalVector = vectorMove + vectorMove1 + vectorMove2;

                            v1[0] = v1[0] + finalVector.X;
                            v1[1] = v1[1] + finalVector.Y;
                            v1[2] = v1[2] + finalVector.Z;

                            return v1;
                        }
                    }
                    else
                    {
                        var relativePlacement1 = _objectPlacement1.RelativePlacement as IIfcAxis2Placement3D;

                        p0 = relativePlacement1.P[0];
                        p1 = relativePlacement1.P[1];
                        p2 = relativePlacement1.P[2];

                        var moveAxisX2 = relativePlacement1.Location.X * p0;
                        var moveAxisY2 = relativePlacement1.Location.Y * p1;
                        var moveAxisZ2 = relativePlacement1.Location.Z * p2;

                        var vectorMove1 = moveAxisX2 + moveAxisY2 + moveAxisZ2;

                        Xbim.Common.Geometry.XbimVector3D axisX1 = p0;
                        Xbim.Common.Geometry.XbimVector3D axisY1 = p1;
                        Xbim.Common.Geometry.XbimVector3D axisZ1 = p2;

                        var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                        var moveAxisX1 = relativePlacement.Location.X * p0;
                        var moveAxisY1 = relativePlacement.Location.Y * p1;
                        var moveAxisZ1 = relativePlacement.Location.Z * p2;

                        var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                        var finalVector = vectorMove + vectorMove1 ;

                        v1[0] = v1[0] + finalVector.X;
                        v1[1] = v1[1] + finalVector.Y;
                        v1[2] = v1[2] + finalVector.Z;

                        return v1;
                    }

                }
                else
                {
                    var relativePlacement = _objectPlacement.RelativePlacement as IIfcAxis2Placement3D;

                    p0 = relativePlacement.P[0];
                    p1 = relativePlacement.P[1];
                    p2 = relativePlacement.P[2];

                    var moveAxisX1 = relativePlacement.Location.X * p0;
                    var moveAxisY1 = relativePlacement.Location.Y * p1;
                    var moveAxisZ1 = relativePlacement.Location.Z * p2;

                    var vectorMove = moveAxisX1 + moveAxisY1 + moveAxisZ1;

                    Xbim.Common.Geometry.XbimVector3D axisX = p0;
                    Xbim.Common.Geometry.XbimVector3D axisY = p1;
                    Xbim.Common.Geometry.XbimVector3D axisZ = p2;

                    var finalVector = vectorMove;

                    v1[0] = v1[0] + finalVector.X;
                    v1[1] = v1[1] + finalVector.Y;
                    v1[2] = v1[2] + finalVector.Z;

                    return v1;
                }


            }
            else
            {
                
                return v1;
            }
        }

        public static string UnPascalCase(string text) 
        { 
            if (string.IsNullOrWhiteSpace(text)) 
                return ""; 
            var newText = new StringBuilder(text.Length * 2); 
            newText.Append(text[0]); 
            for (int i = 1; i < text.Length; i++) 
            { 
                var currentUpper = char.IsUpper(text[i]); 
                var prevUpper = char.IsUpper(text[i - 1]); 
                var nextUpper = (text.Length > i + 1) ? char.IsUpper(text[i + 1]) || char.IsWhiteSpace(text[i + 1]) : prevUpper; 
                var spaceExists = char.IsWhiteSpace(text[i - 1]); 
                if (currentUpper && !spaceExists && (!nextUpper || !prevUpper)) newText.Append(' '); 
                newText.Append(text[i]); 
            } 
            return newText.ToString(); 
        }

       

    }
}
