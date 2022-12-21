using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums.CustomApis;
using FakeXrmEasy.Abstractions.FakeMessageExecutors;
using Microsoft.Xrm.Sdk;
using System;

namespace MyPluginsSampleTests
{
    public class AutomaticCustomApiExecutor : ICustomApiFakeMessageExecutor
    {

        public AutomaticCustomApiExecutor(Type pluginType)
        {

        }

        public IPlugin PluginType => throw new NotImplementedException();

        public string MessageName => throw new NotImplementedException();

        public CustomProcessingStepType CustomProcessingType => throw new NotImplementedException();

        public BindingType BindingType => throw new NotImplementedException();

        public string EntityLogicalName => throw new NotImplementedException();

        public bool CanExecute(OrganizationRequest request)
        {
            throw new NotImplementedException();
        }

        public OrganizationResponse Execute(OrganizationRequest request, IXrmFakedContext ctx)
        {
            throw new NotImplementedException();
        }

        public Type GetResponsibleRequestType()
        {
            throw new NotImplementedException();
        }
    }
}
