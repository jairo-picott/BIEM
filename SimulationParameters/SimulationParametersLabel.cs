using System.IO;

namespace BIEM.SimulationParameters
{
    public class SimulationParametersLabel
    {
        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {
                sw.WriteLine("!!----File Generated using BIEM: BUIILDING INFORMATION AND ENERGY MODELING----------------");
                sw.WriteLine("!!----Institute for Sutainability and Innovation in Structural Engineering----------------");
                sw.WriteLine("!!----University of Minho, Portugal-------------------------------------------------------");
                sw.WriteLine("!!----Funded by: Cognitive CMMS - Cognitive Computerized Maintenance Management System----");
                sw.WriteLine("!!----Created by: Jairo B. Picott - jairobpicott@gmail.com--------------------------------");
                sw.WriteLine("!!----2021--------------------------------------------------------------------------------");
                sw.WriteLine("!!----------------------------------------------------------------------------------------");
                sw.WriteLine("!!----------------------------------------------------------------------------------------");
                sw.WriteLine("!!----------------------------------------------------------------------------------------");
                sw.WriteLine("!!----------------------------------------------------------------------------------------");
                sw.WriteLine("!!----ALL THE OBJECTS OF SIMULATION PARAMETERS----");
                
            }

        }
    }
}
