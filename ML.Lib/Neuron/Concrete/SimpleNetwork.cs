using System.Collections.Generic;

namespace ML.Lib.Neuron
{
    public class SimpleNetwork : INetwork
    {
        public List<Layer> Layer {get;set;}


        //initialize network sizes.Length layers
        //each layer has int size assigned
        public SimpleNetwork(IEnumerable<int> sizes)
        {
            
            foreach(int size in sizes)
            {
                Layer l = new Layer();
                Layer.Add(l);
                //Create layer of "size" neurons
                for(int i = 0; i < size; i++)
                {
                    l.neurons.Add(new SimpleNeuron());
                }
            }
        }

        public SimpleNetwork(){}

        public void Train<T>(T input, int epochs)
        {
            
        }

        public O Calculate<T, O>(T input)
        {
            return default(O);
        }
    }

}