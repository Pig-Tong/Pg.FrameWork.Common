using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}
