using System;
using System.Drawing;


namespace task1
{
    class Program
    {
        static void Main(string[] args)
        {
            Graphics graphics = new Graphics();
            Bitmap b = graphics.UseFilter(
                new double[][]
                {
                    new double[] {-1.0,-1.0,-1.0},
                    new double[]{-1.0, 8.0,-1.0},
                    new double[]{-1.0, -1.0,-1.0},
                },
            new Bitmap("Resources/cat3.jpg"));

            b.Save("newCat3.jpg");

            Data d = new Data();
            d.Load("Resources/irisdataset.csv", "|");
            d.Shuffle();
            d.Normalize();
            d.Save("newIris.csv");


        }
    }
}
