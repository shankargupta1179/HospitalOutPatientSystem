using System;
using System.Xml;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace HospitalOutPatientSystem.modules
{
    public class ConnectDb
    {
        private XmlElement? root { get; set; }
        public MySqlConnection EstablishConnection()
        {
            string server = "localhost";
            string database = "hospital_management_system";
            string username = "root";
            string password = "shankar";
            string constring = $"SERVER={server};DATABASE={database};UID={username};PASSWORD={password};";
            MySqlConnection conn = new MySqlConnection(constring);
            conn.Open();
            return conn;
        }

        public string GetQuery(string queryName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\admin\\source\\repos\\HospitalOutPatientSystem\\HospitalOutPatientSystem\\utils\\DbOperations.xml");
            root = doc.DocumentElement!;
            XmlNodeList? elementList = root.GetElementsByTagName(queryName);
            if (elementList.Count==0) return "Invalid query, Try again";
            return elementList[0]!.InnerXml;
        }

        public static int GetCountOfPatients()
        {
            int count = 0;
            ConnectDb connectDb = new ConnectDb();
            MySqlConnection conn = connectDb.EstablishConnection();
            string query = connectDb.GetQuery("GetAllRecords");
            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                count++;
            }
            conn.Close();
            
            return count;   
        }
    }
}
