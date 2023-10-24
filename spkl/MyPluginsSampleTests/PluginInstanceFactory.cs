using FakeXrmEasy.Samples.PluginsWithSpkl;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace MyPluginsSampleTests
{
    public class PluginInstanceFactory
    {
        private readonly Dictionary<Type, IPlugin> _customInstances;

        public PluginInstanceFactory()
        {
            _customInstances = new Dictionary<Type, IPlugin>();
            _customInstances.Add(typeof(FollowUpPlugin), new FollowUpPlugin("Injected Value"));
        }
        public IPlugin CreateInstanceFor(Type pluginType) 
        {
            if (_customInstances.ContainsKey(pluginType))
                return _customInstances[pluginType];

            return null;
        } 
    }
}
