using System.IO;
using System.Collections.Generic;

namespace BIEM.ZoneAirFlow
{
    public class ZoneVentilationWindandStackOpenArea
    {
        private string _name;
        public string Name { get=>_name; set=>_name=value; }

        private string _zoneName;
        public string ZoneName { get=>_zoneName; set=>_zoneName=value; }

        private double? _openingArea;
        public double? OpeningArea
        {
            get => _openingArea;
            set
            {
                if (value >= 0)
                {
                    _openingArea = value;
                }
            }
        }

        private string _openingAreaFractionScheduleName;
        public string OpeningAreaFractionScheduleName { get=>_openingAreaFractionScheduleName; set=>_openingAreaFractionScheduleName=value; }

        private string _openingEffectiveness = "autocalculate";
        public string OpeningEffectiveness { get=>_openingEffectiveness; set=>_openingEffectiveness=value; }

        private double? _effectiveAngle = 0;
        public double? EffectiveAngle
        {
            get => _effectiveAngle;
            set
            {
                if (value>=0 && value <=360)
                {
                    _effectiveAngle = value;
                }
            }
        }

        private double? _heightDifference;
        public double? HeightDifference
        {
            get => _heightDifference;
            set
            {
                if (value >= 0)
                {
                    _heightDifference = value;
                }
            }
        }

        private string _dichargeCoefficientforOpening = "autocalculate";
        public string DischargeCoefficientforOpening { get=>_dichargeCoefficientforOpening; set=>_dichargeCoefficientforOpening=value; }

        private double? _minimumIndoorTemperature = -100;
        public double? MinimumIndoorTemperature
        {
            get => _minimumIndoorTemperature;
            set
            {
                if (value >= -100 && value <= 100)
                {
                    _minimumIndoorTemperature = value;
                }
            }
        }

        private string _minimumIndoorTemperatureScheduleName;
        public string MinimumIndoorTemperatureScheduleName { get => _minimumIndoorTemperatureScheduleName; set => _minimumIndoorTemperatureScheduleName = value; }

        private double? _maximumIndoorTemperature = 100;
        public double? MaximumIndoorTemperature
        {
            get => _maximumIndoorTemperature;
            set
            {
                if (value >= -100 && value <= 100)
                {
                    _maximumIndoorTemperature = value;
                }
            }
        }

        private string _maximumIndoorTemperatureScheduleName;
        public string MaximumIndoorTemperatureScheduleName { get => _maximumIndoorTemperatureScheduleName; set => _maximumIndoorTemperatureScheduleName = value; }

        private double? _deltaTemperature = -100;
        public double? DeltaTemperature
        {
            get => _deltaTemperature;
            set
            {
                if (value >= -100)
                {
                    _deltaTemperature = value;
                }
            }
        }

        private string _deltaTemperatureScheduleName;
        public string DeltaTemperatureScheduleName { get => _deltaTemperatureScheduleName; set => _deltaTemperatureScheduleName = value; }

        private double? _minimumOutdoorTemperature = -100;
        public double? MinimumOutdoorTemperature
        {
            get => _minimumIndoorTemperature;
            set
            {
                if (value >= -100 && value <= 100)
                {
                    _minimumIndoorTemperature = value;
                }
            }
        }

        private string _minimumOutdoorTemperatureScheduleName;
        public string MinimumOutdoorTemperatureScheduleName { get => _minimumOutdoorTemperatureScheduleName; set => _minimumOutdoorTemperatureScheduleName = value; }

        private double? _maximumOutdoorTemperature = 100;
        public double? MaximumOutdoorTemperature
        {
            get => _maximumOutdoorTemperature;
            set
            {
                if (value >= -100 && value <= 100)
                {
                    _maximumOutdoorTemperature = value;
                }
            }
        }

        private string _maximumOutdoorTemperatureScheduleName;
        public string MaximumOutdoorTemperatureScheduleName { get => _maximumOutdoorTemperatureScheduleName; set => _maximumOutdoorTemperatureScheduleName = value; }

        private double? _maximumWindSpeed = 40;
        public double? MaximumWindSpeed
        {
            get => _maximumWindSpeed;
            set
            {
                if (value >= 0 && value <= 40)
                {
                    _maximumWindSpeed = value;
                }
            }
        }
        public ZoneVentilationWindandStackOpenArea() { }

        private static List<ZoneVentilationWindandStackOpenArea> list = new List<ZoneVentilationWindandStackOpenArea>();

