using DataverseEntitiesSpkl;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Plugins.Audit;
using FakeXrmEasy.Samples.PluginsWithSpkl;
using System.Linq;
using Xunit;

namespace MyPluginsSampleTests
{
    public class FollowUpPluginTests : FakeXrmEasyAutomaticRegistrationTestsBase
    {
        public FollowUpPluginTests()
        {

        }

        [Fact]
        public void Should_automatically_register_attribute()
        {
            var account = new Contact();
            _service.Create(account);

            var pluginStepAudit = _context.GetPluginStepAudit();
            var auditedStep = pluginStepAudit.CreateQuery().FirstOrDefault();
            Assert.NotNull(auditedStep);

            Assert.Equal("Create", auditedStep.MessageName);
            Assert.Equal(ProcessingStepStage.Preoperation, auditedStep.Stage);
            Assert.Equal(typeof(FollowUpPlugin), auditedStep.PluginAssemblyType);
        }
    }
}
