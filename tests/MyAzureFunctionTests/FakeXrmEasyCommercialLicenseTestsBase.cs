using System.Reflection;
using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.FakeMessageExecutors;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.Samples.Tests.Shared.CommercialLicense;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace MyAzureFunctionTests;

public class FakeXrmEasyCommercialLicenseTestsBase
{
    protected readonly IXrmFakedContext _context;
    protected readonly IOrganizationServiceAsync2 _service;

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
            .SetSubscriptionStorageProvider(new SubscriptionBlobStorageProvider(), renewalRequested: true)
            .Build();

        _service = _context.GetAsyncOrganizationService2();
    }
}