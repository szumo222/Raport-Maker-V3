using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    class DateRange
    {
        public List<List_date> ArrayBeforeDistinct { get; set; } = new List<List_date>();
        private List<DateTime> GetDateRange(DateTime StartingDate, DateTime EndingDate)
        {
            if (StartingDate > EndingDate)
            {
                return null;
            }
            List<DateTime> rv = new List<DateTime>();
            DateTime tmpDate = StartingDate;
            do
            {
                rv.Add(tmpDate);
                tmpDate = tmpDate.AddDays(1);
            } while (tmpDate <= EndingDate);
            return rv;
        }

        public List<List_date> MonthRange(DatePicker datepicker1, DatePicker datepicker2)
        {
            DateTime StartingDate = datepicker1.SelectedDate.Value;
            DateTime EndingDate = datepicker2.SelectedDate.Value;
            foreach (DateTime date in GetDateRange(StartingDate, EndingDate))
            {
                List_date list_date = new List_date
                {
                    List_date_month = date.Month.ToString(),
                    List_date_year = date.Year.ToString(),
                    List_date_day = date.Day.ToString()
                };
                ArrayBeforeDistinct.Add(list_date);
            }
            return ArrayBeforeDistinct.Distinct().ToList();
        }
    }
}
