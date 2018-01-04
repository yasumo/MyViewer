using MyViewerLib;
using SlideShow.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SlideShow.ViewModels
{
    public class ThumbnailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        List<Folder> FolderList;

        private ListCollectionView _PersonListView;
        public ListCollectionView PersonListView
        {
            get
            { return _PersonListView; }
            set
            {
                if (_PersonListView == value)
                    return;
                _PersonListView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PersonListView)));
            }
        }


        private List<string> PersonList;
        public void Initialize(ThumbnailWindow thumbnailWindow,List<Folder> folderList)
        {
            FolderList = folderList;

            var fo = new FolderOperator();
            var imgFilePathList = fo.GetAllFilePathList(fo.GetFolderPathList(FolderList), SlideShowConst.PIC_EXT_LIST);

            this.PersonList = new List<string>(imgFilePathList);

            this.PersonListView = new ListCollectionView(this.PersonList);
            this.PersonListView.CurrentChanged += PersonListView_CurrentChanged;
        }

        void PersonListView_CurrentChanged(object sender, EventArgs e)
        {
            var lv = sender as ICollectionView;
            if (lv.CurrentPosition < 0)
            {
                System.Diagnostics.Trace.WriteLine("選択無し");
                return;
            }
            var name = lv.CurrentItem as string;
            System.Diagnostics.Trace.WriteLine(
                string.Format("CurrentChanged:位置={0},名前={1}",
                lv.CurrentPosition,
                name));
        }

    }
}
