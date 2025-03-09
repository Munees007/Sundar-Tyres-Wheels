using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using Sundar_Tyres___Wheels.Database;

namespace Sundar_Tyres___Wheels.Pages
{
    public partial class ViewCustomer : Page
    {
        public ViewCustomer()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            try
            {
                DatabaseHandler.Instance.openConnection();
                string query = "SELECT * FROM Customers";
                using (MySqlCommand cmd = new MySqlCommand(query, DatabaseHandler.Instance.Connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Customer> customers = new List<Customer>();
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                Contact = reader["Contact"].ToString(),
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Area = reader["Area"].ToString(),
                                City = reader["City"].ToString(),
                                District = reader["District"].ToString(),
                                State = reader["State"].ToString(),
                                Country = reader["Country"].ToString(),
                                PinCode = reader["PinCode"].ToString(),
                                Nationality = reader["Nationality"].ToString(),
                                MobileNo1 = reader["MobileNo1"].ToString(),
                                MobileNo2 = reader["MobileNo2"].ToString(),
                                Email = reader["Email"].ToString(),
                                Website = reader["Website"].ToString(),
                                IsSMSAllowed = Convert.ToBoolean(reader["IsSMSAllowed"]),
                                IsEmailAllowed = Convert.ToBoolean(reader["IsEmailAllowed"]),
                                IsWhatsAppAllowed = Convert.ToBoolean(reader["IsWhatsAppAllowed"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                CreatedTime = Convert.ToDateTime(reader["CreatedAt"])
                            });
                        }
                        CustomerListView.ItemsSource = customers;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
            finally
            {
                DatabaseHandler.Instance.CloseConnection();
            }
        }

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerListView.SelectedItem is Customer selectedCustomer)
            {
                ShowCustomerPopup(selectedCustomer);
            }
        }

        private void ShowCustomerPopup(Customer customer)
        {
            string details = $"Name: {customer.Name}\n" +
                             $"Contact: {customer.Contact}\n" +
                             $"Address: {customer.Address}\n" +
                             $"Area: {customer.Area}\n" +
                             $"City: {customer.City}\n" +
                             $"District: {customer.District}\n" +
                             $"State: {customer.State}\n" +
                             $"Country: {customer.Country}\n" +
                             $"Pin Code: {customer.PinCode}\n" +
                             $"Nationality: {customer.Nationality}\n" +
                             $"Mobile 1: {customer.MobileNo1}\n" +
                             $"Mobile 2: {customer.MobileNo2}\n" +
                             $"Email: {customer.Email}\n" +
                             $"Website: {customer.Website}\n" +
                             $"SMS Allowed: {customer.IsSMSAllowed}\n" +
                             $"Email Allowed: {customer.IsEmailAllowed}\n" +
                             $"WhatsApp Allowed: {customer.IsWhatsAppAllowed}\n" +
                             $"Created By: {customer.CreatedBy}\n" +
                             $"Created Time: {customer.CreatedTime}";

            MessageBox.Show(details, "Customer Details", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public class Customer
    {
        public string Contact { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PinCode { get; set; }
        public string Nationality { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool IsSMSAllowed { get; set; }
        public bool IsEmailAllowed { get; set; }
        public bool IsWhatsAppAllowed { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
