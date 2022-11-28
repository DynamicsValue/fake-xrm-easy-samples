using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.Middleware.Pipeline;
using FakeXrmEasy.Plugins.PluginSteps;
using Microsoft.Xrm.Sdk;
using MyPluginsSample;
using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace MyPluginsSampleTests
{
    public class FakeXrmEasyAutomaticRegistrationTestsBase
    {
        protected readonly IXrmFakedContext _context;
        protected readonly IOrganizationService _service;

        public FakeXrmEasyAutomaticRegistrationTestsBase()
        {
            _context = MiddlewareBuilder
                            .New()

                            // Add* -> Middleware configuration
                            .AddCrud()
                            .AddFakeMessageExecutors(Assembly.GetAssembly(typeof(AddListMembersListRequestExecutor)))
                            .AddPipelineSimulation(new PipelineOptions()
                            {
                                UseAutomaticPluginStepRegistration = true,
                                PluginAssemblies = new List<Assembly>()
                                {
                                    Assembly.GetAssembly(typeof(FollowUpPlugin))
                                },
                                CustomPluginStepDiscoveryFunction = PluginStepDiscoveryFn
                            })

                            // Use* -> Defines pipeline sequence
                            .UsePipelineSimulation()
                            .UseCrud()
                            .UseMessages()

                            .SetLicense(FakeXrmEasyLicense.RPL_1_5)
                            .Build();

            _service = _context.GetOrganizationService();
        }

        private Func<Assembly, IEnumerable<PluginStepDefinition>> PluginStepDiscoveryFn = (Assembly assembly) =>
        {
            return new List<PluginStepDefinition>();
        };

    }
}
