using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.FakeMessageExecutors;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.Middleware.Pipeline;
using Microsoft.PowerPlatform.Dataverse.Client;
using System.Reflection;

namespace MyAzureFunctionTests
{
    public class FakeXrmEasyPipelineTestsBase
    {
        protected readonly IXrmFakedContext _context;
        protected readonly IOrganizationServiceAsync2 _service;

        public FakeXrmEasyPipelineTestsBase() 
        {
            _context = MiddlewareBuilder
                            .New()

                            // Add* -> Middleware configuration
                            .AddCrud()
                            .AddFakeMessageExecutors(Assembly.GetAssembly(typeof(AddListMembersListRequestExecutor)))
                            .AddPipelineSimulation(new PipelineOptions() { UsePluginStepAudit = true })

                            // Use* -> Defines pipeline sequence
                            .UsePipelineSimulation()
                            .UseCrud()
                            .UseMessages()

                            .SetLicense(FakeXrmEasyLicense.RPL_1_5)
                            .Build();

            _service = _context.GetAsyncOrganizationService2();
        }
    }
}
