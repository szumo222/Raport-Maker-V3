using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WpfApp1;

class XsltTranfromGetResult
{
    private static readonly List<string> array_of_all_files_lines = new List<string>();
    private static readonly List<string> array_of_elements = new List<string>();
    public string Main_file_xslt { get; set; }

    /* Metoda wywoływana po przeprowadzeniu transfomarty XSLT / operowanie na plikach w folderze pomocniczym raport_maker_help
        * if wichRadioButtonIsChecked = 1 - then it's raport Stoart
        * if wichRadioButtonIsChecked = 2 - then it's reklamy, własnej klasy, własnej nazwy
        * if wichRadioButtonIsChecked = else then it's zaiks, materiały */
    public static void Main_Function_After_XSLT(byte wichRadioButtonIsChecked, bool deleteAdditionalXsltFile, string Main_file_xslt, string File_name, string First_line_of_the_output_file, string Folder_for_xslt_files, List<string> Array_of_all_xml_files)
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
            if (wichRadioButtonIsChecked == 1)
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
        if (wichRadioButtonIsChecked == 1)
        {
            PrepareStoart stroartPrepare = new PrepareStoart();
            output_array = stroartPrepare.Stoart_Array_Prepare(array_of_elements, iiii);
            output_array.Insert(0, First_line_of_the_output_file);
            File.WriteAllLines(File_name + "_ze_zliczaniem.txt", output_array, Encoding.UTF8);
            array_of_elements.Insert(0, First_line_of_the_output_file);
            File.WriteAllLines(File_name, array_of_elements, Encoding.UTF8);
        }
        //Zapisanie pliku dla reklamy, własnej klasy, własnej nazwy
        else if (wichRadioButtonIsChecked == 2)
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

        if (deleteAdditionalXsltFile)
        {
            if (File.Exists(Main_file_xslt)) File.Delete(Main_file_xslt);
        }

        FileInfo f1 = new FileInfo(File_name);
        string textblock_content = "Zakończono.Plik \n\n" + f1.Name + "\n\nzostał zapisany.";
        Window1 window1 = new Window1(textblock_content);
        window1.Show();
    }
}