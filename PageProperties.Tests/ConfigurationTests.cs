namespace PageProperties.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    class ConfigurationTests
    {
        #region Methods

        [Test]
        public void TestGetSection()
        {
            var section = (PageProperties.Configuration.AssemblySection)ConfigurationManager.GetSection("PageProperties");
            Assert.NotNull(section, "Section was null! ?");

            Assert.AreEqual(2, section.AssemblysCollection.Count, "Count is not 2! ?");

            Assert.AreEqual("PageProperties", section.AssemblysCollection["PageProperties"].Assembly);
        }

        #endregion Methods
    }
}