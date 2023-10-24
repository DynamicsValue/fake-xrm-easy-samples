using DataverseEntitiesSpkl;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Plugins.Audit;
using System.Linq;
using Xunit;

namespace MyPluginsSampleTests
{
    public class FollowUpPluginWithConfigurationTests : FakeXrmEasyAutomaticRegistrationTestsBase
    {
        public FollowUpPluginWithConfigurationTests()
        {

        }

        [Fact]
        public void Should_automatically_register_attribute_and_pass_configurations()
        {
            var account = new Account();
            _service.Create(account);

            var pluginStepAudit = _context.GetPluginStepAudit();
            var auditedSteps = pluginStepAudit.CreateQuery().ToList();
            Assert.Single(auditedSteps);

            Assert.Equal("secureConfig", auditedSteps[0].PluginStepDefinition.Configurations.SecureConfig);
            Assert.Equal("unsecureConfig", auditedSteps[0].PluginStepDefinition.Configurations.UnsecureConfig);
        }
    }
}
