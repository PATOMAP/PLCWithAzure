using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace ClientOpcUaTiaPortal.item
{
    public class itemDB : INotifyPropertyChanged
    {
        public List<itemDB> _item { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }
        public string type1 { get; set; }
        private string _value { get; set; }

        public bool arrayAct { get; set; }
        public bool structAct { get; set; }

        public char _readWrite{ get; set; }

        private Brush _backgroundColor;
        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                if (_backgroundColor != value)
                {
                    _backgroundColor = value;
                    OnPropertyChanged(nameof(BackgroundColor));
                }
            }
        }


        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public itemDB()
        {
            _item = new List<itemDB>();
            BackgroundColor = new SolidColorBrush(Colors.AliceBlue);
            _readWrite = 'r';
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}



