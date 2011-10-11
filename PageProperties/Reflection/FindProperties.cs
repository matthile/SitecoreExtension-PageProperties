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

        public static Dictionary<Type, List<PropertyInfo>> Properties;

        #endregion Fields

        #region Methods

        public static Dictionary<Type, List<PropertyInfo>> GetProperties()
        {
            if (Properties != null)
                return FindProperties.Properties;

            FindProperties.Properties = new Dictionary<Type, List<PropertyInfo>>();
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
                            if (FindProperties.Properties.ContainsKey(type))
                            {
                                FindProperties.Properties[type].Add(propertyInfo);
                            }
                            else
                            {
                                FindProperties.Properties.Add(type, new List<PropertyInfo>() { propertyInfo });
                            }
                        }

                    }
                }
            }

            return FindProperties.Properties;
        }

        #endregion Methods
    }
}