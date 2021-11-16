using System.IO;
using System.Collections.Generic;

namespace BIEM.SimulationParameters
{
    public enum ZoneSizingCalculationType
    {
        Yes,
        No
    }
    public enum SystemSizingCalculationType
    {
        Yes,
        No
    }
    public enum PlantSizingCalculationType
    {
        Yes,
        No
    }
    public enum SimulationforSizingPeriodsType
    {
        Yes,
        No
    }
    public enum SimulationforWeatherFileRunPeriodsType
    {
        Yes,
        No
    }
    
    public enum HVACSizingSimulationforSizingPeriodsType
    {
        Yes,
        No
    }
    
    //---------------SimulationControl
    //ASK FOR USER THE INSTRUCTIONS AND TYPE OF SIMULATION MUST BE DONE
    //THIS CAN BE REPLAE WITH CHECK BOX IN AND GRAPHIC INTERFACE
    //THE DEFAULT VALUES ARE SET FOR CARRY ON BASIC SIMULATION INCLUDING
    //WEATHER FILES AND SIZING PERIODS
    public class SimulationControl
    {
        private ZoneSizingCalculationType? _doZoneSizingCalculation;
        public ZoneSizingCalculationType? DoZoneSizingCalculation
        {
            get => _doZoneSizingCalculation;
            set => _doZoneSizingCalculation = value;
        }

        private SystemSizingCalculationType? _doSistemSizingCalculation;
        public SystemSizingCalculationType? DoSystemSizingCalculation
        {
            get => _doSistemSizingCalculation;
            set => _doSistemSizingCalculation = value;
        }

        private PlantSizingCalculationType? _doPlantSizingCalculation;
        public PlantSizingCalculationType? DoPlantSizingCalculation
        {
            get => _doPlantSizingCalculation;
            set => _doPlantSizingCalculation = value;
        }

        private SimulationforSizingPeriodsType? _runSimulationforSizingPeriods;
        public SimulationforSizingPeriodsType? RunSimulationforSizingPeriods
        {
            get => _runSimulationforSizingPeriods;
            set => _runSimulationforSizingPeriods = value;
        }

        private SimulationforWeatherFileRunPeriodsType? _runSimulationforWeatherFileRunPeriods;
        public SimulationforWeatherFileRunPeriodsType? RunSimulationforWeatherFileRunPeriods
        {
            get => _runSimulationforWeatherFileRunPeriods;
            set => _runSimulationforWeatherFileRunPeriods = value;
        }

        private HVACSizingSimulationforSizingPeriodsType? _doHVACSizingSimulationforSizingPeriods;
        public HVACSizingSimulationforSizingPeriodsType? DoHVACSizingSimulationforSizingPeriods
        {
            get => _doHVACSizingSimulationforSizingPeriods;
            set => _doHVACSizingSimulationforSizingPeriods = value;
        }

        private int? _maximumNumberofHVACSizingSimulationPasses = 1;
        public int? MaximumNumberofHVACSizingSimulationPasses
        {
            get => _maximumNumberofHVACSizingSimulationPasses;
            set
            {
                if (value >= 1)
                {
                    _maximumNumberofHVACSizingSimulationPasses = value;
                }
            }
        }

        public SimulationControl() { }

        private static List<SimulationControl> list = new List<SimulationControl>();

        public static void Add(SimulationControl simulationControl)
        {
            list.Add(simulationControl);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"SimulationControl,\n" +
                    ($"  {list[i].DoZoneSizingCalculation}" + ",").PadRight(27, ' ') + " !-Do Zone Sizing Calculation\n" +
                    ($"  {list[i].DoSystemSizingCalculation}" + ",").PadRight(27, ' ') + " !-Do System Sizing Calculation\n" +
                    ($"  {list[i].DoPlantSizingCalculation}" + ",").PadRight(27, ' ') + " !-Do Plant Sizing Calculation\n" +
                    ($"  {list[i].RunSimulationforSizingPeriods}" + ",").PadRight(27, ' ') + " !-RunSimulationforSizingPeriods\n" +
                    ($"  {list[i].RunSimulationforWeatherFileRunPeriods}" + ",").PadRight(27, ' ') + " !-Run Simulation for Weather File Run Periods\n" +
                    ($"  {list[i].DoHVACSizingSimulationforSizingPeriods}" + ",").PadRight(27, ' ') + " !-Do HVAC Sizing Simulation for Sizing Periods\n" +
                    ($"  {list[i].MaximumNumberofHVACSizingSimulationPasses}" + ";").PadRight(27, ' ') + " !-Maximum Number of HVAC Sizing Simulation Passes";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in SimulationControl.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(SimulationParameters.SimulationControl simulation)
        {
            simulation.DoZoneSizingCalculation = null;
            simulation.DoSystemSizingCalculation = null;
            simulation.DoPlantSizingCalculation = null;
            simulation.RunSimulationforSizingPeriods = null;
            simulation.RunSimulationforWeatherFileRunPeriods = null;
            simulation.DoHVACSizingSimulationforSizingPeriods = null;
            simulation.MaximumNumberofHVACSizingSimulationPasses = null;
        }
        
    }
}
