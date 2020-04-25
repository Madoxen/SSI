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
        }

        public double CalculateOutput()
        {
            return ActivationFunction.Perform(InputFunction.Perform(IncomingConnections));
        }
    }

}