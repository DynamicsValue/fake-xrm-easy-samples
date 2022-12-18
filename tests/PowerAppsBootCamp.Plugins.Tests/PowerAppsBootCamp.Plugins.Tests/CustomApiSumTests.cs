using DataverseEntities;
using Xunit;

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class CustomApiSumTests : FakeXrmEasyPipelineWithCustomApisTestsBase
    {
        [Fact]
        public void Should_execute_custom_api()
        {
            var sumResponse = _service.Execute(new dv_SumRequest()
            {
                Prop1 = 7,
                Prop2 = 3
            });

            Assert.Equal(10, sumResponse.Results["Sum"]);
        }
    }
}
