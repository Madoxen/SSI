namespace ML.Lib.Neuron
{
    public interface IConnection
    {
        double Weight {get;}
        double GetOutput();
    }

}