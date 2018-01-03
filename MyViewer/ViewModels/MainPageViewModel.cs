﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.ViewManagement;

namespace MyViewer.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public Views.MainPage View { get; private set; } = null;
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

        public void Initialize(Views.MainPage mainPage)
        {
            View = mainPage;
            SlideShow = new SimpleCommand(SlideShowMethod);
            Search = new SimpleCommand(SearchMethod);
            CreateIndex = new SimpleCommand(CreateIndexMethod);
        }

        private void SlideShowMethod()
        {
            var view = ApplicationView.GetForCurrentView();
            view.TryEnterFullScreenMode();
            LogText = "aaaa:" + SearchText;
        }
        private void SearchMethod()
        {
            var view = ApplicationView.GetForCurrentView();
            view.ExitFullScreenMode();
            LogText = "bbbb:" + SearchText;
        }
        private void CreateIndexMethod()
        {
            LogText = "cccc:" + SearchText;
        }

    }
}