using System;
using System.Linq;
using SFA.DAS.CollectionEarnings.Calculator.Context;
using StructureMap;
using StructureMap.Pipeline;

namespace SFA.DAS.CollectionEarnings.Calculator.DependencyResolution
{
    public class ConnectionStringPolicy : ConfiguredInstancePolicy
    {
        private readonly ContextWrapper _contextWrapper;

        public ConnectionStringPolicy(ContextWrapper contextWrapper)
        {
            _contextWrapper = contextWrapper;
        }

        protected override void apply(Type pluginType, IConfiguredInstance instance)
        {
            var parameter = instance.Constructor.GetParameters().FirstOrDefault(x => x.Name == "connectionString");

            if (parameter != null)
            {
                var connectionString = _contextWrapper.GetPropertyValue(ContextPropertyKeys.TransientDatabaseConnectionString);
                instance.Dependencies.AddForConstructorParameter(parameter, connectionString);
            }
        }
    }
}