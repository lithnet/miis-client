using System;
using System.Xml;

namespace Lithnet.Miiserver.Client
{
    public abstract class CSObjectBase : XmlObjectBase
    {
        protected CSObjectBase(XmlNode node)
            : base(node)
        {
        }

        /// <summary>
        /// Gets the security account manager (SAM) account name for the user
        /// </summary>
        public string AccountName => this.GetValue<string>("account-name");

        /// <summary>
        /// Gets the distinguished name of the connector space object.
        /// </summary>
        public string DN => this.GetValue<string>("@cs-dn");

        /// <summary>
        /// Gets the domain name of the connector space object.
        /// </summary>
        public string DomainName => this.GetValue<string>("domain-name");

        /// <summary>
        /// Gets the domain name of the connector space object in domain name system (DNS) format.
        /// </summary>
        public string FullyQualifiedDomainName => this.GetValue<string>("fully-qualified-domain-name");

        /// <summary>
        /// Gets the GUID that is used to identify the connector space object in the database.
        /// </summary>
        public Guid ID => this.GetValue<Guid>("@id");

        /// <summary>
        /// Gets the GUID of the management agent for the connector space object.
        /// </summary>
        public Guid MAID => this.GetValue<Guid>("ma-id");

        /// <summary>
        /// Gets the name of the management agent for the connector space object.
        /// </summary>
        public string MaName => this.GetValue<string>("ma-name");

        /// <summary>
        /// Gets the GUID of the metaverse object that is joined to the connector space object.
        /// </summary>
        public Guid? MvGuid => this.MetaverseLink?.MVObjectID;

        /// <summary>
        /// Gets the name of the primary object type of the connector space object.
        /// </summary>
        public string ObjectType => this.GetValue<string>("@object-type");

        /// <summary>
        /// Gets the GUID of the management agent partition with the connector space object.
        /// </summary>
        public Guid? PartitionGuid => this.GetValue<Guid?>("partition-id");

        /// <summary>
        /// Gets the display name of the management agent partition with the connector space object.
        /// </summary>
        public string PartitionName => this.GetValue<string>("partition-name");

        public bool PendingRefDelete => this.GetValue<bool>("pending-ref-delete");

        public bool SeenByImport => this.GetValue<bool>("seen-by-import");

        public bool IsConnector => this.GetValue<bool>("connector");

        public ConnectorState ConnectorState => this.GetValue<ConnectorState>("connector-state");

        public bool RebuildInProgress => this.GetValue<bool>("rebuild-in-progress");

        public bool Obsoletion => this.GetValue<bool>("obsoletion");

        public bool NeedFullSync => this.GetValue<bool>("need-full-sync");

        public bool IsPlaceholderParent => this.GetValue<bool>("placeholder-parent");

        public bool IsPlaceholderLink => this.GetValue<bool>("placeholder-link");

        public bool IsPlaceholderDelete => this.GetValue<bool>("placeholder-delete");

        public bool IsPending => this.GetValue<bool>("pending");

        public bool PendingReferenceRetry => this.GetValue<bool>("ref-retry");

        public bool PendingRenameRetry => this.GetValue<bool>("rename-retry");

        public string ImportDeltaOperation => this.GetValue<string>("import-delta-operation");

        public string ExportDeltaOperation => this.GetValue<string>("export-delta-operation");

        public DateTime? LastImportDeltaTime => this.GetValue<DateTime?>("last-import-delta-time");

        public DateTime? LastExportDeltaTime => this.GetValue<DateTime?>("last-export-delta-time");

        public string DisconnectionType => this.GetValue<string>("disconnection-type");

        public Guid? DisconnectionID => this.GetValue<Guid?>("disconnection-id");

        public DateTime? DisconnectionTime => this.GetValue<DateTime?>("disconnection-time");

        public MVLink MetaverseLink => this.GetObject<MVLink>("mv-link");

        public Delta UnappliedExportDelta => this.GetObject<Delta>("unapplied-export/delta");

        public Delta EscrowedExportDelta => this.GetObject<Delta>("escrowed-export/delta");

        public Delta UnconfirmedExportDelta => this.GetObject<Delta>("unconfirmed-export/delta");

        public Delta PendingImportDelta => this.GetObject<Delta>("pending-import/delta");

        public Hologram SynchronizedHologram => this.GetObject<Hologram>("synchronized-hologram/entry");

        public Hologram UnappliedExportHologram => this.GetObject<Hologram>("unapplied-export-hologram/entry");

        public Hologram EscrowedExportHologram => this.GetObject<Hologram>("escrowed-export-hologram/entry");

        public Hologram UnconfirmedExportHologram => this.GetObject<Hologram>("unconfirmed-export-hologram/entry");

        public Hologram PendingImportHologram => this.GetObject<Hologram>("pending-import-hologram/entry");

        public override string ToString()
        {
            return this.DN;
        }
    }
}