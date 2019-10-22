using System.Collections.Generic;
using System.Windows.Controls;
using WpfApp1;

class SelectedRaport
{
    private FirstPartOfFileName firstPartOfFileName = new FirstPartOfFileName();
    private FirstPartOfFileNameForCustomRaports firstPartOfFileNameForCustomRaports = new FirstPartOfFileNameForCustomRaports();
    public SelectedRaportToReturn SelectedRaportToReturn { get; set; } = new SelectedRaportToReturn();
    private PartOfFileNameToReturn PartOfFileNameFromReturn { get; set; }
    private List<List_date> ArrayOfListDates { get; }
    private string DestinationFolder { get; }
    private AuditonFolder AuditonFolder { get; }
    public SelectedRaport(List<List_date> arrayOfListDates, string destinationFolder, AuditonFolder auditonFolder)
    {
        this.ArrayOfListDates = arrayOfListDates;
        this.DestinationFolder = destinationFolder;
        this.AuditonFolder = auditonFolder;
    }

    //Sprawdzanie który rodzaj raportu został wybrany i przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego
    public SelectedRaportToReturn WhichRaportHasBeenSelected(RadioButton radioButton_1, RadioButton radioButton_2, RadioButton radioButton_5, RadioButton radioButton_9, RadioButton radioButton_10)
    {
        //For Zaiks
        if (radioButton_1.IsChecked == true)
        {
            firstPartOfFileName = new FirstPartOfFileName(ArrayOfListDates, DestinationFolder, AuditonFolder);
            PartOfFileNameFromReturn = firstPartOfFileName.SetPartOfFileName(@"raport_zaiks_",
                                                                                "Data|Godz.aud.|Tytul audycji|Tytul utworu|Kompozytor|Autor tekstu|Tlumacz|Czas|Wykonawca|Producent|Wydawca|",
                                                                                @"raportdlazaikkopias.xslt");
            SelectedRaportToReturn.OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
            SelectedRaportToReturn.ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
        }
        //For Stoart
        else if (radioButton_2.IsChecked == true)
        {
            firstPartOfFileName = new FirstPartOfFileName(ArrayOfListDates, DestinationFolder, AuditonFolder);
            PartOfFileNameFromReturn = firstPartOfFileName.SetPartOfFileName(@"raport_stoart_",
                                                                                "Lp|WYKONAWCA|TUTYŁ UTWORU|CZAS UTWORU|ILOŚĆ NADAŃ|TYTUŁ PŁYTY|NUMER KATALOGOWY PŁYTY|WYDAWCA|ROK WYDANIA|POLSKA/ZAGRANICA(PL/Z)|KOD ISRC|",
                                                                                @"raportdlastoartkapias.xslt");
            SelectedRaportToReturn.OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
            SelectedRaportToReturn.ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
        }
        //For Materiały
        else if (radioButton_5.IsChecked == true)
        {
            firstPartOfFileName = new FirstPartOfFileName(ArrayOfListDates, DestinationFolder, AuditonFolder);
            PartOfFileNameFromReturn = firstPartOfFileName.SetPartOfFileName(@"raport_materialy_",
                                                                                "Data;Godz.aud.;Tytul audycji;Godz. emisji;Długość;Tytuł;Autor;",
                                                                                @"raportmaterialykopia.xslt");
            SelectedRaportToReturn.OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
            SelectedRaportToReturn.ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
        }
        //By class name or/and name
        else if (radioButton_9.IsChecked == true)
        {
            Window_custom_raport window_Custom_Raport = new Window_custom_raport();
            window_Custom_Raport.ShowDialog();

            firstPartOfFileNameForCustomRaports = new FirstPartOfFileNameForCustomRaports(ArrayOfListDates, DestinationFolder, AuditonFolder);
            PartOfFileNameFromReturn = firstPartOfFileNameForCustomRaports.SetPartOfFileName(@"raport_z_",
                                                                                                window_Custom_Raport.Part_of_File_Name,
                                                                                                "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                                                                                @"raport_custom_raport.xslt");
            SelectedRaportToReturn.OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
            SelectedRaportToReturn.ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
        }
        //By selected raport XSLT
        else if (radioButton_10.IsChecked == true)
        {
            Window_XSLT_chosing window_xslt_chosing = new Window_XSLT_chosing();
            window_xslt_chosing.ShowDialog();
            SelectedRaportToReturn.CustomRaportWithCalculatingOrNo = window_xslt_chosing.RadioButtonWithSummary;

            firstPartOfFileNameForCustomRaports = new FirstPartOfFileNameForCustomRaports(ArrayOfListDates, DestinationFolder, AuditonFolder);
            PartOfFileNameFromReturn = firstPartOfFileNameForCustomRaports.SetPartOfFileName(@"raport_z_wybranego_wzoru_",
                                                                                                window_xslt_chosing.Part_of_File_Name,
                                                                                                "Data|Godz.aud.|Tytul audycji|Tytul elementu|Kompozytor|Autor|Czas|",
                                                                                                @"raport_XSLT_chosing.xslt");
            SelectedRaportToReturn.OutputFileInfo = PartOfFileNameFromReturn.OutputFileInfoToReturn;
            SelectedRaportToReturn.ArrayOfAllXmlFiles = PartOfFileNameFromReturn.ArrayToReturn;
        }
        else
        {
            Window1 window1 = new Window1("Wybierz rodzaj raportu!");
            window1.Show();
            SelectedRaportToReturn.Error = true;
            SelectedRaportToReturn.ArrayOfAllXmlFiles = null;
            SelectedRaportToReturn.OutputFileInfo = null;
        }
        return SelectedRaportToReturn;
    }
}