using System;

namespace ML.Lib.Fuzzy
{
    public class TriangleFuzzifier : IFuzzifier
    {
        public string Label { get; set; }

        private double a, b, c;


        public TriangleFuzzifier(double a, double b, double c, string Label)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.Label = Label;
        }

        public double Fuzzify(double x)
        {
            if ((a < x) && (x <= b)) return (x - a) / (b - a);
            if ((b < x) && (x < c)) return (c - x) / (c - b);
            return 0;
        }
    }
}