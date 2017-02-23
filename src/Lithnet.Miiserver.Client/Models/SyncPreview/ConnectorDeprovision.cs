using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ConnectorDeprovision : XmlObjectBase
    {
        internal ConnectorDeprovision(XmlNode node)
            : base(node)
        {
        }

        public bool HasError => this.GetValue<bool>("@has-error");

        public Error Error => this.GetObject<Error>("error");

        public string  Type => this.GetValue<string>("cs-deprovisioning-action/@type");

        public string Status => this.GetValue<string>("cs-deprovisioning-action/@status");

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
