using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Window_insert_name : Window
    {
        string xstl_content;
        public string NameOfTheTitleWrittenByUser;
        bool text_changed;
        public bool correct;
        string path1 = @"raport_custom_title_name_backup.xslt";
        string path2 = @"raport_custom_title_name.xslt";

        public Window_insert_name()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            text_changed = false;
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
            correct = false;
            this.Close();
        }

        //Minimalizowanie okna
        private void Minimalize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TextBox_1_TextChanged(object sender, TextChangedEventArgs args)
        {
            text_changed = true;
        }

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            xstl_content = File.ReadAllText(path1);
            if (text_changed)
            {
                NameOfTheTitleWrittenByUser = TextBox_1.Text;
                xstl_content = Regex.Replace(xstl_content, "NameOfTheTitleWrittenByUser", NameOfTheTitleWrittenByUser);
                xstl_content = Regex.Replace(xstl_content, "LengthOfTheTitleWrittenByUser", (NameOfTheTitleWrittenByUser.Length + 1).ToString());
                File.WriteAllText(path2, xstl_content);
                correct = true;
                this.Close();
            }
            else
            {
                Window1 window1 = new Window1("Wpisz nazwę!");
                window1.ShowDialog();
                return;
            }
        }
    }
}