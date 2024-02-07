using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    public class OrderItem
    {
        public string MenuName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => Price * Quantity;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        private ObservableCollection<OrderItem> orderItems;
        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WpfApp3.Properties.Settings.testConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            NAME.Text = LOGIN.FirstLastName;
            orderItems = new ObservableCollection<OrderItem>();
            dataGrid.ItemsSource = orderItems; // Assuming your DataGrid is named dataGrid
            displayRestaurants();
        }
        private void displayRestaurants()
        {
            try
            {
                string query = "SELECT * FROM Restaurants";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable RestoTable = new DataTable();
                    sqlDataAdapter.Fill(RestoTable);
                    RestoList.DisplayMemberPath = "Restaurant_Name";
                    RestoList.SelectedValuePath = "Id";
                    RestoList.ItemsSource = RestoTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        //private void DisplayMenu()
        //{
        //    try
        //    {
        //        string query = "SELECT * FROM Menu m INNER JOIN Restaurants r ON m.RestId = r.Id WHERE RestId = @Id";
        //        SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
        //        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
        //        using (sqlDataAdapter)
        //        {
        //            sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = RestoList.SelectedValue;

        //            DataTable MenuTable = new DataTable();
        //            sqlDataAdapter.Fill(MenuTable);
        //            INGREDIENTS.ItemsSource = null;

        //            MENU.DisplayMemberPath = "Name";
        //            MENU.SelectedValuePath = "Id";
        //            MENU.ItemsSource = MenuTable.DefaultView;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        private void DisplayMenu()
        {
            try
            {
                string query = "SELECT m.Id, m.Name, m.Price, m.Calories FROM Menu m INNER JOIN Restaurants r ON m.RestId = r.Id WHERE RestId = @Id";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = RestoList.SelectedValue;

                    DataTable MenuTable = new DataTable();
                    sqlDataAdapter.Fill(MenuTable);
                    INGREDIENTS.ItemsSource = null;

                    MENU.DisplayMemberPath = "Name";
                    MENU.SelectedValuePath = "Id";
                    MENU.ItemsSource = MenuTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void RestoListSelecitonCHange(object sender, SelectionChangedEventArgs e)
        {
            DisplayMenu();
        }

        private void IngredientsDetails(object sender, SelectionChangedEventArgs e)
        {
            DisplayIngredientsDetails();

        }


        public void DisplayIngredientsDetails()
        {
            try
            {
                if (MENU.SelectedValue != null)
                {
                    string query = "SELECT * FROM Ingredients I INNER JOIN HasAnIngredient H ON I.Id = H.Ingredient_Id WHERE H.Menu_Id = @Id";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    using (sqlDataAdapter)
                    {
                        sqlCommand.Parameters.Add("@Id", SqlDbType.Int).Value = MENU.SelectedValue;

                        DataTable IngredientsTable = new DataTable();
                        sqlDataAdapter.Fill(IngredientsTable);

                        // Clear the items before populating
                        INGREDIENTS.ItemsSource = null;

                        INGREDIENTS.DisplayMemberPath = "Name";
                        INGREDIENTS.SelectedValuePath = "Id";
                        INGREDIENTS.ItemsSource = IngredientsTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddToOrder(string menuName, decimal price, int quantity)
        {
            OrderItem newItem = new OrderItem
            {
                MenuName = menuName,
                Price = price,
                Quantity = quantity
            };

            orderItems.Add(newItem);
        }

        private void AddToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Get selected menu item details (adjust this part based on your actual implementation)
            string selectedMenuName = MENU.SelectedItem != null ? (MENU.SelectedItem as DataRowView)["Name"].ToString() : "";

            if (MENU.SelectedItem != null && decimal.TryParse((MENU.SelectedItem as DataRowView)["Price"].ToString(), out decimal selectedPrice)) { }
            else
            {
                selectedPrice = 0;
            }
            //decimal selectedPrice = decimal.TryParse(MENU.SelectedItem != null ? (MENU.SelectedItem as DataRowView)["Price"]:"0"); // Replace with the actual price from your data
            int selectedQuantity = 1; // You can adjust the quantity based on user input

            // Add the selected menu item to the order
            AddToOrder(selectedMenuName, selectedPrice, selectedQuantity);
        }
    }
}

