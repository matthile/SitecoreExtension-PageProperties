using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PageProperties.Attributes;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.HtmlControls;

namespace PageProperties.Controls
{
    using System;

    using Sitecore;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Shell.Applications.ContentEditor;
    using Sitecore.Web.UI.Pages;

    class Edit : DialogForm
    {
        #region Fields

        private Sitecore.Web.UI.HtmlControls.Scrollbox InputFields;

        //private Sitecore.Web.UI.HtmlControls.Edit Description;
        Item item;

        #endregion Fields

        #region Methods

        protected void Items_Click(string input)
        {
            Console.Write("HELLO!!!");
        }

        protected bool IsValidField(string fieldname)
        {
            var templateFieldItem = item.Template.Fields.Where(field => field.Name == fieldname).DefaultIfEmpty(null).SingleOrDefault();

            if (templateFieldItem == null)
                return false;

            return true;
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
                            instance = Activator.CreateInstance(type.Key);

                        Control control = Activator.CreateInstance(attribute.ControlType) as Control;
                        Assert.IsNotNull(control, "Controltype in attribute, is not a valid Sitecore.Web.UI.HtmlControls control");

                        string propertyResult = propertyInfo.GetValue(instance, null).ToString();
                        Label controlLabel = new Label();
                        controlLabel.Value = !string.IsNullOrEmpty(attribute.Name)
                                                 ? attribute.Name
                                                 : propertyInfo.Name;
                        control.Value = propertyResult;
                        control.ID = string.Format("{0}.{1}_{2}",type.Key.Namespace, type.Key.Name, propertyInfo.Name);
                        InputFields.Controls.Add(controlLabel);
                        InputFields.Controls.Add(control);

                    }
                    
                }
            }

            //List<Types> typeses = Reflection.GetTypes();
            //foreach (Types typese in typeses)
            //{
            //    // Grabbing the specific static method
            //    MethodInfo methodInfo = typese.type.GetMethod("IsValid", System.Reflection.BindingFlags.Static | BindingFlags.Public);

            //    // Binding the method info to generic arguments
            //    bool returnValue = (bool)methodInfo.Invoke(null, new object[] { item });
            //    if (returnValue)
            //    {
            //        foreach (PropertyInfo property in typese.Property)
            //        {
            //            object instance = Activator.CreateInstance(typese.type, new object[] { item });
            //            FieldNotVisibleInWebEdit first = property.GetCustomAttributes(typeof (FieldNotVisibleInWebEdit), false).First() as FieldNotVisibleInWebEdit;

            //            PropertyInfo propertyInfo = property;
            //            string value = propertyInfo.GetValue(instance, null).ToString();
            //            string txtLabel = !string.IsNullOrEmpty(first.FieldName) ? first.FieldName : property.Name;
            //            Literal label = new Literal("<br />" + txtLabel);
            //            Sitecore.Web.UI.HtmlControls.Edit inputField = new Sitecore.Web.UI.HtmlControls.Edit();
            //            inputField.ID = string.Format("{0}_{1}", typese.type.Name, property.Name);
            //            inputField.Value = value;

            //            InputFields.Controls.Add(label);
            //            InputFields.Controls.Add(inputField);
            //        }

            //    }

            //}
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            //ItemUri uri = ItemUri.ParseQueryString();
            //item = Database.GetItem(uri);
            //List<Types> typeses = Reflection.GetTypes();
            //foreach (Types typese in typeses)
            //{
            //    // Grabbing the specific static method
            //    MethodInfo methodInfo = typese.type.GetMethod("IsValid", System.Reflection.BindingFlags.Static | BindingFlags.Public);

            //    // Binding the method info to generic arguments
            //    bool returnValue = (bool)methodInfo.Invoke(null, new object[] { item });
            //    if (returnValue)
            //    {
            //        foreach (PropertyInfo property in typese.Property)
            //        {
            //            Sitecore.Web.UI.HtmlControls.Edit first = InputFields.Controls.OfType<Sitecore.Web.UI.HtmlControls.Edit>().Where(t => t.ID == string.Format("{0}_{1}", typese.type.Name, property.Name)).First();
            //            object instance = Activator.CreateInstance(typese.type, new object[] { item });
            //            instance.GetType().GetMethod("BeginEdit").Invoke(instance, null);

            //            PropertyInfo propertyInfo = property;
            //            propertyInfo.SetValue(instance, first.Value, null);
            //            instance.GetType().GetMethod("EndEdit").Invoke(instance, null);
            //        }

            //    }

            //}
        }

        void button_OnClick(object sender, EventArgs e)
        {
            foreach (var control in InputFields.Controls)
            {
                var sasd = control;
            }
        }

        #endregion Methods
    }
}