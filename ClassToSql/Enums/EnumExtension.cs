using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ClassToSql.Enums
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举值的显示名称
        /// 有指定Display属性时返回该属性的对应名称，否则返回字段本身的名称
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            // 获取枚举值类型和名称
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name == null)
            {
                // 值不在类型中时返回数字
                return Convert.ToInt32(value).ToString();
            }
            // 获取Display属性
            var field = type.GetField(Enum.GetName(type, value));
            var displayAttribute = field.GetCustomAttributes<DisplayAttribute>().FirstOrDefault();
            if (displayAttribute != null)
            {
                return displayAttribute.GetName() ?? displayAttribute.GetShortName();
            }

            // 返回默认名称
            return name;
        }
    }
}
