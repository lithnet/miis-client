using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class AlgorithmStep : XmlObjectBase
    {
        internal AlgorithmStep(XmlNode node)
            :base(node)
        {
            this.Value = node.InnerText;
        }

        public Guid MAID => this.GetValue<Guid>("@ma-id");

        public string DN => this.GetValue<string>("@dn");

        public string Value { get; private set; }
    }
}
