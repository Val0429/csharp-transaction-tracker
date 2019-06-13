using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Constant.Configuration;

namespace Constant.Utility
{
    public static class ActivatorUtility
    {
        public static T Create<T>(string sectionName, params object[] constructorParams)
        {
            var instance = Create(sectionName, constructorParams);

            return instance == null ? default(T) : (T)instance;
        }

        public static object Create(string sectionName, params object[] constructorParams)
        {
            var activatorSection = ConfigurationManager.GetSection(sectionName) as ActivatorSection;
            if (activatorSection == null) return null;

            if (constructorParams.Any())
            {
                return Create(activatorSection, constructorParams);
            }

            var parameters = activatorSection.Parameters.Select(p => p.Value).Cast<object>().ToArray();

            return parameters.Any() ? Create(activatorSection, parameters) : Create(activatorSection);
        }

        public static T Create<T>(string sectionName, IDictionary<string, object> assigningParameters)
        {
            var activatorSection = ConfigurationManager.GetSection(sectionName) as ActivatorSection;
            if (activatorSection == null) return default(T);

            var instance = Create<T>(activatorSection);
            SetInstanceProperty(instance, assigningParameters);

            return instance;
        }

        private static void SetInstanceProperty(object instance, IDictionary<string, object> assigningParameters)
        {
            Type instanceType = instance.GetType();
            foreach (var parameter in assigningParameters)
            {
                var property = instanceType.GetProperty(parameter.Key);
                if (property != null)
                {
                    property.SetValue(instance, parameter.Value, null);
                }
            }
        }

        public static T Create<T>(ActivatorSection activatorSection, params object[] constructorParams)
        {
            return (T)Create(activatorSection, constructorParams);
        }

        public static object Create(ActivatorSection activatorSection, params object[] constructorParams)
        {
            Type targetType = activatorSection.Assembly.GetType(activatorSection.ClassName);

            return constructorParams.Any() ? Activator.CreateInstance(targetType, constructorParams) : Activator.CreateInstance(targetType);
        }

        public static T CreateFrom<T>(string sectionName, IDictionary<string, object> assigningParameters)
        {
            var instance = CreateFrom<T>(sectionName);

            if (instance != null)
            {
                SetInstanceProperty(instance, assigningParameters);
            }

            return instance;
        }

        public static T CreateFrom<T>(string sectionName)
        {
            var activatorSection = ConfigurationManager.GetSection(sectionName) as ActivatorSection;
            if (activatorSection == null) return default(T);

            var objectHandle = Activator.CreateInstanceFrom(activatorSection.AssemblyPath, activatorSection.ClassName);
            T instance = (T)objectHandle.Unwrap();

            return instance;
        }

        public static T CreateFrom<T>(this ActivatorSection activator)
        {
            Type targetType = activator.Assembly.GetType(activator.ClassName);

            var parameters = activator.Parameters.Select(p => p.Value).Cast<object>().ToArray();

            return parameters.Any() ? (T)Activator.CreateInstance(targetType, parameters) : (T)Activator.CreateInstance(targetType);
        }
    }
}
