using Microsoft.Xrm.Sdk;
using System;

namespace FakeXrmEasy.Samples.PluginsWithSpkl
{
    [CrmPluginRegistration(CustomApiSumPlugin_Message)]
    public class CustomApiSumPlugin : IPlugin
    {
        public const string CustomApiSumPlugin_Message = "dv_Sum";

        public void Execute(IServiceProvider serviceProvider)
        {
            var tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var pluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            var serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            var svc = serviceFactory.CreateOrganizationService(pluginContext.UserId);

            if (pluginContext.MessageName != CustomApiSumPlugin_Message)
            {
                throw new InvalidPluginExecutionException($"{nameof(CustomApiSumPlugin)} registered incorrectly");
            }

            if (!pluginContext.InputParameters.TryGetValue("Prop1", out var prop1))
            {
                throw new InvalidPluginExecutionException($"{nameof(CustomApiSumPlugin)} cannot find parameter 'Prop1' in input");
            };

            if (!pluginContext.InputParameters.TryGetValue("Prop2", out var prop2))
            {
                throw new InvalidPluginExecutionException($"{nameof(CustomApiSumPlugin)} cannot find parameter 'Prop2' in input");
            };

            pluginContext.OutputParameters["Sum"] = (int) prop1 + (int) prop2;
        }
    }
}
