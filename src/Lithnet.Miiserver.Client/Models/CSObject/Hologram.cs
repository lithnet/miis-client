using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace Lithnet.Miiserver.Client
{
    [Serializable]
    public class Hologram : CSEntryBase
    {
        internal Hologram(XmlNode node)
            : base(node)
        {
        }

        protected Hologram(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IReadOnlyDictionary<string, Attribute> Attributes
        {
            get
            {
                return this.GetReadOnlyObjectDictionary<string, Attribute>("attr", (t) => t.Name, StringComparer.OrdinalIgnoreCase);
            }
        }

        [DataMember(Name = "attributes")]
        private IEnumerable<Attribute> AttributesInternal => this.Attributes.Values;

        [Serialize]
        public string PrimaryObjectClass => this.GetValue<string>("primary-objectclass");

        [Serialize]
        public IReadOnlyList<string> ObjectClasses => this.GetReadOnlyValueList<string>("objectclass/oc-value", "objectclasses");
    }
}