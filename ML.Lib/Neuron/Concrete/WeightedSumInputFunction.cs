using System.Linq;
using System.Collections.Generic;

namespace ML.Lib.Neuron
{

    public class WeightedSumFunction : IInputFunction<List<IConnection>>
    {
        public double Perform(List<IConnection> input)
        {
            return input.Select(x => x.Weight * x.GetOutput()).Sum();
        }
    }

}
