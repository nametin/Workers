
using System.ComponentModel;


namespace Workers.Models
{
    public class Employee : INotifyPropertyChanged
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private Position _position;
        private int _experience;
        private Status _status;

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

        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
