namespace ML.Lib.Neuron
{
    public class Connection : IConnection
    {
        private INeuron from;
        private INeuron to;
        public double Weight { get; set; }

        public double Output {get;set;}

        public Connection() { }
        public Connection(INeuron from, INeuron to, double Weight)
        {
            this.from = from;
            this.to = to;
            this.Weight = Weight;
        }
    }
}


