namespace WpfApp1
{
    class OutputFileInfo
    {
        public string FileName { get; }
        public string FirstLineOfTheOutputFile { get; }
        public string MainFileXslt { get; }
        public OutputFileInfo(string fileName, string firstLineOfTheOutputFile, string mainFileXslt)
        {
            this.FileName = fileName;
            this.FirstLineOfTheOutputFile = firstLineOfTheOutputFile;
            this.MainFileXslt = mainFileXslt;
        }
        public OutputFileInfo() { }
    }
}
