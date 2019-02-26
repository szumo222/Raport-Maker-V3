using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Xsl;
using Path = System.IO.Path;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string miesiac;
        string rok;
        bool czynadpisac;
        string f_xslt;
        string[] day;
        string destination_folder_zaiks;
        string destination_folder_stoart;
        string destination_folder_ekstra_zaiks;
        string destination_folder_ekstra_stoart;
        string get_folder_szczecin_zaiks;
        string get_folder_szczecin_stoart;
        string get_folder_szn_ekstra_zaiks;
        string get_folder_szn_ekstra_stoart;
        string get_folder_zaiks;
        string get_folder_stoart;
        string fname;
        string fname_part;
        int szn_or_szn_ekstra = 0;
        bool error = false;
        bool text_box_text_change = false;
        List<string> array2 = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            radioButton_1.IsChecked = false;
            radioButton_2.IsChecked = false;
            radioButton_3.IsChecked = false;
            radioButton_4.IsChecked = false;
        }

        private void MonthlyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBox_1.Text = DataPicker_1.SelectedDate.Value.ToString("MMMM");
            TextBox_2.Text = DataPicker_1.SelectedDate.Value.ToString("yyyy");
            text_box_text_change = true;
        }

        private void Button_1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (text_box_text_change == false)
                {
                    MessageBox.Show("Ustaw datę!", "Raport Maker V2");
                    return;
                }
                ProgressBar_1.Opacity = 100;
                ProgressBar_1.IsIndeterminate = true;
                XmlDocument doc = new XmlDocument();
                doc.Load(@"config_raport_maker.xml");
                XmlNodeList xml_zaiks = doc.GetElementsByTagName("zaiks");

                Parallel.For(0, xml_zaiks.Count, i=>
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

                /*for (int i = 0; i < xml_zaiks.Count; i++)
                {
                    XmlNode xml_dest_folder_zaiks = xml_zaiks[i].SelectSingleNode("destination_folder");
                    XmlNode xml_dest_folder_ekstra_zaiks = xml_zaiks[i].SelectSingleNode("destination_folder_ekstra");
                    XmlNode xml_get_folder_zaiks = xml_zaiks[i].SelectSingleNode("get_folder_szczecin");
                    XmlNode xml_get_folder_x_zaiks = xml_zaiks[i].SelectSingleNode("get_folder_x");
                    destination_folder_zaiks = xml_dest_folder_zaiks.InnerText;
                    destination_folder_ekstra_zaiks = xml_dest_folder_ekstra_zaiks.InnerText;
                    get_folder_szczecin_zaiks = xml_get_folder_zaiks.InnerText;
                    get_folder_szn_ekstra_zaiks = xml_get_folder_x_zaiks.InnerText;
                }*/
                XmlNodeList xml_stoart = doc.GetElementsByTagName("stoart");

                Parallel.For(0, xml_stoart.Count, i=>
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

                /*for (int i = 0; i < xml_stoart.Count; i++)
                {
                    XmlNode xml_dest_folder_stoart = xml_stoart[i].SelectSingleNode("destination_folder");
                    XmlNode xml_dest_folder_ekstra_stoart = xml_stoart[i].SelectSingleNode("destination_folder_ekstra");
                    XmlNode xml_get_folder_stoart = xml_stoart[i].SelectSingleNode("get_folder_szczecin");
                    XmlNode xml_get_folder_x_stoart = xml_stoart[i].SelectSingleNode("get_folder_x");
                    destination_folder_stoart = xml_dest_folder_stoart.InnerText;
                    destination_folder_ekstra_stoart = xml_dest_folder_ekstra_stoart.InnerText;
                    get_folder_szczecin_stoart = xml_get_folder_stoart.InnerText;
                    get_folder_szn_ekstra_stoart = xml_get_folder_x_stoart.InnerText;
                }*/

                miesiac = DataPicker_1.SelectedDate.Value.Month.ToString();
                rok = DataPicker_1.SelectedDate.Value.Year.ToString();

                Radiocheck_ktory_folder();
                Radiocheck();

                if (error == true) return;

                List<string> array3 = new List<string>();
                List<string> array4 = new List<string>();
                czynadpisac = false;
                int z = 0;

                if (File.Exists(fname))
                {
                    if (MessageBox.Show("Raport już istnieje\nCzy chcesz go nadpisać?", "Raport Maker V2", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        czynadpisac = false;
                        return;
                    }
                    else
                    {
                        FileInfo fff = new FileInfo(fname);
                        fff.Delete();
                        czynadpisac = true;
                    }/*
                    DialogResult dr = MessageBox.Show("Raport już istnieje\nCzy chcesz go nadpisać?", "Raport Maker V2", MessageBoxButtons.YesNo);
                    switch (dr)
                    {
                        case DialogResult.Yes:
                            FileInfo fff = new FileInfo(fname);
                            fff.Delete();
                            czynadpisac = true;
                            break;
                        case DialogResult.No:
                            czynadpisac = false;
                            return;
                    }*/
                }
                else if (!File.Exists(fname)) czynadpisac = true;

                if (czynadpisac == true)
                {
                    Parallel.ForEach(array2, file =>
                    {
                        XslCompiledTransform xslt2 = new XslCompiledTransform();
                        xslt2.Load(f_xslt);
                        string path = Path.GetFileNameWithoutExtension(file);
                        string f_out2 = @"raport_maker_help\" + path + "_" + z + ".txt";
                        xslt2.Transform(file, f_out2);
                        z++;
                    });
                    /*foreach (string file in array2)
                    {
                        XslCompiledTransform xslt2 = new XslCompiledTransform();
                        xslt2.Load(f_xslt);
                        string path = Path.GetFileNameWithoutExtension(file);
                        string f_out2 = @"raport_maker_help\" + path + "_" + z + ".txt";
                        xslt2.Transform(file, f_out2);
                        z++;
                    }*/
                    string[] array = Directory.GetFiles(@"raport_maker_help\", "*.txt", SearchOption.AllDirectories);

                    Parallel.ForEach(array, file =>
                    {
                        string[] lines = File.ReadAllLines(file);
                        for(int i = 0; i < lines.Length; i++)
                        {
                            array3.Add(lines[i]);
                        }
                    });

                    /*
                    foreach (string file in array)
                    {
                        string[] lines = File.ReadAllLines(file);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            array3.Add(lines[i]);
                        }
                    }*/
                    array3.Sort();
                    array4.Add("Data|Godz.aud.|Tytul audycji|Tytul utworu|Kompozytor|Autor tekstu|Tlumacz|Czas|Wykonawca|Producent|Wydawca|");

                    Parallel.ForEach(array3, s =>
                    {
                        StringBuilder ss = new StringBuilder(s);
                        ss.Remove(17, 6);
                        array4.Add(ss.ToString());
                        //array4.Add(s);
                    });

                    /*foreach (string s in array3)
                    {
                        StringBuilder ss = new StringBuilder(s);
                        ss.Remove(17, 6);
                        array4.Add(ss.ToString());
                        //array4.Add(s);
                    }*/
                    File.WriteAllLines(fname, array4, Encoding.UTF8);

                    string dsa = @"raport_maker_help\";
                    DirectoryInfo di = new DirectoryInfo(dsa);
                    array2.Clear();
                    array3.Clear();
                    array4.Clear();

                    Parallel.ForEach(di.GetFiles(), file =>
                    {
                        file.Delete();
                    });
                    /*foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }*/
                }
                else
                {
                    MessageBox.Show("Wybierz folder do zapisu!", "Raport Maker V2");
                    return;
                }
                FileInfo f1 = new FileInfo(fname);
                ProgressBar_1.Opacity = 30;
                ProgressBar_1.IsIndeterminate = false;
                MessageBox.Show("Zakończono. Plik \n\n" + f1.Name + "\n\nzostał zapisany.", "Raport Maker V2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex);
                FileInfo ffff = new FileInfo(fname);
                ffff.Delete();
                array2.Clear();
            }
        }

        private void Radiocheck()
        {
            //Zaiks
            if (radioButton_1.IsChecked == true)
            {
                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_zaiks + @"raport_zaiks_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_zaiks + @"raport_zaiks_" + fname_part;
                }

                f_xslt = @"raportdlazaikkopias.xslt";
                string[] folder_days_zaiks_dir = Directory.GetDirectories(get_folder_zaiks + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_zaiks_dir, dir =>
                {
                    day = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);
                    foreach (string d in day)
                    {
                        array2.Add(d);
                    }
                });

                /*foreach (string dir in folder_days_zaiks_dir)
                {
                    day = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);
                    foreach (string d in day)
                    {
                        array2.Add(d);
                    }
                }*/
            }

            //Stoart
            else if (radioButton_2.IsChecked == true)
            {
                if (szn_or_szn_ekstra == 1)
                {
                    fname = destination_folder_stoart + @"raport_stoart_" + fname_part;
                }
                else if (szn_or_szn_ekstra == 2)
                {
                    fname = destination_folder_ekstra_stoart + @"raport_stoart_" + fname_part;
                }

                f_xslt = @"raportdlastoartkapias.xslt";
                string[] folder_days_stoart_dir = Directory.GetDirectories(get_folder_stoart + rok + @"\" + miesiac + @"\");

                Parallel.ForEach(folder_days_stoart_dir, dir =>
                {
                    day = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);
                    foreach (string d in day)
                    {
                        array2.Add(d);
                    }
                });

                /*foreach (string dir in folder_days_stoart_dir)
                {
                    day = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);
                    foreach (string d in day)
                    {
                        array2.Add(d);
                    }
                }*/
            }

            //Brak
            else
            {
                MessageBox.Show("Wybierz rodzaj raportu!", "Raport Maker V2");
                error = true;
            }

        }

        private void Radiocheck_ktory_folder()
        {
            if (radioButton_4.IsChecked == true)
            {
                get_folder_zaiks = get_folder_szczecin_zaiks;
                get_folder_stoart = get_folder_szczecin_stoart;
                fname_part = @"szczecin_" + miesiac + "_" + rok + ".txt";
                szn_or_szn_ekstra = 1;

            }
            else if (radioButton_3.IsChecked == true)
            {
                get_folder_zaiks = get_folder_szn_ekstra_zaiks;
                get_folder_stoart = get_folder_szn_ekstra_stoart;
                fname_part = @"szczecin_ekstra_" + miesiac + "_" + rok + ".txt";
                szn_or_szn_ekstra = 2;
            }
            else
            {
                MessageBox.Show("Wybierz który folder!", "Raport Maker V2");
                error = true;
            }
        }
    }
}