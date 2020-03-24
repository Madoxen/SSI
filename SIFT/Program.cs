using System;
using System.Drawing;
using ML.Lib.Image;
using ML.Lib;

namespace SiftProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap b = new Bitmap("Resources/cat3.jpg");
            SIFT.Perform(b);
        }
    }
}
