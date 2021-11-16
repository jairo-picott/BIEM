using System.IO;

namespace BIEM.ComplianceObjects
{
    public class ComplianceObjectsLabel
    {
        public static void Get(string idfFile)
        {
            using (StreamWriter sw = File.AppendText(idfFile))
            {

                sw.WriteLine("!!----ALL THE OBJECTS OF COMPLIANCE OBJECTS----");

            }

        }
    }
}
