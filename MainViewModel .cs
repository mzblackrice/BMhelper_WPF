using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMhelper_WPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Actor> _actors;
        public ObservableCollection<Actor> Actors
        {
            get { return _actors; }
            set
            {
                _actors = value;
                OnPropertyChanged(nameof(Actors));
            }
        }

        public MainViewModel()
        {
            Actors = new ObservableCollection<Actor>();

            // 在这里添加Actor对象到Actors集合中
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
