using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Path = System.IO.Path;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        private bool text_box_text_change_flag = false;
        private bool text_box_2_text_change_flag = false;
        private readonly string folder_for_xslt_files = @"raport_maker_help\";
        public int custom_raport_with_calculating_or_no = 0;
        private AuditonFolder auditonFolder = new AuditonFolder();
        private OutputFileInfo OutputFileInfo { get; set; } = new OutputFileInfo();
        private readonly FoldersNameFromConfigXmlFile foldersNameFromConfigXmlFile = new FoldersNameFromConfigXmlFile();

        List<List_date> Array_of_list_date { get; set; } = new List<List_date>();
        public bool Overwrite_the_file_flag { get; set; }
        public string[] Days { get; set; }
        public bool Error { get; set; }
        public List<string> Array_of_all_xml_files { get; set; } = new List<string>();
        

        public MainWindow()
        {
            
            InitializeComponent();
            radioButton_1.IsChecked = false;
            grid_main.Visibility = Visibility.Hidden;
            grid_start.Visibility = Visibility.Visible;
            radioButton_2.IsChecked = radioButton_3.IsChecked = radioButton_4.IsChecked = radioButton_5.IsChecked = radioButton_6.IsChecked = radioButton_7.IsChecked = radioButton_8.IsChecked = false;
            TextBlock_1.Text = "Aplikacja tworzy raporty z programu DigAIRange." +
                "\n\nW następnym oknie należy wybrać:" +
                "\n\n\tdatę początkową" +
                "\n\tdatę końcową" +
                "\n\trodzaj raportu" +
                "\n\taudycję" +
                "\n\nDziękuję.";
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            if(!Directory.Exists(folder_for_xslt_files))
            {
                Directory.CreateDirectory(folder_for_xslt_files);
            }
            DirectoryInfo di = new DirectoryInfo(folder_for_xslt_files);
            Parallel.ForEach(di.GetFiles(), file =>
            {
                file.Delete();
            });
        }

        //Selektor kalendarza
        private void MonthlyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_1.Text = DataPicker_1.SelectedDate.Value.ToString("d MMMM yyyy");
            text_box_text_change_flag = true;
        }

        //Selektor kalendarza
        private void MonthlyCalendar_2_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_2.Text = DataPicker_2.SelectedDate.Value.ToString("d MMMM yyyy");
            text_box_2_text_change_flag = true;
        }

        //Główny przycisk
        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Error = false;
                if ((text_box_text_change_flag == false) || (text_box_2_text_change_flag == false))
                {
                    Window1 window1 = new Window1("Ustaw datę!");
                    window1.ShowDialog();
                    return;
                }
                foldersNameFromConfigXmlFile.ConfigFolderNamesFromConfigFile(Error);
                if (Error == true)
                {
                    ErrorSetGUIElements();
                }
                DateRange dateRange = new DateRange();
                Array_of_list_date = dateRange.MonthRange(DataPicker_1, DataPicker_2);
                Radiocheck_ktory_folder();
                //Przerwanie programu gdy pojawi się error
                if (Error == true)
                {
                    ErrorSetGUIElements();
                }
                else
                {
                    Radiocheck();
                    //Przerwanie programu gdy pojawi się Error
                    if (Error == true)
                    {
                        ErrorSetGUIElements();
                    }
                    else
                    {
                        List<string> array3 = new List<string>();
                        List<string> array4 = new List<string>();
                        Overwrite_the_file_flag = false;
                        int z = 0;
                        //Sprawdzanie czy dany plik wyjściowy już istnieje
                        if (File.Exists(OutputFileInfo.FileName))
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
                                FileInfo fff = new FileInfo(OutputFileInfo.FileName);
                                fff.Delete();
                                Overwrite_the_file_flag = true;
                            }
                        }
                        else 
                            Overwrite_the_file_flag = true;

                        if (Overwrite_the_file_flag == true)
                        {
                            List<string> path = new List<string>();
                            foreach (string file in Array_of_all_xml_files)
                            {
                                path.Add(Path.GetFileNameWithoutExtension(file));
                            }
                            DirectoryInfo di = new DirectoryInfo(folder_for_xslt_files);
                            Parallel.ForEach(di.GetFiles(), file =>
                            {
                                file.Delete();
                            });
                            //Osbługa transformaty XSLT na osobnym wątku w celu nie zastygania UI
                            BackgroundWorker bw = new BackgroundWorker();
                            bw.DoWork += new DoWorkEventHandler((sender1, args) => XsltTransform.Main_Function_XSLT_Transform(Array_of_all_xml_files,
                                                                                                                              path,
                                                                                                                              OutputFileInfo.MainFileXslt,
                                                                                                                              z));

                            byte wichRadioButtonIsChecked = 0;
                            if (radioButton_2.IsChecked == true)
                                wichRadioButtonIsChecked = 1;
                            else if (radioButton_6.IsChecked == true
                                || radioButton_7.IsChecked == true
                                || radioButton_8.IsChecked == true
                                || radioButton_9.IsChecked == true
                                || (radioButton_10.IsChecked == true) && (custom_raport_with_calculating_or_no == 1))
                                wichRadioButtonIsChecked = 2;
                            else
                                wichRadioButtonIsChecked = 0;

                            bool deleteAdditionalXsltFile = false;
                            if (radioButton_7.IsChecked == true || radioButton_8.IsChecked == true || radioButton_9.IsChecked == true || radioButton_10.IsChecked == true)
                                deleteAdditionalXsltFile = true;

                            bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender1, args) =>
                            {
                                XsltTranfromGetResult.Main_Function_After_XSLT(wichRadioButtonIsChecked,
                                                                               deleteAdditionalXsltFile,
                                                                               OutputFileInfo.MainFileXslt,
                                                                               OutputFileInfo.FileName,
                                                                               OutputFileInfo.FirstLineOfTheOutputFile,
                                                                               folder_for_xslt_files,
                                                                               Array_of_all_xml_files);
                                ResetGUIElementsAfterXSLT();
                            });
                            bw.RunWorkerAsync();
                            bw.Dispose();
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
                FileInfo ffff = new FileInfo(OutputFileInfo.FileName);
                ffff.Delete();
                Array_of_all_xml_files.Clear();
                return;
            }
        }

        //Reset GUI Elements to start state
        private void ResetGUIElementsAfterXSLT()
        {
            Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = DataPicker_2.IsEnabled = true;
            ProgressBar_1.IsIndeterminate = false;
            ProgressBar_1.Opacity = 0.1;
        }

        //Resetowanie elementów okna do stanu początkowego po błędzie
        private void ErrorSetGUIElements()
        {
            Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = true;
            ProgressBar_1.IsIndeterminate = false;
            ProgressBar_1.Opacity = 0.1;
            return;
        }

        private void Get_months_folders(string get_folder)
        {
            List<string> folder_days_dir = new List<string>();
            foreach (List_date date in Array_of_list_date)
            {
                folder_days_dir.Add(get_folder + date.List_date_year + @"\" + date.List_date_month + @"\" + date.List_date_day);
                Console.WriteLine(get_folder + date.List_date_year + @"\" + date.List_date_month + @"\" + date.List_date_day);
            }
            foreach(string dir in folder_days_dir)
            {
                string dirTmp = dir + @"\Shows";
                Days = Directory.EnumerateFiles(dirTmp, "*.xml", SearchOption.AllDirectories).ToArray();
                foreach (string day in Days)
                {
                    Array_of_all_xml_files.Add(day);
                }
            }
        }

        //Przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego dla raportów zaiks, stoart, materiały
        private void Radiocheck_zaiks_stoart_materialy_reklama(string middle_part_of_f_name, string f1_line, string file_xslt, string dest_folder, string get_folder)
        {

            Error = false;
            OutputFileInfo = new OutputFileInfo(fileName: dest_folder + middle_part_of_f_name + auditonFolder.Part_of_file_name,
                                                firstLineOfTheOutputFile: f1_line,
                                                mainFileXslt: file_xslt);
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
                OutputFileInfo = new OutputFileInfo(fileName: dest_folder + middle_part_of_f_name + window_middle_part_of_file_name + "_" + auditonFolder.Part_of_file_name,
                                                    firstLineOfTheOutputFile: f1_line,
                                                    mainFileXslt: file_xslt);
                Get_months_folders(get_folder);
            }
        }

        //Sprawdzanie który rodzaj raportu został wybrany i przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego
        private void Radiocheck()
        {
            try
            {
                //For Zaiks
                if (radioButton_1.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_zaiks_",
                                                              "Data|Godz.aud.|Tytul audycji|Tytul utworu|Kompozytor|Autor tekstu|Tlumacz|Czas|Wykonawca|Producent|Wydawca|",
                                                              @"raportdlazaikkopias.xslt",
                                                              foldersNameFromConfigXmlFile.DestinationFolder,
                                                              auditonFolder.Get_source_folder);
                }
                //For Stoart
                else if (radioButton_2.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_stoart_",
                                                      "Lp|WYKONAWCA|TUTYŁ UTWORU|CZAS UTWORU|ILOŚĆ NADAŃ|TYTUŁ PŁYTY|NUMER KATALOGOWY PŁYTY|WYDAWCA|ROK WYDANIA|POLSKA/ZAGRANICA(PL/Z)|KOD ISRC|",
                                                      @"raportdlastoartkapias.xslt",
                                                      foldersNameFromConfigXmlFile.DestinationFolder,
                                                      auditonFolder.Get_source_folder);
                }
                //For Materiały
                else if (radioButton_5.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_materialy_",
                                                      "Data;Godz.aud.;Tytul audycji;Godz. emisji;Długość;Tytuł;Autor;",
                                                      @"raportmaterialykopia.xslt",
                                                      foldersNameFromConfigXmlFile.DestinationFolder,
                                                      auditonFolder.Get_source_folder);
                }
                //By class "reklama"
                else if (radioButton_6.IsChecked == true)
                {
                    Radiocheck_zaiks_stoart_materialy_reklama(@"raport_reklamy_",
                                                      "Data|Godz.aud.|Tytul audycji|Tytul reklamy|Kompozytor|Autor|Czas|",
                                                      @"raportreklamakapias.xslt",
                                                      foldersNameFromConfigXmlFile.DestinationFolder,
                                                      auditonFolder.Get_source_folder);
                }
                //By own class
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
                                              foldersNameFromConfigXmlFile.DestinationFolder,
                                              auditonFolder.Get_source_folder);
                }
                //By name
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
                                              foldersNameFromConfigXmlFile.DestinationFolder,
                                              auditonFolder.Get_source_folder);
                }
                //By class name or/and name
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
                                              foldersNameFromConfigXmlFile.DestinationFolder,
                                              auditonFolder.Get_source_folder);
                }
                //By selected raport XSLT
                else if (radioButton_10.IsChecked == true)
                {
                    Error = false;
                    Window_XSLT_chosing window_xslt_chosing = new Window_XSLT_chosing();
                    window_xslt_chosing.ShowDialog();
                    custom_raport_with_calculating_or_no = window_xslt_chosing.Radio_int;

                    Radiocheck_custom_raports(window_xslt_chosing.Correct,
                                              @"raport_z_wybranego_wzoru_",
                                              window_xslt_chosing.Part_of_File_Name,
                                              "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                              @"raport_XSLT_chosing.xslt",
                                              foldersNameFromConfigXmlFile.DestinationFolder,
                                              auditonFolder.Get_source_folder);
                }
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
                FileInfo ffff = new FileInfo(OutputFileInfo.FileName);
                ffff.Delete();
                Array_of_all_xml_files.Clear();
                return;
            }
        }

        //Check which directory is selected and set part name of the output file
        private void Radiocheck_ktory_folder()
        {
            if (radioButton_3.IsChecked == true)
            {
                AuditonFolder auditonFolderTmp = new AuditonFolder(getSourceFolder: foldersNameFromConfigXmlFile.GetFolderSzczecin,
                                                                   partOfFileName: @"szczecin_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt",
                                                                   SznOrSznEkstra: 1);
                auditonFolder = auditonFolderTmp;
                Error = false;
            }
            else if (radioButton_4.IsChecked == true)
            {
                AuditonFolder auditonFolderTmp = new AuditonFolder(getSourceFolder: foldersNameFromConfigXmlFile.GetFolderSzczecinEkstra,
                                                                   partOfFileName: @"szczecin_FM_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt",
                                                                   SznOrSznEkstra: 2);
                auditonFolder = auditonFolderTmp;
                Error = false;
            }
            else
            {
                Window1 window1 = new Window1("Wybierz który raport!");
                window1.Show();
                Error = true;
            }
        }

        //Change view to main
        private void Button_2_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Visibility = Visibility.Visible;
            grid_start.Visibility = Visibility.Hidden;
        }

        //Information
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string context_menu = "Tytył:" +
                "\tRaport Maker V3" +
                "\nAutor:" +
                "\tPatryk Szumielewicz" +
                "\nE-mail:" +
                "\tszumielewiczpatryk@gmail.com" +
                "\nWersja:" +
                "\t" + version;
            Window1 window11 = new Window1(context_menu);
            window11.Show();
        }
    }
}