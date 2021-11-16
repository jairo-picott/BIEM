using System.Collections.Generic;
using System.IO;

namespace BIEM.SimulationParameters
{
    public enum ShadingCalculationMethodEnum
    {
        PolygonClipping,
        PixelCounting,
        Scheduled,
        Imported
    }

    public enum ShadingCalculationUpdateFrequencyMethodEnum
    {
        Periodic,
        Timestep
    }

    public enum PolygonClippingAlgorithmEnum
    {
        SutherlandHodgman,
        ConvexWeilerAtherton,
        SlaterBarskyandSutherlandHodgman
    }

    public enum SkyDiffuseModelingAlgorithmEnum
    {
        SimpleSkyDiffuseModeling,
        DetailedSkyDiffuseModeling
    }

    public enum OutputExternalShadingCalculationResultsEnum
    {
        Yes,
        No
    }

    public enum DisableSelfShadingWithinShadingZoneGroupsEnum
    {
        Yes,
        No
    }

    public enum DisabeSelfShadingFromShadingZoneGroupstoOtherZonesEnum
    {
        Yes,
        No
    }
    public class ShadowCalculation
    {
        private ShadingCalculationMethodEnum _shadingCalculationMethod = ShadingCalculationMethodEnum.PolygonClipping;
        public ShadingCalculationMethodEnum ShadingCalculationMethod { get => _shadingCalculationMethod; set => _shadingCalculationMethod = value; }

        private ShadingCalculationUpdateFrequencyMethodEnum _shadingCalculationUpdateFrequencyMethod = ShadingCalculationUpdateFrequencyMethodEnum.Periodic;
        public ShadingCalculationUpdateFrequencyMethodEnum ShadingCalculationUpdateFrequencyMethod { get => _shadingCalculationUpdateFrequencyMethod; set => _shadingCalculationUpdateFrequencyMethod = value; }

        private double _shadingCalculationUpdateFrequency = 20;
        public double ShadingCalculationUpdateFrequency
        {
            get => _shadingCalculationUpdateFrequency;
            set
            {
                if (value>=1)
                {
                    _shadingCalculationUpdateFrequency = value;
                }
            }
        }

        private double _maximumFiguresinShadowOverlapCalculations = 15000;
        public double MaximumFiguresinShadowOverlapCalculations
        {
            get => _maximumFiguresinShadowOverlapCalculations;
            set
            {
                if (value >= 200)
                {
                    _maximumFiguresinShadowOverlapCalculations = value;
                }
            }
        }

        private PolygonClippingAlgorithmEnum _polygonClippingAlgorithm = PolygonClippingAlgorithmEnum.SutherlandHodgman;
        public PolygonClippingAlgorithmEnum PolygonClippingAlgorithm { get => _polygonClippingAlgorithm; set => _polygonClippingAlgorithm = value; }

        private double _pixelCountingResolution = 512;
        public double PixelCountingResolution { get => _pixelCountingResolution; set => _pixelCountingResolution = value; }

        private SkyDiffuseModelingAlgorithmEnum _skyDiffuseModelingAlgorithm = SkyDiffuseModelingAlgorithmEnum.SimpleSkyDiffuseModeling;
        public SkyDiffuseModelingAlgorithmEnum SkyDiffuseModelingAlgorithm { get => _skyDiffuseModelingAlgorithm; set => _skyDiffuseModelingAlgorithm = value; }

        private OutputExternalShadingCalculationResultsEnum _outputExternalShadingCalculationResults = OutputExternalShadingCalculationResultsEnum.No;
        public OutputExternalShadingCalculationResultsEnum OutputExternalShadingCalculationResults { get => _outputExternalShadingCalculationResults; set => _outputExternalShadingCalculationResults = value; }

        private DisableSelfShadingWithinShadingZoneGroupsEnum _disableSelfShadingWithinShadingZoneGroups = DisableSelfShadingWithinShadingZoneGroupsEnum.No;
        public DisableSelfShadingWithinShadingZoneGroupsEnum DisableSelfShadingWithinShadingZoneGroups { get => _disableSelfShadingWithinShadingZoneGroups; set => _disableSelfShadingWithinShadingZoneGroups = value; }

        private DisabeSelfShadingFromShadingZoneGroupstoOtherZonesEnum _disabeSelfShadingFromShadingZoneGroupstoOtherZones = DisabeSelfShadingFromShadingZoneGroupstoOtherZonesEnum.No;
        public DisabeSelfShadingFromShadingZoneGroupstoOtherZonesEnum DisabeSelfShadingFromShadingZoneGroupstoOtherZones { get => _disabeSelfShadingFromShadingZoneGroupstoOtherZones; set => _disabeSelfShadingFromShadingZoneGroupstoOtherZones = value; }

