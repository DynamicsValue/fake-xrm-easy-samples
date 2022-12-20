using DataverseEntities;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Pipeline;
using FakeXrmEasy.Plugins.Audit;
using FakeXrmEasy.Plugins.PluginSteps;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using Xunit;

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class CustomApiSumTests : FakeXrmEasyPipelineWithCustomApisTestsBase
    {
        // 100.000

        private class SomePlugin : IPlugin
        {
            public void Execute(IServiceProvider serviceProvider)
            {
                
            }
        }

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
        public void Should_trigger_another_plugin_registered_against_the_custom_api()
        {
            var request = new dv_SumRequest()
            {
                Prop1 = 7,
                Prop2 = 3
            };

            _context.RegisterPluginStep<SomePlugin>(new PluginStepDefinition()
            {
                MessageName = request.RequestName,
                Stage = ProcessingStepStage.Postoperation
            });

            var sumResponse = _service.Execute(request) as dv_SumResponse;

            Assert.Equal(10, sumResponse.Sum);

            var pluginStepAudit = _context.GetPluginStepAudit();
            var stepsAudit = pluginStepAudit.CreateQuery().ToList();

            Assert.Single(stepsAudit);
            var auditedStep = stepsAudit[0];

            Assert.Equal(request.RequestName, auditedStep.MessageName);
            Assert.Equal(ProcessingStepStage.Postoperation, auditedStep.Stage);
            Assert.Equal(typeof(SomePlugin), auditedStep.PluginAssemblyType);
        }
    }
}
