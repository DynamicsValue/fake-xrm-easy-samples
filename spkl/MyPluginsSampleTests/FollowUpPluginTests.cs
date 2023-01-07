using DataverseEntities;
using FakeItEasy;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using FakeXrmEasy.Plugins.Audit;
using FakeXrmEasy.Samples.PluginsWithSpkl;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
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

            // There are 3 plugins registered so far - one Pre-validation, one Pre-operation, one Post-Operation
            Assert.Equal(3, pluginSteps.Count());

            // Check that registration ok with the pre-operation Create plugin
            // I couldn't find another way to uniquely identify a step as the Name is not populated. Fine until we have another pre-operation plugin registered...!
            var preOperationProcessingStep = (from pluginStep in pluginSteps
                                              where (int)pluginStep.Stage.Value == (int)ProcessingStepStage.Preoperation
                                              select pluginStep).First();

            Assert.Equal((int)ProcessingStepMode.Synchronous, (int)preOperationProcessingStep.Mode.Value);
            Assert.Equal(1, preOperationProcessingStep.Rank);

            var sdkMessage = _context.CreateQuery<SdkMessage>()
                                    .Where(mes => mes.Id == preOperationProcessingStep.SdkMessageId.Id)
                                    .FirstOrDefault();
            Assert.NotNull(sdkMessage);
            Assert.Equal(MessageNameEnum.Create.ToString(), sdkMessage.Name);

            var sdkMessageFilter = _context.CreateQuery<SdkMessageFilter>()
                                    .Where(messageFilter => messageFilter.Id == preOperationProcessingStep.SdkMessageFilterId.Id)
                                    .FirstOrDefault();
            Assert.NotNull(sdkMessageFilter);
            Assert.Equal(Contact.EntityLogicalName, (string)sdkMessageFilter["entitylogicalname"]);

            // Check that all registration ok with the pre-validation Update plugin
            var preValidationProcessingStep = (from pluginStep in pluginSteps
                                               where (int)pluginStep.Stage.Value == (int)ProcessingStepStage.Prevalidation
                                               select pluginStep).First();

            Assert.Equal((int)ProcessingStepMode.Synchronous, (int)preValidationProcessingStep.Mode.Value);
            Assert.Equal(1, preValidationProcessingStep.Rank);
            Assert.Equal(2, preValidationProcessingStep.FilteringAttributes.Split(',').Count());
            Assert.Equal("firstname", preValidationProcessingStep.FilteringAttributes.Split(',')[0]);

            sdkMessage = _context.CreateQuery<SdkMessage>()
                                    .Where(mes => mes.Id == preValidationProcessingStep.SdkMessageId.Id)
                                    .FirstOrDefault();
            Assert.Equal(MessageNameEnum.Update.ToString(), sdkMessage.Name);

            sdkMessageFilter = _context.CreateQuery<SdkMessageFilter>()
                                    .Where(messageFilter => messageFilter.Id == preValidationProcessingStep.SdkMessageFilterId.Id)
                                    .FirstOrDefault();
            Assert.Equal(Contact.EntityLogicalName, (string)sdkMessageFilter["entitylogicalname"]);

        }

        [Fact]
        public void Should_fire_on_create_of_contact()
        {
            Contact contact = new Contact() { Id = Guid.NewGuid() };
            _service.Create(contact);

            // Followup plugin should fire and create a task
            A.CallTo(() => _service.Create(A<Entity>.That.Matches(q => q.LogicalName == "task"))).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public void Should_be_audited()
        {
            Contact contact = new Contact() { Id = Guid.NewGuid() };
            _service.Create(contact);

            var pluginStepAudit = _context.GetPluginStepAudit();
            var stepsAudit = pluginStepAudit.CreateQuery().ToList();

            Assert.Single(stepsAudit);
            var auditedStep = stepsAudit[0];

            Assert.Equal(MessageNameEnum.Create.ToString(), auditedStep.MessageName);
            Assert.Equal(ProcessingStepStage.Preoperation, auditedStep.Stage);
            Assert.Equal(typeof(FollowUpPlugin), auditedStep.PluginAssemblyType);
        }

        [Fact]
        public void Should_fire_on_update_of_firstname()
        {
            Contact contact = new Contact() { Id = Guid.NewGuid(), FirstName = "Charlie", LastName = "Brown" };
            _service.Create(contact);

            contact.FirstName = "Peppermint";
            contact.LastName = "Patty";

            _service.Update(contact);

            contact = (Contact)_service.Retrieve(contact.LogicalName, contact.Id, new ColumnSet(true));

            var pluginStepAudit = _context.GetPluginStepAudit();
            var stepsAudit = pluginStepAudit.CreateQuery().ToList();

            // One should fire on Create, and one on update
            Assert.Equal(2, stepsAudit.Count);

        }

        [Fact]
        public void Should_populate_preImage_attributes_correctly()
        {
            string firstName = "Charlie";
            string lastName = "Brown";
            Contact contact = new Contact() { Id = Guid.NewGuid(), FirstName = firstName, LastName = lastName };
            _service.Create(contact);

            contact.FirstName = "Peppermint";
            contact.LastName = "Patty";

            _service.Update(contact);

            contact = (Contact)_service.Retrieve(contact.LogicalName, contact.Id, new ColumnSet(true));

            Assert.Equal($"Customer has changed name from {firstName} {lastName} to {contact.FirstName} {contact.LastName}", contact.Description);
        }

        [Fact]
        public void Should_not_fire_on_update_of_jobtitle()
        {
            Contact contact = new Contact() { Id = Guid.NewGuid(), FirstName = "Charlie", LastName = "Brown" };
            _service.Create(contact);

            contact.JobTitle = "C";
            _service.Update(contact);

            var pluginStepAudit = _context.GetPluginStepAudit();
            var stepsAudit = pluginStepAudit.CreateQuery().ToList();

            // Only one plugin should have fired, but two did...
            // The Post update should only fire if firstname or lastname are updated.
            Assert.Single(stepsAudit);
            var auditedStep = stepsAudit[0];

            Assert.Equal(MessageNameEnum.Create.ToString(), auditedStep.MessageName);
            Assert.Equal(ProcessingStepStage.Preoperation, auditedStep.Stage);
            Assert.Equal(typeof(FollowUpPlugin), auditedStep.PluginAssemblyType);
        }
    }
}
