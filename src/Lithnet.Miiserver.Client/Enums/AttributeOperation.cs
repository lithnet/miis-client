using System;
using System.Xml.Serialization;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Defines an operation on an attribute
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public enum AttributeOperation
    {
        /// <summary>
        /// No operation was performed
        /// </summary>
        [XmlEnum("none")]
        None = 0,

        /// <summary>
        /// The attribute was added
        /// </summary>
        [XmlEnum("add")]
        Add,

        /// <summary>
        /// The attribute was replaced
        /// </summary>
        [XmlEnum("replace")]
        Replace,

        /// <summary>
        /// The attribute was updated
        /// </summary>
        [XmlEnum("update")]
        Update,

        /// <summary>
        /// The attribute was deleted
        /// </summary>
        [XmlEnum("delete")]
        Delete,
    }
}
