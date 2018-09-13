using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Helpers
{
    public static class ConvertHelpers
    {
        public static string ToSafeString(this object value)
        {
            return value == null ? null : value.ToString();
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string SafeTrim(this string value)
        {
            return value.IsEmpty() ? null : value.Trim();
        }

        public static object GetPropertyValue<TKey>(this TKey data, string propertyName, bool throwException = true) where TKey : class
        {
            var propty = data.GetType().GetProperties().Where(a => a.Name == propertyName).FirstOrDefault();
            if (propty == null)
            {
                if (throwException)
                    throw new Exception(propertyName + " - Column not exist");
                else
                    return null;
            }
            else
                return propty.GetValue(data, null);
        }
    }
}
