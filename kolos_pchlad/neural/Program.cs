using System;
using System.IO;
using NeuralNetwork;

namespace neural
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length > 0)
            {
                if (!File.Exists(args[0]))
                    TrainNew();
                Network net = new Network(4, new int[] { 4, 4, 4, 4 }, 2);
                net.LoadWeights(File.ReadAllLines(args[0]));


                double[][][] data = Loader.Load("data.csv");
                //Final test, take test set and test correctness
                HighestHitTest t = new HighestHitTest(net);
                t.Test(data[2], data[3]);
            }
            else
            {
                TrainNew();
            }


        }

        static void TrainNew()
        {
            //Net with 6 layers
            Network net = new Network(4, new int[] { 4, 4, 4, 4 }, 2);
            //    net.testStrategy = new HighestHitTest(net);

            double[][][] data = Loader.Load("data.csv");
            net.Train(data, 3000);

            //Final test, take test set and test correctness
            HighestHitTest t = new HighestHitTest(net);
            t.Test(data[2], data[3]);
        }
    }
}
