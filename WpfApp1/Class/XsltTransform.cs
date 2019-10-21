using System.Collections.Generic;
using System.Xml.Xsl;

class XsltTransform
{
    //Metoda odpowiadająca za transformatę XSLT
    public static void Main_Function_XSLT_Transform(List<string> array2, List<string> path, string f_xslt, int z)
    {
        XslCompiledTransform xslt2 = new XslCompiledTransform();
        xslt2.Load(f_xslt);
        foreach (string file in array2)
        {
            string File_after_xslt;
            //Console.WriteLine(file);
            File_after_xslt = @"raport_maker_help\" + path[z] + "_" + z + ".txt";
            xslt2.Transform(file, File_after_xslt);
            File_after_xslt = "";
            z++;
        }
    }
}