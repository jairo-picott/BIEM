using System;
using System.Collections.Generic;
using System.IO;

namespace BIEM.OutputReporting
{
    public enum ActuatorAvailabilityDictionaryReportingEnum
    {
        None,
        NotByUniqueKeyNames,
        Verbose
    }

    public enum InternalVariableAvailabilityDictionaryReportingEnum
    {
        None,
        NotByUniqueKeyNames,
        Verbose
    }

    public enum EMSRuntimeLanguageDebugOutputLevelEnum
    {
        None,
        ErrorsOnly,
        Verbose
    }
    public class OutputEnergyManagementSystem
    {
        private ActuatorAvailabilityDictionaryReportingEnum? _actuatorAvailabilityDictionaryReporting = ActuatorAvailabilityDictionaryReportingEnum.None;
        private InternalVariableAvailabilityDictionaryReportingEnum? _internalVariableAvailabilityDictionaryReporting = InternalVariableAvailabilityDictionaryReportingEnum.None;
        private EMSRuntimeLanguageDebugOutputLevelEnum? _EMSRuntimeLanguageDebugOutputLevel = EMSRuntimeLanguageDebugOutputLevelEnum.None;

        public ActuatorAvailabilityDictionaryReportingEnum? ActuatorAvailabilityDictionaryReporting { get => _actuatorAvailabilityDictionaryReporting; set => _actuatorAvailabilityDictionaryReporting = value; }
        public InternalVariableAvailabilityDictionaryReportingEnum? InternalVariableAvailabilityDictionaryReporting { get => _internalVariableAvailabilityDictionaryReporting; set => _internalVariableAvailabilityDictionaryReporting = value; }
        public EMSRuntimeLanguageDebugOutputLevelEnum? EMSRuntimeLanguageDebugOutputLevel { get => _EMSRuntimeLanguageDebugOutputLevel; set => _EMSRuntimeLanguageDebugOutputLevel = value; }

        public OutputEnergyManagementSystem() { }

        private static List<OutputEnergyManagementSystem> list = new List<OutputEnergyManagementSystem>();

        public static void Add(OutputEnergyManagementSystem output)
        {
            list.Add(output);

        
        }

        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Output:EnergyManagementSystem,\n" +
                    ($"  {list[i].ActuatorAvailabilityDictionaryReporting}" + ",").PadRight(27, ' ') + " !-Actuator Availability Dictionary Reporting\n" +
                    ($"  {list[i].InternalVariableAvailabilityDictionaryReporting}" + ",").PadRight(27, ' ') + " !-Internal Variable Availability Dictionary Reporting\n" +
                    ($"  {list[i].EMSRuntimeLanguageDebugOutputLevel}" + ";").PadRight(27, ' ') + " !-EMS Runtime Language Debug Output Level";

            }
            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in OutputEnergyManagementSystem.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }

        public static void Get()
        {

            foreach (string line in OutputEnergyManagementSystem.Read())
            {
                Console.WriteLine(line);
            }


        }
        public static void Clear()
        {
            list.Clear();
        }
    }
}
