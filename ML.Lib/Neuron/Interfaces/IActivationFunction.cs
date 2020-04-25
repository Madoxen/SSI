namespace ML.Lib.Neuron
{   
    public interface IActivationFunction
    {
        //The "Output" function of a neuron
        //In normal-case scenario should receive input from IInputFunction
        double Perform(double input);

        //Performs first derivative
        //used in optimalization of a network 
        double PerformDerivative(double input);
    }
}