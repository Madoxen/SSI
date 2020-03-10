using System;
using System.Collections.Generic;
using System.IO;
using ML.Lib;


namespace iris_classifier
{
    public class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(args[0]);
            double[][] data = new double[lines.Length][];
            List<string> names = new List<string>(); //species names


            //Data loading
            for (int i = 0; i < lines.Length; i++)
            {
                string l = lines[i]; //current line
                string[] tokens = l.Split(",");
                int dataRowLength = tokens.Length + 2;
                data[i] = new double[dataRowLength]; //we need to store 2 additional doubles signifying species

                if (!names.Contains(tokens[tokens.Length - 1]))
                    names.Add(tokens[tokens.Length - 1]);


                for (int j = 0; j < dataRowLength - 3; j++)
                {
                    data[i][j] = Convert.ToDouble(tokens[j]);
                }

                data[i][tokens.Length - 1 + names.IndexOf(tokens[tokens.Length - 1])] = 1.0;

            }


            //Data normalization
            for (int i = 0; i < data[0].Length - 2; i++)
            {
                data.SetColumn(Normalizator.Normalize(data.GetColumn(i), 0.0, 1.0), i);
            }
            Shuffler.Shuffle(data);

            for (int i = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    Console.Write("|" + data[i][j] + "|---");
                }
                Console.WriteLine();
            }
        }



    }
}
