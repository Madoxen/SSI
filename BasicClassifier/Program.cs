using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace BasicClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
         
            List<IdentifiedObject> objects = new List<IdentifiedObject>();
            string[] data = File.ReadAllLines(args[0]);
            List<string> properties = data[0].Split(',').ToList();
            for(int i = 1; i < data.Length; i++)
            {
                string[] tokens = data[i].Split(" ");
                IdentifiedObject obj = new IdentifiedObject();
                objects.Add(obj);
                obj.id = tokens[0];
                for(int j = 1; j < tokens.Length; j++)
                {
                    if(tokens[j] == "1")
                    {
                        obj.properties.Add(properties[j-1]);
                    }
                }

               /* Console.Write("Loaded: " + obj.id + " that is: ");
                foreach(string p in obj.properties)
                {
                    Console.Write(p + " ");
                }
                Console.WriteLine();*/
            }

            
            List<ClassifierInfo> parameters = new List<ClassifierInfo>();
            data = File.ReadAllLines(args[1]);
            foreach(string d in data)
            {
                string[] tokens = d.Split(',');
                ClassifierInfo i = new ClassifierInfo(tokens[0], Convert.ToDouble(tokens[1]));
                parameters.Add(i);
            }

            List<ClassifierInfo> results = Classifier.Classify(objects, parameters);
            results.ForEach(x=> Console.WriteLine(x.label + " w:" + x.match));
            Console.WriteLine("Best match: " + results.OrderByDescending(x=>x.match).ToList()[0].label);

        }
    }
}