        private string _shadingZoneGroup1ZoneListName;
        public string ShadingZoneGroup1ZoneListName { get => _shadingZoneGroup1ZoneListName; set => _shadingZoneGroup1ZoneListName = value; }
        private string _shadingZoneGroup2ZoneListName;
        public string ShadingZoneGroup2ZoneListName { get => _shadingZoneGroup2ZoneListName; set => _shadingZoneGroup2ZoneListName = value; }
        private string _shadingZoneGroup3ZoneListName;
        public string ShadingZoneGroup3ZoneListName { get => _shadingZoneGroup3ZoneListName; set => _shadingZoneGroup3ZoneListName = value; }
        private string _shadingZoneGroup4ZoneListName;
        public string ShadingZoneGroup4ZoneListName { get => _shadingZoneGroup4ZoneListName; set => _shadingZoneGroup4ZoneListName = value; }
        private string _shadingZoneGroup5ZoneListName;
        public string ShadingZoneGroup5ZoneListName { get => _shadingZoneGroup5ZoneListName; set => _shadingZoneGroup5ZoneListName = value; }
        private string _shadingZoneGroup6ZoneListName;
        public string ShadingZoneGroup6ZoneListName { get => _shadingZoneGroup6ZoneListName; set => _shadingZoneGroup6ZoneListName = value; }

        public ShadowCalculation() { }

        private static List<ShadowCalculation> list = new List<ShadowCalculation>();

        public static void Add(ShadowCalculation shadowCalculation)
        {
            list.Add(shadowCalculation);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ShadowCalculation,\n" +
                    ($"  {list[i].ShadingCalculationMethod}" + ",").PadRight(27, ' ') + " !-Shading Calculation Method\n" +
                    ($"  {list[i].ShadingCalculationUpdateFrequencyMethod}" + ",").PadRight(27, ' ') + " !-Shading Calculation Update Frequency Method\n" +
                    ($"  {list[i].ShadingCalculationUpdateFrequency}" + ",").PadRight(27, ' ') + " !-Shading Calculation Update Frequency\n" +
                    ($"  {list[i].MaximumFiguresinShadowOverlapCalculations}" + ",").PadRight(27, ' ') + " !-Maximum Figures in Shadow Overlap Calculations\n" +
                    ($"  {list[i].PolygonClippingAlgorithm}" + ",").PadRight(27, ' ') + " !-Polygon Clipping Algorithm\n" +
                    ($"  {list[i].PixelCountingResolution}" + ",").PadRight(27, ' ') + " !-Pixel Counting Resolution\n" +
                    ($"  {list[i].SkyDiffuseModelingAlgorithm}" + ",").PadRight(27, ' ') + " !-Sky Diffuse Modeling Algorithm\n" +
                    ($"  {list[i].OutputExternalShadingCalculationResults}" + ",").PadRight(27, ' ') + " !-Output External Shading Calculation Results\n" +
                    ($"  {list[i].DisableSelfShadingWithinShadingZoneGroups}" + ",").PadRight(27, ' ') + " !-Disable Self Shading Within Shading Zone Groups\n" +
                    ($"  {list[i].DisabeSelfShadingFromShadingZoneGroupstoOtherZones}" + ",").PadRight(27, ' ') + " !-Disabe Self Shading From Shading Zone Groups to Other Zones\n" +
                    ($"  {list[i].ShadingZoneGroup1ZoneListName}" + ",").PadRight(27, ' ') + " !-Shading Zone Group 1 Zone List Name\n" +
                    ($"  {list[i].ShadingZoneGroup2ZoneListName}" + ",").PadRight(27, ' ') + " !-Shading Zone Group 2 Zone List Name\n" +
                    ($"  {list[i].ShadingZoneGroup3ZoneListName}" + ",").PadRight(27, ' ') + " !-Shading Zone Group 3 Zone List Name\n" +
                    ($"  {list[i].ShadingZoneGroup4ZoneListName}" + ",").PadRight(27, ' ') + " !-Shading Zone Group 4 Zone List Name\n" +
                    ($"  {list[i].ShadingZoneGroup5ZoneListName}" + ",").PadRight(27, ' ') + " !-Shading Zone Group 5 Zone List Name\n" +
                    ($"  {list[i].ShadingZoneGroup6ZoneListName}" + ";").PadRight(27, ' ') + " !-Shading Zone Group 6 Zone List Name";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ShadowCalculation.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Clear()
        {
            list.Clear();
        }

    }
}
