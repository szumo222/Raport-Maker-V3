using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class Window_insert_name : Window
    {
        public string Xstl_content;
        public bool Text_changed;
        public readonly string Path1 = @"raport_custom_title_name_backup.xslt";
        public readonly string Path2 = @"raport_custom_title_name.xslt";
        public string NameOfTheTitleWrittenByUser { get; private set; }
        public bool Correct { get; private set; }

        public Window_insert_name()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Text_changed = false;
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
            Correct = false;
            this.Close();
        }

        //Minimalizowanie okna
        private void Minimalize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void TextBox_1_TextChanged(object sender, TextChangedEventArgs args)
        {
            Text_changed = true;
        }

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            Xstl_content = File.ReadAllText(Path1);
            if (Text_changed)
            {
                NameOfTheTitleWrittenByUser = TextBox_1.Text;
                Xstl_content = Regex.Replace(Xstl_content, "NameOfTheTitleWrittenByUser", NameOfTheTitleWrittenByUser);
                Xstl_content = Regex.Replace(Xstl_content, "LengthOfTheTitleWrittenByUser", (NameOfTheTitleWrittenByUser.Length + 1).ToString());
                File.WriteAllText(Path2, Xstl_content);
                Correct = true;
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