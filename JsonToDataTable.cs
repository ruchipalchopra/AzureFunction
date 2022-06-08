using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

public class Program
{
    static void Main(string[] args)
    {
       DataTable dt = (DataTable)JsonConvert.DeserializeObject("Output.json", (typeof(DataTable)));
        // convert the list to a DataTable
    }
}