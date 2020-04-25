namespace ML.Lib.Neuron
{
    //Common interface for all neural networks
    public interface INetwork
    {
        void Train(double[][] input, double[][] expectedValues, int epochs); //Train network according to given inputs and expected values
        double[] Calculate(double[] input); // Calculate output of network given the input

    }
}