using System.IO;
using System.Collections.Generic;


namespace BIEM.SimulationParameters
{
    public class Version
    {
        private double? _versionIdentifier = 9.5;
        public double? VersionIdentifier
        {
            get => _versionIdentifier;
            set => _versionIdentifier = value;
        }

        public Version() { }
        private static List<Version> list = new List<Version>();

        public static void Add(Version version)
        {
            list.Add(version);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"Version,\n" +
                    ($"  {list[i].VersionIdentifier}" + ";").PadRight(27, ' ') + " !-Version Identifier";

            }

            return print;
        }

        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                foreach (string line in Version.Read())
                {
                    sw.WriteLine(line);
                }
            }

        }
        public static void Collect(SimulationParameters.Version version)
        {
            version.VersionIdentifier = null;
        }

    }
}
