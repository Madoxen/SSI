using System.Collections.Generic;

namespace ML.Lib.Neuron
{
    public class SimpleNeuron : INeuron
    {
        public IActivationFunction ActivationFunction {get;set;}
        public IInputFunction<List<IConnection>> InputFunction {get;set;}
        public List<IConnection> IncomingConnections { get;set;}
        public List<IConnection> OutcomingConnections { get;set;}

        public SimpleNeuron()
        {
            ActivationFunction = new SigmoidActivationFunction();
            InputFunction = new WeightedSumInputFunction();
            IncomingConnections = new List<IConnection>();
            OutcomingConnections = new List<IConnection>();
        }

        //Calculates output of this neuron 
        public double CalculateOutput()
        {
            return ActivationFunction.Perform(InputFunction.Perform(IncomingConnections));
        }


        //Pushes value d to Output property of Outcoming Connections
        public void PushToOutput(double d)
        {
            foreach(IConnection output in OutcomingConnections)
            {
                output.Output = d;
            }
        }



    }

}