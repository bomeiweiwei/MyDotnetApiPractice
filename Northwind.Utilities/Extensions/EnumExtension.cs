using System;
using System.ComponentModel;
using System.Globalization;

namespace Northwind.Utilities.Extensions
{
    public static class EnumExtension
    { /// <summary>
      /// 取得 enum 的 Description 名稱
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="e"></param>
      /// <returns></returns>
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            Type type = e.GetType();
            Array values = System.Enum.GetValues(type);
            foreach (int val in values)
            {
                if (val == e.ToInt32(CultureInfo.InvariantCulture))
                {
                    var memInfo = type.GetMember(type.GetEnumName(val));
                    var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descriptionAttributes.Length > 0)
                    {
                        // we're only getting the first description we find
                        // others will be ignored
                        description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                    }
                    break;
                }
            }

            return description;
        }

    }
}

