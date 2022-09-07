using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.PowerPlatform.Dataverse.Client;
using System.Runtime.CompilerServices;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using DataverseEntities;

[assembly: InternalsVisibleTo("MyAzureFunctionTests")]
namespace DynamicsValue.AzFunctions
{
    public static class CreateContactFn
    {
        [FunctionName("CreateContactFn")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string firstName = req.Query["firstname"];
            string email = req.Query["email"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            firstName = firstName ?? data?.firstName;
            email = email ?? data?.email;

            
            var dataverseUrl = System.Environment.GetEnvironmentVariable("DataverseUrl");
            var clientId = System.Environment.GetEnvironmentVariable("ClientId");
            var clientSecret = System.Environment.GetEnvironmentVariable("ClientSecret");

            var client = new ServiceClient(new System.Uri(dataverseUrl), clientId, clientSecret, false);
            if(!client.IsReady)
            {
                return new ObjectResult("Couldn not connect to dataverse") { StatusCode = 401 };
            }

            var result = await CreateContact(client, firstName, email);
            return new OkObjectResult(JsonConvert.SerializeObject(result));
        }

        internal static async Task<GenericResult> CreateContact(IOrganizationServiceAsync2 service, string firstName, string email)
        {

            await service.CreateAsync(new Contact()
            {
                ["firstname"] = firstName,
                ["emailaddress1"] = email
            });

            return GenericResult.Succeed();
        }

        internal static GenericResult CreateContactSync(IOrganizationService service, string firstName, string email)
        {
            /*
            await service.CreateAsync(new Entity("contact") 
            {
                ["firstname"] = firstName,
                ["emailaddress1"] = email
            });
            */

            //service.Create(new Entity("contact")
            //{
            //    ["firstname"] = firstName,
            //    ["emailaddress1"] = email
            //});

            service.Execute(new CreateRequest()
            {
                Target =
                new Entity("contact")
                {
                    ["firstname"] = firstName,
                    ["emailaddress1"] = email
                }
            });
            return GenericResult.Succeed();
        }
    }
}
