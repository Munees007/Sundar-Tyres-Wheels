using MySql.Data.MySqlClient;
using Sundar_Tyres___Wheels.Database;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Sundar_Tyres___Wheels.Pages
{
    /// <summary>
    /// Interaction logic for ViewVehicles.xaml
    /// </summary>
    public partial class ViewVehicles : Page
    {
        public ViewVehicles()
        {
            InitializeComponent();
            LoadVehicles();
        }

        public void loadVehicle_Click(object sender, RoutedEventArgs e)
        {
            LoadVehicles();
        }
        private void LoadVehicles()
        {
            try
            {
                DatabaseHandler.Instance.openConnection();
                string query = "SELECT VehicleNo, Description, Driver, IsInactive, ModelCode, VehicleMake, VehicleModel, MfgYear, ModelName, CustomerContact FROM Vehicles";
                MySqlCommand cmd = new MySqlCommand(query, DatabaseHandler.Instance.Connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgVehicles.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vehicles: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DatabaseHandler.Instance.CloseConnection();
            }
        }

        private void SearchVehicle_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                LoadVehicles(); // Reload all if search is empty
                return;
            }

            try
            {
                DatabaseHandler.Instance.openConnection();
                string query = "SELECT * FROM Vehicles WHERE VehicleNo LIKE @Search OR CustomerContact LIKE @Search";
                MySqlCommand cmd = new MySqlCommand(query, DatabaseHandler.Instance.Connection);
                cmd.Parameters.AddWithValue("@Search", "%" + searchQuery + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgVehicles.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching vehicles: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DatabaseHandler.Instance.CloseConnection();
            }
        }
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "")
                txtPlaceholder.Visibility = Visibility.Collapsed;
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                txtPlaceholder.Visibility = Visibility.Visible;
        }

    }
}
