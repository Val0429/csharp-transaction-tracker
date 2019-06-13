using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Constant.Serialization
{
    public static class XmlSerialization
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlText"></param>
        /// <param name="encoding"></param>
        /// <exception cref="System.Text.EncoderFallbackException"></exception>
        /// <returns></returns>
        public static T Deserialize<T>(this string xmlText, Encoding encoding = null)
        {
            if (String.IsNullOrEmpty(xmlText)) return default(T);

            encoding = encoding ?? Encoding.UTF8;

            byte[] data = encoding.GetBytes(xmlText);
            using (var stream = new MemoryStream(data))
            {
                var ser = new XmlSerializer(typeof(T));
                var entity = (T)ser.Deserialize(stream);
                return entity;
            }
        }

        public static string Serialize<T>(this T entity)
        {
            using (var stream = new MemoryStream())
            {
                Type type = entity.GetType();
                var ser = new XmlSerializer(type);
                ser.Serialize(stream, entity);
                var data = stream.ToArray();
                return Encoding.UTF8.GetString(data);
            }
        }

        //public static string Serialize<T>(this T entity, ITextSerializer serializer)
        //{
        //    return serializer.Serialize(entity);
        //}

        //public static string Serialize<T>(this T entity, ITextSerializerProvider provider)
        //{
        //    var serializer = provider.GetSerializer();

        //    var data = serializer.Serialize(entity);

        //    return data;
        //}
    }
}