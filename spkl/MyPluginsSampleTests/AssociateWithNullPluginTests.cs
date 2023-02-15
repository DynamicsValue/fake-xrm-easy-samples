using DataverseEntities;
using FakeXrmEasy.Abstractions.Plugins.Enums;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using Xunit;
using FakeXrmEasy.Plugins.Audit;
using FakeXrmEasy.Samples.PluginsWithSpkl;
using FakeXrmEasy.Abstractions;

namespace MyPluginsSampleTests
{
    public class AssociateWithNullPluginTests : FakeXrmEasyAutomaticRegistrationTestsBase
    {
        public AssociateWithNullPluginTests()
        {
            _context.AddRelationship("account_primary_contact", new XrmFakedRelationship()
            {
                Entity1Attribute = "contactid",
                Entity2Attribute = "accountid",
                Entity1LogicalName = "contact",
                Entity2LogicalName = "account",
                RelationshipType = XrmFakedRelationship.FakeRelationshipType.OneToMany
            });
        }

        [Fact]
        public void Should_trigger_plugin_with_null_entity_logical_name()
        {
            _service.Associate("dummy", Guid.NewGuid(), new Relationship("account_primary_contact"), new EntityReferenceCollection());

            var auditedStep = _context.GetPluginStepAudit()
                                .CreateQuery()
                                .Where(step => step.PluginAssemblyType == typeof(AssociateWithNullPlugin))
                                .FirstOrDefault();

            Assert.NotNull(auditedStep);
        }
    }
}
