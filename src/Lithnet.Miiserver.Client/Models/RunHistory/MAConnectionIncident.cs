namespace Lithnet.Miiserver.Client
{
    using System;
    using System.Xml;

    public class MAConnectionIncident : XmlObjectBase
    {
        internal MAConnectionIncident(XmlNode node)
            :base(node)
        {
        }

        public string ConnectionResult
        {
            get
            {
                return this.GetValue<string>("connection-result");
            }
        }

        public DateTime? Date
        {
            get
            {
                return this.GetValue<DateTime?>("date");
            }
        }

        public string Server
        {
            get
            {
                return this.GetValue<string>("server");
            }
        }

        public CDError CDError
        {
            get
            {
                return this.GetObject<CDError>("cd-error");
            }
        }
    }
}
