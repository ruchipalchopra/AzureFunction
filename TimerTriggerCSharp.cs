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
using System.Data;

namespace bw.StoreCommitHistoryData
{
    public static class TimerTriggerCSharp
    {
        private const string RequestUri = "https://dev.azure.com/ruchichopra0770/DevLearnings/_apis/git/repositories/0be387e0-8b49-4b47-89dd-d72cb252fc0e/commits?api-version=5.1";

        [FunctionName("TimerTriggerCSharp")]
        public static async void Run([TimerTrigger("0 */2 * * * *")]TimerInfo myTimer, ILogger log)
        {
	
        	var personalaccesstoken = "ykrhouyr2jukvudmea4vqqbtbnjbhzcoexz7xrbuw2kcofvuhwna";

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
				DataTable dt = (DataTable)JsonConvert.DeserializeObject(body, (typeof(DataTable)));
				Console.Write(dt);
      			if(!string.IsNullOrEmpty(body))
       			{
        		    string name;

        		    name = "commithistory" + TimeZoneInfo.Local.BaseUtcOffset;

         		   await AddCommitHistory(name + ".json", body);
        		}
   			 }
		}

    }
	

        private async static Task AddCommitHistory(string name, string data)
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
    
            container = client.GetContainerReference("attestation-data");
   
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