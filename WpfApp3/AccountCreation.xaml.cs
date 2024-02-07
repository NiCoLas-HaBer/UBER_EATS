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
    /// Interaction logic for AccountCreation.xaml
    /// </summary>
    public partial class AccountCreation : Window
    {
        SqlConnection sqlConnection;
        public AccountCreation()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WpfApp3.Properties.Settings.testConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

        }

        private void CreateMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "SELECT * FROM BankCard";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable storeTable = new DataTable();
                    sqlDataAdapter.Fill(storeTable);

                    string cardnoValueToCheck = Card_Number.Text;
                    string pinValueToCheck = Pin.Text;

                    string Pass = Password.Password;
                    string PassConf = Password_Confirmation.Password;

                    int i = 0;
                    foreach (DataRow row in storeTable.Rows)
                    {
                        if ((row["CardNo"].ToString() == cardnoValueToCheck & row["Pin"].ToString() == pinValueToCheck) & Pass ==PassConf)
                        { 
                            i = 1;
                            List<SqlParameter> parameters = new List<SqlParameter>()
                            {
                                new SqlParameter("@UserName",SqlDbType.NVarChar){Value = UserName.Text},
                                new SqlParameter("@Password",SqlDbType.NVarChar){Value = Password.Password},
                                new SqlParameter("@Address",SqlDbType.NVarChar){Value = Adress.Text},
                                new SqlParameter("@CardNo",SqlDbType.NVarChar){Value = Card_Number.Text},
                                new SqlParameter("@Pin",SqlDbType.NVarChar){Value = Pin.Text},
                                new SqlParameter("@email",SqlDbType.NVarChar){Value = e_mail.Text},
                                new SqlParameter("@Card_Id",SqlDbType.NVarChar){Value = row["Id"]}

                            };

                            query = "INSERT INTO User_ VALUES(@UserName,@Password,@Address,@CardNo,@Pin,@email,@Card_Id)";
                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                            sqlConnection.Open();
                            sqlCommand.Parameters.AddRange(parameters.ToArray());
                            DataTable storetable = new DataTable();
                            using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand)) adapter.Fill(storetable);

                            MessageBox.Show("The account was created");


                        }

                    }
                    if (i == 0) { MessageBox.Show("Failed"); }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message);
            }
            finally
            {
                sqlConnection.Close();

            }

        }

        private void BackMethod(object sender, RoutedEventArgs e)
        {
            LOGIN lOGIN = new LOGIN();
            lOGIN.Show();
            this.Close();
            
            
        }

        private void HelpMethod(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bank Card No:0000000000\nPin:000");
        }
    }
}
