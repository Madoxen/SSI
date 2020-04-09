using System;

namespace ML.Lib.Fuzzy
{
    public class TrapezoidFuzzifier : IFuzzifier
    {

        public string Label { get; set; }


        private double a, b, c, d;


        public TrapezoidFuzzifier(double a, double b, double c, double d, string Label)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.Label = Label;
        }

        public double Fuzzify(double x)
        {
            if (x <= a) return 0;
            if (x > a && x <= b) return (x - a) / (b - a);
            if (x > b && x < c) return 1;
            if (x >= c && x <= d) return (d - x) / (d - c);
            return 0;
        }
    }
}