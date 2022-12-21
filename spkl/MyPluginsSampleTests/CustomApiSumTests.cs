using DataverseEntities;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Plugins.Audit;
using FakeXrmEasy.Samples.PluginsWithSpkl;
using System.Linq;
using Xunit;

namespace MyPluginsSampleTests
{
    public class CustomApiSumTests : FakeXrmEasyAutomaticRegistrationTestsBase
    {
        [Fact]
        public void Should_execute_custom_api()
        {
            var sumResponse = _service.Execute(new dv_SumRequest()
            {
                Prop1 = 7,
                Prop2 = 3
            }) as dv_SumResponse;

            Assert.Equal(10, sumResponse.Sum);
        }

        [Fact]
        public void Should_trigger_another_plugin_registered_against_the_custom_api_using_automatic_registration()
        {
            var request = new dv_SumRequest()
            {
                Prop1 = 7,
                Prop2 = 3
            };

            var sumResponse = _service.Execute(request) as dv_SumResponse;

            Assert.Equal(10, sumResponse.Sum);

            var pluginStepAudit = _context.GetPluginStepAudit();
            var stepsAudit = pluginStepAudit.CreateQuery().ToList();

            Assert.Single(stepsAudit);
            var auditedStep = stepsAudit[0];

            Assert.Equal(request.RequestName, auditedStep.MessageName);
            Assert.Equal(ProcessingStepStage.Postoperation, auditedStep.Stage);
            Assert.Equal(typeof(SomePostOperationPlugin), auditedStep.PluginAssemblyType);
        }
    }
}
