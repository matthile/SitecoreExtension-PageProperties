namespace PageProperties.Configuration
{
    using System.Configuration;

    public class AssemblysCollection : ConfigurationElementCollection
    {
        #region Indexers

        public new AssemblyElement this[string assembly]
        {
            get { return (AssemblyElement)BaseGet(assembly); }
        }

        #endregion Indexers

        #region Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).Assembly;
        }

        #endregion Methods
    }
}