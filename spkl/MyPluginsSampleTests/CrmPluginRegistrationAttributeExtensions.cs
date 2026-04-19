using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Plugins.PluginImages;
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

            if (filteringAttributes.IndexOf(",") < 0)
                return new List<string>();

            return filteringAttributes
                        .Split(',')
                        .Select(attribute => attribute.Trim())
                        .ToList();
        }

        public static PluginStepDefinition ToPluginStepDefinition(this CrmPluginRegistrationAttribute attribute, Type pluginAssemblyType)
        {
            PluginStepConfigurations configurations = null;
            if(!string.IsNullOrWhiteSpace(attribute.SecureConfiguration) || !string.IsNullOrWhiteSpace(attribute.UnSecureConfiguration))
            {
                configurations = new PluginStepConfigurations()
                {
                    SecureConfig = attribute.SecureConfiguration,
                    UnsecureConfig = attribute.UnSecureConfiguration
                };
            }

            var pluginInstanceFactory = new PluginInstanceFactory();
            List<PluginImageDefinition> images = new List<PluginImageDefinition>();

            if (attribute.Image1Name != null)
            {
                images.Add(new PluginImageDefinition(attribute.Image1Name, (ProcessingStepImageType)attribute.Image1Type, attribute.Image1Attributes.Split(',')));
            }

            if (attribute.Image2Name != null)
            {
                images.Add(new PluginImageDefinition(attribute.Image2Name, (ProcessingStepImageType)attribute.Image2Type, attribute.Image2Attributes.Split(',')));
            }

            return new PluginStepDefinition()
            {
                PluginType = pluginAssemblyType.FullName,
                MessageName = attribute.Message,
                EntityLogicalName = string.IsNullOrEmpty(attribute.EntityLogicalName) ? null : attribute.EntityLogicalName,
                FilteringAttributes = attribute.FilteringAttributes.ToFilteringAttributesEnumerable(),
                Id = !string.IsNullOrEmpty(attribute.Id) ? new Guid(attribute.Id) : Guid.Empty,
                Mode = attribute.ExecutionMode == ExecutionModeEnum.Synchronous ? ProcessingStepMode.Synchronous : ProcessingStepMode.Asynchronous,
                Rank = attribute.ExecutionOrder,
                Stage = (ProcessingStepStage) (int) attribute.Stage,
                Configurations = configurations,
                //PluginInstance = pluginInstanceFactory.CreateInstanceFor(pluginAssemblyType)  needs https://github.com/DynamicsValue/fake-xrm-easy/issues/118 first
                ImagesDefinitions = images,
            };
        }
    }
}
