using System.Linq;
using DataverseEntities;
using Xunit;


// Uncomment this block to test your own commercial license key

namespace FakeXrmEasy.Samples.Plugins.Tests
{
    public class CommercialLicenseKeyTests: FakeXrmEasyCommercialLicenseTestsBase
    {
        [Fact]
        public void Should_not_crash()
        {
            var account = new Account() { Name = "Some name" };

            _service.Create(account);

            var accountAfter = _context.CreateQuery<Account>().FirstOrDefault();
            Assert.NotNull(accountAfter);
        }
    }
}
