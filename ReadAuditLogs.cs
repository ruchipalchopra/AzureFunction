using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.WindowsAzure.Storage;
using System.Net;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;
using Microsoft.Azure.WebJobs.Host;

namespace bw.ReadAuditLogs
{
 [StorageAccount("MyStorageConnectionAppSetting")]
    public static class ReadAuditLogs
    {
        private const string RequestUri = "https://auditservice.dev.azure.com/ruchichopra0770/_apis/audit/auditlog?api-version=5.1-preview.1";

        [FunctionName("ReadAuditLogs")]
        public static async void Run([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
        {
	
        	var personalaccesstoken = "<pat>";

			using (HttpClient client = new HttpClient())
			{
			client.DefaultRequestHeaders.Accept.Add(
				new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
				Convert.ToBase64String(
					System.Text.ASCIIEncoding.ASCII.GetBytes(
						string.Format("{0}:{1}", "", personalaccesstoken))));

			HttpResponseMessage response = await client.GetAsync(requestUri: RequestUri);

            response.EnsureSuccessStatusCode();
            if (response.Content.Headers?.ContentType?.MediaType == "application/json")
  			{
  			    string body;
    			body = await response.Content.ReadAsStringAsync();
				File.WriteAllText("./Output.jscsrc",body);
      			if(!string.IsNullOrEmpty(body))
       			{
        		    string name;

        		    name = "AuditLogs" + TimeZoneInfo.Local.BaseUtcOffset;

         		   await AddRecievedAuditData(name + ".json", body);
        		}
   			 }
		}

    }
	

        private async static Task AddRecievedAuditData(string name, string data)
        {
	        string accessKey;
	        string accountName;
	        string connectionString;
	        CloudBlobClient client;
            CloudBlobContainer container;
            CloudBlockBlob blob;
	        CloudStorageAccount storageAccount;

	        accessKey = "NTH4rKiOGzUwQRuYPdgjEjf7+s1NMRo2LIZxeJ0eUmrvsLZPU+Oe6fi1sdD4kvSXEiUdPNcZGYZ469rMlRQuHg==";
	        accountName = "storageaccountcommi84ac";
            connectionString = "DefaultEndpointsProtocol=https;AccountName=" + accountName + ";AccountKey=" + accessKey + ";EndpointSuffix=core.windows.net";
	        storageAccount = CloudStorageAccount.Parse(connectionString);

            client = storageAccount.CreateCloudBlobClient();
    
            container = client.GetContainerReference("Auditlog-data");
   
            await container.CreateIfNotExistsAsync();

            blob = container.GetBlockBlobReference(name);
            blob.Properties.ContentType = "application/json";

            using (Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
            await blob.UploadFromStreamAsync(stream);
            }
        }
    }
}