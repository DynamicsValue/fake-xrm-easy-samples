
using DataverseEntities;
using Microsoft.Xrm.Sdk;
using System;
using System.ServiceModel;

namespace MyPluginsSample
{
    [CrmPluginRegistration(
        MessageNameEnum.Create, 
        Contact.EntityLogicalName, 
        StageEnum.PreOperation, 
        ExecutionModeEnum.Synchronous,
        "", "PreCreate Contact",
        1, IsolationModeEnum.Sandbox
    )]
    public class FollowUpPlugin : IPlugin
    {
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
                Entity entity = (Entity)context.InputParameters["Target"];
                //</snippetFollowupPlugin2>

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "contact")
                    return;

                try
                {
                    // Create a task activity to follow up with the account customer in 7 days.
                    Entity followup = new Entity("task");

                    followup["subject"] = "Send e-mail to the new customer.";
                    followup["description"] =
                        "Follow up with the customer. Check if there are any new issues that need resolution.";
                    followup["scheduledstart"] = DateTime.Now.AddDays(7);
                    followup["scheduledend"] = DateTime.Now.AddDays(7);
                    followup["category"] = context.PrimaryEntityName;

                    // Refer to the contact in the task activity.
                    if (context.OutputParameters.Contains("id"))
                    {
                        Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                        string regardingobjectidType = "contact";

                        followup["regardingobjectid"] =
                        new EntityReference(regardingobjectidType, regardingobjectid);
                    }

                    //<snippetFollowupPlugin4>
                    // Obtain the organization service reference.
                    IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
                    //</snippetFollowupPlugin4>

                    // Create the task in Microsoft Dynamics CRM.
                    tracingService.Trace("FollowupPlugin: Creating the task activity.");
                    service.Create(followup);
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
