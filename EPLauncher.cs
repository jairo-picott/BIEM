using System.Diagnostics;
using System.IO;



namespace BIEM
{
    public class EPLauncher
    {
        
        public static void EPLaunch(string WFile, string version, string idffile)
        {

            string BIEMpath = $@"C:\{version}\RunEPlusBIEM.bat";
            string IDFFIle = $@"{idffile}";
            Process p = new Process();

            if (File.Exists(BIEMpath))
            {
               
                
                p.StartInfo.FileName = BIEMpath;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.Arguments =$"{IDFFIle}, {WFile}";
                p.Start();
                
            }

            

            
            

        }
        
        public static void GetFormat(string version, string idffile)
        {
            //OPENING IDF EDITOR
            string IDFfile = $@"{idffile}.idf";
            string IDFEditor = $@"C:\{version}\PreProcess\IDFEditor\IDFEditor.exe";
            Process e = new Process();
            e.StartInfo = new ProcessStartInfo(IDFEditor, IDFfile)
            {
                UseShellExecute = false
            };
            e.Start();
            //-----------------------
            //CREATING BAT FILE TO GET READY FOR THE SIMULATION

            string path = $@"C:\{version}\RunEPlus.bat";
            string BIEMpath = $@"C:\{version}\RunEPlusBIEM.bat";
            if (File.Exists(BIEMpath))
            {
                File.Delete(BIEMpath);
            }

            StreamReader sr = new StreamReader(path);
            StreamWriter sw = new StreamWriter(BIEMpath);

            string lines = sr.ReadToEnd();
            if (lines != null)
            {
                if (lines == " set input_path=ExampleFiles\\")
                {
                    sw.WriteLine(" set input_path=");
                }
                else if (lines == " set output_path=ExampleFiles\\Outputs\\")
                {
                    sw.WriteLine(" set output_path=");
                }
                else
                {
                    sw.WriteLine(lines);
                }
            }
            sw.Close();
            sr.Close();
            
            
            
            
            
        }





    }
}
