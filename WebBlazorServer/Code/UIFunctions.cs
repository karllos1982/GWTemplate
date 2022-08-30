using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UIFunctions
/// </summary>
public class UIFunctions
{
	public UIFunctions()
	{
		
	}

    public static string ListToJSON(object list)
    {
        string ret = "";
       
        return ret;
    }

  

    public static string DomainToJSON(object list)
    {
        string ret = "";

        return ret;
    }
}

public class UIBaseItem
{
    public string ID;
    public string Value;
   
    public UIBaseItem(string id, string value)
    {
        //JavaUnicodeParser jup = new JavaUnicodeParser();

        ID = id;
        Value = value;
        
    }

    public UIBaseItem()
    {

    }

        
}

public static class UtilFunctions
{
    public static string DayName(DateTime date)
    {
        string ret = "";

        switch (date.DayOfWeek)
        {
            case System.DayOfWeek.Sunday:
                ret = "Dom";
                break;

            case System.DayOfWeek.Monday:
                ret = "Seg";
                break;

            case System.DayOfWeek.Tuesday:
                ret = "Ter";
                break;

            case System.DayOfWeek.Wednesday:
                ret = "Qua";
                break;

            case System.DayOfWeek.Thursday:
                ret = "Qui";
                break;

            case System.DayOfWeek.Friday:
                ret = "Sex";
                break;

            case System.DayOfWeek.Saturday:
                ret = "Sab";
                break;

        }

        return ret;
    }

    public static string MonthName(int month)
    {
        string ret = "";

        List<string> lista = new List<string>();

        lista.Add("Janeiro");
        lista.Add("Fevereiro");
        lista.Add("Março");
        lista.Add("Abril");
        lista.Add("Maio");
        lista.Add("Junho");
        lista.Add("Julho");
        lista.Add("Agosto");
        lista.Add("Setembro");
        lista.Add("Outubro");
        lista.Add("Novembro");
        lista.Add("Dezembro");

        ret = lista[month - 1];

        return ret;
    }
}



