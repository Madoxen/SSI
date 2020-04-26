using System.Collections.Generic;

namespace ML.Lib.Neuron
{
    public class InputNeuron : INeuron
    {

        public List<IConnection> OutcomingConnections { get; set; }


        //Unused values
        public List<IConnection> IncomingConnections { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double PreviousPartialDerivate { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public double LastOutputValue { get {return OutputtingValue;} set {throw new System.Exception();}}
        public double LastInputValue { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public IActivationFunction ActivationFunction { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }



        //Set this value to input values
        public double OutputtingValue { get; set; }
       

        public InputNeuron()
        {
            OutcomingConnections = new List<IConnection>();
        }

        public double CalculateOutput()
        {
            return OutputtingValue;
        }

        public void PushToOutput(double d)
        {
            foreach (IConnection output in OutcomingConnections)
            {
                output.Output = d;
            }
        }
    }



}