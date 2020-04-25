using System;
using System.Collections.Generic;

namespace ML.Lib.Neuron
{
    //Holds list of neurons
    public class Layer
    {
        public List<INeuron> Neurons{get;set;}

        public Layer()
        {
            Neurons = new List<INeuron>();
        }
            


    }

}