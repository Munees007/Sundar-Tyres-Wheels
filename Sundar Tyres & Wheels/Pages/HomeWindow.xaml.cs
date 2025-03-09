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
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {

        public HomeWindow()
        {
            InitializeComponent();
        }
        public void LoginOut()
        {
            Properties.Settings.Default.IsLogin = false;
            Properties.Settings.Default.Username = "";
            Properties.Settings.Default.Branch = "";
            Properties.Settings.Default.Save();
            Login login = new Login();
            login.Show();
            this.Close();
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem selectedItem)
            {
                switch (selectedItem.Name)
                {
                    case "addCustomer":
                        MainFrame.Navigate(new AddCustomer());
                        break;
                    case "viewCustomer":
                        MainFrame.Navigate(new ViewCustomer());
                        break;
                    case "updateCustomer":
                        MainFrame.Navigate(new UpdateCustomer());
                        break;
                    case "addVehicle":
                        MainFrame.Navigate(new AddVehicle());
                        break;
                    case "viewVehicle":
                        MainFrame.Navigate(new ViewVehicles());
                        break;
                    case "logOut":
                        LoginOut();
                        break;
                    default:
                        MainFrame.Content = null;
                        break;
                }
            }
        }
        private void DragWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove();
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
    }
}
