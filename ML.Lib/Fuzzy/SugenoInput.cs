using System.Collections.Generic;

namespace ML.Lib.Fuzzy
{
    public class SugenoInput
    {
        public string Label;
        public Dictionary<string, double> values;


        public SugenoInput(string label)
        {
            Label = label;
            values = new Dictionary<string, double>();
        }



        public SugenoInput(string label, Dictionary<string, double> values)
        {
            Label = label;
            this.values = new Dictionary<string, double>(values);
        }


    }

}