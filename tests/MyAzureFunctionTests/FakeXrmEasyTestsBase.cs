using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums;
using FakeXrmEasy.Middleware;
using FakeXrmEasy.Middleware.Crud;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace MyAzureFunctionTests
{
    public class FakeXrmEasyTestsBase
    {
        protected readonly IXrmFakedContext _context;
        protected readonly IOrganizationServiceAsync2 _service;

        public FakeXrmEasyTestsBase() 
        {
            _context = MiddlewareBuilder
                            .New()
                            .AddCrud()
                            
                            .UseCrud()
                            .SetLicense(FakeXrmEasyLicense.RPL_1_5)
                            .Build();

            _service = _context.GetAsyncOrganizationService2();
        }
    }
}
