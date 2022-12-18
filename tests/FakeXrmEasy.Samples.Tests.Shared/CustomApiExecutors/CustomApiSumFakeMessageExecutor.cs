using DataverseEntities;
using FakeXrmEasy.Abstractions.FakeMessageExecutors;
using FakeXrmEasy.Plugins.Middleware.CustomApis;
using FakeXrmEasy.Samples.Plugins;

namespace FakeXrmEasy.Samples.Tests.Shared.CustomApiExecutors
{
    public class CustomApiSumFakeMessageExecutor : CustomApiFakeMessageExecutor<CustomApiSumPlugin, dv_SumRequest>, ICustomApiFakeMessageExecutor
    {

    }
}
