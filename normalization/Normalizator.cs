using System;

namespace ML.Lib
{
    class Normalizator
    {
        static void Main(string[] args)
        {
            double[] set = {1.0, 2.0, 3.0, 4.0, 5.0, 5.0, 4.0};
            double[] normalized = Normalize(set, 1.0, 0.0);
            for (int i = 0; i < normalized.Length; i++)
            {
                Console.Write(normalized[i] + " ");
            }
            
        }

        static double Max(double[] input)
        {
            double result = double.MinValue;
            for (int i = 0; i < input.Length; i++)
            {
                if(result < input[i])
                    result = input[i];
            }
            return result;
        }

        static double Min(double[] input)
        {
            double result = double.MaxValue;
            for (int i = 0; i < input.Length; i++)
            {
                if(result > input[i])
                    result = input[i];
            }
            return result;
        }

        static double[] Normalize(double[] input, double nmax, double nmin)
        {
            double[] result = new double[input.Length];
            double min = Min(input);
            double max = Max(input);

            for (int i = 0; i < input.Length; i++)
            {
                result[i] = ((input[i] - min) / (max - min)) * (nmax - nmin) + nmin;
            }
            return result;
        }



    }
}
