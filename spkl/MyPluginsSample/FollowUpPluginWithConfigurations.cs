
using DataverseEntitiesSpkl;
using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace FakeXrmEasy.Samples.PluginsWithSpkl
{
    [CrmPluginRegistration(
        MessageNameEnum.Create, 
        Account.EntityLogicalName, 
        StageEnum.PreOperation, 
        ExecutionModeEnum.Synchronous,
        "", "PreCreate Account",
        1, IsolationModeEnum.Sandbox,
        SecureConfiguration = "secureConfig",
        UnSecureConfiguration = "unsecureConfig"
    )]
    public class FollowUpPluginWithConfigurations : IPlugin
    {
        private readonly string _unsecureConfig;
        private readonly string _secureString;

        public FollowUpPluginWithConfigurations(string unsecureString, string secureString)
        {
            _unsecureConfig = unsecureString;
            _secureString = secureString;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
           
        }
    }
}
