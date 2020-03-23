using System;
using System.Diagnostics;

namespace ML.Lib.Image
{
    public class GaussianFilter : Filter
    {

        private double _deviation;

        //Creates gaussian filter 
        public GaussianFilter(double deviation, int width, int height)
        {
            _deviation = deviation;
            filterData = new double[width][];
            for (int i = 0; i < width; i++)
            {
                filterData[i] = new double[height];
            }

            Point2D pivot = new Point2D(width / 2, height/2);

            for (int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    filterData[i][j] = GaussianFunction(pivot, new Point2D(i,j));
                }
            }

        }



        private double GaussianFunction(Point2D pivot, Point2D current)
        {   
            Debug.WriteLine(pivot - current);
            return GaussianFunction(pivot - current);
        }

        private double GaussianFunction(Point2D distance)
        {
            return Math.Exp( -((Math.Pow(distance.x, 2) + Math.Pow(distance.y, 2)) / (2 * Math.Pow(_deviation, 2)))) / (2 * Math.PI * _deviation * _deviation);
        }

        public override string ToString()
        {
            string result = "";
            foreach (var d in filterData)
            {
                foreach (var c in d)
                {
                    result += c.ToString() + " ";
                }
                result += "\n";
            }
            return result;
        }


    }




}