using DataverseEntities;
using FakeXrmEasy.Plugins;
using FakeXrmEasy.Samples.PluginsWithSpkl;
using Xunit;
using Microsoft.Xrm.Sdk;
using System;

namespace MyPluginsSampleTests
{
    // Tests developed for this issue
    // https://github.com/DynamicsValue/fake-xrm-easy/issues/85

    public class TestExceptionsThrownInPipeline : FakeXrmEasyAutomaticRegistrationTestsBase
    {
        // This test passes as the plugin is invoked directly
        [Fact]
        public void When_A_Resource_Request_Is_Created_It_Will_Get_Its_Name_From_Its_Related_Opportunity_And_Team()
        {
            // Arrange
            Contact target = new Contact() { Id = Guid.NewGuid() };
            XrmFakedPluginExecutionContext pluginExecutionContext = _context.GetDefaultPluginContext();
            ParameterCollection _inputParameters = new ParameterCollection { { "Target", target } };
            pluginExecutionContext.InputParameters = _inputParameters;
            pluginExecutionContext.Stage = (int)StageEnum.PreValidation;
            pluginExecutionContext.MessageName = MessageNameEnum.Create.ToString();

            //Act
            Action act = () => _context.ExecutePluginWith<PluginWhichThrowsAnException>(pluginExecutionContext);
            
            //Assert
            InvalidPluginExecutionException exception = Assert.Throws<InvalidPluginExecutionException>(act);            
            Assert.Equal(PluginWhichThrowsAnException.PluginExceptionMessage, exception.Message);

        }

        // This test fails - because the plugin is invoked through pipeline simulation, not directly
        [Fact]
        public void When_A_Resource_Request_Is_Created_Without_A_Team_An_Exception_Will_Get_Thrown()
        {
            // Arrange
            Contact contact = new Contact() { Id = Guid.NewGuid() };
            // Act
            Action act = () => _service.Create(contact);
            //assert
            InvalidPluginExecutionException exception = Assert.Throws<InvalidPluginExecutionException>(act);
            Assert.Equal(PluginWhichThrowsAnException.PluginExceptionMessage, exception.Message);            
        }

    }
}