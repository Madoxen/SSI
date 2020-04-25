namespace ML.Lib.Neuron
{
    public interface IConnection
    {

        INeuron To {get;}
        INeuron From {get;}

        double Weight {get;set;}
        double PreviousWeight{get;}
        double Output {get;set;}

        void UpdateWeight(double newWeight);
    }

}