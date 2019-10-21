using System.Windows;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class Window2 : Window
    {
        public bool Czynadpisac { get; set; }

        public Window2()
        {
            InitializeComponent();
            TextBlock_1.Text = "Raport już istnieje\nCzy chcesz go nadpisać?";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            Czynadpisac = true;
            this.Close();
        }

        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            Czynadpisac = false;
            this.Close();
        }
    }
}
