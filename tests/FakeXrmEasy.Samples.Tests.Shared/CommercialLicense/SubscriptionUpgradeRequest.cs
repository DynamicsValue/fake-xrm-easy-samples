using FakeXrmEasy.Abstractions.CommercialLicense;
using System;

namespace FakeXrmEasy.Samples.Tests.Shared.CommercialLicense
{
    public class SubscriptionUpgradeRequest : ISubscriptionUpgradeRequest
    {
        public DateTime FirstRequestDate { get; set; }
        public long PreviousNumberOfUsers { get; set; }
    }
}
