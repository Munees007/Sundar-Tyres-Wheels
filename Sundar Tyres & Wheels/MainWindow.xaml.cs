using MySql.Data.MySqlClient;
using Sundar_Tyres___Wheels.Database;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sundar_Tyres___Wheels
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DatabaseHandler.Instance.openConnection();
            FetchData();
        }

        private void AddTempData()
        {
            
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
            Application.Current.Shutdown();
        }

        public void minimize_window(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void maximize_window(Object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
    }
}