using System.Collections.Generic;

class SelectedRaportToReturn
{
    public OutputFileInfo OutputFileInfo { get; set; }
    public List<string> ArrayOfAllXmlFiles { get; set; }
    public bool Error { get; set; } = false;
    public bool CustomRaportWithCalculatingOrNo { get; set; } = false;
}