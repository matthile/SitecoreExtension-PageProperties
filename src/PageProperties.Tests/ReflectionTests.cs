namespace PageProperties.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    [TestFixture]
    class ReflectionTests
    {
        #region Methods

        [Test]
        public void GetAssemblis()
        {
            var properties = PageProperties.Reflection.FindProperties.GetProperties();
            Assert.AreEqual(1, properties.Count);
        }

        #endregion Methods
    }
}