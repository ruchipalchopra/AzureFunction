/*using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Company.FunctionPAT
{
    public static class HttpTriggerCSharpPAT
    {
        [FunctionName("HttpTriggerCSharp")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
       
        var vstsCollectionUrl = "https://myaccount.visualstudio.com"; 
        var clientId = "43e45275-c132-4f6b-ad07-3ffc2c1370ad"; 
        var tenant= "dfb45909-68b3-4407-8f3a-1712a3bd1ab3";
        var VSTSResourceId = "499b84ac-1321-427f-aa17-267ca6975798"; 

        var personalaccesstoken = "ykrhouyr2jukvudmea4vqqbtbnjbhzcoexz7xrbuw2kcofvuhwna";
        var ctx = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);

        DeviceCodeResult codeResult = ctx.AcquireDeviceCodeAsync(VSTSResourceId, clientId).Result;
        Console.WriteLine("You need to sign in.");
        Console.WriteLine("Message: " + codeResult.Message + "\n");
        var result = ctx.AcquireTokenByDeviceCodeAsync(codeResult).Result;

		using (HttpClient client = new HttpClient())
		{
			client.DefaultRequestHeaders.Accept.Add(
				new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
				Convert.ToBase64String(
					System.Text.ASCIIEncoding.ASCII.GetBytes(
						string.Format("{0}:{1}", "", personalaccesstoken))));

			using (HttpResponseMessage response = await client.GetAsync(
						"https://dev.azure.com/ruchichopra0770/DevLearnings/_apis/git/repositories/0be387e0-8b49-4b47-89dd-d72cb252fc0e/commits?api-version=5.1"))
            {
				response.EnsureSuccessStatusCode();
				string responseMessage = await response.Content.ReadAsStringAsync();
			
            return new OkObjectResult(responseMessage);
			}
		}
        }
    }
}*/


