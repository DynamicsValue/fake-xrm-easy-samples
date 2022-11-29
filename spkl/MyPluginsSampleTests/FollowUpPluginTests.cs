using DataverseEntities;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Plugins.PluginSteps;
using System.Linq;
using Xunit;

namespace MyPluginsSampleTests
{
    public class FollowUpPluginTests : FakeXrmEasyAutomaticRegistrationTestsBase
    {
        public FollowUpPluginTests()
        {

        }

        [Fact]
        public void Should_automatically_register_attribute()
        {
            var pluginSteps = _context.CreateQuery<SdkMessageProcessingStep>().ToList();
            
            Assert.Single(pluginSteps);

            var processingStep = pluginSteps.FirstOrDefault();
            Assert.Equal((int) ProcessingStepStage.Preoperation, processingStep.Stage.Value);
            Assert.Equal((int) ProcessingStepMode.Synchronous, processingStep.Mode.Value);
            Assert.Equal(1, processingStep.Rank);

            var sdkMessage = _context.CreateQuery<SdkMessage>()
                                    .Where(mes => mes.Id == processingStep.SdkMessageId.Id)
                                    .FirstOrDefault();
            Assert.NotNull(sdkMessage);
            Assert.Equal("Create", sdkMessage.Name);

            var sdkMessageFilter = _context.CreateQuery<SdkMessageFilter>()
                                    .Where(messageFilter => messageFilter.Id == processingStep.SdkMessageFilterId.Id)
                                    .FirstOrDefault();
            Assert.NotNull(sdkMessageFilter);
            Assert.Equal(Contact.EntityLogicalName, (string)sdkMessageFilter["entitylogicalname"]);

        }
    }
}
