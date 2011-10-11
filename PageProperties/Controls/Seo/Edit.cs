using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GNReSound.CustomItems.Document.Data;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;

namespace GNReSound.BL.Controls.Seo
{
    class Edit : DialogForm
    {
        private Sitecore.Web.UI.HtmlControls.Scrollbox InputFields;
        private Sitecore.Web.UI.HtmlControls.Edit Description;
        Item item;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;
            ItemUri uri = ItemUri.ParseQueryString();
            item = Database.GetItem(uri);
            List<Types> typeses = Reflection.GetTypes();
            foreach (Types typese in typeses)
            {
                // Grabbing the specific static method
                MethodInfo methodInfo = typese.type.GetMethod("IsValid", System.Reflection.BindingFlags.Static | BindingFlags.Public);

                // Binding the method info to generic arguments
                bool returnValue = (bool)methodInfo.Invoke(null, new object[] { item });
                if (returnValue)
                {
                    foreach (PropertyInfo property in typese.Property)
                    {
                        object instance = Activator.CreateInstance(typese.type, new object[] { item });
                        FieldNotVisibleInWebEdit first = property.GetCustomAttributes(typeof (FieldNotVisibleInWebEdit), false).First() as FieldNotVisibleInWebEdit;

                        PropertyInfo propertyInfo = property;
                        string value = propertyInfo.GetValue(instance, null).ToString();
                        string txtLabel = !string.IsNullOrEmpty(first.FieldName) ? first.FieldName : property.Name;
                        Literal label = new Literal("<br />" + txtLabel);
                        Sitecore.Web.UI.HtmlControls.Edit inputField = new Sitecore.Web.UI.HtmlControls.Edit();
                        inputField.ID = string.Format("{0}_{1}", typese.type.Name, property.Name);
                        inputField.Value = value;

                        InputFields.Controls.Add(label);
                        InputFields.Controls.Add(inputField);
                    }

                }
                

            }
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            ItemUri uri = ItemUri.ParseQueryString();
            item = Database.GetItem(uri);
            List<Types> typeses = Reflection.GetTypes();
            foreach (Types typese in typeses)
            {
                // Grabbing the specific static method
                MethodInfo methodInfo = typese.type.GetMethod("IsValid", System.Reflection.BindingFlags.Static | BindingFlags.Public);

                // Binding the method info to generic arguments
                bool returnValue = (bool)methodInfo.Invoke(null, new object[] { item });
                if (returnValue)
                {
                    foreach (PropertyInfo property in typese.Property)
                    {
                        Sitecore.Web.UI.HtmlControls.Edit first = InputFields.Controls.OfType<Sitecore.Web.UI.HtmlControls.Edit>().Where(t => t.ID == string.Format("{0}_{1}", typese.type.Name, property.Name)).First();
                        object instance = Activator.CreateInstance(typese.type, new object[] { item });
                        instance.GetType().GetMethod("BeginEdit").Invoke(instance, null);

                        PropertyInfo propertyInfo = property;
                        propertyInfo.SetValue(instance, first.Value, null);
                        instance.GetType().GetMethod("EndEdit").Invoke(instance, null);
                    }

                }
                

            }
        }

        protected void Items_Click(string input)
        {
            Console.Write("HELLO!!!");
        }
        void button_OnClick(object sender, EventArgs e)
        {
            foreach (var control in InputFields.Controls)
            {
                var sasd = control;
            }
        }
    }
}
