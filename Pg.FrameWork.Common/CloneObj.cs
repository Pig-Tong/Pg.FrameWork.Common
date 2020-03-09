using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Pg.FrameWork.Common
{
    /// <summary>
    /// 克隆对象类
    /// </summary>
    public static class CloneObj
    {
        /// <summary>
        /// 深度复制
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T obj)
        {
            object obj2;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(memoryStream, obj);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                obj2 = xmlSerializer.Deserialize(memoryStream);
                memoryStream.Close();
            }
            return (T)obj2;
        }

        
        /// <summary>
        /// 利用反射实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByReflection<T>(T obj)
        {
            if (obj is string || obj.GetType().IsValueType)
                return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance);
            foreach(var field in fields)
            {
                try
                {
                    field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
                }
                catch { }
            }

            return (T)retval;
        }
    }
}
