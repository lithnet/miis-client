namespace Lithnet.Miiserver.Client
{
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Represents an attribute change and its value collection
    /// </summary>
    public class AttributeChange : Attribute
    {
        internal AttributeChange(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the type of operation performed on the object
        /// </summary>
        public AttributeOperation Operation => this.GetValue<AttributeOperation>("@operation");

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (this.Operation == AttributeOperation.None)
            {
                return $"{this.Name}:{this.Values.Select(t => t.ToString()).ToCommaSeparatedString()}";
            }
            else
            {
                return $"{this.Operation}:{this.Name}:{this.Values.Select(t => t.ToString()).ToCommaSeparatedString()}";
            }
        }
    }
}