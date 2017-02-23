using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents a change to an attribute value
    /// </summary>
    public class AttributeValueChange : AttributeValue
    {
        internal AttributeValueChange(XmlNode node, AttributeType type)
            :base(node, type)
        {
        }

        /// <summary>
        /// Gets the type of change performed on the value
        /// </summary>
        public AttributeValueOperation Operation => this.GetValue<AttributeValueOperation>("@operation");

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns>A string representation of the object</returns>
        public override string ToString()
        {
            if (this.Operation == AttributeValueOperation.None)
            {
                return $"{this.ValueString}";
            }
            else
            {
                return $"{this.Operation}: {this.ValueString}";
            }
        }
    }
}
