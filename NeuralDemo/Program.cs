using System;
using System.IO;
using ML.Lib.Neuron;


namespace NeuralDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] layerSizes = new int[] { 4, 10, 3 };
            SimpleNetwork net = new SimpleNetwork(layerSizes);
            string[] lines = File.ReadAllLines("Resources/irisDataset.csv");
            double[][] results = new double[lines.Length - 1][];
            double[][] expectedValues = new double[lines.Length - 1][];
            
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split("|");
                results[i - 1] = new double[4];
                expectedValues[i - 1] = new double[3];
                results[i - 1][0] = Convert.ToDouble(tokens[0]);
                results[i - 1][1] = Convert.ToDouble(tokens[1]);
                results[i - 1][2] = Convert.ToDouble(tokens[2]);
                results[i - 1][3] = Convert.ToDouble(tokens[3]);

                expectedValues[i - 1][0] = Convert.ToDouble(tokens[4]);
                expectedValues[i - 1][1] = Convert.ToDouble(tokens[5]);
                expectedValues[i - 1][2] = Convert.ToDouble(tokens[6]);
            }


            net.Train(results, expectedValues, 1000);

            int s = 0;
            foreach (double[] res in results)
            {

                double[] output = net.Calculate(res);

                Console.WriteLine("Sample #" + s + "|setosa: " + output[0] + "|versicolor: " + output[1] + "|virginica:" + output[2]);
                s++;
            }
            Console.WriteLine(net.Dump());
        }
    }
}
