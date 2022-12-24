using FakeXrmEasy.Abstractions;
using FakeXrmEasy.Abstractions.Enums.CustomApis;
using FakeXrmEasy.Abstractions.FakeMessageExecutors;
using FakeXrmEasy.Abstractions.Middleware;
using FakeXrmEasy.Middleware.Messages;
using FakeXrmEasy.Plugins.Middleware.CustomApis;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MyPluginsSampleTests
{
    public static class AutomaticCustomApiFakeMessageExecutorExtensions
    {
        private class RequestProxyType
        {
            public Type RequestType { get; set; }
            public RequestProxyAttribute Attribute { get; set; }
        }

        private class ResponseProxyType
        {
            public Type ResponseType { get; set; }
            public ResponseProxyAttribute Attribute { get; set; }
        }

        private class CrmPluginRegistrationAttributeWithType
        {
            public Type PluginType { get; set; }
            public CrmPluginRegistrationAttribute RegistrationAttribute { get; set; }

            public ICustomApiFakeMessageExecutor ToCustomApiFakeMessageExecutor(
                IEnumerable<RequestProxyType> requestAttributes,
                IEnumerable<ResponseProxyType> responseAttributes)
            {
                var specificRequestAttributeType = requestAttributes
                            .Where(request => request.Attribute.Name == RegistrationAttribute.Message)
                            .Select(request => request.RequestType)
                            .FirstOrDefault();

                if(specificRequestAttributeType == null)
                {
                    specificRequestAttributeType = typeof(OrganizationRequest);
                }

                var specificResponseAttributeType = responseAttributes
                            .Where(response => response.Attribute.Name == RegistrationAttribute.Message)
                            .Select(response => response.ResponseType)
                            .FirstOrDefault();

                if (specificResponseAttributeType == null)
                {
                    specificResponseAttributeType = typeof(OrganizationResponse);
                }

                return Activator.CreateInstance(
                                        typeof(CustomApiFakeMessageExecutor<,,>)
                                    .MakeGenericType(new Type[] {
                                        PluginType,
                                        specificRequestAttributeType,
                                        specificResponseAttributeType
                                    })) as ICustomApiFakeMessageExecutor;
            }
        }

        public static IMiddlewareBuilder AddAutomaticCustomApiFakeMessageExecutors(this IMiddlewareBuilder builder, Assembly assembly = null)
        {
            builder.Add(context => {
                if (assembly == null) assembly = Assembly.GetExecutingAssembly();

                var customApiRegistrations = (from t in assembly.GetTypes()
                                          let attributes = t.GetCustomAttributes(typeof(CrmPluginRegistrationAttribute), true)
                                          where attributes != null && attributes.Length > 0
                                          select attributes
                                                .Cast<CrmPluginRegistrationAttribute>()
                                                .Where(pluginAttribute => pluginAttribute.Stage == null) //Custom Apis only//Custom Apis only
                                                .Select(pluginAttribute => new CrmPluginRegistrationAttributeWithType
                                                {
                                                    PluginType = t,
                                                    RegistrationAttribute = pluginAttribute
                                                })
                                            ).SelectMany(customApi => customApi).AsEnumerable();

                var requestAttributes = (from t in assembly.GetTypes()
                                         let attributes = t.GetCustomAttributes(typeof(RequestProxyAttribute), true)
                                         where attributes != null
                                                 && attributes.Length > 0
                                         let attribute = (RequestProxyAttribute)attributes[0]
                                         select new RequestProxyType()
                                         {
                                             RequestType = t,
                                             Attribute = attribute
                                         })
                                         .AsEnumerable();

                var responseAttributes = (from t in assembly.GetTypes()
                                         let attributes = t.GetCustomAttributes(typeof(ResponseProxyAttribute), true)
                                         where attributes != null
                                                 && attributes.Length > 0
                                         let attribute = (ResponseProxyAttribute) attributes[0]
                                         select new ResponseProxyType()
                                         {
                                             ResponseType = t,
                                             Attribute = attribute
                                         })
                                         .AsEnumerable();

                var customApiExecutors = customApiRegistrations.Select(customApi =>
                                        customApi.ToCustomApiFakeMessageExecutor(requestAttributes, responseAttributes))
                                    .ToDictionary(t => t.MessageName, t => (IFakeMessageExecutor)t);

                var messageExecutors = new GenericMessageExecutors(customApiExecutors);

                if (!context.HasProperty<GenericMessageExecutors>())
                    context.SetProperty(messageExecutors);
                else
                {
                    foreach (var messageExecutorKey in messageExecutors.Keys)
                    {
                        builder.AddFakeMessageExecutor(messageExecutors[messageExecutorKey]);
                    }
                }
            });

            return builder;
        }
    }
}
