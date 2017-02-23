using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public class MAConnectionIncident : XmlObjectBase
    {
        internal MAConnectionIncident(XmlNode node)
            :base(node)
        {
        }

        public string ConnectionResult => this.GetValue<string>("connection-result");

        public DateTime? Date => this.GetValue<DateTime?>("date");

        public string Server => this.GetValue<string>("server");

        public CDError CDError => this.GetObject<CDError>("cd-error");
    }
}
