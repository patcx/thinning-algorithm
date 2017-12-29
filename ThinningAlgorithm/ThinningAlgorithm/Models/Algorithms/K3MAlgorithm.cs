using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinningAlgorithm.Helpers;

namespace ThinningAlgorithm.Models.Algorithms
{
    public class K3MAlgorithm : ITransformAlgorithm
    {
        private int[][] A;

        public K3MAlgorithm()
        {
            A = new int[6][];
            A[0] = new int[]
            {
                3, 5, 7, 12, 13, 14, 15, 20,
                21, 22, 23, 28, 29, 30, 31, 48,
                52, 53, 54, 55, 56, 60, 61, 62,
                63, 65, 67, 69, 71, 77, 79, 80,
                81, 83, 84, 85, 86, 87, 88, 89,
                91, 92, 93, 94, 95, 97, 99, 101,
                103, 109, 111, 112, 113, 115, 116, 117,
                118, 119, 120, 121, 123, 124, 125, 126,
                127, 131, 133, 135, 141, 143, 149, 151,
                157, 159, 181, 183, 189, 191, 192, 193,
                195, 197, 199, 205, 207, 208, 209, 211,
                212, 213, 214, 215, 216, 217, 219, 220,
                221, 222, 223, 224, 225, 227, 229, 231,
                237, 239, 240, 241, 243, 244, 245, 246,
                247, 248, 249, 251, 252, 253, 254, 255
            };
            A[1] = new int[] {7, 14, 28, 56, 112, 131, 193, 224};
            A[2] = new int[] {7, 14, 15, 28, 30, 56, 60, 112, 120, 131, 135, 193, 195, 224, 225, 240};
            A[3] = new int[] {7, 14, 15, 28, 30, 31, 56, 60, 62, 112, 120,124, 131, 135, 143, 193, 195, 199, 224, 225, 227, 240, 241, 248};
            A[4] = new int[]
            {
                7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120, 124, 126, 131, 135, 143, 159, 193, 195, 199, 207, 224,
                225, 227, 231, 240, 241, 243, 248, 249, 252
            };
            A[5] = new int[]
            {
                7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120,
                124, 126, 131, 135, 143, 159, 191, 193, 195, 199,
                207, 224, 225, 227, 231, 239, 240, 241, 243, 248,
                249, 251, 252, 254
            };
        }

        public void Transform(Bitmap bmp)
        {
            var result = bmp;
            bool isAnyChange = false;
            var imageSource = result.ConvertToBytes();
            do
            {
                isAnyChange = false;
                for (int i = 0; i < 6; i++)
                {
                    if (Phase(imageSource, i) && i!=0)
                        isAnyChange = true;
                }
                PhaseUnmark(imageSource);

            } while (isAnyChange); 

            result.LoadFromBytes(imageSource, 1);
        }

        private void PhaseUnmark(byte[,] imageSource)
        {
            for (int j = 1; j < imageSource.GetLength(0) - 1; j++)
            {
                for (int i = 1; i < imageSource.GetLength(1) - 1; i++)
                {
                    if (imageSource[j, i] == 2)
                        imageSource[j, i] = 1;
                }
            }
        }

        private bool Phase(byte[,] imageSource, int phaseNr)
        {
            bool isAnyChange = false;   
            for (int j = 1; j < imageSource.GetLength(0) - 1; j++)
            {
                for (int i = 1; i < imageSource.GetLength(1) - 1; i++)
                {
                    if ((imageSource[j, i] != 2 && phaseNr != 0) || (phaseNr == 0 && imageSource[j,i] != 1))
                        continue;

                    var weight = GetWeightOfPixels(imageSource, j, i);
                    if (A[phaseNr].Contains(weight))
                    {
                        isAnyChange = true;
                        imageSource[j, i] = (byte)(phaseNr == 0 ? 2 : 0);
                    }
                }
            }

            return isAnyChange;
        }


        private int GetWeightOfPixels(byte[,] imageSource, int j, int i)
        {
            int sum = 0;
            if (imageSource[j - 1, i] > 0)
                sum += 1;
            if (imageSource[j - 1, i + 1] > 0)
                sum += 2;
            if (imageSource[j, i + 1] > 0)
                sum += 4;
            if (imageSource[j + 1, i + 1] > 0)
                sum += 8;
            if (imageSource[j + 1, i] > 0)
                sum += 16;
            if (imageSource[j + 1, i - 1] > 0)
                sum += 32;
            if (imageSource[j, i - 1] > 0)
                sum += 64;
            if (imageSource[j - 1, i - 1] > 0)
                sum += 128;

            return sum;
        }
    }
}
