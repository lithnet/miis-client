using System;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;

namespace Lithnet.Miiserver.Client
{
    [DataContract]
    //[JsonObject(MemberSerialization.OptIn)]
    public class CSObjectBase : XmlObjectBase
    {
        internal CSObjectBase(XmlNode node)
            : base(node)
        {
        }

        protected CSObjectBase(SerializationInfo info, StreamingContext context)
        : base(info, context)
        {
        }

        /// <summary>
        /// Gets the security account manager (SAM) account name for the user
        /// </summary>
        //[JsonProperty("account-name")]
        [DataMember(Name ="account-name")]
        public string AccountName => this.GetValue<string>("account-name");

        /// <summary>
        /// Gets the distinguished name of the connector space object.
        /// </summary>
        //[JsonProperty("cs-dn")]
        [DataMember]
        public string DN => this.GetValue<string>("@cs-dn", "cs-dn");

        /// <summary>
        /// Gets the domain name of the connector space object.
        /// </summary>
        //[JsonProperty("domain-name")]
        [DataMember]
        public string DomainName => this.GetValue<string>("domain-name");

        /// <summary>
        /// Gets the domain name of the connector space object in domain name system (DNS) format.
        /// </summary>
        //[JsonProperty("fully-qualified-domain-name")]
        [DataMember]
        public string FullyQualifiedDomainName => this.GetValue<string>("fully-qualified-domain-name");

        /// <summary>
        /// Gets the GUID that is used to identify the connector space object in the database.
        /// </summary>
        //[JsonProperty("id")]
        [DataMember]
        public Guid ID => this.GetValue<Guid>("@id", "id");

        /// <summary>
        /// Gets the GUID of the management agent for the connector space object.
        /// </summary>
        //[JsonProperty("ma-id")]
        [DataMember]
        public Guid MAID => this.GetValue<Guid>("ma-id");

        /// <summary>
        /// Gets the name of the management agent for the connector space object.
        /// </summary>
        //[JsonProperty("ma-name")]
        [DataMember]
        public string MaName => this.GetValue<string>("ma-name");

        /// <summary>
        /// Gets the GUID of the metaverse object that is joined to the connector space object.
        /// </summary>
        //[JsonProperty("mv-id")]
        [DataMember]
        public Guid? MvGuid => this.MetaverseLink?.MVObjectID;

        /// <summary>
        /// Gets the name of the primary object type of the connector space object.
        /// </summary>
        //[JsonProperty("object-type")]
        [DataMember]
        public string ObjectType => this.GetValue<string>("@object-type", "object-type");

        /// <summary>
        /// Gets the GUID of the management agent partition with the connector space object.
        /// </summary>
        //[JsonProperty("partition-id")]
        [DataMember]
        public Guid? PartitionGuid => this.GetValue<Guid?>("partition-id");

        /// <summary>
        /// Gets the display name of the management agent partition with the connector space object.
        /// </summary>
        //[JsonProperty("partition-name")]
        [DataMember]
        public string PartitionName => this.GetValue<string>("partition-name");

        //[JsonProperty("pending-ref-delete")]
        [DataMember]
        public bool PendingRefDelete => this.GetValue<bool>("pending-ref-delete");

        //[JsonProperty("seen-by-import")]
        [DataMember]
        public bool SeenByImport => this.GetValue<bool>("seen-by-import");

        //[JsonProperty("connector")]
        [DataMember]
        public bool IsConnector => this.GetValue<bool>("connector");

        //[JsonProperty("connector-state")]
        [DataMember]
        public ConnectorState ConnectorState => this.GetValue<ConnectorState>("connector-state");

        //[JsonProperty("rebuild-in-progress")]
        [DataMember]
        public bool RebuildInProgress => this.GetValue<bool>("rebuild-in-progress");

        //[JsonProperty("obsoletion")]
        [DataMember]
        public bool Obsoletion => this.GetValue<bool>("obsoletion");

        //[JsonProperty("need-full-sync")]
        [DataMember]
        public bool NeedFullSync => this.GetValue<bool>("need-full-sync");

