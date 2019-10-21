using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class Window1 : Window
    {
        public Window1(string s)
        {
            InitializeComponent();
            TextBlock_1.Text = s;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
            this.Close();
        }

        //Minimalizowanie okna
        private void Minimalize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
