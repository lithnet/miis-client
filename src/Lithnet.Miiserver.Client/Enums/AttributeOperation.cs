namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    [XmlTypeAttribute(AnonymousType = true)]
    public enum AttributeOperation
    {
        [XmlEnum("none")]
        None = 0,

        [XmlEnum("add")]
        Add,

        [XmlEnum("replace")]
        Replace,

        [XmlEnum("update")]
        Update,

        [XmlEnum("delete")]
        Delete,
    }
}
