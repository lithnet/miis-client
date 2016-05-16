namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public enum ExtensionCallSite
    {
        [XmlEnum("none")]
        None = 0,
        
        [XmlEnum("initialize")]
        Initialize,

        [XmlEnum("connector-filter")]
        ConnectorFilter,

        [XmlEnum("join-mapping")]
        JoinMapping,

        [XmlEnum("join-resolution")]
        JoinResolution,

        [XmlEnum("projection")]
        Projection,

        [XmlEnum("import-flow")]
        ImportFlow,

        [XmlEnum("export-flow")]
        ExportFlow,

        [XmlEnum("provisioning")]
        Provisioning,

        [XmlEnum("mv-deletion")]
        MVDeletion,
    }
}
