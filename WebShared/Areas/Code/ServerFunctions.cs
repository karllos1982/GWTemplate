using Microsoft.JSInterop;
using GW.Helpers;

/// <summary>
/// Summary description for UtilFunctions
/// </summary>
///

public class ServerFunctions
{
	public ServerFunctions()
	{
		

	}

    public string LoadTextFile(string filename)
    {
        string ret = "";

        if (File.Exists(filename))
        {
            ret = File.ReadAllText(filename);
        }

        return ret;


    }
    //prepara o conteúdo json convertendo os valores dos campos 
    //em fields de Numeric pra String
    public string PrepareContentJSON(string content,string[] fields)
    {
        string ret = content ;
        int ind = 0;
        int ind_end = 0;
        string aux_val = "";
        string aux = "";

        foreach (string str in fields)
        {
            ind = ret.IndexOf(str);
            ind_end = -1;
            if (ind > -1)
            {
                ind = ind + str.Length + 2;
                for (int i = ind; i < ret.Length ; i++)
                {
                    aux = ret.Substring(i, 1);
                    if (aux == ",")
                    {
                        ind_end = i;
                        break;
                    }

                }
            }

            if (ind_end > -1)
            {
                aux_val = ret.Substring(ind , ind_end - ind );
                ret = ret.Replace(aux_val, "\"" +  aux_val + "\"");
            }
        }

        return ret;
    }

    //

    public async Task SetCookie(IJSRuntime jsruntime, string name, string value, string expires)
    {
        value = Utilities.ConvertToBase64(value);

         await jsruntime.InvokeVoidAsync("cookieFunctions.createCookie", name, value, expires);        
    }

    public async Task<string> GetCookie(IJSRuntime jsruntime, string name)
    {
        string ret = "";
                       
        ret = await jsruntime.InvokeAsync<string>("cookieFunctions.getCookie", name)  ;
      
        ret = Utilities.ConvertFromBase64(ret);
                  
        return ret;
    }

    public void ClearCookie(IJSRuntime jsruntime, string name)
    {
        ValueTask t = jsruntime.InvokeVoidAsync("cookieFunctions.removeCookie", name);
    }

    //


    public async Task SaveLocalData(IJSRuntime jsruntime, string name, string value)
    {
        value = Utilities.ConvertToBase64(value);

        await jsruntime.InvokeVoidAsync("dataStorage.saveData", name, value);
    }

    public async Task<string> ReadLocalData(IJSRuntime jsruntime, string name)
    {
        string ret = "";

        ret = await jsruntime.InvokeAsync<string>("dataStorage.readData", name);

        ret = Utilities.ConvertFromBase64(ret);

        return ret;
    }

    public async Task ClearLocalData(IJSRuntime jsruntime, string name)
    {
        await jsruntime.InvokeVoidAsync("dataStorage.clearData", name);
    }


    public class PostFileStatus
    {
        public string Message { get; set; }
        public string FileName { get; set; }
        public string VirtualPath { get; set; }

    }


}