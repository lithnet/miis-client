using System;
using System.Xml.Serialization;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Defines the data type of an attribute
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public enum AttributeType
    {
        /// <summary>
        /// An unknown data type
        /// </summary>
        [XmlEnum("unknown")]
        Unknown = 0,

        /// <summary>
        /// A binary attribute
        /// </summary>
        [XmlEnum("binary")]
        Binary,

        /// <summary>
        /// A string attribute
        /// </summary>
        [XmlEnum("string")]
        String,

        /// <summary>
        /// An integer attribute
        /// </summary>
        [XmlEnum("integer")]
        Integer,

        /// <summary>
        /// A boolean attribute
        /// </summary>
        [XmlEnum("boolean")]
        Boolean,

        /// <summary>
        /// A reference attribute
        /// </summary>
        [XmlEnum("reference")]
        Reference
    }
}
