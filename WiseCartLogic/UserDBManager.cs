using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace WiseCartLogic
{
    public class UserDBManager : IDBManager
    {
        public string UsersXmlPath { get; set; }
        public UsersDB UsersDB { get; set; }

        public UserDBManager()
        {
            UsersXmlPath = @"..\..\..\Resources\UsersData\UsersData.xml";
            UsersDB = new UsersDB();
        }

        public void SaveDB()
        {
            File.Delete(UsersXmlPath);

            XmlSerializer serializer = new XmlSerializer(typeof(UsersDB));

            using (StreamWriter file = new StreamWriter(UsersXmlPath))
            {
                serializer.Serialize(file, UsersDB);
            }
        }

        public void ReadDB()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(UsersDB));

            if (File.Exists(UsersXmlPath))
            {
                using (TextReader reader = new StreamReader(UsersXmlPath))
                {
                    UsersDB = (UsersDB) deserializer.Deserialize(reader);
                }
            }
            else
            {
                Debug.WriteLine("File missing");
            }
        }

        public void UpdateDB()
        {
            throw new NotImplementedException();
        }
    }
}