using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThinningAlgorithm.Helpers;

namespace ThinningAlgorithm.Models.Algorithms
{
    public class KMMAlgorithm : ITransformAlgorithm
    {
        public void Transform(Bitmap bmp)
        {
            var result = bmp;

            var imageSource = result.ConvertToBytes();

            bool isAnyRemoved2 = false;
            bool isAnyRemoved3 = false;
            do
            {
                FillWithTwoAndThree(imageSource);
                RemoveContourPoints(imageSource);
                isAnyRemoved2 = RemoveWithinDeletionArray(imageSource, 2);
                isAnyRemoved3 = RemoveWithinDeletionArray(imageSource, 3);

            } while (isAnyRemoved2 || isAnyRemoved3);

            result.LoadFromBytes(imageSource, 1);
        }

        private bool RemoveWithinDeletionArray(byte[,] imageSource, int n)
        {
            var deletionArray = new int[]
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

            bool isAnyRemoved = false;

            for (int j = 1; j < imageSource.GetLength(0) - 1; j++)
            {
                for (int i = 1; i < imageSource.GetLength(1) - 1; i++)
                {
                    if (imageSource[j, i] != n)
                        continue;

                    int weight = GetWeightOfPixels(imageSource, j, i);
                    if (deletionArray.Contains(weight))
                    {
                        imageSource[j, i] = 0;
                        isAnyRemoved = true;
                    }
                    else
                    {
                        imageSource[j, i] = 1;
                    }
                }
            }

            return isAnyRemoved;
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

        private void RemoveContourPoints(byte[,] imageSource)
        {
            List<(int, int)> indexesToClear = new List<ValueTuple<int, int>>();

            for (int j = 1; j < imageSource.GetLength(0) - 1; j++)
            {
                for (int i = 1; i < imageSource.GetLength(1) - 1; i++)
                {
                    if (imageSource[j, i] == 0)
                        continue;

                    List<(int, int)> indexes = new List<ValueTuple<int, int>>();

                    indexes.Add((j + 1, i + 1));
                    indexes.Add((j + 1, i));
                    indexes.Add((j + 1, i - 1));
                    indexes.Add((j, i - 1));
                    indexes.Add((j - 1, i - 1));
                    indexes.Add((j - 1, i));
                    indexes.Add((j - 1, i + 1));
                    indexes.Add((j, i + 1));

                    int counter = 0;
                    if (imageSource[indexes[0].Item1, indexes[0].Item2] > 0)
                    {
                        int k = 1;
                        while (k < indexes.Count && imageSource[indexes[k].Item1, indexes[k].Item2] > 0)
                            k++;

                        while (k < indexes.Count && imageSource[indexes[k].Item1, indexes[k].Item2] <= 0)
                        {
                            k++;
                            counter++;
                        }

                        while (k < indexes.Count && imageSource[indexes[k].Item1, indexes[k].Item2] > 0)
                            k++;

                        if (k != indexes.Count)
                            continue;

                        counter = 8 - counter;
                    }
                    else
                    {
                        int k = 1;
                        while (k < indexes.Count && imageSource[indexes[k].Item1, indexes[k].Item2] <= 0)
                            k++;

                        while (k < indexes.Count && imageSource[indexes[k].Item1, indexes[k].Item2] > 0)
                        {
                            k++;
                            counter++;
                        }

                        while (k < indexes.Count && imageSource[indexes[k].Item1, indexes[k].Item2] <= 0)
                            k++;

                        if (k != indexes.Count)
                            continue;
                    }

                    if (counter >= 2 && counter <= 4)
                    {
                        imageSource[j, i] = 4;
                        indexesToClear.Add((j, i));
                    }
                }
            }

            foreach (var index in indexesToClear)
            {
                imageSource[index.Item1, index.Item2] = 0;
            }
        }

        private void FillWithTwoAndThree(byte[,] imageSource)
        {
            for (int j = 1; j < imageSource.GetLength(0) - 1; j++)
            {
                for (int i = 1; i < imageSource.GetLength(1) - 1; i++)
                {
                    if (imageSource[j, i] == 0)
                        continue;
                    bool hasAllNeighbours = true;
                    int emptyCorners = 0;
                    if (imageSource[j, i - 1] == 0)
                    {
                        imageSource[j, i] = 2;
                        hasAllNeighbours = false;
                    }
                    else if (imageSource[j, i + 1] == 0)
                    {
                        imageSource[j, i] = 2;
                        hasAllNeighbours = false;
                    }
                    else if (imageSource[j - 1, i] == 0)
                    {
                        imageSource[j, i] = 2;
                        hasAllNeighbours = false;
                    }
                    else if (imageSource[j + 1, i] == 0)
                    {
                        imageSource[j, i] = 2;
                        hasAllNeighbours = false;
                    }
                    else if (imageSource[j - 1, i - 1] == 0)
                        emptyCorners++;
                    else if (imageSource[j - 1, i + 1] == 0)
                        emptyCorners++;
                    else if (imageSource[j + 1, i - 1] == 0)
                        emptyCorners++;
                    else if (imageSource[j + 1, i + 1] == 0)
                        emptyCorners++;

                    if (hasAllNeighbours && emptyCorners == 1)
                    {
                        imageSource[j, i] = 3;
                    }
                }
            }
        }
    }
}
