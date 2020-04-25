namespace ML.Lib.Neuron
{
    //Common interface for all neural networks
    public interface INetwork
    {
        void Train<T>(T input, int epochs); //Train network according to given inputs
        O Calculate<T, O>(T input); // Calculate output of network given the input

    }
}