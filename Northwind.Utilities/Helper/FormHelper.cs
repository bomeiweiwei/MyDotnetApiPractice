using System;
using System.Reflection;

namespace Northwind.Utilities.Helper
{
    public static class FormHelper
    {
        /// <summary>
        /// 將物件轉換為 Dictionary<string, string>，可用於 application/x-www-form-urlencoded 傳送。
        /// 僅會包含非 null 且非空白字串的屬性。
        /// </summary>
        public static Dictionary<string, string> ToFormDictionary(object obj)
        {
            if (obj == null)
                return new Dictionary<string, string>();

            return obj.GetType()
                      .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                      .Where(p => p.CanRead)
                      .Select(p => new { p.Name, Value = p.GetValue(obj) })
                      .Where(p => p.Value != null && !string.IsNullOrWhiteSpace(p.Value.ToString()))
                      .ToDictionary(p => p.Name, p => p.Value.ToString());
        }
    }
}

