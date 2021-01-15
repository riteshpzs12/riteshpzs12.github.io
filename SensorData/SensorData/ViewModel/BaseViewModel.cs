using System;
using System.ComponentModel;
using SensorData.ContainerHelper;

namespace SensorData.ViewModel
{
    public abstract class BaseViewModel : IBaseViewModel, INotifyPropertyChanged
    {
        protected BaseViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }
    }
}
