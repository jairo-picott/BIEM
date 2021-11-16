using BIEM.InternalGains;
using BIEM.LocationAndClimate;
using BIEM.Schedules;
using BIEM.SimulationParameters;
using BIEM.SurfaceConstructionElements;
using BIEM.ThermalZonesandSurfaces;
using BIEM.ZoneAirFlow;
using BIEM.ZoneHVACConstrolsandThermostats;
using BIEM.ZoneHVACForcedAirUnits;
using BIEM.ZoneHVACEquipmentConnections;
using BIEM.OutputReporting;
using BIEM.ComplianceObjects;

namespace BIEM
{

    public class PrinterIDF
    {
        public static void Print(string idfFile)
        {
            SimulationParametersLabel.Get(idfFile);
            Version.Get(idfFile);
            SimulationControl.Get(idfFile);
            PerformancePrecisionTradeoffs.Get(idfFile);
            Building.Get(idfFile);
            ShadowCalculation.Get(idfFile);
            SurfaceConvectionAlgorithmInside.Get(idfFile);
            SurfaceConvectionAlgorithmOutside.Get(idfFile);
            HeatBalanceAlgorithm.Get(idfFile);
            HeatBalanceSettingsConductionFiniteDifference.Get(idfFile);
            ZoneAirHeatBalanceAlgorithm.Get(idfFile);
            ZoneAirContaminantBalance.Get(idfFile);
            ZoneAirMassFlowConservation.Get(idfFile);
            ZoneCapacitanceMultiplierResearchSpecial.Get(idfFile);
            Timestep.Get(idfFile);
            ConvergenceLimits.Get(idfFile);
            HVACSystemRootFindingAlgorithm.Get(idfFile);

            //-----
            ComplianceObjectsLabel.Get(idfFile);
            ComplianceBuilding.Get(idfFile);

            //----
            LocationandClimateLabel.Get(idfFile);
            SiteLocation.Get(idfFile);
            SiteVariableLocation.Get(idfFile);
            SizingPeriodDesignDay.Get(idfFile);
            SizingPeriodWeatherFileDays.Get(idfFile);
            SizingPeriodWeatherFileConditionType.Get(idfFile);
            RunPeriod.Get(idfFile);
            RunPeriodControlSpecialDays.Get(idfFile);
            RunPeriodControlDaylightSavingTime.Get(idfFile);
            WeatherPropertySkyTemperature.Get(idfFile);
            SiteWeatherStation.Get(idfFile);
            SiteHeightVariation.Get(idfFile);
            SiteGroundTemperatureBuildingSurface.Get(idfFile);
            SiteGroundTemperatureFCfactorMethod.Get(idfFile);
            SiteGroundTemperatureShallow.Get(idfFile);
            SiteGroundTemperatureDeep.Get(idfFile);
            SiteGroundTemperatureUndisturbedFiniteDifference.Get(idfFile);
            SiteGroundTemperatureUndisturbedKusudaAchenbach.Get(idfFile);
            SiteGroundTemperatureUndisturbedXing.Get(idfFile);
            SiteGroundDomainSlab.Get(idfFile);
            SiteGroundDomainBasement.Get(idfFile);
            SiteGroundReflectance.Get(idfFile);
            SiteGroundReflectanceSnowModifier.Get(idfFile);
            SiteWaterMainsTemperature.Get(idfFile);
            SitePrecipitation.Get(idfFile);
            RoofIrrigation.Get(idfFile);
            SiteSolarAndVisibleSpectrum.Get(idfFile);
            SiteSpectrumData.Get(idfFile);

            //---
            SchedulesLabel.Get(idfFile);
            ScheduleTypeLimits.Get(idfFile);
            ScheduleDayHourly.Get(idfFile);
            ScheduleDayInterval.Get(idfFile);
            ScheduleWeekDaily.Get(idfFile);
            ScheduleWeekCompact.Get(idfFile);
            ScheduleCompact.Get(idfFile);
            ScheduleConstant.Get(idfFile);
            ScheduleFileShading.Get(idfFile);
            ScheduleFile.Get(idfFile);

            //---
            Material.Get(idfFile);
            WindowMaterialGlazing.Get(idfFile);
            WindowMaterialSimpleGlazingSystem.Get(idfFile);
            Construction.Get(idfFile);

            //---
            GlobalGeometryRules.Get(idfFile);
            Zone.Get(idfFile);
            BuildingSurfaceDetailed.Get(idfFile);
            FenestrationSurfaceDetailed.Get(idfFile);

            //---
            People.Get(idfFile);
            Lights.Get(idfFile);
            ElectricEquipments.Get(idfFile);

            //---
            ZoneInfiltrationDesignFlowRate.Get(idfFile);
            ZoneVentilationDesignFlowRate.Get(idfFile);
            ZoneVentilationWindandStackOpenArea.Get(idfFile);

            //---
            ZoneControlThermostat.Get(idfFile);
            ThermostatSetpointDualSetpoint.Get(idfFile);

            //---
            ZoneHVACIdealLoadsAirSystem.Get(idfFile);

            //---
            ZoneHVACEquipmentList.Get(idfFile);
            ZoneHVACEquipmentConnections.ZoneHVACEquipmentConnections.Get(idfFile);

            //---
            OutputVariableDictionary.Get(idfFile);
            OutputVariable.Get(idfFile);
            OutputSurfaceDrawing.Get(idfFile);

        }
        
        

    }

    public class ErrorPrinter
    {
        public static void Print()
        {
            Errors.Get();
        }
    }
}
