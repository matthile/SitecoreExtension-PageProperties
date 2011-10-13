using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.SecurityModel;

namespace PageProperties.SitecoreTest
{
    public class Frontpage
    {

        [PageProperties.Attributes.FieldNotVisibleInWebEdit(Fieldname = "TextBox")]
        public string TextBox
        {
            get { return Sitecore.Context.Item["TextBox"]; }
            set
            {
                using (new SecurityDisabler())
                {
                    Sitecore.Context.Item.Editing.BeginEdit();
                    Sitecore.Context.Item.Fields["TextBox"].Value = value;
                    Sitecore.Context.Item.Editing.EndEdit();
                    Sitecore.Context.Item.Editing.AcceptChanges();

                }
            }
        }
    }
}
