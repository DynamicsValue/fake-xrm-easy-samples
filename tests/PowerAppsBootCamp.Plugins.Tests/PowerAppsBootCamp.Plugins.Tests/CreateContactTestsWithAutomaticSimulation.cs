using System.Linq;
using DataverseEntities;
using Microsoft.Xrm.Sdk.Messages;
using Xunit;

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class CreateContactTestsWithAutomaticSimulation : FakeXrmEasyAutomaticPipelineTestsBase
    {
        [Fact]
        public void Should_create_contact_with_pipeline()
        {
            _service.Execute(new CreateRequest()
            {
                Target =
                new Contact()
                {
                    ["firstname"] = "Joe",
                    ["emailaddress1"] = "joe@satriani.com"
                }
            });

    
            var contacts = _context.CreateQuery("contact").ToList(); 
            Assert.Single(contacts);

            Assert.Equal("Joe", contacts[0]["firstname"]);
            Assert.Equal("joe@satriani.com", contacts[0]["emailaddress1"]);
         }

        
    }
}
