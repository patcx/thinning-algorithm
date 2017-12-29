using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using ThinningAlgorithm.Models;
using Microsoft.Win32;

namespace ThinningAlgorithm.ViewModels
{
    public class MainVM : ObservableObject
    {
        private ImageModel model;

        public bool IsLoading { get; set; }
        public BitmapImage Image { get; private set; }

        public ICommand Loaded => new RelayCommand(() =>
        {
            try
            {
                LoadImageFromPath(@"E:\OneDrive\Fingerprints\test.bmp");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        });

        public ICommand LoadImage => new RelayCommand(() =>
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName != null && File.Exists(dialog.FileName))
            {
                try
                {
                   LoadImageFromPath(dialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        });

        private void LoadImageFromPath(string path)
        {
            IsLoading = true;
            RaisePropertyChanged("IsLoading");

            Task.Run(() =>
            {
                model = new ImageModel(path);
                ReloadImage();

                IsLoading = false;
                RaisePropertyChanged("IsLoading");
            });
           
        }

        private void ReloadImage()
        {
            Image = model.GetImageResult();
            RaisePropertyChanged("Image");
        }
    }
}
