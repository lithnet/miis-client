using System;
using System.Xml.Serialization;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Defines the operation that can be performed on an attribute's value
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public enum AttributeValueOperation
    {
        /// <summary>
        /// The attribute value has not changed
        /// </summary>
        [XmlEnum("none")]
        None = 0,

        /// <summary>
        /// The attribute value was added
        /// </summary>
        [XmlEnum("add")]
        Add,
        
        /// <summary>
        /// The attribute value was updated
        /// </summary>
        [XmlEnum("update")]
        Update,
        
        /// <summary>
        /// The attribute value was deleted
        /// </summary>
        [XmlEnum("delete")]
        Delete,
    }
}
