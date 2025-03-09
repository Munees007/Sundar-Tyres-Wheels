using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using Sundar_Tyres___Wheels.Database;
using Sundar_Tyres___Wheels.Helper;
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
using System.Windows.Shapes;

namespace Sundar_Tyres___Wheels.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private bool isPasswordVisible = false;
        private TextBox txtUsername;
        private PasswordBox txtPassword;
        private ComboBox cmbBranch;
        public Login()
        {
            InitializeComponent();   
            Loaded += Login_Loaded;
        }
        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername = Username;
            cmbBranch = Branch;
            PasswordBox passwordBox = Password.Template.FindName("PART_PasswordBox", Password) as PasswordBox;
            if (passwordBox != null)
            {
                txtPassword = passwordBox;  // Assign the correct reference
            }
        }

        public void RegisterUser(object sender,RoutedEventArgs e)
        {
            // Get values from UI
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string branch = (cmbBranch.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(branch))
            {
                MessageBox.Show("All fields are required!" + username + password + branch, "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DatabaseHandler.Instance.openConnection();

                // ❌ Check if a user already exists in the same branch
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Branch = @branch";
                using (MySqlCommand checkCmd = new(checkQuery, DatabaseHandler.Instance.Connection))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    checkCmd.Parameters.AddWithValue("@branch", branch);

                    int existingUserCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (existingUserCount > 0)
                    {
                        MessageBox.Show("User already exists in this branch!", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        DatabaseHandler.Instance.CloseConnection();
                        return;
                    }
                }

                // 🔐 Hash the password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                // ✅ Insert user into database
                string query = "INSERT INTO Users (Username, PasswordHash, Branch) VALUES (@username, @passwordHash, @branch)";
                using (MySqlCommand cmd = new(query, DatabaseHandler.Instance.Connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@passwordHash", hashedPassword);
                    cmd.Parameters.AddWithValue("@branch", branch);

                    int result = cmd.ExecuteNonQuery();
                    DatabaseHandler.Instance.CloseConnection();

                    if (result > 0)
                    {
                        MessageBox.Show("Registration Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Registration Failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration Error: " + ex.Message);
            }
        }

        // ✅ LOGIN METHOD
        public void AuthenticateUser(object sender,RoutedEventArgs e)
        {
            // Get values from UI
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string branch = (cmbBranch.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(branch))
            {
                MessageBox.Show("All fields are required!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DatabaseHandler.Instance.openConnection();

                // ❌ Check if username exists in the selected branch
                string query = "SELECT PasswordHash FROM Users WHERE Username = @username AND Branch = @branch";
                using (MySqlCommand cmd = new(query, DatabaseHandler.Instance.Connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@branch", branch);

                    object result = cmd.ExecuteScalar();
                    DatabaseHandler.Instance.CloseConnection();

                    if (result != null)
                    {
                        string storedHashedPassword = result.ToString();

                        // 🔐 Verify password
                        if (BCrypt.Net.BCrypt.Verify(password, storedHashedPassword))
                        {
                            MessageBox.Show("Login Successful!", "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);

                            // ✅ Store login session
                            Properties.Settings.Default.Username = username;
                            Properties.Settings.Default.Branch = branch;
                            Properties.Settings.Default.IsLogin = true;
                            Properties.Settings.Default.Save();

                            // Open Main Window
                            HomeWindow home = new HomeWindow();
                            home.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Password!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("User not found in the selected branch!", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Error: " + ex.Message);
            }
        }


        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {

            // Get the button that was clicked
            Button button = sender as Button;
            if (button == null) return;

            // Find the parent Grid
            Grid parentGrid = button.Parent as Grid;
            if (parentGrid == null) return;

            // Find PasswordBox and TextBox inside the Grid
            PasswordBox passwordBox = parentGrid.Children[0] as PasswordBox;
            TextBox textBox = parentGrid.Children[1] as TextBox;


            Image EyeIcon = button.Content as Image;
            if (EyeIcon == null) return;
            Console.Write(EyeIcon.Name);

            if (passwordBox == null || textBox == null) return;

            // Toggle visibility and copy text
            isPasswordVisible = !isPasswordVisible;
            if (isPasswordVisible)
            {
                // Hide password (switch back to PasswordBox)
                passwordBox.Password = textBox.Text; // Sync password
                textBox.Visibility = Visibility.Collapsed;
                passwordBox.Visibility = Visibility.Visible;

                //button.Content = "👁"; // Normal eye icon
                EyeIcon.Source = new BitmapImage(new Uri("/Resources/Images/eye.png", UriKind.Relative));
            }
            else
            {
                // Show password (switch to TextBox)
                textBox.Text = passwordBox.Password; // Sync password
                textBox.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;
                //button.Content = "🚫"; // Strikethrough eye icon
                EyeIcon.Source = new BitmapImage(new Uri("/Resources/Images/hidden.png", UriKind.Relative));
            }
        }

        private void FetchData()
        {

            string query = "SELECT * FROM user";
            MySqlCommand cmd = new(query, DatabaseHandler.Instance.Connection);

            try
            {
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MessageBox.Show("Data: " + reader["name"]);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Query Error: " + ex.Message);
            }
        }

        public void close_window(object sender, EventArgs e)
        {
            WindowHelper.CloseWindow();
        }

        public void minimize_window(object sender, EventArgs e)
        {
            WindowHelper.MinimizeWindow(this);
        }

        public void maximize_window(Object sender, EventArgs e)
        {
            WindowHelper.MaximizeWindow(this);
        }

        public void show_password(object sender, EventArgs e)
        {
            MessageBox.Show("Show Password");
        }
    }
}
