using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace task1
{

    public class Graphics
    {
        public Bitmap UseFilter(double[][] filter, Bitmap input)
        {
            BasicMatrixFilter f = new BasicMatrixFilter(filter);
            return f.Operate(input);
        }


    }
}
