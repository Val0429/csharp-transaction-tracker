using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Constant
{
    [XmlType(TypeName = "Adaptor")]
    [XmlInclude(typeof(LicenseInfo))]
    public class Adaptor
    {
        private readonly List<LicenseInfo> _licenseInfo = new List<LicenseInfo>();

        public Adaptor()
        {
        }


        [XmlElement(ElementName = "MAC")]
        public String Mac { get; set; }
        [XmlElement(ElementName = "Description")]
        public String Description { get; set; }

        [XmlElement(ElementName = "IP")]
        public string IP { get; set; }

        [XmlElement(ElementName = "Key")]
        public List<LicenseInfo> LicenseInfo { get { return _licenseInfo; } }
    }

    [XmlType(TypeName = "Key")]
    public class LicenseInfo
    {
        [XmlAttribute(AttributeName = "val")]
        public String Serial { get; set; }

        [XmlAttribute(AttributeName = "Count")]
        public UInt16 Quantity { get; set; }

        [XmlIgnore]
        public Boolean Expired { get; set; }
        [XmlIgnore]
        public Boolean Trial { get; set; }
        [XmlIgnore]
        public String ExpiresDate { get; set; }

        [XmlAttribute(AttributeName = "ProductNO")]
        public string ProductNo { get; set; }

        [XmlAttribute(AttributeName = "Expired")]
        public int ExpiredXmlSurrogate
        {
            get { return Expired ? 1 : 0; }
            set { Expired = value == 1; }
        }

        [XmlAttribute(AttributeName = "ExpireDate")]
        public string ExpireDate
        {
            get { return string.IsNullOrEmpty(ExpiresDate) ? null : ExpiresDate.Replace("-", "/"); }
            set { ExpiresDate = string.IsNullOrEmpty(value) ? null : value.Replace("/", "-"); }
        }
    }

    public class License
    {
        [XmlElement(ElementName = "Adaptor")]
        public Adaptor[] Adaptor { get; set; }

        public ushort Maximum { get; set; }
    }
}