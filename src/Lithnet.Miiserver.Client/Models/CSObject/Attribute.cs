using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents an attribute and its value collection
    /// </summary>
    [DataContract]
    public class Attribute : XmlObjectBase
    {
        private IReadOnlyList<object> values;

        internal Attribute(XmlNode node)
            : base(node)
        {
            this.Name = this.GetValue<string>("@name", "name");
            this.Type = this.GetValue<AttributeType>("@type", "type");
            this.Multivalued = this.GetValue<bool>("@multivalued", "multivalued");
            this.ValuesInternal = this.GetReadOnlyObjectList<AttributeValue>("value", new object[] { this.Type });
        }

        [Serialize]
        [DataMember(Name = "values1")]
        private IReadOnlyList<AttributeValue> ValuesInternal { get; set; }

        /// <summary>
        /// Gets the list of values for the attribute
        /// </summary>
        public IReadOnlyList<object> Values
        {
            get
            {
                if (this.values == null)
                {
                    this.values = this.ValuesInternal.Select(t => t.Value).ToList().AsReadOnly();
                }

                return this.values;
            }
        }

        /// <summary>
        /// Gets the name of the attribute
        /// </summary>
        [Serialize]
        [DataMember(Name = "name1")]
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of the attribute
        /// </summary>
        [Serialize]
        [DataMember(Name = "type1")]

        public AttributeType Type { get; private set; }

        /// <summary>
        /// Gets a value that indicates if the attribute is multivalued or single-valued
        /// </summary>
        [Serialize]
        [DataMember(Name = "multivalued1")]

        public bool Multivalued { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{this.Name}:{this.Values.Select(t => t.ToString()).ToCommaSeparatedString()}";
        }
    }
}