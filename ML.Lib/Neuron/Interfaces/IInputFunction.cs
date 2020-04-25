namespace ML.Lib.Neuron
{

    //Function that changes incoming input
    public interface IInputFunction<T>
    {
        double Perform(T input);
    }

}