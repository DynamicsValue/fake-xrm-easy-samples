using Microsoft.Xrm.Sdk;
using System;

namespace FakeXrmEasy.Samples.PluginsWithSpkl
{
    [CrmPluginRegistration(
        CustomApiSumPlugin_Message,
        null,
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "", "PostOperation dv_Sum",
        1, IsolationModeEnum.Sandbox
    )]
    public class SomePostOperationPlugin : IPlugin
    {
        public const string CustomApiSumPlugin_Message = "dv_Sum";
        
        public void Execute(IServiceProvider serviceProvider)
        {

        }
    }
}
