namespace ML.Lib.Neuron
{

    //A special kind of connection
    //To this connection Network pushes it's input values
    public class OutputConnection : IConnection
    {
        public double Weight
        {
            get { return 1; }
        }

        public double Output {get;set;}

        public double GetOutput()
        {
            return Output;
        }
    }

}