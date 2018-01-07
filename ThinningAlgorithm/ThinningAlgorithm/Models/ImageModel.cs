using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using ThinningAlgorithm.Helpers;
using ThinningAlgorithm.Models.Algorithms;
using MahApps.Metro.Converters;

namespace ThinningAlgorithm.Models
{
    public class ImageModel
    {
        private Bitmap imageResult;
        private Bitmap imageLoaded;


        public ImageModel(string path)
        {
            imageLoaded = ((Bitmap)Image.FromFile(path)).ConvertImage(ImageFormat.Jpeg);
            imageLoaded.ApplyTransform(new Grayscale());
            imageLoaded.ApplyTransform(new Binaryzation(0.3f));
            imageResult = (Bitmap)imageLoaded.Clone();
        }

        public void ProcessKMM()
        {
            imageResult = (Bitmap)imageLoaded.Clone();
            imageResult.ApplyTransform(new KMMAlgorithm());
        }

        public void ProcessK3M()
        {
            imageResult = (Bitmap)imageLoaded.Clone();
            imageResult.ApplyTransform(new K3MAlgorithm());
        }

        public BitmapImage GetImageResult()
        {
            return imageResult.ToBitmapImage();
        }
    }
}
