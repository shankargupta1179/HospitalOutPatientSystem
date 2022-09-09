using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
namespace HospitalOutPatientSystem.modules
{
    public class Patient
    {

        private int patientId = 0;
        private string? patientName = "";
        private int patientAge;
        private string? symptoms;
        private int severity;
        private string? feedback = "";
        private string? prescription = "";
        PriorityQueue<string, int> appointment = new PriorityQueue<string, int>();
        public Patient()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("**********Welcome to Hospital Management System**********");
            MenuPopUp();
        }


        public void MenuPopUp()
        {
            int userChoice;
            Console.WriteLine("1.Add Patient Details");
            Console.WriteLine("2.Call Next Patient");
            Console.WriteLine("3.Write Feedback for Patient");
            Console.WriteLine("4.Write Prescription for Patient");
            Console.WriteLine("5.Search Prescription of Patient");
            Console.WriteLine("6.Generate Patient Records");
            Console.WriteLine("7.Exit");
            userChoice = int.TryParse(Console.ReadLine(), out userChoice) ? userChoice : 5;
            switch (userChoice)
            {
                case 1:
                    AddPatient();
                    MenuPopUp();
                    break;
                case 2:
                    CallNextPatient();
                    MenuPopUp();
                    break;
                case 3:
                    WriteFeedback();
                    MenuPopUp();
                    break;
                case 4:
                    WritePrescription();
                    MenuPopUp();
                    break;
                case 5:
                    SearchForPatientPrescription();
                    MenuPopUp();
                    break;
                case 6:
                    GenerateReportOfPatients();
                    break;
                case 7:
                    break;
                default:
                    Console.WriteLine("\nInvalid Option , Please Enter a Valid Option\n");
                    MenuPopUp();
                    break;
            }

        }
        public void AddPatient()
        {
            patientId = ConnectDb.GetCountOfPatients() + 1;
            Console.WriteLine("\nEnter patient's Name\n");
            patientName = Console.ReadLine();
            Console.WriteLine("\nEnter patient's Age\n");
            patientAge = int.TryParse(Console.ReadLine(), out patientAge) ? patientAge : 0;
            Console.WriteLine("\nEnter the symptoms\n");
            symptoms = Console.ReadLine();
            Console.WriteLine("\nEnter the severity out of 100\n");
            severity = int.TryParse(Console.ReadLine(), out severity) ? severity : 0;
            appointment.Enqueue(patientName!, severity * -1);
            ConnectDb connectDb = new ConnectDb();
            MySqlConnection conn = connectDb.EstablishConnection();
            string query = connectDb.GetQuery("AddPatient");
            query = String.Format(query,patientId,patientName,patientAge,symptoms,severity,feedback,prescription);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void CallNextPatient()
        {
            try
            {
                int count = appointment.Count;
                if (count>0)
                {
                    var nextPatientName = appointment.Dequeue();
                    Console.WriteLine($"\n**********{nextPatientName} will be the next patient . Be Ready**********\n");
                }
                else if (count == 0) throw new Exception("\n**********There are No patients waiting in the queue**********\n");
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void WriteFeedback()
        {
            Console.WriteLine("\nEnter the Name of the person to whom you want to write feedback for\n");
            string? name = Console.ReadLine();
            Console.WriteLine("\nWrite Feedback\n");
            feedback = Console.ReadLine();
            try
            {
                ConnectDb connectDb = new ConnectDb();
                MySqlConnection conn = connectDb.EstablishConnection();
                string query = connectDb.GetQuery("GiveFeedback");
                query = String.Format(query, feedback, name);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                Console.WriteLine("Try Again");
                WriteFeedback();
            }
        }

        public void WritePrescription()
        {
            Console.WriteLine("\nEnter the Name of the person to whom you want to write Prescription for\n");
            string? name = Console.ReadLine();
            Console.WriteLine("\nWrite Prescription\n");
            prescription = Console.ReadLine();
            try
            {
                ConnectDb connectDb = new ConnectDb();
                MySqlConnection conn = connectDb.EstablishConnection();
                string query = connectDb.GetQuery("GivePrescription");
                query = String.Format(query, prescription, name);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch
            {
                Console.WriteLine("\nTry again\n");
                WritePrescription();
            }
        }

        public void SearchForPatientPrescription()
        {
            Console.WriteLine("\nEnter the name of the person whose prescription you wanna find\n");
            string? name = Console.ReadLine();
            try
            {
                ConnectDb connectDb = new ConnectDb();
                MySqlConnection conn = connectDb.EstablishConnection();
                string query = connectDb.GetQuery("SearchPrescription");
                query = String.Format(query,name);
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    string prescription = (string)sdr["patient_prescription"];
                    Console.WriteLine(prescription);
                }
                conn.Close();
            }
            catch
            {
                Console.WriteLine("\nTry again\n");
                SearchForPatientPrescription();
            }
        }

        public void GenerateReportOfPatients()
        {
            int patientsCount = ConnectDb.GetCountOfPatients();
            Console.WriteLine("\nYou have requested for reports of patients\n");
            if (patientsCount == 0)
            {
                Console.WriteLine("\nSorry, There are No records found\n");
                MenuPopUp();
            }
            try
            {
                ConnectDb connectDb = new ConnectDb();
                MySqlConnection conn = connectDb.EstablishConnection();
                string query = connectDb.GetQuery("GetAllRecords");
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader sdr = cmd.ExecuteReader();
                var csv = new StringBuilder();
                string firstLine = "Id,Name,Prescription";
                csv.AppendLine(firstLine);
                while (sdr.Read()) 
                {
                    var newLine = $"{sdr[0]},{sdr[1]},{sdr[6]}";
                    Console.WriteLine(sdr[0] +"," + sdr[1]+"," + sdr[6]);
                    csv.AppendLine(newLine);
                }
                File.WriteAllText("C:\\Users\\admin\\source\\repos\\HospitalOutPatientSystem\\HospitalOutPatientSystem\\storedata\\store.csv", csv.ToString());
                Console.WriteLine();
                conn.Close();
                MenuPopUp();
            }
            catch
            {
                Console.WriteLine("\nTry again\n");
                MenuPopUp();
            }
        }
    }
}
