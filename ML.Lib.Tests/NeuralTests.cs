
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
            int outputCount = 4;
            SimpleNetwork net = new SimpleNetwork(inputCount, layerSizes, outputCount);


            //Check if input connections were connected correctly
            //And check if first layer has correct amount of Outcoming connections
            foreach (INeuron n in net.HiddenLayers[0].neurons)
            {
                Assert.AreEqual(inputCount, n.IncomingConnections.Count);
                Assert.AreEqual(layerSizes[1], n.OutcomingConnections.Count);
            }

            //Check amount of neurons in each hidden layer and in output layer
            Assert.AreEqual(layerSizes[0], net.HiddenLayers[0].neurons.Count);
            Assert.AreEqual(layerSizes[1], net.HiddenLayers[1].neurons.Count);
            Assert.AreEqual(layerSizes[2], net.HiddenLayers[2].neurons.Count);


            //Check amount of connections
            for (int i = 1; i < layerSizes.Length - 1; i++)
            {
                foreach (INeuron n in net.HiddenLayers[i].neurons)
                {
                    Assert.AreEqual(layerSizes[i - 1], n.IncomingConnections.Count);
                    Assert.AreEqual(layerSizes[i + 1], n.OutcomingConnections.Count);
                }

            }

            //Check connections to the last layer
            foreach(INeuron n in net.HiddenLayers.Last().neurons)
            {
                Assert.AreEqual(layerSizes[layerSizes.Length - 2], n.IncomingConnections.Count);
                Assert.AreEqual(outputCount, n.OutcomingConnections.Count);
            }
        }

    }

}