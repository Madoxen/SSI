using System.Collections.Generic;
using System;
namespace ML.Lib.Fuzzy
{
    public class Sugeno
    {
        //Collection of user determined fuzzifiers used in fuzzifying process
        List<IFuzzifier> fuzzifiers = new List<IFuzzifier>();
        public List<IFuzzifier> Fuzzifiers
        {
            get { return fuzzifiers; }
        }

        //Collection of user determined truthValues together with their respective IDs
        Dictionary<string, double> truthValues = new Dictionary<string, double>();
        public Dictionary<string, double> TruthValues
        {
            get { return truthValues; }
        }

        

        public Sugeno()
        {
            
        }
        

        //Performs fuzzy-system operations
        public void Compile(IEnumerable<double> inputs)
        {
                        
        }
    }
}