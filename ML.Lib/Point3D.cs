using System;

namespace ML.Lib
{
    public struct Point3D
    {
        public double x;
        public double y;
        public double z;

        public Point3D(double nx, double ny, double nz)
        {
            x = nx;
            y = ny;
            z = nz;
        }


        //Calculates cartesian distance
        public static double Distance(Point3D a, Point3D b)
        {
            return Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2) + Math.Pow(b.z - a.z, 2));
        }

        public static Point3D operator +(Point3D lh)
        {
            return lh;
        }

        public static Point3D operator -(Point3D lh)
        {
            return new Point3D(-lh.x, -lh.y, -lh.z);
        }

        public static Point3D operator +(Point3D lh, Point3D rh)
        {
            return new Point3D(lh.x + rh.x, lh.y + rh.y, lh.z + rh.z);
        }

        public static Point3D operator -(Point3D lh, Point3D rh)
        {
            return lh + (-rh);
        }

        public override string ToString()
        {
            return x.ToString() + " " + y.ToString();
        }

    }


}