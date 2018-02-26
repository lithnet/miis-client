using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVAttributeQuery
    {
        public DsmlAttribute Attribute { get; set; }

        public object Value { get; set; }

        public MVSearchFilterOperator Operator { get; set; }

        internal XmlElement GetElement(XmlDocument d)
        {
            XmlElement element = d.CreateElement("mv-attr");
            element.SetAttribute("name", this.Attribute.Name);
            element.SetAttribute("type", this.Attribute.TypeName);
            element.SetAttribute("search-type", this.OperatorToSearchType());

            if (this.Operator != MVSearchFilterOperator.IsPresent && this.Operator != MVSearchFilterOperator.IsNotPresent)
            {
                XmlElement valueElement = d.CreateElement("value");
                valueElement.InnerText = this.GetValueString();
                element.AppendChild(valueElement);
            }

            return element;
        }

        private string GetValueString()
        {
            switch (this.Attribute.Type)
            {
                case AttributeType.Binary:

                    if (this.Value is byte[] bytes)
                    {
                        return Convert.ToBase64String(bytes);
                    }

                    if (this.Value is string stringvalue)
                    {
                        return stringvalue;
                    }

                    throw new InvalidCastException("The specified value could not be converted to a binary type");

                case AttributeType.String:
                    return this.Value.ToString();

                case AttributeType.Integer:
                    return "0x" + Convert.ToInt64(this.Value.ToString()).ToString("X");

                case AttributeType.Boolean:
                    return Convert.ToBoolean(this.Value.ToString()).ToString().ToLowerInvariant();

                case AttributeType.Reference:
                    return ((Guid)this.Value).ToMmsGuid();

                case AttributeType.Unknown:
                default:
                    throw new InvalidOperationException("Unsupported attribute type");
            }
        }

        private string OperatorToSearchType()
        {
            switch (this.Operator)
            {
                case MVSearchFilterOperator.Equals:
                    return "exact";

                case MVSearchFilterOperator.Contains:
                    return "contains";

                case MVSearchFilterOperator.NotContains:
                    return "not-contains";

                case MVSearchFilterOperator.IsPresent:
                    return "value-exists";

                case MVSearchFilterOperator.IsNotPresent:
                    return "no-value";

                case MVSearchFilterOperator.StartsWith:
                    return "starts";

                case MVSearchFilterOperator.EndsWith:
                    return "ends";

                default:
                    throw new InvalidOperationException("Unknown search type");
            }
        }
    }
}
/*
<mv-attr name="requestedAccountName" type="string" search-type="starts-with">
    <value>ims</value>
</mv-attr>
*/
