using FakeXrmEasy.Abstractions.CommercialLicense;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FakeXrmEasy.Samples.Tests.Shared.CommercialLicense
{
    public class SubscriptionUsage : ISubscriptionUsage
    {
        public DateTime LastTimeChecked { get; set; }

        [JsonConverter(typeof(JsonConcreteCollectionTypeConverter<ISubscriptionUserInfo, SubscriptionUserInfo>))]
        public ICollection<ISubscriptionUserInfo> Users { get; set; }

        public ISubscriptionUpgradeRequest UpgradeInfo { get; set; }
    }
}
