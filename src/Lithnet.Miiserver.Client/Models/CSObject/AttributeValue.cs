using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents an attribute value
    /// </summary>
    public class AttributeValue : XmlObjectBase
    {
        /// <summary>
        /// The data type of the attribute
        /// </summary>
        private AttributeType type;

        /// <summary>
        /// Initializes a new instance of the AttributeValue class
        /// </summary>
        /// <param name="node">The XML representation of the object</param>
        /// <param name="type">The data type of the attribute</param>
        internal AttributeValue(XmlNode node, AttributeType type)
            :base (node)
        {
            this.type = type;

            this.ValueString = node.InnerText;
            this.DecodeValue();
        }

        /// <summary>
        /// Gets the type of encoding used for the attribute value within the XML structure
        /// </summary>
        private string Encoding => this.GetValue<string>("@encoding");

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        public string ValueString { get; private set; }

        /// <summary>
        /// Gets the value in its native data type
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Gets the value as an integer
        /// </summary>
        public long ValueInteger => (long)this.Value;

        /// <summary>
        /// Gets the value as an boolean value
        /// </summary>
        public bool ValueBoolean => (bool)this.Value;

        /// <summary>
        /// Gets the value as a byte array
        /// </summary>
        public byte[] ValueBinary => (byte[])this.Value;

        /// <summary>
        /// Decodes the value into its native data type
        /// </summary>
        private void DecodeValue()
        {
            if (this.Encoding == "base64")
            {
                this.Value = Convert.FromBase64String(this.ValueString);
            }
            else if (string.IsNullOrWhiteSpace(this.Encoding))
            {
                if (this.type == AttributeType.Integer)
                {
                    this.Value = Convert.ToInt64(this.ValueString, 16);
                    this.ValueString = this.Value.ToString();
                }
                else if (this.type == AttributeType.Boolean)
                {
                    this.Value = Convert.ToBoolean(this.ValueString);
                }
                else
                {
                    this.Value = this.ValueString;
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown encoding type: " + this.Encoding);
            }
        }

        /// <summary>
        /// Returns a string representation of the object
        /// </summary>
        /// <returns>A string representation of the object</returns>
        public override string ToString()
        {
            return this.ValueString;
        }
    }
}