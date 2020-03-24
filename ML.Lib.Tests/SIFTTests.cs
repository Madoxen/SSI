using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML.Lib.Image;
using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;



namespace ML.Lib.Tests
{
    [TestClass]
    public class SIFTTests
    {



        static List<List<Bitmap>> octaves = null;
        static List<List<Bitmap>> DoGs = null;
        static List<SIFT.Keypoint> candidates = null;


        [TestMethod]
        public void Test()
        {
            //  Bitmap b = new Bitmap("Resources/test_image1.jpg");
            //   SIFT.Perform(b).Save("doggo.png");

        }

        [TestMethod]
        public void TestPyramidBuilding()
        {
            Bitmap b = new Bitmap("../../../Resources/cat3.jpg");
            b = BitmapUtils.ConvertToGrayscale(b);
            b = BitmapUtils.Resize(b, 2.0); //resize image 2x times to include highest spacial frequencies
            octaves = SIFT.BuildGaussianPyramid(b, 4, 5);
            int i = 0;
            int j = 0;
            foreach (List<Bitmap> level in octaves)
            {
                foreach (Bitmap bm in level)
                {
                    bm.Save("../../../TestGeneratedFiles/Blur/SIFT_TEST_" + i + "_" + j + ".png");
                    j++;
                }
                j = 0;
                i++;
            }
        }

        [TestMethod]
        public void TestDoGCreation()
        {
            DoGs = SIFT.CreateDoGs(octaves);
            int i = 0;
            int j = 0;
            foreach (List<Bitmap> level in DoGs)
            {
                foreach (Bitmap bm in level)
                {
                    bm.Save("../../../TestGeneratedFiles/DoGs/DoG_TEST_" + i + "_" + j + ".png");
                    j++;
                }
                i++;
                j = 0;
            }
        }

        [TestMethod]
        public void TestFindKeypoints()
        {
            candidates = SIFT.FindKeypoints(DoGs);
            Bitmap currentBitmap = null;
            int currentOctave = -1;
            int currentLayer = -1;
            foreach (SIFT.Keypoint c in candidates)
            {
                if (c.octave != currentOctave || c.layer != currentLayer)
                {
                    currentBitmap?.Save("../../../TestGeneratedFiles/KeypointCandidates/points" + currentOctave + "_" + currentLayer + ".png");
                    currentBitmap = new Bitmap(c.underlayingBitmap); //copy underlaying bitmap
                    currentOctave = c.octave;
                    currentLayer = c.layer;
                }


                currentBitmap.SetPixel((int)c.coords.x, (int)c.coords.y, Color.White);
            }

        }

        [TestMethod]
        public void TestContrastTest()
        {
            int co = candidates.Count;
            SIFT.ContrastTest(candidates);
            Debug.WriteLine("Test: " + (co - candidates.Count).ToString() + " rejected");

            Bitmap currentBitmap = null;
            int currentOctave = -1;
            int currentLayer = -1;
            foreach (SIFT.Keypoint c in candidates)
            {
                if (c.octave != currentOctave || c.layer != currentLayer)
                {
                    currentBitmap?.Save("../../../TestGeneratedFiles/AfterContrastTest/points" + currentOctave + "_" + currentLayer + ".png");
                    currentBitmap = new Bitmap(c.underlayingBitmap); //copy underlaying bitmap
                    currentOctave = c.octave;
                    currentLayer = c.layer;
                }


                currentBitmap.SetPixel((int)c.coords.x, (int)c.coords.y, Color.White);
            }
        }

        [TestMethod]
        public void TestEdgeTest()
        {
            int co = candidates.Count;
            SIFT.EdgeTest(candidates);
            Debug.WriteLine("Test: " + (co - candidates.Count).ToString() + " rejected");

            Bitmap currentBitmap = null;
            int currentOctave = -1;
            int currentLayer = -1;
            foreach (SIFT.Keypoint c in candidates)
            {
                if (c.octave != currentOctave || c.layer != currentLayer)
                {
                    currentBitmap?.Save("../../../TestGeneratedFiles/AfterEdgeTest/points" + currentOctave + "_" + currentLayer + ".png");
                    currentBitmap = new Bitmap(c.underlayingBitmap); //copy underlaying bitmap
                    currentOctave = c.octave;
                    currentLayer = c.layer;
                }


                currentBitmap.SetPixel((int)c.coords.x, (int)c.coords.y, Color.White);
            }
        }

        [TestMethod]
        public void TestAssignParameters()
        {
            SIFT.AssignKeypointParameters(candidates);

            Bitmap currentBitmap = null;
            int currentOctave = -1;
            int currentLayer = -1;
            Pen p = Pens.White;
            foreach (SIFT.Keypoint c in candidates)
            {
                if (c.octave != currentOctave || c.layer != currentLayer)
                {
                    currentBitmap?.Save("../../../TestGeneratedFiles/AfterAssignParameters/points" + currentOctave + "_" + currentLayer + ".png");
                    currentBitmap = new Bitmap(c.underlayingBitmap); //copy underlaying bitmap
                    currentOctave = c.octave;
                    currentLayer = c.layer;
                }

                BitmapUtils.DrawArrow(currentBitmap,p, new PointF((float)c.coords.x, (float)c.coords.y),
                 new PointF((float)(c.coords.x + c.scale*Math.Cos(c.orientation * (Math.PI/180))),
                 (float)(c.coords.y + c.scale*Math.Sin(c.orientation * (Math.PI/180)))),0.4f);
            }
        }
    }
}
