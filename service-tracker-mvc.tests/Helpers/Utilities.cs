using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace service_tracker_mvc.tests.Helpers
{
    public class Utilities
    {
        // via http://weblogs.asp.net/leftslipper/archive/2007/09/24/using-c-3-0-anonymous-types-as-dictionaries.aspx
        public static IEnumerable<PropertyValue> GetProperties(object o)
        {
            if (o != null)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(o);
                foreach (PropertyDescriptor prop in props)
                {
                    object val = prop.GetValue(o);
                    if (val != null)
                    {
                        yield return new PropertyValue { Name = prop.Name, Value = val };
                    }
                }
            }
        }

        public sealed class PropertyValue
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}
