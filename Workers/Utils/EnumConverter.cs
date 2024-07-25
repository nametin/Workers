using System.Globalization;
using System.Windows.Data;
using Workers.Models;

namespace Workers.Utils
{
    public class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Position position)
            {
                if (position == Position.SoftwareEngineer)
                {
                    return "Yazılım Mühendisi";
                }
                else if (position == Position.TestEngineer)
                {
                    return "Test Mühendisi";
                }
                else if (position == Position.SeniorSoftwareEngineer)
                {
                    return "Kıdemli Yazılım Mühendisi";
                }
                else if (position == Position.SeniorTestEngineer)
                {
                    return "Kıdemli Test Mühendisi";
                }
                else if (position == Position.LeadEngineer)
                {
                    return "Lider Mühendis";
                }
                else
                {
                    return "Kıdemli Lider Mühendis";
                }
            }

            if (value is Status status)
            {
                if (status == Status.Hiring)
                {
                    return "İşe Alım";
                }
                else 
                {
                    return "İşten Çıkarma";
                }
            }
            return "";
        }

        public static List<Position> GetPositionList()
        {
            return new List<Position>((Position[])Enum.GetValues(typeof(Position)));
        }

        public static List<Status> GetStatusList()
        {
            return new List<Status>((Status[])Enum.GetValues(typeof(Status)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
