using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ExportChange : XmlObjectBase
    {
        internal ExportChange(XmlNode node)
            : base(node)
        {
        }

        public Guid ID => this.GetValue<Guid>("@id");

        public Guid MAID => this.GetValue<Guid>("@ma-id");

        public string MAName => this.GetValue<string>("@ma-name");

        public string CSOperation => this.GetValue<string>("cs-operation");

        public Hologram Hologram => this.GetObject<Hologram>("entry");

        public override string ToString()
        {
            return this.CSOperation;
        }
    }
}
