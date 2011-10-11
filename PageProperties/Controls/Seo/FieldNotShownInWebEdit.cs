using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GNReSound.BL.Controls.Seo
{
    class FieldNotVisibleInWebEdit : Attribute
    {
        public bool Visible { get; private set; }

        public string FieldName { get; private set; }

        public FieldNotVisibleInWebEdit(string fieldName)
        {
            this.FieldName = fieldName;
            this.Visible = false;
        }

        public FieldNotVisibleInWebEdit(bool value, string fieldName)
        {
            this.FieldName = fieldName;
            this.Visible = value;
        }

        public FieldNotVisibleInWebEdit()
        {
            this.Visible = false;
        }
    }
}
