using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    class Class1
    {
        //Przygotowanie listy do zapisu dla Stoart
        public List<string> Stoart_Array_Prepare(List<string> array4, int iiii)
        {
            List<string> array_help = new List<string>();
            List<string> array_returned = new List<string>();
            //Usuwanie liczby Lp. na początku 
            foreach (string line in array4)
            {
                string replace;
                replace = Regex.Replace(line, @"^\d{1,}\|", "");
                array_help.Add(replace);
            }
            array_help.Sort();

            string linia_liczba_odt_1 = "";
            string linia_liczba_odt_2 = "";
            int odt = 1;

            //Liczenie takich samych wierszy
            for (int i = 0; i < array_help.Count; i++)
            {
                //Dla całej listy bez ostatniego elementu
                if (i < array_help.Count() - 1)
                {
                    //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                    string part_of_line = Regex.Replace(array_help[i].ToString(), @"L.nad.\|.+", "");
                    string part_of_compare_line = Regex.Replace(array_help[i + 1].ToString(), @"L.nad.\|.+", "");
                    if (part_of_line == part_of_compare_line)
                    {
                        linia_liczba_odt_1 = Regex.Replace(array_help[i].ToString(), "L.nad.", odt.ToString());
                        array_returned.Add("");
                        odt++;
                    }
                    //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                    else
                    {
                        linia_liczba_odt_1 = Regex.Replace(array_help[i].ToString(), "L.nad.", odt.ToString());
                        linia_liczba_odt_2 = iiii.ToString() + "|" + linia_liczba_odt_1;
                        iiii++;
                        array_returned.Add(linia_liczba_odt_2);
                        odt = 1;
                    }
                }
                //Dla ostatniego elementu
                else
                {
                    //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                    string part_of_line = Regex.Replace(array_help[i].ToString(), @"L.nad.\|.+", "");
                    string part_of_compare_line = Regex.Replace(array_help[i - 1].ToString(), @"L.nad.\|.+", "");
                    if (part_of_line == part_of_compare_line)
                    {
                        linia_liczba_odt_1 = Regex.Replace(array_help[i].ToString(), "L.nad.", odt.ToString());
                        array_returned.Add("");
                        odt++;
                    }
                    //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                    else
                    {
                        linia_liczba_odt_1 = Regex.Replace(array_help[i].ToString(), "L.nad.", odt.ToString());
                        linia_liczba_odt_2 = iiii.ToString() + "|" + linia_liczba_odt_1;
                        iiii++;
                        array_returned.Add(linia_liczba_odt_2);
                        odt = 1;
                    }
                }
            }

            Clear_empty_line(array_returned);
            return array_returned;
        }

        //Przygotowanie listy do zapisu dla reklam
        public List<string> Reklama_Array_Prepare(List<string> array4)
        {
            List<string> array_returned = new List<string>();
            int suma_czas_reklama = 0;
            int czas_reklaman_dzien = 0;
            List<string> suma_czas_dzien = new List<string>();
            suma_czas_dzien.Clear();

            for (int i = 0; i < array4.Count; i++)
            {
                Match match = Regex.Match(Regex.Match(array4[i], @"\|\d+\|$").ToString(), @"\d+");
                string string_out = Regex.Replace(array4[i], @"\|\d+\|$", "|");
                array_returned.Add(string_out);
                suma_czas_reklama = suma_czas_reklama + int.Parse(match.ToString());
                if (i == 0) czas_reklaman_dzien = int.Parse(match.ToString());
                else if ((i > 0) && (i < array4.Count - 1))
                {
                    Match match_2 = Regex.Match(array4[i], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    Match match_3 = Regex.Match(array4[i - 1], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    if (match_2.ToString() == match_3.ToString())
                    {
                        czas_reklaman_dzien = czas_reklaman_dzien + int.Parse(match.ToString());
                    }
                    else
                    {
                        suma_czas_dzien.Add("Suma elementów z dnia|" + match_3.ToString() + "|" + Calculate_duration(czas_reklaman_dzien) + "|");
                        czas_reklaman_dzien = int.Parse(match.ToString());
                    }
                }
                else if (i == array4.Count - 1)
                {
                    Match match_2 = Regex.Match(array4[i], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    czas_reklaman_dzien = czas_reklaman_dzien + int.Parse(match.ToString());
                    suma_czas_dzien.Add("Suma elementów z dnia|" + match_2.ToString() + "|" + Calculate_duration(czas_reklaman_dzien) + "|");
                }
            }

            for (int i = 0; i < suma_czas_dzien.Count; i++)
            {
                array_returned.Insert(i, suma_czas_dzien[i]);
            }

            array_returned.Insert(0, "Sumaryczny czas elementów w miesiacu|" + Calculate_duration(suma_czas_reklama) + "|");
            return array_returned;
        }

        //Przeliczanie czasu z milisekund na format H:M:S
        private string Calculate_duration(int suma_czas_reklama)
        {
            int godziny = suma_czas_reklama / 3600000;
            int reszta_z_godzin = suma_czas_reklama % 3600000;
            int minuty = reszta_z_godzin / 60000;
            int reszta_z_minut = reszta_z_godzin % 60000;
            int sekundy = reszta_z_minut / 1000;
            return godziny + ":" + minuty + ":" + sekundy;
        }

        public static void Clear_empty_line(List<string> array4)
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
}
