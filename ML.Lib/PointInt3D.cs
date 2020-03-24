namespace ML.Lib.Image
{

    public struct PointInt3D
    {

        public int x;
        public int y;
        public int z;


        public PointInt3D(int nx, int ny, int nz)
        {
            x = nx;
            y = ny;
            z = nz;
        }


        public static PointInt3D operator +(PointInt3D lh)
        {
            return lh;
        }

        public static PointInt3D operator -(PointInt3D lh)
        {
            return new PointInt3D(-lh.x, -lh.y, -lh.z);
        }

        public static PointInt3D operator +(PointInt3D lh, PointInt3D rh)
        {
            return new PointInt3D(lh.x + rh.x, lh.y + rh.y, lh.z + rh.z);
        }

        public static PointInt3D operator -(PointInt3D lh, PointInt3D rh)
        {
            return lh + (-rh);
        }

        public override string ToString()
        {
            return x.ToString() + " " + y.ToString() + " " + z.ToString();
        }


    }


}