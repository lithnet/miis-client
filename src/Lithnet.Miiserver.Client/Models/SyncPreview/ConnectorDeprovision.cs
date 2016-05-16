namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;
    using System.Collections.Generic;

    public class ConnectorDeprovision : NodeCache
    {
        internal ConnectorDeprovision(XmlNode node)
            : base(node)
        {
        }

        public bool HasError
        {
            get
            {
                return this.GetValue<bool>("@has-error");
            }
        }

        public Error Error
        {
            get
            {
                return this.GetObject<Error>("error");
            }
        }

        public string  Type
        {
            get
            {
                return this.GetValue<string>("cs-deprovisioning-action/@type");
            }
        }

        public string Status
        {
            get
            {
                return this.GetValue<string>("cs-deprovisioning-action/@status");
            }
        }

        public override string ToString()
        {
            return this.Status;
        }
    }
}

/*
<de-provisioning-rules>
    <cs-deprovisioning has-error="false">
        <cs-deprovisioning-action type="declared" status="delete-object" />
    </cs-deprovisioning>
</de-provisioning-rules>
*/
