using System;
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
using System.Windows.Input;
using System.Xml;
using System.Xml.Xsl;
using Path = System.IO.Path;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        public bool Overwrite_the_file_flag { get; set; }
        public string Main_file_xslt { get; set; }
        public string[] Day { get; set; }
        public IEnumerable<string> Dayy { get; set; }
        public string Destination_folder_from_config_file { get; set; }
        public string Get_folder_szczecin_from_config_file { get; set; }
        public string Get_folder_szczecin_extra_from_config_file { get; set; }
        public string Get_folder { get; set; }
        public string File_name { get; set; }
        public string Part_of_file_name { get; set; }
        public string Folder_for_xslt_files { get; set; } = @"raport_maker_help\";
        public int Szn_or_szn_ekstra { get; set; } = 0;
        public int Custom_raport_with_calculating_or_no { get; set; } = 0;
        public bool Error { get; set; }
        public bool Text_box_text_change_flag { get; set; } = false;
        public bool Text_box_2_text_change_flag { get; set; } = false;
        public List<string> Array_of_all_xml_files { get; set; } = new List<string>();
        public string First_line_of_the_output_file { get; set; }
        public List<List_date> Array_of_list_date { get; set; } = new List<List_date>();
        public MainWindow()
        {
            
            InitializeComponent();
            radioButton_1.IsChecked = false;
            grid_main.Visibility = Visibility.Hidden;
            grid_start.Visibility = Visibility.Visible;
            radioButton_2.IsChecked = radioButton_3.IsChecked = radioButton_4.IsChecked = radioButton_5.IsChecked = radioButton_6.IsChecked = radioButton_7.IsChecked = radioButton_8.IsChecked = false;
            TextBlock_1.Text = "Aplikacja tworzy raporty z programu DigAIRange.\n\nW następnym oknie należy wybrać:\n\n\tdatę początkową\n\tdatę końcową\n\trodzaj raportu\n\taudycję\n\nDziękuję.";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if(!Directory.Exists(Folder_for_xslt_files))
            {
                Directory.CreateDirectory(Folder_for_xslt_files);
            }
            DirectoryInfo di = new DirectoryInfo(Folder_for_xslt_files);
            Parallel.ForEach(di.GetFiles(), file =>
            {
                file.Delete();
            });
        }

        //Selektor kalendarza
        private void MonthlyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_1.Text = DataPicker_1.SelectedDate.Value.ToString("d MMMM yyyy");
            Text_box_text_change_flag = true;
        }

        //Selektor kalendarza
        private void MonthlyCalendar_2_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_2.Text = DataPicker_2.SelectedDate.Value.ToString("d MMMM yyyy");
            Text_box_2_text_change_flag = true;
        }

        //Główny przycisk
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Error = false;
                if ((Text_box_text_change_flag == false) || (Text_box_2_text_change_flag == false))
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

                DateRange dateRange = new DateRange();

                Array_of_list_date = dateRange.MonthRange(DataPicker_1, DataPicker_2);

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
                        Overwrite_the_file_flag = false;
                        int z = 0;

                        //Sprawdzanie czy dany plik wyjściowy już istnieje
                        if (File.Exists(File_name))
                        {
                            Window2 window2 = new Window2();
                            window2.ShowDialog();
                            if (!window2.Czynadpisac)
                            {
                                Overwrite_the_file_flag = false;
                                return;
                            }
                            else
                            {
                                FileInfo fff = new FileInfo(File_name);
                                fff.Delete();
                                Overwrite_the_file_flag = true;
                            }
                        }
                        else if (!File.Exists(File_name)) Overwrite_the_file_flag = true;

                        if (Overwrite_the_file_flag == true)
                        {
                            List<string> path = new List<string>();
                            foreach (string file in Array_of_all_xml_files)
                            {
                                path.Add(Path.GetFileNameWithoutExtension(file));
                            }

                            DirectoryInfo di = new DirectoryInfo(Folder_for_xslt_files);
                            Parallel.ForEach(di.GetFiles(), file =>
                            {
                                file.Delete();
                            });

                            //Osbługa transformaty XSLT na osobnym wątku w celu nie zastygania UI
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.DoWork += new DoWorkEventHandler((sender1, args) => Main_Function_XSLT_Transform(Array_of_all_xml_files, path, Main_file_xslt, z, sw));
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
                FileInfo ffff = new FileInfo(File_name);
                ffff.Delete();
                Array_of_all_xml_files.Clear();
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

            XmlNodeList Xml_main = doc.GetElementsByTagName("main");
            Parallel.For(0, Xml_main.Count, i =>
            {
                XmlNode xml_dest_folder = Xml_main[i].SelectSingleNode("destination_folder");
                XmlNode xml_get_folder_szczecin = Xml_main[i].SelectSingleNode("get_folder_szczecin");
                XmlNode xml_get_folder_szczecin_ekstra = Xml_main[i].SelectSingleNode("get_folder_szczecin_extra");
                Destination_folder_from_config_file = xml_dest_folder.InnerText;
                Get_folder_szczecin_from_config_file = xml_get_folder_szczecin.InnerText;
                Get_folder_szczecin_extra_from_config_file = xml_get_folder_szczecin_ekstra.InnerText;
            });

            Check_folders_exist();
        }

        //Sprawdzanie dostępu do folderu z plikami xml (w domyślne w sieciowej lokalizacji)
        private void Check_folders_exist()
        {
            string[] folders_exist = { Destination_folder_from_config_file, Get_folder_szczecin_from_config_file, Get_folder_szczecin_extra_from_config_file };

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
                string File_after_xslt;
                Console.WriteLine(file);
                File_after_xslt = @"raport_maker_help\" + path[z] + "_" + z + ".txt";
                xslt2.Transform(file, File_after_xslt);
                File_after_xslt = "";
                z++;
            }
        }

        //Metoda wywoływana po przeprowadzeniu transfomarty XSLT / operowanie na plikach w folderze pomocniczym raport_maker_help
        private void Main_Function_After_XSLT(List<string> array_of_all_files_lines, List<string> array_of_elements, Stopwatch sw)
        {
            IEnumerable<string> array = Directory.EnumerateFiles(@"raport_maker_help\", "*.txt", SearchOption.AllDirectories);
            foreach (string file in array)
            {
                string[] lines = File.ReadAllLines(file);
                for (int i = 0; i < lines.Length; i++)
                {
                    //Every line of every file is added to a array (list), to have possibility to sort it.
                    array_of_all_files_lines.Add(lines[i]);
                }
            }
            array_of_all_files_lines.Sort();
            int iiii = 1;
            foreach (string string_line in array_of_all_files_lines)
            {
                StringBuilder string_builder = new StringBuilder(string_line);

                // Raport Stoart
                if (radioButton_2.IsChecked == true)
                {
                    if (string_builder.Length < 2) string_builder.Remove(0, string_builder.Length);
                    if (string_builder.Length >= 2) string_builder.Remove(0, 6);
                    if (string_builder.Length != 0) { string_builder.Insert(0, iiii.ToString() + "|"); iiii++; }
                    array_of_elements.Add(string_builder.ToString());
                }

                //Raporty zaiks, materiały
                else
                {
                    if (string_builder.Length < 2) string_builder.Remove(0, string_builder.Length);
                    if (string_builder.Length >= 2) string_builder.Remove(17, 6);
                    array_of_elements.Add(string_builder.ToString());
                }
            }
            iiii = 1;
            //Czyszczenie listy z pustych wierszy
            for (int i = array_of_elements.Count - 1; i >= 0; i--)
            {
                if (array_of_elements[i] == "")
                {
                    array_of_elements.RemoveAt(i);
                }
            }

            List<string> output_array = new List<string>();

            //Zapisanie pliku dla stoart
            if (radioButton_2.IsChecked == true)
            {
                PrepareStoart stroartPrepare = new PrepareStoart();
                output_array = stroartPrepare.Stoart_Array_Prepare(array_of_elements, iiii);
                output_array.Insert(0, First_line_of_the_output_file);
                File.WriteAllLines(File_name + "_ze_zliczaniem.txt", output_array, Encoding.UTF8);
                array_of_elements.Insert(0, First_line_of_the_output_file);
                File.WriteAllLines(File_name, array_of_elements, Encoding.UTF8);
            }
            //Zapisanie pliku dla reklamy, własnej klasy, własnej nazwy
            else if ((radioButton_6.IsChecked == true) || (radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true) || (radioButton_9.IsChecked == true) || ((radioButton_10.IsChecked == true) && (Custom_raport_with_calculating_or_no == 1)))
            {
                PrepareWithElemntsSummary withSummaryElementsPrepare = new PrepareWithElemntsSummary();
                output_array = withSummaryElementsPrepare.Summary_Elements_Array_Prepare(array_of_elements);
                output_array.Insert(0, First_line_of_the_output_file);
                File.WriteAllLines(File_name, output_array, Encoding.UTF8);
            }
            //Zapisanie pliku dla zaiks, materiały
            else
            {
                array_of_elements.Insert(0, First_line_of_the_output_file);
                File.WriteAllLines(File_name, array_of_elements, Encoding.UTF8);
            }

            DirectoryInfo di = new DirectoryInfo(Folder_for_xslt_files);
            Array_of_all_xml_files.Clear();
            array_of_all_files_lines.Clear();
            array_of_elements.Clear();
            output_array.Clear();
            Parallel.ForEach(di.GetFiles(), file =>
            {
                file.Delete();
            });

            if((radioButton_7.IsChecked == true) || (radioButton_8.IsChecked == true) || (radioButton_9.IsChecked == true) || (radioButton_10.IsChecked == true))
            {
                if (File.Exists(Main_file_xslt)) File.Delete(Main_file_xslt);
            }

            FileInfo f1 = new FileInfo(File_name);
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
            Parallel.ForEach(Array_of_list_date, date =>
            {
                folder_days_dir.Add(get_folder + date.List_date_year + @"\" + date.List_date_month + @"\" + date.List_date_day);
            });
            Parallel.ForEach(folder_days_dir, dir =>
            {
                RadioCheck_Parrel_ForEach(dir);
            });
        }

        //Przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego dla raportów zaiks, stoart, materiały
        private void Radiocheck_zaiks_stoart_materialy_reklama(string middle_part_of_f_name, string f1_line, string file_xslt, string dest_folder, string get_folder)
        {

            Error = false;
            File_name = dest_folder + middle_part_of_f_name + Part_of_file_name;

            First_line_of_the_output_file = f1_line;
            Main_file_xslt = file_xslt;
            Get_months_folders(get_folder);
        }

        //Przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego dla customowych raportów (wg klasy, nazwy, klasy i/lub nazwy, wybrany plik xslt)
        private void Radiocheck_custom_raports(bool correct, string middle_part_of_f_name, string window_middle_part_of_file_name,  string f1_line, string file_xslt, string dest_folder, string get_folder)
        {
            if (!correct)
            {
                Error = true;
                return;
            }
            else
            {
                File_name = dest_folder + middle_part_of_f_name + window_middle_part_of_file_name + "_" + Part_of_file_name;

                First_line_of_the_output_file = f1_line;
                Main_file_xslt = file_xslt;
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
                                                      Destination_folder_from_config_file,
                                                      Get_folder);
                }

                //Stoart
                else if (radioButton_2.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_stoart_",
                                                      "Lp|WYKONAWCA|TUTYŁ UTWORU|CZAS UTWORU|ILOŚĆ NADAŃ|TYTUŁ PŁYTY|NUMER KATALOGOWY PŁYTY|WYDAWCA|ROK WYDANIA|POLSKA/ZAGRANICA(PL/Z)|KOD ISRC|",
                                                      @"raportdlastoartkapias.xslt",
                                                      Destination_folder_from_config_file,
                                                      Get_folder);
                }

                //Materiały
                else if (radioButton_5.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_materialy_",
                                                      "Data;Godz.aud.;Tytul audycji;Godz. emisji;Długość;Tytuł;Autor;",
                                                      @"raportmaterialykopia.xslt",
                                                      Destination_folder_from_config_file,
                                                      Get_folder);
                }

                //Wg klasy reklama
                else if (radioButton_6.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_reklamy_",
                                                      "Data|Godz.aud.|Tytul audycji|Tytul reklamy|Kompozytor|Autor|Czas|",
                                                      @"raportreklamakapias.xslt",
                                                      Destination_folder_from_config_file,
                                                      Get_folder);
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
                                              Destination_folder_from_config_file,
                                              Get_folder);
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
                                              Destination_folder_from_config_file,
                                              Get_folder);
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
                                              Destination_folder_from_config_file,
                                              Get_folder);
                }

                //Wg wybranego raportu XSLT
                else if (radioButton_10.IsChecked == true)
                {
                    Error = false;

                    Window_XSLT_chosing window_xslt_chosing = new Window_XSLT_chosing();
                    window_xslt_chosing.ShowDialog();
                    Custom_raport_with_calculating_or_no = window_xslt_chosing.Radio_int;

                    Radiocheck_custom_raports(window_xslt_chosing.Correct,
                                              @"raport_z_wybranego_wzoru_",
                                              window_xslt_chosing.Part_of_File_Name,
                                              "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                              @"raport_XSLT_chosing.xslt",
                                              Destination_folder_from_config_file,
                                              Get_folder);
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
                FileInfo ffff = new FileInfo(File_name);
                ffff.Delete();
                Array_of_all_xml_files.Clear();
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
                Array_of_all_xml_files.Add(d);
            }
        }

        //Sprwadznie który folder jest zaznaczony i przypisywanie odpowiedniej częsci nazwy pliku wyjściowego
        private void Radiocheck_ktory_folder()
        {
            if (radioButton_3.IsChecked == true)
            {
                Get_folder = Get_folder_szczecin_from_config_file;
                Part_of_file_name = @"szczecin_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt";
                Szn_or_szn_ekstra = 1;
                Error = false;

            }
            else if (radioButton_4.IsChecked == true)
            {
                Get_folder = Get_folder_szczecin_extra_from_config_file;
                Part_of_file_name = @"szczecin_FM_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt";
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