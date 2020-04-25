using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ML.Lib.Neuron
{


    [TestClass]
    public class NeuralTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            int[] layerSizes = new int[] { 4, 4, 2 };
            int inputCount = 10;
            SimpleNetwork net = new SimpleNetwork(inputCount, layerSizes);


            //Check if input connections were connected correctly
            //And check if first layer has correct amount of Outcoming connections
            foreach (INeuron n in net.HiddenLayers[0].Neurons)
            {
                Assert.AreEqual(inputCount, n.IncomingConnections.Count);
                Assert.AreEqual(layerSizes[1], n.OutcomingConnections.Count);
            }

            //Check amount of neurons in each hidden layer and in output layer
            Assert.AreEqual(layerSizes[0], net.HiddenLayers[0].Neurons.Count);
            Assert.AreEqual(layerSizes[1], net.HiddenLayers[1].Neurons.Count);
            Assert.AreEqual(layerSizes[2], net.HiddenLayers[2].Neurons.Count);


            //Check amount of connections
            for (int i = 1; i < layerSizes.Length - 1; i++)
            {
                foreach (INeuron n in net.HiddenLayers[i].Neurons)
                {
                    Assert.AreEqual(layerSizes[i - 1], n.IncomingConnections.Count);
                    Assert.AreEqual(layerSizes[i + 1], n.OutcomingConnections.Count);
                }

            }

            //Check connections to the last layer
            foreach (INeuron n in net.HiddenLayers.Last().Neurons)
            {
                Assert.AreEqual(layerSizes[layerSizes.Length - 2], n.IncomingConnections.Count);
            }
        }

        [TestMethod]
        public void TestCalculate()
        {
            int[] layerSizes = new int[] { 4, 2 };
            int inputCount = 2;
            SimpleNetwork net = new SimpleNetwork(inputCount, layerSizes);

            double[] output = net.Calculate(new double[] { 1.0, 2.0 });
        }


        [TestMethod]
        public void TestMassCalculate()
        {
            int[] layerSizes = new int[] { 4, 4, 3 };
            int inputCount = 4;
            SimpleNetwork net = new SimpleNetwork(inputCount, layerSizes);
            string[] lines = File.ReadAllLines("Resources/irisDataset.csv");
            double[][] results = new double[lines.Length - 1][];
            for (int i = 1; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split("|");
                results[i - 1] = new double[4];
                results[i - 1][0] = Convert.ToDouble(tokens[0]);
                results[i - 1][1] = Convert.ToDouble(tokens[1]);
                results[i - 1][2] = Convert.ToDouble(tokens[2]);
            }

            foreach (double[] res in results)
            {
                double[] output = net.Calculate(res);
            }
        }


        [TestMethod]
        public void TestTrain()
        {
            int[] layerSizes = new int[] { 4, 4, 3 };
            int inputCount = 4;
            SimpleNetwork net = new SimpleNetwork(inputCount, layerSizes);
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

                expectedValues[i-1][0] = Convert.ToDouble(tokens[4]);
                expectedValues[i-1][1] = Convert.ToDouble(tokens[5]);
                expectedValues[i-1][2] = Convert.ToDouble(tokens[6]);
            }

            net.Train(results, expectedValues, 1000);

            int s = 0;
            foreach (double[] res in results)
            {
                
                double[] output = net.Calculate(res);

                Console.WriteLine("Sample #"+s + "|setosa prob.: " +output[0] + "|versicolor prob.: " + output[1] + "|virginica prob.:" + output[2]);
                s++;
            }
        }
    }

}