using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.IO;

namespace WpfApp1
{
    public partial class Window_insert_class : Window
    {
        private string Xstl_content;
        private string NameOfTheClassWrittenByUser;
        private readonly string Path1 = @"raport_custom_class_backup.xslt";
        private readonly string Path2 = @"raport_custom_class.xslt";
        public string Part_of_File_Name { get; private set; }
        public bool Correct { get; private set; }

        public Window_insert_class()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            radioButton_1.IsChecked = radioButton_2.IsChecked = radioButton_3.IsChecked = radioButton_4.IsChecked = radioButton_5.IsChecked = radioButton_6.IsChecked = radioButton_7.IsChecked = false;
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

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            Xstl_content = File.ReadAllText(Path1);
            if (radioButton_1.IsChecked == true) { NameOfTheClassWrittenByUser = "News"; Part_of_File_Name = radioButton_1.Content.ToString(); }
            else if (radioButton_2.IsChecked == true) { NameOfTheClassWrittenByUser = "Music"; Part_of_File_Name = radioButton_2.Content.ToString(); }
            else if (radioButton_3.IsChecked == true) { NameOfTheClassWrittenByUser = "Cart"; Part_of_File_Name = radioButton_3.Content.ToString(); }
            else if (radioButton_4.IsChecked == true) { NameOfTheClassWrittenByUser = "Commercial"; Part_of_File_Name = radioButton_4.Content.ToString(); }
            else if (radioButton_5.IsChecked == true) { NameOfTheClassWrittenByUser = "Magazine"; Part_of_File_Name = radioButton_5.Content.ToString(); }
            else if (radioButton_6.IsChecked == true) { NameOfTheClassWrittenByUser = "Promotion"; Part_of_File_Name = radioButton_6.Content.ToString(); }
            else if (radioButton_7.IsChecked == true) { NameOfTheClassWrittenByUser = "Audio"; Part_of_File_Name = radioButton_7.Content.ToString(); }
            else
            {
                Window1 window1 = new Window1("Wybierz klasę!");
                window1.ShowDialog();
                return;
            }
            Xstl_content = Regex.Replace(Xstl_content, "NameOfTheClassWrittenByUser", NameOfTheClassWrittenByUser);
            File.WriteAllText(Path2, Xstl_content);
            Correct = true;
            this.Close();
        }
    }
}
