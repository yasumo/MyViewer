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
        public void Initialize(ThumbnailWindow thumbnailWindow)
        {

            this.PersonList = new List<string>()
            {
            "佐藤","高橋","渡辺","山本","小林","小林","小林","小林","小林","小林","小林","小林","小林","小林","小林","小林","小林","小林","小林","小林"
            };

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
