using System.Windows.Controls;
using WpfApp1;

class SelectedAuditionFolder
{
    public static SelectedAuditionFolderToReturn SelectedAuditionFolderToReturn { get; set; } = new SelectedAuditionFolderToReturn();
    private static AuditonFolder AuditonFolder {get; set;}

    //Check which directory is selected and set part name of the output file
    public static SelectedAuditionFolderToReturn WhichAuditionFolderIsSelected(RadioButton radioButton_3, RadioButton radioButton_4, TextBox TextBox_1, TextBox TextBox_2, FoldersNameFromConfigXmlFile foldersNameFromConfigXmlFile)
    {
        if (radioButton_3.IsChecked == true)
        {
            AuditonFolder = new AuditonFolder(foldersNameFromConfigXmlFile.GetFolderSzczecin,
                                                               @"szczecin_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt",
                                                               1);
            SelectedAuditionFolderToReturn.auditonFolder = AuditonFolder;
            SelectedAuditionFolderToReturn.Error = false;
        }
        else if (radioButton_4.IsChecked == true)
        {
            AuditonFolder = new AuditonFolder(foldersNameFromConfigXmlFile.GetFolderSzczecinEkstra,
                                                               @"szczecin_FM_" + TextBox_1.Text + "_" + TextBox_2.Text + ".txt",
                                                               2);
            SelectedAuditionFolderToReturn.auditonFolder = AuditonFolder;
            SelectedAuditionFolderToReturn.Error = false;
        }
        else
        {
            Window1 window1 = new Window1("Wybierz który raport!");
            window1.Show();
            SelectedAuditionFolderToReturn.auditonFolder = null;
            SelectedAuditionFolderToReturn.Error = true;
        }
        return SelectedAuditionFolderToReturn;
    }
}