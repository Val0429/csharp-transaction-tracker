using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Constant.Utility
{
    public static class XmlUtility
    {
        public static XmlElement ToXmlElement(this XElement el)
        {
            var doc = new XmlDocument();
            doc.Load(el.CreateReader());
            return doc.DocumentElement;
        }

        public static XmlElement GetElement(this XmlElement element, string name)
        {
            return element.GetElementsByTagName(name).OfType<XmlElement>().FirstOrDefault();
        }

        public static string GetElementValue(this XmlNode xmlNode, string name)
        {
            var element = xmlNode[name];

            return element != null ? element.InnerText : null;
        }

        public static void AppendOrReplace(this XmlElement element, XmlElement newElement)
        {
            var oldElement = element.GetElement(newElement.Name);
            if (oldElement != null)
            {
                element.ReplaceChild(newElement, oldElement);
            }
            else
            {
                element.AppendChild(newElement);
            }
        }


        /// <summary>
        /// 取代或加入指定節點做為此 System.Xml.Linq.XContainer 的子系。
        /// </summary>
        /// <param name="element"></param>
        /// <param name="node"></param>
        public static void AppendOrReplace(this XContainer element, XElement node)
        {
            var old = element.Element(node.Name);
            if (old == null)
            {
                element.Add(node);
            }
            else
            {
                old.ReplaceWith(node);
            }
        }

        public static string GetAttributeValue(this XElement element, XName xName)
        {
            var attri = element.Attribute(xName);

            return attri == null ? null : attri.Value;
        }
        /// <summary>
        /// 取得具有指定之 System.Xml.Linq.XName 的第一個 (依據文件順序) 子項目的串連文字內容。
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="xName">要比對的 System.Xml.Linq.XName。</param>
        /// <returns>
        /// 符合指定之 System.Xml.Linq.XName 的項目的所有文字內容。如果有多個文字節點，將會串連它們。若找不到項目回傳 null。
        /// </returns>
        public static string GetElementValue(this XElement xElement, XName xName)
        {
            var targetElement = xElement.Element(xName);

            return targetElement == null ? null : targetElement.Value;
        }
        public static IEnumerable<string> GetElementValues(this XElement xElement, XName xName)
        {
            var targetElement = xElement.Elements(xName);

            return targetElement.Select(x => x.Value);
        }
        /// <summary>
        /// 取得值，指出這個項目是否至少有一個要比對的子項目。
        /// </summary>
        /// <param name="xElement"></param>
        /// <param name="xName">要比對的 System.Xml.Linq.XName。</param>
        /// <returns>如果這個項目至少有一個要比對的子項目則為 true，否則為 false。</returns>
        public static bool HasElement(this XElement xElement, XName xName)
        {
            return xElement.Element(xName) != null;
        }

        public static string GetXPathElementValue(this XElement xElement, string xPath)
        {
            var element = xElement.XPathSelectElement(xPath);

            return element != null ? element.Value : null;
        }

        public static string XPathSelectElementValue(this XElement xElement, string expression)
        {
            var targetElement = xElement.XPathSelectElement(expression);

            return targetElement == null ? null : targetElement.Value;
        }
    }
}
