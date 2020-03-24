using System.Drawing;
using System;



namespace ML.Lib.Image
{

    public static class BitmapUtils
    {

        //Subtructs one bitmap from another, given that both of them are of the same size and dimension
        public static Bitmap Subtract(this Bitmap lh, Bitmap rh)
        {
            if (lh.Width != rh.Width || lh.Height != rh.Height)
                throw new ArgumentException("Bitmap are not of the same size");

            Bitmap result = new Bitmap(lh);

            for (int i = 0; i < lh.Width; i++)
            {
                for (int j = 0; j < lh.Height; j++)
                {

                    Color currentPixel = result.GetPixel(i, j);
                    Color currentRHPixel = rh.GetPixel(i, j);

                    int R = ComparableUtils.Bound(currentPixel.R - currentRHPixel.R, 0, 255);
                    int G = ComparableUtils.Bound(currentPixel.G - currentRHPixel.G, 0, 255);
                    int B = ComparableUtils.Bound(currentPixel.B - currentRHPixel.B, 0, 255);

                    result.SetPixel(i, j, Color.FromArgb(R, G, B));
                }
            }
            return result;
        }



        public static Bitmap ConvertToGrayscale(Bitmap bitmap)
        {
            Bitmap b = new Bitmap(bitmap);
            for (int i = 0; i < b.Width; i++)
            {
                for (int j = 0; j < b.Height; j++)
                {

                    Color c = b.GetPixel(i, j);
                    byte g = (byte)((c.R + c.B + c.G) / 3);


                    b.SetPixel(i, j, Color.FromArgb(g, g, g));
                }
            }
            return b;
        }



        //Resize using bilinear interpolation
        public static Bitmap Resize(Bitmap m, double factor)
        {
            int nwidth = (int)(m.Width * factor);
            int nheight = (int)(m.Height * factor);
            // C#
            Bitmap result = new Bitmap(nwidth, nheight);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                g.DrawImage(m, 0, 0, nwidth, nheight);
            }
            return result;
        }






        // Draw arrow heads or tails for the
        // segment from p1 to p2.
        public static void DrawArrow(Bitmap b, Pen pen, PointF p1, PointF p2,
            float length)
        {

            using (Graphics g = Graphics.FromImage(b))
            {
                // Draw the shaft.
                g.DrawLine(pen, p1, p2);

                // Find the arrow shaft unit vector.
                float vx = p2.X - p1.X;
                float vy = p2.Y - p1.Y;
                float dist = (float)Math.Sqrt(vx * vx + vy * vy);
                vx /= dist;
                vy /= dist;


                DrawArrowhead(g, pen, p1, -vx, -vy, length);
            }



        }

        // Draw an arrowhead at the given point
        // in the normalized direction <nx, ny>.
        private static void DrawArrowhead(Graphics gr, Pen pen,
            PointF p, float nx, float ny, float length)
        {
            float ax = length * (-ny - nx);
            float ay = length * (nx - ny);
            PointF[] points =
            {
                new PointF(p.X + ax, p.Y + ay),
                p,
                new PointF(p.X - ay, p.Y + ax)
            };
            gr.DrawLines(pen, points);
        }



    }



}