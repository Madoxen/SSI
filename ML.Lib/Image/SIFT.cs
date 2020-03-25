using System;
using System.Drawing;
using System.Collections.Generic;
using ML.Lib;

namespace ML.Lib.Image
{
    //Performs sift operation on an image
    public class SIFT
    {

        private static readonly int octaves = 4;
        private static readonly int blurLevels = 5;


        public static double sigmaFactor = Math.Sqrt(2);
        public static double scaleChange = 0.5; //Scale change between octaves

        public static double intensityTreshold = 0.03; //treshold that must be exceeded by keypoint to become eligable
        public static double curvatureTreshold = 10;

        public static int collectionRadiusPerOctave = (int)(3.0 * sigmaFactor);
        public static int orientationBins = 36;


        public static int histogramSmoothingPasses = 4;
        public static double histogramEligableTreshold = 0.8;


        private static Bitmap grayBlured;

        public static Bitmap Perform(Bitmap input)
        {

            //Convert input to grayscale
            Bitmap gray = BitmapUtils.ConvertToGrayscale(input);
            //resize picture to be 2x size to detect highest spacial frequencies
            gray = BitmapUtils.Resize(gray, 2.0);
            //Build scale pyramid
            List<List<Bitmap>> scales = BuildGaussianPyramid(gray, octaves, blurLevels);
            List<List<Bitmap>> dogs = CreateDoGs(scales);
            List<Keypoint> keypointCandidates = FindKeypoints(dogs);
            Console.WriteLine("Keypoint Candidates found: " + keypointCandidates.Count);
            //Perform tests
            ContrastTest(keypointCandidates);
            EdgeTest(keypointCandidates);

            AssignKeypointParameters(keypointCandidates, scales);
            AdjustForDoubleSize(keypointCandidates);

            Pen p = Pens.Blue;
            Bitmap result = new Bitmap(input);
            Console.WriteLine("Keypoints left: " + keypointCandidates.Count);
            foreach (SIFT.Keypoint c in keypointCandidates)
            {
                Point2D transformedCoords = c.coords;
                //Transform coords
                if (c.octave != 0)
                {
                    transformedCoords = new Point2D(c.coords.x * (Math.Pow(1 / SIFT.scaleChange, c.octave)), c.coords.y * (Math.Pow(1 / SIFT.scaleChange, c.octave)));
                }

                BitmapUtils.DrawSIFTFeature(result, p, new PointF((float)transformedCoords.x, (float)transformedCoords.y),
                 new PointF((float)(transformedCoords.x + c.scale * 25 * Math.Cos(c.orientation * 10 * (Math.PI / 180))),
                 (float)(transformedCoords.y + c.scale * 25 * Math.Sin(c.orientation * 10 * (Math.PI / 180)))));

            }
            return result;
        }


        //Build pyramid composed of 
        public static List<List<Bitmap>> BuildGaussianPyramid(Bitmap gray, int numberOfOctaves, int numberOfBlurLevels)
        {

            //First create DoG (Diffrence of Gaussians Image) by filtering input with two Gaussian Filters
            //And then substructing results from each other
            GaussianFilter filter = new GaussianFilter(sigmaFactor, 5, 5);
            List<List<Bitmap>> octaves = new List<List<Bitmap>>();

            grayBlured = filter.UseFilter(gray);
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
                scale *= scaleChange;
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
            int val = currentOctave[currentLevel].GetPixel(currentX, currentY).R - 128;



            if (val > 0)
            {
                for (int i = -1; i < 2; i++) //scale axis
                    for (int j = -1; j < 2; j++) // x axis
                        for (int k = -1; k < 2; k++) //y axis
                            if (val < currentOctave[currentLevel].GetPixel(currentX + j, currentY + k).R - 128)
                                return false;
            }
            else
            {
                for (int i = -1; i < 2; i++) //scale axis
                    for (int j = -1; j < 2; j++) // x axis
                        for (int k = -1; k < 2; k++) //y axis
                            if (val > currentOctave[currentLevel].GetPixel(currentX + j, currentY + k).R - 128)
                                return false;
            }
            return true;
        }




        private static void CalculateSubPixelCoords(List<Keypoint> keypoints, List<List<Bitmap>> DoGs)
        {

        }

