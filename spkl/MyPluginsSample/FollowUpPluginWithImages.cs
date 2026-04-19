
using DataverseEntities;
using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace FakeXrmEasy.Samples.PluginsWithSpkl
{
    [CrmPluginRegistration(
        MessageNameEnum.Update,
        Contact.EntityLogicalName,
        StageEnum.PreValidation,
        ExecutionModeEnum.Synchronous,
        filteringAttributes:"firstname,lastname", 
        stepName:"PreCreate Contact",
        executionOrder: 1, 
        isolationModel: IsolationModeEnum.Sandbox,
        Image1Type = ImageTypeEnum.PreImage,
        Image1Name = "PreImage",
        Image1Attributes = "firstname,lastname"
    )]
    public class FollowUpPluginWithImages : IPlugin
    {
        private readonly string preImageAlias = "PreImage";
        public void Execute(IServiceProvider serviceProvider)
        {
            #region Boilerplate
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            //<snippetFollowupPlugin1>
            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));
            //</snippetFollowupPlugin1>

            #endregion

            //<snippetFollowupPlugin2>
            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Contact entity = (Contact)context.InputParameters["Target"];

                Contact preImageEntity = (context.MessageName == "Update" && context.PreEntityImages != null && context.PreEntityImages.Contains(preImageAlias)) ? ((Entity)context.PreEntityImages[preImageAlias]).ToEntity<Contact>() : null;

                //</snippetFollowupPlugin2>

                // Verify that the target entity represents an contact.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "contact")
                    return;

                try
                {
                    if (preImageEntity != null)
                    {
                        if (
                            (entity.Contains("firstname") && entity.FirstName != preImageEntity.FirstName)
                         || (entity.Contains("lastname") && entity.LastName != preImageEntity.LastName)
                         )

                        {
                            entity.Description = $"Customer has changed name from {preImageEntity.FirstName} {preImageEntity.LastName} to {entity.FirstName} {entity.LastName}";
                        }
                    }
                    else
                    {
                        throw new InvalidPluginExecutionException("Pre image entity not found");
                    }



                    //<snippetFollowupPlugin4>
                }
                //<snippetFollowupPlugin3>
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in the FollowUpPlugin plug-in.", ex);
                }
                //</snippetFollowupPlugin3>
                catch (Exception ex)
                {
                    tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}
