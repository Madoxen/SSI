using System;

namespace ML.Lib.Fuzzy
{
    //Example Fuzzifier that uses gaussian function
    public class GaussianFuzzifier : IFuzzifier
    {
        public string Label { get; set; }


        private double m, sigma;


        public GaussianFuzzifier(double m, double sigma, string Label)
        {
            this.m = m;
            this.sigma = sigma;
            this.Label = Label;

        }

        public double Fuzzify(double x)
        {
            return Math.Exp(-((x - m) * (x - m)) / (2.0 * sigma * sigma));
        }
    }
}