        //[JsonProperty("placeholder-parent")]
        [DataMember]
        public bool IsPlaceholderParent => this.GetValue<bool>("placeholder-parent");

        //[JsonProperty("placeholder-link")]
        [DataMember]
        public bool IsPlaceholderLink => this.GetValue<bool>("placeholder-link");

        //[JsonProperty("placeholder-delete")]
        [DataMember]
        public bool IsPlaceholderDelete => this.GetValue<bool>("placeholder-delete");

        //[JsonProperty("pending")]
        [DataMember]
        public bool IsPending => this.GetValue<bool>("pending");

        //[JsonProperty("ref-retry")]
        [DataMember]
        public bool PendingReferenceRetry => this.GetValue<bool>("ref-retry");

        //[JsonProperty("rename-retry")]
        [DataMember]
        public bool PendingRenameRetry => this.GetValue<bool>("rename-retry");

        //[JsonProperty("import-delta-operation")]
        [DataMember]
        public string ImportDeltaOperation => this.GetValue<string>("import-delta-operation");

        //[JsonProperty("export-delete-operation")]
        [DataMember]
        public string ExportDeltaOperation => this.GetValue<string>("export-delta-operation");

        //[JsonProperty("last-import-delta-time")]
        [DataMember]
        public DateTime? LastImportDeltaTime => this.GetValue<DateTime?>("last-import-delta-time");

        //[JsonProperty("last-export-delta-time")]
        [DataMember]
        public DateTime? LastExportDeltaTime => this.GetValue<DateTime?>("last-export-delta-time");

        //[JsonProperty("disconnection-type")]
        [DataMember]
        public string DisconnectionType => this.GetValue<string>("disconnection-type");

        //[JsonProperty("disconnection-id")]
        [DataMember]
        public Guid? DisconnectionID => this.GetValue<Guid?>("disconnection-id");

        //[JsonProperty("disconnection-time")]
        [DataMember]
        public DateTime? DisconnectionTime => this.GetValue<DateTime?>("disconnection-time");

        //[JsonProperty("mv-link")]
        [DataMember]
        public MVLink MetaverseLink => this.GetObject<MVLink>("mv-link");

        //[JsonProperty("unapplied-export-delta")]
        [DataMember]
        public Delta UnappliedExportDelta => this.GetObject<Delta>("unapplied-export/delta", "unapplied-export-delta");

        //[JsonProperty("escrowed-export-delta")]
        [DataMember]
        public Delta EscrowedExportDelta => this.GetObject<Delta>("escrowed-export/delta", "escrowed-export-delta");

        //[JsonProperty("unconfirmed-export-delta")]
        [DataMember]
        public Delta UnconfirmedExportDelta => this.GetObject<Delta>("unconfirmed-export/delta", "unconfirmed-export-delta");

        //[JsonProperty("pending-import-delta")]
        [DataMember]
        public Delta PendingImportDelta => this.GetObject<Delta>("pending-import/delta", "pending-import-delta");

        //[JsonProperty("synchronized-hologram")]
        [DataMember]
        public Hologram SynchronizedHologram => this.GetObject<Hologram>("synchronized-hologram/entry", "synchronized-hologram");

        //[JsonProperty("unapplied-export-hologram")]
        [DataMember]
        public Hologram UnappliedExportHologram => this.GetObject<Hologram>("unapplied-export-hologram/entry", "unapplied-export-hologram");

        //[JsonProperty("escrowed-export-hologram")]
        [DataMember]
        public Hologram EscrowedExportHologram => this.GetObject<Hologram>("escrowed-export-hologram/entry", "escrowed-export-hologram");

        //[JsonProperty("unconfirmed-export-hologram")]
        [DataMember]
        public Hologram UnconfirmedExportHologram => this.GetObject<Hologram>("unconfirmed-export-hologram/entry", "unconfirmed-export-hologram");

        //[JsonProperty("pending-import-hologram")]
        [DataMember]
        public Hologram PendingImportHologram => this.GetObject<Hologram>("pending-import-hologram/entry", "pending-import-hologram");

        public override string ToString()
        {
            return this.DN;
        }
    }
}