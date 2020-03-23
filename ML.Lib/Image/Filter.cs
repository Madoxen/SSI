using System;
using System.Diagnostics;
using System.Drawing;


namespace ML.Lib.Image
{

    //Describes basic 2D filter
    public class Filter
    {
        public double[][] filterData { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }


        public Filter()
        {
            filterData = new double[3][];
            filterData[0] = new double[3] { -1, -1, -1 };
            filterData[1] = new double[3] { -1, 8, -1 };
            filterData[2] = new double[3] { -1, -1, -1 };
        }



        public Bitmap UseFilter(Bitmap input)
        {
            Bitmap map = new Bitmap(input.Width, input.Height);
            for (int i = 0; i < input.Width; i++)
            {
                for (int j = 0; j < input.Height; j++)
                {
                    double maskSum = 0;
                    double R = 0;
                    double B = 0;
                    double G = 0;
                    for (int k = -(filterData.Length / 2); k < (filterData.Length / 2) + 1; k++)
                    {
                        for (int g = -(filterData.Length / 2); g < (filterData.Length / 2) + 1; g++)
                        {
                            if (k + i < 0 || k + i >= input.Width || g + j < 0 || g + j >= input.Height)
                                continue;

                            Color filterPixel = input.GetPixel(k + i, g + j);
                            maskSum += filterData[k + 1][g + 1];
                            R += filterPixel.R * filterData[k + 1][g + 1];
                            B += filterPixel.B * filterData[k + 1][g + 1];
                            G += filterPixel.G * filterData[k + 1][g + 1];
                        }
                    }

                    if (maskSum == 0)
                        maskSum = 1;

                    R =  R / maskSum;
                    B =  B / maskSum;
                    G =  G /maskSum;



                    if (R < 0)
                        R = 0;

                    if (R > 255)
                        R = 255;

                    if (G < 0)
                        G = 0;

                    if (G > 255)
                        G = 255;

                    if (B < 0)
                        B = 0;

                    if (B > 255)
                        B = 255;



                    Color newColor = Color.FromArgb(Convert.ToInt32(R), Convert.ToInt32(G), Convert.ToInt32(B));
                    map.SetPixel(i, j, newColor);
                }
            }

            map.Save("ree.png");
            return map;
        }







    }
}
