using System;
using System.Xml.Serialization;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Defines the connected state of a connector object
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public enum ConnectorState
    {
        /// <summary>
        /// A normal connector or disconnector
        /// </summary>
        [XmlEnum("normal")]
        Normal = 0,

        /// <summary>
        /// An explicit connector or disconnector
        /// </summary>
        [XmlEnum("explicit")]
        Explicit = 1,

        /// <summary>
        /// A filtered disconnector
        /// </summary>
        [XmlEnum("stay")]
        Filtered = 2
    }
}
