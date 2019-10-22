using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        private FirstPartOfFileNameForCustomRaports firstPartOfFileNameForCustomRaports = new FirstPartOfFileNameForCustomRaports();
        private FirstPartOfFileName firstPartOfFileName = new FirstPartOfFileName();
        private PartOfFileNameToReturn PartOfFileNameFromReturn { get; set; }
        private SelectedAuditionFolderToReturn SelectedAuditionFolderFromReturn { get; set; }
        List<List_date> ArrayOfListDates { get; set; } = new List<List_date>();
        public bool OverwrtieFileFlag { get; set; }
        public bool Error { get; set; }
        public List<string> ArrayOfAllXmlFiles { get; set; } = new List<string>();

        public MainWindow()
        {
            
            InitializeComponent();
            radioButton_1.IsChecked = false;
            grid_main.Visibility = Visibility.Hidden;
            grid_start.Visibility = Visibility.Visible;
            radioButton_2.IsChecked = radioButton_3.IsChecked = radioButton_4.IsChecked = radioButton_5.IsChecked = false;
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
                if (Error)
                {
                    ResetGUIElementsAfterXSLT();
                    return;
                }
                DateRange dateRange = new DateRange();
                ArrayOfListDates = dateRange.MonthRange(DataPicker_1, DataPicker_2);
                SelectedAuditionFolderFromReturn = SelectedAuditionFolder.WhichAuditionFolderIsSelected(radioButton_3, radioButton_4, TextBox_1, TextBox_2, foldersNameFromConfigXmlFile);
                if (SelectedAuditionFolderFromReturn.Error == true || SelectedAuditionFolderFromReturn.auditonFolder == null)
                {
                    Error = true;
                    ResetGUIElementsAfterXSLT();
                    return;
                }
                auditonFolder = SelectedAuditionFolderFromReturn.auditonFolder;
                Error = SelectedAuditionFolderFromReturn.Error;
                WhichRaportHasBeenSelected();
                if (Error)
                {
                    ResetGUIElementsAfterXSLT();
                    return;
                }
                List<string> array3 = new List<string>();
                List<string> array4 = new List<string>();
                OverwrtieFileFlag = true;
                int z = 0;

                //Sprawdzanie czy dany plik wyjściowy już istnieje
                if (File.Exists(OutputFileInfo.FileName))
                {
                    Window2 window2 = new Window2();
                    window2.ShowDialog();
                    if (!window2.Czynadpisac)
                    {
                        OverwrtieFileFlag = false;
                        return;
                    }
                    FileInfo fff = new FileInfo(OutputFileInfo.FileName);
                    fff.Delete();
                }
                if (OverwrtieFileFlag)
                {
                    List<string> path = new List<string>();
                    foreach (string file in ArrayOfAllXmlFiles)
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
                    bw.DoWork += new DoWorkEventHandler((sender1, args) => XsltTransform.DoXsltTransform(ArrayOfAllXmlFiles,
                                                                                                                        path,
                                                                                                                        OutputFileInfo.MainFileXslt,
                                                                                                                        z));

                    byte wichRadioButtonIsChecked = 0;
                    if (radioButton_2.IsChecked == true)
                        wichRadioButtonIsChecked = 1;
                    else if (radioButton_9.IsChecked == true
                        || (radioButton_10.IsChecked == true) && (custom_raport_with_calculating_or_no == 1))
                        wichRadioButtonIsChecked = 2;

                    bool deleteAdditionalXsltFile = false;
                    if (radioButton_9.IsChecked == true || radioButton_10.IsChecked == true)
                        deleteAdditionalXsltFile = true;

                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler((sender1, args) =>
                    {
                        XsltTranfromGetResult.GetXsltResults(wichRadioButtonIsChecked,
                                                                        deleteAdditionalXsltFile,
                                                                        OutputFileInfo.MainFileXslt,
                                                                        OutputFileInfo.FileName,
                                                                        OutputFileInfo.FirstLineOfTheOutputFile,
                                                                        folder_for_xslt_files,
                                                                        ArrayOfAllXmlFiles);
                        ResetGUIElementsAfterXSLT();
                    });
                    bw.RunWorkerAsync();
                    bw.Dispose();
                    ProgressBar_1.IsIndeterminate = true;
                    ProgressBar_1.Opacity = 100;
                    Button_1.IsEnabled = groupBox_1.IsEnabled = groupBox_2.IsEnabled = DataPicker_1.IsEnabled = DataPicker_2.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd:\t" + ex, "Raport Maker V3");
                FileInfo ffff = new FileInfo(OutputFileInfo.FileName);
                ffff.Delete();
                ArrayOfAllXmlFiles.Clear();
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

        //Sprawdzanie który rodzaj raportu został wybrany i przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego
        private void WhichRaportHasBeenSelected()
        {
            try
            {
                //For Zaiks
                if (radioButton_1.IsChecked == true)
                {
                    firstPartOfFileName = new FirstPartOfFileName(ArrayOfListDates, foldersNameFromConfigXmlFile.DestinationFolder, auditonFolder);
                    PartOfFileNameFromReturn = firstPartOfFileName.SetPartOfFileName(@"raport_zaiks_",
                                                                                     "Data|Godz.aud.|Tytul audycji|Tytul utworu|Kompozytor|Autor tekstu|Tlumacz|Czas|Wykonawca|Producent|Wydawca|",
                                                                                     @"raportdlazaikkopias.xslt");
                    OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
                    ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
                }
                //For Stoart
                else if (radioButton_2.IsChecked == true)
                {
                    firstPartOfFileName = new FirstPartOfFileName(ArrayOfListDates, foldersNameFromConfigXmlFile.DestinationFolder, auditonFolder);
                    PartOfFileNameFromReturn = firstPartOfFileName.SetPartOfFileName(@"raport_stoart_",
                                                                                     "Lp|WYKONAWCA|TUTYŁ UTWORU|CZAS UTWORU|ILOŚĆ NADAŃ|TYTUŁ PŁYTY|NUMER KATALOGOWY PŁYTY|WYDAWCA|ROK WYDANIA|POLSKA/ZAGRANICA(PL/Z)|KOD ISRC|",
                                                                                     @"raportdlastoartkapias.xslt");
                    OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
                    ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
                }
                //For Materiały
                else if (radioButton_5.IsChecked == true)
                {
                    firstPartOfFileName = new FirstPartOfFileName(ArrayOfListDates, foldersNameFromConfigXmlFile.DestinationFolder, auditonFolder);
                    PartOfFileNameFromReturn = firstPartOfFileName.SetPartOfFileName(@"raport_materialy_",
                                                                                     "Data;Godz.aud.;Tytul audycji;Godz. emisji;Długość;Tytuł;Autor;",
                                                                                     @"raportmaterialykopia.xslt");
                    OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
                    ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
                }
                //By class name or/and name
                else if (radioButton_9.IsChecked == true)
                {
                    Error = false;
                    Window_custom_raport window_Custom_Raport = new Window_custom_raport();
                    window_Custom_Raport.ShowDialog();

                    firstPartOfFileNameForCustomRaports = new FirstPartOfFileNameForCustomRaports(ArrayOfListDates, foldersNameFromConfigXmlFile.DestinationFolder, auditonFolder);
                    PartOfFileNameFromReturn = firstPartOfFileNameForCustomRaports.SetPartOfFileName(@"raport_z_",
                                                                                                     window_Custom_Raport.Part_of_File_Name,
                                                                                                     "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                                                                                     @"raport_custom_raport.xslt");
                    OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
                    ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
                }
                //By selected raport XSLT
                else if (radioButton_10.IsChecked == true)
                {
                    Error = false;
                    Window_XSLT_chosing window_xslt_chosing = new Window_XSLT_chosing();
                    window_xslt_chosing.ShowDialog();
                    custom_raport_with_calculating_or_no = window_xslt_chosing.Radio_int;

                    firstPartOfFileNameForCustomRaports = new FirstPartOfFileNameForCustomRaports(ArrayOfListDates, foldersNameFromConfigXmlFile.DestinationFolder, auditonFolder);
                    PartOfFileNameFromReturn = firstPartOfFileNameForCustomRaports.SetPartOfFileName(@"raport_z_wybranego_wzoru_",
                                                                                                     window_xslt_chosing.Part_of_File_Name,
                                                                                                     "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                                                                                     @"raport_XSLT_chosing.xslt");
                    OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
                    ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
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
                ArrayOfAllXmlFiles.Clear();
                return;
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