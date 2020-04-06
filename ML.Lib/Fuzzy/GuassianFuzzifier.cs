using System;

namespace ML.Lib.Fuzzy
{
    //Example Fuzzifier that uses gaussian function
    public class GaussianFuzzifier : IFuzzifier
    {
        public string Label { get; set; }

        public int InputIndex { get; set; }

        private double m, sigma;


        public GaussianFuzzifier(double m, double sigma, string Label, int InputIndex)
        {
            this.m = m;
            this.sigma = sigma;
            this.Label = Label;
            this.InputIndex = InputIndex;
        }

        public double Fuzzify(double x)
        {
            return Math.Exp(-((x - m) * (x - m)) / (2.0 * sigma * sigma));
        }
    }
}