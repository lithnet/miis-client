namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum ConnectorState
    {
        [XmlEnum("normal")]
        Normal = 0,

        [XmlEnum("explicit")]
        Explicit = 1,

        [XmlEnum("stay")]
        Filtered = 2
    }
}
