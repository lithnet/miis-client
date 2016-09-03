using System;
using System.Xml.Serialization;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Defines a set of delta operations that can be performed on an object
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public enum DeltaOperationType
    {
        /// <summary>
        /// No changes were made to the object
        /// </summary>
        [XmlEnum("none")]
        None = 0,

        /// <summary>
        /// The object was added
        /// </summary>
        [XmlEnum("add")]
        Add,

        /// <summary>
        /// The object was replaced
        /// </summary>
        [XmlEnum("replace")]
        Replace,

        /// <summary>
        /// The object was updated
        /// </summary>
        [XmlEnum("update")]
        Update,

        /// <summary>
        /// The object was deleted
        /// </summary>
        [XmlEnum("delete")]
        Delete,

        /// <summary>
        /// The object was marked as obsolete
        /// </summary>
        [XmlEnum("obsolete")]
        Obsolete,

        /// <summary>
        /// The object was reprovisioned
        /// </summary>
        [XmlEnum("delete-add")]
        DeleteAdd,
    }
}
