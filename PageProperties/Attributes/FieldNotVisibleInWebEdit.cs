namespace PageProperties.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FieldNotVisibleInWebEdit : Attribute
    {
        #region Properties

        public string Name
        {
            get; set;
        }

        public bool Visible
        {
            get; set;
        }

        #endregion Properties
    }
}