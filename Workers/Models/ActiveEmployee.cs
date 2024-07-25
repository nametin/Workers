
using System.ComponentModel;

namespace Workers.Models
{
    public class ActiveEmployee : INotifyPropertyChanged
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private Position _position;
        private int _experience;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public Position Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public int Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                OnPropertyChanged(nameof(Experience));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
