namespace PageProperties.SitecoreTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Sitecore.Data.Items;
    using Sitecore.SecurityModel;

    public class Frontpage
    {
        #region Constructors

        //public Frontpage(Item item)
        //{
        //    this._item = item;
        //}
        public Frontpage(Item asd)
        {
            this._item = asd;
        }

        #endregion Constructors

        #region Properties

        [SitecoreExtension.PageProperties.Attributes.FieldNotVisibleInWebEdit(Fieldname = "Checkbox", ControlType = typeof(Sitecore.Web.UI.HtmlControls.Checkbox))]
        public string CheckBox
        {
            get { return _item["Checkbox"]; }
            set
            {
                using (new SecurityDisabler())
                {
                    _item.Editing.BeginEdit();
                    _item.Fields["Checkbox"].Value = value;
                    _item.Editing.EndEdit();
                    _item.Editing.AcceptChanges();

                }
            }
        }

        [SitecoreExtension.PageProperties.Attributes.FieldNotVisibleInWebEdit(Fieldname = "__Display name", ControlType = typeof(CustomControl))]
        public string Displayname
        {
            get { return _item[Sitecore.FieldIDs.DisplayName]; }
            set
            {
                using (new SecurityDisabler())
                {
                    _item.Editing.BeginEdit();
                    _item.Fields[Sitecore.FieldIDs.DisplayName].Value = value;
                    _item.Editing.EndEdit();
                    _item.Editing.AcceptChanges();

                }
            }
        }

        [SitecoreExtension.PageProperties.Attributes.FieldNotVisibleInWebEdit(Order = 2,ControlType = typeof(Sitecore.Web.UI.HtmlControls.Edit))]
        public string Testting
        {
            get { return _item["TextBox"]; }
            set
            {
                using (new SecurityDisabler())
                {
                    _item.Editing.BeginEdit();
                    _item.Fields["TextBox"].Value = value;
                    _item.Editing.EndEdit();
                    _item.Editing.AcceptChanges();

                }
            }
        }

        [SitecoreExtension.PageProperties.Attributes.FieldNotVisibleInWebEdit(Order = 1,ControlType = typeof(Sitecore.Web.UI.HtmlControls.Edit))]
        public string TextBox
        {
            get { return _item["TextBox"]; }
            set
            {
                using (new SecurityDisabler())
                {
                    _item.Editing.BeginEdit();
                    _item.Fields["TextBox"].Value = value;
                    _item.Editing.EndEdit();
                    _item.Editing.AcceptChanges();

                }
            }
        }

        public Item _item
        {
            get; set;
        }

        #endregion Properties
    }
}