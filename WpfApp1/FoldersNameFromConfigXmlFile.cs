using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace WpfApp1
{
    class FoldersNameFromConfigXmlFile
    {
        public string DestinationFolder { get; private set; }
        public string GetFolderSzczecin { get; private set; }
        public string GetFolderSzczecinEkstra { get; private set; }

        //Set folder names based on configuration file config_raport_maker.xml
        public void ConfigFolderNamesFromConfigFile(bool error)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"config_raport_maker.xml");

            XmlNodeList Xml_main = doc.GetElementsByTagName("main");
            Parallel.For(0, Xml_main.Count, i =>
            {
                XmlNode xml_dest_folder = Xml_main[i].SelectSingleNode("destination_folder");
                XmlNode xml_get_folder_szczecin = Xml_main[i].SelectSingleNode("get_folder_szczecin");
                XmlNode xml_get_folder_szczecin_ekstra = Xml_main[i].SelectSingleNode("get_folder_szczecin_extra");
                DestinationFolder = xml_dest_folder.InnerText;
                GetFolderSzczecin = xml_get_folder_szczecin.InnerText;
                GetFolderSzczecinEkstra = xml_get_folder_szczecin_ekstra.InnerText;
            });

            error = CheckFolderExists(DestinationFolder,
                                GetFolderSzczecin,
                                GetFolderSzczecinEkstra);
        }

        //Check if folders exists
        private bool CheckFolderExists(string destinationFolder, string getFolderSzczecin, string getFolderSzczecinEkstra)
        {
            string[] folders_exist = { destinationFolder, getFolderSzczecin, getFolderSzczecinEkstra };

            for (int i = 0; i < folders_exist.Length; i++)
            {
                if (!Directory.Exists(folders_exist[i]))
                {
                    MessageBox.Show("Nie można znaleźć ścieżki " + folders_exist[i]);
                    return true;
                }
            }
            return false;
        }
    }
}
