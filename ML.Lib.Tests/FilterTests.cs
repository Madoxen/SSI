using Microsoft.VisualStudio.TestTools.UnitTesting;
using ML.Lib.Image;
using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace ML.Lib.Tests
{
    [Ignore]
    [TestClass]
    public class FilterTests
    {
        [TestMethod]
        public void GaussianFilterCtorTest()
        {
            GaussianFilter filter = new GaussianFilter(Math.Sqrt(2), 5, 5);
            
        }


        [TestMethod]
        public void GaussianFilterTest()
        {
            GaussianFilter filter = new GaussianFilter(Math.Sqrt(2), 5, 5);

            Bitmap b = new Bitmap("Resources/test_image1.jpg");
          //  filter.UseFilter(b);

        }

    }
}
