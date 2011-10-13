namespace PageProperties.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using PageProperties.Attributes;

    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Applications.ContentEditor;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.Pages;
    using Sitecore.Web.UI.Sheer;

    class Edit : DialogForm
    {
        #region Fields

        private Sitecore.Web.UI.HtmlControls.Scrollbox InputFields;

        //private Sitecore.Web.UI.HtmlControls.Edit Description;
        Item item;

        #endregion Fields

        #region Methods

        protected bool IsValidField(string fieldname)
        {
            var templateFieldItem = item.Template.Fields.Where(field => field.Name == fieldname).DefaultIfEmpty(null).SingleOrDefault();

            if (templateFieldItem == null)
                return false;

            return true;
        }

        protected void Items_Click(string input)
        {
            Console.Write("HELLO!!!");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;
            ItemUri uri = ItemUri.ParseQueryString();
            item = Database.GetItem(uri);
            var properties = Reflection.FindProperties.GetProperties();

            foreach (var type in properties)
            {
                object instance = null;

                foreach (PropertyInfo propertyInfo in type.Value)
                {
                    var attribute = propertyInfo.GetCustomAttributes(typeof(FieldNotVisibleInWebEdit), true).First() as FieldNotVisibleInWebEdit;
                    if (string.IsNullOrEmpty(attribute.Fieldname) || (!string.IsNullOrEmpty(attribute.Fieldname) && IsValidField(attribute.Fieldname)))
                    {
                        // Create the type if its created
                        if (instance == null)
                        {
                            // Ohh! need item in the class, else the database will be core :(
                            ConstructorInfo constructorInfo = type.Key.GetConstructor(new Type[] {typeof (Item)});
                            Assert.IsNotNull(constructorInfo, "Need a constructor with parameter Sitecore.Data.Items.Item, be sure to save the item, and use it to return field values");
                            instance = Activator.CreateInstance(type.Key, new object[] { this.item });
                        }

                        Control control = Activator.CreateInstance(attribute.ControlType) as Control;
                        Assert.IsNotNull(control, "Controltype in attribute, is not a valid Sitecore.Web.UI.HtmlControls control");

                        string propertyResult = propertyInfo.GetValue(instance, null).ToString();
                        string controlId = string.Format("{0}.{1}_{2}", type.Key.Namespace, type.Key.Name,
                                                         propertyInfo.Name);
                        Label controlLabel = new Label();
                        controlLabel.Header = !string.IsNullOrEmpty(attribute.Name)
                                                 ? attribute.Name
                                                 : propertyInfo.Name;
                        controlLabel.For = controlId;
                        control.Value = propertyResult;
                        control.ID = controlId;
                        InputFields.Controls.Add(controlLabel);
                        InputFields.Controls.Add(control);

                    }

                }
            }
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            ItemUri uri = ItemUri.ParseQueryString();
            item = Database.GetItem(uri);
            var properties = Reflection.FindProperties.GetProperties();

            foreach (var type in properties)
            {
                object instance = null;

                foreach (PropertyInfo propertyInfo in type.Value)
                {
                    var attribute = propertyInfo.GetCustomAttributes(typeof(FieldNotVisibleInWebEdit), true).First() as FieldNotVisibleInWebEdit;
                    if (string.IsNullOrEmpty(attribute.Fieldname) || (!string.IsNullOrEmpty(attribute.Fieldname) && IsValidField(attribute.Fieldname)))
                    {
                        // Create the type if its created
                        if (instance == null)
                        {
                            // Ohh! need item in the class, else the database will be core :(
                            ConstructorInfo constructorInfo = type.Key.GetConstructor(new Type[] { typeof(Item) });
                            Assert.IsNotNull(constructorInfo, "Need a constructor with parameter Sitecore.Data.Items.Item, be sure to save the item, and use it to return field values");
                            instance = Activator.CreateInstance(type.Key, new object[] { this.item });
                        }
                        string controlId = string.Format("{0}.{1}_{2}", type.Key.Namespace, type.Key.Name,
                                                         propertyInfo.Name);
                        Control foundControl = InputFields.Controls.OfType<Control>().Where(c => c.ID == controlId).DefaultIfEmpty(null).First();
                        Assert.IsNotNull(foundControl, "Could not find the control {0}", controlId);
                        propertyInfo.SetValue(instance, foundControl.Value, null);
                    }

                }

                SheerResponse.CloseWindow();
            }
        }

        #endregion Methods
    }
}