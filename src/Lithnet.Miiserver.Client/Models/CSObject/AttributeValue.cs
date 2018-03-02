using System;
using System.Runtime.Serialization;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents an attribute value
    /// </summary>
    [DataContract]
    public class AttributeValue : XmlObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the AttributeValue class
        /// </summary>
        /// <param name="node">The XML representation of the object</param>
        /// <param name="type">The data type of the attribute</param>
        internal AttributeValue(XmlNode node, AttributeType type)
            : base(node)
        {

            this.Encoding = this.GetValue<string>("@encoding");
            this.Type = this.GetValue<AttributeType>("type");
            this.ValueString = this.GetValue<string>(".", "value");
        }

        //protected AttributeValue(SerializationInfo info, StreamingContext context)
        //    : base(info, context)
        //{
        //}

        /// <summary>
        /// Gets the type of encoding used for the attribute value within the XML structure
        /// </summary>
        [Serialize]
        [DataMember(Name = "encoding")]
        private string Encoding { get; set; }

        [Serialize]
        [DataMember(Name = "type")]
        private AttributeType Type { get; set; }

        /// <summary>
        /// Gets the value as a string
        /// </summary>
        [Serialize]
        [DataMember(Name = "value")]
        public string ValueString { get; private set; }

        /// <summary>
        /// Gets the value in its native data type
        /// </summary>
        public object Value
        {
            get
            {
                if (this.Encoding == "base64")
                {
                    return Convert.FromBase64String(this.ValueString);
                }
                else if (string.IsNullOrWhiteSpace(this.Encoding))
                {
                    if (this.Type == AttributeType.Integer)
                    {
                        return Convert.ToInt64(this.ValueString, 16);
                    }
                    else if (this.Type == AttributeType.Boolean)
                    {
                        return Convert.ToBoolean(this.ValueString);
                    }
                    else
                    {
                        return this.ValueString;
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unknown encoding type: " + this.Encoding);
                }
            }
        }

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
        /// Returns a string representation of the object
        /// </summary>
        /// <returns>A string representation of the object</returns>
        public override string ToString()
        {
            return this.ValueString;
        }
    }
}