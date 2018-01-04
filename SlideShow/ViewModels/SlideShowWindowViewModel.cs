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
using SlideShow.Models;
using System.Windows.Input;
using System.Windows;

namespace SlideShow.ViewModels
{
    public class SlideShowWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<Folder> FolderList;
        public SlideShowWindow View { get; private set; } = null;
        List<string> FilePathList;
        int NowFilePathNum = 0;

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

        public ICommand NextImgCommand { get; private set; }
        public ICommand PrevImgCommand { get; private set; }

        private void NextImgMethod()
        {
            NowFilePathNum++;
            displayImage();
        }
        private void PrevImgMethod()
        {
            NowFilePathNum--;
            displayImage();
        }


        private void displayImage()
        {
            if (FilePathList.Count > 0)
            {
                if (NowFilePathNum <= 0)
                {
                    NowFilePathNum = 0;
                }
                if (NowFilePathNum >= FilePathList.Count)
                {
                    NowFilePathNum = FilePathList.Count - 1;
                }

                Bitmap img = (Bitmap)Image.FromFile(FilePathList[NowFilePathNum]);
                IconImageSource = img;
            }
        }



        public void Initialize(SlideShowWindow slideShowWindow,List<Folder>folderList)
        {
            FolderList = folderList;
            View = slideShowWindow;
            View.WindowState = System.Windows.WindowState.Maximized;
            View.WindowStyle = System.Windows.WindowStyle.None;
            var fo = new FolderOperator();

            NextImgCommand = new SimpleCommand(NextImgMethod);
            PrevImgCommand = new SimpleCommand(PrevImgMethod);

            var imgFilePathList = fo.GetAllFilePathList(GetFolderPathList(FolderList), SlideShowConst.PIC_EXT_LIST);
            FilePathList = new List<string>(imgFilePathList.OrderBy(i => Guid.NewGuid()));
            displayImage();

        }

        private IEnumerable<string> GetFolderPathList(List<Folder> folderList)
        {
            foreach(var folder in folderList)
            {
                yield return folder.FolderPath;
            }

        }

    }
    
}
