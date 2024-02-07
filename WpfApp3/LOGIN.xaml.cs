using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for LOGIN.xaml
    /// </summary>
    public partial class LOGIN : Window
    {
        public static string FirstLastName{get;set;}

        SqlConnection sqlConnection;
        public LOGIN()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WpfApp3.Properties.Settings.testConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "SELECT * FROM User_";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable storeTable = new DataTable();
                    sqlDataAdapter.Fill(storeTable);


                    string columnNameToCheck = "UserName";
                    string valueToCheck = UserName.Text;
                    string Pass = password.Password;
                    
                    int i = 0;
                    foreach (DataRow row in storeTable.Rows)
                    {  
                        if (row[columnNameToCheck].ToString() == valueToCheck & row["Password"].ToString() == Pass)
                        {
                            i = 1;
                            MessageBox.Show("Login successful! Welcome "+UserName.Text);
                            FirstLastName = UserName.Text;
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Close();
                        }

                    }
                    if (i == 0) { MessageBox.Show("Incorrect Credentials"); }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }

        }

        private void Account_Creation(object sender, RoutedEventArgs e)
        {
            AccountCreation  AccountCreationWindow= new AccountCreation();
            AccountCreationWindow.Show();
            this.Close();

        }
    }
}