        //Reject keypoint pixels that are below treshold intensity
        public static void ContrastTest(List<Keypoint> keypoints)
        {
            int count = keypoints.Count;
            Console.WriteLine("ContrastTest: Running...");
            //Console.WriteLine(Math.Abs((int)k.underlayingBitmap.GetPixel((int)k.coords.x, (int)k.coords.y).R - 128));

            keypoints.RemoveAll(k => CalculateContrastBlob(k, 1, 1) < intensityTreshold);
            Console.WriteLine("ContrastTest: Excluded - " + (count - keypoints.Count) + " keypoints");
        }


        //Calculates contrast base around the area around keypoint
        private static double CalculateContrastBlob(Keypoint keypoint, int range, double falloff)
        {
            double pixelVal = 0;
            for (int i = -range; i < range + 1; i++)
            {
                for (int j = -range; j < range + 1; j++)
                {
                    if (i + keypoint.intCoords.x < 0 || i + keypoint.intCoords.x >= keypoint.underlayingBitmap.Width ||
                    j + keypoint.intCoords.y < 0 || j + keypoint.intCoords.y >= keypoint.underlayingBitmap.Height)
                        continue;

                    Color pixel = keypoint.underlayingBitmap.GetPixel(i + keypoint.intCoords.x, j + keypoint.intCoords.y);
                    pixelVal += pixel.R / (Math.Abs(range + 1) * falloff); //further pixels are worth less
                }
            }
            pixelVal /= range * range;
            //Console.WriteLine(pixelVal);
            return pixelVal;
        }


