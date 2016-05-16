namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum AttributeType
    {
        [XmlEnum("unknown")]
        Unknown = 0,

        [XmlEnum("binary")]
        Binary,

        [XmlEnum("string")]
        String,

        [XmlEnum("integer")]
        Integer,

        [XmlEnum("boolean")]
        Boolean,

        [XmlEnum("reference")]
        Reference
    }
}
