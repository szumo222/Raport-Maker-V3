using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using System.Xml.Xsl;
using Path = System.IO.Path;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        public bool Czynadpisac { get; set; }
        public string F_xslt { get; set; }
        public string[] Day { get; set; }
        public IEnumerable<string> Dayy { get; set; }
        public string Destination_folder_zaiks { get; set; }
        public string Destination_folder_stoart { get; set; }
        public string Destination_folder_ekstra_zaiks { get; set; }
        public string Destination_folder_ekstra_stoart { get; set; }
        public string Get_folder_szczecin_zaiks { get; set; }
        public string Get_folder_szczecin_stoart { get; set; }
        public string Get_folder_szn_ekstra_zaiks { get; set; }
        public string Get_folder_szn_ekstra_stoart { get; set; }
        public string Get_folder_zaiks { get; set; }
        public string Get_folder_stoart { get; set; }
        public string Fname { get; set; }
        public string Fname_part { get; set; }
        public string Dsa { get; set; } = @"raport_maker_help\";
        public int Szn_or_szn_ekstra { get; set; } = 0;
        public int Ze_zliczaniem_czy_bez { get; set; } = 0;
        public bool Error { get; set; }
        public bool Text_box_text_change { get; set; } = false;
        public bool Text_box_2_text_change { get; set; } = false;
        public List<string> Array2 { get; set; } = new List<string>();
        public string First_line { get; set; }
        public List<List_date> Tab_list_date_before_distinct { get; set; } = new List<List_date>();
        public List<List_date> Tab_list_date { get; set; } = new List<List_date>();
        public MainWindow()
        {
            
            InitializeComponent();
            radioButton_1.IsChecked = false;
            grid_main.Visibility = Visibility.Hidden;
            grid_start.Visibility = Visibility.Visible;
            radioButton_2.IsChecked = radioButton_3.IsChecked = radioButton_4.IsChecked = radioButton_5.IsChecked = radioButton_6.IsChecked = radioButton_7.IsChecked = radioButton_8.IsChecked = false;
            TextBlock_1.Text = "Aplikacja tworzy raporty z programu DigAIRange.\n\nW następnym oknie należy wybrać:\n\n\tdatę początkową\n\tdatę końcową\n\trodzaj raportu\n\taudycję\n\nDziękuję.";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DirectoryInfo di = new DirectoryInfo(Dsa);
            Parallel.ForEach(di.GetFiles(), file =>
            {
                file.Delete();
            });
        }

        //Selektor kalendarza
        private void MonthlyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_1.Text = DataPicker_1.SelectedDate.Value.ToString("d MMMM yyyy");
            Text_box_text_change = true;
        }

        //Selektor kalendarza
        private void MonthlyCalendar_2_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_2.Text = DataPicker_2.SelectedDate.Value.ToString("d MMMM yyyy");
            Text_box_2_text_change = true;
        }

        private List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> rv = new List<DateTime>();
            DateTime tmpDate = StartingDate;
            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= EndingDate);
            return rv;
        }

        private void Month_range(DatePicker d1, DatePicker d2)
        {
            DateTime StartingDate = d1.SelectedDate.Value;
            DateTime EndingDate = d2.SelectedDate.Value;
            foreach (DateTime date in GetDateRange(StartingDate, EndingDate))
            {
                List_date ld = new List_date();
                ld.List_date_month = date.Month.ToString();
                ld.List_date_year = date.Year.ToString();
                ld.List_date_day = date.Day.ToString();
                Tab_list_date_before_distinct.Add(ld);
            }
            Tab_list_date = Tab_list_date_before_distinct.Distinct().ToList();
            foreach(List_date xxx in Tab_list_date)
            {
                Console.WriteLine("Dzień: " + xxx.List_date_day + "\tMiesiac: " + xxx.List_date_month + "\tRok: " + xxx.List_date_year);
            }
        }

        //Główny przycisk
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Error = false;
                if ((Text_box_text_change == false) || (Text_box_2_text_change == false))
                {
                    Window1 window1 = new Window1("Ustaw datę!");
                    window1.ShowDialog();
                    return;
                }

                Main_Function_Config_Raport_Maker();

                if (Error == true)
                {
                    Error_set_components();
                }

                Month_range(DataPicker_1, DataPicker_2);



                Radiocheck_ktory_folder();

                //Przerwanie programu gdy pojawi się error
                if (Error == true)
                {
                    Error_set_components();
                }
                else
                {
                    Radiocheck();

                    //Przerwanie programu gdy pojawi się Error
                    if (Error == true)
                    {
                        Error_set_components();
                    }
                    else
                    {
                        Stopwatch sw = new Stopwatch();
                        List<string> array3 = new List<string>();
                        List<string> array4 = new List<string>();
                        Czynadpisac = false;
                        int z = 0;

                        //Sprawdzanie czy dany plik wyjściowy już istnieje
                        if (File.Exists(Fname))
                        {
                            Window2 window2 = new Window2();
                            window2.ShowDialog();
                            if (!window2.Czynadpisac)
                            {
                                Czynadpisac = false;
                                return;
                            }
                            else
                            {
                                FileInfo fff = new FileInfo(Fname);
                                fff.Delete();
                                Czynadpisac = true;
                            }
                        }
                        else if (!File.Exists(Fname)) Czynadpisac = true;

                        if (Czynadpisac == true)
                        {
                            List<string> path = new List<string>();
                            foreach (string file in Array2)
                            {
                                path.Add(Path.GetFileNameWithoutExtension(file));
                            }

                            DirectoryInfo di = new DirectoryInfo(Dsa);
                            Parallel.ForEach(di.GetFiles(), file =>
                            {
                                file.Delete();
                            });

                            //Osbługa transformaty XSLT na osobnym wątku w celu nie zastygania UI
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.DoWork += new DoWorkEventHandler((sender1, args) => Main_Function_XSLT_Transform(Array2, path, F_xslt, z, sw));
                            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender1, args) => Main_Function_After_XSLT(array3, array4, sw));
                            bw.RunWorkerAsync();
                            ProgressBar_1.IsIndeterminate = true;
                            ProgressBar_1.Opacity = 100;
                            Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = DataPicker_2.IsEnabled = false;
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
                FileInfo ffff = new FileInfo(Fname);
                ffff.Delete();
                Array2.Clear();
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

        //Resetowanie elementów okna do stanu początkowego po błędzie
        private void Error_set_components()
        {
            Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = true;
            ProgressBar_1.IsIndeterminate = false;
            ProgressBar_1.Opacity = 0.1;
            return;
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
                Destination_folder_zaiks = xml_dest_folder_zaiks.InnerText;
                Destination_folder_ekstra_zaiks = xml_dest_folder_ekstra_zaiks.InnerText;
                Get_folder_szczecin_zaiks = xml_get_folder_zaiks.InnerText;
                Get_folder_szn_ekstra_zaiks = xml_get_folder_x_zaiks.InnerText;
            });

            XmlNodeList xml_stoart = doc.GetElementsByTagName("stoart");

            Parallel.For(0, xml_stoart.Count, i =>
            {
                XmlNode xml_dest_folder_stoart = xml_stoart[i].SelectSingleNode("destination_folder");
                XmlNode xml_dest_folder_ekstra_stoart = xml_stoart[i].SelectSingleNode("destination_folder_ekstra");
                XmlNode xml_get_folder_stoart = xml_stoart[i].SelectSingleNode("get_folder_szczecin");
                XmlNode xml_get_folder_x_stoart = xml_stoart[i].SelectSingleNode("get_folder_x");
                Destination_folder_stoart = xml_dest_folder_stoart.InnerText;
                Destination_folder_ekstra_stoart = xml_dest_folder_ekstra_stoart.InnerText;
                Get_folder_szczecin_stoart = xml_get_folder_stoart.InnerText;
                Get_folder_szn_ekstra_stoart = xml_get_folder_x_stoart.InnerText;
            });

            Check_folders_exist();
        }

        //Sprawdzanie dostępu do folderu z plikami xml (w domyślne w sieciowej lokalizacji)
        private void Check_folders_exist()
        {
            string[] folders_exist = { Destination_folder_zaiks , Destination_folder_ekstra_zaiks , Get_folder_szczecin_zaiks, Get_folder_szn_ekstra_zaiks ,
                Destination_folder_stoart , Destination_folder_ekstra_stoart , Get_folder_szczecin_stoart , Get_folder_szn_ekstra_stoart };

            for (int i = 0; i < folders_exist.Length; i++)
            {
                if (!Directory.Exists(folders_exist[i]))
                {
                    MessageBox.Show("Nie można znaleźć ścieżki " + folders_exist[i]);
                    Error = true;
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
            Class1 class1 = new Class1();
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
                class1.Stoart_Array_Prepare(array4, array5, array6, iiii);
            }
            //Wywołanie funkcji do reklamy
            if ((radioButton_6.IsChecked == true) || (radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true) || (radioButton_9.IsChecked == true) || ((radioButton_10.IsChecked == true) && (Ze_zliczaniem_czy_bez == 1)))
            {
                class1.Reklama_Array_Prepare(array4, array5);
            }

            array4.Insert(0, First_line);

            //Zapisanie pliku dla stoart
            if (radioButton_2.IsChecked == true)
            {
                array6.Insert(0, First_line);
                File.WriteAllLines(Fname + "_ze_zliczaniem.txt", array6, Encoding.UTF8);
                File.WriteAllLines(Fname, array4, Encoding.UTF8);
            }
            //Zapisanie pliku dla reklamy, własnej klasy, własnej nazwy
            else if ((radioButton_6.IsChecked == true) || (radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true) || (radioButton_9.IsChecked == true) || ((radioButton_10.IsChecked == true) && (Ze_zliczaniem_czy_bez == 1)))
            {
                array5.Insert(0, First_line);
                File.WriteAllLines(Fname, array5, Encoding.UTF8);
            }
            //Zapisanie pliku dla zaiks, materiały
            else File.WriteAllLines(Fname, array4, Encoding.UTF8);

            DirectoryInfo di = new DirectoryInfo(Dsa);
            Array2.Clear();
            array3.Clear();
            array4.Clear();
            array5.Clear();
            array6.Clear();
            Parallel.ForEach(di.GetFiles(), file =>
            {
                file.Delete();
            });

            if((radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true) || (radioButton_9.IsChecked == true) || (radioButton_10.IsChecked == true))
            {
                if (File.Exists(F_xslt)) File.Delete(F_xslt);
            }

            FileInfo f1 = new FileInfo(Fname);
            string textblock_content = "Zakończono.Plik \n\n" + f1.Name + "\n\nzostał zapisany.";
            Window1 window1 = new Window1(textblock_content);
            sw.Stop();
            Console.WriteLine("Czas wykonania programu: " + Math.Round(TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds).TotalSeconds));
            Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = DataPicker_2.IsEnabled = true;
            ProgressBar_1.IsIndeterminate = false;
            ProgressBar_1.Opacity = 0.1;
            window1.Show();
        }

        private void Get_months_folders(string get_folder)
        {
            List<string> folder_days_dir = new List<string>();
            Parallel.ForEach(Tab_list_date, date =>
            {
                folder_days_dir.Add(get_folder + date.List_date_year + @"\" + date.List_date_month + @"\" + date.List_date_day);
            });
            Parallel.ForEach(folder_days_dir, dir =>
            {
                RadioCheck_Parrel_ForEach(dir);
            });
        }

        //Przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego dla raportów zaiks, stoart, materiały
        private void Radiocheck_zaiks_stoart_materialy_reklama(string middle_part_of_f_name, string f1_line, string file_xslt, string dest_folder, string dest_folder_ekstra, string get_folder)
        {

            Error = false;
            if (Szn_or_szn_ekstra == 1)
            {
                Fname = dest_folder + middle_part_of_f_name + Fname_part;
            }
            else if (Szn_or_szn_ekstra == 2)
            {
                Fname = dest_folder_ekstra + middle_part_of_f_name + Fname_part;
            }

            First_line = f1_line;
            F_xslt = file_xslt;
            Get_months_folders(get_folder);
        }

        //Przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego dla customowych raportów (wg klasy, nazwy, klasy i/lub nazwy, wybrany plik xslt)
        private void Radiocheck_custom_raports(bool correct, string middle_part_of_f_name, string window_middle_part_of_file_name,  string f1_line, string file_xslt, string dest_folder, string dest_folder_ekstra, string get_folder)
        {
            if (!correct)
            {
                Error = true;
                return;
            }
            else
            {
                if (Szn_or_szn_ekstra == 1)
                {
                    Fname = dest_folder + middle_part_of_f_name + window_middle_part_of_file_name + "_" + Fname_part;
                }
                else if (Szn_or_szn_ekstra == 2)
                {
                    Fname = dest_folder_ekstra + middle_part_of_f_name + window_middle_part_of_file_name + "_" + Fname_part;
                }

                First_line = f1_line;
                F_xslt = file_xslt;
                Get_months_folders(get_folder);
            }
        }
        //Sprawdzanie który rodzaj raportu został wybrany i przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego
        private void Radiocheck()
        {
            try
            {
                //Zaiks
                if (radioButton_1.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_zaiks_",
                                                      "Data|Godz.aud.|Tytul audycji|Tytul utworu|Kompozytor|Autor tekstu|Tlumacz|Czas|Wykonawca|Producent|Wydawca|",
                                                      @"raportdlazaikkopias.xslt",
                                                      Destination_folder_zaiks,
                                                      Destination_folder_ekstra_zaiks,
                                                      Get_folder_zaiks);
                }

                //Stoart
                else if (radioButton_2.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_stoart_",
                                                      "Lp|WYKONAWCA|TUTYŁ UTWORU|CZAS UTWORU|ILOŚĆ NADAŃ|TYTUŁ PŁYTY|NUMER KATALOGOWY PŁYTY|WYDAWCA|ROK WYDANIA|POLSKA/ZAGRANICA(PL/Z)|KOD ISRC|",
                                                      @"raportdlastoartkapias.xslt",
                                                      Destination_folder_stoart,
                                                      Destination_folder_ekstra_stoart,
                                                      Get_folder_stoart);
                }

                //Materiały
                else if (radioButton_5.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_materialy_",
                                                      "Data;Godz.aud.;Tytul audycji;Godz. emisji;Długość;Tytuł;Autor;",
                                                      @"raportmaterialykopia.xslt",
                                                      Destination_folder_stoart,
                                                      Destination_folder_ekstra_stoart,
                                                      Get_folder_stoart);
                }

                //Wg klasy reklama
                else if (radioButton_6.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_reklamy_",
                                                      "Data|Godz.aud.|Tytul audycji|Tytul reklamy|Kompozytor|Autor|Czas|",
                                                      @"raportreklamakapias.xslt",
                                                      Destination_folder_stoart,
                                                      Destination_folder_ekstra_stoart,
                                                      Get_folder_stoart);
                }

                //Wg własnej klasy
                else if (radioButton_7.IsChecked == true)
                {
                    Error = false;

                    Window_insert_class window_Insert_Class = new Window_insert_class();
                    window_Insert_Class.ShowDialog();

                    Radiocheck_custom_raports(window_Insert_Class.Correct,
                                              @"raport_z_klasy_",
                                              window_Insert_Class.Part_of_File_Name,
                                              "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                              @"raport_custom_class.xslt",
                                              Destination_folder_stoart,
                                              Destination_folder_ekstra_stoart,
                                              Get_folder_stoart);
                }

                //Wg nazwy
                else if (radioButton_8.IsChecked == true)
                {
                    Error = false;

                    Window_insert_name window_Insert_Name = new Window_insert_name();
                    window_Insert_Name.ShowDialog();

                    Radiocheck_custom_raports(window_Insert_Name.Correct,
                                              @"raport_z_nazwy_",
                                              window_Insert_Name.NameOfTheTitleWrittenByUser,
                                              "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                              @"raport_custom_title_name.xslt",
                                              Destination_folder_stoart,
                                              Destination_folder_ekstra_stoart,
                                              Get_folder_stoart);
                }

                //Wg klasy lub/i nazwy
                else if (radioButton_9.IsChecked == true)
                {
                    Error = false;

                    Window_custom_raport window_Custom_Raport = new Window_custom_raport();
                    window_Custom_Raport.ShowDialog();

                    Radiocheck_custom_raports(window_Custom_Raport.Correct,
                                              @"raport_z_",
                                              window_Custom_Raport.Part_of_File_Name,
                                              "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                              @"raport_custom_raport.xslt",
                                              Destination_folder_stoart,
                                              Destination_folder_ekstra_stoart,
                                              Get_folder_stoart);
                }

                //Wg wybranego raportu XSLT
                else if (radioButton_10.IsChecked == true)
                {
                    Error = false;

                    Window_XSLT_chosing window_xslt_chosing = new Window_XSLT_chosing();
                    window_xslt_chosing.ShowDialog();
                    Ze_zliczaniem_czy_bez = window_xslt_chosing.radio_int;

                    Radiocheck_custom_raports(window_xslt_chosing.Correct,
                                              @"raport_z_wybranego_wzoru_",
                                              window_xslt_chosing.Part_of_File_Name,
                                              "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                              @"raport_XSLT_chosing.xslt",
                                              Destination_folder_stoart,
                                              Destination_folder_ekstra_stoart,
                                              Get_folder_stoart);
                }

                //Brak
                else
                {
                    Window1 window1 = new Window1("Wybierz rodzaj raportu!");
                    window1.Show();
                    Error = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd:\t" + ex, "Raport Maker V3");
                FileInfo ffff = new FileInfo(Fname);
                ffff.Delete();
                Array2.Clear();
                return;
            }
        }

        //Szukanie plików z rozszerzeniem XML -> dodanie ich do listy
        private void RadioCheck_Parrel_ForEach(string dir)
        {
            string dirrr = dir + @"\Shows";
            Dayy = Directory.EnumerateFiles(dirrr, "*.xml", SearchOption.AllDirectories);
            Day = Dayy.ToArray();
            foreach (string d in Day)
            {
                Array2.Add(d);
            }
        }

        //Sprwadznie który folder jest zaznaczony i przypisywanie odpowiedniej częsci nazwy pliku wyjściowego
        private void Radiocheck_ktory_folder()
        {
            if (radioButton_3.IsChecked == true)
            {
                Get_folder_zaiks = Get_folder_szczecin_zaiks;
                Get_folder_stoart = Get_folder_szczecin_stoart;
                Fname_part = @"szczecin_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt";
                Szn_or_szn_ekstra = 1;
                Error = false;

            }
            else if (radioButton_4.IsChecked == true)
            {
                Get_folder_zaiks = Get_folder_szn_ekstra_zaiks;
                Get_folder_stoart = Get_folder_szn_ekstra_stoart;
                Fname_part = @"szczecin_FM_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt";
                Szn_or_szn_ekstra = 2;
                Error = false;
            }
            else
            {
                Window1 window1 = new Window1("Wybierz który raport!");
                window1.Show();
                Error = true;
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