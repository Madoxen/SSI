namespace ML.Lib.Fuzzy
{
    public interface IFuzzifier
    {
        string Label
        {
            get; set;
        }

        double Fuzzify(double x);
        
    }


}