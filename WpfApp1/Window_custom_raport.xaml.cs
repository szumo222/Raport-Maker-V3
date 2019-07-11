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

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window_custom_raport.xaml
    /// </summary>
    public partial class Window_custom_raport : Window
    {
        public bool error = false;
        public Window_custom_raport()
        {
            InitializeComponent();
        }
        private void CheckBox_1_Changed(object sender, RoutedEventArgs e)
        {
            if (checkBox_1.IsChecked == true) grid_class.IsEnabled = true;
            else if (checkBox_1.IsChecked == false) grid_class.IsEnabled = false;
        }

        private void CheckBox_2_Changed(object sender, RoutedEventArgs e)
        {
            if (checkBox_2.IsChecked == true) grid_title_name.IsEnabled = true;
            else if (checkBox_2.IsChecked == false) grid_title_name.IsEnabled = false;
        }

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            error = false;
            this.Close();
        }

        //Możliwość ruszania oknem
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        //Zamykanie okna
        private void Close_Window(object sender, RoutedEventArgs e)
        {
            error = true;
            this.Close();
        }

        //Minimalizowanie okna
        private void Minimalize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

    }
}
