using System;

namespace Frame.Common
{
    public static class UtilityType
    {
        public static bool SetValue(this object obj, string key, object value)
        {
            return obj.SetPropertyValue(key, value) || obj.SetFieldValue(key, value);
        }


        public static bool SetPropertyValue(this object obj, string key, object value)
        {
            var property = obj.GetType().GetProperty(key);
            var result = property != null;
            if (result)
            {
                var newValue = Convert.ChangeType(value, property.PropertyType);
                property.SetValue(obj, newValue);
            }
            return result;
        }
        
        public static bool SetFieldValue(this object obj, string key, object value)
        {
            var field = obj.GetType().GetField(key);
            var result = field != null;
            if (result)
            {
                var newValue = Convert.ChangeType(value, field.FieldType);
                field.SetValue(obj, newValue);
            }
            return result;
        }

    }
}