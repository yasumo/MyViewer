using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// https://github.com/Microsoft/WindowsTemplateStudio/blob/master/templates/_composition/MVVMBasic/Project/Helpers/Observable.cs から引用
// 名前空間を修正
namespace MyViewer.Helpers
{
    public class Observable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}