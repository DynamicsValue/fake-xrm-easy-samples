using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.FakeMessageExecutors;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.Middleware.Pipeline;
using FakeXrmEasy.Plugins.Middleware.CustomApis;
using FakeXrmEasy.Samples.Tests.Shared.CustomApiExecutors;
using Microsoft.Xrm.Sdk;
using System.Reflection;

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class FakeXrmEasyPipelineWithCustomApisTestsBase
    {
        protected readonly IXrmFakedContext _context;
        protected readonly IOrganizationService _service;

        public FakeXrmEasyPipelineWithCustomApisTestsBase() 
        {
            _context = MiddlewareBuilder
                            .New()

                            // Add* -> Middleware configuration
                            .AddCrud()
                            .AddFakeMessageExecutors(Assembly.GetAssembly(typeof(AddListMembersListRequestExecutor)))
                            .AddCustomApiFakeMessageExecutors(Assembly.GetAssembly(typeof(CustomApiSumFakeMessageExecutor)))

                            .AddPipelineSimulation(new PipelineOptions() { UsePluginStepAudit = true })

                            // Use* -> Defines pipeline sequence
                            .UsePipelineSimulation()
                            .UseCrud()
                            .UseMessages()

                            .SetLicense(FakeXrmEasyLicense.RPL_1_5)
                            .Build();

            _service = _context.GetOrganizationService();
        }
    }
}
