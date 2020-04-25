namespace ML.Lib.Neuron
{

    //A special kind of connection
    //To this connection Network pushes it's input values
    public class InputConnection : IConnection
    {
        public double Weight
        {
            get { return 1; }
            set{}
        }

        public double PreviousWeight
        {
            get { return 1; }
        }

        public double Output { get; set; }



        public double GetOutput()
        {
            return Output;
        }

        public void UpdateWeight(double newWeight)
        {
            throw new System.NotImplementedException();
        }



        public INeuron To => throw new System.NotImplementedException();

        public INeuron From => throw new System.NotImplementedException();


    }

}