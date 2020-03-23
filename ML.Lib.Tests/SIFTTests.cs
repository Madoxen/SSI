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
            List<List<Bitmap>> octaves = SIFT.BuildGaussianPyramid(b, 4, 5);
            int i = 0;
            int j = 0;
            foreach (List<Bitmap> level in octaves)
            {
                foreach (Bitmap bm in level)
                {
                    bm.Save("SIFT_TEST_" + i + "_" + j + ".png");
                    j++;
                }
                i++;
            }

        }
    }
}
