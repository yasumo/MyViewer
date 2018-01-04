using MyViewerLib;
using SlideShow.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SlideShow.ViewModels
{
    public class ThumbnailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<Folder> FolderList;

        private ListCollectionView imagePathListBox;
        public ListCollectionView ImagePathListBox
        {
            get
            { return imagePathListBox; }
            set
            {
                if (imagePathListBox == value)
                    return;
                imagePathListBox = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePathListBox)));
            }
        }


        private List<string> ImagePathList;
        public void Initialize(ThumbnailWindow thumbnailWindow,List<Folder> folderList)
        {
            FolderList = folderList;

            var fo = new FolderOperator();
            var imgFilePathList = fo.GetAllFilePathList(fo.GetFolderPathList(FolderList), SlideShowConst.PIC_EXT_LIST);

            this.ImagePathList = new List<string>(imgFilePathList);
            //ObservableCollection
            this.ImagePathListBox = new ListCollectionView(new ObservableCollection<object>(this.ImagePathList));
            this.ImagePathListBox.CurrentChanged += ImagePathListBox_CurrentChanged;
        }

        void ImagePathListBox_CurrentChanged(object sender, EventArgs e)
        {
            var lv = sender as ICollectionView;
            if (lv.CurrentPosition < 0)
            {
                System.Diagnostics.Trace.WriteLine("選択無し");
                return;
            }

            //クリップボードに画像をコピー
            var imgpath = lv.CurrentItem as string;

            using (var memoryStream = new MemoryStream())
            {
                //画像を作成する
                Bitmap bitmap = new Bitmap(imgpath);
                // You need to specify the image format to fill the stream. 
                // I'm assuming it is PNG
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                //画像データをクリップボードにコピーする
                Clipboard.SetImage(CreateBitmapSourceFromBitmap(memoryStream));

                //後片付け
                bitmap.Dispose();

            }

        }

        private static BitmapSource CreateBitmapSourceFromBitmap(Stream stream)
        {
            var bitmapDecoder = BitmapDecoder.Create(
                stream,
                BitmapCreateOptions.PreservePixelFormat,
                BitmapCacheOption.OnLoad);

            // This will disconnect the stream from the image completely...
            var writable = new WriteableBitmap(bitmapDecoder.Frames.Single());
            writable.Freeze();

            return writable;
        }
    }
}
