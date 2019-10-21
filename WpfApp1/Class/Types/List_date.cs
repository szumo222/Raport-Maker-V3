using System.Collections.Generic;

struct List_date
{
    public string List_date_month { get; set; }
    public string List_date_year { get; set; }
    public string List_date_day { get; set; }

    public override bool Equals(object obj)
    {
        return obj is List_date date &&
                List_date_month == date.List_date_month &&
                List_date_year == date.List_date_year &&
                List_date_day == date.List_date_day;
    }

    public override int GetHashCode()
    {
        var hashCode = 73715724;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(List_date_month);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(List_date_year);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(List_date_day);
        return hashCode;
    }

    public static bool operator ==(List_date first, List_date second)
    {
        return Equals(first, second);
    }
    public static bool operator !=(List_date first, List_date second)
    {
        // or !Equals(first, second), but we want to reuse the existing comparison 
        return !(first == second);
    }
}