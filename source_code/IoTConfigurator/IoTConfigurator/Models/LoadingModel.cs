using System;
using System.ComponentModel;

namespace IoTConfigurator.Models
{
	public class LoadingModel : INotifyPropertyChanged
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
        
        public bool IsNotLoading
        {
            get { return !IsLoading; }
        }

        public LoadingModel(bool isLocationOn)
        {
            IsLoading = isLocationOn;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

