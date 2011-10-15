namespace PageProperties.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using PageProperties.Attributes;
    using PageProperties.Configuration;

    // Could be rewritten to autofac or ninject
    public class FindProperties
    {
        #region Fields

        private static Dictionary<Type, List<PropertyInfo>> _properties;
        private static object _syncObject = new object();

        #endregion Fields

        #region Methods

        public static Dictionary<Type, List<PropertyInfo>> GetProperties()
        {
            lock (_syncObject)
            {
                Stopwatch sw = Stopwatch.StartNew();
                List<Task> tasksContainer = new List<Task>();
                if (_properties != null)
                {
                    sw.Stop();
                    Trace.WriteLine(string.Format("Total time taken to search assemblys: {0}ms", sw.ElapsedMilliseconds));
                    return FindProperties._properties;
                }

                FindProperties._properties = new Dictionary<Type, List<PropertyInfo>>();
                var assemblyElements = AssemblySection.GetAssemblies();

                foreach (AssemblyElement assemblyElement in assemblyElements)
                {
                    AssemblyElement element = assemblyElement;
                    Task task = Task.Factory.StartNew(() => SeachAssemblyTask(element));
                    tasksContainer.Add(task);

                }

                Task.WaitAll(tasksContainer.ToArray());
                sw.Stop();
                Trace.WriteLine(string.Format("Total time taken to search assemblys: {0}ms", sw.ElapsedMilliseconds));
                return FindProperties._properties;
            }
        }

        private static void SeachAssemblyTask(AssemblyElement assemblyElement)
        {
            var assembly = Assembly.Load(assemblyElement.Assembly);

            var types = assembly.GetTypes();
            foreach (Type type in types)
            {
                var propertyInfos = type.GetProperties();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    var customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldNotVisibleInWebEditAttribute), true);
                    if (customAttributes.Count() > 0)
                    {
                        lock (FindProperties._properties)
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
        }

        #endregion Methods
    }
}