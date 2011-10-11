namespace PageProperties.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class AssemblySection : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("Assemblys")]
        [ConfigurationCollection(typeof(AssemblysCollection), AddItemName = "Add")]
        public AssemblysCollection AssemblysCollection
        {
            get { return (AssemblysCollection)base["Assemblys"]; }
        }

        #endregion Properties

        #region Methods

        public static IEnumerable<AssemblyElement> GetAssemblies()
        {
            var section = (AssemblySection)ConfigurationManager.GetSection("PageProperties");
            return section.AssemblysCollection.Cast<AssemblyElement>();
        }

        #endregion Methods
    }
}