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
        private bool Text_changed;
        private string Xstl_content;
        private readonly string Path2 = @"raport_XSLT_chosing.xslt";
        public bool RadioButtonWithSummary { get; private set; } = false;
        private bool IfAnyRadioButtonSelected { get; set; }
        public bool Correct { get; private set; }
        public string Part_of_File_Name { get; private set; }

        private OpenFileDialog OpenFileDialog { get; set; } = new OpenFileDialog();

        public Window_XSLT_chosing()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Text_changed = false;
            IfAnyRadioButtonSelected = false;
        }

        private void TextBox_1_TextChanged(object sender, TextChangedEventArgs args)
        {
            Text_changed = true;
        }

        //Okno szukania pliku
        private void Button_3_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            if (OpenFileDialog.ShowDialog() == true)
                TextBox_1.Text = OpenFileDialog.FileName;
        }

        //Tworzenie raportu
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            //Brak wybranego raportu raportu
            if (Text_changed == false)
            {
                Window1 window1 = new Window1("Wybierz plik ze wzorem XSLT!");
                window1.ShowDialog();
                return;
            }

            if (radioButton.IsChecked == true || radioButton_Copy.IsChecked == true)
                IfAnyRadioButtonSelected = true;

            //Brak wybranego raportu raportu
            if (!IfAnyRadioButtonSelected)
            {
                Window1 window1 = new Window1("Wybierz ze zliczaniem czy bez!");
                window1.ShowDialog();
                return;
            }

            //Brak wybranego raportu raportu
            if (radioButton.IsChecked == true) RadioButtonWithSummary = true;
            else RadioButtonWithSummary = false;

            Xstl_content = File.ReadAllText(Path.GetFullPath(OpenFileDialog.FileName));
            File.WriteAllText(Path2, Xstl_content);
            Correct = true;
            this.Close();
        }

        //Informacjie
        private void Button_i_Click(object sender, RoutedEventArgs e)
        {
            string context_menu = "Wybierając opcję ze zliczaniem należy pamiętać," +
                "\niż wzorzez do stworzenia raportu, musi być podobny do pliku " +
                "\nprzyklad_raportu_wg_wzoru.xslt" +
                "\ntj. końcową wartością każdego wyodrębnionego elementu," +
                "\npowinna być liczba z czasem np." +
                "\nFade_MarkOut" +
                "\nWybierając opcję bez liczenia wzór nie ma znaczenia.";
            Window1 window11 = new Window1(context_menu);
            window11.Show();
        }
    }
}
