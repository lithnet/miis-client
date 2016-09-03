namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class AlgorithmStep : XmlObjectBase
    {
        internal AlgorithmStep(XmlNode node)
            :base(node)
        {
            this.Value = node.InnerText;
        }

        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("@ma-id");
            }
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("@dn");
            }
        }

        public string Value { get; private set; }
    }
}
