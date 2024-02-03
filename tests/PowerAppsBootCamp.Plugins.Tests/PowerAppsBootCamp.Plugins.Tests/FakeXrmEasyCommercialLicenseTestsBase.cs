using System.Configuration;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.FakeMessageExecutors;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using Microsoft.Xrm.Sdk;
using System.Reflection;
using FakeXrmEasy.Samples.Tests.Shared.CommercialLicense;

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class FakeXrmEasyCommercialLicenseTestsBase
    {
        protected readonly IXrmFakedContext _context;
        protected readonly IOrganizationService _service;

        public FakeXrmEasyCommercialLicenseTestsBase()
        {
            _context = MiddlewareBuilder
                        .New()

                        // Add* -> Middleware configuration
                        .AddCrud()
                        .AddFakeMessageExecutors(Assembly.GetAssembly(typeof(AddListMembersListRequestExecutor)))

                        // Use* -> Defines pipeline sequence

                        .UseCrud()
                        .UseMessages()

                        .SetLicense(FakeXrmEasyLicense.Commercial)
                        .SetSubscriptionStorageProvider(new SubscriptionBlobStorageProvider())
                        .Build();

            _service = _context.GetOrganizationService();
        }
    }
}
