using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MVDeletion : XmlObjectBase
    {
        internal MVDeletion(XmlNode node)
            : base(node)
        {
        }

        public Error Error => this.GetObject<Error>("error");

        public bool HasError => this.GetValue<bool>("@has-error");

        public string  Type => this.GetValue<string>("mv-deletion-rule/@type");

        public string Status => this.GetValue<string>("mv-deletion-rule/@status");

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
