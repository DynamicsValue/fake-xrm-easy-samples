using System.Linq;
using DataverseEntities;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Pipeline;
using FakeXrmEasy.Plugins.Audit;
using Microsoft.Xrm.Sdk.Messages;
using Xunit;

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class CreateContactTestsWithSimulation : FakeXrmEasyPipelineTestsBase
    {
        [Fact]
        public async void Should_create_contact_with_pipeline()
        {
            _context.RegisterPluginStep<FollowUpPlugin, Contact>("Create", ProcessingStepStage.Preoperation, ProcessingStepMode.Synchronous);

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

            var pluginStepAudit = _context.GetPluginStepAudit();
            var stepsAudit = pluginStepAudit.CreateQuery().ToList();
            Assert.Single(stepsAudit);

            var auditedStep = stepsAudit[0];

            Assert.Equal("Create", auditedStep.MessageName);
            Assert.Equal(typeof(FollowUpPlugin), auditedStep.PluginAssemblyType);
        }

        
    }
}
