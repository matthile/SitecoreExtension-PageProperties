#PageProperties for sitecore
A solution to the editors, using webedit, that not want to see the Sitecore content editor.

## Documentation
	[PageProperties.Attributes.FieldNotVisibleInWebEdit(Fieldname = "Checkbox", ControlType = typeof(Sitecore.Web.UI.HtmlControls.Checkbox))]
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
This example makes a checkbox, and validates that the field is represented int the template of the current item.

### Fields in FieldNotVisibleInWebEdit
#### Fieldname
Sets the fieldname, in the template.
Used to validate that the template has that field

### ControlType
Sets the control, that will get rendered.
Must derive from Sitecore.Web.UI.HtmlControls.Control

### Name
Overrides the default label name.
Default name is the property name

### Order
Modifies the order of the rendered fields.
Default is int.MaxValue

___
##Todo
* Make attribute on class, to implement validation
* Check the order, on the template item if it can be found there
* Check Display name, insted of using propertyname
