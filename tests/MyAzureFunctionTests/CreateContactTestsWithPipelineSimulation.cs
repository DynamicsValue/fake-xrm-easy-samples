using System.Linq;
using DataverseEntities;
using DynamicsValue.AzFunctions;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Pipeline;
using FakeXrmEasy.Plugins.Audit;
using FakeXrmEasy.Samples.Plugins;
using Xunit;

namespace MyAzureFunctionTests
{
    public class CreateContactTestsWithPipelineSimulation : FakeXrmEasyPipelineTestsBase
    {
        [Fact]
        public async void Should_create_contact_and_task_with_pipeline()
        {
            _context.RegisterPluginStep<FollowUpPlugin>("Create", ProcessingStepStage.Postoperation, ProcessingStepMode.Synchronous, rank: 1, primaryEntityTypeCode: Contact.EntityTypeCode);

            var result = await CreateContactFn.CreateContact(_service, "Joe", "joe@satriani.com");
            Assert.True(result.Succeeded);

            var contacts = _context.CreateQuery("contact").ToList();
            Assert.Single(contacts);

            var tasks = _context.CreateQuery<Task>().ToList();
            Assert.Single(tasks);

            Assert.Equal("Joe", contacts[0]["firstname"]);
            Assert.Equal("joe@satriani.com", contacts[0]["emailaddress1"]);
        }

        [Fact]
        public async void Should_create_contact_with_pipeline_and_audit()
        {
            _context.RegisterPluginStep<FollowUpPlugin>("Create", ProcessingStepStage.Postoperation, ProcessingStepMode.Synchronous, rank: 1, primaryEntityTypeCode: Contact.EntityTypeCode);

            var result = await CreateContactFn.CreateContact(_service, "Joe", "joe@satriani.com");
            Assert.True(result.Succeeded);
            
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
