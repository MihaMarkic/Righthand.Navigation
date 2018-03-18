using System;
using System.ComponentModel;

namespace Righthand.Navigation.Sample.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            OnDispose(isDisposing: true);
        }
        protected virtual void OnDispose(bool isDisposing)
        {}

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e) => PropertyChanged?.Invoke(this, e);
    }
}
