namespace PageProperties.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using PageProperties.Attributes;
    using PageProperties.Configuration;

    // Could be rewritten to autofac or ninject
    public class FindProperties
    {
        #region Fields

        private static Dictionary<Type, List<PropertyInfo>> _properties;

        #endregion Fields

        #region Methods

        public static Dictionary<Type, List<PropertyInfo>> GetProperties()
        {
            if (_properties != null)
                return FindProperties._properties;

            FindProperties._properties = new Dictionary<Type, List<PropertyInfo>>();
            var assemblyElements = AssemblySection.GetAssemblies();

            foreach (AssemblyElement assemblyElement in assemblyElements)
            {
                var assembly = Assembly.Load(assemblyElement.Assembly);

                var types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    var propertyInfos = type.GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        var customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldNotVisibleInWebEdit), true);
                        if (customAttributes.Count() > 0)
                        {
                            if (FindProperties._properties.ContainsKey(type))
                            {
                                FindProperties._properties[type].Add(propertyInfo);
                            }
                            else
                            {
                                FindProperties._properties.Add(type, new List<PropertyInfo>() { propertyInfo });
                            }
                        }

                    }
                }
            }

            return FindProperties._properties;
        }
        #endregion Methods
    }
}