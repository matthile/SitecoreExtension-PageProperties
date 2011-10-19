#PageProperties for sitecore
A Sitecore Editor for the Page Edit ribbon to allow editors to edit the "hidden" page properties, such as the Display Name, Meta Description or Taxonomy. 
Keeping the view simple an keeping them out of the Sitecore content editor as long as possible.

Uses attributes on Item Class's for runtime field discovery. 
Can change properties on Sitecore Items or your own custom properties, based on your Item Class implementation.

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
This example makes a checkbox, and with Fieldname set it will validate if the current Item has the field present in the template.

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
##TODO List
* Allow custom field validation through the attributes on the class
* Check the order, on the template item if it can be found there
* Check Display name, insted of using propertyname
