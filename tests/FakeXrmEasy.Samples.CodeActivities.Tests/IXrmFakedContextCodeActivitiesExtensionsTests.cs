using FakeXrmEasy.CodeActivities;
using System.Collections.Generic;
using Xunit;

namespace FakeXrmEasy.Samples.CodeActivities.Tests
{
    public class IXrmFakedContextCodeActivitiesExtensionsTests : FakeXrmEasyTestsBase
    {
        [Fact]
        public void When_the_add_activity_is_executed_the_right_sum_is_returned()
        {
            //Inputs
            var inputs = new Dictionary<string, object>() {
                { "firstSummand", 2 },
                { "secondSummand", 3 }
            };

            var result = _context.ExecuteCodeActivity<AddActivity>(inputs);

            Assert.Equal(5, (int)result["result"]);
        }
    }
}