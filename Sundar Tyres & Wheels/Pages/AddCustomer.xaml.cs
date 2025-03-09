using MySql.Data.MySqlClient;
using Sundar_Tyres___Wheels.Database;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Sundar_Tyres___Wheels.Pages
{
    public partial class AddCustomer : Page
    {
        TextBox name, contact, area, city, mobile1, mobile2, website, email, pincode;
        RichTextBox address;
        ComboBox district, state, country, nationality;
        CheckBox SmsAllowed, EmailAllowed, WhatsappAllowed;

        public AddCustomer()
        {
            InitializeComponent();

            // Initializing UI elements
            name = Name;
            contact = Contact;
            area = Area;
            city = City;
            mobile1 = MobileNo1;
            mobile2 = MobileNo2;
            website = Website;
            email = Email;
            pincode = PinCode;
            address = Address;
            district = District;
            state = State;
            country = Country;
            nationality = Nationality;
            SmsAllowed = isSmsAllowed;
            EmailAllowed = isEmailAllowed;
            WhatsappAllowed = isWhatsappAllowed;
        }

        public void Submit(object sender, RoutedEventArgs e)
        {
            try
            {
                DatabaseHandler.Instance.openConnection(); // Open database connection

                string contactNumber = contact.Text.Trim();
                string fullName = name.Text.Trim();
                string userArea = area.Text.Trim();
                string userCity = city.Text.Trim();
                string userDistrict = (district.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
                string userState = (state.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tamil Nadu";
                string userCountry = (country.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "India";
                string userNationality = (nationality.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "India";
                string userPincode = pincode.Text.Trim();
                string userMobile1 = mobile1.Text.Trim();
                string userMobile2 = mobile2.Text.Trim();
                string userEmail = email.Text.Trim();
                string userWebsite = website.Text.Trim();
                string createdBy = Properties.Settings.Default.Username; // Get logged-in user

                // Convert RichTextBox Address to string
                TextRange textRange = new TextRange(address.Document.ContentStart, address.Document.ContentEnd);
                string userAddress = textRange.Text.Trim();

                // Convert CheckBox values to bool
                bool isSmsAllowed = SmsAllowed.IsChecked ?? false;
                bool isEmailAllowed = EmailAllowed.IsChecked ?? false;
                bool isWhatsappAllowed = WhatsappAllowed.IsChecked ?? false;

                // ❌ Check if contact already exists
                string checkQuery = "SELECT COUNT(*) FROM Customers WHERE Contact = @contact";
                using (MySqlCommand checkCmd = new(checkQuery, DatabaseHandler.Instance.Connection))
                {
                    checkCmd.Parameters.AddWithValue("@contact", contactNumber);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Contact number already exists!", "Duplicate Entry", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                // ✅ Insert new customer
                string insertQuery = @"INSERT INTO Customers 
                    (Contact, Name, Address, Area, City, District, State, Country, PinCode, Nationality, 
                    MobileNo1, MobileNo2, Email, Website, IsSMSAllowed, IsEmailAllowed, IsWhatsAppAllowed, 
                    CreatedBy) 
                    VALUES 
                    (@contact, @name, @address, @area, @city, @district, @state, @country, @pincode, @nationality, 
                    @mobile1, @mobile2, @email, @website, @isSmsAllowed, @isEmailAllowed, @isWhatsappAllowed, 
                    @createdBy)";

                using (MySqlCommand cmd = new(insertQuery, DatabaseHandler.Instance.Connection))
                {
                    cmd.Parameters.AddWithValue("@contact", contactNumber);
                    cmd.Parameters.AddWithValue("@name", fullName);
                    cmd.Parameters.AddWithValue("@address", userAddress);
                    cmd.Parameters.AddWithValue("@area", userArea);
                    cmd.Parameters.AddWithValue("@city", userCity);
                    cmd.Parameters.AddWithValue("@district", userDistrict);
                    cmd.Parameters.AddWithValue("@state", userState);
                    cmd.Parameters.AddWithValue("@country", userCountry);
                    cmd.Parameters.AddWithValue("@pincode", userPincode);
                    cmd.Parameters.AddWithValue("@nationality", userNationality);
                    cmd.Parameters.AddWithValue("@mobile1", userMobile1);
                    cmd.Parameters.AddWithValue("@mobile2", userMobile2);
                    cmd.Parameters.AddWithValue("@email", userEmail);
                    cmd.Parameters.AddWithValue("@website", userWebsite);
                    cmd.Parameters.AddWithValue("@isSmsAllowed", isSmsAllowed);
                    cmd.Parameters.AddWithValue("@isEmailAllowed", isEmailAllowed);
                    cmd.Parameters.AddWithValue("@isWhatsappAllowed", isWhatsappAllowed);
                    cmd.Parameters.AddWithValue("@createdBy", createdBy);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add customer!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DatabaseHandler.Instance.CloseConnection(); // Close connection
            }
        }
    }
}
