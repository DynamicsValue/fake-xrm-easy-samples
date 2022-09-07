using DataverseEntities;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Pipeline;
using FakeXrmEasy.Plugins.Audit;
using Microsoft.Xrm.Sdk.Messages;
using System.Linq;
using Xunit;

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class AccountNumberPluginTests: FakeXrmEasyPipelineTestsBase
    {
        [Fact]
        public void Should_execute_registered_plugin_step()
        {
            _context.RegisterPluginStep<AccountNumberPlugin>("Create", ProcessingStepStage.Preoperation);

            var account = new Account() { Name = "Some name" };

            _service.Execute(new CreateRequest()
            {
                Target = account
            });

            var pluginStepAudit = _context.GetPluginStepAudit();
            var stepsAudit = pluginStepAudit.CreateQuery().ToList();

            Assert.Single(stepsAudit);

            var auditedStep = stepsAudit[0];

            Assert.Equal("Create", auditedStep.MessageName);
            Assert.Equal(typeof(AccountNumberPlugin), auditedStep.PluginAssemblyType);
        }
    }
}
