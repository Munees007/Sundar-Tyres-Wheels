using MySql.Data.MySqlClient;
using Sundar_Tyres___Wheels.Database;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddVehicle.xaml
    /// </summary>
    public partial class AddVehicle : Page
    {
        public AddVehicle()
        {
            InitializeComponent();
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string vehicleNo = txtVehicleNo.Text.Trim();
            string description = txtDescription.Text.Trim();
            string driver = txtDriver.Text.Trim();
            bool isInactive = chkIsInactive.IsChecked ?? false;

            string modelCode = txtModelCode.Text.Trim();
            string vehicleMake = txtVehicleMake.Text.Trim();
            string vehicleModel = txtVehicleModel.Text.Trim();
            string mfgYearText = txtMfgYear.Text.Trim();
            string modelName = txtModelName.Text.Trim();

            string customerContact = txtCustomerContact.Text.Trim(); // Get contact from TextBox

            if (string.IsNullOrWhiteSpace(vehicleNo) || string.IsNullOrWhiteSpace(modelCode))
            {
                MessageBox.Show("Vehicle No and Model Code are required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(mfgYearText, out int mfgYear))
            {
                MessageBox.Show("Manufacturing Year must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(customerContact))
            {
                MessageBox.Show("Customer Contact is required. Please enter a valid contact.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DatabaseHandler.Instance.openConnection();

                // Check if the customer exists
                string checkCustomerQuery = "SELECT COUNT(*) FROM Customers WHERE ContactNumber = @CustomerContact";
                using (MySqlCommand checkCmd = new MySqlCommand(checkCustomerQuery, DatabaseHandler.Instance.Connection))
                {
                    checkCmd.Parameters.AddWithValue("@CustomerContact", customerContact);
                    int customerCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (customerCount == 0)
                    {
                        MessageBox.Show("Customer not found. Please add the customer first before adding the vehicle.",
                                        "Customer Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // Insert the vehicle if the customer exists
                string query = @"
        INSERT INTO Vehicles (VehicleNo, Description, Driver, IsInactive, ModelCode, VehicleMake, VehicleModel, MfgYear, ModelName, CustomerContact)
        VALUES (@VehicleNo, @Description, @Driver, @IsInactive, @ModelCode, @VehicleMake, @VehicleModel, @MfgYear, @ModelName, @CustomerContact)";

                using (MySqlCommand cmd = new MySqlCommand(query, DatabaseHandler.Instance.Connection))
                {
                    cmd.Parameters.AddWithValue("@VehicleNo", vehicleNo);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Driver", driver);
                    cmd.Parameters.AddWithValue("@IsInactive", isInactive);
                    cmd.Parameters.AddWithValue("@ModelCode", modelCode);
                    cmd.Parameters.AddWithValue("@VehicleMake", vehicleMake);
                    cmd.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                    cmd.Parameters.AddWithValue("@MfgYear", mfgYear);
                    cmd.Parameters.AddWithValue("@ModelName", modelName);
                    cmd.Parameters.AddWithValue("@CustomerContact", customerContact);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vehicle added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding vehicle: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DatabaseHandler.Instance.CloseConnection();
            }
        }


        private void ClearForm()
        {
            txtVehicleNo.Clear();
            txtDescription.Clear();
            txtDriver.Clear();
            chkIsInactive.IsChecked = false;

            txtModelCode.Clear();
            txtVehicleMake.Clear();
            txtVehicleModel.Clear();
            txtMfgYear.Clear();
            txtModelName.Clear();

            txtCustomerContact.Clear(); // Clear contact field
        }


    }
}
