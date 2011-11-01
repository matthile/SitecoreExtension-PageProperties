namespace SitecoreExtension.PageProperties.Configuration
{
    using System.Configuration;

    public class AssemblyElement : ConfigurationElement
    {
        #region Properties

        [ConfigurationProperty("Assembly", IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["Assembly"]; }
        }

        #endregion Properties
    }
}