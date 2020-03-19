using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace BayesClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            //load data file
            List<ClassifiedObject> objects = new List<ClassifiedObject>();
            string[] lines = File.ReadAllLines(args[0]);
            foreach(string l in lines)
            {
                string[] tokens = l.Split(" ");
                ClassifiedObject o = new ClassifiedObject();
                o.Label = tokens[0];
                o.Classification = tokens[1];
                for(int i = 2; i < tokens.Length; i++)
                {
                    o.Properties.Add(tokens[i]);
                }
                objects.Add(o);
            }

            ClassifiedObject objectToClassify = new ClassifiedObject() { Label="XD", Properties={"DESZCZOWO", "GORĄCO", "SŁABY"}, Classification=null};            
            

            Classifier c = new Classifier();
            c.LoadDataset(objects, 3);
            c.Classify(objectToClassify,1.0);
            Console.WriteLine( "Object: " + objectToClassify.Label + " has been classified as: " + objectToClassify.Classification);


        }
    }
}
