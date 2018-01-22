using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Bzway.Common.Utility
{
    /// <summary>
    /// Class with methods for saving and loading objects as 
    /// serialized instances.
    /// </summary>
    public static class SerializationHelper
    {
        //<summary>
        //使用XmlSerializer序列化对象
        //</summary>
        //<typeparam name=“T“>需要序列化的对象类型，必须声明[Serializable]特征</typeparam>
        //<param name=“obj“>需要序列化的对象</param>
        public static string XmlSerialize<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 使用XmlSerializer反序列化对象
        /// </summary>
        /// <param name=“xmlOfObject“>需要反序列化的xml字符串</param>
        public static T XmlDeserialize<T>(string xmlOfObject) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (StreamWriter sr = new StreamWriter(ms, Encoding.UTF8))
                {
                    sr.Write(xmlOfObject);
                    sr.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(ms) as T;
                }
            }
        }
        public static T DeserializeObjectJson<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
        public static string SerializeObjectToJson(object o)
        {
            return JsonConvert.SerializeObject(o, Newtonsoft.Json.Formatting.Indented);
        }
        /// <summary>
        /// Loads the specified type based on the specified stream.
        /// </summary>
        /// <param name="stream">stream containing the type.</param>
        /// <returns></returns>
        public static T DeserializeObjectFromXml<T>(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }


        public static string SerializationToSoap(object item)
        {
            if (item == null)
            {
                return string.Empty;
            }
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //Add an empty namespace and empty value
                ns.Add("", "");

                MemoryStream fs = new MemoryStream();
                XmlWriter writer = XmlWriter.Create(fs, settings);
                XmlSerializer xs = new XmlSerializer(item.GetType());
                xs.Serialize(writer, item, ns);
                return System.Text.Encoding.UTF8.GetString(fs.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
