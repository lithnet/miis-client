namespace Lithnet.Miiserver.Client
{
    using System.Collections.Generic;
    using System.Xml;

    public class MAConnection : XmlObjectBase
    {
        internal MAConnection(XmlNode node)
            : base(node)
        {
        }

        public string ConnectionResult
        {
            get
            {
                return this.GetValue<string>("connection-result");
            }
        }

        public string Server
        {
            get
            {
                return this.GetValue<string>("server");
            }
        }

        public IReadOnlyList<MAConnectionIncident> ConnectionLog
        {
            get
            {
                return this.GetReadOnlyObjectList<MAConnectionIncident>("connection-log/incident");
            }
        }
    }
}
