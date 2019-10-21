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

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
