using MyViewerLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace SlideShow.ViewModels
{
    public class SlideShowWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<Folder> FolderList;
        public SlideShowWindow View { get; private set; } = null;

        Bitmap iconImageSource;
        public Bitmap IconImageSource
        {
            get
            {
                return iconImageSource;
            }
            set
            {
                iconImageSource = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IconImageSource)));
            }
        }

        public void Initialize(SlideShowWindow slideShowWindow,List<Folder>folderList)
        {
            FolderList = folderList;
            View = slideShowWindow;
            View.WindowState = System.Windows.WindowState.Maximized;
            View.WindowStyle = System.Windows.WindowStyle.None;
            Bitmap img = (Bitmap)Image.FromFile(@"C:\test.jpg");
            IconImageSource = img;

        }

    }
    
}
