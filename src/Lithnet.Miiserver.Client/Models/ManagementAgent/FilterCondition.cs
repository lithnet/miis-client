namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class FilterCondition : NodeCache
    {
        private object value;

        internal FilterCondition(XmlNode node)
            : base(node)
        {
        }
        
        public string Attribute
        {
            get
            {
                return this.GetValue<string>("@cd-attribute");
            }
        }

        public string Operator
        {
            get
            {
                return this.GetValue<string>("@operator");
            }
        }

        private string Radix
        {
            get
            {
                return this.GetValue<string>("value/@ui-radix");
            }
        }

        private string Encoding
        {
            get
            {
                return this.GetValue<string>("value/@encoding");
            }
        }

        private string RawValue
        {
            get
            {
                return this.GetValue<string>("value");
            }
        }

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

        public string ValueString
        {
            get
            {
                return (string)this.Value;
            }
        }

        public long ValueInteger
        {
            get
            {
                return (long)this.value;
            }
        }

        public bool ValueBoolean
        {
            get
            {
                return (bool)this.value;
            }
        }

        public byte[] ValueBinary
        {
            get
            {
                return (byte[])this.value;
            }
        }

        public override string ToString()
        {
            if (this.Value == null)
            {
                return string.Format("{0} {1}", this.Attribute, this.Operator);
            }
            else
            {
                return string.Format("{0} {1} {2}", this.Attribute, this.Operator, this.RawValue);
            }
        }
    }
}
/*
        <condition cd-attribute="CompanyRuleID" operator="equality">
          <value ui-radix="10">0x1</value>
        </condition>
*/
