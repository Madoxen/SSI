using System.IO;
using System.Collections.Generic;
using System;
using ML.Lib;

namespace task1
{
    public class Data
    {
        double[][] rawData;


        public void Load(string path, string delim)
        {
            string[] lines = File.ReadAllLines(path);
            rawData = new double[lines.Length][];
            for(int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split(delim);
                double[] buff = new double[tokens.Length];
                for(int j = 0; j < tokens.Length; j++)
                {
                    buff[j] = Convert.ToDouble(tokens[j]);
                }
                rawData[i] = buff;
            }
        }

        public void Save(string path)
        {
            using(StreamWriter writer = new StreamWriter(File.OpenWrite(path)))
            {
                foreach(double[] arr in rawData)
                {
                    string currentLine = "";
                    foreach(double d in arr)
                    {
                        currentLine += d + "|";
                    }
                    writer.WriteLine(currentLine);
                }
            }
        }

        public void Shuffle()
        {
            Shuffler.Shuffle(rawData);
        }

        //Normalizes whole data array column wise
        public void Normalize()
        {
            for(int i = 0; i < rawData[0].Length; i++)
            {
                rawData.SetColumn(Normalizator.Normalize(rawData.GetColumn(i),0.0,1.0), i);
            }
        }
    }
}
