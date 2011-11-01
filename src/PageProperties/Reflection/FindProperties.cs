namespace SitecoreExtension.PageProperties.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
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

        private static readonly object SyncObject = new object();

        private static Dictionary<Type, List<PropertyInfo>> _properties;

        #endregion Fields

        #region Methods

        public static Dictionary<Type, List<PropertyInfo>> GetProperties()
        {
            lock (SyncObject)
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
                    Task task = Task.Factory.StartNew(() => SearchAssemblyTask(element));
                    tasksContainer.Add(task);

                }

                Task.WaitAll(tasksContainer.ToArray());
                sw.Stop();
                Trace.WriteLine(string.Format("Total time taken to search assemblies: {0}ms", sw.ElapsedMilliseconds));
                return FindProperties._properties;
            }
        }

        private static void SearchAssemblyTask(AssemblyElement assemblyElement)
        {
            try
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
            catch (FileNotFoundException notFound)
            {
                throw new FileNotFoundException(string.Format("PageProperties: Could not find {0}", notFound.FileName));
            }
        }

        #endregion Methods
    }
}