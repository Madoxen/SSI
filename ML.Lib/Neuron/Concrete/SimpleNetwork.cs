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

        public double LearningRate { get; set; }

        //initialize network sizes.Length layers
        //each layer has int size assigned
        public SimpleNetwork(int inputCount, IEnumerable<int> sizes)
        {
            HiddenLayers = new List<Layer>();
            inputs = new InputConnection[inputCount];
            LearningRate = 0.05;

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




        public void Train(double[][] input, double[][] expectedValues, int epochs)
        {

            for (int i = 0; i < epochs; i++)
            {
                //For each input set
                for (int j = 0; j < input.Length; j++)
                {
                    //Calculate current net iteration
                    double[] currentOutputs = Calculate(input[j]);
                    double globalCurrentCost = CalculateError(expectedValues[j], currentOutputs);
                    HandleOutputLayer(expectedValues[j], currentOutputs);
                    HandleMiddleLayers(expectedValues[j], currentOutputs);
                }
            }


        }

        public double CalculateError(double[] expectedValues, double[] actualValues)
        {
            if (expectedValues.Length != actualValues.Length)
                throw new ArgumentException("expectedValues length: " + expectedValues.Length + "| actualValues: " + actualValues.Length);


            double currentCost = 0;
            for (int i = 0; i < expectedValues.Length; i++)
            {
                currentCost += Math.Pow(expectedValues[i] - actualValues[i], 2);
            }
            currentCost /= 2 * expectedValues.Length;
            return currentCost;
        }



        //Updates weights coming to output layer (last layer)
        private void HandleOutputLayer(double[] expectedValues, double[] currentOutputs)
        {
            //Alter each weight of each neuron in output layer
            for (int i = 0; i < HiddenLayers.Last().Neurons.Count; i++)
            {
                INeuron currNeuron = HiddenLayers.Last().Neurons[i];
                for (int j = 0; j < currNeuron.IncomingConnections.Count; j++)
                {
                    //calc nabla cost / nabla curr weight
                    double incomingNeuronValue = HiddenLayers[HiddenLayers.Count - 2].Neurons.First(x => x.OutcomingConnections.Any(x => x.To == currNeuron)).LastOutputValue;
                    double CostDerivative = (expectedValues[i] - currentOutputs[i]) * currentOutputs[i] * (1 - currentOutputs[i]) * incomingNeuronValue;
                    currNeuron.PreviousPartialDerivate = (expectedValues[i] - currentOutputs[i]) * currentOutputs[i] * (1 - currentOutputs[i]);
                    double newWeight = currNeuron.IncomingConnections[j].Weight - (LearningRate * CostDerivative);
                    currNeuron.IncomingConnections[j].UpdateWeight(newWeight);
                }

            }
        }


        public void HandleMiddleLayers(double[] expectedValues, double[] currentOutputs)
        {
            //For each hidden layer
            for (int i = HiddenLayers.Count - 2; i > 0; i--)
            {
                //For each neuron in ith layer
                for (int j = 0; j < HiddenLayers[i].Neurons.Count; j++)
                {
                    INeuron currentNeuron = HiddenLayers[i].Neurons[j];
                    //for each incoming connection
                    for (int k = 0; k < currentNeuron.IncomingConnections.Count; k++)
                    {
                        IConnection conn = currentNeuron.IncomingConnections[k];
                        //update each incoming connection
                        //in relation to previous connections
                        double sum = 0;
                        //Go to previous(next) layer
                        
                        


                    }
                }
            }
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