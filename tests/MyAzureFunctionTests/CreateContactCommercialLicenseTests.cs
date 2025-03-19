
using System.Linq;
using System.Threading.Tasks;
using DynamicsValue.AzFunctions;
using Xunit;

namespace MyAzureFunctionTests
{
    public class CreateContactCommercialLicenseTests : FakeXrmEasyCommercialLicenseTestsBase
    {
        [Fact]
        public async Task Should_create_contact()
        {
            /* This test will fail if you don't have a valid license key or the blob storage uri in environment variables, please also make sure the Blob SAS Url has both Read and Write permissions */
            var result = await CreateContactFn.CreateContact(_service, "Joe", "joe@satriani.com");
            Assert.True(result.Succeeded);
            
            var contacts = _context.CreateQuery("contact").ToList(); 
            Assert.Single(contacts);

            Assert.Equal("Joe", contacts[0]["firstname"]);
            Assert.Equal("joe@satriani.com", contacts[0]["emailaddress1"]);
        }
    }
}

