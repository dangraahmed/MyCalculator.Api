using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SQLDataAccess.Common
{
    public static class ExtensionMethod
    {
        public static string Serialize<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            try
            {

                var xmlserializer = new XmlSerializer(typeof(T));
                var ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                ns.Add("", "");
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true, }))
                {
                    xmlserializer.Serialize(writer, value, ns);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

        public static string Serialize<T>(this IEnumerable<T> value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                string okok = "<rows>";
                foreach (var item in value)
                {
                    okok += item.Serialize<T>();
                }
                return okok + "</rows>";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }
    }
}
