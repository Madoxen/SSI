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
        static List<List<PointInt3D>> candidates = null;


        [TestMethod]
        public void Test()
        {
            //  Bitmap b = new Bitmap("Resources/test_image1.jpg");
            //   SIFT.Perform(b).Save("doggo.png");

        }

        [TestMethod]
        public void TestPyramidBuilding()
        {
            Bitmap b = new Bitmap("Resources/test_image1.jpg");
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
           
            int currentBitmap = 1;
            for(int i = 0; i < candidates.Count; i++)
            {
                Bitmap b = new Bitmap(DoGs[i][0].Width, DoGs[i][0].Height);
                for(int j = 0; j < candidates[i].Count; j++)
                {
                    if(currentBitmap != candidates[i][j].x)
                    {
                        b.Save("../../../TestGeneratedFiles/KeypointCandidates/" + "points_" + currentBitmap  + "_" + i + "_" + j + ".png");
                        b = new Bitmap(DoGs[i][0].Width, DoGs[i][0].Height);
                        currentBitmap = candidates[i][j].x;
                    }
                        
                    b.SetPixel(candidates[i][j].y, candidates[i][j].z,Color.White);
                }
                
            }

            
        }
    }
}
