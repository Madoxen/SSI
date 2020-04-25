using System.Collections.Generic;
using System.Linq;
namespace ML.Lib.Neuron
{
    public class SimpleNetwork : INetwork
    {
        //Contains all hidden layers and an output layer
        public List<Layer> HiddenLayers { get; set; }
        private InputConnection[] inputs { get; set; }
        private OutputConnection[] outputs {get;set;}

        //initialize network sizes.Length layers
        //each layer has int size assigned
        public SimpleNetwork(int inputCount, IEnumerable<int> sizes, int outputCount)
        {
            HiddenLayers = new List<Layer>();
            inputs = new InputConnection[inputCount];
            outputs = new OutputConnection[outputCount];

            foreach (int size in sizes)
            {
                Layer l = new Layer();
                HiddenLayers.Add(l);
                //Create layer of "size" neurons
                for (int i = 0; i < size; i++)
                {
                    l.neurons.Add(new SimpleNeuron());
                }
            }
            ReconnectNetwork(inputCount, outputCount);
        }

        public SimpleNetwork() { }

        public void Train(double[] input, double[] expectedValues, int epochs)
        {
            return;
        }

        public double[] Calculate(double[] input)
        {
            return null;
        }


        //Connects each layer with next one
        private void ReconnectNetwork(int inputCount, int outputCount)
        {
            System.Random rnd = new System.Random();


            //Create inputCount inputs
            for (int i = 0; i < inputCount; i++)
            {
                InputConnection ic = new InputConnection();
                inputs[i] = ic;
            }

              //Create outputCount outputs
            for (int i = 0; i < outputCount; i++)
            {
                OutputConnection oc = new OutputConnection();
                outputs[i] = oc;
            }


            //Handle input layer
            //Set InputConnections for each neuron in first hidden layer
            foreach (INeuron n in HiddenLayers[0].neurons)
            {
                for (int i = 0; i < inputCount; i++)
                {
                    n.IncomingConnections.Add(inputs[i]);
                }
            }


            //Handle hidden layers
            for (int backLayer = 0; backLayer < HiddenLayers.Count - 1; backLayer++)
            {
                foreach (INeuron back in HiddenLayers[backLayer].neurons)
                {
                    foreach (INeuron front in HiddenLayers[backLayer + 1].neurons)
                    {
                        Connection c = new Connection(back, front, rnd.NextDouble());
                        back.OutcomingConnections.Add(c);
                        front.IncomingConnections.Add(c);
                    }
                }
            }


            //Handle output layer
            //Set OutputConnections for each neuron in last hidden layer
            foreach (INeuron n in HiddenLayers.Last().neurons)
            {
                for (int i = 0; i < outputCount; i++)
                {
                    n.OutcomingConnections.Add(outputs[i]);
                }
            }


        }
    }

}