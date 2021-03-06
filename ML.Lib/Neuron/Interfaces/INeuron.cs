using System.Collections.Generic;

namespace ML.Lib.Neuron
{

    //Common interface for neurons in networks
    //Neuron is a smallest work unit in a network
    public interface INeuron
    {
        List<IConnection> IncomingConnections { get; set; }
        List<IConnection> OutcomingConnections { get; set; }

        IActivationFunction ActivationFunction {get;set;}


        double PreviousPartialDerivate { get; set; }

        double LastOutputValue {get;set;}
        double LastInputValue {get;set;}
        double CalculateOutput();
        void PushToOutput(double d);

    }

}