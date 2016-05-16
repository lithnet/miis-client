namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class MVDeletion : NodeCache
    {
        internal MVDeletion(XmlNode node)
            : base(node)
        {
        }

        public Error Error
        {
            get
            {
                return this.GetObject<Error>("error");
            }
        }

        public bool HasError
        {
            get
            {
                return this.GetValue<bool>("@has-error");
            }
        }

        public string  Type
        {
            get
            {
                return this.GetValue<string>("mv-deletion-rule/@type");
            }
        }

        public string Status
        {
            get
            {
                return this.GetValue<string>("mv-deletion-rule/@status");
            }
        }

        public override string ToString()
        {
            return this.Status;
        }
    }
}

/*
<mv-deletion-rules>
    <mv-deletion has-error="false">
      <mv-deletion-rule type="declared-any" status="delete"></mv-deletion-rule>
    </mv-deletion>
</mv-deletion-rules>

<mv-deletion-rules>
    <mv-deletion has-error="false">
      <mv-deletion-rule type="declared-any" status="dont-delete-wrong-ma"></mv-deletion-rule>
    </mv-deletion>
</mv-deletion-rules>
*/
