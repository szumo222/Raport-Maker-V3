using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using System.Xml.Xsl;
using Path = System.IO.Path;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        string miesiac, rok;
        bool czynadpisac;
        string f_xslt;
        string[] day;
        IEnumerable<string> dayy;
        string destination_folder_zaiks, destination_folder_stoart;
        string destination_folder_ekstra_zaiks, destination_folder_ekstra_stoart;
        string get_folder_szczecin_zaiks, get_folder_szczecin_stoart;
        string get_folder_szn_ekstra_zaiks, get_folder_szn_ekstra_stoart;
        string get_folder_zaiks, get_folder_stoart;
        string fname, fname_part;
        string dsa = @"raport_maker_help\";
        int szn_or_szn_ekstra = 0;
        bool error = false;
        bool text_box_text_change = false;
        List<string> array2 = new List<string>();
        string first_line;

        public MainWindow()
        {
            InitializeComponent();
            radioButton_1.IsChecked = false;
            grid_main.Visibility = Visibility.Hidden;
            grid_start.Visibility = Visibility.Visible;
            radioButton_2.IsChecked = radioButton_3.IsChecked = radioButton_4.IsChecked = radioButton_5.IsChecked = radioButton_6.IsChecked = radioButton_7.IsChecked = radioButton_8.IsChecked = false;
            TextBlock_1.Text = "Aplikacja tworzy raporty z programu DigAIRange.\n\nW następnym oknie należy wybrać:\n\n\tdatę (dzień miesiąca jest bez znaczenia)\n\trodzaj raportu\n\taudycję\n\nDziękuję.";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        //Selektor kalendarza
        private void MonthlyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_1.Text = DataPicker_1.SelectedDate.Value.ToString("MMMM");
            TextBox_2.Text = DataPicker_1.SelectedDate.Value.ToString("yyyy");
            text_box_text_change = true;
        }

        //Główny przycisk
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (text_box_text_change == false)
                {
                    Window1 window1 = new Window1("Ustaw datę!");
                    window1.ShowDialog();
                    return;
                }

                Main_Function_Config_Raport_Maker();

                if (error == true)
                {
                    Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = true;
                    ProgressBar_1.IsIndeterminate = false;
                    ProgressBar_1.Opacity = 0.1;
                    return;
                }

                miesiac = DataPicker_1.SelectedDate.Value.Month.ToString();
                rok = DataPicker_1.SelectedDate.Value.Year.ToString();

                Radiocheck_ktory_folder();

                //Przerwanie programu gdy pojawi się error
                if (error == true)
                {
                    Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = true;
                    ProgressBar_1.IsIndeterminate = false;
                    ProgressBar_1.Opacity = 0.1;
                    return;
                }
                else
                {
                    Radiocheck();

                    //Przerwanie programu gdy pojawi się Error
                    if (error == true)
                    {
                        Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = true;
                        ProgressBar_1.IsIndeterminate = false;
                        ProgressBar_1.Opacity = 0.1;
                        return;
                    }
                    else
                    {
                        Stopwatch sw = new Stopwatch();
                        List<string> array3 = new List<string>();
                        List<string> array4 = new List<string>();
                        czynadpisac = false;
                        int z = 0;

                        //Sprawdzanie czy dany plik wyjściowy już istnieje
                        if (File.Exists(fname))
                        {
                            Window2 window2 = new Window2();
                            window2.ShowDialog();
                            if (!window2.czynadpisac)
                            {
                                czynadpisac = false;
                                return;
                            }
                            else
                            {
                                FileInfo fff = new FileInfo(fname);
                                fff.Delete();
                                czynadpisac = true;
                            }
                        }
                        else if (!File.Exists(fname)) czynadpisac = true;

                        if (czynadpisac == true)
                        {
                            List<string> path = new List<string>();
                            foreach (string file in array2)
                            {
                                path.Add(Path.GetFileNameWithoutExtension(file));
                            }

                            DirectoryInfo di = new DirectoryInfo(dsa);
                            Parallel.ForEach(di.GetFiles(), file =>
                            {
                                file.Delete();
                            });
                            //Osbługa transformaty XSLT na osobnym wątku w celu nie zastygania UI
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.DoWork += new DoWorkEventHandler((sender1, args) => Main_Function_XSLT_Transform(array2, path, f_xslt, z, sw));
                            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender1, args) => Main_Function_After_XSLT(array3, array4, sw));
                            bw.RunWorkerAsync();
                            ProgressBar_1.IsIndeterminate = true;
                            ProgressBar_1.Opacity = 100;
                            Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = false;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd:\t" + ex, "Raport Maker V3");
                FileInfo ffff = new FileInfo(fname);
                ffff.Delete();
                array2.Clear();
                return;
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
            this.Close();
        }

        //Minimalizowanie okna
        private void Minimalize_Window(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        //Metoda przypisująca nazwy folderów wg. pliku konfiguracyjnego config_raport_maker.xml
        private void Main_Function_Config_Raport_Maker()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"config_raport_maker.xml");
            XmlNodeList xml_zaiks = doc.GetElementsByTagName("zaiks");

            Parallel.For(0, xml_zaiks.Count, i =>
            {
                XmlNode xml_dest_folder_zaiks = xml_zaiks[i].SelectSingleNode("destination_folder");
                XmlNode xml_dest_folder_ekstra_zaiks = xml_zaiks[i].SelectSingleNode("destination_folder_ekstra");
                XmlNode xml_get_folder_zaiks = xml_zaiks[i].SelectSingleNode("get_folder_szczecin");
                XmlNode xml_get_folder_x_zaiks = xml_zaiks[i].SelectSingleNode("get_folder_x");
                destination_folder_zaiks = xml_dest_folder_zaiks.InnerText;
                destination_folder_ekstra_zaiks = xml_dest_folder_ekstra_zaiks.InnerText;
                get_folder_szczecin_zaiks = xml_get_folder_zaiks.InnerText;
                get_folder_szn_ekstra_zaiks = xml_get_folder_x_zaiks.InnerText;
            });

            XmlNodeList xml_stoart = doc.GetElementsByTagName("stoart");

            Parallel.For(0, xml_stoart.Count, i =>
            {
                XmlNode xml_dest_folder_stoart = xml_stoart[i].SelectSingleNode("destination_folder");
                XmlNode xml_dest_folder_ekstra_stoart = xml_stoart[i].SelectSingleNode("destination_folder_ekstra");
                XmlNode xml_get_folder_stoart = xml_stoart[i].SelectSingleNode("get_folder_szczecin");
                XmlNode xml_get_folder_x_stoart = xml_stoart[i].SelectSingleNode("get_folder_x");
                destination_folder_stoart = xml_dest_folder_stoart.InnerText;
                destination_folder_ekstra_stoart = xml_dest_folder_ekstra_stoart.InnerText;
                get_folder_szczecin_stoart = xml_get_folder_stoart.InnerText;
                get_folder_szn_ekstra_stoart = xml_get_folder_x_stoart.InnerText;
            });

            Check_folders_exist();
        }

        //Sprawdzanie dostępu do folderu z plikami xml (w domyślne w sieciowej lokalizacji)
        private void Check_folders_exist()
        {
            string[] folders_exist = { destination_folder_zaiks , destination_folder_ekstra_zaiks , get_folder_szczecin_zaiks, get_folder_szn_ekstra_zaiks ,
                destination_folder_stoart , destination_folder_ekstra_stoart , get_folder_szczecin_stoart , get_folder_szn_ekstra_stoart };

            for (int i = 0; i < folders_exist.Length; i++)
            {
                if (!Directory.Exists(folders_exist[i]))
                {
                    MessageBox.Show("Nie można znaleźć ścieżki " + folders_exist[i]);
                    error = true;
                    return;
                }
            }

        }

        //Metoda odpowiadająca za transformatę XSLT
        static void Main_Function_XSLT_Transform(List<string> array2, List<string> path, string f_xslt, int z, Stopwatch sw)
        {
            sw.Start();
            XslCompiledTransform xslt2 = new XslCompiledTransform();
            xslt2.Load(f_xslt);
            foreach (string file in array2)
            {
                string f_out2;
                Console.WriteLine(file);
                f_out2 = @"raport_maker_help\" + path[z] + "_" + z + ".txt";
                xslt2.Transform(file, f_out2);
                f_out2 = "";
                z++;
            }
        }

        //Metoda wywoływana po przeprowadzeniu transfomarty XSLT / operowanie na plikach w folderze pomocniczym raport_maker_help
        private void Main_Function_After_XSLT(List<string> array3, List<string> array4, Stopwatch sw)
        {
            IEnumerable<string> array = Directory.EnumerateFiles(@"raport_maker_help\", "*.txt", SearchOption.AllDirectories);
            foreach (string file in array)
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    array3.Add(lines[i]);
                }
            }
            array3.Sort();
            int iiii = 1;
            foreach (string s in array3)
            {
                StringBuilder ss = new StringBuilder(s);

                // Raport Stoart
                if (radioButton_2.IsChecked == true)
                {
                    if (ss.Length < 2) ss.Remove(0, ss.Length);
                    if (ss.Length >= 2) ss.Remove(0, 6);
                    if (ss.Length != 0) { ss.Insert(0, iiii.ToString() + "|"); iiii++; }
                    array4.Add(ss.ToString());
                }

                //Raporty zaiks, materiały
                else
                {
                    if (ss.Length < 2) ss.Remove(0, ss.Length);
                    if (ss.Length >= 2) ss.Remove(17, 6);
                    array4.Add(ss.ToString());
                }
            }
            iiii = 1;
            //Czyszczenie listy z pustych wierszy
            for (int i = array4.Count - 1; i >= 0; i--)
            {
                if (array4[i] == "")
                {
                    array4.RemoveAt(i);
                }
            }

            List<string> array5 = new List<string>();
            List<string> array6 = new List<string>();

            //Wywołanie funkcji dla stoart
            if (radioButton_2.IsChecked == true)
            {
                Stoart_Array_Prepare(array4, array5, array6, iiii);
            }

            //Wywołanie funkcji do reklamy
            if ((radioButton_6.IsChecked == true) || (radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true))
            {
                Reklama_Array_Prepare(array4, array5);
            }

            array4.Insert(0, first_line);

            //Zapisanie pliku dla stoart
            if (radioButton_2.IsChecked == true)
            {
                array6.Insert(0, first_line);
                File.WriteAllLines(fname + "_ze_zliczaniem.txt", array6, Encoding.UTF8);
                File.WriteAllLines(fname, array4, Encoding.UTF8);
            }
            //Zapisanie pliku dla reklamy, własnej klasy, własnej nazwy
            else if ((radioButton_6.IsChecked == true) || (radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true))
            {
                array5.Insert(0, first_line);
                File.WriteAllLines(fname, array5, Encoding.UTF8);
            }
            //Zapisanie pliku dla zaiks, materiały
            else File.WriteAllLines(fname, array4, Encoding.UTF8);

            DirectoryInfo di = new DirectoryInfo(dsa);
            array2.Clear();
            array3.Clear();
            array4.Clear();
            array5.Clear();
            array6.Clear();
            Parallel.ForEach(di.GetFiles(), file =>
            {
                file.Delete();
            });

            if((radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true))
            {
                if (File.Exists(f_xslt)) File.Delete(f_xslt);
            }

            FileInfo f1 = new FileInfo(fname);
            string textblock_content = "Zakończono.Plik \n\n" + f1.Name + "\n\nzostał zapisany.";
            Window1 window1 = new Window1(textblock_content);
            sw.Stop();
            Console.WriteLine("Czas wykonania programu: " + Math.Round(TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds));
            Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = true;
            ProgressBar_1.IsIndeterminate = false;
            ProgressBar_1.Opacity = 0.1;
            window1.Show();
        }

        //Przygotowanie listy do zapisu dla Stoart
        private void Stoart_Array_Prepare(List<string> array4, List<string> array5, List<string> array6, int iiii)
        {
            //Usuwanie liczby Lp. na początku 
            foreach (string line in array4)
            {
                string replace;
                replace = Regex.Replace(line, @"^\d{1,}\|", "");
                array5.Add(replace);
            }
            array5.Sort();

            string linia_liczba_odt_1 = "";
            string linia_liczba_odt_2 = "";
            int odt = 1;

            //Liczenie takich samych wierszy
            for (int i = 0; i < array5.Count; i++)
            {
                //Dla całej listy bez ostatniego elementu
                if (i < array5.Count() - 1)
                {
                    //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                    string part_of_line = Regex.Replace(array5[i].ToString(), @"L.nad.\|.+", "");
                    string part_of_compare_line = Regex.Replace(array5[i + 1].ToString(), @"L.nad.\|.+", "");
                    if (part_of_line == part_of_compare_line)
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        array6.Add("");
                        odt++;
                    }
                    //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                    else
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        linia_liczba_odt_2 = iiii.ToString() + "|" + linia_liczba_odt_1;
                        iiii++;
                        array6.Add(linia_liczba_odt_2);
                        odt = 1;
                    }
                }
                //Dla ostatniego elementu
                else
                {
                    //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                    string part_of_line = Regex.Replace(array5[i].ToString(), @"L.nad.\|.+", "");
                    string part_of_compare_line = Regex.Replace(array5[i - 1].ToString(), @"L.nad.\|.+", "");
                    if (part_of_line == part_of_compare_line)
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        array6.Add("");
                        odt++;
                    }
                    //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                    else
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        linia_liczba_odt_2 = iiii.ToString() + "|" + linia_liczba_odt_1;
                        iiii++;
                        array6.Add(linia_liczba_odt_2);
                        odt = 1;
                    }
                }
            }
            //Czyszczenie listy z pustych wierwszy
            for (int i = array6.Count - 1; i >= 0; i--)
            {
                if (array6[i] == "")
                {
                    array6.RemoveAt(i);
                }
            }
        }

        //Przygotowanie listy do zapisu dla reklam
        private void Reklama_Array_Prepare(List<string> array4, List<string> array5)
        {
            int suma_czas_reklama = 0;
            int czas_reklaman_dzien = 0;
            List<string> suma_czas_dzien = new List<string>();
            suma_czas_dzien.Clear();
            for (int i = 0; i < array4.Count; i++)
            {
                Match match = Regex.Match(Regex.Match(array4[i], @"\|\d+\|$").ToString(), @"\d+");
                string string_out = Regex.Replace(array4[i], @"\|\d+\|$", "|");
                array5.Add(string_out);
                suma_czas_reklama = suma_czas_reklama + int.Parse(match.ToString());
                if (i == 0) czas_reklaman_dzien = int.Parse(match.ToString());
                else if ((i > 0) && (i < array4.Count - 1))
                {
                    Match match_2 = Regex.Match(array4[i], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    Match match_3 = Regex.Match(array4[i - 1], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    if (match_2.ToString() == match_3.ToString())
                    {
                        czas_reklaman_dzien = czas_reklaman_dzien + int.Parse(match.ToString());
                    }
                    else
                    {
                        suma_czas_dzien.Add("Suma elementów z dnia|" + match_3.ToString() + "|" + Calculate_duration(czas_reklaman_dzien) + "|");
                        czas_reklaman_dzien = int.Parse(match.ToString());
                    }
                }
                else if (i == array4.Count - 1)
                {
                    Match match_2 = Regex.Match(array4[i], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    Match match_3 = Regex.Match(array4[i - 1], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    czas_reklaman_dzien = czas_reklaman_dzien + int.Parse(match.ToString());
                    suma_czas_dzien.Add("Suma elementów z dnia|" + match_2.ToString() + "|" + Calculate_duration(czas_reklaman_dzien) + "|");
                }
            }
            for(int i = 0; i < suma_czas_dzien.Count; i++)
            {
                array5.Insert(i, suma_czas_dzien[i]);
            }
            array5.Insert(0, "Sumaryczny czas elementów w miesiacu|" + Calculate_duration(suma_czas_reklama) + "|");
        }

        //Przeliczanie czasu z milisekund na format H:M:S
        private string Calculate_duration(int suma_czas_reklama)
        {
            int godziny = suma_czas_reklama / 3600000;
            int reszta_z_godzin = suma_czas_reklama % 3600000;
            int minuty = reszta_z_godzin / 60000;
            int reszta_z_minut = reszta_z_godzin % 60000;
            int sekundy = reszta_z_minut / 1000;
            return godziny + ":" + minuty + ":" + sekundy;
        }

        //Sprawdzanie który rodzaj raportu został wybrany i przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego
        private void Radiocheck()
        {
            //Zaiks
            if (radioButton_1.IsChecked == true)
            {
                error = false;
                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_zaiks + @"raport_zaiks_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_zaiks + @"raport_zaiks_" + fname_part;
                }

                first_line = "Data|Godz.aud.|Tytul audycji|Tytul utworu|Kompozytor|Autor tekstu|Tlumacz|Czas|Wykonawca|Producent|Wydawca|";
                f_xslt = @"raportdlazaikkopias.xslt";
                string[] folder_days_zaiks_dir = Directory.GetDirectories(get_folder_zaiks + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_zaiks_dir, dir =>
                {
                    RadioCheck_Parrel_ForEach(dir);
                });
            }

            //Stoart
            else if (radioButton_2.IsChecked == true)
            {
                error = false;
                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_stoart + @"raport_stoart_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_stoart + @"raport_stoart_" + fname_part;
                }

                first_line = "Lp|WYKONAWCA|TUTYŁ UTWORU|CZAS UTWORU|ILOŚĆ NADAŃ|TYTUŁ PŁYTY|NUMER KATALOGOWY PŁYTY|WYDAWCA|ROK WYDANIA|POLSKA/ZAGRANICA(PL/Z)|KOD ISRC|";
                f_xslt = @"raportdlastoartkapias.xslt";
                string[] folder_days_stoart_dir = Directory.GetDirectories(get_folder_stoart + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_stoart_dir, dir =>
                {
                    RadioCheck_Parrel_ForEach(dir);
                });
            }

            //Materiały
            else if (radioButton_5.IsChecked == true)
            {
                error = false;
                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_stoart + @"raport_materialy_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_stoart + @"raport_materialy_" + fname_part;
                }

                first_line = "Data;Godz.aud.;Tytul audycji;Godz. emisji;Długość;Tytuł;Autor;";
                f_xslt = @"raportmaterialykopia.xslt";
                string[] folder_days_stoart_dir = Directory.GetDirectories(get_folder_stoart + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_stoart_dir, dir =>
                {
                    RadioCheck_Parrel_ForEach(dir);
                });
            }

            //Wg klasy reklama
            else if (radioButton_6.IsChecked == true)
            {
                error = false;
                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_stoart + @"raport_reklamy_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_stoart + @"raport_reklamy_" + fname_part;
                }

                first_line = "Data|Godz.aud.|Tytul audycji|Tytul reklamy|Kompozytor|Autor|Czas|";
                f_xslt = @"raportreklamakapias.xslt";
                string[] folder_days_stoart_dir = Directory.GetDirectories(get_folder_stoart + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_stoart_dir, dir =>
                {
                    RadioCheck_Parrel_ForEach(dir);
                });
            }

            //Wg własnej klasy
            else if (radioButton_7.IsChecked == true)
            {
                error = false;

                Window_insert_class window_Insert_Class = new Window_insert_class();
                window_Insert_Class.ShowDialog();

                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_stoart + @"raport_z_klasy_" + window_Insert_Class.Part_of_File_Name + "_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_stoart + @"raport_z_klasy_" + window_Insert_Class.Part_of_File_Name + "_" + fname_part;
                }

                first_line = "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|";
                f_xslt = @"raport_custom_class.xslt";
                string[] folder_days_stoart_dir = Directory.GetDirectories(get_folder_stoart + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_stoart_dir, dir =>
                {
                    RadioCheck_Parrel_ForEach(dir);
                });
            }

            //Wg nazwy
            else if (radioButton_8.IsChecked == true)
            {
                error = false;

                //Window_insert_name window_Insert_Name = new Window_insert_name();
                Window_custom_raport window_Insert_Name = new Window_custom_raport();
                window_Insert_Name.ShowDialog();
                /*
                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_stoart + @"raport_z_nazwy_" + window_Insert_Name.NameOfTheTitleWrittenByUser + "_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_stoart + @"raport_z_nazwy_" + window_Insert_Name.NameOfTheTitleWrittenByUser + "_" + fname_part;
                }*/

                first_line = "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|";
                f_xslt = @"raport_custom_title_name.xslt";
                string[] folder_days_stoart_dir = Directory.GetDirectories(get_folder_stoart + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_stoart_dir, dir =>
                {
                    RadioCheck_Parrel_ForEach(dir);
                });
            }

            //Brak
            else
            {
                Window1 window1 = new Window1("Wybierz rodzaj raportu!");
                window1.Show();
                error = true;
            }
        }

        //Szukanie plików z rozszerzeniem XML -> dodanie ich do listy
        private void RadioCheck_Parrel_ForEach(string dir)
        {
            string dirrr = dir + @"\Shows";
            dayy = Directory.EnumerateFiles(dirrr, "*.xml", SearchOption.AllDirectories);
            day = dayy.ToArray();
            foreach (string d in day)
            {
                array2.Add(d);
            }
        }

        //Sprwadznie który folder jest zaznaczony i przypisywanie odpowiedniej częsci nazwy pliku wyjściowego
        private void Radiocheck_ktory_folder()
        {
            if (radioButton_3.IsChecked == true)
            {
                get_folder_zaiks = get_folder_szczecin_zaiks;
                get_folder_stoart = get_folder_szczecin_stoart;
                fname_part = @"szczecin_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt";
                szn_or_szn_ekstra = 1;
                error = false;

            }
            else if (radioButton_4.IsChecked == true)
            {
                get_folder_zaiks = get_folder_szn_ekstra_zaiks;
                get_folder_stoart = get_folder_szn_ekstra_stoart;
                fname_part = @"szczecin_FM_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt";
                szn_or_szn_ekstra = 2;
                error = false;
            }
            else
            {
                Window1 window1 = new Window1("Wybierz który raport!");
                window1.Show();
                error = true;
            }
        }

        //Przycisk zmiany widoku ze startowego na główny
        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Visibility = Visibility.Visible;
            grid_start.Visibility = Visibility.Hidden;
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