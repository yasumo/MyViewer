using MyViewerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SlideShow
{
    /// <summary>
    /// ThumbnailWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ThumbnailWindow : Window
    {
        public ViewModels.ThumbnailViewModel ViewModel { get; private set; } = new ViewModels.ThumbnailViewModel();

        public ThumbnailWindow(List<Folder> folderList)
        {
            InitializeComponent();
            ViewModel.Initialize(this, folderList);
            this.DataContext = ViewModel;
        }
    }
}
