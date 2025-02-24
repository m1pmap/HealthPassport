﻿using HealthPassport.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HealthPassport.Services
{
    public class ImageUpdater_Service : IImageUpdater
    {
        public void DeleteImage(Image image)
        {
            image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/images/without_picture.png"));
        }

        public void DownloadImageToClipboard(Image image)
        {
            BitmapSource imageSource = (BitmapSource)image.Source;
            Clipboard.SetImage(imageSource);
        }

        public bool DownloadImageToFile(Image image)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg;*.jpeg|All Files|*.*";
            saveFileDialog.Title = "Сохранить изображение сотрудника";

            if (saveFileDialog.ShowDialog() == true)
            {
                BitmapSource imageSource = (BitmapSource)image.Source;

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageSource));

                // Сохранение изображения в файл
                using (FileStream fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }

                return true;
            }
            return false;
        }

        public bool UploadImageFromClipboard(Image image)
        {
            if (Clipboard.ContainsImage()) //Если в буфере есть изображение
            {
                var clipboardImage = Clipboard.GetImage(); //Получение изображения из буфера

                image.Source = clipboardImage;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UploadImageFromFile(Image image)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"; //фильтрация то какие типы расширений может добавлять и видеть пользователь в проводнике
            openFileDialog.FilterIndex = 1; //установка на первый фильтр (значит пользователь сможет только фото видеть) 
            openFileDialog.RestoreDirectory = true; //сохранение директории в которой был пользователь. При повторном открытии она сохранится

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                image.Source = new BitmapImage(new Uri(selectedImagePath));
                return true;
            }
            return false;
        }
    }
}
