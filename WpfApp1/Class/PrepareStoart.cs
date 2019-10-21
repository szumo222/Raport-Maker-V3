using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

class PrepareStoart
{
    private List<string> Array_help = new List<string>();
    private List<string> Array_returned = new List<string>();
    private string Linia_liczba_odt_1;
    private string Linia_liczba_odt_2;
    private int Odt = 1;

    //Przygotowanie listy do zapisu dla Stoart
    public List<string> Stoart_Array_Prepare(List<string> array, int iiii)
    {

        //Usuwanie liczby Lp. na początku 
        foreach (string line in array)
        {
            string replace = Regex.Replace(line, @"^\d{1,}\|", "");
            Array_help.Add(replace);
        }
        Array_help.Sort();

        //Liczenie takich samych wierszy
        for (int i = 0; i < Array_help.Count; i++)
        {
            //Dla całej listy bez ostatniego elementu
            if (i < Array_help.Count() - 1)
            {
                //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                string part_of_line = Regex.Replace(Array_help[i].ToString(), @"L.nad.\|.+", "");
                string part_of_compare_line = Regex.Replace(Array_help[i + 1].ToString(), @"L.nad.\|.+", "");
                if (part_of_line == part_of_compare_line)
                {
                    Linia_liczba_odt_1 = Regex.Replace(Array_help[i].ToString(), "L.nad.", Odt.ToString());
                    Array_returned.Add("");
                    Odt++;
                }
                //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                else
                {
                    Linia_liczba_odt_1 = Regex.Replace(Array_help[i].ToString(), "L.nad.", Odt.ToString());
                    Linia_liczba_odt_2 = iiii.ToString() + "|" + Linia_liczba_odt_1;
                    iiii++;
                    Array_returned.Add(Linia_liczba_odt_2);
                    Odt = 1;
                }
            }
            //Dla ostatniego elementu
            else
            {
                //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                string part_of_line = Regex.Replace(Array_help[i].ToString(), @"L.nad.\|.+", "");
                string part_of_compare_line = Regex.Replace(Array_help[i - 1].ToString(), @"L.nad.\|.+", "");
                if (part_of_line == part_of_compare_line)
                {
                    Linia_liczba_odt_1 = Regex.Replace(Array_help[i].ToString(), "L.nad.", Odt.ToString());
                    Array_returned.Add("");
                    Odt++;
                }
                //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                else
                {
                    Linia_liczba_odt_1 = Regex.Replace(Array_help[i].ToString(), "L.nad.", Odt.ToString());
                    Linia_liczba_odt_2 = iiii.ToString() + "|" + Linia_liczba_odt_1;
                    iiii++;
                    Array_returned.Add(Linia_liczba_odt_2);
                    Odt = 1;
                }
            }
        }

        Clear_empty_line(Array_returned);
        return Array_returned;
    }

    private void Clear_empty_line(List<string> array4)
    {
        for (int i = array4.Count - 1; i >= 0; i--)
        {
            if (array4[i] == "")
            {
                array4.RemoveAt(i);
            }
        }
    }
}