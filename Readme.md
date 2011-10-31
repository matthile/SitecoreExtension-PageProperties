# Page Property Editor for Sitecore Page Editor Mode

A Sitecore Editor for the Page Edit ribbon to allow editors to edit the "hidden" page properties, such as the Display Name, Meta Description or Taxonomy. 
Keeping the view simple an keeping the editor in the page editor context.

1508 / Design in Love with Technology / http://1508.dk

## Requirements
* Sitecore 6.x 
* Sitecore Rocks

## Installation 
Install via nuget install-package SitecoreExtension.PageProperties

Set the Assemblies that are reflected at runtime in Web.Config > PageProperties > Assemblys

-------

## The Page Property Editor
Using the existing context menu in the Page Editor was the most natural placement for the page property editor. It shows only in edit mode and opens a dialog box, where the user can edit the item fields that are otherwise hidden or malplaced in the web interface. 

The Page properties dialog box generates its controls based on the item context and validates on the existence of specific field names prior to displaying the related control type for the fields.

Our projects uses Item code generation and therefore the most elegant model for handling the mapping of fields was to use Attributes set on properties on the generated Item code. At runtime the Page Property Editor uses reflection to find these attributes and validates if it should be rendered to the editor. Using the attributes custom Controls can be used in the Page Property interface (e.g. Checkboxes or Google Maps)

The code behind is simply using the get and set of the property, in combination with the possibility to render with a custom control. The same model as the Item Raw Values rendered through custom controls. Giving a very flexible use range for the editor.

The interface is generated with the current language context and using the field mapping information the relevant template sections and field help is injected in the editor interface, there by keeping most of the native Content Editor features but removing irrelevant fields that e.g. are already editable from the web page in the page editor.

You can install the editor with nuget via a local package, this requires Sitecore Rocks and that the package is located in a local nuget package feed.

-------

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

#### ControlType
Sets the control, that will get rendered.
Must derive from Sitecore.Web.UI.HtmlControls.Control

#### Name
Overrides the default label name.
Default name is the property name

#### Order
Modifies the order of the rendered fields.
Default is int.MaxValue


## Configuration
	  <configSections>
		<section name="PageProperties" type="PageProperties.Configuration.AssemblySection, PageProperties" />
	  </configSections>

	  <PageProperties>
		<Assemblys>
		  <!-- Add Assemblies that will be scanned at runtime for attributes-->	
		  <Add Assembly="PageProperties" />
		</Assemblys>
	  </PageProperties>

-------

## Possible improvements 
* Use the Field Display name instead of using the Property name
* Use the template sort order from the template item, if exists
* Allow custom field validation through the attributes on the class

Feel free to contribute!
