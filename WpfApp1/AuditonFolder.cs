namespace WpfApp1
{
    class AuditonFolder
    {
        public string Get_source_folder { get; }
        public string Part_of_file_name { get; }
        public int Szn_or_szn_ekstra { get; } = 0;
        public AuditonFolder(string getSourceFolder, string partOfFileName, int SznOrSznEkstra)
        {
            this.Get_source_folder = getSourceFolder;
            this.Part_of_file_name = partOfFileName;
            this.Szn_or_szn_ekstra = SznOrSznEkstra;
        }
        public AuditonFolder() { }
    }
}