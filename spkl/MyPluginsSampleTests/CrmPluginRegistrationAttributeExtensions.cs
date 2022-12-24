using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Plugins.PluginSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyPluginsSampleTests
{
    public static class CrmPluginRegistrationAttributeExtensions
    {
        public static IEnumerable<string> ToFilteringAttributesEnumerable(this string filteringAttributes)
        {
            if (string.IsNullOrWhiteSpace(filteringAttributes))
                return new List<string>();

            if(filteringAttributes.IndexOf(",") < 0)
                return new List<string>();

            return filteringAttributes
                        .Split(',')
                        .Select(attribute => attribute.Trim())
                        .ToList();
        }

        public static PluginStepDefinition ToPluginStepDefinition(this CrmPluginRegistrationAttribute attribute, Type pluginAssemblyType)
        {
            return new PluginStepDefinition()
            {
                PluginType = pluginAssemblyType.FullName,
                MessageName = attribute.Message,
                EntityLogicalName = attribute.EntityLogicalName,
                FilteringAttributes = attribute.FilteringAttributes.ToFilteringAttributesEnumerable(),
                Id = !string.IsNullOrEmpty(attribute.Id) ? new Guid(attribute.Id) : Guid.Empty,
                Mode = attribute.ExecutionMode == ExecutionModeEnum.Synchronous ? ProcessingStepMode.Synchronous : ProcessingStepMode.Asynchronous,
                Rank = attribute.ExecutionOrder,
                Stage = (ProcessingStepStage) (int) attribute.Stage,
            };
        }
    }
}
