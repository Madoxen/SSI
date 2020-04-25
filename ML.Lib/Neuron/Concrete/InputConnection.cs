namespace ML.Lib.Neuron
{

    //A special kind of connection
    //To this connection Network pushes it's input values
    public class InputConnection : IConnection
    {
        private INeuron to;

        public double Weight
        {
            get { return 1; }
        }

        public double GetOutput()
        {
            return 1;
        }
    }

}