using System;
using System.Drawing;


namespace ML.Lib.Image
{
    //Performs sift operation on an image
    public class SIFT
    {
        public static void Perform(Bitmap input)
        {


            //First create DoG (Diffrence of Gaussians Image) by filtering input with two Gaussian Filters
            //And then substructing results from each other
            GaussianFilter filterA = new GaussianFilter(Math.Sqrt(2), 5, 5);
            GaussianFilter filterB = new GaussianFilter(2, 5, 5);

            Bitmap A = filterA.UseFilter(input);
            Bitmap B = filterB.UseFilter(input);

            Bitmap DoGImage = new Bitmap(input.Width, input.Height);


            





        }



    }




}