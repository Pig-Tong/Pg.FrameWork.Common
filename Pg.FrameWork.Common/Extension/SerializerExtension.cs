using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Pg.FrameWork.Common.Extension
{
    /// <summary>
    /// 序列化扩展
    /// </summary>
    public static class SerializerExtension
    {
        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialization(this object obj)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Serialize(obj);
            }
            catch (System.Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// json字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserializ<T>(this string json)
        {
            T result = default(T);
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                return javaScriptSerializer.Deserialize<T>(json);
            }
            catch
            {
                return result;
            }
        }

        /// <summary>
        /// 对象转xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SerializeToXml<T>(this T t)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(t.GetType());
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Encoding = new UTF8Encoding(false);
                xmlWriterSettings.Indent = true;
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add("", "");
                using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
                {
                    xmlSerializer.Serialize(xmlWriter, t, xmlSerializerNamespaces);
                }
                string @string = Encoding.UTF8.GetString(memoryStream.ToArray());
                return Regex.Replace(@string, "\r\n\\s*", "");
            }
        }

        /// <summary>
        /// xml转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlToDeserialize<T>(this string xml)
        {
            try
            {
                using (StringReader textReader = new StringReader(xml))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)xmlSerializer.Deserialize(textReader);
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
