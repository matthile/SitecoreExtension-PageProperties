using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GNReSound.BL.Controls.Seo
{
    class Types
    {
        public List<PropertyInfo> Property = new List<PropertyInfo>();

        public Type type;

        public Types(Type foundtype)
        {
            this.type = foundtype;
        }
    }
}
