using ML.Lib.Classification;
using ML.Lib;
using System.Collections.Generic;
using System;
using System.Linq;

namespace KNN
{
    public class Classifier
    {
        //Original data
        double[][] dataArr;
        public Func<double[], double[], double> DistanceMethod;

        public Classifier()
        {
            DistanceMethod = Distance;
        }

        //data object input to be evaluated
        //k - how many closest neighbours will be taken into consideration
        public void Classify(ClassifiedObject<double> input, List<ClassifiedObject<double>> data, int k)
        {
            //Get amount of minimum dimensions
            int dim = data.Max(x => x.Properties.Count);
            if (dim < input.Properties.Count) dim = input.Properties.Count;

            //create data array
            double[][] dataArr = new double[data.Count + 1][];
            for (int i = 0; i < dataArr.Length; i++)
            {
                dataArr[i] = new double[dim];
            }

            //fill data array
            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Properties.Count; j++)
                {
                    dataArr[i][j] = data[i].Properties[j];
                }
            }

            //put last element as an input
            for (int i = 0; i < input.Properties.Count; i++)
            {
                dataArr.Last()[i] = input.Properties[i];
            }

            //Normalize data to 0-1 range
            for (int i = 0; i < dataArr[0].Length; i++)
            {
                double[] col = dataArr.GetColumn(i);
                Normalizator.Normalize(col, 0.0, 1.0);
                dataArr.SetColumn(col, i);
            }

            double[] distances = new double[data.Count];

            //Calculate distances
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = DistanceMethod(dataArr.Last(), dataArr[i]);
            }

            int[] minimalIndexes = FindKMin(distances, k);
            Dictionary<string, int> classesHistogram = new Dictionary<string, int>();  //classification histogram
            //Gather classes
            foreach (int min in minimalIndexes)
            {
                if (!classesHistogram.ContainsKey(data[min].Classification))
                {
                    classesHistogram.Add(data[min].Classification, 1);
                }
                else
                {
                    classesHistogram[data[min].Classification]++;
                }
            }
            //Find maximum value and get key
            input.Classification = classesHistogram.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }

        //returns indexes of k closest neighbours
        int[] FindKMin(double[] distances, int k)
        {
            double[] min = new double[k];
            int[] minIndex = new int[k];
            for (int i = 0; i < k; i++)
            {
                minIndex[i] =  0;
                min[i] = double.PositiveInfinity;
            }

            for (int i = 0; i < distances.Length; i++)
            {
                for (int j = 0; j < k; j++)
                {
                    if (min[j] > distances[i])
                    {
                        min[j] = distances[i];
                        minIndex[j] = i;
                        break;
                    }
                }
            }
            return minIndex;
        }


        double Distance(double[] a, double[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Arrays are not of the same length");

            double distance = 0.0;
            for (int dim = 0; dim < a.Length; dim++)
            {
                distance += Math.Pow(a[dim] - b[dim], 2);
            }
            distance = Math.Sqrt(distance);
            return distance;
        }

    }
}