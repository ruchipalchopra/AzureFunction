using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace jsondt
{
    class Program
    {
       
        static void Main(string[] args)
        {
            DataTable myTable;
            DataRow myNewRow;
            JObject result = JObject.Parse(File.ReadAllText("Output.jscsrc"));

                myTable = new DataTable("Audit Log");
                myTable.Columns.Add("id", typeof(string));
                myTable.Columns.Add("correlationId", typeof(string));
                myTable.Columns.Add("activityId", typeof(string));
                myTable.Columns.Add("actorCUID", typeof(string));
                myTable.Columns.Add("actorUserId", typeof(string));
                myTable.Columns.Add("actorUPN", typeof(string));
                myTable.Columns.Add("authenticationMechanism", typeof(string));
                myTable.Columns.Add("timestamp", typeof(string));
                myTable.Columns.Add("scopeType", typeof(string));
                myTable.Columns.Add("scopeDisplayName", typeof(string));
                myTable.Columns.Add("scopeId", typeof(string));
                myTable.Columns.Add("projectId", typeof(string));
                myTable.Columns.Add("projectName", typeof(string));
                myTable.Columns.Add("ipAddress", typeof(string));
                myTable.Columns.Add("userAgent", typeof(string));
                myTable.Columns.Add("actionId", typeof(string));
                myTable.Columns.Add("data", typeof(string));
                myTable.Columns.Add("details", typeof(string));
                myTable.Columns.Add("area", typeof(string));
                myTable.Columns.Add("category", typeof(string));
                myTable.Columns.Add("categoryDisplayName", typeof(string));
                myTable.Columns.Add("actorDisplayName", typeof(string));
                myTable.Columns.Add("actorImageUrl", typeof(string));

            foreach (var item in result["decoratedAuditLogEntries"])
            {
                myNewRow = myTable.NewRow();
                myNewRow["id"] = item["id"].ToString(); //string
                myNewRow["correlationId"] = item["correlationId"].ToString(); //string
                myNewRow["activityId"] = item["activityId"].ToString(); //string
                myNewRow["actorCUID"] = item["actorCUID"].ToString(); //string
                myNewRow["actorUserId"] = item["actorUserId"].ToString(); //string
                myNewRow["actorUPN"] = item["actorUPN"].ToString(); //string
                myNewRow["authenticationMechanism"] = item["authenticationMechanism"].ToString(); //string
                myNewRow["timestamp"] = item["timestamp"].ToString(); //string
                myNewRow["scopeType"] = item["scopeType"].ToString(); //string
                myNewRow["scopeDisplayName"] = item["scopeDisplayName"].ToString(); //string
                myNewRow["scopeId"] = item["scopeId"].ToString(); //string
                myNewRow["projectId"] = item["projectId"].ToString(); //string
                myNewRow["projectName"] = item["projectName"].ToString(); //string
                myNewRow["ipAddress"] = item["ipAddress"].ToString(); //string
                myNewRow["userAgent"] = item["userAgent"].ToString(); //string
                myNewRow["actionId"] = item["actionId"].ToString(); //string
                myNewRow["data"] = item["data"].ToString().Replace(" ","").Replace("\n",""); //string
                myNewRow["details"] = item["details"].ToString(); //string
                myNewRow["area"] = item["area"].ToString(); //string
                myNewRow["category"] = item["category"].ToString(); //string
                myNewRow["categoryDisplayName"] = item["categoryDisplayName"].ToString(); //string
                myNewRow["actorDisplayName"] = item["actorDisplayName"].ToString(); //string
                myNewRow["actorImageUrl"] = item["actorImageUrl"].ToString(); //string

                myTable.Rows.Add(myNewRow);
            }
            Console.WriteLine("details,categoryDisplayName,actorDisplayName \t");
            foreach (DataRow dr in myTable.Rows)
            {
                Console.WriteLine(dr[0]+","+dr[1]+","+dr[2]);
            }
        StringBuilder fileContent = new StringBuilder();

        foreach (var col in myTable.Columns) 
        {
            fileContent.Append("\"" + col.ToString() + "\"$");
        }

       fileContent.Replace("$", System.Environment.NewLine, fileContent.Length - 1, 1);

        foreach (DataRow dr in myTable.Rows) 
        {
            foreach (var column in dr.ItemArray) 
            {
                fileContent.Append("\"" + column.ToString() + "\"$");
            }

            fileContent.Replace("$", System.Environment.NewLine, fileContent.Length - 1, 1);
        }

        System.IO.File.WriteAllText("Output.csv", fileContent.ToString());
            
}
        }
    }
