using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sundar_Tyres___Wheels.Helper
{
    public class WindowHelper
    {
        public static void CloseWindow()
        {
            Application.Current.Shutdown();
        }

        public static void MinimizeWindow(Window window)
        {
            window.WindowState = WindowState.Minimized;
        }

        public static void MaximizeWindow(Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }
    }
}
