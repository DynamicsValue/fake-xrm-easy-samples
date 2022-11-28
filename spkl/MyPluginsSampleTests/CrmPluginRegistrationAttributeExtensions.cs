using FakeXrmEasy.Plugins.PluginSteps;

namespace MyPluginsSampleTests
{
    public static class CrmPluginRegistrationAttributeExtensions
    {
        public static PluginStepDefinition ToPluginStepDefinition(this CrmPluginRegistrationAttribute attribute)
        {
            return new PluginStepDefinition()
            {

            };
        }
    }
}
