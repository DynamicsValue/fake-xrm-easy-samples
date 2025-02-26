﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace FakeXrmEasy.Samples.CodeActivities
{
    public sealed partial class AddActivity : CodeActivity
    {
        /// <summary>
        /// Performs the addition of two summands
        /// </summary>
        protected override void Execute(CodeActivityContext executionContext)
        {
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory =
                executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service =
                serviceFactory.CreateOrganizationService(context.UserId);

            // Retrieve the summands and perform addition
            this.result.Set(executionContext,
                this.firstSummand.Get(executionContext) +
                this.secondSummand.Get(executionContext));
        }

        // Define Input/Output Arguments
        [Input("First summand")]
        public InArgument<int> firstSummand { get; set; }

        [Input("Second summand")]
        public InArgument<int> secondSummand { get; set; }

        [Output("Result")]
        public OutArgument<int> result { get; set; }
    }
}
