using DataverseEntities;
using FakeXrmEasy.Abstractions.Plugins.Enums;
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
            var sdkMessages = _context.CreateQuery<SdkMessage>()
                                    .Where(mes => mes.Name == "Create")
                                    .ToList();
            Assert.NotEmpty(sdkMessages);

            var sdkMessage = sdkMessages.FirstOrDefault();

            var pluginSteps = _context.CreateQuery<SdkMessageProcessingStep>()
                        .Where(step => step.SdkMessageId.Id == sdkMessage.Id)
                        .ToList();
            
            Assert.NotEmpty(pluginSteps);

            var processingStep = pluginSteps.FirstOrDefault();
            Assert.Equal((int) ProcessingStepStage.Preoperation, (int) processingStep.Stage.Value);
            Assert.Equal((int) ProcessingStepMode.Synchronous, (int) processingStep.Mode.Value);
            Assert.Equal(1, processingStep.Rank);

            var sdkMessageFilter = _context.CreateQuery<SdkMessageFilter>()
                                    .Where(messageFilter => messageFilter.Id == processingStep.SdkMessageFilterId.Id)
                                    .FirstOrDefault();
            Assert.NotNull(sdkMessageFilter);
            Assert.Equal(Contact.EntityLogicalName, (string)sdkMessageFilter["entitylogicalname"]);

        }
    }
}
