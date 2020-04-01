using System;
using ML.Lib.Classification;
using System.IO;
using System.Collections.Generic;

namespace KNN
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ClassifiedObject<double>> objects = new List<ClassifiedObject<double>>();
            string[] lines = File.ReadAllLines("data1.txt");
            foreach(string s in lines)
            {
                string[] tokens =  s.Split(" ");
                ClassifiedObject<double> o = new ClassifiedObject<double>();
                o.Classification = tokens[0];
                for(int i = 1; i < tokens.Length; i++)
                {
                    o.Properties.Add(Convert.ToDouble(tokens[i]));
                }
                objects.Add(o);
            }

            ClassifiedObject<double> inputObj = new ClassifiedObject<double>();
            inputObj.Classification = "None";
            inputObj.Properties.Add(4.5);
            inputObj.Properties.Add(4.5);

            Classifier c = new Classifier();
            c.Classify(inputObj, objects, 4);
            Console.WriteLine(inputObj.Classification);

        }
    }
}
