using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace BIEM
{

    public class Errors
    {
        public string Class { get; set; }
        public string Field { get; set; }
        
        public Errors() { }


        private static List<Errors> list = new List<Errors>();

        public static void Add(Errors errors)
        {
            list.Add(errors);
        }
        private static string[] Read()
        {
            string[] print = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                print[i] = $"{i+1} - An instance in the class {list[i].Class}, has no {list[i].Field}  associated.";
            }

            return print;
        }

        
        public static void Get()
        {
            string directory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\BIEM";
            string er = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\BIEM\ErrorList.txt";
            if (Directory.Exists(directory))
            {

                if (File.Exists(er))
                {
                    File.Delete(er);
                }
                using (StreamWriter sw = File.AppendText(er))
                {
                    foreach (string line in Errors.Read())
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(directory);

                
                if (File.Exists(er))
                {
                    File.Delete(er);
                }
                using (StreamWriter sw = File.AppendText(er))
                {
                    foreach (string line in Errors.Read())
                    {
                        sw.WriteLine(line);
                    }
                }
            }

            var p = new Process();
            p.StartInfo = new ProcessStartInfo(er)
            {
                UseShellExecute = true
            };
            p.Start();


        }
        public static void Clear()
        {
            list.Clear();

        }
    }

}
