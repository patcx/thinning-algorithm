using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ThinningAlgorithm.Models;

namespace ThinningAlgorithm.Helpers
{
    public static class BitmapExtender
    {
        public static Bitmap ConvertImage(this Bitmap originalStream, ImageFormat format)
        {
            var stream = new MemoryStream();
            originalStream.Save(stream, format);
            stream.Position = 0;
            return (Bitmap)Image.FromStream(stream);
        }

        public static void LoadFromBytes(this Bitmap originalStream, byte[,] imageSource, params byte[] filledValues)
        {
            var colorEmpty = Color.FromArgb(255, 255, 255);
            var colorFilled = Color.FromArgb(0, 0, 0);

            for (int j = 0; j < imageSource.GetLength(0); j++)
            {
                for (int i = 0; i < imageSource.GetLength(1); i++)
                {
                    originalStream.SetPixel(i, j, filledValues.Contains(imageSource[j, i]) ? colorFilled : colorEmpty);
                    //originalStream.SetPixel(i, j, imageSource[j,i] == 0 ? colorEmpty : imageSource[j,i] == 2 || imageSource[j,i] == 1 ? colorFilled : imageSource[j,i] == 3 ? Color.Black : Color.Red);

                }
            }
        }

        public static byte[,] ConvertToBytes(this Bitmap image)
        {
            BitmapData sourceData = image.LockBits(new Rectangle(0, 0,
                                      image.Width, image.Height),
                                      ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            image.UnlockBits(sourceData);

            var resultArray = new byte[image.Height, image.Width];
            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    resultArray[j, i] = image.GetPixel(i, j).R != 0 ? (byte)0 : (byte)1;
                }
            }

            BitmapData resultData = image.LockBits(new Rectangle(0, 0,
                                        image.Width, image.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            image.UnlockBits(resultData);

            return resultArray;
        }

        public static BitmapImage ToBitmapImage(this Bitmap bmp)
        {
            using (var memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static void ApplyTransform(this Bitmap bmp, ITransformAlgorithm algorithm)
        {
           algorithm.Transform(bmp);
        }
    }
}
