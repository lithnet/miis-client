using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class ConnectorRename: XmlObjectBase
    {
        internal ConnectorRename(XmlNode node)
            : base(node)
        {
        }

        public string Status => this.GetValue<string>("@status");

        public Guid MAID => this.GetValue<Guid>("ma-guid");

        public string MAName => this.GetValue<string>("ma-name");

        public string OldDN => this.GetValue<string>("old-dn");

        public string NewDN => this.GetValue<string>("new-dn");


        public Guid ID => this.GetValue<Guid>("object-id");

        public string ObjectClass => this.GetValue<string>("primary-objectclass");

        public override string ToString()
        {
            return this.NewDN;
        }
    }
}

/*
<rename-connector status="success">
    <old-dn>cn=Ryan Newington,ou=Office of the CIO,ou=Staff,c=au</old-dn>
    <new-dn>cn=Ryan L Newington,ou=Office of the CIO,ou=Staff,c=AU</new-dn>
    <ma-guid>{0C20E768-3233-4FA0-A821-EA8EB2D34472}</ma-guid>
    <ma-name>MDS</ma-name>
    <object-id>{F16733B3-3434-E511-B8B9-005056BA23AA}</object-id>
    <primary-objectclass>fimAccount</primary-objectclass>
</rename-connector>
*/
