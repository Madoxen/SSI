using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ML.Lib;

namespace task1
{

    /// <summary>
    /// This filter assumes that we are using square arrays
    /// </summary>
    public class BasicMatrixFilter
    {
        public double[][] FilterData { get; set; }
        private double maskSum;

        public BasicMatrixFilter()
        {

        }

        public BasicMatrixFilter(double[][] FilterData)
        {
            this.FilterData = FilterData;
        }


        public Bitmap Operate(Bitmap Input)
        {

            int filterWidth = FilterData[0].Length;
            int filterHeight = FilterData.Length;
            Bitmap output = new Bitmap(Input);
            using (RawBitmap map = new RawBitmap(output))
            {
                Process(map, 0, 0, output.Width, output.Height, filterWidth, filterHeight);
            }

            return output;
        }

        private void Process(RawBitmap data, int x, int y, int endx, int endy, int filterWidth, int filterHeight)
        {
            filterWidth /= 2;
            filterHeight /= 2;

            maskSum = 0;
            for (int i = 0; i < FilterData.Length; i++)
            {
                for (int j = 0; j < FilterData[0].Length; j++)
                {
                    maskSum += FilterData[i][j];
                }
            }

            for (int i = 0; i < data.Width; i++)
            {
                for (int j = 0; j < data.Height; j++)
                {
                    double R = 0;
                    double B = 0;
                    double G = 0;


                    for (int k = -filterWidth; k < filterWidth + 1; k++)
                    {
                        for (int g = -filterHeight; g < filterHeight + 1; g++)
                        {
                            if (k + i < 0 || k + i >= data.Width || g + j < 0 || g + j >= data.Height)
                                continue;

                            Color filterPixel = data.GetPixel(k + i, g + j);
                            R += filterPixel.R * FilterData[k + (filterWidth)][g + (filterHeight)];
                            G += filterPixel.G * FilterData[k + (filterWidth)][g + (filterHeight)];
                            B += filterPixel.B * FilterData[k + (filterWidth)][g + (filterHeight)];
                        }
                    }

                    if (maskSum == 0)
                        maskSum = 1;

                    R = R / maskSum;
                    G = G / maskSum;
                    B = B / maskSum;


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
                    data.SetPixel(i, j, newColor);
                }
            }
        }
    }
}