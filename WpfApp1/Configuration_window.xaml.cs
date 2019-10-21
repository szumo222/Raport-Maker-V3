using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy Configuration_Window.xaml
    /// </summary>
    public partial class Configuration_Window : Window
    {
        public Configuration_Window()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        //Main button
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
        }

        //Informacjie
        private void Button_i_Click(object sender, RoutedEventArgs e)
        {
            string context_menu = "Wybierając opcję ze zliczaniem należy pamiętać,\niż wzorzez do stworzenia raportu, musi być podobny do pliku \nprzyklad_raportu_wg_wzoru.xslt\ntj. końcową wartością każdego wyodrębnionego elementu,\npowinna być liczba z czasem np.\nFade_MarkOut\nWybierając opcję bez liczenia wzór nie ma znaczenia.";
            Window1 window11 = new Window1(context_menu);
            window11.Show();
        }

        //Look for forder to save output file
        private void Button_2_Click(object sender, RoutedEventArgs e)
        {

        }

        //Look for forder with Szczecin XML
        private void Button_3_Click(object sender, RoutedEventArgs e)
        {

        }

        //Look for forder with Szczecin Ekrstra XML
        private void Button_4_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
