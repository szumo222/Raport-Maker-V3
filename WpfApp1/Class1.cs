using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    class Class1
    {
        //Przygotowanie listy do zapisu dla Stoart
        public void Stoart_Array_Prepare(List<string> array4, List<string> array5, List<string> array6, int iiii)
        {
            //Usuwanie liczby Lp. na początku 
            foreach (string line in array4)
            {
                string replace;
                replace = Regex.Replace(line, @"^\d{1,}\|", "");
                array5.Add(replace);
            }
            array5.Sort();

            string linia_liczba_odt_1 = "";
            string linia_liczba_odt_2 = "";
            int odt = 1;

            //Liczenie takich samych wierszy
            for (int i = 0; i < array5.Count; i++)
            {
                //Dla całej listy bez ostatniego elementu
                if (i < array5.Count() - 1)
                {
                    //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                    string part_of_line = Regex.Replace(array5[i].ToString(), @"L.nad.\|.+", "");
                    string part_of_compare_line = Regex.Replace(array5[i + 1].ToString(), @"L.nad.\|.+", "");
                    if (part_of_line == part_of_compare_line)
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        array6.Add("");
                        odt++;
                    }
                    //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                    else
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        linia_liczba_odt_2 = iiii.ToString() + "|" + linia_liczba_odt_1;
                        iiii++;
                        array6.Add(linia_liczba_odt_2);
                        odt = 1;
                    }
                }
                //Dla ostatniego elementu
                else
                {
                    //Jeżeli wierwsze są takie same to wstawia pusty wierwsz i zwiększa licznik
                    string part_of_line = Regex.Replace(array5[i].ToString(), @"L.nad.\|.+", "");
                    string part_of_compare_line = Regex.Replace(array5[i - 1].ToString(), @"L.nad.\|.+", "");
                    if (part_of_line == part_of_compare_line)
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        array6.Add("");
                        odt++;
                    }
                    //Jeżeli nie są takie same to wstawia wiersz z ilość takich samych wierwszy
                    else
                    {
                        linia_liczba_odt_1 = Regex.Replace(array5[i].ToString(), "L.nad.", odt.ToString());
                        linia_liczba_odt_2 = iiii.ToString() + "|" + linia_liczba_odt_1;
                        iiii++;
                        array6.Add(linia_liczba_odt_2);
                        odt = 1;
                    }
                }
            }
            //Czyszczenie listy z pustych wierwszy
            for (int i = array6.Count - 1; i >= 0; i--)
            {
                if (array6[i] == "")
                {
                    array6.RemoveAt(i);
                }
            }
        }

        //Przygotowanie listy do zapisu dla reklam
        public void Reklama_Array_Prepare(List<string> array4, List<string> array5)
        {
            int suma_czas_reklama = 0;
            int czas_reklaman_dzien = 0;
            List<string> suma_czas_dzien = new List<string>();
            suma_czas_dzien.Clear();
            for (int i = 0; i < array4.Count; i++)
            {
                Match match = Regex.Match(Regex.Match(array4[i], @"\|\d+\|$").ToString(), @"\d+");
                string string_out = Regex.Replace(array4[i], @"\|\d+\|$", "|");
                array5.Add(string_out);
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
                    //Match match_3 = Regex.Match(array4[i - 1], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    czas_reklaman_dzien = czas_reklaman_dzien + int.Parse(match.ToString());
                    suma_czas_dzien.Add("Suma elementów z dnia|" + match_2.ToString() + "|" + Calculate_duration(czas_reklaman_dzien) + "|");
                }
            }
            for (int i = 0; i < suma_czas_dzien.Count; i++)
            {
                array5.Insert(i, suma_czas_dzien[i]);
            }
            array5.Insert(0, "Sumaryczny czas elementów w miesiacu|" + Calculate_duration(suma_czas_reklama) + "|");
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
    }
}
