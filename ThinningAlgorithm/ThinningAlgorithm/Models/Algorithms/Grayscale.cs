using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinningAlgorithm.Models.Algorithms
{
    public class Grayscale : ITransformAlgorithm
    {
        public void Transform(Bitmap bmp)
        {
            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    var col = pixel.R / 3 + pixel.G / 3 + pixel.B / 3;
                    bmp.SetPixel(i, j, Color.FromArgb(col, col, col));
                }
            }
        }
    }
}
