namespace ML.Lib.Image
{

    public struct PointInt2D
    {

        public int x;
        public int y;


        public PointInt2D(int nx, int ny)
        {
            x = nx;
            y = ny;
        }


        public static PointInt2D operator +(PointInt2D lh)
        {
            return lh;
        }

        public static PointInt2D operator -(PointInt2D lh)
        {
            return new PointInt2D(-lh.x, -lh.y);
        }

        public static PointInt2D operator +(PointInt2D lh, PointInt2D rh)
        {
            return new PointInt2D(lh.x + rh.x, lh.y + rh.y);
        }

        public static PointInt2D operator -(PointInt2D lh, PointInt2D rh)
        {
            return lh + (-rh);
        }


        
    }


}