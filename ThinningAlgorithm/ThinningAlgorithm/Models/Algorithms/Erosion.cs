using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinningAlgorithm.Models.Algorithms
{
    public class Erosion : ITransformAlgorithm
    {
        private int repeat;

        public Erosion(int repeat = 1)
        {
            this.repeat = repeat;
        }

        private void ExecuteErosion(Bitmap bmp)
        {
            bool[,] pixels = new bool[bmp.Height, bmp.Width];

            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    var pixel = bmp.GetPixel(i, j);
                    if (pixel.R != 0)
                    {
                        SetNeighbours(pixels, j, i, true);
                    }
                }
            }

            for (int j = 0; j < bmp.Height; j++)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    if (pixels[j, i])
                        bmp.SetPixel(i, j, Color.White);
                }
            }
        }

        private void SetNeighbours(bool[,] array, int j, int i, bool result)
        {
            array[j, i] = result;

            if (j - 1 >= 0)
            {
                array[j - 1, i] = result;

                if (i - 1 >= 0)
                {
                    array[j - 1, i - 1] = result;
                }

                if (i + 1 < array.GetLength(1))
                {
                    array[j - 1, i + 1] = result;
                }

            }

            if (j + 1 < array.GetLength(0))
            {
                array[j + 1, i] = result;

                if (i - 1 >= 0)
                {
                    array[j + 1, i - 1] = result;
                }

                if (i + 1 < array.GetLength(1))
                {
                    array[j + 1, i + 1] = result;
                }

            }

            if (i - 1 >= 0)
            {
                array[j, i - 1] = result;
            }

            if (i + 1 < array.GetLength(1))
            {
                array[j, i + 1] = result;
            }
        }

        public void Transform(Bitmap bmp)
        {
            for (int i = 0; i < repeat; i++)
            {
                ExecuteErosion(bmp);
            }
        }
    }
}
