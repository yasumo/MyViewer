﻿using System;
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
            LogText = "cccc:" + SearchText;
        }

    }
}