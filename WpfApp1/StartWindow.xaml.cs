using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
    /// Logika interakcji dla klasy StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public StartWindow()
        {
            InitializeComponent();
            TextBlock_1.Text = "Aplikacja tworzy raporty z programu DigAIRange.\n\nW następnym oknie należy wybrać:\n\n\tdatę (dzień miesiąca jest bez znaczenia)\n\trodzaj raportu\n\taudycję\n\nDziękuję.";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0,0,2);
            dispatcherTimer.Start();
        }

        //Przełączenie okna po czasie
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
            dispatcherTimer.Stop();
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
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        //Informacje
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string context_menu = "Tytył:\tRaport Maker V3\nAutor:\tPatryk Szumielewicz\nE-mail:\tszumielewiczpatryk@gmail.com\nWersja:\t" + version;
            Window1 window11 = new Window1(context_menu);
            window11.Show();
        }
    }
}
