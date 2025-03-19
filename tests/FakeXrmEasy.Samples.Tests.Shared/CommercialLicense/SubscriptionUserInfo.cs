using FakeXrmEasy.Abstractions.CommercialLicense;
using System;

namespace FakeXrmEasy.Samples.Tests.Shared.CommercialLicense
{
    public class SubscriptionUserInfo : ISubscriptionUserInfo
    {
        public DateTime LastTimeUsed { get; set; }
        public string UserName { get; set; }
    }
}
