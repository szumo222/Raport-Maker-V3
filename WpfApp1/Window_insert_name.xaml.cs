using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.IO;

namespace WpfApp1
{
    public partial class Window_insert_name : Window
    {
        string xstl_content;
        public string NameOfTheTitleWrittenByUser;
        string path1 = @"raport_custom_title_name_backup.xslt";
        string path2 = @"raport_custom_title_name.xslt";

        public Window_insert_name()
        {
            InitializeComponent();
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
            xstl_content = File.ReadAllText(path1);
            NameOfTheTitleWrittenByUser = TextBox_1.Text;
            xstl_content = Regex.Replace(xstl_content, "NameOfTheTitleWrittenByUser", NameOfTheTitleWrittenByUser);
            xstl_content = Regex.Replace(xstl_content, "LengthOfTheTitleWrittenByUser", (NameOfTheTitleWrittenByUser.Length + 1).ToString());
            File.WriteAllText(path2, xstl_content);
            this.Close();
        }
    }
}