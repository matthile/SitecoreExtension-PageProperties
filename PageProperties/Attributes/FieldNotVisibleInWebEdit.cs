namespace PageProperties.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;

    public class FieldNotVisibleInWebEdit : Attribute
    {
        #region Fields

        private Type _controlType;

        #endregion Fields

        #region Properties

        public string Fieldname { get; set; }

        public Type ControlType
        {
            get
            {
                if (this._controlType == null)
                    return typeof (TextBox);
                return _controlType;
            }
            set { _controlType = value; }
        }

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