using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Window_custom_raport.xaml
    /// </summary>
    public partial class Window_custom_raport : Window
    {
        public string NameOfTheClassWrittenByUser { get; set; }
        public string Part_of_File_Name { get; set; }
        public string NameOfTheTitleWrittenByUser { get; set; }
        public bool Correct { get; set; }
        public bool Text_changed { get; set; }
        public string Xstl_content { get; set; }
        public string Part_1_of_File_Name1 { get; set; }
        public string Part_2_of_File_Name1 { get; set; }
        public string Path1 { get; set; } = @"raport_custom_raport_backup.xslt";
        public string Path2 { get; set; } = @"raport_custom_raport.xslt";

        public Window_custom_raport()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Text_changed = false;
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

        private void TextBox_1_TextChanged(object sender, TextChangedEventArgs args)
        {
            Text_changed = true;
        }

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            Xstl_content = File.ReadAllText(Path1);

            //SPrawdzanie czy jest wybrana opcja Klasy
            if (checkBox_1.IsChecked == true)
            {
                if (radioButton_1.IsChecked == true) { NameOfTheClassWrittenByUser = "News"; Part_1_of_File_Name1 = radioButton_1.Content.ToString(); }
                else if (radioButton_2.IsChecked == true) { NameOfTheClassWrittenByUser = "Music"; Part_1_of_File_Name1 = radioButton_2.Content.ToString(); }
                else if (radioButton_3.IsChecked == true) { NameOfTheClassWrittenByUser = "Cart"; Part_1_of_File_Name1 = radioButton_3.Content.ToString(); }
                else if (radioButton_4.IsChecked == true) { NameOfTheClassWrittenByUser = "Commercial"; Part_1_of_File_Name1 = radioButton_4.Content.ToString(); }
                else if (radioButton_5.IsChecked == true) { NameOfTheClassWrittenByUser = "Magazine"; Part_1_of_File_Name1 = radioButton_5.Content.ToString(); }
                else if (radioButton_6.IsChecked == true) { NameOfTheClassWrittenByUser = "Promotion"; Part_1_of_File_Name1 = radioButton_6.Content.ToString(); }
                else if (radioButton_7.IsChecked == true) { NameOfTheClassWrittenByUser = "Audio"; Part_1_of_File_Name1 = radioButton_7.Content.ToString(); }
                else
                {
                    Window1 window1 = new Window1("Wybierz klasę!");
                    window1.ShowDialog();
                    return;
                }
            }

            //Sprawdzanie czy jest wybrana opcja Nazwy
            if (checkBox_2.IsChecked == true)
            {
                if (Text_changed == true)
                {
                    NameOfTheTitleWrittenByUser = TextBox_1.Text;
                    Part_2_of_File_Name1 = TextBox_1.Text;
                }
                else
                {
                    Window1 window1 = new Window1("Wpisz nazwę!");
                    window1.ShowDialog();
                    return;
                }
            }

            //Ustawianie pliku do raportu
            //Tylko klasa
            if((checkBox_1.IsChecked == true) && (checkBox_2.IsChecked == false))
            {
                string regex_class = @"Element[Class='" + NameOfTheClassWrittenByUser + "']";
                Xstl_content = Regex.Replace(Xstl_content, "PetlaForEachDlaElementu", regex_class);
                Xstl_content = Regex.Replace(Xstl_content, "LengthOfTheTitleWrittenByUser", "2");
                Xstl_content = Regex.Replace(Xstl_content, "WarunekIfPoczatek", "");
                Xstl_content = Regex.Replace(Xstl_content, "WarunekIfKoniec", "");
                Part_of_File_Name = Part_1_of_File_Name1;
            }
            //Tylko nazwa
            else if ((checkBox_1.IsChecked == false) && (checkBox_2.IsChecked == true))
            {
                Xstl_content = Regex.Replace(Xstl_content, "PetlaForEachDlaElementu", "Element");
                string regex_title_name_1 = "<xsl:if test=" + '"' + "$part_of_title='" + NameOfTheTitleWrittenByUser + "'" + '"' + ">";
                string regex_title_name_2 = "</xsl:if>";
                Xstl_content = Regex.Replace(Xstl_content, "WarunekIfPoczatek", regex_title_name_1);
                Xstl_content = Regex.Replace(Xstl_content, "WarunekIfKoniec", regex_title_name_2);
                Xstl_content = Regex.Replace(Xstl_content, "LengthOfTheTitleWrittenByUser", (NameOfTheTitleWrittenByUser.Length + 1).ToString());
                Part_of_File_Name = Part_2_of_File_Name1;
            }
            //Klasa i nazwa
            else if ((checkBox_1.IsChecked == true) && (checkBox_2.IsChecked == true))
            {
                string regex_class = @"Element[Class='" + NameOfTheClassWrittenByUser + "']";
                Xstl_content = Regex.Replace(Xstl_content, "PetlaForEachDlaElementu", regex_class);
                string regex_title_name_1 = "<xsl:if test=" + '"' + "$part_of_title='" + NameOfTheTitleWrittenByUser + "'" + '"' + ">";
                string regex_title_name_2 = "</xsl:if>";
                Xstl_content = Regex.Replace(Xstl_content, "WarunekIfPoczatek", regex_title_name_1);
                Xstl_content = Regex.Replace(Xstl_content, "WarunekIfKoniec", regex_title_name_2);
                Xstl_content = Regex.Replace(Xstl_content, "LengthOfTheTitleWrittenByUser", (NameOfTheTitleWrittenByUser.Length + 1).ToString());
                Part_of_File_Name = Part_1_of_File_Name1 + "_" + Part_2_of_File_Name1;
            }
            //Brak zaznaczenia
            else if ((checkBox_1.IsChecked == false) && (checkBox_2.IsChecked == false))
            {
                Window1 window1 = new Window1("Zaznacz jedną z opcji!");
                window1.ShowDialog();
                return;
            }
            File.WriteAllText(Path2, Xstl_content);
            Correct = true;
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
            Correct = false;
            this.Close();
        }

        //Minimalizowanie okna
        private void Minimalize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}