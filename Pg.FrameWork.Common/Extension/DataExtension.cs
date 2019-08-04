using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// 数据扩展
    /// </summary>
    public static class DataExtension
    {
        public static List<T> ToList<T>(this IDataReader reader) where T : class, new()
        {
            List<T> list = new List<T>();
            DataTable schemaTable = reader.GetSchemaTable();
            try
            {
                while (reader.Read())
                {
                    T val = new T();
                    if (schemaTable != null)
                    {
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string name = row[0].ToString();
                            PropertyInfo property = typeof(T).GetProperty(name);
                            if (!(property == null) && property.CanWrite)
                            {
                                property.SetValue(val, ConvertExtension.ConvertHelper(reader[name], property.PropertyType), null);
                            }
                        }
                    }
                    list.Add(val);
                }
                return list;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Dispose();
                    reader.Close();
                }
            }
        }

        public static List<T> ToList<T>(this DataTable dt) where T : class, new()
        {
            List<T> list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T val = new T();
                try
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        string columnName = column.ColumnName;
                        PropertyInfo property = typeof(T).GetProperty(columnName);
                        if (!(property == null) && property.CanWrite)
                        {
                            property.SetValue(val, ConvertExtension.ConvertHelper(row[columnName], property.PropertyType), null);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                list.Add(val);
            }
            return list;
        }

        public static List<T> ToList<T>(this DataSet ds) where T : class, new()
        {
            return ds.Tables[0].ToList<T>();
        }

        public static List<T> ToList<T>(this DataSet ds, int dataTableIndex) where T : class, new()
        {
            return ds.Tables[dataTableIndex].ToList<T>();
        }

        public static T ToModel<T>(this IDataReader reader) where T : class, new()
        {
            T val = new T();
            DataTable schemaTable = reader.GetSchemaTable();
            try
            {
                while (reader.Read())
                {
                    if (schemaTable != null)
                    {
                        foreach (DataRow row in schemaTable.Rows)
                        {
                            string name = row[0].ToString();
                            PropertyInfo property = typeof(T).GetProperty(name);
                            if (!(property == null) && property.CanWrite)
                            {
                                property.SetValue(val, ConvertExtension.ConvertHelper(reader[name], property.PropertyType), null);
                            }
                        }
                    }
                }
                return val;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (!reader.IsClosed)
                {
                    reader.Dispose();
                    reader.Close();
                }
            }
        }

        public static T ToModel<T>(this DataTable dt) where T : class, new()
        {
            T val = new T();
            if (dt.Rows.Count <= 0)
            {
                return val;
            }
            try
            {
                foreach (DataColumn column in dt.Columns)
                {
                    string columnName = column.ColumnName;
                    PropertyInfo property = typeof(T).GetProperty(columnName);
                    if (!(property == null) && property.CanWrite)
                    {
                        property.SetValue(val, ConvertExtension.ConvertHelper(dt.Rows[0][columnName], property.PropertyType), null);
                    }
                }
                return val;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public static T ToModel<T>(this DataSet ds, int dataTableIndex = 0) where T : class, new()
        {
            return ds.Tables[0].ToModel<T>();
        }
    }
}
