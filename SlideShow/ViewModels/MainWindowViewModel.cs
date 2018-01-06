using MyViewerLib;
using SlideShow.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SlideShow.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        List<Folder> SearchedFolderList = new List<Folder>();
        public MainWindow View { get; private set; } = null;
        public event PropertyChangedEventHandler PropertyChanged;
        string searchText;
        public string SearchText
        {
            get
            {
                return searchText;
            }
            set
            {
                searchText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SearchText)));
            }
        }
        string logText;
        public string LogText
        {
            get
            {
                return logText;
            }
            set
            {
                logText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogText)));
            }
        }

        public ICommand SlideShow { get; private set; }
        public ICommand Thumbnail { get; private set; }
        public ICommand FolderSearch { get; private set; }
        public ICommand TagRelationSearch { get; private set; }
        public ICommand TagSearch { get; private set; }
        public ICommand CreateIndex { get; private set; }

        public void Initialize(MainWindow mainWindow)
        {
            View = mainWindow;
            SlideShow = new SimpleCommand(SlideShowMethod);
            Thumbnail = new SimpleCommand(ThumbnailMethod);
            FolderSearch = new SimpleCommand(FolderSearchMethod);
            TagRelationSearch = new SimpleCommand(TagRelationSearchMethod);
            TagSearch = new SimpleCommand(TagSearchMethod);
            CreateIndex = new SimpleCommand(CreateIndexMethod);
            SearchText = "";
            LogText = "";
        }

        private void SlideShowMethod()
        {
            var win = new SlideShowWindow(SearchedFolderList);
            win.ShowDialog();
        }

        private void ThumbnailMethod()
        {
            var win = new ThumbnailWindow(SearchedFolderList);
            win.ShowDialog();
        }

        private void FolderSearchMethod()
        {
            using (var dao = new Dao(Settings.GetSqliteFilePath()))
            {
                LogText = "";
                if (SearchText.Trim().Length == 0)
                {
                    //空の時はタグ名全部返す
                    foreach (var tagAndNum in dao.GetAllTagAndNum())
                    {
                        LogText += tagAndNum.TagName + ":" + tagAndNum.TagNum + Environment.NewLine;
                    }
                }
                else
                {
                    //値があるときはディレクトリを返す
                    string[] delimiter = { "," };

                    var tags = SearchText.Split(delimiter,StringSplitOptions.None);
                    var tagList = new List<string>(tags.Distinct());
                    SearchedFolderList = dao.GetFolderIdListHaving(tagList);
                    var imgnum = 0L;
                    var tmpText = "";
                    //foreach (var tag in dao.GetOtherTag(tagList))
                    //foreach (var tag in dao.GetRelationTagList(tagList))
                    foreach (var folder in SearchedFolderList)
                    {
                        imgnum += folder.InsideFileNum.GetValueOrDefault(0L);
                        tmpText += folder.InsideFileNum + ":" + folder.FolderPath + Environment.NewLine;
                        //tmpText += tag.tagNum + ":" + tag.tagName + Environment.NewLine;
                    }
                    tmpText = imgnum.ToString() + Environment.NewLine + tmpText;
                    LogText = tmpText;
                }
            }
        }

        private void TagRelationSearchMethod()
        {
            using (var dao = new Dao(Settings.GetSqliteFilePath()))
            {
                LogText = "";
                if (SearchText.Trim().Length == 0)
                {
                    //空の時はタグ名全部返す
                    foreach (var tagAndNum in dao.GetAllTagAndNum())
                    {
                        LogText += tagAndNum.TagName + ":" + tagAndNum.TagNum + Environment.NewLine;
                    }
                }
                else
                {
                    //値があるときはディレクトリを返す
                    string[] delimiter = { "," };

                    var tags = SearchText.Split(delimiter, StringSplitOptions.None);
                    var tagList = new List<string>(tags.Distinct());
                    SearchedFolderList = dao.GetFolderIdListHaving(tagList);
                    var tmpText = "";
                    //foreach (var tag in dao.GetOtherTag(tagList))
                    foreach (var tag in dao.GetRelationTagList(tagList))
                    {
                        tmpText += tag.tagNum + "\t\t" + tag.tagName + Environment.NewLine;
                    }
                    LogText = tmpText;
                }
            }
        }

        private void TagSearchMethod()
        {
            using (var dao = new Dao(Settings.GetSqliteFilePath()))
            {
                LogText = "";
                if (SearchText.Trim().Length == 0)
                {
                    //空の時はタグ名全部返す
                    foreach (var tagAndNum in dao.GetAllTagAndNum())
                    {
                        LogText += tagAndNum.TagName + ":" + tagAndNum.TagNum + Environment.NewLine;
                    }
                }
                else
                {
                    //値があるときはディレクトリを返す
                    string[] delimiter = { "," };

                    var tags = SearchText.Split(delimiter, StringSplitOptions.None);
                    var tagList = new List<string>(tags.Distinct());
                    SearchedFolderList = dao.GetFolderIdListHaving(tagList);
                    var tmpText = "";
                    foreach (var tag in dao.GetAllTag().Where(x=>tagList.Where(y=>x.TagName.Contains(y)).Count()>0))
                    {
                        tmpText += tag.TagName + Environment.NewLine;
                    }
                    LogText = tmpText;
                }
            }
        }

        private void CreateIndexMethod()
        {
            Console.WriteLine("create start");
            IndexUtils.Create();
            Console.WriteLine("create end");
        }

    }
}