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

        public static double intensityTreshold = 0.2; //treshold that must be exceeded by keypoint to become eligable
        public static double curvatureTreshold = 2;


        public static Bitmap Perform(Bitmap input)
        {
            List<PointInt2D> keypoints = new List<PointInt2D>();
            //Convert input to grayscale
            Bitmap gray = BitmapUtils.ConvertToGrayscale(input);

            //Build scale pyramid
            List<List<Bitmap>> scales = BuildGaussianPyramid(gray, octaves, blurLevels);
            List<List<Bitmap>> dogs = CreateDoGs(scales);
            List<Keypoint> keypointCandidates = FindKeypoints(dogs);
            //Perform tests
            ContrastTest(keypointCandidates);
            EdgeTest(keypointCandidates);

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
                for (int j = 0; j < numberOfBlurLevels - 1; j++)
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


        public static List<List<Bitmap>> CreateDoGs(List<List<Bitmap>> octaves)
        {

            List<List<Bitmap>> result = new List<List<Bitmap>>();
            for (int i = 0; i < octaves.Count; i++)
            {
                result.Add(new List<Bitmap>());
                for (int j = 0; j < octaves[i].Count - 1; j++)
                {
                    result[i].Add(BitmapUtils.Subtract(octaves[i][j], octaves[i][j + 1]));
                }
            }
            return result;
        }



        //This method finds subpixel minima/maxima
        public static List<Keypoint> FindKeypoints(List<List<Bitmap>> DoGs)
        {
            List<Keypoint> candidates = new List<Keypoint>();
            int octaveNumber = 0;
            foreach (List<Bitmap> octave in DoGs) //octave axis
            {
                for (int i = 1; i < octave.Count - 1; i++) // for each middle layer
                {
                    for (int j = 1; j < octave[i].Width - 1; j++) //pixel X axis, ignore edges
                    {
                        for (int k = 1; k < octave[i].Height - 1; k++) //pixel Y axis, ignore edges
                        {
                            if (IsPixelEligable(octave, i, j, k))
                            {
                                candidates.Add(new Keypoint(j, k, i, octaveNumber, octave[i]));
                            }
                        }
                    }
                }
                octaveNumber++;
            }

            return candidates;
        }



        private static bool IsPixelEligable(List<Bitmap> currentOctave, int currentLevel, int currentX, int currentY)
        {
            int previousSign = 0; //trinary variable 0 -> equals; 1 -> bigger then; -1 -> smaller then 
            int currentSign = 0;
            bool firstTime = false;
            Color checkedPixel = currentOctave[currentLevel].GetPixel(currentX, currentY);

            for (int i = -1; i < 2; i++) //scale axis
            {
                for (int j = -1; j < 2; j++) // x axis
                {
                    for (int k = -1; k < 2; k++) //y axis
                    {
                        Color currentPixel = currentOctave[currentLevel + i].GetPixel(currentX + j, currentY + k);

                        if (checkedPixel.R == currentPixel.R && j != 0 && k != 0 && i != 0)
                            return false;

                        if (checkedPixel.R > currentPixel.R)
                            currentSign = 1;
                        if (checkedPixel.R < currentPixel.R)
                            currentSign = -1;


                        if (!firstTime)
                        {
                            previousSign = currentSign;
                            firstTime = true;
                            continue;
                        }

                        if (currentSign != previousSign)
                            return false;

                    }
                }
            }
            return true;
        }




        private static void CalculateSubPixelCoords(List<Keypoint> keypoints, List<List<Bitmap>> DoGs)
        {
            
        }

        //Reject keypoint pixels that are below treshold intensity
        public static void ContrastTest(List<Keypoint> keypoints)
        {
            for(int i = 0; i < keypoints.Count; i++)
            {
                Keypoint k = keypoints[i];
                if(k.underlayingBitmap.GetPixel((int)k.coords.x, (int)k.coords.y).GetBrightness() < intensityTreshold)
                    keypoints.Remove(k);
            }
        }



        //Reject keypoints that are on edge
        public static void EdgeTest(List<Keypoint> keypoints)
        {
            double[][] hessian = new double[2][];
            hessian[0] = new double[2];
            hessian[1] = new double[2];

            for(int i = 0; i < keypoints.Count; i++)
            {
                Keypoint k = keypoints[i];

                //dxx
                double dxx = (k.underlayingBitmap.GetPixel(k.intCoords.x + 1, k.intCoords.y).R -
                2 * k.underlayingBitmap.GetPixel(k.intCoords.x, k.intCoords.y).R
                + k.underlayingBitmap.GetPixel(k.intCoords.x - 1, k.intCoords.y).R) / 4.0;

                //dyy
                double dyy = (k.underlayingBitmap.GetPixel(k.intCoords.x, k.intCoords.y + 1).R
                - 2 * k.underlayingBitmap.GetPixel(k.intCoords.x, k.intCoords.y).R 
                + k.underlayingBitmap.GetPixel(k.intCoords.x, k.intCoords.y - 1).R) / 4.0;


                //dxy also dyx from derivative theorem
                double dxy = (k.underlayingBitmap.GetPixel(k.intCoords.x + 1, k.intCoords.y + 1).R -
                k.underlayingBitmap.GetPixel(k.intCoords.x - 1, k.intCoords.y + 1).R -
                k.underlayingBitmap.GetPixel(k.intCoords.x + 1, k.intCoords.y - 1).R +
                k.underlayingBitmap.GetPixel(k.intCoords.x - 1, k.intCoords.y - 1).R) / 4.0;


                double trace = dxx + dyy;
                double determinant = dxx*dxy - dyy*dxy;
                double check = (trace*trace)/determinant;
                //If check is bigger then curvature then reject it
                if(check >= ( curvatureTreshold + 1.0 )*( curvatureTreshold + 1.0 ) / curvatureTreshold)
                    keypoints.Remove(k);
            }

           

        }



        //Calculates derivative around a pixel
        private static Point3D Derivative3D(List<Bitmap> dogOctave, PointInt3D pixelCoord)
        {
            double dx = (dogOctave[pixelCoord.z].GetPixel(pixelCoord.x + 1, pixelCoord.y).R - dogOctave[pixelCoord.z].GetPixel(pixelCoord.x - 1, pixelCoord.y).R) / 2.0;
            double dy = (dogOctave[pixelCoord.z].GetPixel(pixelCoord.x, pixelCoord.y + 1).R - dogOctave[pixelCoord.z].GetPixel(pixelCoord.x, pixelCoord.y - 1).R) / 2.0;
            double dz = (dogOctave[pixelCoord.z + 1].GetPixel(pixelCoord.x, pixelCoord.y).R - dogOctave[pixelCoord.z - 1].GetPixel(pixelCoord.x, pixelCoord.y).R) / 2.0;
            return new Point3D(dx, dy, dz);
        }

        //Calculates hessian matrix out of octave around given pixel
        // dxx dxy dxz
        // dyx dyy dyz
        // dzx dzy dzz
        private static double[][] Hessian3D(List<Bitmap> dogOctave, PointInt3D pixelCoord)
        {

            double[][] result = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                result[i] = new double[3];
            }

            double kernelPixel = dogOctave[pixelCoord.z].GetPixel(pixelCoord.x, pixelCoord.y).R;

            //dxx
            double dxx = (dogOctave[pixelCoord.z].GetPixel(pixelCoord.x + 1, pixelCoord.y).R -
            2 * kernelPixel
             + dogOctave[pixelCoord.z].GetPixel(pixelCoord.x - 1, pixelCoord.y).R) / 4.0;

            //dyy
            double dyy = (dogOctave[pixelCoord.z].GetPixel(pixelCoord.x, pixelCoord.y + 1).R
             - 2 * kernelPixel 
             + dogOctave[pixelCoord.z].GetPixel(pixelCoord.x, pixelCoord.y - 1).R) / 4.0;
            //dzz
            double dzz = (dogOctave[pixelCoord.z + 1].GetPixel(pixelCoord.x, pixelCoord.y).R -
             2 * kernelPixel + 
             dogOctave[pixelCoord.z - 1].GetPixel(pixelCoord.x, pixelCoord.y - 1).R) / 4.0;

            //dxy also dyx from derivative theorem
            double dxy = (dogOctave[pixelCoord.z].GetPixel(pixelCoord.x + 1, pixelCoord.y + 1).R -
            dogOctave[pixelCoord.z].GetPixel(pixelCoord.x - 1, pixelCoord.y + 1).R -
            dogOctave[pixelCoord.z].GetPixel(pixelCoord.x + 1, pixelCoord.y - 1).R +
            dogOctave[pixelCoord.z].GetPixel(pixelCoord.x - 1, pixelCoord.y - 1).R) / 4.0;


            //dxz also dzx from derivative theorem
            double dxz = (dogOctave[pixelCoord.z + 1].GetPixel(pixelCoord.x + 1, pixelCoord.y).R -
            dogOctave[pixelCoord.z + 1].GetPixel(pixelCoord.x - 1, pixelCoord.y).R -
            dogOctave[pixelCoord.z - 1].GetPixel(pixelCoord.x + 1, pixelCoord.y).R +
            dogOctave[pixelCoord.z - 1].GetPixel(pixelCoord.x - 1, pixelCoord.y).R) / 4.0;


            //dyz also dzy from derivative theorem
            double dyz = (dogOctave[pixelCoord.z + 1].GetPixel(pixelCoord.x, pixelCoord.y + 1).R -
            dogOctave[pixelCoord.z + 1].GetPixel(pixelCoord.x, pixelCoord.y - 1).R -
            dogOctave[pixelCoord.z - 1].GetPixel(pixelCoord.x, pixelCoord.y + 1).R +
            dogOctave[pixelCoord.z - 1].GetPixel(pixelCoord.x, pixelCoord.y - 1).R) / 4.0;

            result[0][0] = dxx;
            result[0][1] = dxy;
            result[0][2] = dxz;
            result[1][0] = dxy;
            result[1][1] = dyy;
            result[1][2] = dyz;
            result[2][0] = dxz;
            result[2][1] = dyz;
            result[2][2] = dzz;

            return result;
        }


        





        public class Keypoint
        {
            public Point2D coords; // x -> Pixel X axis, Y-> Pixel Y Axis, Z -> Octave Axis



            public PointInt2D intCoords
            {
                get {return new PointInt2D((int)coords.x,(int)coords.y);}
            }


            public double magnitude;
            public double orientation;


            public int octave;
            public int layer;

            public readonly Bitmap underlayingBitmap; //Underlaying bitmap in which this keypoint was created;

            public Keypoint(int x, int y, int layer, int octave, Bitmap underlayingBitmap)
            {
                coords = new Point2D(x, y);
                magnitude = 0;
                orientation = 0;
                this.octave = octave;
                this.layer = layer;
                this.underlayingBitmap = underlayingBitmap;
            }

        }



    }
}