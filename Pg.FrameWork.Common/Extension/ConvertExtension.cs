using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    public static class ConvertExtension
    {
        public static object ConvertHelper(object value, Type conversionType)
        {
            Type underlyingType = Nullable.GetUnderlyingType(conversionType);
            if (underlyingType != null)
            {
                if (value == DBNull.Value)
                {
                    return null;
                }
                if (underlyingType.IsEnum)
                {
                    value = Enum.Parse(underlyingType, value.ToString());
                }
                return Convert.ChangeType(value, underlyingType);
            }
            if (conversionType.IsEnum)
            {
                return Enum.Parse(conversionType, value.ToString());
            }
            return Convert.ChangeType(value, conversionType);
        }

        public static decimal? ConvertToDecimalNull(object targetObj)
        {
            if (targetObj == null || targetObj == DBNull.Value)
            {
                return null;
            }
            return Convert.ToDecimal(targetObj);
        }

        public static int? ConvertToIntNull(object targetObj)
        {
            if (targetObj == null || targetObj == DBNull.Value)
            {
                return null;
            }
            return Convert.ToInt32(targetObj);
        }

        public static string ConvertToString(object obj)
        {
            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            if (entitys == null || entitys.Count < 1)
            {
                throw new System.Exception("需转换的集合为空");
            }
            Type type = entitys[0].GetType();
            PropertyInfo[] properties = type.GetProperties();
            DataTable dataTable = new DataTable();
            PropertyInfo[] array = properties;
            foreach (PropertyInfo propertyInfo in array)
            {
                dataTable.Columns.Add(propertyInfo.Name);
            }
            foreach (T entity in entitys)
            {
                object obj = entity;
                if (obj.GetType() != type)
                {
                    throw new System.Exception("要转换的集合元素类型不一致");
                }
                object[] array2 = new object[properties.Length];
                for (int j = 0; j < properties.Length; j++)
                {
                    array2[j] = properties[j].GetValue(obj, null);
                }
                dataTable.Rows.Add(array2);
            }
            return dataTable;
        }
    }
}
