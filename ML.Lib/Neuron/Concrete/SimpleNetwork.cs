using System;
using System.Collections.Generic;
using System.Linq;
namespace ML.Lib.Neuron
{
    public class SimpleNetwork : INetwork
    {
        //Contains input layer all hidden layers and an output layer
        public List<Layer> Layers { get; set; }
        public double LearningRate { get; set; }


        public static System.Random rnd = new System.Random();
        //initialize network sizes.Length layers
        //each layer has int size assigned
        public SimpleNetwork(IEnumerable<int> sizes)
        {
            Layers = new List<Layer>();
            LearningRate = 0.05;

            //Create input layer
            Layer inputLayer = new Layer();
            Layers.Add(inputLayer);
            for (int i = 0; i < sizes.ToList()[0]; i++)
            {
                inputLayer.Neurons.Add(new InputNeuron());
            }



            foreach (int size in sizes.Skip(1))
            {
                Layer l = new Layer();
                Layers.Add(l);
                //Create layer of "size" neurons
                for (int i = 0; i < size; i++)
                {
                    l.Neurons.Add(new SimpleNeuron());
                }
            }
            ReconnectNetwork();
        }

        public SimpleNetwork() { }




        public void Train(double[][] input, double[][] expectedValues, int epochs)
        {
            //Cost function: 1/2n SUM_i (expected_i - output_i)^2
            //Cost for one training example C = 1/n SUM_i C_i
            //C_i = 1/2 ||(expected - output)^2|| (|| <- norm operation sum all in vector)
            //=============================================================================
            //Error in output layer Er^L_j = nabla C / nabla A^L_j * activationFuncDerivative(z^L_J)
            //Where z -> weighted input to the given layer's Lth jth Neuron
            //a -> Activation (output) of Layer's Lth jth Neuron
            //=================================================================
            //Error in the next layers Er^L = weight_prev_layer * error_prev_layer * activationFuncDerivative(z^L)




            //================================
            //The Backpropagation Algorithm
            //================================
            /*
            For each epoch
                For each training example:
                    1. Feedforward for given input from training example
                    Save information about each weighted input to each neuron
                    and about each activation for every neuron
                    2. Calculate Output Layer Error 
                    3. Backpropagate error,
                    For each Layer from preLastOne to second one:
                    Compute Error for every weight using error from previous layer
                    4. Gradient(direction) is given by multiplying error of neuron and activation 
                    from next layer that it's connected to 
                    5. Gradient Descent using Gradients move weights in the right direction.
            */



            double[][] errors = new double[Layers.Count][];
            for (int i = 0; i < Layers.Count; i++)
            {
                errors[i] = new double[Layers[i].Neurons.Count];
            }



            for (int i = 0; i < epochs; i++)
            {
                //For each input set
                for (int j = 0; j < input.Length; j++)
                {
                    //Calculate current net iteration
                    double[] currentOutputs = Calculate(input[j]);


                    //Calculate errors for output layer
                    for (int k = 0; k < Layers.Last().Neurons.Count; k++)
                    {
                        INeuron currentNeuron = Layers.Last().Neurons[k];
                        double currentError = (currentOutputs[k] - expectedValues[j][k]) * currentNeuron.ActivationFunction.PerformDerivative(currentNeuron.LastInputValue);
                        errors[Layers.Count - 1][k] = currentError;
                    }

                    //Once we have output layer errors calculated, we can use them to calculate next layers errors
                    for (int k = Layers.Count - 2; k > 0; k--)
                    {
                        for (int l = 0; l < Layers[k].Neurons.Count; l++)
                        {
                            errors[k][l] = 0;
                            for (int m = 0; m < Layers[k + 1].Neurons.Count; m++)
                            {
                                errors[k][l] += errors[k + 1][m] * Layers[k + 1].Neurons[m].IncomingConnections[l].Weight;
                            }
                            errors[k][l] *= Layers[k].Neurons[l].ActivationFunction.PerformDerivative(Layers[k].Neurons[l].LastInputValue);
                        }
                    }

                    //Now, when we have calculated all of errors for each neuron in our net
                    //We can perform weight update, the order does not matter as we already calculated everything needed
                    for (int k = Layers.Count - 1; k > 0; k--)
                    {
                        for (int l = 0; l < Layers[k].Neurons.Count; l++)
                        {
                            for (int m = 0; m < Layers[k - 1].Neurons.Count; m++)
                            {
                                double grad = errors[k][l] * Layers[k-1].Neurons[m].LastOutputValue * 2 * LearningRate;
                                //Update weight
                                Layers[k].Neurons[l].IncomingConnections[m].UpdateWeight(-grad);
                            }
                        }
                    }

   
                }
            }
        }




        public double[] Calculate(double[] input)
        {
            if (input.Length != Layers[0].Neurons.Count)
                throw new ArgumentException("Expected input of length: " + Layers[0].Neurons.Count + "| got: " + input.Length);

            //push values onto input connections
            for (int i = 0; i < input.Length; i++)
            {
                if (!(Layers[0].Neurons[i] is InputNeuron inputNeuron))
                    throw new Exception("Other neurons than InputNeurons were found in input layer");

                inputNeuron.OutputtingValue = input[i];
            }

            //propagate values forward
            foreach (Layer l in Layers)
            {
                foreach (INeuron n in l.Neurons)
                {
                    //Calculate output of given neuron...
                    double currVal = n.CalculateOutput();
                    //... and then push that output value to all of Outcoming connection of this neuron
                    n.PushToOutput(currVal);
                }
            }

            double[] result = new double[Layers.Last().Neurons.Count];
            //pack values from last layer 
            for (int i = 0; i < Layers.Last().Neurons.Count; i++)
            {
                result[i] = Layers.Last().Neurons[i].CalculateOutput();
            }
            return result;
        }


        //Connects each layer with next one
        private void ReconnectNetwork()
        {
            for (int backLayer = 0; backLayer < Layers.Count - 1; backLayer++)
            {
                foreach (INeuron back in Layers[backLayer].Neurons)
                {
                    foreach (INeuron front in Layers[backLayer + 1].Neurons)
                    {
                        Connection c = new Connection(back, front, rnd.NextDouble() * (0.5 + 0.5) - 0.5);
                        back.OutcomingConnections.Add(c);
                        front.IncomingConnections.Add(c);
                    }
                }
            }
        }



        public string Dump()
        {
            string result = "Weights:";
            for (int i = 0; i < Layers.Count; i++)
            {
                result += "Layer#" + i + "\n";
                for (int j = 0; j < Layers[i].Neurons.Count; j++)
                {
                    result += "Neuron#" + j;
                    for (int k = 0; k < Layers[i].Neurons[j].OutcomingConnections.Count; k++)
                    {
                        result += "|" + Layers[i].Neurons[j].OutcomingConnections[k].Weight + "|";
                    }
                    result += "\n";
                }

            }

            return result;
        }
    }

}