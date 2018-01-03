using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyViewer.ViewModels
{
    public class MainPageViewModel : Helpers.Observable
    {
        public Views.MainPage View { get; private set; } = null;

        public void Initialize(Views.MainPage mainPage)
        {
            View = mainPage;
        }
    }
}