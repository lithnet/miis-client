using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class DsmlAttribute : XmlObjectBase
    {
        private const string DsmlAttributeTypeBinary = "1.3.6.1.4.1.1466.115.121.1.5";
        private const string DsmlAttributeTypeBoolean = "1.3.6.1.4.1.1466.115.121.1.7";
        private const string DsmlAttributeTypeReference = "1.3.6.1.4.1.1466.115.121.1.12";
        private const string DsmlAttributeTypeInteger = "1.3.6.1.4.1.1466.115.121.1.27";
        private const string DsmlAttributeTypeString = "1.3.6.1.4.1.1466.115.121.1.15";

        private AttributeType type = AttributeType.Unknown;

        internal DsmlAttribute(XmlNode node)
            : base(node)
        {
        }

        public string Name => this.GetValue<string>("dsml:name");

        public AttributeType Type
        {
            get
            {
                if (this.type == AttributeType.Unknown)
                {
                    string syntax = this.GetValue<string>("dsml:syntax");

                    switch (syntax)
                    {
                        case DsmlAttribute.DsmlAttributeTypeBinary:
                            this.type = AttributeType.Binary;
                            break;

                        case DsmlAttribute.DsmlAttributeTypeBoolean:
                            this.type = AttributeType.Boolean;
                            break;

                        case DsmlAttribute.DsmlAttributeTypeInteger:
                            this.type = AttributeType.Integer;
                            break;

                        case DsmlAttribute.DsmlAttributeTypeReference:
                            this.type = AttributeType.Reference;
                            break;

                        case DsmlAttribute.DsmlAttributeTypeString:
                            this.type = AttributeType.String;
                            break;

                        default:
                            throw new InvalidOperationException("Unknown dsml attribute type");
                    }
                }

                return this.type;
            }
        }

        public bool Multivalued => !this.GetValue<bool>("@single-value");

        public bool Indexable => this.GetValue<bool>("@ms-dsml:indexable");

        public bool Indexed => this.GetValue<bool>("@ms-dsml:indexed");

        public override string ToString()
        {
            return this.Name;
        }

        internal string TypeName
        {
            get
            {
                switch (this.Type)
                {
                    case AttributeType.Binary:
                        return "binary";
                    case AttributeType.String:
                        return "string";
                    case AttributeType.Integer:
                        return "number";
                    case AttributeType.Boolean:
                        return "boolean";
                    case AttributeType.Reference:
                        return "reference";
                    case AttributeType.Unknown:
                    default:
                        throw new InvalidOperationException("Unknown attibute type");
                }
            }
        }
    }
}

/* 
<dsml:attribute-type id="accountName" single-value="true" ms-dsml:indexable="true" ms-dsml:indexed="true">
  <dsml:name>accountName</dsml:name>
  <dsml:syntax>1.3.6.1.4.1.1466.115.121.1.15</dsml:syntax>
</dsml:attribute-type>
*/
