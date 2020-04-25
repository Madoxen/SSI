using System;
using System.Collections.Generic;
using System.Linq;
namespace ML.Lib.Neuron
{
    public class SimpleNetwork : INetwork
    {
        //Contains all hidden layers and an output layer
        public List<Layer> HiddenLayers { get; set; }
        private InputConnection[] inputs { get; set; }

        //initialize network sizes.Length layers
        //each layer has int size assigned
        public SimpleNetwork(int inputCount, IEnumerable<int> sizes)
        {
            HiddenLayers = new List<Layer>();
            inputs = new InputConnection[inputCount];

            foreach (int size in sizes)
            {
                Layer l = new Layer();
                HiddenLayers.Add(l);
                //Create layer of "size" neurons
                for (int i = 0; i < size; i++)
                {
                    l.Neurons.Add(new SimpleNeuron());
                }
            }
            ReconnectNetwork(inputCount);
        }

        public SimpleNetwork() { }

        public void Train(double[] input, double[] expectedValues, int epochs)
        {
            return;
        }

        public double[] Calculate(double[] input)
        {
            if (input.Length != inputs.Length)
                throw new ArgumentException("Expected input of length: " + inputs.Length + "| got: " + input.Length);

            //push values onto input connections
            for (int i = 0; i < input.Length; i++)
            {
                inputs[i].Output = input[i];
            }

            //propagate values forward
            foreach (Layer l in HiddenLayers)
            {
                foreach (INeuron n in l.Neurons)
                {
                    //Calculate output of given neuron...
                    double currVal = n.CalculateOutput();
                    //... and then push that output value to all of Outcoming connection of this neuron
                    n.PushToOutput(currVal);
                }
            }

            double[] result = new double[HiddenLayers.Last().Neurons.Count];
            //pack values from last layer 
            for (int i = 0; i < HiddenLayers.Last().Neurons.Count; i++)
            {
                result[i] = HiddenLayers.Last().Neurons[i].CalculateOutput();
            }
            return result;
        }


        //Connects each layer with next one
        private void ReconnectNetwork(int inputCount)
        {
            System.Random rnd = new System.Random();


            //Create inputCount inputs
            for (int i = 0; i < inputCount; i++)
            {
                InputConnection ic = new InputConnection();
                inputs[i] = ic;
            }



            //Handle input layer
            //Set InputConnections for each neuron in first hidden layer
            foreach (INeuron n in HiddenLayers[0].Neurons)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    n.IncomingConnections.Add(inputs[i]);
                }
            }


            //Handle hidden layers
            for (int backLayer = 0; backLayer < HiddenLayers.Count - 1; backLayer++)
            {
                foreach (INeuron back in HiddenLayers[backLayer].Neurons)
                {
                    foreach (INeuron front in HiddenLayers[backLayer + 1].Neurons)
                    {
                        Connection c = new Connection(back, front, rnd.NextDouble());
                        back.OutcomingConnections.Add(c);
                        front.IncomingConnections.Add(c);
                    }
                }
            }
        }
    }

}