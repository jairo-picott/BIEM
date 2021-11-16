using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BIEM.OutputProcessing
{
    public class Variable
    {
        private string Name { get; set; }
        private double[] Data { get; set; }

        List<Variable> variables = new List<Variable>();
        public void Add(string name, double[] data)
        {

            variables.Add(new Variable { Name = name, Data = data });
        }
        public static void Read(Variable variable)
        {
            Console.WriteLine($"Variable {variable.Name}, ");
            foreach (var line in variable.Data)
            {
                Console.WriteLine($"{line}");
            }


        }
        public static Variable GetVariableData(int index)
        {
            Variable variable = new Variable();
            string csvFile = @"C:\Users\jairo\Documents\example112.csv";
            var lines = File.ReadAllLines(csvFile);
            int NumberofLines = lines.Length - 1;

            string[] HeadLine = lines[0].Split(',');
            int NumberofVariables = HeadLine.Length - 1;


            double[] data = new double[NumberofLines];
            for (int i = 1; i < NumberofLines; i++)
            {
                var line = lines[i].Split(',');
                data[0] = 0;
                data[i] = Convert.ToDouble(line[index]);

            }
            int format = HeadLine[index].IndexOf("[") - 1;
            var fixedVariable = HeadLine[index].Substring(0, format);
            fixedVariable = Regex.Replace(fixedVariable, @"\s", "");
            variable.Name = fixedVariable;
            variable.Data = data;
            variable.Add(fixedVariable, data);

            return variable;


        }
        public static string[] ReadVariables(string pathFile)
        {
            string csvFile = pathFile;
            var lines = File.ReadAllLines(csvFile);



            //-----
            string[] HeadLine = lines[0].Split(',');
            OutputProcessing op = new OutputProcessing();
            op.NumberofVariables = HeadLine.Length - 1;
            for (int i = 1; i < HeadLine.Length; i++)
            {

                string[] ObjectVariable = HeadLine[i].Split(':');
                for (int j = 0; j < ObjectVariable.Length; j = j + 2)
                {
                    for (int k = 1; k < ObjectVariable.Length + 1; k = k + 2)
                    {
                        int format = ObjectVariable[k].IndexOf("[") - 1;
                        var fixedVariable = ObjectVariable[k].Substring(0, format);
                        fixedVariable = Regex.Replace(fixedVariable, @"\s", "");
                        VariableNameType value = (VariableNameType)Enum.Parse(typeof(VariableNameType), fixedVariable);

                        op.Add(value, ObjectVariable[j]);




                    }
                }
            }
            return op.Read();

        }
        public static void PlotBars(Variable variable)
        {

        }

    }
}