        //Reject keypoints that are on edge
        public static void EdgeTest(List<Keypoint> keypoints)
        {
            Console.WriteLine("EdgeTest running...");
            int count = keypoints.Count;

            for (int i = 0; i < keypoints.Count; i++)
            {
                Keypoint k = keypoints[i];
                PointInt2D intCoords = k.intCoords;

                //dxx
                double dxx = (k.underlayingBitmap.GetPixel(intCoords.x + 1, intCoords.y).R -
                2 * k.underlayingBitmap.GetPixel(intCoords.x, intCoords.y).R
                + k.underlayingBitmap.GetPixel(intCoords.x - 1, intCoords.y).R);

                //dyy
                double dyy = (k.underlayingBitmap.GetPixel(intCoords.x, intCoords.y + 1).R
                - 2 * k.underlayingBitmap.GetPixel(intCoords.x, intCoords.y).R
                + k.underlayingBitmap.GetPixel(intCoords.x, intCoords.y - 1).R);


                //dxy also dyx from derivative theorem
                double dxy = (k.underlayingBitmap.GetPixel(intCoords.x + 1, intCoords.y + 1).R -
                k.underlayingBitmap.GetPixel(intCoords.x - 1, intCoords.y + 1).R -
                k.underlayingBitmap.GetPixel(intCoords.x + 1, intCoords.y - 1).R +
                k.underlayingBitmap.GetPixel(intCoords.x - 1, intCoords.y - 1).R) / 4.0;


                double trace = dxx + dyy;
                double determinant = dxx * dxy - dyy * dxy;


                /* negative determinant -> curvatures have different signs; reject feature */
                if (determinant <= 0)
                {
                    keypoints.Remove(k);
                    i--;
                    continue;
                }

                double check = (trace * trace) / determinant;

              //  Console.WriteLine(check + " VS " + ((curvatureTreshold + 1.0) * (curvatureTreshold + 1.0) / curvatureTreshold).ToString());
                //If check is bigger then curvature then reject it
                if (check >= (curvatureTreshold + 1.0) * (curvatureTreshold + 1.0) / curvatureTreshold)
                {
                    keypoints.Remove(k);
                    i--;
                }

            }
            Console.WriteLine("EdgeTest: Excluded - " + (count - keypoints.Count) + " keypoints");
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
        //used for calculating subpixel maxima/minima coords
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


        private double[][] Invert33Matrix(double[][] m)
        {

            double[][] result = new double[3][];
            for (int i = 0; i < 3; i++)
                result[i] = new double[3];

            // computes the inverse of a matrix m
            double det = m[0][0] * (m[1][1] * m[2][2] - m[2][1] * m[1][2]) -
                        m[0][1] * (m[1][0] * m[2][2] - m[1][2] * m[2][0]) +
                        m[0][2] * (m[1][0] * m[2][1] - m[1][1] * m[2][0]);

            double invdet = 1 / det;


            result[0][0] = (m[1][1] * m[2][2] - m[2][1] * m[1][2]) * invdet;
            result[0][1] = (m[0][2] * m[2][1] - m[0][1] * m[2][2]) * invdet;
            result[0][2] = (m[0][1] * m[1][2] - m[0][2] * m[1][1]) * invdet;
            result[1][0] = (m[1][2] * m[2][0] - m[1][0] * m[2][2]) * invdet;
            result[1][1] = (m[0][0] * m[2][2] - m[0][2] * m[2][0]) * invdet;
            result[1][2] = (m[1][0] * m[0][2] - m[0][0] * m[1][2]) * invdet;
            result[2][0] = (m[1][0] * m[2][1] - m[2][0] * m[1][1]) * invdet;
            result[2][1] = (m[2][0] * m[0][1] - m[0][0] * m[2][1]) * invdet;
            result[2][2] = (m[0][0] * m[1][1] - m[1][0] * m[0][1]) * invdet;

            return result;
        }


        //Calculates and assings keypoint magnitude and orientation
        public static void AssignKeypointParameters(List<Keypoint> keypoints, List<List<Bitmap>> initialSmooth)
        {

            for (int a = 0; a < keypoints.Count; a++)
            {
                Keypoint k = keypoints[a];

                int r = ((octaves - k.octave) * collectionRadiusPerOctave) + 2; //"radius" is simply square size
                double[] histogram = new double[36];
                double exp_denom = sigmaFactor * sigmaFactor * 2.0;

                //build histogram
                for (int i = -r; i <= r; i++)
                {
                    for (int j = -r; j <= r; j++)
                    {
                        if (i + k.coords.x < 0 || j + k.coords.y < 0 || i + k.coords.x > k.underlayingBitmap.Width || j + k.coords.y > k.underlayingBitmap.Height)
                            continue;

                        double dx = (double)initialSmooth[k.octave][1].GetPixel(k.intCoords.x + 1, k.intCoords.y).R - (double)initialSmooth[k.octave][1].GetPixel(k.intCoords.x - 1, k.intCoords.y).R;
                        double dy = (double)initialSmooth[k.octave][1].GetPixel(k.intCoords.x, k.intCoords.y + 1).R - (double)initialSmooth[k.octave][1].GetPixel(k.intCoords.x, k.intCoords.y - 1).R;
                        double magnitude = Math.Sqrt(dx * dx + dy * dy);
                        double orientation = Math.Atan2(dy, dx) * (180.0 / Math.PI);
                        orientation = orientation + 180;

                        double w = Math.Exp(-(i * i + j * j) / exp_denom);
                        int binID = (int)(Math.Round(orientation) / (360.0 / (double)orientationBins));
                        binID = (binID < orientationBins) ? binID : 0;
                        histogram[binID] += w * magnitude;
                    }
                }


                //Calculate scale
                k.orientation = histogram.MaxAt();
                k.scale = sigmaFactor * Math.Pow(scaleChange, k.octave);
                //    Console.WriteLine(k.orientation);
            }
        }



        public static void AdjustForDoubleSize(List<Keypoint> keypoints)
        {
            foreach (Keypoint k in keypoints)
            {
                k.coords = new Point2D(k.coords.x / 2.0, k.coords.y / 2.0);
                k.scale /= 2.0;
            }

        }


        public class Keypoint
        {
            public Point2D coords; // x -> Pixel X axis, Y-> Pixel Y Axis, Z -> Octave Axis



            public PointInt2D intCoords
            {
                get { return new PointInt2D((int)coords.x, (int)coords.y); }
            }


            public double scale;
            public double orientation;


            public int octave;
            public int layer;

            public readonly Bitmap underlayingBitmap; //Underlaying bitmap in which this keypoint was created;

            public Keypoint(int x, int y, int layer, int octave, Bitmap underlayingBitmap)
            {
                coords = new Point2D(x, y);
                scale = 0;
                orientation = 0;
                this.octave = octave;
                this.layer = layer;
                this.underlayingBitmap = underlayingBitmap;
            }

            //Creates deep copy of a keypoint
            public Keypoint(Keypoint k)
            {
                coords = new Point2D(coords.x, coords.y);
                scale = k.scale;
                orientation = k.orientation;
                octave = k.octave;
                layer = k.layer;
                underlayingBitmap = k.underlayingBitmap;
            }

            //Gets pixel from underlying bitmap
            public Color GetPixel()
            {
                return underlayingBitmap.GetPixel(intCoords.x, intCoords.y);
            }

            public Color GetPixel(PointInt2D offset)
            {
                return underlayingBitmap.GetPixel(intCoords.x + offset.x, intCoords.y + offset.y);
            }
        }

    }
}

