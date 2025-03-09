using MySql.Data.MySqlClient;
using Sundar_Tyres___Wheels.Database;
using Sundar_Tyres___Wheels.Helper;
using Sundar_Tyres___Wheels.Pages;
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
            //DatabaseHandler.Instance.openConnection();
            if(Properties.Settings.Default.IsLogin==true)
            {
                HomeWindow homeWindow = new HomeWindow();
                homeWindow.Show();
            }
            else
            {
                Login login = new Login();
                login.Show();
            }
            this.Close();
        }

        
    }
}