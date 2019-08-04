using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            string text = value.ToString();
            Type type = value.GetType();
            FieldInfo field = type.GetField(text);
            object[] customAttributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes.Length == 0)
            {
                return text;
            }
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)customAttributes[0];
            return descriptionAttribute.Description;
        }
    }
}
