using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Lithnet.Miiserver.Client
{
    public class CSMVLink : NodeCache
    {
        internal CSMVLink(XmlNode node)
            : base(node)
        {
        }

        public string LineageID
        {
            get
            {
                return this.GetValue<string>("@lineage-id");
            }
        }

        public string LineageType
        {
            get
            {
                return this.GetValue<string>("@lineage-type");
            }
        }

        public DateTime? LineageTime
        {
            get
            {
                return this.GetValue<DateTime?>("@lineage-time");
            }
        }


        public string ConnectorSpaceDN
        {
            get
            {
                return this.GetValue<string>("cs-dn");
            }
        }

        public Guid ConnectorSpaceID
        {
            get
            {
                return this.GetValue<Guid>("cs-guid");

            }
        }

        public string ManagementAgentName
        {
            get
            {
                return this.GetValue<string>("ma-name");
            }
        }

        public Guid ManagementAgentID
        {
            get
            {
                return this.GetValue<Guid>("ma-guid");
            }
        }

        public CSObject GetCSObject()
        {
            return CSObject.GetCSObject(this.ConnectorSpaceID);
        }
    }
}