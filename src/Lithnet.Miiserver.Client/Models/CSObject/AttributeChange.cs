using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents an attribute change and its value collection
    /// </summary>
    public class AttributeChange : XmlObjectBase
    {
        internal AttributeChange(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the type of operation performed on the object
        /// </summary>
        public AttributeOperation Operation => this.GetValue<AttributeOperation>("@operation");


        public IReadOnlyList<AttributeValueChange> ValueChanges => this.GetReadOnlyObjectList<AttributeValueChange>("value", this.Type);
      
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
            if (this.Operation == AttributeOperation.None)
            {
                return $"{this.Name}:{this.ValueChanges.Select(t => $"{t.Operation}:{t.Value}").ToCommaSeparatedString()}";
            }
            else
            {
                return $"{this.Operation}:{this.Name}:({this.ValueChanges.Select(t => $"{t.Operation}:{t.Value}").ToCommaSeparatedString()})";
            }
        }
    }
}