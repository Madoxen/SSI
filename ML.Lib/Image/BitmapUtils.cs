using System.Drawing;
using System;
using System.Drawing.Imaging;
using System.Linq;

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

            int maxDiff = int.MinValue;
            int minDiff = int.MaxValue;

            int[][][] data = new int[3][][];
            for (int i = 0; i < 3; i++)
            {
                data[i] = new int[lh.Width][];
                for (int j = 0; j < lh.Width; j++)
                {
                    data[i][j] = new int[lh.Height];
                }
            }



            for (int i = 0; i < lh.Width; i++)
            {
                for (int j = 0; j < lh.Height; j++)
                {

                    Color currentPixel = result.GetPixel(i, j);
                    Color currentRHPixel = rh.GetPixel(i, j);

                    data[0][i][j] = currentPixel.R - currentRHPixel.R;
                    data[1][i][j] = currentPixel.G - currentRHPixel.G;
                    data[2][i][j] = currentPixel.B - currentRHPixel.B;



                    if (data[0][i][j] > maxDiff)
                        maxDiff = data[0][i][j];
                    if (data[1][i][j] > maxDiff)
                        maxDiff = data[1][i][j];
                    if (data[2][i][j] > maxDiff)
                        maxDiff = data[2][i][j];

                    if (data[0][i][j] < minDiff)
                        minDiff = data[0][i][j];
                    if (data[1][i][j] < minDiff)
                        minDiff = data[1][i][j];
                    if (data[2][i][j] < minDiff)
                        minDiff = data[2][i][j];
                }
            }

            //Normalize data
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < lh.Width; j++)
                {
                    for (int k = 0; k < lh.Height; k++)
                    {
                        data[i][j][k] = (data[i][j][k] - minDiff)*(int)(255.0/(double)(maxDiff - minDiff));
                    }
                }
            }

            for (int j = 0; j < lh.Width; j++)
            {
                for (int k = 0; k < lh.Height; k++)
                {
                    result.SetPixel(j, k, Color.FromArgb(data[0][j][k], data[1][j][k], data[2][j][k]));
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
        public static void DrawSIFTFeature(Bitmap b, Pen pen, PointF p1, PointF p2)
        {

            using (Graphics g = Graphics.FromImage(b))
            {
                // Draw the shaft.
                g.DrawLine(pen, p1, p2);
                int length = (int)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2)) * 2;
                g.DrawEllipse(pen, (int)p1.X - (length / 2), (int)p1.Y - (length / 2), length, length);
            }

        }



    }



}