namespace Lithnet.Miiserver.Client
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Represents an attribute and its value collection
    /// </summary>
    public class Attribute : XmlObjectBase
    {
        private IReadOnlyList<object> values;
       
        internal Attribute(XmlNode node)
            : base(node)
        {
        }

        private IReadOnlyList<AttributeValue> ValuesInternal => this.GetReadOnlyObjectList<AttributeValue>("value", this.Type);

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
        public string Name => this.GetValue<string>("@name");

        /// <summary>
        /// Gets the type of the attribute
        /// </summary>
        public AttributeType Type => this.GetValue<AttributeType>("@type");

        /// <summary>
        /// Gets a value that indicates if the attribute is multivalued or single-valued
        /// </summary>
        public bool Multivalued => this.GetValue<bool>("@multivalued");

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{this.Name}:{this.Values.Select(t => t.ToString()).ToCommaSeparatedString()}";
        }
    }
}