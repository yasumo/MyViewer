using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MyViewerLib;

namespace SlideShow.Models
{
    public class ImageSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string filePath = value as string;
            if (filePath == null)
            {
                return DependencyProperty.UnsetValue;
            }
            var picDir = Settings.GetPicDir();
            var thumbnailDir = Settings.GetThumbDir();
            var io = new ImageOperator(picDir, thumbnailDir);
            var thumbnail = io.GetThumbnail(filePath);

            var absPath = System.IO.Path.GetFullPath(thumbnail);
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(absPath);
            //image.DecodePixelHeight = 100;
            //image.DecodePixelWidth = 100;
            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
