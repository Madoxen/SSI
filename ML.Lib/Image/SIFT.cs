using System;
using System.Drawing;
using System.Collections.Generic;


namespace ML.Lib.Image
{
    //Performs sift operation on an image
    public class SIFT
    {

        private static readonly int octaves = 4;
        private static readonly int blurLevels = 5; 

        public static Bitmap Perform(Bitmap input)
        {
            List<PointInt2D> keypoints = new List<PointInt2D>();
            //Convert input to grayscale
            Bitmap gray = BitmapUtils.ConvertToGrayscale(input);
            
            //Build scale pyramid
            List<List<Bitmap>> scales = BuildGaussianPyramid(gray, octaves, blurLevels);


            return gray;
        }


        //Build pyramid composed of 
        public static List<List<Bitmap>> BuildGaussianPyramid(Bitmap gray, int numberOfOctaves, int numberOfBlurLevels)
        {

            //First create DoG (Diffrence of Gaussians Image) by filtering input with two Gaussian Filters
            //And then substructing results from each other
            GaussianFilter filter = new GaussianFilter(Math.Sqrt(2), 5, 5);
            List<List<Bitmap>> octaves = new List<List<Bitmap>>();

            Bitmap current = new Bitmap(gray);
            double scale = 1.0;
            for (int i = 0; i < numberOfOctaves; i++)
            {
                //shrink to half a size
                octaves.Add(new List<Bitmap>());
                octaves[i].Add(current); //Add current, not blurred bitmap;
                for(int j = 0; j < numberOfBlurLevels; j++)
                {
                    //blur bitmap
                    current = filter.UseFilter(current);
                    octaves[i].Add(current);
                }
                scale *= 0.5;
                current = BitmapUtils.Resize(gray, scale);
            }

            return octaves;
        }



        private static List<Point2D> FindKeypoints()
        {

return null;
        }


        private static bool CheckCandidatePixel(Bitmap DoGImage, int i, int j)
        {
            Color CurrentlyInvestigatedPixel = DoGImage.GetPixel(i, j);

            //Search 3x3 area with ij in center
            for (int k = -1; k < 2; k++)
            {
                for (int l = -1; l < 2; l++)
                {
                    Color CurrentPixel = DoGImage.GetPixel(i + k, j + l);
                    if (CurrentlyInvestigatedPixel.R == CurrentPixel.R ||
                    CurrentlyInvestigatedPixel.G == CurrentPixel.G ||
                    CurrentlyInvestigatedPixel.B == CurrentPixel.B) { return false; }
                }
            }

            return true;
        }



    }





}