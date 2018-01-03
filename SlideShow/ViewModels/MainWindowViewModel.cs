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
        public ICommand Search { get; private set; }
        public ICommand CreateIndex { get; private set; }

        public void Initialize(MainWindow mainWindow)
        {
            View = mainWindow;
            SlideShow = new SimpleCommand(SlideShowMethod);
            Search = new SimpleCommand(SearchMethod);
            CreateIndex = new SimpleCommand(CreateIndexMethod);
        }

        private void SlideShowMethod()
        {
            //var view = ApplicationView.GetForCurrentView();
            //view.TryEnterFullScreenMode();
            View.WindowState = System.Windows.WindowState.Normal;
            View.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            LogText = "aaaa:" + SearchText;
        }
        private void SearchMethod()
        {
            //var view = ApplicationView.GetForCurrentView();
            //view.ExitFullScreenMode();
            View.WindowState = System.Windows.WindowState.Maximized;
            View.WindowStyle = System.Windows.WindowStyle.None;
            LogText = "bbbb:" + SearchText;
        }
        private void CreateIndexMethod()
        {

            FolderOperator.DeleteOldFiles(Settings.GetThumbDir(), 30);
            using (var dao = new Dao(Settings.GetSqliteFilePath()))
            {
                dao.ClearTmpTable();

                var fo = new FolderOperator();
                int folderNum = 0;
                //画像のベースフォルダのすべてのフォルダに対して
                foreach (var folderPath in fo.GetAllFolderPathList(Settings.GetPicDir()))
                {
                    //画像ファイル拡張子のファイルの数を数えて
                    var filenum = fo.GetFileNum(folderPath, SlideShowConst.PIC_EXT_LIST);

                    //画像ファイル拡張子のファイルの数が1個以上であれば
                    if (filenum > 0)
                    {
                        //フォルダ情報を登録し
                        var folderId = dao.InsertFolderTable(folderPath, filenum);

                        var tagIdList = new List<long>();
                        //そのフォルダのタグを分解して
                        foreach (var tag in fo.GetTagList(folderPath))
                        {
                            //タグIDを発行してもらい
                            var tagid = dao.SearchOrInsertTagTable(tag);
                            tagIdList.Add(tagid);
                        }
                        //登録する
                        if (tagIdList.Count > 0)
                        {
                            var folderTagIdList = dao.InsertMulutiFolderTagTable(folderId,tagIdList);
                            Console.WriteLine(@"{0}:{1}:{2}:{3}", folderNum, filenum, folderTagIdList.Count, folderPath);
                        }

                    }

                    folderNum++;

                }
            }
        }

    }
}