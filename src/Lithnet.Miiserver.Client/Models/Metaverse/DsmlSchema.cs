using System;
using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class DsmlSchema : XmlObjectBase
    {
        internal DsmlSchema(XmlNode node)
            : base(node)
        {
        }

        public IReadOnlyDictionary<string, DsmlObjectClass> ObjectClasses
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, DsmlObjectClass>("dsml:directory-schema/dsml:class", t => t.Name, StringComparer.OrdinalIgnoreCase, new object[] { this.Attributes });
            }
        }

        public IReadOnlyDictionary<string, DsmlAttribute> Attributes
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, DsmlAttribute>("dsml:directory-schema/dsml:attribute-type", t => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        internal static DsmlSchema GetMVSchema(string xml)
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml(xml);
            return new DsmlSchema(d.SelectSingleNode("mv-data/schema/dsml:dsml", XmlObjectBase.GetNSManager(d.NameTable)));
        }
    }
}
