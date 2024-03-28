using DataverseEntities;
using Microsoft.Xrm.Sdk;
using System;

namespace FakeXrmEasy.Samples.PluginsWithSpkl
{
    [CrmPluginRegistration(message: MessageNameEnum.Create,
    entityLogicalName: Contact.EntityLogicalName,
    stage: StageEnum.PreValidation,
    executionMode: ExecutionModeEnum.Synchronous,
    filteringAttributes: "",
    stepName: "PluginWhichThrowsAnException",
    executionOrder: 1,
    isolationModel: IsolationModeEnum.Sandbox,
    Description = "This plugin throws an exception ",
    Id = "d2231f29-dc15-48ee-ae56-98b2d13473cd"
)]
    // Plugin which just thorws an exception - created for the purposes of testing this issue:
    // https://github.com/DynamicsValue/fake-xrm-easy/issues/85
    public class PluginWhichThrowsAnException : IPlugin
    {
        public const string PluginExceptionMessage = "An error occured";

        public void Execute(IServiceProvider serviceProvider)
        {
            throw new InvalidPluginExecutionException(PluginExceptionMessage);
        }
    }
}
