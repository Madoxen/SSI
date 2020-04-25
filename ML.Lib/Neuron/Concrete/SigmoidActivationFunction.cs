using System;

namespace ML.Lib.Neuron
{
    public class SigmoidActivationFunction : IActivationFunction
    {
        public double Perform(double input)
        {
            return 1 / (1 + Math.Exp(-input));
        }

        public double PerformDerivative(double input)
        {
            return Perform(input) * (1 -  Perform(input));
        }
    }

}