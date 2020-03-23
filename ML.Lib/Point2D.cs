using System;

namespace ML.Lib
{
    public struct Point2D
    {
        public double x;
        public double y;

        public Point2D(double nx, double ny)
        {
            x = nx;
            y = ny;
        }


        //Calculates cartesian distance
        public static double Distance(Point2D a, Point2D b)
        {
            return Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2));
        }

        public static Point2D operator +(Point2D lh)
        {
            return lh;
        }

        public static Point2D operator -(Point2D lh)
        {
            return new Point2D(-lh.x, -lh.y);
        }

        public static Point2D operator +(Point2D lh, Point2D rh)
        {
            return new Point2D(lh.x + rh.x, lh.y + rh.y);
        }

        public static Point2D operator -(Point2D lh, Point2D rh)
        {
            return lh + (-rh);
        }

        public override string ToString()
        {
            return x.ToString() + " " + y.ToString();
        }

    }


}