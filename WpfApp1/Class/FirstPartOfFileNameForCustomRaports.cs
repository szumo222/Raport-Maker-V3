﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

//Przypisanie nazwy pierwszej częsci nazwy pliku wyjściowego dla customowych raportów(wg klasy, nazwy, klasy i/lub nazwy, wybrany plik xslt)
class FirstPartOfFileNameForCustomRaports
{
    private string[] Days { get; set; }
    public List<string> ArrayOfAllXmlFiles { get; set; } = new List<string>();
    private List<List_date> ArrayOfListDate { get; }
    private string DestinationFolder { get; }
    private AuditonFolder AuditonFolder { get; }
    public FirstPartOfFileNameForCustomRaports(List<List_date> arrayOfListDate, string destinationFolder, AuditonFolder auditonFolder)
    {
        this.ArrayOfListDate = arrayOfListDate;
        this.DestinationFolder = destinationFolder;
        this.AuditonFolder = auditonFolder;
    }
    public FirstPartOfFileNameForCustomRaports() { }

    public PartOfFileNameToReturn SetPartOfFileName(string middle_part_of_f_name,
                                                    string window_middle_part_of_file_name,
                                                    string f1_line,
                                                    string file_xslt)
    {
        PartOfFileNameToReturn partOfFileNameToReturn = new PartOfFileNameToReturn
        {
            OutputFileInfoToReturn = new OutputFileInfo(fileName: DestinationFolder + middle_part_of_f_name + window_middle_part_of_file_name + "_" + AuditonFolder.Part_of_file_name,
                                            firstLineOfTheOutputFile: f1_line,
                                            mainFileXslt: file_xslt)
        };
        List<string> folder_days_dir = new List<string>();
        foreach (List_date date in ArrayOfListDate)
        {
            folder_days_dir.Add(AuditonFolder.Get_source_folder + date.List_date_year + @"\" + date.List_date_month + @"\" + date.List_date_day);
        }
        foreach (string dir in folder_days_dir)
        {
            string dirTmp = dir + @"\Shows";
            Days = Directory.EnumerateFiles(dirTmp, "*.xml", SearchOption.AllDirectories).ToArray();
            foreach (string day in Days)
            {
                ArrayOfAllXmlFiles.Add(day);
            }
        }
        partOfFileNameToReturn.ArrayToReturn = ArrayOfAllXmlFiles;
        return partOfFileNameToReturn;
    }
}