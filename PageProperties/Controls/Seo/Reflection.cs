using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using GNReSound.CustomItems.Document.Data;
using MS.Internal.Xml.XPath;

namespace GNReSound.BL.Controls.Seo
{
    class Reflection
    {
        public static List<Types> Types;
        public static List<Types> GetTypes()
        {
            if (Reflection.Types != null)
                return Types;

            Types = new List<Types>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                Types foundType = null;
                PropertyInfo[] propertyInfos = type.GetProperties();
                foreach (var propertyInfo in propertyInfos)
                {
                    object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(FieldNotVisibleInWebEdit),false);

                    if (customAttributes.Length > 0)
                    {
                        foreach (var attribute in customAttributes)
                        {
                            FieldNotVisibleInWebEdit fieldNotVisibleInWebEdit = (FieldNotVisibleInWebEdit)attribute;

                            if (!fieldNotVisibleInWebEdit.Visible)
                            {
                                if (foundType == null)
                                    foundType = new Types(type);
                                foundType.Property.Add(propertyInfo);    
                            }
                            
                        }
                    }
                }
                if (foundType != null)
                    Types.Add(foundType);
            }

            return Types;
        }
    }
}
