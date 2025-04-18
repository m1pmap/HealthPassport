﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HealthPassport.Interfaces
{
    public interface IImageUpdater
    {
        bool UploadImageFromFile(Image image); //Загрузка картинки из файла
        bool DownloadImageToFile(Image image); //Сохранение картинки в файл
        void DeleteImage(Image image); //Удаление картинки
        bool UploadImageFromClipboard(Image image); //Загрузка картинки из буфера обмена
        void DownloadImageToClipboard(Image image); //Сохранение картинки в буфер обмена
    }
}
