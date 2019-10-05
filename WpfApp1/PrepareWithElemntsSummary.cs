using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    class PrepareWithElemntsSummary
    {
        private List<string> Array_returned { get; set; } = new List<string>();
        private List<string> Suma_czas_dzien { get; set; } = new List<string>();
        private int Suma_czas_reklama { get; set; } = 0;
        private int Czas_reklaman_dzien { get; set; } = 0;

        //Przygotowanie listy do zapisu dla reklam
        public List<string> Summary_Elements_Array_Prepare(List<string> array4)
        {
            Suma_czas_dzien.Clear();

            for (int i = 0; i < array4.Count; i++)
            {
                Match match = Regex.Match(Regex.Match(array4[i], @"\|\d+\|$").ToString(), @"\d+");
                string string_out = Regex.Replace(array4[i], @"\|\d+\|$", "|");

                Array_returned.Add(string_out);

                Suma_czas_reklama = Suma_czas_reklama + int.Parse(match.ToString());

                if (i == 0) Czas_reklaman_dzien = int.Parse(match.ToString());
                else if ((i > 0) && (i < array4.Count - 1))
                {
                    Match match_2 = Regex.Match(array4[i], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    Match match_3 = Regex.Match(array4[i - 1], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    if (match_2.ToString() == match_3.ToString())
                    {
                        Czas_reklaman_dzien = Czas_reklaman_dzien + int.Parse(match.ToString());
                    }
                    else
                    {
                        Suma_czas_dzien.Add("Suma elementów z dnia|" + match_3.ToString() + "|" + Calculate_duration(Czas_reklaman_dzien) + "|");
                        Czas_reklaman_dzien = int.Parse(match.ToString());
                    }
                }
                else if (i == array4.Count - 1)
                {
                    Match match_2 = Regex.Match(array4[i], @"^\d{1,4}-\d{1,2}-\d{1,2}");
                    Czas_reklaman_dzien = Czas_reklaman_dzien + int.Parse(match.ToString());
                    Suma_czas_dzien.Add("Suma elementów z dnia|" + match_2.ToString() + "|" + Calculate_duration(Czas_reklaman_dzien) + "|");
                }
            }

            for (int i = 0; i < Suma_czas_dzien.Count; i++)
            {
                Array_returned.Insert(i, Suma_czas_dzien[i]);
            }

            Array_returned.Insert(0, "Sumaryczny czas elementów w miesiacu|" + Calculate_duration(Suma_czas_reklama) + "|");
            return Array_returned;
        }

        //Przeliczanie czasu z milisekund na format H:M:S
        public static string Calculate_duration(int suma_czas_reklama)
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
