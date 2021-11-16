using System;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;
using BIEM.Schedules;
using BIEM.Tools;
using BIEM.LocationAndClimate;
using BIEM.SimulationParameters;
using BIEM.SurfaceConstructionElements;
using BIEM.ThermalZonesandSurfaces;
using BIEM.OutputReporting;
using System.Globalization;
using BIEM.InternalGains;
using BIEM.ZoneHVACConstrolsandThermostats;
using BIEM.ZoneHVACForcedAirUnits;
using BIEM.ZoneHVACEquipmentConnections;

namespace BIEM
{

    public class IFCReader
    {

     
        public static void Read(string IfcFileName)
        {
            //string IfcFileName = @"D:\CASA TRISQUEL\CURSO BIM\prueba3.ifc";
            using (var model = IfcStore.Open(IfcFileName))
            {
                var app = model.Instances.OfType<IIfcApplication>().ToList().FirstOrDefault();
                var appName = app.ApplicationFullName;

                if (appName != "")
                {

                    //----IFC SITE - Building
                    Building building = new Building();
                    var site = model.Instances.OfType<IIfcSite>().FirstOrDefault();
                    building.Name = site.GlobalId.ToString();
                    var north = model.Instances.OfType<IIfcGeometricRepresentationContext>().FirstOrDefault();
                    var xx = north.TrueNorth.X;
                    var yy = north.TrueNorth.Y;
                    var angulo = Math.Atan2(xx, yy) * (180 / Math.PI);
                    building.NorthAxis = -1 * angulo;
                    building.Terrain = TerrainType.City;
                    building.LoadsConvergenceToleranceValue = 0.04;
                    building.TemperatureConvergenceToleranceValue = 0.4;
                    building.SolarDistribution = SolarDistributionType.FullExterior;
                    building.MaximumNumberofWarmupDays = 25;
                    building.MinimumNumberofWarmupDays = 6;
                    Building.Add(building);

                    //-----IFC SPACE
                    var spaces = model.Instances.OfType<IIfcSpace>().ToList();
                    foreach (var space in spaces)
                    {
                        Zone z = new Zone();
                        z.Name = space.Name;
                        //---zone properties
                        
                        z.DirectionofRelativeNorth = 0;
                        z.XOrigin = 0;
                        z.YOrigin = 0;
                        z.ZOrigin = 0;
                        z.Type = 1;
                        z.Multiplier = 1;

                        Zone.Add(z);
                        
                        

                        /*
                        //------
                        ZoneInfiltrationDesignFlowRate dfr = new ZoneInfiltrationDesignFlowRate();
                        // fZoneArea in L/(seg.m2)
                        double fZoneArea = Convert.ToDouble(Tool.GetSpaceProperty(space, "Calculated Supply Airflow per area"), CultureInfo.InvariantCulture.NumberFormat);
                        dfr.Name = $"{space.GlobalId}-ZInfil";
                        dfr.ZoneorZoneListName = space.GlobalId;
                        dfr.ScheduleName = "Always ON";
                        dfr.DesignFlowRateCalculationMethod = "Flow/Area";
                        dfr.FlowperZoneFloorArea = fZoneArea / 1000;
                        dfr.ConstantTermCoefficient = 0;
                        dfr.TemperatureTermCoefficient = 0;
                        dfr.VelocityTermCoefficient = 0.2237;
                        dfr.VelocitySquaredTermCoefficient = 0;
                        ZoneInfiltrationDesignFlowRate.Add(dfr);
                        */
                        /*
                        //----
                        ZoneVentilationDesignFlowRate vdfr = new ZoneVentilationDesignFlowRate();
                        vdfr.Name = $"{space.GlobalId}-Vent";
                        vdfr.ZoneorZoneListName = space.GlobalId;
                        vdfr.ScheduleName = "Always ON";
                        vdfr.DesignFlowRateCalculationMethod = "Flow/Area";
                        vdfr.FlowRateperZoneFloorArea = fZoneArea / 1000;
                        vdfr.VentilationType = "Natural";
                        vdfr.FanTotalEfficiency = 1;
                        vdfr.ConstantTermCoefficient = 1;
                        vdfr.MinimumIndoorTemperature = 20;
                        vdfr.MaximumIndoorTemperature = 25;
                        vdfr.DeltaTemperature = 0.5;
                        vdfr.MinimumOutdoorTemperature = 10;
                        vdfr.MaximumOutdoorTemperature = 40;
                        vdfr.MaximumWindSpeed = 10;
                        ZoneVentilationDesignFlowRate.Add(vdfr);
                        */
                        
                        //----
                        ZoneControlThermostat t = new ZoneControlThermostat();
                        t.Name = $"Thermostat-{space.Name}";
                        t.ZoneorZoneListName = space.Name;
                        t.ControlTypeScheduleName = "ALWAYS 4";
                        t.Control1Name = $"Office Thermostat DSP";
                        t.Control1ObjectType = "ThermostatSetpoint:DualSetpoint";
                        ZoneControlThermostat.Add(t);
                        
                        
                        //-----
                        ThermostatSetpointDualSetpoint tds = new ThermostatSetpointDualSetpoint();
                        tds.Name = $"Office Thermostat DSP";
                        tds.HeatingSetpointTemperatureScheduleName = $"ALWAYS 0";
                        tds.CoolingSetpointTemperatureScheduleName = $"ALWAYS 50";
                        ThermostatSetpointDualSetpoint.Add(tds);
                        
                        
                        //-----
                        ZoneHVACIdealLoadsAirSystem las = new ZoneHVACIdealLoadsAirSystem();
                        las.Name = $"Purchased Air-{space.Name}";
                        las.ZoneSupplyAirNodeName = $"Supply-{space.Name}";
                        las.MaximumHeatingSupplyAirTemperature = 50;
                        las.MinimumCoolingSupplyAirTemperature = 13;
                        las.MaximumHeatingSupplyAirHumidityRatio = 0.0156;
                        las.MinimumCoolingSupplyAirHumidityRatio = 0.0077;
                        las.HeatingLimit = HeatingLimitEnum.NoLimit;
                        las.CoolingLimit = CoolingLimitEnum.NoLimit;
                        las.DehumidificationControlType = DehumidificationControlTypeEnum.ConstantSupplyHumidityRatio;
                        las.CoolingSensibleHeatRatio = 0.7;
                        las.HumidificationControlType = HumidificationControlTypeEnum.ConstantSupplyHumidityRatio;
                        ZoneHVACIdealLoadsAirSystem.Add(las);
                        
                        
                        //----
                        ZoneHVACEquipmentList el = new ZoneHVACEquipmentList();
                        el.Name = $"Equipment-{space.Name}";
                        el.LoadDistributionScheme = LoadDistributionSchemeEnum.SequentialLoad;
                        el.ZoneEquipment1ObjectType = "ZoneHVAC:IdealLoadsAirSystem";
                        el.ZoneEquipment1Name = $"Purchased Air-{space.Name}";
                        el.ZoneEquipment1CoolingSequence = 1;
                        el.ZoneEquipment1HeatingorNoLoadSequence = 1;
                        ZoneHVACEquipmentList.Add(el);
                        
                        
                        //-----
                        ZoneHVACEquipmentConnections.ZoneHVACEquipmentConnections ec = new ZoneHVACEquipmentConnections.ZoneHVACEquipmentConnections();
                        ec.Name = $"{space.Name}";
                        ec.ConditioningEquipmentListName = $"Equipment-{space.Name}";
                        ec.AirInletNodeorNodeListName = $"Supply-{space.Name}";
                        ec.AirNodeName = $"Node-{space.Name}";
                        ec.ReturnAirNodeorNodeListName = $"Return-{space.Name}";
                        ZoneHVACEquipmentConnections.ZoneHVACEquipmentConnections.Add(ec);

                        /*
                        //----
                        ScheduleCompact ps = new ScheduleCompact();
                        var id = space.Name.ToString();
                        var numPeople = Tool.GetSpaceProperty(space, "Sensible Heat Gain per person");

                        ps.Name = $"Activity-{id}";
                        ps.ScheduleTypeLimitsName = "Any Number";
                        ps.Field1 = "Through: 12/31";
                        ps.Field2 = "For: AllDays";
                        ps.Field3 = "Until: 24:00";
                        ps.Field4 = $"{numPeople}";
                        ScheduleCompact.Add(ps);

                        
                        //-------
                        People p = new People();
                        p.Name = $"People-{id}";
                        p.ZoneorZoneListName = id;
                        p.NumberofpeopleScheduleName = $"Fraction-{id}";
                        p.NumberofPeopleCalculationMethod = NumberofPeopleCalculationMethodType.People;
                        p.NumberofPeople = Convert.ToDouble(numPeople, CultureInfo.InvariantCulture.NumberFormat);
                        p.FractionRadiant = 0.3;
                        p.ActivityLevelScheduleName = $"Activity-{id}";
                        People.Add(p);
                        //InternalGainsDAT.PeopleDAT.Add(p);
                        //InternalGains.People.Collect(p);
                        */

                        //-------
                        Lights l = new Lights();
                        var lightingLevel = Convert.ToDouble(Tool.GetSpaceProperty(space, "Specified Lighting Load"), CultureInfo.InvariantCulture.NumberFormat);
                        l.Name = $"Lights-{space.Name}";
                        l.ZoneorZoneListName = space.Name;
                        l.ScheduleName = $"Office Lighting";
                        l.DesignLevelCalculationMethod = DesignLevelCalculationMethodLightingLevelType.LightingLevel;
                        l.LightingLevel = lightingLevel;
                        l.ReturnAirFraction = 0;
                        l.FractionRadiant = 0.72;
                        l.FractionVisible = 0.18;
                        l.FractionReplaceable = 1;
                        l.EndUseSubcategory = "GeneralLights";
                        Lights.Add(l);
                        
                        /*
                        //------
                        ElectricEquipments ee = new ElectricEquipments();
                        var ELevel = Convert.ToDouble(Tool.GetSpaceProperty(space, "Specified Power Load"), CultureInfo.InvariantCulture.NumberFormat);
                        ee.Name = $"ElectricE-{id}";
                        ee.ZoneorZoneListName = id;
                        ee.ScheduleName = $"Fraction-{id}";
                        ee.DesignLevelCalculationMethod = DesignLevelCalculationMethodElectricEquipmentType.EquipmentLevel;
                        ee.DesignLevel = ELevel;
                        ee.FractionLatent = 0;
                        ee.FractionRadiant = 0.3;
                        ee.FractionLost = 0;
                        ee.EndUseSubcategory = "General";
                        ElectricEquipments.Add(ee);
                        */
                    }


                    //---IFC BUILDING
                    SiteLocation sl = new SiteLocation();
                    var location = model.Instances.OfType<IIfcBuilding>().FirstOrDefault();
                    sl.Name = location.BuildingAddress.Country;
                    var place = model.Instances.OfType<IIfcSite>().FirstOrDefault();
                    sl.Latitude = place.RefLatitude.Value.AsDouble;
                    sl.Longitude = place.RefLongitude.Value.AsDouble;
                    sl.TimeZone = 1;
                    sl.Elevation = place.RefElevation.Value;
                    SiteLocation.Add(sl);


                    //IfcRelSpaceBoundary - BuildingSurface:Detailed
                    //todo: adjust the surfaces to the center line of the wall
                    var relSpaceBoundaries = model.Instances.OfType<IIfcRelSpaceBoundary>().ToList();
                    foreach (IIfcRelSpaceBoundary relSpaceBoundary in relSpaceBoundaries)
                    {
                        //---------------------------------------
                        //---Get the Space Data
                        var space = relSpaceBoundary.RelatingSpace;
                        var theSpace = space as IIfcSpace;
                        double[] spaceLocation = Tool.GetVerticeSpace(theSpace);
                        

                        var localPlacement = theSpace.ObjectPlacement as IIfcLocalPlacement;
                        var relativePlacement = localPlacement.RelativePlacement as IIfcAxis2Placement3D;
                        var spaceXAxis = relativePlacement.P[0];
                        var spaceYAxis = relativePlacement.P[1];
                        var spaceZAxis = relativePlacement.P[2];

                        //-------------------------------------
                        //----Get the Building Element Data
                        var buildingElement = relSpaceBoundary.RelatedBuildingElement as IIfcBuildingElement;
                        
                       

                        //-------Get points of the boundary plane
                        var connectionGeometry = relSpaceBoundary.ConnectionGeometry;
                        var _connectionGeometry = connectionGeometry as IIfcConnectionSurfaceGeometry;
                        var surfaceonRelatingElement = _connectionGeometry.SurfaceOnRelatingElement;
                        var _surfaceonRelatingElement = surfaceonRelatingElement as IIfcCurveBoundedPlane;
                        var outerBoundary = _surfaceonRelatingElement.OuterBoundary;
                        var basisSurface = _surfaceonRelatingElement.BasisSurface;
                        var _basisSurface = basisSurface as IIfcPlane;
                        var position = _basisSurface.Position;
                        var internalOrExternalBoundary = relSpaceBoundary.InternalOrExternalBoundary.ToString();

                        double[] planeLocation = new double[3];
                        planeLocation[0] = position.Location.X;
                        planeLocation[1] = position.Location.Y;
                        planeLocation[2] = position.Location.Z;



                        var planeLocation0 = spaceXAxis * planeLocation[0];
                        var planeLocation1 = spaceYAxis * planeLocation[1];
                        var planeLocation2 = spaceZAxis * planeLocation[2];

                        var planeLocationAdj = planeLocation0 + planeLocation1 + planeLocation2;

                        

                        planeLocation[0] = spaceLocation[0] + planeLocationAdj.X;
                        planeLocation[1] = spaceLocation[1] + planeLocationAdj.Y;
                        planeLocation[2] = spaceLocation[2] + planeLocationAdj.Z;

                        if (buildingElement != null)
                        {
                            Console.WriteLine($"{planeLocation[0]},{planeLocation[1]}, {planeLocation[2]} ---PLane Location");
                            double[] buildingElementLocation = Tool.GetVerticeBuildingElement(buildingElement);
                            Console.WriteLine($"{buildingElementLocation[0]},{buildingElementLocation[1]}, {buildingElementLocation[2]} ---building Location");
                        }

                       
                        
                        

                        double[] planeXAxis = new double[3];
                        planeXAxis[0] = position.P[0].X;
                        planeXAxis[1] = position.P[0].Y;
                        planeXAxis[2] = position.P[0].Z;

                        double[] planeYAxis = new double[3];
                        planeYAxis[0] = position.P[1].X;
                        planeYAxis[1] = position.P[1].Y;
                        planeYAxis[2] = position.P[1].Z;

                        double[] normalToPlane = new double[3];
                        normalToPlane[0] = position.P[2].X;
                        normalToPlane[1] = position.P[2].Y;
                        normalToPlane[2] = position.P[2].Z;

                       

                        double[] refDirectionPlane0 = new double[3];
                        refDirectionPlane0[0] = position.RefDirection.X;
                        refDirectionPlane0[1] = position.RefDirection.Y;
                        refDirectionPlane0[2] = position.RefDirection.Z;

                        double[] planeAxis = new double[3];
                        planeAxis[0] = position.Axis.X;
                        planeAxis[1] = position.Axis.Y;
                        planeAxis[2] = position.Axis.Z;

                        bool AxisX = false;
                        bool AxisY = false;
                        bool AxisZ = false;

                        if (planeAxis[0] != 0)
                        {
                            AxisX = true;
                        }
                        if (planeAxis[1] != 0)
                        {
                            AxisY = true;
                        }
                        if (planeAxis[2] != 0)
                        {
                            AxisZ = true;
                        }

                        int pointsCount = new int();

                        #region points Creation os arrays to contain the coordinates XYZ
                        double[] v2 = new double[3];
                        double[] v3 = new double[3];
                        double[] v4 = new double[3];
                        double[] v5 = new double[3];
                        double[] v6 = new double[3];
                        double[] v7 = new double[3];
                        double[] v8 = new double[3];
                        double[] v9 = new double[3];
                        double[] v10 = new double[3];
                        double[] v11 = new double[3];
                        double[] v12 = new double[3];
                        double[] v13 = new double[3];
                        double[] v14 = new double[3];
                        double[] v15 = new double[3];
                        double[] v16 = new double[3];
                        double[] p2 = new double[3];
                        double[] p3 = new double[3];
                        double[] p4 = new double[3];
                        double[] p5 = new double[3];
                        double[] p6 = new double[3];
                        double[] p7 = new double[3];
                        double[] p8 = new double[3];
                        double[] p9 = new double[3];
                        double[] p10 = new double[3];
                        double[] p11 = new double[3];
                        double[] p12 = new double[3];
                        double[] p13 = new double[3];
                        double[] p14 = new double[3];
                        double[] p15 = new double[3];
                        double[] p16 = new double[3];
                        #endregion points

                        if (outerBoundary is IIfcPolyline)
                        {
                            var _outerBoundary = outerBoundary as IIfcPolyline;
                            var Points = _outerBoundary.Points.ToList();

                            pointsCount = Points.Count;

                            for (int i = 1; i < pointsCount; i++)
                            {
                                int y = i;
                                //If RefDirection is (1,0,0)
                                if (refDirectionPlane0[0] != 0 && refDirectionPlane0[1] == 0 && refDirectionPlane0[2] == 0)
                                {
                                    if (AxisY)
                                    {
                                        if (y == 1)
                                        {
                                            v2[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v2[1] = planeLocation[1];
                                            v2[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 2)
                                        {
                                            v3[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v3[1] = planeLocation[1];
                                            v3[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 3)
                                        {
                                            v4[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v4[1] = planeLocation[1];
                                            v4[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 4)
                                        {
                                            v5[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v5[1] = planeLocation[1];
                                            v5[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 5)
                                        {
                                            v6[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v6[1] = planeLocation[1];
                                            v6[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 6)
                                        {
                                            v7[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v7[1] = planeLocation[1];
                                            v7[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 7)
                                        {
                                            v8[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v8[1] = planeLocation[1];
                                            v8[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 8)
                                        {
                                            v9[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v9[1] = planeLocation[1];
                                            v9[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 9)
                                        {
                                            v10[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v10[1] = planeLocation[1];
                                            v10[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 10)
                                        {
                                            v11[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v11[1] = planeLocation[1];
                                            v11[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 11)
                                        {
                                            v12[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v12[1] = planeLocation[1];
                                            v12[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 12)
                                        {
                                            v13[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v13[1] = planeLocation[1];
                                            v13[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 13)
                                        {
                                            v14[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v14[1] = planeLocation[1];
                                            v14[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 14)
                                        {
                                            v15[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v15[1] = planeLocation[1];
                                            v15[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 15)
                                        {
                                            v16[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v16[1] = planeLocation[1];
                                            v16[2] = planeLocation[2] + Points[i].Y;
                                        }

                                    }
                                    if (AxisZ)
                                    {
                                        if (y == 1)
                                        {
                                            v2[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v2[1] = planeLocation[1] + Points[i].Y;
                                            v2[2] = planeLocation[2];
                                        }
                                        if (y == 2)
                                        {
                                            v3[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v3[1] = planeLocation[1] + Points[i].Y;
                                            v3[2] = planeLocation[2];
                                        }
                                        if (y == 3)
                                        {
                                            v4[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v4[1] = planeLocation[1] + Points[i].Y;
                                            v4[2] = planeLocation[2];
                                        }
                                        if (y == 4)
                                        {
                                            v5[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v5[1] = planeLocation[1] + Points[i].Y;
                                            v5[2] = planeLocation[2];
                                        }
                                        if (y == 5)
                                        {
                                            v6[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v6[1] = planeLocation[1] + Points[i].Y;
                                            v6[2] = planeLocation[2];
                                        }
                                        if (y == 6)
                                        {
                                            v7[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v7[1] = planeLocation[1] + Points[i].Y;
                                            v7[2] = planeLocation[2];
                                        }
                                        if (y == 7)
                                        {
                                            v8[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v8[1] = planeLocation[1] + Points[i].Y;
                                            v8[2] = planeLocation[2];
                                        }
                                        if (y == 8)
                                        {
                                            v9[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v9[1] = planeLocation[1] + Points[i].Y;
                                            v9[2] = planeLocation[2];
                                        }
                                        if (y == 9)
                                        {
                                            v10[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v10[1] = planeLocation[1] + Points[i].Y;
                                            v10[2] = planeLocation[2];
                                        }
                                        if (y == 10)
                                        {
                                            v11[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v11[1] = planeLocation[1] + Points[i].Y;
                                            v11[2] = planeLocation[2];
                                        }
                                        if (y == 11)
                                        {
                                            v12[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v12[1] = planeLocation[1] + Points[i].Y;
                                            v12[2] = planeLocation[2];
                                        }
                                        if (y == 12)
                                        {
                                            v13[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v13[1] = planeLocation[1] + Points[i].Y;
                                            v13[2] = planeLocation[2];
                                        }
                                        if (y == 13)
                                        {
                                            v14[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v14[1] = planeLocation[1] + Points[i].Y;
                                            v14[2] = planeLocation[2];
                                        }
                                        if (y == 14)
                                        {
                                            v15[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v15[1] = planeLocation[1] + Points[i].Y;
                                            v15[2] = planeLocation[2];
                                        }
                                        if (y == 15)
                                        {
                                            v16[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                            v16[1] = planeLocation[1] + Points[i].Y;
                                            v16[2] = planeLocation[2];
                                        }

                                    }

                                }
                                //if RefDirection is (0,1,0)
                                else if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] != 0 && refDirectionPlane0[2] == 0)
                                {
                                    if (AxisX)
                                    {
                                        if (y == 1)
                                        {
                                            v2[0] = planeLocation[0];
                                            v2[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v2[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 2)
                                        {
                                            v3[0] = planeLocation[0];
                                            v3[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v3[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 3)
                                        {
                                            v4[0] = planeLocation[0];
                                            v4[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v4[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 4)
                                        {
                                            v5[0] = planeLocation[0];
                                            v5[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v5[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 5)
                                        {
                                            v6[0] = planeLocation[0];
                                            v6[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v6[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 6)
                                        {
                                            v7[0] = planeLocation[0];
                                            v7[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v7[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 7)
                                        {
                                            v8[0] = planeLocation[0];
                                            v8[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v8[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 8)
                                        {
                                            v9[0] = planeLocation[0];
                                            v9[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v9[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 9)
                                        {
                                            v10[0] = planeLocation[0];
                                            v10[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v10[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 10)
                                        {
                                            v11[0] = planeLocation[0];
                                            v11[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v11[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 11)
                                        {
                                            v12[0] = planeLocation[0];
                                            v12[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v12[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 12)
                                        {
                                            v13[0] = planeLocation[0];
                                            v13[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v13[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 13)
                                        {
                                            v14[0] = planeLocation[0];
                                            v14[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v14[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 14)
                                        {
                                            v15[0] = planeLocation[0];
                                            v15[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v15[2] = planeLocation[2] + Points[i].Y;
                                        }
                                        if (y == 15)
                                        {
                                            v16[0] = planeLocation[0];
                                            v16[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v16[2] = planeLocation[2] + Points[i].Y;
                                        }
                                    }
                                    if (AxisZ)
                                    {
                                        if (y == 1)
                                        {
                                            v2[0] = planeLocation[0] + Points[i].Y;
                                            v2[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v2[2] = planeLocation[2];
                                        }
                                        if (y == 2)
                                        {
                                            v3[0] = planeLocation[0] + Points[i].Y;
                                            v3[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v3[2] = planeLocation[2];
                                        }
                                        if (y == 3)
                                        {
                                            v4[0] = planeLocation[0] + Points[i].Y;
                                            v4[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v4[2] = planeLocation[2];
                                        }
                                        if (y == 4)
                                        {
                                            v5[0] = planeLocation[0] + Points[i].Y;
                                            v5[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v5[2] = planeLocation[2];
                                        }
                                        if (y == 5)
                                        {
                                            v6[0] = planeLocation[0] + Points[i].Y;
                                            v6[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v6[2] = planeLocation[2];
                                        }
                                        if (y == 6)
                                        {
                                            v7[0] = planeLocation[0] + Points[i].Y;
                                            v7[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v7[2] = planeLocation[2];
                                        }
                                        if (y == 7)
                                        {
                                            v8[0] = planeLocation[0] + Points[i].Y;
                                            v8[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v8[2] = planeLocation[2];
                                        }
                                        if (y == 8)
                                        {
                                            v9[0] = planeLocation[0] + Points[i].Y;
                                            v9[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v9[2] = planeLocation[2];
                                        }
                                        if (y == 9)
                                        {
                                            v10[0] = planeLocation[0] + Points[i].Y;
                                            v10[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v10[2] = planeLocation[2];
                                        }
                                        if (y == 10)
                                        {
                                            v11[0] = planeLocation[0] + Points[i].Y;
                                            v11[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v11[2] = planeLocation[2];
                                        }
                                        if (y == 11)
                                        {
                                            v12[0] = planeLocation[0] + Points[i].Y;
                                            v12[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v12[2] = planeLocation[2];
                                        }
                                        if (y == 12)
                                        {
                                            v13[0] = planeLocation[0] + Points[i].Y;
                                            v13[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v13[2] = planeLocation[2];
                                        }
                                        if (y == 13)
                                        {
                                            v14[0] = planeLocation[0] + Points[i].Y;
                                            v14[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v14[2] = planeLocation[2];
                                        }
                                        if (y == 14)
                                        {
                                            v15[0] = planeLocation[0] + Points[i].Y;
                                            v15[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v15[2] = planeLocation[2];
                                        }
                                        if (y == 15)
                                        {
                                            v16[0] = planeLocation[0] + Points[i].Y;
                                            v16[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                            v16[2] = planeLocation[2];
                                        }
                                    }
                                }
                                //if RefDirection is (0,0,1)
                                else if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] == 0 && refDirectionPlane0[2] != 0)
                                {
                                    if (AxisX)
                                    {
                                        if (y == 1)
                                        {
                                            v2[0] = planeLocation[0];
                                            v2[1] = planeLocation[1] + Points[i].Y;
                                            v2[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 2)
                                        {
                                            v3[0] = planeLocation[0];
                                            v3[1] = planeLocation[1] + Points[i].Y;
                                            v3[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 3)
                                        {
                                            v4[0] = planeLocation[0];
                                            v4[1] = planeLocation[1] + Points[i].Y;
                                            v4[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 4)
                                        {
                                            v5[0] = planeLocation[0];
                                            v5[1] = planeLocation[1] + Points[i].Y;
                                            v5[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 5)
                                        {
                                            v6[0] = planeLocation[0];
                                            v6[1] = planeLocation[1] + Points[i].Y;
                                            v6[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 6)
                                        {
                                            v7[0] = planeLocation[0];
                                            v7[1] = planeLocation[1] + Points[i].Y;
                                            v7[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 7)
                                        {
                                            v8[0] = planeLocation[0];
                                            v8[1] = planeLocation[1] + Points[i].Y;
                                            v8[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 8)
                                        {
                                            v9[0] = planeLocation[0];
                                            v9[1] = planeLocation[1] + Points[i].Y;
                                            v9[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 9)
                                        {
                                            v10[0] = planeLocation[0];
                                            v10[1] = planeLocation[1] + Points[i].Y;
                                            v10[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 10)
                                        {
                                            v11[0] = planeLocation[0];
                                            v11[1] = planeLocation[1] + Points[i].Y;
                                            v11[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 11)
                                        {
                                            v12[0] = planeLocation[0];
                                            v12[1] = planeLocation[1] + Points[i].Y;
                                            v12[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 12)
                                        {
                                            v13[0] = planeLocation[0];
                                            v13[1] = planeLocation[1] + Points[i].Y;
                                            v13[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 13)
                                        {
                                            v14[0] = planeLocation[0];
                                            v14[1] = planeLocation[1] + Points[i].Y;
                                            v14[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 14)
                                        {
                                            v15[0] = planeLocation[0];
                                            v15[1] = planeLocation[1] + Points[i].Y;
                                            v15[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 15)
                                        {
                                            v16[0] = planeLocation[0];
                                            v16[1] = planeLocation[1] + Points[i].Y;
                                            v16[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                    }
                                    if (AxisY)
                                    {
                                        if (y == 1)
                                        {
                                            v2[0] = planeLocation[0] + Points[i].Y;
                                            v2[1] = planeLocation[1];
                                            v2[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 2)
                                        {
                                            v3[0] = planeLocation[0] + Points[i].Y;
                                            v3[1] = planeLocation[1];
                                            v3[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 3)
                                        {
                                            v4[0] = planeLocation[0] + Points[i].Y;
                                            v4[1] = planeLocation[1];
                                            v4[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 4)
                                        {
                                            v5[0] = planeLocation[0] + Points[i].Y;
                                            v5[1] = planeLocation[1];
                                            v5[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 5)
                                        {
                                            v6[0] = planeLocation[0] + Points[i].Y;
                                            v6[1] = planeLocation[1];
                                            v6[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 6)
                                        {
                                            v7[0] = planeLocation[0] + Points[i].Y;
                                            v7[1] = planeLocation[1];
                                            v7[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 7)
                                        {
                                            v8[0] = planeLocation[0] + Points[i].Y;
                                            v8[1] = planeLocation[1];
                                            v8[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 8)
                                        {
                                            v9[0] = planeLocation[0] + Points[i].Y;
                                            v9[1] = planeLocation[1];
                                            v9[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 9)
                                        {
                                            v10[0] = planeLocation[0] + Points[i].Y;
                                            v10[1] = planeLocation[1];
                                            v10[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 10)
                                        {
                                            v11[0] = planeLocation[0] + Points[i].Y;
                                            v11[1] = planeLocation[1];
                                            v11[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 11)
                                        {
                                            v12[0] = planeLocation[0] + Points[i].Y;
                                            v12[1] = planeLocation[1];
                                            v12[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 12)
                                        {
                                            v13[0] = planeLocation[0] + Points[i].Y;
                                            v13[1] = planeLocation[1];
                                            v13[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 13)
                                        {
                                            v14[0] = planeLocation[0] + Points[i].Y;
                                            v14[1] = planeLocation[1];
                                            v14[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                        }
                                        if (y == 14)
                                        {
                                            v15[0] = planeLocation[0] + Points[i].Y;
                                            v15[1] = planeLocation[1];
                                            v15[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                        if (y == 15)
                                        {
                                            v16[0] = planeLocation[0] + Points[i].Y;
                                            v16[1] = planeLocation[1];
                                            v16[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                        }
                                    }
                                }


                            }

                            // Define points in counterclock wise
                           

                            p2[0] = v2[0];
                            p2[1] = v2[1];
                            p2[2] = v2[2];

                            p3[0] = v3[0];
                            p3[1] = v3[1];
                            p3[2] = v3[2];

                            p4[0] = v4[0];
                            p4[1] = v4[1];
                            p4[2] = v4[2];

                            p5[0] = v5[0];
                            p5[1] = v5[1];
                            p5[2] = v5[2];

                            p6[0] = v6[0];
                            p6[1] = v6[1];
                            p6[2] = v6[2];

                            p7[0] = v7[0];
                            p7[1] = v7[1];
                            p7[2] = v7[2];

                            p8[0] = v8[0];
                            p8[1] = v8[1];
                            p8[2] = v8[2];

                            p9[0] = v9[0];
                            p9[1] = v9[1];
                            p9[2] = v9[2];

                            p10[0] = v10[0];
                            p10[1] = v10[1];
                            p10[2] = v10[2];

                            p11[0] = v11[0];
                            p11[1] = v11[1];
                            p11[2] = v11[2];

                            p12[0] = v12[0];
                            p12[1] = v12[1];
                            p12[2] = v12[2];

                            p13[0] = v13[0];
                            p13[1] = v13[1];
                            p13[2] = v13[2];

                            p14[0] = v14[0];
                            p14[1] = v14[1];
                            p14[2] = v14[2];

                            p15[0] = v15[0];
                            p15[1] = v15[1];
                            p15[2] = v15[2];

                            p16[0] = v16[0];
                            p16[1] = v16[1];
                            p16[2] = v16[2];


                            //If RefDirection is (0,1,0)
                            if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] != 0 && refDirectionPlane0[2] == 0)
                            {
                                if (AxisZ)
                                {
                                    if (pointsCount - 1 == 4)
                                    {
                                        p2[0] = v4[0];
                                        p2[1] = v4[1];
                                        p2[2] = v4[2];

                                        p3[0] = v3[0];
                                        p3[1] = v3[1];
                                        p3[2] = v3[2];

                                        p4[0] = v2[0];
                                        p4[1] = v2[1];
                                        p4[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 5)
                                    {
                                        p2[0] = v5[0];
                                        p2[1] = v5[1];
                                        p2[2] = v5[2];

                                        p3[0] = v4[0];
                                        p3[1] = v4[1];
                                        p3[2] = v4[2];

                                        p4[0] = v3[0];
                                        p4[1] = v3[1];
                                        p4[2] = v3[2];

                                        p5[0] = v2[0];
                                        p5[1] = v2[1];
                                        p5[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 6)
                                    {
                                        p2[0] = v6[0];
                                        p2[1] = v6[1];
                                        p2[2] = v6[2];

                                        p3[0] = v5[0];
                                        p3[1] = v5[1];
                                        p3[2] = v5[2];

                                        p4[0] = v4[0];
                                        p4[1] = v4[1];
                                        p4[2] = v4[2];

                                        p5[0] = v3[0];
                                        p5[1] = v3[1];
                                        p5[2] = v3[2];

                                        p6[0] = v2[0];
                                        p6[1] = v2[1];
                                        p6[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 7)
                                    {
                                        p2[0] = v7[0];
                                        p2[1] = v7[1];
                                        p2[2] = v7[2];

                                        p3[0] = v6[0];
                                        p3[1] = v6[1];
                                        p3[2] = v6[2];

                                        p4[0] = v5[0];
                                        p4[1] = v5[1];
                                        p4[2] = v5[2];

                                        p5[0] = v4[0];
                                        p5[1] = v4[1];
                                        p5[2] = v4[2];

                                        p6[0] = v3[0];
                                        p6[1] = v3[1];
                                        p6[2] = v3[2];

                                        p7[0] = v2[0];
                                        p7[1] = v2[1];
                                        p7[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 8)
                                    {
                                        p2[0] = v8[0];
                                        p2[1] = v8[1];
                                        p2[2] = v8[2];

                                        p3[0] = v7[0];
                                        p3[1] = v7[1];
                                        p3[2] = v7[2];

                                        p4[0] = v6[0];
                                        p4[1] = v6[1];
                                        p4[2] = v6[2];

                                        p5[0] = v5[0];
                                        p5[1] = v5[1];
                                        p5[2] = v5[2];

                                        p6[0] = v4[0];
                                        p6[1] = v4[1];
                                        p6[2] = v4[2];

                                        p7[0] = v3[0];
                                        p7[1] = v3[1];
                                        p7[2] = v3[2];

                                        p8[0] = v2[0];
                                        p8[1] = v2[1];
                                        p8[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 9)
                                    {
                                        p2[0] = v9[0];
                                        p2[1] = v9[1];
                                        p2[2] = v9[2];

                                        p3[0] = v8[0];
                                        p3[1] = v8[1];
                                        p3[2] = v8[2];

                                        p4[0] = v7[0];
                                        p4[1] = v7[1];
                                        p4[2] = v7[2];

                                        p5[0] = v6[0];
                                        p5[1] = v6[1];
                                        p5[2] = v6[2];

                                        p6[0] = v5[0];
                                        p6[1] = v5[1];
                                        p6[2] = v5[2];

                                        p7[0] = v4[0];
                                        p7[1] = v4[1];
                                        p7[2] = v4[2];

                                        p8[0] = v3[0];
                                        p8[1] = v3[1];
                                        p8[2] = v3[2];

                                        p9[0] = v2[0];
                                        p9[1] = v2[1];
                                        p9[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 10)
                                    {
                                        p2[0] = v10[0];
                                        p2[1] = v10[1];
                                        p2[2] = v10[2];

                                        p3[0] = v9[0];
                                        p3[1] = v9[1];
                                        p3[2] = v9[2];

                                        p4[0] = v8[0];
                                        p4[1] = v8[1];
                                        p4[2] = v8[2];

                                        p5[0] = v7[0];
                                        p5[1] = v7[1];
                                        p5[2] = v7[2];

                                        p6[0] = v6[0];
                                        p6[1] = v6[1];
                                        p6[2] = v6[2];

                                        p7[0] = v5[0];
                                        p7[1] = v5[1];
                                        p7[2] = v5[2];

                                        p8[0] = v4[0];
                                        p8[1] = v4[1];
                                        p8[2] = v4[2];

                                        p9[0] = v3[0];
                                        p9[1] = v3[1];
                                        p9[2] = v3[2];

                                        p10[0] = v2[0];
                                        p10[1] = v2[1];
                                        p10[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 11)
                                    {
                                        p2[0] = v11[0];
                                        p2[1] = v11[1];
                                        p2[2] = v11[2];

                                        p3[0] = v10[0];
                                        p3[1] = v10[1];
                                        p3[2] = v10[2];

                                        p4[0] = v9[0];
                                        p4[1] = v9[1];
                                        p4[2] = v9[2];

                                        p5[0] = v8[0];
                                        p5[1] = v8[1];
                                        p5[2] = v8[2];

                                        p6[0] = v7[0];
                                        p6[1] = v7[1];
                                        p6[2] = v7[2];

                                        p7[0] = v6[0];
                                        p7[1] = v6[1];
                                        p7[2] = v6[2];

                                        p8[0] = v5[0];
                                        p8[1] = v5[1];
                                        p8[2] = v5[2];

                                        p9[0] = v4[0];
                                        p9[1] = v4[1];
                                        p9[2] = v4[2];

                                        p10[0] = v3[0];
                                        p10[1] = v3[1];
                                        p10[2] = v3[2];

                                        p11[0] = v2[0];
                                        p11[1] = v2[1];
                                        p11[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 12)
                                    {
                                        p2[0] = v12[0];
                                        p2[1] = v12[1];
                                        p2[2] = v12[2];

                                        p3[0] = v11[0];
                                        p3[1] = v11[1];
                                        p3[2] = v11[2];

                                        p4[0] = v10[0];
                                        p4[1] = v10[1];
                                        p4[2] = v10[2];

                                        p5[0] = v9[0];
                                        p5[1] = v9[1];
                                        p5[2] = v9[2];

                                        p6[0] = v8[0];
                                        p6[1] = v8[1];
                                        p6[2] = v8[2];

                                        p7[0] = v7[0];
                                        p7[1] = v7[1];
                                        p7[2] = v7[2];

                                        p8[0] = v6[0];
                                        p8[1] = v6[1];
                                        p8[2] = v6[2];

                                        p9[0] = v5[0];
                                        p9[1] = v5[1];
                                        p9[2] = v5[2];

                                        p10[0] = v4[0];
                                        p10[1] = v4[1];
                                        p10[2] = v4[2];

                                        p11[0] = v3[0];
                                        p11[1] = v3[1];
                                        p11[2] = v3[2];

                                        p12[0] = v2[0];
                                        p12[1] = v2[1];
                                        p12[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 13)
                                    {
                                        p2[0] = v13[0];
                                        p2[1] = v13[1];
                                        p2[2] = v13[2];

                                        p3[0] = v12[0];
                                        p3[1] = v12[1];
                                        p3[2] = v12[2];

                                        p4[0] = v11[0];
                                        p4[1] = v11[1];
                                        p4[2] = v11[2];

                                        p5[0] = v10[0];
                                        p5[1] = v10[1];
                                        p5[2] = v10[2];

                                        p6[0] = v9[0];
                                        p6[1] = v9[1];
                                        p6[2] = v9[2];

                                        p7[0] = v8[0];
                                        p7[1] = v8[1];
                                        p7[2] = v8[2];

                                        p8[0] = v7[0];
                                        p8[1] = v7[1];
                                        p8[2] = v7[2];

                                        p9[0] = v6[0];
                                        p9[1] = v6[1];
                                        p9[2] = v6[2];

                                        p10[0] = v5[0];
                                        p10[1] = v5[1];
                                        p10[2] = v5[2];

                                        p11[0] = v4[0];
                                        p11[1] = v4[1];
                                        p11[2] = v4[2];

                                        p12[0] = v3[0];
                                        p12[1] = v3[1];
                                        p12[2] = v3[2];

                                        p13[0] = v2[0];
                                        p13[1] = v2[1];
                                        p13[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 14)
                                    {
                                        p2[0] = v14[0];
                                        p2[1] = v14[1];
                                        p2[2] = v14[2];

                                        p3[0] = v13[0];
                                        p3[1] = v13[1];
                                        p3[2] = v13[2];

                                        p4[0] = v12[0];
                                        p4[1] = v12[1];
                                        p4[2] = v12[2];

                                        p5[0] = v11[0];
                                        p5[1] = v11[1];
                                        p5[2] = v11[2];

                                        p6[0] = v10[0];
                                        p6[1] = v10[1];
                                        p6[2] = v10[2];

                                        p7[0] = v9[0];
                                        p7[1] = v9[1];
                                        p7[2] = v9[2];

                                        p8[0] = v8[0];
                                        p8[1] = v8[1];
                                        p8[2] = v8[2];

                                        p9[0] = v7[0];
                                        p9[1] = v7[1];
                                        p9[2] = v7[2];

                                        p10[0] = v6[0];
                                        p10[1] = v6[1];
                                        p10[2] = v6[2];

                                        p11[0] = v5[0];
                                        p11[1] = v5[1];
                                        p11[2] = v5[2];

                                        p12[0] = v4[0];
                                        p12[1] = v4[1];
                                        p12[2] = v4[2];

                                        p13[0] = v3[0];
                                        p13[1] = v3[1];
                                        p13[2] = v3[2];

                                        p14[0] = v2[0];
                                        p14[1] = v2[1];
                                        p14[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 15)
                                    {
                                        p2[0] = v15[0];
                                        p2[1] = v15[1];
                                        p2[2] = v15[2];

                                        p3[0] = v14[0];
                                        p3[1] = v14[1];
                                        p3[2] = v14[2];

                                        p4[0] = v13[0];
                                        p4[1] = v13[1];
                                        p4[2] = v13[2];

                                        p5[0] = v12[0];
                                        p5[1] = v12[1];
                                        p5[2] = v12[2];

                                        p6[0] = v11[0];
                                        p6[1] = v11[1];
                                        p6[2] = v11[2];

                                        p7[0] = v10[0];
                                        p7[1] = v10[1];
                                        p7[2] = v10[2];

                                        p8[0] = v9[0];
                                        p8[1] = v9[1];
                                        p8[2] = v9[2];

                                        p9[0] = v8[0];
                                        p9[1] = v8[1];
                                        p9[2] = v8[2];

                                        p10[0] = v7[0];
                                        p10[1] = v7[1];
                                        p10[2] = v7[2];

                                        p11[0] = v6[0];
                                        p11[1] = v6[1];
                                        p11[2] = v6[2];

                                        p12[0] = v5[0];
                                        p12[1] = v5[1];
                                        p12[2] = v5[2];

                                        p13[0] = v4[0];
                                        p13[1] = v4[1];
                                        p13[2] = v4[2];

                                        p14[0] = v3[0];
                                        p14[1] = v3[1];
                                        p14[2] = v3[2];

                                        p15[0] = v2[0];
                                        p15[1] = v2[1];
                                        p15[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 16)
                                    {
                                        p2[0] = v16[0];
                                        p2[1] = v16[1];
                                        p2[2] = v16[2];

                                        p3[0] = v15[0];
                                        p3[1] = v15[1];
                                        p3[2] = v15[2];

                                        p4[0] = v14[0];
                                        p4[1] = v14[1];
                                        p4[2] = v14[2];

                                        p5[0] = v13[0];
                                        p5[1] = v13[1];
                                        p5[2] = v13[2];

                                        p6[0] = v12[0];
                                        p6[1] = v12[1];
                                        p6[2] = v12[2];

                                        p7[0] = v11[0];
                                        p7[1] = v11[1];
                                        p7[2] = v11[2];

                                        p8[0] = v10[0];
                                        p8[1] = v10[1];
                                        p8[2] = v10[2];

                                        p9[0] = v9[0];
                                        p9[1] = v9[1];
                                        p9[2] = v9[2];

                                        p10[0] = v8[0];
                                        p10[1] = v8[1];
                                        p10[2] = v8[2];

                                        p11[0] = v7[0];
                                        p11[1] = v7[1];
                                        p11[2] = v7[2];

                                        p12[0] = v6[0];
                                        p12[1] = v6[1];
                                        p12[2] = v6[2];

                                        p13[0] = v5[0];
                                        p13[1] = v5[1];
                                        p13[2] = v5[2];

                                        p14[0] = v4[0];
                                        p14[1] = v4[1];
                                        p14[2] = v4[2];

                                        p15[0] = v3[0];
                                        p15[1] = v3[1];
                                        p15[2] = v3[2];

                                        p16[0] = v2[0];
                                        p16[1] = v2[1];
                                        p16[2] = v2[2];


                                    }
                                }

                            }
                            //if RefDirection is (0, 0, 1)
                            else if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] == 0 && refDirectionPlane0[2] != 0)
                            {
                                if (AxisX || AxisY)
                                {
                                    if (pointsCount - 1 == 4)
                                    {
                                        p2[0] = v4[0];
                                        p2[1] = v4[1];
                                        p2[2] = v4[2];

                                        p3[0] = v3[0];
                                        p3[1] = v3[1];
                                        p3[2] = v3[2];

                                        p4[0] = v2[0];
                                        p4[1] = v2[1];
                                        p4[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 5)
                                    {
                                        p2[0] = v5[0];
                                        p2[1] = v5[1];
                                        p2[2] = v5[2];

                                        p3[0] = v4[0];
                                        p3[1] = v4[1];
                                        p3[2] = v4[2];

                                        p4[0] = v3[0];
                                        p4[1] = v3[1];
                                        p4[2] = v3[2];

                                        p5[0] = v2[0];
                                        p5[1] = v2[1];
                                        p5[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 6)
                                    {
                                        p2[0] = v6[0];
                                        p2[1] = v6[1];
                                        p2[2] = v6[2];

                                        p3[0] = v5[0];
                                        p3[1] = v5[1];
                                        p3[2] = v5[2];

                                        p4[0] = v4[0];
                                        p4[1] = v4[1];
                                        p4[2] = v4[2];

                                        p5[0] = v3[0];
                                        p5[1] = v3[1];
                                        p5[2] = v3[2];

                                        p6[0] = v2[0];
                                        p6[1] = v2[1];
                                        p6[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 7)
                                    {
                                        p2[0] = v7[0];
                                        p2[1] = v7[1];
                                        p2[2] = v7[2];

                                        p3[0] = v6[0];
                                        p3[1] = v6[1];
                                        p3[2] = v6[2];

                                        p4[0] = v5[0];
                                        p4[1] = v5[1];
                                        p4[2] = v5[2];

                                        p5[0] = v4[0];
                                        p5[1] = v4[1];
                                        p5[2] = v4[2];

                                        p6[0] = v3[0];
                                        p6[1] = v3[1];
                                        p6[2] = v3[2];

                                        p7[0] = v2[0];
                                        p7[1] = v2[1];
                                        p7[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 8)
                                    {
                                        p2[0] = v8[0];
                                        p2[1] = v8[1];
                                        p2[2] = v8[2];

                                        p3[0] = v7[0];
                                        p3[1] = v7[1];
                                        p3[2] = v7[2];

                                        p4[0] = v6[0];
                                        p4[1] = v6[1];
                                        p4[2] = v6[2];

                                        p5[0] = v5[0];
                                        p5[1] = v5[1];
                                        p5[2] = v5[2];

                                        p6[0] = v4[0];
                                        p6[1] = v4[1];
                                        p6[2] = v4[2];

                                        p7[0] = v3[0];
                                        p7[1] = v3[1];
                                        p7[2] = v3[2];

                                        p8[0] = v2[0];
                                        p8[1] = v2[1];
                                        p8[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 9)
                                    {
                                        p2[0] = v9[0];
                                        p2[1] = v9[1];
                                        p2[2] = v9[2];

                                        p3[0] = v8[0];
                                        p3[1] = v8[1];
                                        p3[2] = v8[2];

                                        p4[0] = v7[0];
                                        p4[1] = v7[1];
                                        p4[2] = v7[2];

                                        p5[0] = v6[0];
                                        p5[1] = v6[1];
                                        p5[2] = v6[2];

                                        p6[0] = v5[0];
                                        p6[1] = v5[1];
                                        p6[2] = v5[2];

                                        p7[0] = v4[0];
                                        p7[1] = v4[1];
                                        p7[2] = v4[2];

                                        p8[0] = v3[0];
                                        p8[1] = v3[1];
                                        p8[2] = v3[2];

                                        p9[0] = v2[0];
                                        p9[1] = v2[1];
                                        p9[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 10)
                                    {
                                        p2[0] = v10[0];
                                        p2[1] = v10[1];
                                        p2[2] = v10[2];

                                        p3[0] = v9[0];
                                        p3[1] = v9[1];
                                        p3[2] = v9[2];

                                        p4[0] = v8[0];
                                        p4[1] = v8[1];
                                        p4[2] = v8[2];

                                        p5[0] = v7[0];
                                        p5[1] = v7[1];
                                        p5[2] = v7[2];

                                        p6[0] = v6[0];
                                        p6[1] = v6[1];
                                        p6[2] = v6[2];

                                        p7[0] = v5[0];
                                        p7[1] = v5[1];
                                        p7[2] = v5[2];

                                        p8[0] = v4[0];
                                        p8[1] = v4[1];
                                        p8[2] = v4[2];

                                        p9[0] = v3[0];
                                        p9[1] = v3[1];
                                        p9[2] = v3[2];

                                        p10[0] = v2[0];
                                        p10[1] = v2[1];
                                        p10[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 11)
                                    {
                                        p2[0] = v11[0];
                                        p2[1] = v11[1];
                                        p2[2] = v11[2];

                                        p3[0] = v10[0];
                                        p3[1] = v10[1];
                                        p3[2] = v10[2];

                                        p4[0] = v9[0];
                                        p4[1] = v9[1];
                                        p4[2] = v9[2];

                                        p5[0] = v8[0];
                                        p5[1] = v8[1];
                                        p5[2] = v8[2];

                                        p6[0] = v7[0];
                                        p6[1] = v7[1];
                                        p6[2] = v7[2];

                                        p7[0] = v6[0];
                                        p7[1] = v6[1];
                                        p7[2] = v6[2];

                                        p8[0] = v5[0];
                                        p8[1] = v5[1];
                                        p8[2] = v5[2];

                                        p9[0] = v4[0];
                                        p9[1] = v4[1];
                                        p9[2] = v4[2];

                                        p10[0] = v3[0];
                                        p10[1] = v3[1];
                                        p10[2] = v3[2];

                                        p11[0] = v2[0];
                                        p11[1] = v2[1];
                                        p11[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 12)
                                    {
                                        p2[0] = v12[0];
                                        p2[1] = v12[1];
                                        p2[2] = v12[2];

                                        p3[0] = v11[0];
                                        p3[1] = v11[1];
                                        p3[2] = v11[2];

                                        p4[0] = v10[0];
                                        p4[1] = v10[1];
                                        p4[2] = v10[2];

                                        p5[0] = v9[0];
                                        p5[1] = v9[1];
                                        p5[2] = v9[2];

                                        p6[0] = v8[0];
                                        p6[1] = v8[1];
                                        p6[2] = v8[2];

                                        p7[0] = v7[0];
                                        p7[1] = v7[1];
                                        p7[2] = v7[2];

                                        p8[0] = v6[0];
                                        p8[1] = v6[1];
                                        p8[2] = v6[2];

                                        p9[0] = v5[0];
                                        p9[1] = v5[1];
                                        p9[2] = v5[2];

                                        p10[0] = v4[0];
                                        p10[1] = v4[1];
                                        p10[2] = v4[2];

                                        p11[0] = v3[0];
                                        p11[1] = v3[1];
                                        p11[2] = v3[2];

                                        p12[0] = v2[0];
                                        p12[1] = v2[1];
                                        p12[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 13)
                                    {
                                        p2[0] = v13[0];
                                        p2[1] = v13[1];
                                        p2[2] = v13[2];

                                        p3[0] = v12[0];
                                        p3[1] = v12[1];
                                        p3[2] = v12[2];

                                        p4[0] = v11[0];
                                        p4[1] = v11[1];
                                        p4[2] = v11[2];

                                        p5[0] = v10[0];
                                        p5[1] = v10[1];
                                        p5[2] = v10[2];

                                        p6[0] = v9[0];
                                        p6[1] = v9[1];
                                        p6[2] = v9[2];

                                        p7[0] = v8[0];
                                        p7[1] = v8[1];
                                        p7[2] = v8[2];

                                        p8[0] = v7[0];
                                        p8[1] = v7[1];
                                        p8[2] = v7[2];

                                        p9[0] = v6[0];
                                        p9[1] = v6[1];
                                        p9[2] = v6[2];

                                        p10[0] = v5[0];
                                        p10[1] = v5[1];
                                        p10[2] = v5[2];

                                        p11[0] = v4[0];
                                        p11[1] = v4[1];
                                        p11[2] = v4[2];

                                        p12[0] = v3[0];
                                        p12[1] = v3[1];
                                        p12[2] = v3[2];

                                        p13[0] = v2[0];
                                        p13[1] = v2[1];
                                        p13[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 14)
                                    {
                                        p2[0] = v14[0];
                                        p2[1] = v14[1];
                                        p2[2] = v14[2];

                                        p3[0] = v13[0];
                                        p3[1] = v13[1];
                                        p3[2] = v13[2];

                                        p4[0] = v12[0];
                                        p4[1] = v12[1];
                                        p4[2] = v12[2];

                                        p5[0] = v11[0];
                                        p5[1] = v11[1];
                                        p5[2] = v11[2];

                                        p6[0] = v10[0];
                                        p6[1] = v10[1];
                                        p6[2] = v10[2];

                                        p7[0] = v9[0];
                                        p7[1] = v9[1];
                                        p7[2] = v9[2];

                                        p8[0] = v8[0];
                                        p8[1] = v8[1];
                                        p8[2] = v8[2];

                                        p9[0] = v7[0];
                                        p9[1] = v7[1];
                                        p9[2] = v7[2];

                                        p10[0] = v6[0];
                                        p10[1] = v6[1];
                                        p10[2] = v6[2];

                                        p11[0] = v5[0];
                                        p11[1] = v5[1];
                                        p11[2] = v5[2];

                                        p12[0] = v4[0];
                                        p12[1] = v4[1];
                                        p12[2] = v4[2];

                                        p13[0] = v3[0];
                                        p13[1] = v3[1];
                                        p13[2] = v3[2];

                                        p14[0] = v2[0];
                                        p14[1] = v2[1];
                                        p14[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 15)
                                    {
                                        p2[0] = v15[0];
                                        p2[1] = v15[1];
                                        p2[2] = v15[2];

                                        p3[0] = v14[0];
                                        p3[1] = v14[1];
                                        p3[2] = v14[2];

                                        p4[0] = v13[0];
                                        p4[1] = v13[1];
                                        p4[2] = v13[2];

                                        p5[0] = v12[0];
                                        p5[1] = v12[1];
                                        p5[2] = v12[2];

                                        p6[0] = v11[0];
                                        p6[1] = v11[1];
                                        p6[2] = v11[2];

                                        p7[0] = v10[0];
                                        p7[1] = v10[1];
                                        p7[2] = v10[2];

                                        p8[0] = v9[0];
                                        p8[1] = v9[1];
                                        p8[2] = v9[2];

                                        p9[0] = v8[0];
                                        p9[1] = v8[1];
                                        p9[2] = v8[2];

                                        p10[0] = v7[0];
                                        p10[1] = v7[1];
                                        p10[2] = v7[2];

                                        p11[0] = v6[0];
                                        p11[1] = v6[1];
                                        p11[2] = v6[2];

                                        p12[0] = v5[0];
                                        p12[1] = v5[1];
                                        p12[2] = v5[2];

                                        p13[0] = v4[0];
                                        p13[1] = v4[1];
                                        p13[2] = v4[2];

                                        p14[0] = v3[0];
                                        p14[1] = v3[1];
                                        p14[2] = v3[2];

                                        p15[0] = v2[0];
                                        p15[1] = v2[1];
                                        p15[2] = v2[2];
                                    }
                                    if (pointsCount - 1 == 16)
                                    {
                                        p2[0] = v16[0];
                                        p2[1] = v16[1];
                                        p2[2] = v16[2];

                                        p3[0] = v15[0];
                                        p3[1] = v15[1];
                                        p3[2] = v15[2];

                                        p4[0] = v14[0];
                                        p4[1] = v14[1];
                                        p4[2] = v14[2];

                                        p5[0] = v13[0];
                                        p5[1] = v13[1];
                                        p5[2] = v13[2];

                                        p6[0] = v12[0];
                                        p6[1] = v12[1];
                                        p6[2] = v12[2];

                                        p7[0] = v11[0];
                                        p7[1] = v11[1];
                                        p7[2] = v11[2];

                                        p8[0] = v10[0];
                                        p8[1] = v10[1];
                                        p8[2] = v10[2];

                                        p9[0] = v9[0];
                                        p9[1] = v9[1];
                                        p9[2] = v9[2];

                                        p10[0] = v8[0];
                                        p10[1] = v8[1];
                                        p10[2] = v8[2];

                                        p11[0] = v7[0];
                                        p11[1] = v7[1];
                                        p11[2] = v7[2];

                                        p12[0] = v6[0];
                                        p12[1] = v6[1];
                                        p12[2] = v6[2];

                                        p13[0] = v5[0];
                                        p13[1] = v5[1];
                                        p13[2] = v5[2];

                                        p14[0] = v4[0];
                                        p14[1] = v4[1];
                                        p14[2] = v4[2];

                                        p15[0] = v3[0];
                                        p15[1] = v3[1];
                                        p15[2] = v3[2];

                                        p16[0] = v2[0];
                                        p16[1] = v2[1];
                                        p16[2] = v2[2];


                                    }
                                }
                            }

                        }
                        else if (outerBoundary is IIfcCompositeCurve)
                        {
                            var _outerBoundary = outerBoundary as IIfcCompositeCurve;
                            var parentCurve = _outerBoundary.Segments.FirstOrDefault().ParentCurve as IIfcPolyline;
                            var Points = parentCurve.Points.ToList();

                            pointsCount = Points.Count;

                            for (int i = 0; i < pointsCount; i++)
                            {
                                int y = i;

                                double[] moveAxisXPlane = new double[3];
                                double[] moveAxisYPlane = new double[3];
                                

                                moveAxisXPlane[0] = Points[i].X * planeXAxis[0];
                                moveAxisXPlane[1] = Points[i].X * planeXAxis[1];
                                moveAxisXPlane[2] = Points[i].X * planeXAxis[2];
                                

                                moveAxisYPlane[0] = Points[i].Y * planeYAxis[0];
                                moveAxisYPlane[1] = Points[i].Y * planeYAxis[1];
                                moveAxisYPlane[2] = Points[i].Y * planeYAxis[2];

                                var moveinAxisXSpace0 = moveAxisXPlane[0] * spaceXAxis;
                                var moveinAxisXSpace1 = moveAxisXPlane[1] * spaceYAxis;
                                var moveinAxisXSpace2 = moveAxisXPlane[2] * spaceZAxis;


                                var moveinAxisYSpace0 = moveAxisYPlane[0] * spaceXAxis;
                                var moveinAxisYSpace1 = moveAxisYPlane[1] * spaceYAxis;
                                var moveinAxisYSpace2 = moveAxisYPlane[2] * spaceZAxis;


                                var vectorMove = moveinAxisXSpace0 + moveinAxisXSpace1 + moveinAxisXSpace2 + moveinAxisYSpace0 + moveinAxisYSpace1 + moveinAxisYSpace2;
                                
                                if (y == 1)
                                {
                                    v2[0] = planeLocation[0] + vectorMove.X;
                                    v2[1] = planeLocation[1] + vectorMove.Y;
                                    v2[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 2)
                                {
                                    v3[0] = planeLocation[0] + vectorMove.X;
                                    v3[1] = planeLocation[1] + vectorMove.Y;
                                    v3[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 3)
                                {
                                    v4[0] = planeLocation[0] + vectorMove.X;
                                    v4[1] = planeLocation[1] + vectorMove.Y;
                                    v4[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 4)
                                {
                                    v5[0] = planeLocation[0] + vectorMove.X;
                                    v5[1] = planeLocation[1] + vectorMove.Y;
                                    v5[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 5)
                                {
                                    v6[0] = planeLocation[0] + vectorMove.X;
                                    v6[1] = planeLocation[1] + vectorMove.Y;
                                    v6[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 6)
                                {
                                    v7[0] = planeLocation[0] + vectorMove.X;
                                    v7[1] = planeLocation[1] + vectorMove.Y;
                                    v7[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 7)
                                {
                                    v8[0] = planeLocation[0] + vectorMove.X;
                                    v8[1] = planeLocation[1] + vectorMove.Y;
                                    v8[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 8)
                                {
                                    v9[0] = planeLocation[0] + vectorMove.X;
                                    v9[1] = planeLocation[1] + vectorMove.Y;
                                    v9[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 9)
                                {
                                    v10[0] = planeLocation[0] + vectorMove.X;
                                    v10[1] = planeLocation[1] + vectorMove.Y;
                                    v10[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 10)
                                {
                                    v11[0] = planeLocation[0] + vectorMove.X;
                                    v11[1] = planeLocation[1] + vectorMove.Y;
                                    v11[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 11)
                                {
                                    v12[0] = planeLocation[0] + vectorMove.X;
                                    v12[1] = planeLocation[1] + vectorMove.Y;
                                    v12[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 12)
                                {
                                    v13[0] = planeLocation[0] + vectorMove.X;
                                    v13[1] = planeLocation[1] + vectorMove.Y;
                                    v13[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 13)
                                {
                                    v14[0] = planeLocation[0] + vectorMove.X;
                                    v14[1] = planeLocation[1] + vectorMove.Y;
                                    v14[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 14)
                                {
                                    v15[0] = planeLocation[0] + vectorMove.X;
                                    v15[1] = planeLocation[1] + vectorMove.Y;
                                    v15[2] = planeLocation[2] + vectorMove.Z;
                                }
                                else if (y == 15)
                                {
                                    v16[0] = planeLocation[0] + vectorMove.X;
                                    v16[1] = planeLocation[1] + vectorMove.Y;
                                    v16[2] = planeLocation[2] + vectorMove.Z;
                                }
                                #region lastVersion to get vertices from List of points
                                /*
                                //--Axis X of reference plane 1,0,0 or -1,0,0
                                if (planeXAxis[0] != 0 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    //--Axis Y of reference plane 0,1,0 or 0,-1,0
                                    if (planeYAxis[0] == 0 && planeYAxis[1] != 0 && planeYAxis[2] == 0)
                                    {
                                        //--Axis normal to plane 0,0,1 or 0, 0, -1
                                        if (normalToPlane[0] == 0 && normalToPlane[1] == 0 && normalToPlane[2] != 0)
                                        {
                                            if (y == 1)
                                            {
                                                v2[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v2[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v2[2] = planeLocation[2];
                                            }
                                            else if (y == 2)
                                            {
                                                v3[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v3[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v3[2] = planeLocation[2];
                                            }
                                            else if (y == 3)
                                            {
                                                v4[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v4[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v4[2] = planeLocation[2];
                                            }
                                            else if (y == 4)
                                            {
                                                v5[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v5[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v5[2] = planeLocation[2];
                                            }
                                            else if (y == 5)
                                            {
                                                v6[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v6[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v6[2] = planeLocation[2];
                                            }
                                            else if (y == 6)
                                            {
                                                v7[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v7[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v7[2] = planeLocation[2];
                                            }
                                            else if (y == 7)
                                            {
                                                v8[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v8[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v8[2] = planeLocation[2];
                                            }
                                            else if (y == 8)
                                            {
                                                v9[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v9[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v9[2] = planeLocation[2];
                                            }
                                            else if (y == 9)
                                            {
                                                v10[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v10[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v10[2] = planeLocation[2];
                                            }
                                            else if (y == 10)
                                            {
                                                v11[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v11[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v11[2] = planeLocation[2];
                                            }
                                            else if (y == 11)
                                            {
                                                v12[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v12[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v12[2] = planeLocation[2];
                                            }
                                            else if (y == 12)
                                            {
                                                v13[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v13[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v13[2] = planeLocation[2];
                                            }
                                            else if (y == 13)
                                            {
                                                v14[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v14[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v14[2] = planeLocation[2];
                                            }
                                            else if (y == 14)
                                            {
                                                v15[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v15[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v15[2] = planeLocation[2];
                                            }
                                            else if (y == 15)
                                            {
                                                v16[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v16[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v16[2] = planeLocation[2];
                                            }

                                        }


                                    }
                                    //--Axis Y of reference plane 0,0,1 or 0, 0, -1
                                    else if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] != 0)
                                    {
                                        //--Axis normal to plane 0,1,0 or 0,-1,0
                                        if (normalToPlane[0] == 0 && normalToPlane[1] != 0 && normalToPlane[2] == 0)
                                        {
                                            if (y == 1)
                                            {
                                                v2[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v2[1] = planeLocation[1];
                                                v2[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 2)
                                            {
                                                v3[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v3[1] = planeLocation[1];
                                                v3[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 3)
                                            {
                                                v4[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v4[1] = planeLocation[1];
                                                v4[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 4)
                                            {
                                                v5[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v5[1] = planeLocation[1];
                                                v5[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 5)
                                            {
                                                v6[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v6[1] = planeLocation[1];
                                                v6[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 6)
                                            {
                                                v7[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v7[1] = planeLocation[1];
                                                v7[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 7)
                                            {
                                                v8[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v8[1] = planeLocation[1];
                                                v8[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 8)
                                            {
                                                v9[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v9[1] = planeLocation[1];
                                                v9[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 9)
                                            {
                                                v10[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v10[1] = planeLocation[1];
                                                v10[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 10)
                                            {
                                                v11[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v11[1] = planeLocation[1];
                                                v11[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 11)
                                            {
                                                v12[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v12[1] = planeLocation[1];
                                                v12[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 12)
                                            {
                                                v13[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v13[1] = planeLocation[1];
                                                v13[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 13)
                                            {
                                                v14[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v14[1] = planeLocation[1];
                                                v14[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 14)
                                            {
                                                v15[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v15[1] = planeLocation[1];
                                                v15[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 15)
                                            {
                                                v16[0] = planeLocation[0] + planeXAxis[0] * Points[i].X;
                                                v16[1] = planeLocation[1];
                                                v16[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                        }
                                        
                                    }

                                }
                                //--Axis X of reference plane 0,1,0 or 0,-1,0
                                else if (planeXAxis[0] == 0 && planeXAxis[1] != 0 && planeXAxis[2] == 0)
                                {
                                    //--Axis Y of reference plane 1,0,0 or -1,0,0
                                    if (planeYAxis[0] != 0 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {
                                        //--Axis normal to plane 0,0,1 or 0, 0, -1
                                        if (normalToPlane[0] == 0 && normalToPlane[1] == 0 && normalToPlane[2] != 0)
                                        {
                                            if (y == 1)
                                            {
                                                v2[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v2[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v2[2] = planeLocation[2];
                                            }
                                            else if (y == 2)
                                            {
                                                v3[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v3[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v3[2] = planeLocation[2];
                                            }
                                            else if (y == 3)
                                            {
                                                v4[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v4[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v4[2] = planeLocation[2];
                                            }
                                            else if (y == 4)
                                            {
                                                v5[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v5[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v5[2] = planeLocation[2];
                                            }
                                            else if (y == 5)
                                            {
                                                v6[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v6[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v6[2] = planeLocation[2];
                                            }
                                            else if (y == 6)
                                            {
                                                v7[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v7[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v7[2] = planeLocation[2];
                                            }
                                            else if (y == 7)
                                            {
                                                v8[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v8[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v8[2] = planeLocation[2];
                                            }
                                            else if (y == 8)
                                            {
                                                v9[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v9[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v9[2] = planeLocation[2];
                                            }
                                            else if (y == 9)
                                            {
                                                v10[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v10[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v10[2] = planeLocation[2];
                                            }
                                            else if (y == 10)
                                            {
                                                v11[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v11[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v11[2] = planeLocation[2];
                                            }
                                            else if (y == 11)
                                            {
                                                v12[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v12[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v12[2] = planeLocation[2];
                                            }
                                            else if (y == 12)
                                            {
                                                v13[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v13[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v13[2] = planeLocation[2];
                                            }
                                            else if (y == 13)
                                            {
                                                v14[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v14[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v14[2] = planeLocation[2];
                                            }
                                            else if (y == 14)
                                            {
                                                v15[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v15[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v15[2] = planeLocation[2];
                                            }
                                            else if (y == 15)
                                            {
                                                v16[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v16[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v16[2] = planeLocation[2];
                                            }

                                        }
                                    }
                                    //--Axis Y of reference plane 0,0,1 or 0, 0, -1
                                    else if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] != 0)
                                    {
                                        //--Axis normal to plane 1,0,0 or -1,0,0
                                        if (normalToPlane[0] != 0 && normalToPlane[1] == 0 && normalToPlane[2] == 0)
                                        {
                                            if (y == 1)
                                            {
                                                v2[0] = planeLocation[0];
                                                v2[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v2[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 2)
                                            {
                                                v3[0] = planeLocation[0];
                                                v3[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v3[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 3)
                                            {
                                                v4[0] = planeLocation[0];
                                                v4[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v4[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 4)
                                            {
                                                v5[0] = planeLocation[0];
                                                v5[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v5[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 5)
                                            {
                                                v6[0] = planeLocation[0];
                                                v6[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v6[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 6)
                                            {
                                                v7[0] = planeLocation[0];
                                                v7[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v7[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 7)
                                            {
                                                v8[0] = planeLocation[0];
                                                v8[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v8[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 8)
                                            {
                                                v9[0] = planeLocation[0];
                                                v9[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v9[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 9)
                                            {
                                                v10[0] = planeLocation[0];
                                                v10[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v10[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 10)
                                            {
                                                v11[0] = planeLocation[0];
                                                v11[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v11[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 11)
                                            {
                                                v12[0] = planeLocation[0];
                                                v12[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v12[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 12)
                                            {
                                                v13[0] = planeLocation[0];
                                                v13[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v13[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 13)
                                            {
                                                v14[0] = planeLocation[0];
                                                v14[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v14[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 14)
                                            {
                                                v15[0] = planeLocation[0];
                                                v15[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v15[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                            else if (y == 15)
                                            {
                                                v16[0] = planeLocation[0];
                                                v16[1] = planeLocation[1] + planeXAxis[1] * Points[i].X;
                                                v16[2] = planeLocation[2] + planeYAxis[2] * Points[i].Y;
                                            }
                                        }

                                    }

                                }
                                //--Axis X of reference plane 0,0,1 or 0, 0, -1
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] != 0)
                                {
                                    //--Axis Y of reference plane 1,0,0 or -1,0,0
                                    if (planeYAxis[0] != 0 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {
                                        //--Axis normal to plane 0,1,0 or 0,-1,0
                                        if (normalToPlane[0] == 0 && normalToPlane[1] != 0 && normalToPlane[2] == 0)
                                        {
                                            if (y == 1)
                                            {
                                                v2[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v2[1] = planeLocation[1];
                                                v2[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 2)
                                            {
                                                v3[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v3[1] = planeLocation[1];
                                                v3[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 3)
                                            {
                                                v4[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v4[1] = planeLocation[1];
                                                v4[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 4)
                                            {
                                                v5[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v5[1] = planeLocation[1];
                                                v5[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 5)
                                            {
                                                v6[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v6[1] = planeLocation[1];
                                                v6[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 6)
                                            {
                                                v7[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v7[1] = planeLocation[1];
                                                v7[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 7)
                                            {
                                                v8[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v8[1] = planeLocation[1];
                                                v8[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 8)
                                            {
                                                v9[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v9[1] = planeLocation[1];
                                                v9[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 9)
                                            {
                                                v10[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v10[1] = planeLocation[1];
                                                v10[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 10)
                                            {
                                                v11[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v11[1] = planeLocation[1];
                                                v11[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 11)
                                            {
                                                v12[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v12[1] = planeLocation[1];
                                                v12[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 12)
                                            {
                                                v13[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v13[1] = planeLocation[1];
                                                v13[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 13)
                                            {
                                                v14[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v14[1] = planeLocation[1];
                                                v14[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 14)
                                            {
                                                v15[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v15[1] = planeLocation[1];
                                                v15[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 15)
                                            {
                                                v16[0] = planeLocation[0] + planeYAxis[0] * Points[i].Y;
                                                v16[1] = planeLocation[1];
                                                v16[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                        }
                                    }
                                    //--Axis Y of reference plane 0,1,0 or 0,-1,0
                                    else if (planeYAxis[0] == 0 && planeYAxis[1] != 0 && planeYAxis[2] == 0)
                                    {
                                        //--Axis normal to plane 1,0,0 or -1,0,0
                                        if (normalToPlane[0] != 0 && normalToPlane[1] == 0 && normalToPlane[2] == 0)
                                        {
                                            if (y == 1)
                                            {
                                                v2[0] = planeLocation[0];
                                                v2[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v2[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 2)
                                            {
                                                v3[0] = planeLocation[0];
                                                v3[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v3[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 3)
                                            {
                                                v4[0] = planeLocation[0];
                                                v4[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v4[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 4)
                                            {
                                                v5[0] = planeLocation[0];
                                                v5[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v5[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 5)
                                            {
                                                v6[0] = planeLocation[0];
                                                v6[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v6[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 6)
                                            {
                                                v7[0] = planeLocation[0];
                                                v7[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v7[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 7)
                                            {
                                                v8[0] = planeLocation[0];
                                                v8[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v8[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 8)
                                            {
                                                v9[0] = planeLocation[0];
                                                v9[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v9[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 9)
                                            {
                                                v10[0] = planeLocation[0];
                                                v10[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v10[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 10)
                                            {
                                                v11[0] = planeLocation[0];
                                                v11[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v11[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 11)
                                            {
                                                v12[0] = planeLocation[0];
                                                v12[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v12[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 12)
                                            {
                                                v13[0] = planeLocation[0];
                                                v13[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v13[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 13)
                                            {
                                                v14[0] = planeLocation[0];
                                                v14[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v14[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 14)
                                            {
                                                v15[0] = planeLocation[0];
                                                v15[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v15[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                            else if (y == 15)
                                            {
                                                v16[0] = planeLocation[0];
                                                v16[1] = planeLocation[1] + planeYAxis[1] * Points[i].Y;
                                                v16[2] = planeLocation[2] + planeXAxis[2] * Points[i].X;
                                            }
                                        }
                                    }
                                }
                                */
                                #endregion lastVersion
                            }

                            // Define points in counterclock wise


                            p2[0] = v2[0];
                            p2[1] = v2[1];
                            p2[2] = v2[2];

                            p3[0] = v3[0];
                            p3[1] = v3[1];
                            p3[2] = v3[2];

                            p4[0] = v4[0];
                            p4[1] = v4[1];
                            p4[2] = v4[2];

                            p5[0] = v5[0];
                            p5[1] = v5[1];
                            p5[2] = v5[2];

                            p6[0] = v6[0];
                            p6[1] = v6[1];
                            p6[2] = v6[2];

                            p7[0] = v7[0];
                            p7[1] = v7[1];
                            p7[2] = v7[2];

                            p8[0] = v8[0];
                            p8[1] = v8[1];
                            p8[2] = v8[2];

                            p9[0] = v9[0];
                            p9[1] = v9[1];
                            p9[2] = v9[2];

                            p10[0] = v10[0];
                            p10[1] = v10[1];
                            p10[2] = v10[2];

                            p11[0] = v11[0];
                            p11[1] = v11[1];
                            p11[2] = v11[2];

                            p12[0] = v12[0];
                            p12[1] = v12[1];
                            p12[2] = v12[2];

                            p13[0] = v13[0];
                            p13[1] = v13[1];
                            p13[2] = v13[2];

                            p14[0] = v14[0];
                            p14[1] = v14[1];
                            p14[2] = v14[2];

                            p15[0] = v15[0];
                            p15[1] = v15[1];
                            p15[2] = v15[2];

                            p16[0] = v16[0];
                            p16[1] = v16[1];
                            p16[2] = v16[2];


                            //(1,0,0) * (0,1,0) * (0,0,1)
                            if (normalToPlane[0] == -1 && normalToPlane[1] == 0 && normalToPlane[2] == 0)
                            {
                                if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == 1)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == -1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {
                                            
                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }
                                        
                                        
                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == -1)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == 1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == -1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == -1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                            }
                            else if (normalToPlane[0] == 1 && normalToPlane[1] == 0 && normalToPlane[2] == 0)
                            {
                                if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == 1)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == -1)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == -1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == -1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == -1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == 1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                            }
                            else if (normalToPlane[0] == 0 && normalToPlane[1] == -1 && normalToPlane[2] == 0)
                            {
                                if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == 1)
                                {
                                    if (planeYAxis[0] == 1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == -1)
                                {
                                    if (planeYAxis[0] == -1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == -1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == -1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                            }
                            else if (normalToPlane[0] == 0 && normalToPlane[1] == 1 && normalToPlane[2] == 0)
                            {
                                if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == 1)
                                {
                                    if (planeYAxis[0] == -1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 0 && planeXAxis[2] == -1)
                                {
                                    if (planeYAxis[0] == 1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == 1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == -1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 0 && planeYAxis[2] == -1)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                            }
                            else if (normalToPlane[0] == 0 && normalToPlane[1] == 0 && normalToPlane[2] == 1)
                            {
                                if (planeXAxis[0] == -1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == -1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == 1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == -1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == -1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                            }
                            else if (normalToPlane[0] == 0 && normalToPlane[1] == 0 && normalToPlane[2] == -1)
                            {
                                if (planeXAxis[0] == 0 && planeXAxis[1] == 1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == -1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 0 && planeXAxis[1] == -1 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 1 && planeYAxis[1] == 0 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == 1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == 1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                                else if (planeXAxis[0] == -1 && planeXAxis[1] == 0 && planeXAxis[2] == 0)
                                {
                                    if (planeYAxis[0] == 0 && planeYAxis[1] == -1 && planeYAxis[2] == 0)
                                    {

                                        if (pointsCount - 1 == 4)
                                        {

                                            p2[0] = v5[0];
                                            p2[1] = v5[1];
                                            p2[2] = v5[2];

                                            p3[0] = v4[0];
                                            p3[1] = v4[1];
                                            p3[2] = v4[2];

                                            p4[0] = v3[0];
                                            p4[1] = v3[1];
                                            p4[2] = v3[2];

                                            p5[0] = v2[0];
                                            p5[1] = v2[1];
                                            p5[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 5)
                                        {
                                            p2[0] = v6[0];
                                            p2[1] = v6[1];
                                            p2[2] = v6[2];

                                            p3[0] = v5[0];
                                            p3[1] = v5[1];
                                            p3[2] = v5[2];

                                            p4[0] = v4[0];
                                            p4[1] = v4[1];
                                            p4[2] = v4[2];

                                            p5[0] = v3[0];
                                            p5[1] = v3[1];
                                            p5[2] = v3[2];

                                            p6[0] = v2[0];
                                            p6[1] = v2[1];
                                            p6[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 6)
                                        {
                                            p2[0] = v7[0];
                                            p2[1] = v7[1];
                                            p2[2] = v7[2];

                                            p3[0] = v6[0];
                                            p3[1] = v6[1];
                                            p3[2] = v6[2];

                                            p4[0] = v5[0];
                                            p4[1] = v5[1];
                                            p4[2] = v5[2];

                                            p5[0] = v4[0];
                                            p5[1] = v4[1];
                                            p5[2] = v4[2];

                                            p6[0] = v3[0];
                                            p6[1] = v3[1];
                                            p6[2] = v3[2];

                                            p7[0] = v2[0];
                                            p7[1] = v2[1];
                                            p7[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 7)
                                        {
                                            p2[0] = v8[0];
                                            p2[1] = v8[1];
                                            p2[2] = v8[2];

                                            p3[0] = v7[0];
                                            p3[1] = v7[1];
                                            p3[2] = v7[2];

                                            p4[0] = v6[0];
                                            p4[1] = v6[1];
                                            p4[2] = v6[2];

                                            p5[0] = v5[0];
                                            p5[1] = v5[1];
                                            p5[2] = v5[2];

                                            p6[0] = v4[0];
                                            p6[1] = v4[1];
                                            p6[2] = v4[2];

                                            p7[0] = v3[0];
                                            p7[1] = v3[1];
                                            p7[2] = v3[2];

                                            p8[0] = v2[0];
                                            p8[1] = v2[1];
                                            p8[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 8)
                                        {
                                            p2[0] = v9[0];
                                            p2[1] = v9[1];
                                            p2[2] = v9[2];

                                            p3[0] = v8[0];
                                            p3[1] = v8[1];
                                            p3[2] = v8[2];

                                            p4[0] = v7[0];
                                            p4[1] = v7[1];
                                            p4[2] = v7[2];

                                            p5[0] = v6[0];
                                            p5[1] = v6[1];
                                            p5[2] = v6[2];

                                            p6[0] = v5[0];
                                            p6[1] = v5[1];
                                            p6[2] = v5[2];

                                            p7[0] = v4[0];
                                            p7[1] = v4[1];
                                            p7[2] = v4[2];

                                            p8[0] = v3[0];
                                            p8[1] = v3[1];
                                            p8[2] = v3[2];

                                            p9[0] = v2[0];
                                            p9[1] = v2[1];
                                            p9[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 9)
                                        {
                                            p2[0] = v10[0];
                                            p2[1] = v10[1];
                                            p2[2] = v10[2];

                                            p3[0] = v9[0];
                                            p3[1] = v9[1];
                                            p3[2] = v9[2];

                                            p4[0] = v8[0];
                                            p4[1] = v8[1];
                                            p4[2] = v8[2];

                                            p5[0] = v7[0];
                                            p5[1] = v7[1];
                                            p5[2] = v7[2];

                                            p6[0] = v6[0];
                                            p6[1] = v6[1];
                                            p6[2] = v6[2];

                                            p7[0] = v5[0];
                                            p7[1] = v5[1];
                                            p7[2] = v5[2];

                                            p8[0] = v4[0];
                                            p8[1] = v4[1];
                                            p8[2] = v4[2];

                                            p9[0] = v3[0];
                                            p9[1] = v3[1];
                                            p9[2] = v3[2];

                                            p10[0] = v2[0];
                                            p10[1] = v2[1];
                                            p10[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 10)
                                        {
                                            p2[0] = v11[0];
                                            p2[1] = v11[1];
                                            p2[2] = v11[2];

                                            p3[0] = v10[0];
                                            p3[1] = v10[1];
                                            p3[2] = v10[2];

                                            p4[0] = v9[0];
                                            p4[1] = v9[1];
                                            p4[2] = v9[2];

                                            p5[0] = v8[0];
                                            p5[1] = v8[1];
                                            p5[2] = v8[2];

                                            p6[0] = v7[0];
                                            p6[1] = v7[1];
                                            p6[2] = v7[2];

                                            p7[0] = v6[0];
                                            p7[1] = v6[1];
                                            p7[2] = v6[2];

                                            p8[0] = v5[0];
                                            p8[1] = v5[1];
                                            p8[2] = v5[2];

                                            p9[0] = v4[0];
                                            p9[1] = v4[1];
                                            p9[2] = v4[2];

                                            p10[0] = v3[0];
                                            p10[1] = v3[1];
                                            p10[2] = v3[2];

                                            p11[0] = v2[0];
                                            p11[1] = v2[1];
                                            p11[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 11)
                                        {
                                            p2[0] = v12[0];
                                            p2[1] = v12[1];
                                            p2[2] = v12[2];

                                            p3[0] = v11[0];
                                            p3[1] = v11[1];
                                            p3[2] = v11[2];

                                            p4[0] = v10[0];
                                            p4[1] = v10[1];
                                            p4[2] = v10[2];

                                            p5[0] = v9[0];
                                            p5[1] = v9[1];
                                            p5[2] = v9[2];

                                            p6[0] = v8[0];
                                            p6[1] = v8[1];
                                            p6[2] = v8[2];

                                            p7[0] = v7[0];
                                            p7[1] = v7[1];
                                            p7[2] = v7[2];

                                            p8[0] = v6[0];
                                            p8[1] = v6[1];
                                            p8[2] = v6[2];

                                            p9[0] = v5[0];
                                            p9[1] = v5[1];
                                            p9[2] = v5[2];

                                            p10[0] = v4[0];
                                            p10[1] = v4[1];
                                            p10[2] = v4[2];

                                            p11[0] = v3[0];
                                            p11[1] = v3[1];
                                            p11[2] = v3[2];

                                            p12[0] = v2[0];
                                            p12[1] = v2[1];
                                            p12[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 12)
                                        {
                                            p2[0] = v13[0];
                                            p2[1] = v13[1];
                                            p2[2] = v13[2];

                                            p3[0] = v12[0];
                                            p3[1] = v12[1];
                                            p3[2] = v12[2];

                                            p4[0] = v11[0];
                                            p4[1] = v11[1];
                                            p4[2] = v11[2];

                                            p5[0] = v10[0];
                                            p5[1] = v10[1];
                                            p5[2] = v10[2];

                                            p6[0] = v9[0];
                                            p6[1] = v9[1];
                                            p6[2] = v9[2];

                                            p7[0] = v8[0];
                                            p7[1] = v8[1];
                                            p7[2] = v8[2];

                                            p8[0] = v7[0];
                                            p8[1] = v7[1];
                                            p8[2] = v7[2];

                                            p9[0] = v6[0];
                                            p9[1] = v6[1];
                                            p9[2] = v6[2];

                                            p10[0] = v5[0];
                                            p10[1] = v5[1];
                                            p10[2] = v5[2];

                                            p11[0] = v4[0];
                                            p11[1] = v4[1];
                                            p11[2] = v4[2];

                                            p12[0] = v3[0];
                                            p12[1] = v3[1];
                                            p12[2] = v3[2];

                                            p13[0] = v2[0];
                                            p13[1] = v2[1];
                                            p13[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 13)
                                        {
                                            p2[0] = v14[0];
                                            p2[1] = v14[1];
                                            p2[2] = v14[2];

                                            p3[0] = v13[0];
                                            p3[1] = v13[1];
                                            p3[2] = v13[2];

                                            p4[0] = v12[0];
                                            p4[1] = v12[1];
                                            p4[2] = v12[2];

                                            p5[0] = v11[0];
                                            p5[1] = v11[1];
                                            p5[2] = v11[2];

                                            p6[0] = v10[0];
                                            p6[1] = v10[1];
                                            p6[2] = v10[2];

                                            p7[0] = v9[0];
                                            p7[1] = v9[1];
                                            p7[2] = v9[2];

                                            p8[0] = v8[0];
                                            p8[1] = v8[1];
                                            p8[2] = v8[2];

                                            p9[0] = v7[0];
                                            p9[1] = v7[1];
                                            p9[2] = v7[2];

                                            p10[0] = v6[0];
                                            p10[1] = v6[1];
                                            p10[2] = v6[2];

                                            p11[0] = v5[0];
                                            p11[1] = v5[1];
                                            p11[2] = v5[2];

                                            p12[0] = v4[0];
                                            p12[1] = v4[1];
                                            p12[2] = v4[2];

                                            p13[0] = v3[0];
                                            p13[1] = v3[1];
                                            p13[2] = v3[2];

                                            p14[0] = v2[0];
                                            p14[1] = v2[1];
                                            p14[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 14)
                                        {
                                            p2[0] = v15[0];
                                            p2[1] = v15[1];
                                            p2[2] = v15[2];

                                            p3[0] = v14[0];
                                            p3[1] = v14[1];
                                            p3[2] = v14[2];

                                            p4[0] = v13[0];
                                            p4[1] = v13[1];
                                            p4[2] = v13[2];

                                            p5[0] = v12[0];
                                            p5[1] = v12[1];
                                            p5[2] = v12[2];

                                            p6[0] = v11[0];
                                            p6[1] = v11[1];
                                            p6[2] = v11[2];

                                            p7[0] = v10[0];
                                            p7[1] = v10[1];
                                            p7[2] = v10[2];

                                            p8[0] = v9[0];
                                            p8[1] = v9[1];
                                            p8[2] = v9[2];

                                            p9[0] = v8[0];
                                            p9[1] = v8[1];
                                            p9[2] = v8[2];

                                            p10[0] = v7[0];
                                            p10[1] = v7[1];
                                            p10[2] = v7[2];

                                            p11[0] = v6[0];
                                            p11[1] = v6[1];
                                            p11[2] = v6[2];

                                            p12[0] = v5[0];
                                            p12[1] = v5[1];
                                            p12[2] = v5[2];

                                            p13[0] = v4[0];
                                            p13[1] = v4[1];
                                            p13[2] = v4[2];

                                            p14[0] = v3[0];
                                            p14[1] = v3[1];
                                            p14[2] = v3[2];

                                            p15[0] = v2[0];
                                            p15[1] = v2[1];
                                            p15[2] = v2[2];
                                        }
                                        if (pointsCount - 1 == 15)
                                        {
                                            p2[0] = v16[0];
                                            p2[1] = v16[1];
                                            p2[2] = v16[2];

                                            p3[0] = v15[0];
                                            p3[1] = v15[1];
                                            p3[2] = v15[2];

                                            p4[0] = v14[0];
                                            p4[1] = v14[1];
                                            p4[2] = v14[2];

                                            p5[0] = v13[0];
                                            p5[1] = v13[1];
                                            p5[2] = v13[2];

                                            p6[0] = v12[0];
                                            p6[1] = v12[1];
                                            p6[2] = v12[2];

                                            p7[0] = v11[0];
                                            p7[1] = v11[1];
                                            p7[2] = v11[2];

                                            p8[0] = v10[0];
                                            p8[1] = v10[1];
                                            p8[2] = v10[2];

                                            p9[0] = v9[0];
                                            p9[1] = v9[1];
                                            p9[2] = v9[2];

                                            p10[0] = v8[0];
                                            p10[1] = v8[1];
                                            p10[2] = v8[2];

                                            p11[0] = v7[0];
                                            p11[1] = v7[1];
                                            p11[2] = v7[2];

                                            p12[0] = v6[0];
                                            p12[1] = v6[1];
                                            p12[2] = v6[2];

                                            p13[0] = v5[0];
                                            p13[1] = v5[1];
                                            p13[2] = v5[2];

                                            p14[0] = v4[0];
                                            p14[1] = v4[1];
                                            p14[2] = v4[2];

                                            p15[0] = v3[0];
                                            p15[1] = v3[1];
                                            p15[2] = v3[2];

                                            p16[0] = v2[0];
                                            p16[1] = v2[1];
                                            p16[2] = v2[2];
                                        }


                                    }
                                }
                            }


                        }


                        //--------------------------------------
                        //---Build the EnergyPlus Classes depending on type of building Element

                        if (buildingElement is IIfcWall)
                        {
                            
                            var theWall = buildingElement as IIfcWall;
                            BuildingSurfaceDetailed wall = new BuildingSurfaceDetailed();
                           
                            wall.Name = theWall.Name + "-" + theSpace.GlobalId;
                            wall.Name = BuildingSurfaceDetailed.DuplicateNameFix(wall);
                            wall.SurfaceType = SurfaceTypeEnum.Wall;

                            var DefinedBy = theWall.IsTypedBy.FirstOrDefault().RelatingType.Name;
                            var wallType = model.Instances.FirstOrDefault<IIfcWallType>(d => d.Name == DefinedBy);

                            var hasAssociations = theWall.HasAssociations.FirstOrDefault() as IIfcRelAssociatesMaterial;
                            var relatingMaterial = hasAssociations.RelatingMaterial as IIfcMaterialLayerSetUsage;
                            wall.ConstructionName = relatingMaterial.ForLayerSet.LayerSetName;

                            wall.ZoneName = theSpace.Name;
                            if (internalOrExternalBoundary == "INTERNAL")
                            {
                                wall.OutsideBoundaryCondition = OutsideBoundaryConditionEnum.Zone;
                                var providesBoundaries = theWall.ProvidesBoundaries.ToList();
                                var Space = providesBoundaries[1].RelatingSpace;
                                var _space = Space as IIfcSpace;
                                wall.OutsideBoundaryConditionObject = _space.Name;
                                wall.SunExposure = SunExposureEnum.NoSun;
                                wall.WindExposure = WindExposureEnum.NoWind;

                            }
                            else
                            {
                                wall.OutsideBoundaryCondition = OutsideBoundaryConditionEnum.Outdoors;
                                wall.SunExposure = SunExposureEnum.SunExposed;
                                wall.WindExposure = WindExposureEnum.WindExposed;

                            }


                            wall.Vertex1XCoordinate = Math.Round(p2[0], 3);
                            wall.Vertex1YCoordinate = Math.Round(p2[1], 3);
                            wall.Vertex1ZCoordinate = Math.Round(p2[2], 3);

                            wall.Vertex2XCoordinate = Math.Round(p3[0], 3);
                            wall.Vertex2YCoordinate = Math.Round(p3[1], 3);
                            wall.Vertex2ZCoordinate = Math.Round(p3[2], 3);

                            wall.Vertex3XCoordinate = Math.Round(p4[0], 3);
                            wall.Vertex3YCoordinate = Math.Round(p4[1], 3);
                            wall.Vertex3ZCoordinate = Math.Round(p4[2], 3);


                            if (pointsCount - 1 >= 4)
                            {
                                wall.Vertex4XCoordinate = Math.Round(p5[0], 3);
                                wall.Vertex4YCoordinate = Math.Round(p5[1], 3);
                                wall.Vertex4ZCoordinate = Math.Round(p5[2], 3);
                            }
                            if (pointsCount - 1 >= 5)
                            {
                                wall.Vertex5XCoordinate = Math.Round(p6[0], 3);
                                wall.Vertex5YCoordinate = Math.Round(p6[1], 3);
                                wall.Vertex5ZCoordinate = Math.Round(p6[2], 3);
                            }
                            if (pointsCount - 1 >= 6)
                            {
                                wall.Vertex6XCoordinate = Math.Round(p7[0], 3);
                                wall.Vertex6YCoordinate = Math.Round(p7[1], 3);
                                wall.Vertex6ZCoordinate = Math.Round(p7[2], 3);
                            }
                            if (pointsCount - 1 >= 7)
                            {
                                wall.Vertex7XCoordinate = Math.Round(p8[0], 3);
                                wall.Vertex7YCoordinate = Math.Round(p8[1], 3);
                                wall.Vertex7ZCoordinate = Math.Round(p8[2], 3);
                            }
                            if (pointsCount - 1 >= 8)
                            {
                                wall.Vertex8XCoordinate = Math.Round(p9[0], 3);
                                wall.Vertex8YCoordinate = Math.Round(p9[1], 3);
                                wall.Vertex8ZCoordinate = Math.Round(p9[2], 3);
                            }
                            if (pointsCount - 1 >= 9)
                            {
                                wall.Vertex9XCoordinate = Math.Round(p10[0], 3);
                                wall.Vertex9YCoordinate = Math.Round(p10[1], 3);
                                wall.Vertex9ZCoordinate = Math.Round(p10[2], 3);
                            }
                            if (pointsCount - 1 >= 10)
                            {
                                wall.Vertex10XCoordinate = Math.Round(p11[0], 3);
                                wall.Vertex10YCoordinate = Math.Round(p11[1], 3);
                                wall.Vertex10ZCoordinate = Math.Round(p11[2], 3);
                            }
                            if (BuildingSurfaceDetailed.DuplicateCheck(wall))
                            {

                            }
                            else
                            {
                                BuildingSurfaceDetailed.Add(wall);
                            }

                            




                        }
                        else if (buildingElement is IIfcSlab)
                        {
                            var theSlab = buildingElement as IIfcSlab;
                            BuildingSurfaceDetailed slab = new BuildingSurfaceDetailed();
                            slab.Name = theSlab.Name + "-" + theSpace.GlobalId;
                            slab.Name = BuildingSurfaceDetailed.DuplicateNameFix(slab);
                            slab.SurfaceType = SurfaceTypeEnum.Floor;

                            var DefinedBy = theSlab.IsTypedBy.FirstOrDefault().RelatingType.Name; 
                            var slabType = model.Instances.FirstOrDefault<IIfcSlabType>(d => d.Name == DefinedBy);
                            var hasAssociations = theSlab.HasAssociations.FirstOrDefault() as IIfcRelAssociatesMaterial;
                            var relatingMaterial = hasAssociations.RelatingMaterial as IIfcMaterialLayerSetUsage;
                            slab.ConstructionName = relatingMaterial.ForLayerSet.LayerSetName;

                            slab.ZoneName = theSpace.Name;
                            slab.OutsideBoundaryCondition = OutsideBoundaryConditionEnum.Outdoors;
                            slab.SunExposure = SunExposureEnum.SunExposed;
                            slab.WindExposure = WindExposureEnum.WindExposed;

                            slab.NumberofVertices = "autocalculate";

                            slab.Vertex1XCoordinate = Math.Round(p2[0], 3);
                            slab.Vertex1YCoordinate = Math.Round(p2[1], 3);
                            slab.Vertex1ZCoordinate = Math.Round(p2[2], 3);

                            slab.Vertex2XCoordinate = Math.Round(p3[0], 3);
                            slab.Vertex2YCoordinate = Math.Round(p3[1], 3);
                            slab.Vertex2ZCoordinate = Math.Round(p3[2], 3);

                            slab.Vertex3XCoordinate = Math.Round(p4[0], 3);
                            slab.Vertex3YCoordinate = Math.Round(p4[1], 3);
                            slab.Vertex3ZCoordinate = Math.Round(p4[2], 3);


                            if (pointsCount - 1 >= 4)
                            {
                                slab.Vertex4XCoordinate = Math.Round(p5[0], 3);
                                slab.Vertex4YCoordinate = Math.Round(p5[1], 3);
                                slab.Vertex4ZCoordinate = Math.Round(p5[2], 3);
                            }
                            if (pointsCount - 1 >= 5)
                            {
                                slab.Vertex5XCoordinate = Math.Round(p6[0], 3);
                                slab.Vertex5YCoordinate = Math.Round(p6[1], 3);
                                slab.Vertex5ZCoordinate = Math.Round(p6[2], 3);
                            }
                            if (pointsCount - 1 >= 6)
                            {
                                slab.Vertex6XCoordinate = Math.Round(p7[0], 3);
                                slab.Vertex6YCoordinate = Math.Round(p7[1], 3);
                                slab.Vertex6ZCoordinate = Math.Round(p7[2], 3);
                            }
                            if (pointsCount - 1 >= 7)
                            {
                                slab.Vertex7XCoordinate = Math.Round(p8[0], 3);
                                slab.Vertex7YCoordinate = Math.Round(p8[1], 3);
                                slab.Vertex7ZCoordinate = Math.Round(p8[2], 3);
                            }
                            if (pointsCount - 1 >= 8)
                            {
                                slab.Vertex8XCoordinate = Math.Round(p9[0], 3);
                                slab.Vertex8YCoordinate = Math.Round(p9[1], 3);
                                slab.Vertex8ZCoordinate = Math.Round(p9[2], 3);
                            }
                            if (pointsCount - 1 >= 9)
                            {
                                slab.Vertex9XCoordinate = Math.Round(p10[0], 3);
                                slab.Vertex9YCoordinate = Math.Round(p10[1], 3);
                                slab.Vertex9ZCoordinate = Math.Round(p10[2], 3);
                            }
                            if (pointsCount - 1 >= 10)
                            {
                                slab.Vertex10XCoordinate = Math.Round(p11[0], 3);
                                slab.Vertex10YCoordinate = Math.Round(p11[1], 3);
                                slab.Vertex10ZCoordinate = Math.Round(p11[2], 3);
                            }
                            if (BuildingSurfaceDetailed.DuplicateCheck(slab))
                            {

                            }
                            else
                            {
                                BuildingSurfaceDetailed.Add(slab);
                            }

                        }
                        else if (buildingElement is IIfcDoor)
                        {
                            var theDoor = buildingElement as IIfcDoor;
                            FenestrationSurfaceDetailed door = new FenestrationSurfaceDetailed();
                            door.Name = theDoor.Name + "-" + theSpace.GlobalId;
                            door.Name = FenestrationSurfaceDetailed.DuplicateNameFix(door);
                            door.SurfaceType = SurfaceTypeEnumFSD.Door;

                            var hasAssociations = theDoor.HasAssociations.FirstOrDefault() as IIfcRelAssociatesMaterial;
                            
                            var relatingMaterial = hasAssociations.RelatingMaterial as IIfcMaterial;
                            Console.WriteLine($"Door Has Associations {hasAssociations.RelatedObjects[0]}");
                            if (relatingMaterial != null)
                            {
                                door.ConstructionName = $"{relatingMaterial.Name}";
                            }
                            
                            var openingElement = theDoor.FillsVoids.FirstOrDefault().RelatingOpeningElement as IIfcOpeningElement;
                            var voidsElement = openingElement.VoidsElements as IIfcRelVoidsElement;
                            door.BuildingSurfaceName = voidsElement.RelatingBuildingElement.Name + "-" + theSpace.GlobalId;

                            door.NumberofVertices = "autocalculate";

                            door.Vertex1XCoordinate = Math.Round(p2[0], 3);
                            door.Vertex1YCoordinate = Math.Round(p2[1], 3);
                            door.Vertex1ZCoordinate = Math.Round(p2[2], 3);

                            door.Vertex2XCoordinate = Math.Round(p3[0], 3);
                            door.Vertex2YCoordinate = Math.Round(p3[1], 3);
                            door.Vertex2ZCoordinate = Math.Round(p3[2], 3);

                            door.Vertex3XCoordinate = Math.Round(p4[0], 3);
                            door.Vertex3YCoordinate = Math.Round(p4[1], 3);
                            door.Vertex3ZCoordinate = Math.Round(p4[2], 3);


                            if (pointsCount - 1 >= 4)
                            {
                                door.Vertex4XCoordinate = Math.Round(p5[0], 3);
                                door.Vertex4YCoordinate = Math.Round(p5[1], 3);
                                door.Vertex4ZCoordinate = Math.Round(p5[2], 3);
                            }
                            
                            if (FenestrationSurfaceDetailed.DuplicateCheck(door))
                            {

                            }
                            else
                            {
                                FenestrationSurfaceDetailed.Add(door);
                            }
                        }
                        #region VirtualBoundary Contain virtual Space Boundary geometry
                        /*
                        else if (buildingElement == null)
                        {
                            
                            BuildingSurfaceDetailed empty = new BuildingSurfaceDetailed();
                            empty.Name = "VirtualBoundary" + "-" + theSpace.GlobalId;
                            empty.Name = BuildingSurfaceDetailed.DuplicateNameFix(empty);

                            Console.WriteLine($"{planeLocation[0]}, {planeLocation[1]}, {planeLocation[2]} ---plane Location NULLL");

                            empty.ZoneName = theSpace.Name;
                            empty.OutsideBoundaryCondition = OutsideBoundaryConditionEnum.Outdoors;
                            empty.SunExposure = SunExposureEnum.SunExposed;
                            empty.WindExposure = WindExposureEnum.WindExposed;

                            empty.NumberofVertices = "autocalculate";

                            empty.Vertex1XCoordinate = p2[0];
                            empty.Vertex1YCoordinate = p2[1];
                            empty.Vertex1ZCoordinate = p2[2];

                            empty.Vertex2XCoordinate = p3[0];
                            empty.Vertex2YCoordinate = p3[1];
                            empty.Vertex2ZCoordinate = p3[2];

                            empty.Vertex3XCoordinate = p4[0];
                            empty.Vertex3YCoordinate = p4[1];
                            empty.Vertex3ZCoordinate = p4[2];


                            if (pointsCount - 1 >= 4)
                            {
                                empty.Vertex4XCoordinate = p5[0];
                                empty.Vertex4YCoordinate = p5[1];
                                empty.Vertex4ZCoordinate = p5[2];
                            }
                            if (pointsCount - 1 >= 5)
                            {
                                empty.Vertex5XCoordinate = p6[0];
                                empty.Vertex5YCoordinate = p6[1];
                                empty.Vertex5ZCoordinate = p6[2];
                            }
                            if (pointsCount - 1 >= 6)
                            {
                                empty.Vertex6XCoordinate = p7[0];
                                empty.Vertex6YCoordinate = p7[1];
                                empty.Vertex6ZCoordinate = p7[2];
                            }
                            if (pointsCount - 1 >= 7)
                            {
                                empty.Vertex7XCoordinate = p8[0];
                                empty.Vertex7YCoordinate = p8[1];
                                empty.Vertex7ZCoordinate = p8[2];
                            }
                            if (pointsCount - 1 >= 8)
                            {
                                empty.Vertex8XCoordinate = p9[0];
                                empty.Vertex8YCoordinate = p9[1];
                                empty.Vertex8ZCoordinate = p9[2];
                            }
                            if (pointsCount - 1 >= 9)
                            {
                                empty.Vertex9XCoordinate = p10[0];
                                empty.Vertex9YCoordinate = p10[1];
                                empty.Vertex9ZCoordinate = p10[2];
                            }
                            if (pointsCount - 1 >= 10)
                            {
                                empty.Vertex10XCoordinate = p11[0];
                                empty.Vertex10YCoordinate = p11[1];
                                empty.Vertex10ZCoordinate = p11[2];
                            }
                            if (BuildingSurfaceDetailed.DuplicateCheck(empty))
                            {

                            }
                            else
                            {
                                BuildingSurfaceDetailed.Add(empty);
                            }
                        }
                        */
#endregion VirtualBoundary
                    }

                    //IfcOpeningElement - FenestrationSurface:Detailed
                    //todo: find the correct surface name for window
                    var openingElements = model.Instances.OfType<IIfcOpeningElement>().ToList();
                    foreach (IIfcOpeningElement openingElement in openingElements)
                    {
                        //--------Geometry
                        // using Opening data to get geometry of the window
                        FenestrationSurfaceDetailed fsd = new FenestrationSurfaceDetailed();
                        if (openingElement.HasFillings.FirstOrDefault() != null)
                        {
                            var relatedBuildingElement = openingElement.HasFillings.FirstOrDefault().RelatedBuildingElement;
                            if (relatedBuildingElement is IIfcWindow)
                            {
                                var window = relatedBuildingElement as IIfcWindow;
                                fsd.Name = window.Name;
                                fsd.SurfaceType = SurfaceTypeEnumFSD.Window;
                                fsd.ConstructionName = window.IsTypedBy.FirstOrDefault().RelatingType.Name;
                                var voidsElement = openingElement.VoidsElements as IIfcRelVoidsElement;
                                fsd.BuildingSurfaceName = voidsElement.RelatingBuildingElement.Name;
                                Console.WriteLine(openingElement.VoidsElements);
                            }
                            else if (relatedBuildingElement is IIfcDoor)
                            {
                                var door = relatedBuildingElement as IIfcDoor;
                                fsd.Name = door.Name;
                                fsd.SurfaceType = SurfaceTypeEnumFSD.Door;
                                var hasAssociations = door.HasAssociations.FirstOrDefault() as IIfcRelAssociatesMaterial;
                                var relatingMaterial = hasAssociations.RelatingMaterial as IIfcMaterial;
                                if (relatingMaterial != null)
                                {
                                    fsd.ConstructionName = $"{relatingMaterial.Name}";
                                }
                                
                                var voidsElement = openingElement.VoidsElements as IIfcRelVoidsElement;
                                fsd.BuildingSurfaceName = voidsElement.RelatingBuildingElement.Name;
                                
                            }
                        } 
                        
                        var localPlacement = openingElement.ObjectPlacement;
                        var _localPlacement = localPlacement as IIfcLocalPlacement;
                        var relativePlacementOp = _localPlacement.RelativePlacement;
                        var _relativePlacementOp = relativePlacementOp as IIfcAxis2Placement3D;
                        double[] IPOpening = new double[3];
                        IPOpening[0] = _relativePlacementOp.Location.X;
                        IPOpening[1] = _relativePlacementOp.Location.Y;
                        IPOpening[2] = _relativePlacementOp.Location.Z;   //Contain initial point of the opening relative to the wall 
                        

                        var placementRelTo = _localPlacement.PlacementRelTo;
                        var _placementRelTo = placementRelTo as IIfcLocalPlacement;
                        var relativePlacementWall = _placementRelTo.RelativePlacement;
                        var _relativePlacementWall = relativePlacementWall as IIfcAxis2Placement3D;
                        var p0 = _relativePlacementWall.P[0];
                        var p1 = _relativePlacementWall.P[1];
                        var p2 = _relativePlacementWall.P[2];
                        double[] IPWall = new double[3];
                        IPWall[0] = _relativePlacementWall.Location.X;
                        IPWall[1] = _relativePlacementWall.Location.Y;
                        IPWall[2] = _relativePlacementWall.Location.Z;   // Contain initial point of the wall relative to the building

                        double[] refDirection = new double[3];
                        if (_relativePlacementWall.RefDirection != null)
                        {
                            var wallRefDirection = _relativePlacementWall.RefDirection;
                            refDirection[0] = wallRefDirection.X;
                            refDirection[1] = wallRefDirection.Y;
                            refDirection[2] = wallRefDirection.Z;
                        }
                        else
                        {
                            refDirection[0] = 1;
                            refDirection[1] = 0;
                            refDirection[2] = 0;
                        } //Define RefDirection of the wall to calculate de IP of the opening relative to the building


                        double[] v1 = new double[3]; // array of double v1, v2, v3, v4 contain coordinates of the vertices
                        double[] v2 = new double[3];
                        double[] v3 = new double[3];
                        double[] v4 = new double[3];
                        double[] v5 = new double[3];
                        if (refDirection[1] != 0)
                        {
                            v1[0] = IPWall[0] + IPOpening[1] * refDirection[0];
                            v1[1] = IPWall[1] + IPOpening[0] * refDirection[1];
                            v1[2] = IPWall[2] + IPOpening[2] * refDirection[2];
                        }
                        else
                        {
                            v1[0] = IPWall[0] + IPOpening[0] * refDirection[0];
                            v1[1] = IPWall[1] + IPOpening[1] * refDirection[1];
                            v1[2] = IPWall[2] + IPOpening[2] * refDirection[2];
                        }

                        

                        double[] axisExtruded = new double[3];
                        double[] dimensions = new double[3];
                        if (openingElement.Representation != null)
                        {
                            if (openingElement.Representation.Representations[0].Items[0] is IIfcExtrudedAreaSolid)
                            {
                                var representation = openingElement.Representation.Representations[0].Items[0];
                                var _representation = representation as IIfcExtrudedAreaSolid;

                                var pXDim = _representation.Position.P[0]; //AxisX
                                var pYDim = _representation.Position.P[1]; //AxisY
                                var pDepth = _representation.Position.P[2]; //AxisZ

                                var depth = _representation.Depth;
                                var sweptArea = _representation.SweptArea;

                                if (sweptArea is IIfcRectangleProfileDef)
                                {
                                    var _sweptArea = sweptArea as IIfcRectangleProfileDef;
                                    var XDim = _sweptArea.XDim;
                                    var YDim = _sweptArea.YDim;

                                    var d0 = pXDim * XDim;
                                    var d1 = pYDim * YDim;
                                    var d2 = pDepth * depth;
                                    var dFinal = d0 + d1 + d2; //Vector with the lenght in each direction for the solid

                                    dimensions[0] = dFinal.X;
                                    dimensions[1] = dFinal.Y;
                                    dimensions[2] = dFinal.Z;

                                    if (refDirection[0] != 0)
                                    {
                                        dimensions[0] = dimensions[0] * refDirection[0];
                                    }
                                    if (refDirection[1] != 0)
                                    {
                                        dimensions[1] = dimensions[1] * refDirection[1];
                                    }
                                    if (refDirection[2] != 0)
                                    {
                                        dimensions[2] = dimensions[2] * refDirection[2];
                                    } //Correction of the direction of the vector dimensions to calculate the remaining vertices


                                    if (Math.Abs(dimensions[0]) < Math.Abs(dimensions[1]))
                                    {
                                        if (Math.Abs(dimensions[0]) < Math.Abs(dimensions[2]))
                                        {
                                            v2[0] = v1[0];
                                            v2[1] = v1[1] + dimensions[1];
                                            v2[2] = v1[2];

                                            v3[0] = v2[0];
                                            v3[1] = v2[1];
                                            v3[2] = v2[2] + dimensions[2];

                                            v4[0] = v1[0];
                                            v4[1] = v1[1];
                                            v4[2] = v1[2] + dimensions[2];
                                        }
                                        else
                                        {
                                            v2[0] = v1[0] + dimensions[0];
                                            v2[1] = v1[1];
                                            v2[2] = v1[2];

                                            v3[0] = v2[0];
                                            v3[1] = v2[1] + dimensions[1];
                                            v3[2] = v2[2];

                                            v4[0] = v1[0];
                                            v4[1] = v1[1] + dimensions[1];
                                            v4[2] = v1[2];
                                        }
                                    }
                                    else
                                    {
                                        v2[0] = v1[0] + dimensions[0];
                                        v2[1] = v1[1];
                                        v2[2] = v1[2];

                                        v3[0] = v2[0];
                                        v3[1] = v2[1];
                                        v3[2] = v2[2] + dimensions[2];

                                        v4[0] = v1[0];
                                        v4[1] = v1[1];
                                        v4[2] = v1[2] + dimensions[2];
                                    }
                                }
                                else if (sweptArea is IIfcArbitraryClosedProfileDef)
                                {
                                    var _sweptArea = sweptArea as IIfcArbitraryClosedProfileDef;
                                    var outerCurve = _sweptArea.OuterCurve as IIfcIndexedPolyCurve;
                                    var pointsList2D = outerCurve.Points as IIfcCartesianPointList2D;
                                    var points = pointsList2D.CoordList;
                                    for (int i = 0; i < points.Count; i++)
                                    {
                                        int y = i;
                                        var moveAxisXOpening = points[i][0] * pXDim;
                                        var moveAxisYOpening = points[i][1] * pYDim;
                                        var vectorMove = moveAxisXOpening + moveAxisYOpening;

                                        if (y == 0)
                                        {
                                            v2[0] = v1[0] + vectorMove.X;
                                            v2[1] = v1[1] + vectorMove.Y;
                                            v2[2] = v1[2] + vectorMove.Z;
                                        }
                                        else if (y == 1)
                                        {
                                            v3[0] = v1[0] + vectorMove.X;
                                            v3[1] = v1[1] + vectorMove.Y;
                                            v3[2] = v1[2] + vectorMove.Z;
                                        }
                                        else if (y == 2)
                                        {
                                            v4[0] = v1[0] + vectorMove.X;
                                            v4[1] = v1[1] + vectorMove.Y;
                                            v4[2] = v1[2] + vectorMove.Z;
                                        }
                                        else if (y == 3)
                                        {
                                            v5[0] = v1[0] + vectorMove.X;
                                            v5[1] = v1[1] + vectorMove.Y;
                                            v5[2] = v1[2] + vectorMove.Z;
                                        }

                                        #region VerticesOpening Last Version
                                        /*
                                        if (pXDim.X != 0)
                                        {
                                            if (pYDim.Y != 0)
                                            {
                                                if (y == 0  )
                                                {
                                                    v2[0] = v1[0] + pXDim.X * points[i][0];
                                                    v2[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v2[2] = v1[2];
                                                }
                                                else if (y == 1)
                                                {
                                                    v3[0] = v1[0] + pXDim.X * points[i][0];
                                                    v3[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v3[2] = v1[2];
                                                }
                                                else if (y == 2)
                                                {
                                                    v4[0] = v1[0] + pXDim.X * points[i][0];
                                                    v4[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v4[2] = v1[2];
                                                }
                                                else if (y == 3)
                                                {
                                                    v5[0] = v1[0] + pXDim.X * points[i][0];
                                                    v5[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v5[2] = v1[2];
                                                }

                                            }
                                            else if (pYDim.Z != 0)
                                            {
                                                if (y == 0)
                                                {
                                                    v2[0] = v1[0] + pXDim.X * points[i][0];
                                                    v2[1] = v1[1];
                                                    v2[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                                else if (y == 1)
                                                {
                                                    v3[0] = v1[0] + pXDim.X * points[i][0];
                                                    v3[1] = v1[1];
                                                    v3[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                                else if (y == 2)
                                                {
                                                    v4[0] = v1[0] + pXDim.X * points[i][0];
                                                    v4[1] = v1[1];
                                                    v4[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                                else if (y == 3)
                                                {
                                                    v5[0] = v1[0] + pXDim.X * points[i][0];
                                                    v5[1] = v1[1];
                                                    v5[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                                
                                            }
                                        }
                                        else if (pXDim.Y != 0)
                                        {
                                            if (pYDim.X != 0)
                                            {
                                                if (y == 0)
                                                {
                                                    v2[0] = v1[0] + pYDim.X * points[i][1];
                                                    v2[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v2[2] = v1[2] ;
                                                }
                                                else if (y == 1)
                                                {
                                                    v3[0] = v1[0] + pYDim.X * points[i][1];
                                                    v3[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v3[2] = v1[2];
                                                }
                                                else if (y == 2)
                                                {
                                                    v4[0] = v1[0] + pYDim.X * points[i][1];
                                                    v4[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v4[2] = v1[2];
                                                }
                                                else if (y == 3)
                                                {
                                                    v5[0] = v1[0] + pYDim.X * points[i][1];
                                                    v5[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v5[2] = v1[2];
                                                }

                                            }
                                            else if (pYDim.Z != 0)
                                            {
                                                if (y == 0)
                                                {
                                                    v2[0] = v1[0];
                                                    v2[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v2[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                                else if (y == 1)
                                                {
                                                    v3[0] = v1[0];
                                                    v3[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v3[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                                else if (y == 2)
                                                {
                                                    v4[0] = v1[0];
                                                    v4[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v4[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                                else if (y == 3)
                                                {
                                                    v5[0] = v1[0];
                                                    v5[1] = v1[1] + pXDim.Y * points[i][0];
                                                    v5[2] = v1[2] + pYDim.Z * points[i][1];
                                                }
                                            }
                                        }
                                        else if (pXDim.Z != 0)
                                        {
                                            if (pYDim.X != 0)
                                            {
                                                if (y == 0)
                                                {
                                                    v2[0] = v1[0] + pYDim.X * points[i][1];
                                                    v2[1] = v1[1];
                                                    v2[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                                else if (y == 1)
                                                {
                                                    v3[0] = v1[0] + pYDim.X * points[i][1];
                                                    v3[1] = v1[1];
                                                    v3[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                                else if (y == 2)
                                                {
                                                    v4[0] = v1[0] + pYDim.X * points[i][1];
                                                    v4[1] = v1[1];
                                                    v4[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                                else if (y == 3)
                                                {
                                                    v5[0] = v1[0] + pYDim.X * points[i][1];
                                                    v5[1] = v1[1];
                                                    v5[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                            }
                                            else if (pYDim.Y != 0)
                                            {
                                                if (y == 0)
                                                {
                                                    v2[0] = v1[0];
                                                    v2[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v2[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                                else if (y == 1)
                                                {
                                                    v3[0] = v1[0];
                                                    v3[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v3[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                                else if (y == 2)
                                                {
                                                    v4[0] = v1[0];
                                                    v4[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v4[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                                else if (y == 3)
                                                {
                                                    v5[0] = v1[0];
                                                    v5[1] = v1[1] + pYDim.Y * points[i][1];
                                                    v5[2] = v1[2] + pXDim.Z * points[i][0];
                                                }
                                            }
                                        }
                                        */
                                        #endregion VerticesOpening
                                    }

                                }
 

                                if (v2[0] <= v3[0] && v2[1] <= v3[1])
                                {
                                    fsd.Vertex1XCoordinate = v2[0];
                                    fsd.Vertex1YCoordinate = v2[1];
                                    fsd.Vertex1ZCoordinate = v2[2];
                                    fsd.Vertex2XCoordinate = v3[0];
                                    fsd.Vertex2YCoordinate = v3[1];
                                    fsd.Vertex2ZCoordinate = v3[2];
                                    fsd.Vertex3XCoordinate = v4[0];
                                    fsd.Vertex3YCoordinate = v4[1];
                                    fsd.Vertex3ZCoordinate = v4[2];
                                    fsd.Vertex4XCoordinate = v5[0];
                                    fsd.Vertex4YCoordinate = v5[1];
                                    fsd.Vertex4ZCoordinate = v5[2];
                                }
                                else
                                {
                                    fsd.Vertex1XCoordinate = v3[0];
                                    fsd.Vertex1YCoordinate = v3[1];
                                    fsd.Vertex1ZCoordinate = v3[2];
                                    fsd.Vertex2XCoordinate = v2[0];
                                    fsd.Vertex2YCoordinate = v2[1];
                                    fsd.Vertex2ZCoordinate = v2[2];
                                    fsd.Vertex3XCoordinate = v5[0];
                                    fsd.Vertex3YCoordinate = v5[1];
                                    fsd.Vertex3ZCoordinate = v5[2];
                                    fsd.Vertex4XCoordinate = v4[0];
                                    fsd.Vertex4YCoordinate = v4[1];
                                    fsd.Vertex4ZCoordinate = v4[2];
                                }
                                if (FenestrationSurfaceDetailed.DuplicateCheck(fsd))
                                {

                                }
                                else
                                {
                                    //FenestrationSurfaceDetailed.Add(fsd);
                                }

                            }
                        }



                        
                        
                    }

                    //IfcMaterialLayer - Material
                    var materialsLayer = model.Instances.OfType<IIfcMaterialLayer>().ToList();
                    foreach (IIfcMaterialLayer materialLayer in materialsLayer)
                    {
                        var material = materialLayer.Material as IIfcMaterial;
                        Material mat = new Material();
                        if (material.AssociatedTo.FirstOrDefault() != null)
                        {
                            mat.Name = $"{material.Name}";
                        }
                        else
                        {
                            mat.Name = $"{material.Name} - {materialLayer.LayerThickness}";
                        }
                        
                        mat.Roughness = RoughnessType.Rough;
                        mat.Thickness = materialLayer.LayerThickness;

                        mat.Conductivity = Convert.ToDouble(Tool.GetMaterialProperty(material, "ThermalConductivity"));
                        mat.Density = Convert.ToDouble(Tool.GetMaterialProperty(material, "MassDensity"));
                        mat.SpecificHeat = Convert.ToDouble(Tool.GetMaterialProperty(material, "SpecificHeatCapacity"));

                        Material.Add(mat);
                        
                    }

                    //IfcMaterialConstituent - Material
                    var materialConstituents = model.Instances.OfType<IIfcMaterialConstituent>().ToList();
                    foreach (IIfcMaterialConstituent materialConstituent in materialConstituents)
                    {
                        var material = materialConstituent.Material as IIfcMaterial;
                        Material mat = new Material();
                        mat.Name = $"{material.Name}";
                        mat.Roughness = RoughnessType.Rough;
                        mat.Thickness = 0.03;
                        mat.Conductivity = Convert.ToDouble(Tool.GetMaterialProperty(material, "ThermalConductivity"));
                        mat.Density = Convert.ToDouble(Tool.GetMaterialProperty(material, "MassDensity"));
                        mat.SpecificHeat = Convert.ToDouble(Tool.GetMaterialProperty(material, "SpecificHeatCapacity"));

                        Material.Add(mat);
                    }

                    //IfcWindow - WindowMaterial:SimpleGlazingSystem - Construction
                    var windows = model.Instances.OfType<IIfcWindow>().ToList();
                    foreach (IIfcWindow window in windows)
                    {
                        WindowMaterialSimpleGlazingSystem simpleGlazingSystem = new WindowMaterialSimpleGlazingSystem();
                        var DefinedBy = window.IsTypedBy.FirstOrDefault().RelatingType.Name;
                        var windowType = model.Instances.FirstOrDefault<IIfcWindowType>(d => d.Name == DefinedBy);

                        simpleGlazingSystem.Name = windowType.Name;


                        if (Tool.GetWindowTypeProperty(windowType, "Heat Transfer Coefficient (U)") != null)
                        {
                            simpleGlazingSystem.UFactor = Convert.ToDouble(Tool.GetWindowTypeProperty(windowType, "Heat Transfer Coefficient (U)").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        }
                        if (Tool.GetWindowTypeProperty(windowType, "Solar Heat Gain Coefficient") != null)
                        {
                            simpleGlazingSystem.SolarHeatGainCoefficient = Convert.ToDouble(Tool.GetWindowTypeProperty(windowType, "Solar Heat Gain Coefficient").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        }
                        if (Tool.GetWindowTypeProperty(windowType, "Visual Light Transmittance") != null)
                        {
                            simpleGlazingSystem.VisibleTransmittance = Convert.ToDouble(Tool.GetWindowTypeProperty(windowType, "Visual Light Transmittance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                        }

                        WindowMaterialSimpleGlazingSystem.Add(simpleGlazingSystem);
                        //------
                        Construction construction = new Construction();
                        construction.Name = windowType.Name;
                        construction.OutsideLayer = windowType.Name;
                        Construction.Add(construction);
                    }

                    //IfcMaterialLayerSetUsage - Construction
                    var relAssociatesMaterials = model.Instances.OfType<IIfcRelAssociatesMaterial>().ToList();
                    foreach (IIfcRelAssociatesMaterial relAssociatesMaterial in relAssociatesMaterials)
                    {
                        var relatingMaterial = relAssociatesMaterial.RelatingMaterial;
                        var relatedObject = relAssociatesMaterial.RelatedObjects.FirstOrDefault() as IIfcBuildingElement;
                        
                        if (relatingMaterial is IIfcMaterial)
                        {
                            var _relatingMaterial = relatingMaterial as IIfcMaterial;
                            Construction construction = new Construction();
                            construction.Name = _relatingMaterial.Name;
                            construction.OutsideLayer = _relatingMaterial.Name;
                            Construction.Add(construction);
                        }
                        else if (relatingMaterial is IIfcMaterialLayerSetUsage)
                        {
                            var _relatingMaterial = relatingMaterial as IIfcMaterialLayerSetUsage;
                            Construction construction = new Construction();
                            construction.Name = _relatingMaterial.ForLayerSet.LayerSetName;
                            var materialLayers = _relatingMaterial.ForLayerSet.MaterialLayers.ToList();
                            for (int i = 0; i < materialLayers.Count; i++)
                            {
                                int y = i + 1;

                                switch (y)
                                {
                                    case 1:
                                        construction.OutsideLayer = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 2:
                                        construction.Layer2 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 3:
                                        construction.Layer3 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 4:
                                        construction.Layer4 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 5:
                                        construction.Layer5 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 6:
                                        construction.Layer6 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 7:
                                        construction.Layer7 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 8:
                                        construction.Layer8 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 9:
                                        construction.Layer9 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    case 10:
                                        construction.Layer10 = $"{materialLayers[i].Material.Name} - {materialLayers[i].LayerThickness}";
                                        break;
                                    default:
                                        break;
                                }

                                
                                
                            }
                            Construction.Add(construction);
                        }
                        else if (relatingMaterial is IIfcMaterialConstituentSet)
                        {
                            //todo: find a way to label the mane of the construction.
                            
                        }
                    }


                }

                #region REVIT Version
                else
                { 
                    //----IFC SITE - Building
                    Building building = new Building();
                    var site = model.Instances.OfType<IIfcSite>().FirstOrDefault();
                    building.Name = site.GlobalId.ToString();
                    var north = model.Instances.OfType<IIfcGeometricRepresentationContext>().FirstOrDefault();
                    var xx = north.TrueNorth.X;
                    var yy = north.TrueNorth.Y;
                    var angulo = Math.Atan2(xx, yy) * (180 / Math.PI);
                    building.NorthAxis = -1 * angulo;
                    building.Terrain = TerrainType.City;
                    building.LoadsConvergenceToleranceValue = 0.04;
                    building.TemperatureConvergenceToleranceValue = 0.4;
                    building.SolarDistribution = SolarDistributionType.FullExterior;
                    building.MaximumNumberofWarmupDays = 25;
                    building.MinimumNumberofWarmupDays = 6;
                    Building.Add(building);

                   

                    //-----IFC SPACE
                    var spaces = model.Instances.OfType<IIfcSpace>().ToList();
                    foreach (var space in spaces)
                    {
                        Zone z = new Zone();
                        z.Name = space.GlobalId;
                        //---zone properties

                        z.DirectionofRelativeNorth = 0;
                        z.XOrigin = 0;
                        z.YOrigin = 0;
                        z.ZOrigin = 0;
                        z.Type = 1;
                        z.Multiplier = 1;

                        if (Tool.GetSpaceProperty(space, "Area").ToString() == "true")
                        {
                            z.PartofTotalFloorArea = PartofTotalAreEnum.Yes;

                        }
                        else
                        {
                            z.PartofTotalFloorArea = PartofTotalAreEnum.No;
                        }
                        Zone.Add(z);
                        Console.WriteLine(z.Name);
                        
                        /*
                        //------
                        ZoneInfiltrationDesignFlowRate dfr = new ZoneInfiltrationDesignFlowRate();
                        // fZoneArea in L/(seg.m2)
                        double fZoneArea = Convert.ToDouble(Tool.GetSpaceProperty(space, "Calculated Supply Airflow per area"), CultureInfo.InvariantCulture.NumberFormat);
                        dfr.Name = $"{space.GlobalId}-ZInfil";
                        dfr.ZoneorZoneListName = space.GlobalId;
                        dfr.ScheduleName = "Always ON";
                        dfr.DesignFlowRateCalculationMethod = "Flow/Area";
                        dfr.FlowperZoneFloorArea = fZoneArea / 1000;
                        dfr.ConstantTermCoefficient = 0;
                        dfr.TemperatureTermCoefficient = 0;
                        dfr.VelocityTermCoefficient = 0.2237;
                        dfr.VelocitySquaredTermCoefficient = 0;
                        ZoneInfiltrationDesignFlowRate.Add(dfr);
                        */
                        /*
                        //----
                        ZoneVentilationDesignFlowRate vdfr = new ZoneVentilationDesignFlowRate();
                        vdfr.Name = $"{space.GlobalId}-Vent";
                        vdfr.ZoneorZoneListName = space.GlobalId;
                        vdfr.ScheduleName = "Always ON";
                        vdfr.DesignFlowRateCalculationMethod = "Flow/Area";
                        vdfr.FlowRateperZoneFloorArea = fZoneArea / 1000;
                        vdfr.VentilationType = "Natural";
                        vdfr.FanTotalEfficiency = 1;
                        vdfr.ConstantTermCoefficient = 1;
                        vdfr.MinimumIndoorTemperature = 20;
                        vdfr.MaximumIndoorTemperature = 25;
                        vdfr.DeltaTemperature = 0.5;
                        vdfr.MinimumOutdoorTemperature = 10;
                        vdfr.MaximumOutdoorTemperature = 40;
                        vdfr.MaximumWindSpeed = 10;
                        ZoneVentilationDesignFlowRate.Add(vdfr);
                        */
                        /*
                        //----
                        ZoneControlThermostat t = new ZoneControlThermostat();
                        t.Name = $"ZCT-{space.GlobalId}";
                        t.ZoneorZoneListName = space.GlobalId;
                        t.ControlTypeScheduleName = "Always 4";
                        t.Control1Name = $"T-{space.GlobalId}";
                        t.Control1ObjectType = "ThermostatSetpoint:DualSetpoint";
                        ZoneControlThermostat.Add(t);
                        */
                        /*
                        //-----
                        ThermostatSetpointDualSetpoint tds = new ThermostatSetpointDualSetpoint();
                        tds.Name = $"T-{space.GlobalId}";
                        tds.HeatingSetpointTemperatureScheduleName = $"HeatingSP-{space.GlobalId}";
                        tds.CoolingSetpointTemperatureScheduleName = $"CoolingSP-{space.GlobalId}";
                        ThermostatSetpointDualSetpoint.Add(tds);
                        */
                        /*
                        //-----
                        ZoneHVACIdealLoadsAirSystem las = new ZoneHVACIdealLoadsAirSystem();
                        las.Name = $"LAS-{space.GlobalId}";
                        las.ZoneSupplyAirNodeName = $"Supply-{space.GlobalId}";
                        las.MaximumHeatingSupplyAirTemperature = 50;
                        las.MinimumCoolingSupplyAirTemperature = 13;
                        las.MaximumHeatingSupplyAirHumidityRatio = 0.0156;
                        las.MinimumCoolingSupplyAirHumidityRatio = 0.0077;
                        las.HeatingLimit = "NoLimit";
                        las.CoolingLimit = "NoLimit";
                        las.DehumidificationControlType = "ConstantSensibleHeatRatio";
                        las.CoolingSensibleHeatRatio = 0.7;
                        las.HumidificationControlType = "ConstantSupplyHumidityRatio";
                        las.DemandControlledVentilationType = "None";
                        las.OutdoorAirEconomizerType = "NoEconomizer";
                        las.HeatRecoveryType = "None";
                        las.SensibleHeatRecoveryEffectiveness = 0.7;
                        las.LatentHeatRecoveryEffectiveness = 0.65;
                        ZoneHVACIdealLoadsAirSystem.Add(las);
                        */
                        /*
                        //----
                        ZoneHVACEquipmentList el = new ZoneHVACEquipmentList();
                        el.Name = $"EqL-{space.GlobalId}";
                        el.LoadDistributionScheme = "SequentialLoad";
                        el.ZoneEquipment1ObjectType = "ZoneHVAC:IdealLoadsAirSystem";
                        el.ZoneEquipment1Name = $"LAS-{space.GlobalId}";
                        el.ZoneEquipment1CoolingSequence = 1;
                        el.ZoneEquipment1HeatingorNoLoadSequence = 1;
                        ZoneHVACEquipmentList.Add(el);
                        */
                        /*
                        //-----
                        ZoneHVACEquipmentConnections ec = new ZoneHVACEquipmentConnections();
                        ec.Name = $"{space.GlobalId}";
                        ec.ConditioningEquipmentListName = $"Eql-{space.GlobalId}";
                        ec.AirInletNodeorNodeListName = $"Supply-{space.GlobalId}";
                        ec.AirNodeName = $"Node-{space.GlobalId}";
                        ec.ReturnAirNodeorNodeListName = $"Return-{space.GlobalId}";
                        ZoneHVACEquipmentConnections.Add(ec);
                        */

                        //----
                        ScheduleCompact ps = new ScheduleCompact();
                        var id = space.GlobalId.ToString();
                        var numPeople = Tool.GetSpaceProperty(space, "Sensible Heat Gain per person");

                        ps.Name = $"Activity-{id}";
                        ps.ScheduleTypeLimitsName = "Any Number";
                        ps.Field1 = "Through: 12/31";
                        ps.Field2 = "For: AllDays";
                        ps.Field3 = "Until: 24:00";
                        ps.Field4 = $"{numPeople}";
                        ScheduleCompact.Add(ps);

                        /*
                        //-------
                        People p = new People();
                        p.Name = $"People-{id}";
                        p.ZoneorZoneListName = id;
                        p.NumberofpeopleScheduleName = $"Fraction-{id}";
                        p.NumberofPeopleCalculationMethod = NumberofPeopleCalculationMethodType.People;
                        p.NumberofPeople = Convert.ToDouble(numPeople, CultureInfo.InvariantCulture.NumberFormat);
                        p.FractionRadiant = 0.3;
                        p.ActivityLevelScheduleName = $"Activity-{id}";
                        People.Add(p);
                        //InternalGainsDAT.PeopleDAT.Add(p);
                        //InternalGains.People.Collect(p);
                        */
                        /*
                        //-------
                        Lights l = new Lights();
                        var lightingLevel = Convert.ToDouble(Tool.GetSpaceProperty(space, "Specified Lighting Load"), CultureInfo.InvariantCulture.NumberFormat);
                        l.Name = $"Lights-{id}";
                        l.ZoneorZoneListName = id;
                        l.ScheduleName = $"Fraction-{id}";
                        l.DesignLevelCalculationMethod = DesignLevelCalculationMethodLightingLevelType.LightingLevel;
                        l.LightingLevel = lightingLevel;
                        l.ReturnAirFraction = 0.2;
                        l.FractionRadiant = 0.59;
                        l.FractionVisible = 0.2;
                        l.FractionReplaceable = 0;
                        l.EndUseSubcategory = "GeneralLights";
                        Lights.Add(l);
                        */
                        /*
                        //------
                        ElectricEquipments ee = new ElectricEquipments();
                        var ELevel = Convert.ToDouble(Tool.GetSpaceProperty(space, "Specified Power Load"), CultureInfo.InvariantCulture.NumberFormat);
                        ee.Name = $"ElectricE-{id}";
                        ee.ZoneorZoneListName = id;
                        ee.ScheduleName = $"Fraction-{id}";
                        ee.DesignLevelCalculationMethod = DesignLevelCalculationMethodElectricEquipmentType.EquipmentLevel;
                        ee.DesignLevel = ELevel;
                        ee.FractionLatent = 0;
                        ee.FractionRadiant = 0.3;
                        ee.FractionLost = 0;
                        ee.EndUseSubcategory = "General";
                        ElectricEquipments.Add(ee);
                        */
                    }


                    //---IFC BUILDING
                    SiteLocation sl = new SiteLocation();
                    var location = model.Instances.OfType<IIfcBuilding>().FirstOrDefault();
                    sl.Name = location.BuildingAddress.Country;
                    var place = model.Instances.OfType<IIfcSite>().FirstOrDefault();
                    sl.Latitude = place.RefLatitude.Value.AsDouble;
                    sl.Longitude = place.RefLongitude.Value.AsDouble;
                    sl.TimeZone = 1;
                    sl.Elevation = place.RefElevation.Value;
                    SiteLocation.Add(sl);



                    //---IFC REL SPACE BOUNDARY
                    var relSpaceBoundaries = model.Instances.OfType<IIfcRelSpaceBoundary>().ToList();
                    foreach (IIfcRelSpaceBoundary relSpaceBoundary in relSpaceBoundaries)
                    {

                        //-------Get points of the boundary plane
                        var connectionGeometry = relSpaceBoundary.ConnectionGeometry;
                        var _connectionGeometry = connectionGeometry as IIfcConnectionSurfaceGeometry;
                        var surfaceonRelatingElement = _connectionGeometry.SurfaceOnRelatingElement;
                        var _surfaceonRelatingElement = surfaceonRelatingElement as IIfcCurveBoundedPlane;
                        var outerBoundary = _surfaceonRelatingElement.OuterBoundary;
                        var basisSurface = _surfaceonRelatingElement.BasisSurface;
                        var _basisSurface = basisSurface as IIfcPlane;
                        var position = _basisSurface.Position;
                        var internalOrExternalBoundary = relSpaceBoundary.InternalOrExternalBoundary.ToString();

                        double[] planeLocation = new double[3];
                        planeLocation[0] = Math.Round(position.Location.X, 2);
                        planeLocation[1] = Math.Round(position.Location.Y, 2);
                        planeLocation[2] = Math.Round(position.Location.Z, 2);


                        double[] refDirectionPlane0 = new double[3];
                        refDirectionPlane0[0] = position.RefDirection.X;
                        refDirectionPlane0[1] = position.RefDirection.Y;
                        refDirectionPlane0[2] = position.RefDirection.Z;

                        double[] planeAxis = new double[3];
                        planeAxis[0] = position.Axis.X;
                        planeAxis[1] = position.Axis.Y;
                        planeAxis[2] = position.Axis.Z;

                        bool AxisX = false;
                        bool AxisY = false;
                        bool AxisZ = false;

                        if (planeAxis[0] != 0)
                        {
                            AxisX = true;
                        }
                        if (planeAxis[1] != 0)
                        {
                            AxisY = true;
                        }
                        if (planeAxis[2] != 0)
                        {
                            AxisZ = true;
                        }




                        var _outerBoundary = outerBoundary as IIfcPolyline;
                        var Points = _outerBoundary.Points.ToList();

                        double[] v2 = new double[3];
                        double[] v3 = new double[3];
                        double[] v4 = new double[3];
                        double[] v5 = new double[3];
                        double[] v6 = new double[3];
                        double[] v7 = new double[3];
                        double[] v8 = new double[3];
                        double[] v9 = new double[3];
                        double[] v10 = new double[3];
                        double[] v11 = new double[3];
                        double[] v12 = new double[3];
                        double[] v13 = new double[3];
                        double[] v14 = new double[3];
                        double[] v15 = new double[3];
                        double[] v16 = new double[3];

                        var pointsCount = Points.Count;

                        for (int i = 1; i < pointsCount; i++)
                        {
                            int y = i;
                            //If RefDirection is (1,0,0)
                            if (refDirectionPlane0[0] != 0 && refDirectionPlane0[1] == 0 && refDirectionPlane0[2] == 0)
                            {
                                if (AxisY)
                                {
                                    if (y == 1)
                                    {
                                        v2[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v2[1] = planeLocation[1];
                                        v2[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 2)
                                    {
                                        v3[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v3[1] = planeLocation[1];
                                        v3[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 3)
                                    {
                                        v4[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v4[1] = planeLocation[1];
                                        v4[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 4)
                                    {
                                        v5[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v5[1] = planeLocation[1];
                                        v5[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 5)
                                    {
                                        v6[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v6[1] = planeLocation[1];
                                        v6[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 6)
                                    {
                                        v7[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v7[1] = planeLocation[1];
                                        v7[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 7)
                                    {
                                        v8[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v8[1] = planeLocation[1];
                                        v8[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 8)
                                    {
                                        v9[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v9[1] = planeLocation[1];
                                        v9[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 9)
                                    {
                                        v10[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v10[1] = planeLocation[1];
                                        v10[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 10)
                                    {
                                        v11[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v11[1] = planeLocation[1];
                                        v11[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 11)
                                    {
                                        v12[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v12[1] = planeLocation[1];
                                        v12[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 12)
                                    {
                                        v13[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v13[1] = planeLocation[1];
                                        v13[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 13)
                                    {
                                        v14[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v14[1] = planeLocation[1];
                                        v14[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 14)
                                    {
                                        v15[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v15[1] = planeLocation[1];
                                        v15[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 15)
                                    {
                                        v16[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v16[1] = planeLocation[1];
                                        v16[2] = planeLocation[2] + Points[i].Y;
                                    }

                                }
                                if (AxisZ)
                                {
                                    if (y == 1)
                                    {
                                        v2[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v2[1] = planeLocation[1] + Points[i].Y;
                                        v2[2] = planeLocation[2];
                                    }
                                    if (y == 2)
                                    {
                                        v3[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v3[1] = planeLocation[1] + Points[i].Y;
                                        v3[2] = planeLocation[2];
                                    }
                                    if (y == 3)
                                    {
                                        v4[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v4[1] = planeLocation[1] + Points[i].Y;
                                        v4[2] = planeLocation[2];
                                    }
                                    if (y == 4)
                                    {
                                        v5[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v5[1] = planeLocation[1] + Points[i].Y;
                                        v5[2] = planeLocation[2];
                                    }
                                    if (y == 5)
                                    {
                                        v6[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v6[1] = planeLocation[1] + Points[i].Y;
                                        v6[2] = planeLocation[2];
                                    }
                                    if (y == 6)
                                    {
                                        v7[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v7[1] = planeLocation[1] + Points[i].Y;
                                        v7[2] = planeLocation[2];
                                    }
                                    if (y == 7)
                                    {
                                        v8[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v8[1] = planeLocation[1] + Points[i].Y;
                                        v8[2] = planeLocation[2];
                                    }
                                    if (y == 8)
                                    {
                                        v9[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v9[1] = planeLocation[1] + Points[i].Y;
                                        v9[2] = planeLocation[2];
                                    }
                                    if (y == 9)
                                    {
                                        v10[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v10[1] = planeLocation[1] + Points[i].Y;
                                        v10[2] = planeLocation[2];
                                    }
                                    if (y == 10)
                                    {
                                        v11[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v11[1] = planeLocation[1] + Points[i].Y;
                                        v11[2] = planeLocation[2];
                                    }
                                    if (y == 11)
                                    {
                                        v12[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v12[1] = planeLocation[1] + Points[i].Y;
                                        v12[2] = planeLocation[2];
                                    }
                                    if (y == 12)
                                    {
                                        v13[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v13[1] = planeLocation[1] + Points[i].Y;
                                        v13[2] = planeLocation[2];
                                    }
                                    if (y == 13)
                                    {
                                        v14[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v14[1] = planeLocation[1] + Points[i].Y;
                                        v14[2] = planeLocation[2];
                                    }
                                    if (y == 14)
                                    {
                                        v15[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v15[1] = planeLocation[1] + Points[i].Y;
                                        v15[2] = planeLocation[2];
                                    }
                                    if (y == 15)
                                    {
                                        v16[0] = planeLocation[0] + refDirectionPlane0[0] * Points[i].X;
                                        v16[1] = planeLocation[1] + Points[i].Y;
                                        v16[2] = planeLocation[2];
                                    }

                                }

                            }
                            //if RefDirection is (0,1,0)
                            else if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] != 0 && refDirectionPlane0[2] == 0)
                            {
                                if (AxisX)
                                {
                                    if (y == 1)
                                    {
                                        v2[0] = planeLocation[0];
                                        v2[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v2[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 2)
                                    {
                                        v3[0] = planeLocation[0];
                                        v3[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v3[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 3)
                                    {
                                        v4[0] = planeLocation[0];
                                        v4[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v4[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 4)
                                    {
                                        v5[0] = planeLocation[0];
                                        v5[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v5[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 5)
                                    {
                                        v6[0] = planeLocation[0];
                                        v6[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v6[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 6)
                                    {
                                        v7[0] = planeLocation[0];
                                        v7[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v7[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 7)
                                    {
                                        v8[0] = planeLocation[0];
                                        v8[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v8[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 8)
                                    {
                                        v9[0] = planeLocation[0];
                                        v9[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v9[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 9)
                                    {
                                        v10[0] = planeLocation[0];
                                        v10[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v10[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 10)
                                    {
                                        v11[0] = planeLocation[0];
                                        v11[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v11[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 11)
                                    {
                                        v12[0] = planeLocation[0];
                                        v12[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v12[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 12)
                                    {
                                        v13[0] = planeLocation[0];
                                        v13[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v13[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 13)
                                    {
                                        v14[0] = planeLocation[0];
                                        v14[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v14[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 14)
                                    {
                                        v15[0] = planeLocation[0];
                                        v15[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v15[2] = planeLocation[2] + Points[i].Y;
                                    }
                                    if (y == 15)
                                    {
                                        v16[0] = planeLocation[0];
                                        v16[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v16[2] = planeLocation[2] + Points[i].Y;
                                    }
                                }
                                if (AxisZ)
                                {
                                    if (y == 1)
                                    {
                                        v2[0] = planeLocation[0] + Points[i].Y;
                                        v2[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v2[2] = planeLocation[2];
                                    }
                                    if (y == 2)
                                    {
                                        v3[0] = planeLocation[0] + Points[i].Y;
                                        v3[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v3[2] = planeLocation[2];
                                    }
                                    if (y == 3)
                                    {
                                        v4[0] = planeLocation[0] + Points[i].Y;
                                        v4[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v4[2] = planeLocation[2];
                                    }
                                    if (y == 4)
                                    {
                                        v5[0] = planeLocation[0] + Points[i].Y;
                                        v5[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v5[2] = planeLocation[2];
                                    }
                                    if (y == 5)
                                    {
                                        v6[0] = planeLocation[0] + Points[i].Y;
                                        v6[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v6[2] = planeLocation[2];
                                    }
                                    if (y == 6)
                                    {
                                        v7[0] = planeLocation[0] + Points[i].Y;
                                        v7[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v7[2] = planeLocation[2];
                                    }
                                    if (y == 7)
                                    {
                                        v8[0] = planeLocation[0] + Points[i].Y;
                                        v8[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v8[2] = planeLocation[2];
                                    }
                                    if (y == 8)
                                    {
                                        v9[0] = planeLocation[0] + Points[i].Y;
                                        v9[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v9[2] = planeLocation[2];
                                    }
                                    if (y == 9)
                                    {
                                        v10[0] = planeLocation[0] + Points[i].Y;
                                        v10[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v10[2] = planeLocation[2];
                                    }
                                    if (y == 10)
                                    {
                                        v11[0] = planeLocation[0] + Points[i].Y;
                                        v11[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v11[2] = planeLocation[2];
                                    }
                                    if (y == 11)
                                    {
                                        v12[0] = planeLocation[0] + Points[i].Y;
                                        v12[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v12[2] = planeLocation[2];
                                    }
                                    if (y == 12)
                                    {
                                        v13[0] = planeLocation[0] + Points[i].Y;
                                        v13[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v13[2] = planeLocation[2];
                                    }
                                    if (y == 13)
                                    {
                                        v14[0] = planeLocation[0] + Points[i].Y;
                                        v14[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v14[2] = planeLocation[2];
                                    }
                                    if (y == 14)
                                    {
                                        v15[0] = planeLocation[0] + Points[i].Y;
                                        v15[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v15[2] = planeLocation[2];
                                    }
                                    if (y == 15)
                                    {
                                        v16[0] = planeLocation[0] + Points[i].Y;
                                        v16[1] = planeLocation[1] + refDirectionPlane0[1] * Points[i].X;
                                        v16[2] = planeLocation[2];
                                    }
                                }
                            }
                            //if RefDirection is (0,0,1)
                            else if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] == 0 && refDirectionPlane0[2] != 0)
                            {
                                if (AxisX)
                                {
                                    if (y == 1)
                                    {
                                        v2[0] = planeLocation[0];
                                        v2[1] = planeLocation[1] + Points[i].Y;
                                        v2[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 2)
                                    {
                                        v3[0] = planeLocation[0];
                                        v3[1] = planeLocation[1] + Points[i].Y;
                                        v3[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 3)
                                    {
                                        v4[0] = planeLocation[0];
                                        v4[1] = planeLocation[1] + Points[i].Y;
                                        v4[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 4)
                                    {
                                        v5[0] = planeLocation[0];
                                        v5[1] = planeLocation[1] + Points[i].Y;
                                        v5[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 5)
                                    {
                                        v6[0] = planeLocation[0];
                                        v6[1] = planeLocation[1] + Points[i].Y;
                                        v6[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 6)
                                    {
                                        v7[0] = planeLocation[0];
                                        v7[1] = planeLocation[1] + Points[i].Y;
                                        v7[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 7)
                                    {
                                        v8[0] = planeLocation[0];
                                        v8[1] = planeLocation[1] + Points[i].Y;
                                        v8[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 8)
                                    {
                                        v9[0] = planeLocation[0];
                                        v9[1] = planeLocation[1] + Points[i].Y;
                                        v9[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 9)
                                    {
                                        v10[0] = planeLocation[0];
                                        v10[1] = planeLocation[1] + Points[i].Y;
                                        v10[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 10)
                                    {
                                        v11[0] = planeLocation[0];
                                        v11[1] = planeLocation[1] + Points[i].Y;
                                        v11[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 11)
                                    {
                                        v12[0] = planeLocation[0];
                                        v12[1] = planeLocation[1] + Points[i].Y;
                                        v12[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 12)
                                    {
                                        v13[0] = planeLocation[0];
                                        v13[1] = planeLocation[1] + Points[i].Y;
                                        v13[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 13)
                                    {
                                        v14[0] = planeLocation[0];
                                        v14[1] = planeLocation[1] + Points[i].Y;
                                        v14[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 14)
                                    {
                                        v15[0] = planeLocation[0];
                                        v15[1] = planeLocation[1] + Points[i].Y;
                                        v15[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 15)
                                    {
                                        v16[0] = planeLocation[0];
                                        v16[1] = planeLocation[1] + Points[i].Y;
                                        v16[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                }
                                if (AxisY)
                                {
                                    if (y == 1)
                                    {
                                        v2[0] = planeLocation[0] + Points[i].Y;
                                        v2[1] = planeLocation[1];
                                        v2[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 2)
                                    {
                                        v3[0] = planeLocation[0] + Points[i].Y;
                                        v3[1] = planeLocation[1];
                                        v3[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 3)
                                    {
                                        v4[0] = planeLocation[0] + Points[i].Y;
                                        v4[1] = planeLocation[1];
                                        v4[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 4)
                                    {
                                        v5[0] = planeLocation[0] + Points[i].Y;
                                        v5[1] = planeLocation[1];
                                        v5[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 5)
                                    {
                                        v6[0] = planeLocation[0] + Points[i].Y;
                                        v6[1] = planeLocation[1];
                                        v6[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 6)
                                    {
                                        v7[0] = planeLocation[0] + Points[i].Y;
                                        v7[1] = planeLocation[1];
                                        v7[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 7)
                                    {
                                        v8[0] = planeLocation[0] + Points[i].Y;
                                        v8[1] = planeLocation[1];
                                        v8[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 8)
                                    {
                                        v9[0] = planeLocation[0] + Points[i].Y;
                                        v9[1] = planeLocation[1];
                                        v9[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 9)
                                    {
                                        v10[0] = planeLocation[0] + Points[i].Y;
                                        v10[1] = planeLocation[1];
                                        v10[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 10)
                                    {
                                        v11[0] = planeLocation[0] + Points[i].Y;
                                        v11[1] = planeLocation[1];
                                        v11[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 11)
                                    {
                                        v12[0] = planeLocation[0] + Points[i].Y;
                                        v12[1] = planeLocation[1];
                                        v12[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 12)
                                    {
                                        v13[0] = planeLocation[0] + Points[i].Y;
                                        v13[1] = planeLocation[1];
                                        v13[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 13)
                                    {
                                        v14[0] = planeLocation[0] + Points[i].Y;
                                        v14[1] = planeLocation[1];
                                        v14[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X;
                                    }
                                    if (y == 14)
                                    {
                                        v15[0] = planeLocation[0] + Points[i].Y;
                                        v15[1] = planeLocation[1];
                                        v15[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                    if (y == 15)
                                    {
                                        v16[0] = planeLocation[0] + Points[i].Y;
                                        v16[1] = planeLocation[1];
                                        v16[2] = planeLocation[2] + refDirectionPlane0[2] * Points[i].X; ;
                                    }
                                }
                            }


                        }

                        // Define points in counterclock wise
                        double[] p2 = new double[3];
                        double[] p3 = new double[3];
                        double[] p4 = new double[3];
                        double[] p5 = new double[3];
                        double[] p6 = new double[3];
                        double[] p7 = new double[3];
                        double[] p8 = new double[3];
                        double[] p9 = new double[3];
                        double[] p10 = new double[3];
                        double[] p11 = new double[3];
                        double[] p12 = new double[3];
                        double[] p13 = new double[3];
                        double[] p14 = new double[3];
                        double[] p15 = new double[3];
                        double[] p16 = new double[3];

                        p2[0] = v2[0];
                        p2[1] = v2[1];
                        p2[2] = v2[2];

                        p3[0] = v3[0];
                        p3[1] = v3[1];
                        p3[2] = v3[2];

                        p4[0] = v4[0];
                        p4[1] = v4[1];
                        p4[2] = v4[2];

                        p5[0] = v5[0];
                        p5[1] = v5[1];
                        p5[2] = v5[2];

                        p6[0] = v6[0];
                        p6[1] = v6[1];
                        p6[2] = v6[2];

                        p7[0] = v7[0];
                        p7[1] = v7[1];
                        p7[2] = v7[2];

                        p8[0] = v8[0];
                        p8[1] = v8[1];
                        p8[2] = v8[2];

                        p9[0] = v9[0];
                        p9[1] = v9[1];
                        p9[2] = v9[2];

                        p10[0] = v10[0];
                        p10[1] = v10[1];
                        p10[2] = v10[2];

                        p11[0] = v11[0];
                        p11[1] = v11[1];
                        p11[2] = v11[2];

                        p12[0] = v12[0];
                        p12[1] = v12[1];
                        p12[2] = v12[2];

                        p13[0] = v13[0];
                        p13[1] = v13[1];
                        p13[2] = v13[2];

                        p14[0] = v14[0];
                        p14[1] = v14[1];
                        p14[2] = v14[2];

                        p15[0] = v15[0];
                        p15[1] = v15[1];
                        p15[2] = v15[2];

                        p16[0] = v16[0];
                        p16[1] = v16[1];
                        p16[2] = v16[2];


                        //If RefDirection is (0,1,0)
                        if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] != 0 && refDirectionPlane0[2] == 0)
                        {
                            if (AxisZ)
                            {
                                if (pointsCount - 1 == 4)
                                {
                                    p2[0] = v4[0];
                                    p2[1] = v4[1];
                                    p2[2] = v4[2];

                                    p3[0] = v3[0];
                                    p3[1] = v3[1];
                                    p3[2] = v3[2];

                                    p4[0] = v2[0];
                                    p4[1] = v2[1];
                                    p4[2] = v2[2];
                                }
                                if (pointsCount - 1 == 5)
                                {
                                    p2[0] = v5[0];
                                    p2[1] = v5[1];
                                    p2[2] = v5[2];

                                    p3[0] = v4[0];
                                    p3[1] = v4[1];
                                    p3[2] = v4[2];

                                    p4[0] = v3[0];
                                    p4[1] = v3[1];
                                    p4[2] = v3[2];

                                    p5[0] = v2[0];
                                    p5[1] = v2[1];
                                    p5[2] = v2[2];
                                }
                                if (pointsCount - 1 == 6)
                                {
                                    p2[0] = v6[0];
                                    p2[1] = v6[1];
                                    p2[2] = v6[2];

                                    p3[0] = v5[0];
                                    p3[1] = v5[1];
                                    p3[2] = v5[2];

                                    p4[0] = v4[0];
                                    p4[1] = v4[1];
                                    p4[2] = v4[2];

                                    p5[0] = v3[0];
                                    p5[1] = v3[1];
                                    p5[2] = v3[2];

                                    p6[0] = v2[0];
                                    p6[1] = v2[1];
                                    p6[2] = v2[2];
                                }
                                if (pointsCount - 1 == 7)
                                {
                                    p2[0] = v7[0];
                                    p2[1] = v7[1];
                                    p2[2] = v7[2];

                                    p3[0] = v6[0];
                                    p3[1] = v6[1];
                                    p3[2] = v6[2];

                                    p4[0] = v5[0];
                                    p4[1] = v5[1];
                                    p4[2] = v5[2];

                                    p5[0] = v4[0];
                                    p5[1] = v4[1];
                                    p5[2] = v4[2];

                                    p6[0] = v3[0];
                                    p6[1] = v3[1];
                                    p6[2] = v3[2];

                                    p7[0] = v2[0];
                                    p7[1] = v2[1];
                                    p7[2] = v2[2];
                                }
                                if (pointsCount - 1 == 8)
                                {
                                    p2[0] = v8[0];
                                    p2[1] = v8[1];
                                    p2[2] = v8[2];

                                    p3[0] = v7[0];
                                    p3[1] = v7[1];
                                    p3[2] = v7[2];

                                    p4[0] = v6[0];
                                    p4[1] = v6[1];
                                    p4[2] = v6[2];

                                    p5[0] = v5[0];
                                    p5[1] = v5[1];
                                    p5[2] = v5[2];

                                    p6[0] = v4[0];
                                    p6[1] = v4[1];
                                    p6[2] = v4[2];

                                    p7[0] = v3[0];
                                    p7[1] = v3[1];
                                    p7[2] = v3[2];

                                    p8[0] = v2[0];
                                    p8[1] = v2[1];
                                    p8[2] = v2[2];
                                }
                                if (pointsCount - 1 == 9)
                                {
                                    p2[0] = v9[0];
                                    p2[1] = v9[1];
                                    p2[2] = v9[2];

                                    p3[0] = v8[0];
                                    p3[1] = v8[1];
                                    p3[2] = v8[2];

                                    p4[0] = v7[0];
                                    p4[1] = v7[1];
                                    p4[2] = v7[2];

                                    p5[0] = v6[0];
                                    p5[1] = v6[1];
                                    p5[2] = v6[2];

                                    p6[0] = v5[0];
                                    p6[1] = v5[1];
                                    p6[2] = v5[2];

                                    p7[0] = v4[0];
                                    p7[1] = v4[1];
                                    p7[2] = v4[2];

                                    p8[0] = v3[0];
                                    p8[1] = v3[1];
                                    p8[2] = v3[2];

                                    p9[0] = v2[0];
                                    p9[1] = v2[1];
                                    p9[2] = v2[2];
                                }
                                if (pointsCount - 1 == 10)
                                {
                                    p2[0] = v10[0];
                                    p2[1] = v10[1];
                                    p2[2] = v10[2];

                                    p3[0] = v9[0];
                                    p3[1] = v9[1];
                                    p3[2] = v9[2];

                                    p4[0] = v8[0];
                                    p4[1] = v8[1];
                                    p4[2] = v8[2];

                                    p5[0] = v7[0];
                                    p5[1] = v7[1];
                                    p5[2] = v7[2];

                                    p6[0] = v6[0];
                                    p6[1] = v6[1];
                                    p6[2] = v6[2];

                                    p7[0] = v5[0];
                                    p7[1] = v5[1];
                                    p7[2] = v5[2];

                                    p8[0] = v4[0];
                                    p8[1] = v4[1];
                                    p8[2] = v4[2];

                                    p9[0] = v3[0];
                                    p9[1] = v3[1];
                                    p9[2] = v3[2];

                                    p10[0] = v2[0];
                                    p10[1] = v2[1];
                                    p10[2] = v2[2];
                                }
                                if (pointsCount - 1 == 11)
                                {
                                    p2[0] = v11[0];
                                    p2[1] = v11[1];
                                    p2[2] = v11[2];

                                    p3[0] = v10[0];
                                    p3[1] = v10[1];
                                    p3[2] = v10[2];

                                    p4[0] = v9[0];
                                    p4[1] = v9[1];
                                    p4[2] = v9[2];

                                    p5[0] = v8[0];
                                    p5[1] = v8[1];
                                    p5[2] = v8[2];

                                    p6[0] = v7[0];
                                    p6[1] = v7[1];
                                    p6[2] = v7[2];

                                    p7[0] = v6[0];
                                    p7[1] = v6[1];
                                    p7[2] = v6[2];

                                    p8[0] = v5[0];
                                    p8[1] = v5[1];
                                    p8[2] = v5[2];

                                    p9[0] = v4[0];
                                    p9[1] = v4[1];
                                    p9[2] = v4[2];

                                    p10[0] = v3[0];
                                    p10[1] = v3[1];
                                    p10[2] = v3[2];

                                    p11[0] = v2[0];
                                    p11[1] = v2[1];
                                    p11[2] = v2[2];
                                }
                                if (pointsCount - 1 == 12)
                                {
                                    p2[0] = v12[0];
                                    p2[1] = v12[1];
                                    p2[2] = v12[2];

                                    p3[0] = v11[0];
                                    p3[1] = v11[1];
                                    p3[2] = v11[2];

                                    p4[0] = v10[0];
                                    p4[1] = v10[1];
                                    p4[2] = v10[2];

                                    p5[0] = v9[0];
                                    p5[1] = v9[1];
                                    p5[2] = v9[2];

                                    p6[0] = v8[0];
                                    p6[1] = v8[1];
                                    p6[2] = v8[2];

                                    p7[0] = v7[0];
                                    p7[1] = v7[1];
                                    p7[2] = v7[2];

                                    p8[0] = v6[0];
                                    p8[1] = v6[1];
                                    p8[2] = v6[2];

                                    p9[0] = v5[0];
                                    p9[1] = v5[1];
                                    p9[2] = v5[2];

                                    p10[0] = v4[0];
                                    p10[1] = v4[1];
                                    p10[2] = v4[2];

                                    p11[0] = v3[0];
                                    p11[1] = v3[1];
                                    p11[2] = v3[2];

                                    p12[0] = v2[0];
                                    p12[1] = v2[1];
                                    p12[2] = v2[2];
                                }
                                if (pointsCount - 1 == 13)
                                {
                                    p2[0] = v13[0];
                                    p2[1] = v13[1];
                                    p2[2] = v13[2];

                                    p3[0] = v12[0];
                                    p3[1] = v12[1];
                                    p3[2] = v12[2];

                                    p4[0] = v11[0];
                                    p4[1] = v11[1];
                                    p4[2] = v11[2];

                                    p5[0] = v10[0];
                                    p5[1] = v10[1];
                                    p5[2] = v10[2];

                                    p6[0] = v9[0];
                                    p6[1] = v9[1];
                                    p6[2] = v9[2];

                                    p7[0] = v8[0];
                                    p7[1] = v8[1];
                                    p7[2] = v8[2];

                                    p8[0] = v7[0];
                                    p8[1] = v7[1];
                                    p8[2] = v7[2];

                                    p9[0] = v6[0];
                                    p9[1] = v6[1];
                                    p9[2] = v6[2];

                                    p10[0] = v5[0];
                                    p10[1] = v5[1];
                                    p10[2] = v5[2];

                                    p11[0] = v4[0];
                                    p11[1] = v4[1];
                                    p11[2] = v4[2];

                                    p12[0] = v3[0];
                                    p12[1] = v3[1];
                                    p12[2] = v3[2];

                                    p13[0] = v2[0];
                                    p13[1] = v2[1];
                                    p13[2] = v2[2];
                                }
                                if (pointsCount - 1 == 14)
                                {
                                    p2[0] = v14[0];
                                    p2[1] = v14[1];
                                    p2[2] = v14[2];

                                    p3[0] = v13[0];
                                    p3[1] = v13[1];
                                    p3[2] = v13[2];

                                    p4[0] = v12[0];
                                    p4[1] = v12[1];
                                    p4[2] = v12[2];

                                    p5[0] = v11[0];
                                    p5[1] = v11[1];
                                    p5[2] = v11[2];

                                    p6[0] = v10[0];
                                    p6[1] = v10[1];
                                    p6[2] = v10[2];

                                    p7[0] = v9[0];
                                    p7[1] = v9[1];
                                    p7[2] = v9[2];

                                    p8[0] = v8[0];
                                    p8[1] = v8[1];
                                    p8[2] = v8[2];

                                    p9[0] = v7[0];
                                    p9[1] = v7[1];
                                    p9[2] = v7[2];

                                    p10[0] = v6[0];
                                    p10[1] = v6[1];
                                    p10[2] = v6[2];

                                    p11[0] = v5[0];
                                    p11[1] = v5[1];
                                    p11[2] = v5[2];

                                    p12[0] = v4[0];
                                    p12[1] = v4[1];
                                    p12[2] = v4[2];

                                    p13[0] = v3[0];
                                    p13[1] = v3[1];
                                    p13[2] = v3[2];

                                    p14[0] = v2[0];
                                    p14[1] = v2[1];
                                    p14[2] = v2[2];
                                }
                                if (pointsCount - 1 == 15)
                                {
                                    p2[0] = v15[0];
                                    p2[1] = v15[1];
                                    p2[2] = v15[2];

                                    p3[0] = v14[0];
                                    p3[1] = v14[1];
                                    p3[2] = v14[2];

                                    p4[0] = v13[0];
                                    p4[1] = v13[1];
                                    p4[2] = v13[2];

                                    p5[0] = v12[0];
                                    p5[1] = v12[1];
                                    p5[2] = v12[2];

                                    p6[0] = v11[0];
                                    p6[1] = v11[1];
                                    p6[2] = v11[2];

                                    p7[0] = v10[0];
                                    p7[1] = v10[1];
                                    p7[2] = v10[2];

                                    p8[0] = v9[0];
                                    p8[1] = v9[1];
                                    p8[2] = v9[2];

                                    p9[0] = v8[0];
                                    p9[1] = v8[1];
                                    p9[2] = v8[2];

                                    p10[0] = v7[0];
                                    p10[1] = v7[1];
                                    p10[2] = v7[2];

                                    p11[0] = v6[0];
                                    p11[1] = v6[1];
                                    p11[2] = v6[2];

                                    p12[0] = v5[0];
                                    p12[1] = v5[1];
                                    p12[2] = v5[2];

                                    p13[0] = v4[0];
                                    p13[1] = v4[1];
                                    p13[2] = v4[2];

                                    p14[0] = v3[0];
                                    p14[1] = v3[1];
                                    p14[2] = v3[2];

                                    p15[0] = v2[0];
                                    p15[1] = v2[1];
                                    p15[2] = v2[2];
                                }
                                if (pointsCount - 1 == 16)
                                {
                                    p2[0] = v16[0];
                                    p2[1] = v16[1];
                                    p2[2] = v16[2];

                                    p3[0] = v15[0];
                                    p3[1] = v15[1];
                                    p3[2] = v15[2];

                                    p4[0] = v14[0];
                                    p4[1] = v14[1];
                                    p4[2] = v14[2];

                                    p5[0] = v13[0];
                                    p5[1] = v13[1];
                                    p5[2] = v13[2];

                                    p6[0] = v12[0];
                                    p6[1] = v12[1];
                                    p6[2] = v12[2];

                                    p7[0] = v11[0];
                                    p7[1] = v11[1];
                                    p7[2] = v11[2];

                                    p8[0] = v10[0];
                                    p8[1] = v10[1];
                                    p8[2] = v10[2];

                                    p9[0] = v9[0];
                                    p9[1] = v9[1];
                                    p9[2] = v9[2];

                                    p10[0] = v8[0];
                                    p10[1] = v8[1];
                                    p10[2] = v8[2];

                                    p11[0] = v7[0];
                                    p11[1] = v7[1];
                                    p11[2] = v7[2];

                                    p12[0] = v6[0];
                                    p12[1] = v6[1];
                                    p12[2] = v6[2];

                                    p13[0] = v5[0];
                                    p13[1] = v5[1];
                                    p13[2] = v5[2];

                                    p14[0] = v4[0];
                                    p14[1] = v4[1];
                                    p14[2] = v4[2];

                                    p15[0] = v3[0];
                                    p15[1] = v3[1];
                                    p15[2] = v3[2];

                                    p16[0] = v2[0];
                                    p16[1] = v2[1];
                                    p16[2] = v2[2];


                                }
                            }

                        }
                        //if RefDirection is (0, 0, 1)
                        else if (refDirectionPlane0[0] == 0 && refDirectionPlane0[1] == 0 && refDirectionPlane0[2] != 0)
                        {
                            if (AxisX || AxisY)
                            {
                                if (pointsCount - 1 == 4)
                                {
                                    p2[0] = v4[0];
                                    p2[1] = v4[1];
                                    p2[2] = v4[2];

                                    p3[0] = v3[0];
                                    p3[1] = v3[1];
                                    p3[2] = v3[2];

                                    p4[0] = v2[0];
                                    p4[1] = v2[1];
                                    p4[2] = v2[2];
                                }
                                if (pointsCount - 1 == 5)
                                {
                                    p2[0] = v5[0];
                                    p2[1] = v5[1];
                                    p2[2] = v5[2];

                                    p3[0] = v4[0];
                                    p3[1] = v4[1];
                                    p3[2] = v4[2];

                                    p4[0] = v3[0];
                                    p4[1] = v3[1];
                                    p4[2] = v3[2];

                                    p5[0] = v2[0];
                                    p5[1] = v2[1];
                                    p5[2] = v2[2];
                                }
                                if (pointsCount - 1 == 6)
                                {
                                    p2[0] = v6[0];
                                    p2[1] = v6[1];
                                    p2[2] = v6[2];

                                    p3[0] = v5[0];
                                    p3[1] = v5[1];
                                    p3[2] = v5[2];

                                    p4[0] = v4[0];
                                    p4[1] = v4[1];
                                    p4[2] = v4[2];

                                    p5[0] = v3[0];
                                    p5[1] = v3[1];
                                    p5[2] = v3[2];

                                    p6[0] = v2[0];
                                    p6[1] = v2[1];
                                    p6[2] = v2[2];
                                }
                                if (pointsCount - 1 == 7)
                                {
                                    p2[0] = v7[0];
                                    p2[1] = v7[1];
                                    p2[2] = v7[2];

                                    p3[0] = v6[0];
                                    p3[1] = v6[1];
                                    p3[2] = v6[2];

                                    p4[0] = v5[0];
                                    p4[1] = v5[1];
                                    p4[2] = v5[2];

                                    p5[0] = v4[0];
                                    p5[1] = v4[1];
                                    p5[2] = v4[2];

                                    p6[0] = v3[0];
                                    p6[1] = v3[1];
                                    p6[2] = v3[2];

                                    p7[0] = v2[0];
                                    p7[1] = v2[1];
                                    p7[2] = v2[2];
                                }
                                if (pointsCount - 1 == 8)
                                {
                                    p2[0] = v8[0];
                                    p2[1] = v8[1];
                                    p2[2] = v8[2];

                                    p3[0] = v7[0];
                                    p3[1] = v7[1];
                                    p3[2] = v7[2];

                                    p4[0] = v6[0];
                                    p4[1] = v6[1];
                                    p4[2] = v6[2];

                                    p5[0] = v5[0];
                                    p5[1] = v5[1];
                                    p5[2] = v5[2];

                                    p6[0] = v4[0];
                                    p6[1] = v4[1];
                                    p6[2] = v4[2];

                                    p7[0] = v3[0];
                                    p7[1] = v3[1];
                                    p7[2] = v3[2];

                                    p8[0] = v2[0];
                                    p8[1] = v2[1];
                                    p8[2] = v2[2];
                                }
                                if (pointsCount - 1 == 9)
                                {
                                    p2[0] = v9[0];
                                    p2[1] = v9[1];
                                    p2[2] = v9[2];

                                    p3[0] = v8[0];
                                    p3[1] = v8[1];
                                    p3[2] = v8[2];

                                    p4[0] = v7[0];
                                    p4[1] = v7[1];
                                    p4[2] = v7[2];

                                    p5[0] = v6[0];
                                    p5[1] = v6[1];
                                    p5[2] = v6[2];

                                    p6[0] = v5[0];
                                    p6[1] = v5[1];
                                    p6[2] = v5[2];

                                    p7[0] = v4[0];
                                    p7[1] = v4[1];
                                    p7[2] = v4[2];

                                    p8[0] = v3[0];
                                    p8[1] = v3[1];
                                    p8[2] = v3[2];

                                    p9[0] = v2[0];
                                    p9[1] = v2[1];
                                    p9[2] = v2[2];
                                }
                                if (pointsCount - 1 == 10)
                                {
                                    p2[0] = v10[0];
                                    p2[1] = v10[1];
                                    p2[2] = v10[2];

                                    p3[0] = v9[0];
                                    p3[1] = v9[1];
                                    p3[2] = v9[2];

                                    p4[0] = v8[0];
                                    p4[1] = v8[1];
                                    p4[2] = v8[2];

                                    p5[0] = v7[0];
                                    p5[1] = v7[1];
                                    p5[2] = v7[2];

                                    p6[0] = v6[0];
                                    p6[1] = v6[1];
                                    p6[2] = v6[2];

                                    p7[0] = v5[0];
                                    p7[1] = v5[1];
                                    p7[2] = v5[2];

                                    p8[0] = v4[0];
                                    p8[1] = v4[1];
                                    p8[2] = v4[2];

                                    p9[0] = v3[0];
                                    p9[1] = v3[1];
                                    p9[2] = v3[2];

                                    p10[0] = v2[0];
                                    p10[1] = v2[1];
                                    p10[2] = v2[2];
                                }
                                if (pointsCount - 1 == 11)
                                {
                                    p2[0] = v11[0];
                                    p2[1] = v11[1];
                                    p2[2] = v11[2];

                                    p3[0] = v10[0];
                                    p3[1] = v10[1];
                                    p3[2] = v10[2];

                                    p4[0] = v9[0];
                                    p4[1] = v9[1];
                                    p4[2] = v9[2];

                                    p5[0] = v8[0];
                                    p5[1] = v8[1];
                                    p5[2] = v8[2];

                                    p6[0] = v7[0];
                                    p6[1] = v7[1];
                                    p6[2] = v7[2];

                                    p7[0] = v6[0];
                                    p7[1] = v6[1];
                                    p7[2] = v6[2];

                                    p8[0] = v5[0];
                                    p8[1] = v5[1];
                                    p8[2] = v5[2];

                                    p9[0] = v4[0];
                                    p9[1] = v4[1];
                                    p9[2] = v4[2];

                                    p10[0] = v3[0];
                                    p10[1] = v3[1];
                                    p10[2] = v3[2];

                                    p11[0] = v2[0];
                                    p11[1] = v2[1];
                                    p11[2] = v2[2];
                                }
                                if (pointsCount - 1 == 12)
                                {
                                    p2[0] = v12[0];
                                    p2[1] = v12[1];
                                    p2[2] = v12[2];

                                    p3[0] = v11[0];
                                    p3[1] = v11[1];
                                    p3[2] = v11[2];

                                    p4[0] = v10[0];
                                    p4[1] = v10[1];
                                    p4[2] = v10[2];

                                    p5[0] = v9[0];
                                    p5[1] = v9[1];
                                    p5[2] = v9[2];

                                    p6[0] = v8[0];
                                    p6[1] = v8[1];
                                    p6[2] = v8[2];

                                    p7[0] = v7[0];
                                    p7[1] = v7[1];
                                    p7[2] = v7[2];

                                    p8[0] = v6[0];
                                    p8[1] = v6[1];
                                    p8[2] = v6[2];

                                    p9[0] = v5[0];
                                    p9[1] = v5[1];
                                    p9[2] = v5[2];

                                    p10[0] = v4[0];
                                    p10[1] = v4[1];
                                    p10[2] = v4[2];

                                    p11[0] = v3[0];
                                    p11[1] = v3[1];
                                    p11[2] = v3[2];

                                    p12[0] = v2[0];
                                    p12[1] = v2[1];
                                    p12[2] = v2[2];
                                }
                                if (pointsCount - 1 == 13)
                                {
                                    p2[0] = v13[0];
                                    p2[1] = v13[1];
                                    p2[2] = v13[2];

                                    p3[0] = v12[0];
                                    p3[1] = v12[1];
                                    p3[2] = v12[2];

                                    p4[0] = v11[0];
                                    p4[1] = v11[1];
                                    p4[2] = v11[2];

                                    p5[0] = v10[0];
                                    p5[1] = v10[1];
                                    p5[2] = v10[2];

                                    p6[0] = v9[0];
                                    p6[1] = v9[1];
                                    p6[2] = v9[2];

                                    p7[0] = v8[0];
                                    p7[1] = v8[1];
                                    p7[2] = v8[2];

                                    p8[0] = v7[0];
                                    p8[1] = v7[1];
                                    p8[2] = v7[2];

                                    p9[0] = v6[0];
                                    p9[1] = v6[1];
                                    p9[2] = v6[2];

                                    p10[0] = v5[0];
                                    p10[1] = v5[1];
                                    p10[2] = v5[2];

                                    p11[0] = v4[0];
                                    p11[1] = v4[1];
                                    p11[2] = v4[2];

                                    p12[0] = v3[0];
                                    p12[1] = v3[1];
                                    p12[2] = v3[2];

                                    p13[0] = v2[0];
                                    p13[1] = v2[1];
                                    p13[2] = v2[2];
                                }
                                if (pointsCount - 1 == 14)
                                {
                                    p2[0] = v14[0];
                                    p2[1] = v14[1];
                                    p2[2] = v14[2];

                                    p3[0] = v13[0];
                                    p3[1] = v13[1];
                                    p3[2] = v13[2];

                                    p4[0] = v12[0];
                                    p4[1] = v12[1];
                                    p4[2] = v12[2];

                                    p5[0] = v11[0];
                                    p5[1] = v11[1];
                                    p5[2] = v11[2];

                                    p6[0] = v10[0];
                                    p6[1] = v10[1];
                                    p6[2] = v10[2];

                                    p7[0] = v9[0];
                                    p7[1] = v9[1];
                                    p7[2] = v9[2];

                                    p8[0] = v8[0];
                                    p8[1] = v8[1];
                                    p8[2] = v8[2];

                                    p9[0] = v7[0];
                                    p9[1] = v7[1];
                                    p9[2] = v7[2];

                                    p10[0] = v6[0];
                                    p10[1] = v6[1];
                                    p10[2] = v6[2];

                                    p11[0] = v5[0];
                                    p11[1] = v5[1];
                                    p11[2] = v5[2];

                                    p12[0] = v4[0];
                                    p12[1] = v4[1];
                                    p12[2] = v4[2];

                                    p13[0] = v3[0];
                                    p13[1] = v3[1];
                                    p13[2] = v3[2];

                                    p14[0] = v2[0];
                                    p14[1] = v2[1];
                                    p14[2] = v2[2];
                                }
                                if (pointsCount - 1 == 15)
                                {
                                    p2[0] = v15[0];
                                    p2[1] = v15[1];
                                    p2[2] = v15[2];

                                    p3[0] = v14[0];
                                    p3[1] = v14[1];
                                    p3[2] = v14[2];

                                    p4[0] = v13[0];
                                    p4[1] = v13[1];
                                    p4[2] = v13[2];

                                    p5[0] = v12[0];
                                    p5[1] = v12[1];
                                    p5[2] = v12[2];

                                    p6[0] = v11[0];
                                    p6[1] = v11[1];
                                    p6[2] = v11[2];

                                    p7[0] = v10[0];
                                    p7[1] = v10[1];
                                    p7[2] = v10[2];

                                    p8[0] = v9[0];
                                    p8[1] = v9[1];
                                    p8[2] = v9[2];

                                    p9[0] = v8[0];
                                    p9[1] = v8[1];
                                    p9[2] = v8[2];

                                    p10[0] = v7[0];
                                    p10[1] = v7[1];
                                    p10[2] = v7[2];

                                    p11[0] = v6[0];
                                    p11[1] = v6[1];
                                    p11[2] = v6[2];

                                    p12[0] = v5[0];
                                    p12[1] = v5[1];
                                    p12[2] = v5[2];

                                    p13[0] = v4[0];
                                    p13[1] = v4[1];
                                    p13[2] = v4[2];

                                    p14[0] = v3[0];
                                    p14[1] = v3[1];
                                    p14[2] = v3[2];

                                    p15[0] = v2[0];
                                    p15[1] = v2[1];
                                    p15[2] = v2[2];
                                }
                                if (pointsCount - 1 == 16)
                                {
                                    p2[0] = v16[0];
                                    p2[1] = v16[1];
                                    p2[2] = v16[2];

                                    p3[0] = v15[0];
                                    p3[1] = v15[1];
                                    p3[2] = v15[2];

                                    p4[0] = v14[0];
                                    p4[1] = v14[1];
                                    p4[2] = v14[2];

                                    p5[0] = v13[0];
                                    p5[1] = v13[1];
                                    p5[2] = v13[2];

                                    p6[0] = v12[0];
                                    p6[1] = v12[1];
                                    p6[2] = v12[2];

                                    p7[0] = v11[0];
                                    p7[1] = v11[1];
                                    p7[2] = v11[2];

                                    p8[0] = v10[0];
                                    p8[1] = v10[1];
                                    p8[2] = v10[2];

                                    p9[0] = v9[0];
                                    p9[1] = v9[1];
                                    p9[2] = v9[2];

                                    p10[0] = v8[0];
                                    p10[1] = v8[1];
                                    p10[2] = v8[2];

                                    p11[0] = v7[0];
                                    p11[1] = v7[1];
                                    p11[2] = v7[2];

                                    p12[0] = v6[0];
                                    p12[1] = v6[1];
                                    p12[2] = v6[2];

                                    p13[0] = v5[0];
                                    p13[1] = v5[1];
                                    p13[2] = v5[2];

                                    p14[0] = v4[0];
                                    p14[1] = v4[1];
                                    p14[2] = v4[2];

                                    p15[0] = v3[0];
                                    p15[1] = v3[1];
                                    p15[2] = v3[2];

                                    p16[0] = v2[0];
                                    p16[1] = v2[1];
                                    p16[2] = v2[2];


                                }
                            }
                        }






                        //-------------------------------------
                        //----Get the Building Element Data
                        var buildingElement = relSpaceBoundary.RelatedBuildingElement;



                        //---------------------------------------
                        //---Get the Space Data
                        var space = relSpaceBoundary.RelatingSpace;
                        var theSpace = space as IIfcSpace;


                        //--------------------------------------
                        //---Build the EnergyPlus Classes depending on type of building Element
                        if (buildingElement is IIfcWall)
                        {

                            var theWall = buildingElement as IIfcWall;
                            BuildingSurfaceDetailed wall = new BuildingSurfaceDetailed();

                            wall.Name = theWall.Name + "-" + theSpace.GlobalId;
                            wall.Name = BuildingSurfaceDetailed.DuplicateNameFix(wall);
                            wall.SurfaceType = SurfaceTypeEnum.Wall;

                            var DefinedBy = theWall.ObjectType.Value;
                            var wallType = model.Instances.FirstOrDefault<IIfcWallType>(d => d.Name == DefinedBy);

                            wall.ConstructionName = wallType.GlobalId;
                            wall.ZoneName = theSpace.GlobalId;
                            if (internalOrExternalBoundary == "INTERNAL")
                            {
                                wall.OutsideBoundaryCondition = OutsideBoundaryConditionEnum.Zone;
                                var providesBoundaries = theWall.ProvidesBoundaries.ToList();
                                var Space = providesBoundaries[1].RelatingSpace;
                                var _space = Space as IIfcSpace;
                                wall.OutsideBoundaryConditionObject = _space.GlobalId;
                                wall.SunExposure = SunExposureEnum.NoSun;
                                wall.WindExposure = WindExposureEnum.NoWind;

                            }
                            else
                            {
                                wall.OutsideBoundaryCondition = OutsideBoundaryConditionEnum.Outdoors;
                                wall.SunExposure = SunExposureEnum.SunExposed;
                                wall.WindExposure = WindExposureEnum.WindExposed;

                            }


                            wall.Vertex1XCoordinate = planeLocation[0];
                            wall.Vertex1YCoordinate = planeLocation[1];
                            wall.Vertex1ZCoordinate = planeLocation[2];

                            wall.Vertex2XCoordinate = p2[0];
                            wall.Vertex2YCoordinate = p2[1];
                            wall.Vertex2ZCoordinate = p2[2];

                            wall.Vertex3XCoordinate = p3[0];
                            wall.Vertex3YCoordinate = p3[1];
                            wall.Vertex3ZCoordinate = p3[2];


                            if (pointsCount - 1 >= 4)
                            {
                                wall.Vertex4XCoordinate = p4[0];
                                wall.Vertex4YCoordinate = p4[1];
                                wall.Vertex4ZCoordinate = p4[2];
                            }
                            if (pointsCount - 1 >= 5)
                            {
                                wall.Vertex5XCoordinate = p5[0];
                                wall.Vertex5YCoordinate = p5[1];
                                wall.Vertex5ZCoordinate = p5[2];
                            }
                            if (pointsCount - 1 >= 6)
                            {
                                wall.Vertex6XCoordinate = p6[0];
                                wall.Vertex6YCoordinate = p6[1];
                                wall.Vertex6ZCoordinate = p6[2];
                            }
                            if (pointsCount - 1 >= 7)
                            {
                                wall.Vertex7XCoordinate = p7[0];
                                wall.Vertex7YCoordinate = p7[1];
                                wall.Vertex7ZCoordinate = p7[2];
                            }
                            if (pointsCount - 1 >= 8)
                            {
                                wall.Vertex8XCoordinate = p8[0];
                                wall.Vertex8YCoordinate = p8[1];
                                wall.Vertex8ZCoordinate = p8[2];
                            }
                            if (pointsCount - 1 >= 9)
                            {
                                wall.Vertex9XCoordinate = p9[0];
                                wall.Vertex9YCoordinate = p9[1];
                                wall.Vertex9ZCoordinate = p9[2];
                            }
                            if (pointsCount - 1 >= 10)
                            {
                                wall.Vertex10XCoordinate = p10[0];
                                wall.Vertex10YCoordinate = p10[1];
                                wall.Vertex10ZCoordinate = p10[2];
                            }
                            if (BuildingSurfaceDetailed.DuplicateCheck(wall))
                            {

                            }
                            else
                            {
                                BuildingSurfaceDetailed.Add(wall);
                            }



                        }
                        else if (buildingElement is IIfcSlab)
                        {
                            var theSlab = buildingElement as IIfcSlab;
                            BuildingSurfaceDetailed slab = new BuildingSurfaceDetailed();
                            slab.Name = theSlab.Name + "-" + theSpace.GlobalId;
                            slab.Name = BuildingSurfaceDetailed.DuplicateNameFix(slab);
                            slab.SurfaceType = SurfaceTypeEnum.Floor;

                            var DefinedBy = theSlab.ObjectType.Value;
                            var slabType = model.Instances.FirstOrDefault<IIfcSlabType>(d => d.Name == DefinedBy);
                            slab.ConstructionName = slabType.GlobalId;

                            slab.ZoneName = theSpace.GlobalId;
                            slab.OutsideBoundaryCondition = OutsideBoundaryConditionEnum.Outdoors;
                            slab.SunExposure = SunExposureEnum.SunExposed;
                            slab.WindExposure = WindExposureEnum.WindExposed;

                            slab.NumberofVertices = "autocalculate";

                            slab.Vertex1XCoordinate = planeLocation[0];
                            slab.Vertex1YCoordinate = planeLocation[1];
                            slab.Vertex1ZCoordinate = planeLocation[2];

                            slab.Vertex2XCoordinate = p2[0];
                            slab.Vertex2YCoordinate = p2[1];
                            slab.Vertex2ZCoordinate = p2[2];

                            slab.Vertex3XCoordinate = p3[0];
                            slab.Vertex3YCoordinate = p3[1];
                            slab.Vertex3ZCoordinate = p3[2];


                            if (pointsCount - 1 >= 4)
                            {
                                slab.Vertex4XCoordinate = p4[0];
                                slab.Vertex4YCoordinate = p4[1];
                                slab.Vertex4ZCoordinate = p4[2];
                            }
                            if (pointsCount - 1 >= 5)
                            {
                                slab.Vertex5XCoordinate = p5[0];
                                slab.Vertex5YCoordinate = p5[1];
                                slab.Vertex5ZCoordinate = p5[2];
                            }
                            if (pointsCount - 1 >= 6)
                            {
                                slab.Vertex6XCoordinate = p6[0];
                                slab.Vertex6YCoordinate = p6[1];
                                slab.Vertex6ZCoordinate = p6[2];
                            }
                            if (pointsCount - 1 >= 7)
                            {
                                slab.Vertex7XCoordinate = p7[0];
                                slab.Vertex7YCoordinate = p7[1];
                                slab.Vertex7ZCoordinate = p7[2];
                            }
                            if (pointsCount - 1 >= 8)
                            {
                                slab.Vertex8XCoordinate = p8[0];
                                slab.Vertex8YCoordinate = p8[1];
                                slab.Vertex8ZCoordinate = p8[2];
                            }
                            if (pointsCount - 1 >= 9)
                            {
                                slab.Vertex9XCoordinate = p9[0];
                                slab.Vertex9YCoordinate = p9[1];
                                slab.Vertex9ZCoordinate = p9[2];
                            }
                            if (pointsCount - 1 >= 10)
                            {
                                slab.Vertex10XCoordinate = p10[0];
                                slab.Vertex10YCoordinate = p10[1];
                                slab.Vertex10ZCoordinate = p10[2];
                            }
                            if (BuildingSurfaceDetailed.DuplicateCheck(slab))
                            {

                            }
                            else
                            {
                                BuildingSurfaceDetailed.Add(slab);
                            }

                        }
                        else if (buildingElement is IIfcWindow)
                        {
                            var theWindow = buildingElement as IIfcWindow;


                            FenestrationSurfaceDetailed fsd = new FenestrationSurfaceDetailed();
                            //---
                            ///Host element of the window
                            var id = theWindow.GlobalId;

                            var bSurfaceName = theWindow.FillsVoids.First();
                            var _bSurfaceName = bSurfaceName as IIfcRelFillsElement;
                            var openingElement = _bSurfaceName.RelatingOpeningElement;
                            var _openingElement = openingElement as IIfcOpeningElement;


                            fsd.Name = theWindow.Name;
                            fsd.SurfaceType = SurfaceTypeEnumFSD.Window;

                            var DefinedBy = theWindow.ObjectType.Value;
                            var windowType = model.Instances.FirstOrDefault<IIfcWindowType>(d => d.Name == DefinedBy);
                            fsd.ConstructionName = windowType.GlobalId;

                            fsd.BuildingSurfaceName = BuildingSurfaceDetailed.FindSurface(_openingElement.VoidsElements.RelatingBuildingElement.Name + "-" + theSpace.GlobalId);



                            //--------Geometry
                            // using Opening data to get geometry of the window

                            var localPlacement = _openingElement.ObjectPlacement;
                            var _localPlacement = localPlacement as IIfcLocalPlacement;
                            var relativePlacementOp = _localPlacement.RelativePlacement;
                            var _relativePlacementOp = relativePlacementOp as IIfcAxis2Placement3D;
                            double[] IPOpening = new double[3];
                            IPOpening[0] = _relativePlacementOp.Location.X;
                            IPOpening[1] = _relativePlacementOp.Location.Y;
                            IPOpening[2] = _relativePlacementOp.Location.Z;   //Contain initial point of the opening relative to the wall 

                            var placementRelTo = _localPlacement.PlacementRelTo;
                            var _placementRelTo = placementRelTo as IIfcLocalPlacement;
                            var relativePlacementWall = _placementRelTo.RelativePlacement;
                            var _relativePlacementWall = relativePlacementWall as IIfcAxis2Placement3D;
                            var p0 = _relativePlacementWall.P[0];
                            var p1 = _relativePlacementWall.P[1];
                            double[] IPWall = new double[3];
                            IPWall[0] = _relativePlacementWall.Location.X;
                            IPWall[1] = _relativePlacementWall.Location.Y;
                            IPWall[2] = _relativePlacementWall.Location.Z;   // Contain initial point of the wall relative to the building


                            double[] refDirection = new double[3];
                            if (_relativePlacementWall.RefDirection != null)
                            {
                                var wallRefDirection = _relativePlacementWall.RefDirection;
                                refDirection[0] = wallRefDirection.X;
                                refDirection[1] = wallRefDirection.Y;
                                refDirection[2] = wallRefDirection.Z;
                            }
                            else
                            {
                                refDirection[0] = 1;
                                refDirection[1] = 0;
                                refDirection[2] = 0;
                            } //Define RefDirection of the wall to calculate de IP of the opening relative to the building

                            double[] v1 = new double[3]; // array of double v1, v2, v3, v4 contain coordinates of the vertices
                            if (refDirection[1] != 0)
                            {
                                v1[0] = IPWall[0] + IPOpening[1] * refDirection[0];
                                v1[1] = IPWall[1] + IPOpening[0] * refDirection[1];
                                v1[2] = IPWall[2] + IPOpening[2] * refDirection[2];
                            }
                            else
                            {
                                v1[0] = IPWall[0] + IPOpening[0] * refDirection[0];
                                v1[1] = IPWall[1] + IPOpening[1] * refDirection[1];
                                v1[2] = IPWall[2] + IPOpening[2] * refDirection[2];
                            }



                            double[] axisExtruded = new double[3];
                            double[] dimensions = new double[3];
                            if (_openingElement.Representation != null)
                            {
                                if (_openingElement.Representation.Representations[0].Items[0] is IIfcExtrudedAreaSolid)
                                {
                                    var representation = _openingElement.Representation.Representations[0].Items[0];
                                    var _representation = representation as IIfcExtrudedAreaSolid;

                                    var pXDim = _representation.Position.P[0];
                                    var pYDim = _representation.Position.P[1];
                                    var pDepth = _representation.Position.P[2]; //p[3:3] contain vector direction for diferent measures

                                    var depth = _representation.Depth;
                                    var sweptArea = _representation.SweptArea;
                                    var _sweptArea = sweptArea as IIfcRectangleProfileDef;
                                    var XDim = _sweptArea.XDim;
                                    var YDim = _sweptArea.YDim;

                                    var d0 = pXDim * XDim;
                                    var d1 = pYDim * YDim;
                                    var d2 = pDepth * depth;
                                    var dFinal = d0 + d1 + d2; //Vector with the lenght in each direction for the solid

                                    dimensions[0] = dFinal.X;
                                    dimensions[1] = dFinal.Y;
                                    dimensions[2] = dFinal.Z;

                                }
                            }



                            if (refDirection[0] != 0)
                            {
                                dimensions[0] = dimensions[0] * refDirection[0];
                            }
                            if (refDirection[1] != 0)
                            {
                                dimensions[1] = dimensions[1] * refDirection[1];
                            }
                            if (refDirection[2] != 0)
                            {
                                dimensions[2] = dimensions[2] * refDirection[2];
                            } //Correction of the direction of the vector dimensions to calculate the remaining vertices


                            if (Math.Abs(dimensions[0]) < Math.Abs(dimensions[1]))
                            {
                                if (Math.Abs(dimensions[0]) < Math.Abs(dimensions[2]))
                                {
                                    v2[0] = v1[0];
                                    v2[1] = v1[1] + dimensions[1];
                                    v2[2] = v1[2];

                                    v3[0] = v2[0];
                                    v3[1] = v2[1];
                                    v3[2] = v2[2] + dimensions[2];

                                    v4[0] = v1[0];
                                    v4[1] = v1[1];
                                    v4[2] = v1[2] + dimensions[2];
                                }
                                else
                                {
                                    v2[0] = v1[0] + dimensions[0];
                                    v2[1] = v1[1];
                                    v2[2] = v1[2];

                                    v3[0] = v2[0];
                                    v3[1] = v2[1] + dimensions[1];
                                    v3[2] = v2[2];

                                    v4[0] = v1[0];
                                    v4[1] = v1[1] + dimensions[1];
                                    v4[2] = v1[2];
                                }
                            }
                            else
                            {
                                v2[0] = v1[0] + dimensions[0];
                                v2[1] = v1[1];
                                v2[2] = v1[2];

                                v3[0] = v2[0];
                                v3[1] = v2[1];
                                v3[2] = v2[2] + dimensions[2];

                                v4[0] = v1[0];
                                v4[1] = v1[1];
                                v4[2] = v1[2] + dimensions[2];
                            }

                            if (v1[0] <= v2[0] && v1[1] <= v2[1])
                            {
                                fsd.Vertex1XCoordinate = v1[0];
                                fsd.Vertex1YCoordinate = v1[1];
                                fsd.Vertex1ZCoordinate = v1[2];
                                fsd.Vertex2XCoordinate = v2[0];
                                fsd.Vertex2YCoordinate = v2[1];
                                fsd.Vertex2ZCoordinate = v2[2];
                                fsd.Vertex3XCoordinate = v3[0];
                                fsd.Vertex3YCoordinate = v3[1];
                                fsd.Vertex3ZCoordinate = v3[2];
                                fsd.Vertex4XCoordinate = v4[0];
                                fsd.Vertex4YCoordinate = v4[1];
                                fsd.Vertex4ZCoordinate = v4[2];
                            }
                            else
                            {
                                fsd.Vertex1XCoordinate = v2[0];
                                fsd.Vertex1YCoordinate = v2[1];
                                fsd.Vertex1ZCoordinate = v2[2];
                                fsd.Vertex2XCoordinate = v1[0];
                                fsd.Vertex2YCoordinate = v1[1];
                                fsd.Vertex2ZCoordinate = v1[2];
                                fsd.Vertex3XCoordinate = v4[0];
                                fsd.Vertex3YCoordinate = v4[1];
                                fsd.Vertex3ZCoordinate = v4[2];
                                fsd.Vertex4XCoordinate = v3[0];
                                fsd.Vertex4YCoordinate = v3[1];
                                fsd.Vertex4ZCoordinate = v3[2];
                            }
                            if (FenestrationSurfaceDetailed.DuplicateCheck(fsd))
                            {

                            }
                            else
                            {
                                FenestrationSurfaceDetailed.Add(fsd);
                            }



                        }
                        else if (buildingElement is IIfcDoor)
                        {
                            var theDoor = buildingElement as IIfcDoor;


                            FenestrationSurfaceDetailed fsd = new FenestrationSurfaceDetailed();
                            //---
                            ///Host element of the window
                            var id = theDoor.GlobalId;

                            var bSurfaceName = theDoor.FillsVoids.First();
                            var _bSurfaceName = bSurfaceName as IIfcRelFillsElement;
                            var openingElement = _bSurfaceName.RelatingOpeningElement;
                            var _openingElement = openingElement as IIfcOpeningElement;


                            fsd.Name = theDoor.Name;
                            fsd.SurfaceType = SurfaceTypeEnumFSD.Door;

                            var DefinedBy = theDoor.ObjectType.Value;
                            var doorType = model.Instances.FirstOrDefault<IIfcDoorType>(d => d.Name == DefinedBy);
                            fsd.ConstructionName = doorType.GlobalId;

                            fsd.BuildingSurfaceName = BuildingSurfaceDetailed.FindSurface(_openingElement.VoidsElements.RelatingBuildingElement.Name + "-" + theSpace.GlobalId);


                            //geometry
                            var localPlacement = _openingElement.ObjectPlacement;
                            var _localPlacement = localPlacement as IIfcLocalPlacement;
                            var relativePlacementOp = _localPlacement.RelativePlacement;
                            var _relativePlacementOp = relativePlacementOp as IIfcAxis2Placement3D;
                            double[] IPOpening = new double[3];
                            IPOpening[0] = _relativePlacementOp.Location.X;
                            IPOpening[1] = _relativePlacementOp.Location.Y;
                            IPOpening[2] = _relativePlacementOp.Location.Z;   //Contain initial point of the opening relative to the wall 

                            var placementRelTo = _localPlacement.PlacementRelTo;
                            var _placementRelTo = placementRelTo as IIfcLocalPlacement;
                            var relativePlacementWall = _placementRelTo.RelativePlacement;
                            var _relativePlacementWall = relativePlacementWall as IIfcAxis2Placement3D;
                            var p0 = _relativePlacementWall.P[0];
                            var p1 = _relativePlacementWall.P[1];
                            double[] IPWall = new double[3];
                            IPWall[0] = _relativePlacementWall.Location.X;
                            IPWall[1] = _relativePlacementWall.Location.Y;
                            IPWall[2] = _relativePlacementWall.Location.Z;   // Contain initial point of the wall relative to the building


                            double[] refDirection = new double[3];
                            if (_relativePlacementWall.RefDirection != null)
                            {
                                var wallRefDirection = _relativePlacementWall.RefDirection;
                                refDirection[0] = wallRefDirection.X;
                                refDirection[1] = wallRefDirection.Y;
                                refDirection[2] = wallRefDirection.Z;
                            }
                            else
                            {
                                refDirection[0] = 1;
                                refDirection[1] = 0;
                                refDirection[2] = 0;
                            } //Define RefDirection of the wall to calculate de IP of the opening relative to the building


                            double[] v1 = new double[3]; // array of double v1, v2, v3, v4 contain coordinates of the vertices
                            if (refDirection[1] != 0)
                            {
                                v1[0] = IPWall[0] + IPOpening[1] * refDirection[0];
                                v1[1] = IPWall[1] + IPOpening[0] * refDirection[1];
                                v1[2] = IPWall[2] + IPOpening[2] * refDirection[2];
                            }
                            else
                            {
                                v1[0] = IPWall[0] + IPOpening[0] * refDirection[0];
                                v1[1] = IPWall[1] + IPOpening[1] * refDirection[1];
                                v1[2] = IPWall[2] + IPOpening[2] * refDirection[2];
                            }



                            double[] axisExtruded = new double[3];
                            double[] dimensions = new double[3];
                            if (_openingElement.Representation != null)
                            {
                                if (_openingElement.Representation.Representations[0].Items[0] is IIfcExtrudedAreaSolid)
                                {
                                    var representation = _openingElement.Representation.Representations[0].Items[0];
                                    var _representation = representation as IIfcExtrudedAreaSolid;

                                    var pXDim = _representation.Position.P[0];
                                    var pYDim = _representation.Position.P[1];
                                    var pDepth = _representation.Position.P[2]; //p[3:3] contain vector direction for diferent measures

                                    //Console.WriteLine(door.GlobalId);
                                    //Console.WriteLine($"{refDirection[0]}, {refDirection[1]}, {refDirection[2]} - Ref Direction");
                                    //Console.WriteLine($"{pXDim}, {pYDim}, {pDepth} ");

                                    var depth = _representation.Depth;
                                    var sweptArea = _representation.SweptArea;
                                    var _sweptArea = sweptArea as IIfcRectangleProfileDef;
                                    var XDim = _sweptArea.XDim;
                                    var YDim = _sweptArea.YDim;

                                    //Console.WriteLine($"{XDim}, {YDim}, {depth} ");

                                    Xbim.Common.Geometry.IVector3D dFinal;
                                    if (refDirection[1] != 0) //The direction of the wall change the relation between the Opening location referenced to the global coordinate system
                                    {
                                        var d0 = pYDim * XDim;
                                        var d1 = pXDim * YDim;
                                        var d2 = pDepth * depth;
                                        dFinal = d0 + d1 + d2;
                                        dimensions[0] = dFinal.Y;
                                        dimensions[1] = dFinal.X;
                                        dimensions[2] = dFinal.Z;
                                    }
                                    else
                                    {
                                        var d0 = pYDim * XDim;
                                        var d1 = pXDim * YDim;
                                        var d2 = pDepth * depth;
                                        dFinal = d0 + d1 + d2; //Vector with the lenght in each direction for the solid
                                        dimensions[0] = dFinal.X;
                                        dimensions[1] = dFinal.Y;
                                        dimensions[2] = dFinal.Z;
                                    }

                                }
                            }


                            if (refDirection[0] != 0)
                            {
                                dimensions[0] = dimensions[0] * refDirection[0];
                            }
                            if (refDirection[1] != 0)
                            {
                                dimensions[1] = dimensions[1] * refDirection[1];
                            }
                            if (refDirection[2] != 0)
                            {
                                dimensions[2] = dimensions[2] * refDirection[2];
                            } //Correction of the direction of the vector dimensions to calculate the remaining vertices
    ;

                            if (Math.Abs(dimensions[0]) < Math.Abs(dimensions[1]))
                            {
                                if (Math.Abs(dimensions[0]) < Math.Abs(dimensions[2]))
                                {
                                    v2[0] = v1[0];
                                    v2[1] = v1[1] + dimensions[1];
                                    v2[2] = v1[2];

                                    v3[0] = v2[0];
                                    v3[1] = v2[1];
                                    v3[2] = v2[2] + dimensions[2];

                                    v4[0] = v1[0];
                                    v4[1] = v1[1];
                                    v4[2] = v1[2] + dimensions[2];
                                }
                                else
                                {
                                    v2[0] = v1[0] + dimensions[0];
                                    v2[1] = v1[1];
                                    v2[2] = v1[2];

                                    v3[0] = v2[0];
                                    v3[1] = v2[1] + dimensions[1];
                                    v3[2] = v2[2];

                                    v4[0] = v1[0];
                                    v4[1] = v1[1] + dimensions[1];
                                    v4[2] = v1[2];
                                }
                            }
                            else
                            {
                                v2[0] = v1[0] + dimensions[0];
                                v2[1] = v1[1];
                                v2[2] = v1[2];

                                v3[0] = v2[0];
                                v3[1] = v2[1];
                                v3[2] = v2[2] + dimensions[2];

                                v4[0] = v1[0];
                                v4[1] = v1[1];
                                v4[2] = v1[2] + dimensions[2];
                            }

                            if (v1[0] <= v2[0] && v1[1] <= v2[1])
                            {
                                fsd.Vertex1XCoordinate = v1[0];
                                fsd.Vertex1YCoordinate = v1[1];
                                fsd.Vertex1ZCoordinate = v1[2];
                                fsd.Vertex2XCoordinate = v2[0];
                                fsd.Vertex2YCoordinate = v2[1];
                                fsd.Vertex2ZCoordinate = v2[2];
                                fsd.Vertex3XCoordinate = v3[0];
                                fsd.Vertex3YCoordinate = v3[1];
                                fsd.Vertex3ZCoordinate = v3[2];
                                fsd.Vertex4XCoordinate = v4[0];
                                fsd.Vertex4YCoordinate = v4[1];
                                fsd.Vertex4ZCoordinate = v4[2];
                            }
                            else
                            {
                                fsd.Vertex1XCoordinate = v2[0];
                                fsd.Vertex1YCoordinate = v2[1];
                                fsd.Vertex1ZCoordinate = v2[2];
                                fsd.Vertex2XCoordinate = v1[0];
                                fsd.Vertex2YCoordinate = v1[1];
                                fsd.Vertex2ZCoordinate = v1[2];
                                fsd.Vertex3XCoordinate = v4[0];
                                fsd.Vertex3YCoordinate = v4[1];
                                fsd.Vertex3ZCoordinate = v4[2];
                                fsd.Vertex4XCoordinate = v3[0];
                                fsd.Vertex4YCoordinate = v3[1];
                                fsd.Vertex4ZCoordinate = v3[2];
                            }

                            if (FenestrationSurfaceDetailed.DuplicateCheck(fsd))
                            {

                            }
                            else
                            {
                                FenestrationSurfaceDetailed.Add(fsd);
                            }

                        }


                    }


                    //---IFC BUILDING ELEMENTS

                    var elements = model.Instances.OfType<IIfcBuildingElement>().ToList();
                    foreach (var element in elements)
                    {
                        Construction co = new Construction();
                        Material mat = new Material();
                        WindowMaterialSimpleGlazingSystem wmg = new WindowMaterialSimpleGlazingSystem();
                        var type = element.GetType().Name;
                        if (type == "IfcWall" || type == "IfcWallType")
                        {

                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcWall>(d => d.GlobalId == id);
                            if (theElement != null)
                            {

                                var _theElement = theElement as IIfcWall;
                                var DefinedBy = _theElement.ObjectType.Value;
                                var wallType = model.Instances.FirstOrDefault<IIfcWallType>(d => d.Name == DefinedBy);

                                mat.Name = wallType.GlobalId;
                                mat.Roughness = RoughnessType.Rough;

                                if (Tool.GetWallTypeProperty(wallType, "Width") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetWallTypeProperty(wallType, "Width").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "wallConductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "wallConductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "wallDensity") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "wallDensity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "wallSpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "wallSpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetWallTypeProperty(wallType, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetWallTypeProperty(wallType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetWallTypeProperty(wallType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetWallTypeProperty(wallType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                Material.Add(mat);
                                //-----
                                co.Name = wallType.GlobalId;
                                co.OutsideLayer = wallType.GlobalId;
                                Construction.Add(co);

                            }

                        }
                        else if (type == "IfcBuildingElementProxy")
                        {
                            /*
                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcBuildingElementProxy>(d => d.GlobalId == id);
                            if (theElement != null)
                            {
                                mat.Name = id;
                                mat.Roughness = "Rough";

                                if (Tool.GetElementProperty(theElement, "Width") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetElementProperty(theElement, "Width").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Conductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "Conductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Density") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "Density").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "SpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "SpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                Material.Add(mat);
                                //---
                                co.Name = id;
                                co.OutsideLayer = id;
                                Construction.Add(co);
                            }
                            */
                        }
                        else if (type == "IfcCovering" || type == "IfcCoveringType")
                        {
                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcCovering>(d => d.GlobalId == id);
                            if (theElement != null)
                            {

                                var _theElement = theElement as IIfcCovering;
                                var DefinedBy = _theElement.ObjectType.Value;
                                var ceilingType = model.Instances.FirstOrDefault<IIfcCoveringType>(d => d.Name == DefinedBy);

                                mat.Name = ceilingType.GlobalId;
                                mat.Roughness = RoughnessType.Rough;

                                if (Tool.GetCeilingTypeProperty(ceilingType, "Thickness") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetCeilingTypeProperty(ceilingType, "Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "ceilingConductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "ceilingConductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "ceilingDensity") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "ceilingDensity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "ceilingSpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "ceilingSpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetCeilingTypeProperty(ceilingType, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetCeilingTypeProperty(ceilingType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetCeilingTypeProperty(ceilingType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetCeilingTypeProperty(ceilingType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                Material.Add(mat);
                                //---
                                co.Name = ceilingType.GlobalId;
                                co.OutsideLayer = ceilingType.GlobalId;
                                Construction.Add(co);
                            }
                        }
                        else if (type == "IfcBeam")
                        {
                            /*
                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcBeam>(d => d.GlobalId == id);
                            if (theElement != null)
                            {
                                mat.Name = id;
                                mat.Roughness = "Rough";
                                if (Tool.GetElementProperty(theElement, "Thickness") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetElementProperty(theElement, "Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Conductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "Conductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Density") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "Density").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "SpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "SpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                Material.Add(mat);
                                //---
                                co.Name = id;
                                co.OutsideLayer = id;
                                Construction.Add(co);
                            }
                            */
                        }
                        else if (type == "IfcColumn")
                        {
                            /*
                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcColumn>(d => d.GlobalId == id);
                            if (theElement != null)
                            {
                                mat.Name = id;
                                mat.Roughness = "Rough";
                                if (Tool.GetElementProperty(theElement, "Thickness") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetElementProperty(theElement, "Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Conductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "Conductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Density") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "Density").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "SpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "SpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                Material.Add(mat);
                                //-----
                                co.Name = id;
                                co.OutsideLayer = id;
                                Construction.Add(co);

                            }*/
                        }
                        else if (type == "IfcCurtainWall")
                        {
                            /*
                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcCurtainWall>(d => d.GlobalId == id);
                            if (theElement != null)
                            {
                                mat.Name = id;
                                mat.Roughness = "Rough";
                                if (Tool.GetElementProperty(theElement, "Thickness") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetElementProperty(theElement, "Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Conductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "Conductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Density") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "Density").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "SpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "SpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);

                                }
                                Material.Add(mat);
                                //----
                                co.Name = id;
                                co.OutsideLayer = id;
                                Construction.Add(co);
                            }*/
                        }
                        else if (type == "IfcDoor")
                        {
                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcDoor>(d => d.GlobalId == id);
                            if (theElement != null)
                            {

                                var _theElement = theElement as IIfcDoor;
                                var DefinedBy = _theElement.ObjectType.Value;
                                var doorType = model.Instances.FirstOrDefault<IIfcDoorType>(d => d.Name == DefinedBy);

                                mat.Name = doorType.GlobalId;
                                mat.Roughness = RoughnessType.Rough;

                                if (Tool.GetDoorTypeProperty(doorType, "Thickness") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetDoorTypeProperty(doorType, "Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "doorConductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "doorConductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "doorDensity") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "doorDensity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "doorSpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "doorSpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetDoorTypeProperty(doorType, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetDoorTypeProperty(doorType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetDoorTypeProperty(doorType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetDoorTypeProperty(doorType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                Material.Add(mat);
                                //------
                                co.Name = doorType.GlobalId;
                                co.OutsideLayer = doorType.GlobalId;
                                Construction.Add(co);
                            }
                        }
                        else if (type == "IfcWindow")
                        {

                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcWindow>(d => d.GlobalId == id);
                            if (theElement != null)
                            {

                                var _theElement = theElement as IIfcWindow;
                                var DefinedBy = _theElement.ObjectType.Value;
                                var windowType = model.Instances.FirstOrDefault<IIfcWindowType>(d => d.Name == DefinedBy);

                                wmg.Name = windowType.GlobalId;


                                if (Tool.GetWindowTypeProperty(windowType, "Heat Transfer Coefficient (U)") != null)
                                {
                                    wmg.UFactor = Convert.ToDouble(Tool.GetWindowTypeProperty(windowType, "Heat Transfer Coefficient (U)").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetWindowTypeProperty(windowType, "Solar Heat Gain Coefficient") != null)
                                {
                                    wmg.SolarHeatGainCoefficient = Convert.ToDouble(Tool.GetWindowTypeProperty(windowType, "Solar Heat Gain Coefficient").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetWindowTypeProperty(windowType, "Visual Light Transmittance") != null)
                                {
                                    wmg.VisibleTransmittance = Convert.ToDouble(Tool.GetWindowTypeProperty(windowType, "Visual Light Transmittance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }

                                WindowMaterialSimpleGlazingSystem.Add(wmg);
                                //------
                                co.Name = windowType.GlobalId;
                                co.OutsideLayer = windowType.GlobalId;
                                Construction.Add(co);


                            }
                        }
                        else if (type == "IfcSlab")
                        {
                            string isRoof = null;
                            var id = element.GlobalId.ToString();
                            var theElement = model.Instances.FirstOrDefault<IIfcSlab>(d => d.GlobalId.ToString() == id);
                            if (theElement != null)
                            {
                                var group = model.Instances.OfType<IIfcRelAggregates>().ToList();

                                foreach (var e in group)
                                {
                                    var relatedo = e.RelatedObjects.ToList();
                                    var relatingo = e.RelatingObject;
                                    foreach (var o in relatedo)
                                    {
                                        if ((o is IIfcSlab) && (relatingo is IIfcRoof))
                                        {
                                            var slabid = o.GlobalId.ToString();
                                            var roofid = relatingo.GlobalId.ToString();
                                            if (slabid == id)
                                            {
                                                isRoof = roofid;
                                            }

                                        }

                                    }

                                }

                                if (isRoof != null)
                                {
                                    var roof = model.Instances.FirstOrDefault<IIfcRoof>(d => d.GlobalId.ToString() == isRoof);
                                    if (theElement != null)
                                    {
                                        mat.Name = id;
                                        mat.Roughness = RoughnessType.Rough;
                                        var _theElement = theElement as IIfcRoof;
                                        //var DefinedBy = _theElement.ObjectType.Value;
                                        //var roofType = model.Instances.FirstOrDefault<IIfcRoofType>(d => d.Name == DefinedBy);
                                        if (Tool.GetElementProperty(theElement, "roofThickness") != null)
                                        {
                                            mat.Thickness = Convert.ToDouble(Tool.GetElementProperty(theElement, "Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                        }
                                        if (Tool.GetElementProperty(theElement, "roofConductivity") != null)
                                        {
                                            mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "roofConductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                        }
                                        if (Tool.GetElementProperty(theElement, "roofDensity") != null)
                                        {
                                            mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "roofDensity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                        }
                                        if (Tool.GetElementProperty(theElement, "roofSpecificHeat") != null)
                                        {
                                            mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "roofSpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                        }
                                        if (Tool.GetElementProperty(theElement, "Absorptance") != null)
                                        {
                                            mat.ThermalAbsortance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                            mat.SolarAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                            mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                        }
                                        Material.Add(mat);
                                        //------
                                        co.Name = id;
                                        co.OutsideLayer = id;
                                        Construction.Add(co);
                                    }
                                }
                                else
                                {

                                    var _theElement = theElement as IIfcSlab;
                                    var DefinedBy = _theElement.ObjectType.Value;
                                    var slabType = model.Instances.FirstOrDefault<IIfcSlabType>(d => d.Name == DefinedBy);

                                    mat.Name = slabType.GlobalId;
                                    mat.Roughness = RoughnessType.Rough;

                                    if (Tool.GetFloorTypeProperty(slabType, "Default Thickness") != null)
                                    {
                                        mat.Thickness = Convert.ToDouble(Tool.GetFloorTypeProperty(slabType, "Default Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    }
                                    if (Tool.GetElementProperty(theElement, "floorConductivity") != null)
                                    {
                                        mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "floorConductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    }
                                    if (Tool.GetElementProperty(theElement, "floorDensity") != null)
                                    {
                                        mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "floorDensity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    }
                                    if (Tool.GetElementProperty(theElement, "floorSpecificHeat") != null)
                                    {
                                        mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "floorSpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    }
                                    if (Tool.GetFloorTypeProperty(slabType, "Absorptance") != null)
                                    {
                                        mat.ThermalAbsortance = Convert.ToDouble(Tool.GetFloorTypeProperty(slabType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                        mat.SolarAbsorptance = Convert.ToDouble(Tool.GetFloorTypeProperty(slabType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                        mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetFloorTypeProperty(slabType, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    }
                                    Material.Add(mat);
                                    //------
                                    co.Name = slabType.GlobalId;
                                    co.OutsideLayer = slabType.GlobalId;
                                    Construction.Add(co);
                                }

                            }

                        }
                        else if (type == "IfcRoof")
                        {
                            var id = element.GlobalId;
                            var theElement = model.Instances.FirstOrDefault<IIfcRoof>(d => d.GlobalId == id);
                            if (theElement != null)
                            {

                                mat.Name = id;
                                mat.Roughness = RoughnessType.Rough;
                                var _theElement = theElement as IIfcRoof;
                                //var DefinedBy = _theElement.ObjectType.Value;
                                //var roofType = model.Instances.FirstOrDefault<IIfcRoofType>(d => d.Name == DefinedBy);
                                if (Tool.GetElementProperty(theElement, "roofThickness") != null)
                                {
                                    mat.Thickness = Convert.ToDouble(Tool.GetElementProperty(theElement, "Thickness").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "roofConductivity") != null)
                                {
                                    mat.Conductivity = Convert.ToDouble(Tool.GetElementProperty(theElement, "roofConductivity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "roofDensity") != null)
                                {
                                    mat.Density = Convert.ToDouble(Tool.GetElementProperty(theElement, "roofDensity").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "roofSpecificHeat") != null)
                                {
                                    mat.SpecificHeat = Convert.ToDouble(Tool.GetElementProperty(theElement, "roofSpecificHeat").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }
                                if (Tool.GetElementProperty(theElement, "Absorptance") != null)
                                {
                                    mat.ThermalAbsortance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.SolarAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                    mat.VisibleAbsorptance = Convert.ToDouble(Tool.GetElementProperty(theElement, "Absorptance").ToString(), CultureInfo.InvariantCulture.NumberFormat);
                                }

                                Material.Add(mat);
                                //------
                                co.Name = id;
                                co.OutsideLayer = id;
                                Construction.Add(co);
                            }
                        }
                    }
                }
                #endregion REVIT


            }
        }
        
    }

    public class UserInputParameters
    {
        public static void Read()
        {
            BIEM.SimulationParameters.Version version = new BIEM.SimulationParameters.Version();
            BIEM.SimulationParameters.Version.Add(version);

            /*
            SimulationParameters.SimulationControl SC = new SimulationParameters.SimulationControl();
            SC.doZoneSizingCalculation = SCA1;
            SC.doSystemSizingCalculation = SCA2;
            SC.doPlantSizingCalculation = SCA3;
            SC.runSimulationforSizingPeriods = SCA4;
            SC.runSimulationforWeatherFileRunPeriods = SCA5;
            SC.doHVACSizingSimulationforSizingPeriods = SCA6;
            SC.maximumNumberofHVACSizingSimulationPasses = SCN1;
            SimulationParametersDAT.SimulationControlDAT.Add(SC);
            SimulationParameters.SimulationControl.Collect(SC);*/

            ShadowCalculation shadowCalculation = new ShadowCalculation();
            shadowCalculation.ShadingCalculationMethod = ShadingCalculationMethodEnum.PolygonClipping;
            shadowCalculation.ShadingCalculationUpdateFrequencyMethod = ShadingCalculationUpdateFrequencyMethodEnum.Periodic;
            shadowCalculation.ShadingCalculationUpdateFrequency = 20;
            shadowCalculation.MaximumFiguresinShadowOverlapCalculations = 15000;
            shadowCalculation.PolygonClippingAlgorithm = PolygonClippingAlgorithmEnum.SutherlandHodgman;
            shadowCalculation.PixelCountingResolution = 512;
            shadowCalculation.SkyDiffuseModelingAlgorithm = SkyDiffuseModelingAlgorithmEnum.SimpleSkyDiffuseModeling;
            shadowCalculation.OutputExternalShadingCalculationResults = OutputExternalShadingCalculationResultsEnum.No;
            shadowCalculation.DisableSelfShadingWithinShadingZoneGroups = DisableSelfShadingWithinShadingZoneGroupsEnum.No;
            shadowCalculation.DisabeSelfShadingFromShadingZoneGroupstoOtherZones = DisabeSelfShadingFromShadingZoneGroupstoOtherZonesEnum.No;
            ShadowCalculation.Add(shadowCalculation);

            SurfaceConvectionAlgorithmInside scai = new SurfaceConvectionAlgorithmInside();
            scai.Algorithm = SurfaceConvectionAlgorithmInsideType.TARP;
            SurfaceConvectionAlgorithmInside.Add(scai);

            SurfaceConvectionAlgorithmOutside scao = new SurfaceConvectionAlgorithmOutside();
            scao.Algorithm = SurfaceConvectionAlgorithmOutsideType.TARP;
            SurfaceConvectionAlgorithmOutside.Add(scao);

            HeatBalanceAlgorithm hba = new HeatBalanceAlgorithm();
            hba.Algorithm = HeatBalanceAlgorithmType.ConductionTransferFunction;
            HeatBalanceAlgorithm.Add(hba);

            Timestep ts = new Timestep();
            ts.NumberofTimestepsperHour = 4;
            Timestep.Add(ts);

            /*
            LocationAndClimate.SizingPeriodDesignDay dd = new LocationAndClimate.SizingPeriodDesignDay();
            dd.Name = "SummerDay";
            dd.Month = 8;
            dd.DayofMonth = 21;
            dd.DayType = "SummerDesignDay";
            dd.WindSpeed = 3.61;
            dd.WindDirection = 30;
            LocationAndClimateDAT.SizingPeriodDesignDayDAT.Add(dd);
            LocationAndClimate.SizingPeriodDesignDay.Collect(dd);*/

            /*
            LocationAndClimate.RunPeriod rp = new LocationAndClimate.RunPeriod();
            rp.Name = "RunPeriodDefault";
            rp.BeginMonth = 1;
            rp.BeginDayofMonth = 1;
            rp.EndMonth = 1;
            rp.EndDayofMonth = 30;
            LocationAndClimateDAT.RunPeriodDAT.Add(rp);
            LocationAndClimate.RunPeriod.Collect(rp);*/

            /*
            SiteGroundTemperatureBuildingSurface gt = new SiteGroundTemperatureBuildingSurface();
            gt.January = 18;
            gt.February = 18;
            gt.March = 18;
            gt.April = 18;
            gt.May = 18;
            gt.June = 18;
            gt.July = 18;
            gt.August = 18;
            gt.September = 18;
            gt.October = 18;
            gt.November = 18;
            gt.December = 18;
            SiteGroundTemperatureBuildingSurface.Add(gt);
            */
            
            ScheduleTypeLimits fraction = new ScheduleTypeLimits();
            fraction.Name = "Fraction";
            fraction.LowerLimitValue = 0;
            fraction.UpperLimitValue = 1;
            fraction.NumericType = NumericType.Continuous;
            ScheduleTypeLimits.Add(fraction);

            ScheduleTypeLimits an = new ScheduleTypeLimits();
            an.Name = "Any Number";
            ScheduleTypeLimits.Add(an);

            ScheduleCompact on = new ScheduleCompact();
            on.Name = "Office Lighting";
            on.ScheduleTypeLimitsName = "Fraction";
            on.Field1 = "Through: 12/31";
            on.Field2 = "For: AllDays";
            on.Field3 = "Until 24:00";
            on.Field4 = "1";
            ScheduleCompact.Add(on);

            ScheduleCompact a4 = new ScheduleCompact();
            a4.Name = "ALWAYS 4";
            a4.ScheduleTypeLimitsName = "Any Number";
            a4.Field1 = "Through: 12/31";
            a4.Field2 = "For: AllDays";
            a4.Field3 = "Until 24:00";
            a4.Field4 = "4";
            ScheduleCompact.Add(a4);

            ScheduleCompact a0 = new ScheduleCompact();
            a0.Name = "ALWAYS 0";
            a0.ScheduleTypeLimitsName = "Any Number";
            a0.Field1 = "Through: 12/31";
            a0.Field2 = "For: AllDays";
            a0.Field3 = "Until 24:00";
            a0.Field4 = "0";
            ScheduleCompact.Add(a0);

            ScheduleCompact a50 = new ScheduleCompact();
            a50.Name = "ALWAYS 50";
            a50.ScheduleTypeLimitsName = "Any Number";
            a50.Field1 = "Through: 12/31";
            a50.Field2 = "For: AllDays";
            a50.Field3 = "Until 24:00";
            a50.Field4 = "50";
            ScheduleCompact.Add(a50);

            GlobalGeometryRules ggr = new GlobalGeometryRules();
            ggr.StartingVertexPosition = StartingVertexPositionEnum.LowerLeftCorner;
            ggr.VertexEntryDirection = VertexEntryDirectionEnum.CounterClockwise;
            ggr.CoordinateSystem = CoordinateSystemEnum.Relative;
            GlobalGeometryRules.Add(ggr);

            OutputVariableDictionary vd = new OutputVariableDictionary();
            vd.KeyField = KeyFieldType.regular;
            vd.SortOption = SortOptionType.Name;
            OutputVariableDictionary.Add(vd);

            OutputVariable ov1 = new OutputVariable();
            ov1.KeyValue = "*";
            ov1.VariableName = "Zone Mean Radiant Temperature";
            ov1.ReportingFrequency = ReportingFrequencyType.Timestep;
            OutputVariable.Add(ov1);

            OutputVariable ov2 = new OutputVariable();
            ov2.KeyValue = "*";
            ov2.VariableName = "Zone Mean Radiant Temperature";
            ov2.ReportingFrequency = ReportingFrequencyType.Timestep;
            OutputVariable.Add(ov2);

            OutputVariable ov3 = new OutputVariable();
            ov3.KeyValue = "*";
            ov3.VariableName = "Zone Mean Radiant Temperature";
            ov3.ReportingFrequency = ReportingFrequencyType.Timestep;
            OutputVariable.Add(ov3);

            OutputVariable ov4 = new OutputVariable();
            ov4.KeyValue = "*";
            ov4.VariableName = "";
            ov4.ReportingFrequency = ReportingFrequencyType.Timestep;
            OutputVariable.Add(ov4);

            OutputSurfaceDrawing outputSurfaceDrawing = new OutputSurfaceDrawing();
            outputSurfaceDrawing.ReporType = ReportType.DXF;
            outputSurfaceDrawing.ReportSpecifications1 = ReportSpecifications1Type.RegularPolyline;
            OutputSurfaceDrawing.Add(outputSurfaceDrawing);



        }
    }

    
}
