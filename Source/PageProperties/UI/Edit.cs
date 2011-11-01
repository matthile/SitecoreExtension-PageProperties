namespace SitecoreExtension.PageProperties.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using SitecoreExtension.PageProperties.Attributes;

    using Sitecore;
    using Sitecore.Configuration;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Globalization;
    using Sitecore.Web.UI.HtmlControls;
    using Sitecore.Web.UI.Pages;
    using Sitecore.Web.UI.Sheer;

    using Section = SitecoreExtension.PageProperties.UI.HtmlControls.Section;

    class Edit : DialogForm
    {
        #region Fields

        private readonly Dictionary<string, Section> _sections = new Dictionary<string, Section>();

        private Sitecore.Web.UI.HtmlControls.Scrollbox InputFields;
        Item item;

        #endregion Fields

        #region Methods

        protected bool IsValidField(string fieldname)
        {
            var templateFieldItem = item.Template.Fields.Where(field => field.Name == fieldname).SingleOrDefault();

            if (templateFieldItem == null)
                return false;

            //todo validate user rights

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

                List<string> asd = new List<string>();
                foreach (PropertyInfo propertyInfo in type.Value.OrderBy(t =>
                                                                             {
                                                                                 var attribute = t.GetCustomAttributes(typeof(FieldNotVisibleInWebEditAttribute), true).First() as FieldNotVisibleInWebEditAttribute;
                                                                                 return attribute.Order;
                                                                             }))
                {
                    var attribute = propertyInfo.GetCustomAttributes(typeof(FieldNotVisibleInWebEditAttribute), true).First() as FieldNotVisibleInWebEditAttribute;
                    if (string.IsNullOrEmpty(attribute.Fieldname) || (!string.IsNullOrEmpty(attribute.Fieldname) && IsValidField(attribute.Fieldname)))
                    {
                        this.PopulateInstance(type, ref instance);

                        Control control = Activator.CreateInstance(attribute.ControlType) as Control;
                        Assert.IsNotNull(control, "Controltype in attribute, is not a valid Sitecore.Web.UI.HtmlControls control");

                        string propertyResult = propertyInfo.GetValue(instance, null).ToString();
                        string controlId = string.Format("{0}.{1}_{2}", type.Key.Namespace, type.Key.Name,
                                                         propertyInfo.Name);
                        Label controlLabel = new Label();
                        string header = !string.IsNullOrEmpty(attribute.Name)
                                       ? attribute.Name
                                       : propertyInfo.Name;
                        string description = this.GetDescription(attribute.Fieldname);
                        if (!string.IsNullOrEmpty(description))
                            header += string.Format(" - {0}", description);

                        controlLabel.Header = header;
                        controlLabel.ToolTip = this.GetToolTip(attribute.Fieldname);
                        controlLabel.For = controlId;
                        control.Value = propertyResult;
                        control.ID = controlId;

                        Section section = this.GetSectionControl(attribute.Fieldname);

                        section.Controls.Add(controlLabel);
                        section.Controls.Add(control);
                    }

                }
            }
            foreach (Section section in _sections.OrderBy(t => t.Value.Order).Select(t => t.Value))
            {
                InputFields.Controls.Add(section);
            }
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            ItemUri uri = ItemUri.ParseQueryString();
            item = Database.GetItem(uri);
            var properties = Reflection.FindProperties.GetProperties();
            IEnumerable<Control> controlsOnPage = this.GetControls();
            foreach (var type in properties)
            {
                object instance = null;

                foreach (PropertyInfo propertyInfo in type.Value)
                {
                    var attribute = propertyInfo.GetCustomAttributes(typeof(FieldNotVisibleInWebEditAttribute), true).First() as FieldNotVisibleInWebEditAttribute;
                    if (string.IsNullOrEmpty(attribute.Fieldname) || (!string.IsNullOrEmpty(attribute.Fieldname) && IsValidField(attribute.Fieldname)))
                    {
                        PopulateInstance(type, ref instance);
                        string controlId = string.Format("{0}.{1}_{2}", type.Key.Namespace, type.Key.Name,
                                                         propertyInfo.Name);
                        Control foundControl = controlsOnPage.Where(c => c.ID == controlId).DefaultIfEmpty(null).First();
                        Assert.IsNotNull(foundControl, "Could not find the control {0}", controlId);
                        propertyInfo.SetValue(instance, foundControl.Value, null);
                    }

                }

                SheerResponse.CloseWindow();
            }
        }

        private IEnumerable<Control> GetControls()
        {
            foreach (Control sectionControl in InputFields.Controls)
            {
                foreach (Control control in sectionControl.Controls)
                {
                    yield return control;
                }
            }
        }

        private string GetDescription(string fieldname)
        {
            if (this.IsValidField(fieldname))
            {
                var templateFieldItem = item.Template.Fields.Where(field => field.Name == fieldname).SingleOrDefault();
                return templateFieldItem.ToolTip;
            }

            return string.Empty;
        }

        private Section GetSectionControl(string fieldName)
        {
            if (this.IsValidField(fieldName))
            {
                Database database = Factory.GetDatabase(item.Database.Name);
                TemplateItem template = database.GetItem(item.Template.ID, Context.Language);
                var templateFieldItem = template.Fields.Where(field => field.Name == fieldName).SingleOrDefault();
                if (templateFieldItem == null)
                    return this._sections["Misc"];

                if (this._sections.ContainsKey(templateFieldItem.Section.DisplayName))
                    return this._sections[templateFieldItem.Section.DisplayName];

                Section section = new Section();
                section.Header = templateFieldItem.Section.DisplayName;
                section.Order = templateFieldItem.Section.Sortorder;
                this._sections.Add(templateFieldItem.Section.DisplayName, section);
                return section;
            }
            if (this._sections.ContainsKey("misc"))
                return this._sections["misc"];

            using (var miscSection = new Section() { Header = Translate.Text("General"), Order = int.MaxValue })
            {
                this._sections.Add("misc", miscSection);

                return miscSection;
            }
        }

        private string GetToolTip(string fieldname)
        {
            if (this.IsValidField(fieldname))
            {
                var templateFieldItem = item.Template.Fields.Where(field => field.Name == fieldname).SingleOrDefault();
                return templateFieldItem.Description;
            }

            return string.Empty;
        }

        private void PopulateInstance(KeyValuePair<Type, List<PropertyInfo>> type, ref object instance)
        {
            if (instance == null)
            {
                ConstructorInfo constructorInfo = type.Key.GetConstructor(new Type[] { typeof(Item) });
                if (constructorInfo == null)
                {
                    // Assumes the class aint need to access the item
                    instance = Activator.CreateInstance(type.Key, null);
                }
                else
                {
                    // The class needs access to the item
                    instance = Activator.CreateInstance(type.Key, new object[] { this.item });
                }
            }
        }

        #endregion Methods
    }
}