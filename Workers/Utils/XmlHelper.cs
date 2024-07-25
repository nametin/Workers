
using System.IO;
using System.Xml.Serialization;
using Workers.Models;

namespace Workers.Utils
{
    public class XmlHelper
    {

        public static void SaveToXml(List<ActiveEmployee> employees, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ActiveEmployee>));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, employees);
            }
        }

        public static List<ActiveEmployee> LoadFromXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ActiveEmployee>));
            using (TextReader reader = new StreamReader(filePath))
            {
                return (List<ActiveEmployee>)serializer.Deserialize(reader);
            }
        }
    }
}
