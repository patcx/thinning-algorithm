using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinningAlgorithm.Models.Algorithms
{
    public class Binaryzation : ITransformAlgorithm
    {
        private float threshold;


        public Binaryzation(float threshold = 0.5f)
        {
            this.threshold = threshold;
        }

        public void Transform(Bitmap bmp)
        {
            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.R > 256*threshold)
                        bmp.SetPixel(i, j, Color.White);
                    else
                        bmp.SetPixel(i, j, Color.Black);

                }
            }
        }
    }
}
