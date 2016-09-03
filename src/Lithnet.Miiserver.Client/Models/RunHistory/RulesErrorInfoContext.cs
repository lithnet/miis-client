namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class RulesErrorInfoContext : XmlObjectBase
    {
        internal RulesErrorInfoContext(XmlNode node)
            : base(node)
        {
        }

        public AttributeFlow AttributeFlow
        {
            get
            {
                return this.GetObject<AttributeFlow>("attribute-mapping");
            }
        }

        public Guid MAID
        {
            get
            {
                return this.GetValue<Guid>("@ma-id");
            }
        }

        public string MAName
        {
            get
            {
                return this.GetValue<string>("@ma-name");
            }
        }

        public string CSObjectID
        {
            get
            {
                return this.GetValue<string>("@cs-object-id");
            }
        }

        public string DN
        {
            get
            {
                return this.GetValue<string>("@dn");
            }
        }
    }
}
