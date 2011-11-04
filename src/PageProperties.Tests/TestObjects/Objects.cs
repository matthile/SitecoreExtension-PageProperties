namespace PageProperties.Tests.TestObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Objects
    {
        #region Properties

        [SitecoreExtension.PageProperties.Attributes.FieldNotVisibleInWebEdit(Name = "test")]
        public string Test
        {
            get; set;
        }

        #endregion Properties
    }
}