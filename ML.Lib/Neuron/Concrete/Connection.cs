namespace ML.Lib.Neuron
{
    public class Connection : IConnection
    {
        private INeuron from;
        private INeuron to;

        public INeuron To { get { return to; } }
        public INeuron From { get { return from; } }
        public double Weight { get; set; }

        public double Output { get; set; }
        public double PreviousWeight { get; set; }

        public Connection() { }
        public Connection(INeuron from, INeuron to, double Weight)
        {
            this.from = from;
            this.to = to;
            this.Weight = Weight;
        }

        public void UpdateWeight(double newWeight)
        {
            PreviousWeight = Weight;
            Weight = newWeight;
        }
    }
}


