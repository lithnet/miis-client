using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class FilterCondition : XmlObjectBase
    {
        private object value;

        internal FilterCondition(XmlNode node)
            : base(node)
        {
        }
        
        public string Attribute => this.GetValue<string>("@cd-attribute");

        public string Operator => this.GetValue<string>("@operator");

        private string Radix => this.GetValue<string>("value/@ui-radix");

        private string Encoding => this.GetValue<string>("value/@encoding");

        private string RawValue => this.GetValue<string>("value");

        public object Value
        {
            get
            {
                if (this.value == null)
                {
                    if (!string.IsNullOrEmpty(this.RawValue))
                    {
                        if (this.Encoding == "base64")
                        {
                            this.value = Convert.FromBase64String(this.RawValue);
                        }
                        else if (this.Radix == "10")
                        {
                            this.value = Convert.ToInt64(this.RawValue, 16);
                        }
                        else
                        {
                            if (this.RawValue == "true")
                            {
                                this.value = true;
                            }
                            else if (this.RawValue == "false")
                            {
                                this.value = false;
                            }
                            else
                            {
                                this.value = this.RawValue;
                            }
                        }
                    }
                }

                return this.value;
            }
        }

        public string ValueString => (string)this.Value;

        public long ValueInteger => (long)this.value;

        public bool ValueBoolean => (bool)this.value;

        public byte[] ValueBinary => (byte[])this.value;

        public override string ToString()
        {
            if (this.Value == null)
            {
                return $"{this.Attribute} {this.Operator}";
            }
            else
            {
                return $"{this.Attribute} {this.Operator} {this.RawValue}";
            }
        }
    }
}
/*
        <condition cd-attribute="CompanyRuleID" operator="equality">
          <value ui-radix="10">0x1</value>
        </condition>
*/
