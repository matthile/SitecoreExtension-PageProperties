namespace PageProperties.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [AttributeUsage(AttributeTargets.Property)]
    public class FieldNotVisibleInWebEditAttribute : Attribute
    {
        #region Fields

        private Type _controlType;

        #endregion Fields

        #region Properties

        /// <summary>
        /// this is the control, that will be rendered on pageproperties
        /// Default is Sitecore.Web.UI.HtmlControls.Edit
        /// </summary>
        public Type ControlType
        {
            get
            {
                if (this._controlType == null)
                    return typeof(Sitecore.Web.UI.HtmlControls.Edit);
                return _controlType;
            }
            set { _controlType = value; }
        }

        /// <summary>
        /// If this is set, the fieldname, will be validated on the current item.
        /// </summary>
        public string Fieldname
        {
            get; set;
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