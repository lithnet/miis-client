using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    /// <summary>
    /// Represents a link between a connector space object and a metaverse object
    /// </summary>
    public class CSMVLink : XmlObjectBase
    {
        internal CSMVLink(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the lineage ID for this link
        /// </summary>
        public string LineageID => this.GetValue<string>("@lineage-id");

        /// <summary>
        /// Gets the type of the link
        /// </summary>
        public string LineageType => this.GetValue<string>("@lineage-type");

        /// <summary>
        /// Gets the date and time the link was created
        /// </summary>
        public DateTime? LineageTime => this.GetValue<DateTime?>("@lineage-time");
        
        /// <summary>
        /// Gets the DN of the connector space object
        /// </summary>
        public string ConnectorSpaceDN => this.GetValue<string>("cs-dn");

        /// <summary>
        /// Gets the GUID of the connector space object
        /// </summary>
        public Guid ConnectorSpaceID => this.GetValue<Guid>("cs-guid");

        /// <summary>
        /// Gets the connected management agent name
        /// </summary>
        public string ManagementAgentName => this.GetValue<string>("ma-name");

        /// <summary>
        /// Gets the GUID of the connected management agent
        /// </summary>
        public Guid ManagementAgentID => this.GetValue<Guid>("ma-guid");

        /// <summary>
        /// Gets the connector space object referenced by this link
        /// </summary>
        /// <returns></returns>
        public CSObject GetCSObject()
        {
            return CSObject.GetCSObject(this.ConnectorSpaceID);
        }
    }
}