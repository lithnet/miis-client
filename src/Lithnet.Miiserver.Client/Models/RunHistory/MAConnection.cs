using System.Collections.Generic;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MAConnection : XmlObjectBase
    {
        internal MAConnection(XmlNode node)
            : base(node)
        {
        }

        public string ConnectionResult => this.GetValue<string>("connection-result");

        public string Server => this.GetValue<string>("server");

        public IReadOnlyList<MAConnectionIncident> ConnectionLog => this.GetReadOnlyObjectList<MAConnectionIncident>("connection-log/incident");
    }
}