        public static void Add(ZoneVentilationWindandStackOpenArea zoneVentilationWindandStackOpenArea)
        {
            list.Add(zoneVentilationWindandStackOpenArea);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"ZoneVentilation:WindandStackOpenArea,\n" +
                    ($"  {list[i].Name}" + ",").PadRight(27, ' ') + " !-Name\n" +
                    ($"  {list[i].ZoneName}" + ",").PadRight(27, ' ') + " !-Zone Name\n" +
                    ($"  {list[i].OpeningArea}" + ",").PadRight(27, ' ') + " !-Opening Area{{ m2 }}\n" +
                    ($"  {list[i].OpeningAreaFractionScheduleName}" + ",").PadRight(27, ' ') + " !-Opening Area Fraction Schedule Name\n" +
                    ($"  {list[i].OpeningEffectiveness}" + ",").PadRight(27, ' ') + " !-Opening Effectiveness{{ dimensionless }}\n" +
                    ($"  {list[i].EffectiveAngle}" + ",").PadRight(27, ' ') + " !-Effective Angle{{ deg }}\n" +
                    ($"  {list[i].HeightDifference}" + ",").PadRight(27, ' ') + " !-Height Difference{{ m }}\n" +
                    ($"  {list[i].DischargeCoefficientforOpening}" + ",").PadRight(27, ' ') + " !-Discharge Coefficient for Opening\n" +
                    ($"  {list[i].MinimumIndoorTemperature}" + ",").PadRight(27, ' ') + " !-Minimum Indoor Temperature{{ C }}\n" +
                    ($"  {list[i].MinimumIndoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Minimum Indoor Temperature Schedule Name\n" +
                    ($"  {list[i].MaximumIndoorTemperature}" + ",").PadRight(27, ' ') + " !-Maximum Indoor Temperature{{ C }}\n" +
                    ($"  {list[i].MaximumIndoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Maximum Indoor Temperature Schedule Name\n" +
                    ($"  {list[i].DeltaTemperature}" + ",").PadRight(27, ' ') + " !-Delta Temperature{{ deltaC }}\n" +
                    ($"  {list[i].DeltaTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Delta Temperature Schedule Name\n" +
                    ($"  {list[i].MinimumOutdoorTemperature}" + ",").PadRight(27, ' ') + " !-Minimum Outdoor Temperature{{ C }}\n" +
                    ($"  {list[i].MinimumOutdoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Minimum Outdoor Temperature Schedule Name\n" +
                    ($"  {list[i].MaximumOutdoorTemperature}" + ",").PadRight(27, ' ') + " !-Maximum Outdoor Temperature{{ C }}\n" +
                    ($"  {list[i].MaximumOutdoorTemperatureScheduleName}" + ",").PadRight(27, ' ') + " !-Maximum Outdoor Temperature Schedule Name\n" +
                    ($"  {list[i].MaximumWindSpeed}" + ";").PadRight(27, ' ') + " !-Maximum Wind Speed{{ m / s }}";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in ZoneVentilationWindandStackOpenArea.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }


        public static void Collect(ZoneVentilationWindandStackOpenArea zoneVentilation)
        {
            zoneVentilation.Name = null;
            zoneVentilation.ZoneName = null;
            zoneVentilation.OpeningArea = null;
            zoneVentilation.OpeningAreaFractionScheduleName = null;
            zoneVentilation.OpeningEffectiveness = null;
            zoneVentilation.EffectiveAngle = null;
            zoneVentilation.HeightDifference = null;
            zoneVentilation.DischargeCoefficientforOpening = null;
            zoneVentilation.MinimumIndoorTemperature = null;
            zoneVentilation.MinimumIndoorTemperatureScheduleName = null;
            zoneVentilation.MaximumIndoorTemperature = null;
            zoneVentilation.MaximumIndoorTemperatureScheduleName = null;
            zoneVentilation.DeltaTemperature = null;
            zoneVentilation.DeltaTemperatureScheduleName = null;
            zoneVentilation.MinimumOutdoorTemperature = null;
            zoneVentilation.MinimumOutdoorTemperatureScheduleName = null;
            zoneVentilation.MaximumOutdoorTemperature = null;
            zoneVentilation.MaximumOutdoorTemperatureScheduleName = null;
            zoneVentilation.MaximumWindSpeed = null;
        }
    }
}
