using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window_XSLT_chosing.xaml
    /// </summary>
    public partial class Window_XSLT_chosing : Window
    {
        public string NameOfTheClassWrittenByUser { get; set; }
        public string Part_of_File_Name { get; set; }
        public string NameOfTheTitleWrittenByUser { get; set; }
        public bool Correct { get; set; }
        public bool Text_changed { get; set; }
        public string Xstl_content { get; set; }
        public string Part_1_of_File_Name1 { get; set; }
        public string Part_2_of_File_Name1 { get; set; }
        public string Path1 { get; set; }
        public string Path2 { get; set; } = @"raport_XSLT_chosing.xslt";
        public int Radio_int { get; set; } = 0;
        public OpenFileDialog openFileDialog = new OpenFileDialog();

        public Window_XSLT_chosing()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Text_changed = false;
        }

        private void TextBox_1_TextChanged(object sender, TextChangedEventArgs args)
        {
            Text_changed = true;
        }

        //Okno szukania pliku
        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                TextBox_1.Text = openFileDialog.FileName;
        }

        //Tworzenie raportu
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton.IsChecked == true) Radio_int = 1;
            else if (radioButton_Copy.IsChecked == true) Radio_int = 2;
            //Brak wybranego raportu raportu
            if (Text_changed == false)
            {
                Window1 window1 = new Window1("Wybierz plik ze wzorem XSLT!");
                window1.ShowDialog();
                return;
            }

            //Brak wybranego raportu raportu
            if (Radio_int == 0)
            {
                Window1 window1 = new Window1("Wybierz ze zliczaniem czy bez!");
                window1.ShowDialog();
                return;
            }

            //Wybrany raport
            else
            {
                Path1 = Path.GetFullPath(openFileDialog.FileName);
                Xstl_content = File.ReadAllText(Path1);
                File.WriteAllText(Path2, Xstl_content);
                Correct = true;
                this.Close();
            }
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

        //Informacjie
        private void Button_i_Click(object sender, RoutedEventArgs e)
        {
            string context_menu = "Wybierając opcję ze zliczaniem należy pamiętać,\niż wzorzez do stworzenia raportu, musi być podobny do pliku \nprzyklad_raportu_wg_wzoru.xslt\ntj. końcową wartością każdego wyodrębnionego elementu,\npowinna być liczba z czasem np.\nFade_MarkOut\nWybierając opcję bez liczenia wzór nie ma znaczenia.";
            Window1 window11 = new Window1(context_menu);
            window11.Show();
        }
    }
}
